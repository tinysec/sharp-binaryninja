using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a registered type parser that can parse C/C++ type declarations into
    /// Binary Ninja type objects. TypeParser handles are always borrowed — the parser
    /// lifetime is managed by the native engine's global registry.
    /// </summary>
    public sealed class TypeParser : AbstractSafeHandle<TypeParser>
    {
        /// <summary>
        /// Initializes a new TypeParser wrapper around an existing borrowed handle.
        /// The handle is never owned — the parser lifetime is managed by the native engine.
        /// </summary>
        /// <param name="handle">The native pointer to the BNTypeParser object.</param>
        internal TypeParser(IntPtr handle)
            : base(handle, false)
        {
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNTypeParser pointer.</param>
        /// <returns>A new TypeParser instance that will not free the handle on dispose.</returns>
        internal static TypeParser? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new TypeParser(handle);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNTypeParser pointer.</param>
        /// <returns>A new TypeParser instance that will not free the handle on dispose.</returns>
        internal static TypeParser MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new TypeParser(handle);
        }

        /// <summary>
        /// No-op release: type parser handles are always borrowed from the global registry
        /// and must not be freed by this wrapper.
        /// </summary>
        /// <returns>True (always, since no deallocation is performed).</returns>
        protected override bool ReleaseHandle()
        {
            // Parser objects are borrowed from the global registry; the native engine owns their lifetime.
            return true;
        }

        /// <summary>
        /// Gets the registered name that uniquely identifies this type parser.
        /// </summary>
        public string Name
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the parser name.
                IntPtr raw = NativeMethods.BNGetTypeParserName(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the default type parser registered in the engine.
        /// Returns null if no default parser is configured.
        /// </summary>
        /// <returns>The default TypeParser instance, or null.</returns>
        public static TypeParser? GetDefault()
        {
            // Query the global registry for the default parser; borrowed handle.
            return TypeParser.BorrowHandle(
                NativeMethods.BNGetDefaultTypeParser()
            );
        }

		/// <summary>
		/// Formats parser diagnostics using the core's canonical diagnostic representation.
		/// </summary>
		/// <param name="errors">The parser diagnostics to format.</param>
		/// <returns>The formatted diagnostics.</returns>
		public static string FormatParseErrors(IReadOnlyList<TypeParserError> errors)
		{
			if (null == errors)
			{
				throw new ArgumentNullException(nameof(errors));
			}

			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				BNTypeParserError[] nativeErrors = new BNTypeParserError[errors.Count];
				for (int i = 0; i < errors.Count; i++)
				{
					TypeParserError? error = errors[i];
					if (null == error)
					{
						throw new ArgumentException("Parser error entries cannot be null.", nameof(errors));
					}

					nativeErrors[i] = error.ToNativeEx(allocator);
				}

				IntPtr nativeArray = allocator.AllocStructArray(nativeErrors);
				return UnsafeUtils.TakeUtf8String(
					NativeMethods.BNFormatTypeParserParseErrors(nativeArray, (ulong)nativeErrors.Length));
			}
		}

        /// <summary>
        /// Looks up a registered type parser by its name.
        /// Returns null if no parser is registered with the given name.
        /// </summary>
        /// <param name="name">The registered name of the type parser.</param>
        /// <returns>The matching TypeParser instance, or null if not found.</returns>
        public static TypeParser? GetByName(string name)
        {
            // Query the global registry for a parser with the given name; borrowed handle.
            return TypeParser.BorrowHandle(
                NativeMethods.BNGetTypeParserByName(name ?? string.Empty)
            );
        }

        /// <summary>
        /// Gets the display text for a type parser option with the given value.
        /// </summary>
        /// <param name="option">The parser option to query.</param>
        /// <param name="value">The value of the option.</param>
        /// <returns>The display text for the option, or null on failure.</returns>
        public string? GetOptionText(TypeParserOption option , string value)
        {
            // 1. Call the native function; result is a char* the core allocates.
            IntPtr resultPointer;
            bool ok = NativeMethods.BNGetTypeParserOptionText(
                this.handle ,
                option ,
                value ?? string.Empty ,
                out resultPointer
            );

            // 2. Decode + free the result, or null on failure.
            if (!ok)
            {
                return null;
            }

            return UnsafeUtils.TakeUtf8String(resultPointer);
        }

        /// <summary>
        /// Parses a single type string into a qualified name and type.
        /// </summary>
        /// <param name="source">The type string to parse (e.g. "int*").</param>
        /// <param name="platform">The platform for type context.</param>
        /// <param name="existingTypes">The type container with existing types for reference.</param>
        /// <param name="errors">Output array of parser errors if parsing fails.</param>
        /// <returns>The parsed qualified name and type, or null on failure.</returns>
        public unsafe QualifiedNameAndType? ParseTypeString(
            string source ,
            Platform platform ,
            TypeContainer existingTypes ,
            out TypeParserError[] errors
        )
        {
            // 1. Validate required parameters.
            if (null == platform)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            if (null == existingTypes)
            {
                throw new ArgumentNullException(nameof(existingTypes));
            }

            // 2. Stack-allocate result and error output variables.
            BNQualifiedNameAndType rawResult = default;
            IntPtr errorsPointer = IntPtr.Zero;
            ulong errorCount = 0;

            // 3. Call the native function.
            bool ok = NativeMethods.BNTypeParserParseTypeString(
                this.handle ,
                source ?? string.Empty ,
                platform.DangerousGetHandle() ,
                existingTypes.DangerousGetHandle() ,
                (IntPtr)(&rawResult) ,
                (IntPtr)(&errorsPointer) ,
                (IntPtr)(&errorCount)
            );

            // 4. On success, return the parsed result.
            if (ok)
            {
                errors = Array.Empty<TypeParserError>();

                return QualifiedNameAndType.FromNative(rawResult);
            }
            else
            {
                // 5. On failure, convert the native error array to managed objects.
                errors = UnsafeUtils.TakeStructArrayEx<BNTypeParserError , TypeParserError>(
                    errorsPointer ,
                    errorCount ,
                    TypeParserError.FromNative ,
                    NativeMethods.BNFreeTypeParserErrors
                );

                return null;
            }
        }

        /// <summary>
        /// Parses types from a source string (e.g. a C header).
        /// </summary>
        /// <param name="source">The source text to parse.</param>
        /// <param name="fileName">The virtual filename for error messages.</param>
        /// <param name="platform">The platform for type context.</param>
        /// <param name="existingTypes">The type container with existing types for reference.</param>
        /// <param name="options">Compiler options (e.g. "-std=c99").</param>
        /// <param name="includeDirs">Include directory paths for resolving #include directives.</param>
        /// <param name="autoTypeSource">The auto type source identifier.</param>
        /// <param name="errors">Output array of parser errors if parsing fails.</param>
        /// <returns>The parsed type result, or null on failure.</returns>
        public unsafe TypeParserResult? ParseTypesFromSource(
            string source ,
            string fileName ,
            Platform platform ,
            TypeContainer existingTypes ,
            string[] options ,
            string[] includeDirs ,
            string autoTypeSource ,
            out TypeParserError[] errors
        )
        {
            // 1. Validate required parameters.
            if (null == platform)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            if (null == existingTypes)
            {
                throw new ArgumentNullException(nameof(existingTypes));
            }

            // 2. Stack-allocate result and error output variables.
            BNTypeParserResult rawResult = default;
            IntPtr errorsPointer = IntPtr.Zero;
            ulong errorCount = 0;

            // 3. Prepare safe arrays for options and include dirs.
            string[] safeOptions = options ?? Array.Empty<string>();
            string[] safeIncludeDirs = includeDirs ?? Array.Empty<string>();

            // 4. Call the native function. options/includeDirs are const char**
            // UTF-8 input blocks; build them by hand because .NET cannot apply
            // LPUTF8Str to string[] array elements (non-ASCII would otherwise
            // corrupt through the system ANSI code page).
            bool ok;
            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                IntPtr optionsBlock = allocator.AllocUtf8StringArray(safeOptions);
                IntPtr includeDirsBlock = allocator.AllocUtf8StringArray(safeIncludeDirs);

                ok = NativeMethods.BNTypeParserParseTypesFromSource(
                    this.handle ,
                    source ?? string.Empty ,
                    fileName ?? string.Empty ,
                    platform.DangerousGetHandle() ,
                    existingTypes.DangerousGetHandle() ,
                    optionsBlock ,
                    (ulong)safeOptions.Length ,
                    includeDirsBlock ,
                    (ulong)safeIncludeDirs.Length ,
                    autoTypeSource ?? string.Empty ,
                    (IntPtr)(&rawResult) ,
                    (IntPtr)(&errorsPointer) ,
                    (IntPtr)(&errorCount)
                );
            }

            // 5. On success, return the parsed result.
            if (ok)
            {
                errors = Array.Empty<TypeParserError>();

                return TypeParserResult.TakeNative(rawResult);
            }
            else
            {
                // 6. On failure, convert the native error array to managed objects.
                errors = UnsafeUtils.TakeStructArrayEx<BNTypeParserError , TypeParserError>(
                    errorsPointer ,
                    errorCount ,
                    TypeParserError.FromNative ,
                    NativeMethods.BNFreeTypeParserErrors
                );

                return null;
            }
        }

        /// <summary>
        /// Preprocesses a source string (runs the C preprocessor only, without parsing types).
        /// </summary>
        /// <param name="source">The source text to preprocess.</param>
        /// <param name="fileName">The virtual filename for error messages.</param>
        /// <param name="platform">The platform for the preprocessor context.</param>
        /// <param name="existingTypes">The type container with existing types for reference.</param>
        /// <param name="options">Compiler options.</param>
        /// <param name="includeDirs">Include directory paths for resolving #include directives.</param>
        /// <param name="errors">Output array of parser errors if preprocessing fails.</param>
        /// <returns>The preprocessed source text, or null on failure.</returns>
        public unsafe string? PreprocessSource(
            string source ,
            string fileName ,
            Platform platform ,
            TypeContainer existingTypes ,
            string[] options ,
            string[] includeDirs ,
            out TypeParserError[] errors
        )
        {
            // 1. Validate required parameters.
            if (null == platform)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            if (null == existingTypes)
            {
                throw new ArgumentNullException(nameof(existingTypes));
            }

            // 2. Prepare error output variables.
            IntPtr errorsPointer = IntPtr.Zero;
            ulong errorCount = 0;

            string[] safeOptions = options ?? Array.Empty<string>();
            string[] safeIncludeDirs = includeDirs ?? Array.Empty<string>();

            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                // 3. Build the const char** option / include-dir blocks as UTF-8.
                IntPtr optionsBlock = allocator.AllocUtf8StringArray(safeOptions);
                IntPtr includeDirsBlock = allocator.AllocUtf8StringArray(safeIncludeDirs);

                // 4. Call the native function; output is an out char* the core allocates.
                IntPtr outputPointer;
                bool ok = NativeMethods.BNTypeParserPreprocessSource(
                    this.handle ,
                    source ?? string.Empty ,
                    fileName ?? string.Empty ,
                    platform.DangerousGetHandle() ,
                    existingTypes.DangerousGetHandle() ,
                    optionsBlock ,
                    (ulong)safeOptions.Length ,
                    includeDirsBlock ,
                    (ulong)safeIncludeDirs.Length ,
                    out outputPointer ,
                    (IntPtr)(&errorsPointer) ,
                    (IntPtr)(&errorCount)
                );

                // 5. On success, decode + free the preprocessed output.
                if (ok)
                {
                    errors = Array.Empty<TypeParserError>();

                    return UnsafeUtils.TakeUtf8String(outputPointer);
                }
                else
                {
                    // 6. On failure, convert the native error array to managed objects.
                    errors = UnsafeUtils.TakeStructArrayEx<BNTypeParserError , TypeParserError>(
                        errorsPointer ,
                        errorCount ,
                        TypeParserError.FromNative ,
                        NativeMethods.BNFreeTypeParserErrors
                    );

                    return null;
                }
            }
        }

        /// <summary>
        /// Retrieves all registered type parsers from the engine.
        /// Each returned parser is a borrowed reference managed by the native engine.
        /// </summary>
        /// <returns>An array of all registered TypeParser instances.</returns>
        public static unsafe TypeParser[] GetList()
        {
            // 1. Stack-allocate the count variable.
            ulong count = 0;

            // 2. Retrieve the native array of parser pointers.
            IntPtr arrayPointer = NativeMethods.BNGetTypeParserList((IntPtr)(&count));

            // 3. Convert to managed array of borrowed handles and free the native pointer array.
            return UnsafeUtils.TakeHandleArray<TypeParser>(
                arrayPointer ,
                count ,
                TypeParser.MustBorrowHandle ,
                NativeMethods.BNFreeTypeParserList
            );
        }
    }
}
