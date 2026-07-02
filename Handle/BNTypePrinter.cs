using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a registered type printer that renders Binary Ninja type objects into
    /// human-readable text. TypePrinter handles are always borrowed — the printer
    /// lifetime is managed by the native engine's global registry.
    /// </summary>
    public sealed class TypePrinter : AbstractSafeHandle<TypePrinter>
    {
        /// <summary>
        /// Initializes a new TypePrinter wrapper around an existing borrowed handle.
        /// The handle is never owned — the printer lifetime is managed by the native engine.
        /// </summary>
        /// <param name="handle">The native pointer to the BNTypePrinter object.</param>
        internal TypePrinter(IntPtr handle)
            : base(handle, false)
        {
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNTypePrinter pointer.</param>
        /// <returns>A new TypePrinter instance that will not free the handle on dispose.</returns>
        internal static TypePrinter? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new TypePrinter(handle);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNTypePrinter pointer.</param>
        /// <returns>A new TypePrinter instance that will not free the handle on dispose.</returns>
        internal static TypePrinter MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new TypePrinter(handle);
        }

        /// <summary>
        /// No-op release: type printer handles are always borrowed from the global registry
        /// and must not be freed by this wrapper.
        /// </summary>
        /// <returns>True (always, since no deallocation is performed).</returns>
        protected override bool ReleaseHandle()
        {
            // Printer objects are borrowed from the global registry; the native engine owns their lifetime.
            return true;
        }

        /// <summary>
        /// Gets the registered name that uniquely identifies this type printer.
        /// </summary>
        public string Name
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the printer name.
                IntPtr raw = NativeMethods.BNGetTypePrinterName(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Looks up a registered type printer by its name.
        /// Returns null if no printer is registered with the given name.
        /// </summary>
        /// <param name="name">The registered name of the type printer.</param>
        /// <returns>The matching TypePrinter instance, or null if not found.</returns>
        public static TypePrinter? GetByName(string name)
        {
            // Query the global registry for a printer with the given name.
            return TypePrinter.BorrowHandle(
                NativeMethods.BNGetTypePrinterByName(name ?? string.Empty)
            );
        }

        /// <summary>
        /// Gets the default type printer, selected by the "analysis.types.printerName" setting.
        /// Mirrors Python TypePrinter.default (there is no dedicated core symbol; the default is
        /// the registered printer named by that setting). Returns null if unset or not registered.
        /// </summary>
        public static TypePrinter? GetDefault()
        {
            using (Settings settings = new Settings())
            {
                string name = settings.GetString(
                    "analysis.types.printerName",
                    null,
                    null,
                    out SettingsScope scope
                );

                if (string.IsNullOrEmpty(name))
                {
                    return null;
                }

                return TypePrinter.GetByName(name);
            }
        }

        /// <summary>
        /// Gets the full type definition lines for a type, rendered through this printer.
        /// </summary>
        /// <param name="type">The type to render.</param>
        /// <param name="types">The type container for resolving type references.</param>
        /// <param name="name">The qualified name of the type.</param>
        /// <param name="paddingCols">Number of padding columns for indentation.</param>
        /// <param name="collapsed">Whether to render the type in collapsed form.</param>
        /// <param name="escaping">The token escaping mode.</param>
        /// <returns>An array of type definition lines, or an empty array on failure.</returns>
        public unsafe TypeDefinitionLine[] GetTypeLines(
            BinaryNinja.Type type ,
            TypeContainer types ,
            QualifiedName name ,
            int paddingCols ,
            bool collapsed ,
            TokenEscapingType escaping
        )
        {
            // 1. Validate required parameters.
            if (null == type)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (null == types)
            {
                throw new ArgumentNullException(nameof(types));
            }

            // 2. Stack-allocate result pointer and count.
            IntPtr resultPointer = IntPtr.Zero;
            ulong resultCount = 0;

            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                // 3. Call the native function.
                bool ok = NativeMethods.BNGetTypePrinterTypeLines(
                    this.handle ,
                    type.DangerousGetHandle() ,
                    types.DangerousGetHandle() ,
                    allocator.AllocStruct<BNQualifiedName>(name.ToNativeEx(allocator)) ,
                    paddingCols ,
                    collapsed ,
                    escaping ,
                    (IntPtr)(&resultPointer) ,
                    (IntPtr)(&resultCount)
                );

                // 4. If successful, convert the native array to managed objects.
                if (!ok)
                {
                    return Array.Empty<TypeDefinitionLine>();
                }

                return UnsafeUtils.TakeStructArrayEx<BNTypeDefinitionLine , TypeDefinitionLine>(
                    resultPointer ,
                    resultCount ,
                    TypeDefinitionLine.FromNative ,
                    NativeMethods.BNFreeTypeDefinitionLineList
                );
            }
        }

        /// <summary>
        /// Gets the full string representation of a type through this printer.
        /// </summary>
        /// <param name="type">The type to render.</param>
        /// <param name="platform">The platform for the type context.</param>
        /// <param name="name">The qualified name of the type.</param>
        /// <param name="escaping">The token escaping mode.</param>
        /// <returns>The type string, or null on failure.</returns>
        public string? GetTypeString(
            BinaryNinja.Type type ,
            Platform platform ,
            QualifiedName name ,
            TokenEscapingType escaping
        )
        {
            // 1. Validate required parameters.
            if (null == type)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (null == platform)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            // 2. Prepare the output array for the result string pointer.
            string[] result = new string[1];

            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                // 3. Call the native function.
                bool ok = NativeMethods.BNGetTypePrinterTypeString(
                    this.handle ,
                    type.DangerousGetHandle() ,
                    platform.DangerousGetHandle() ,
                    allocator.AllocStruct<BNQualifiedName>(name.ToNativeEx(allocator)) ,
                    escaping ,
                    result
                );

                // 4. Return the result string or null on failure.
                if (!ok)
                {
                    return null;
                }

                return result[0];
            }
        }

        /// <summary>
        /// Gets the portion of a type string that appears after the type name.
        /// </summary>
        /// <param name="type">The type to render.</param>
        /// <param name="platform">The platform for the type context.</param>
        /// <param name="escaping">The token escaping mode.</param>
        /// <returns>The after-name string, or null on failure.</returns>
        public string? GetTypeStringAfterName(
            BinaryNinja.Type type ,
            Platform platform ,
            TokenEscapingType escaping
        )
        {
            // 1. Validate required parameters.
            if (null == type)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (null == platform)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            // 2. Prepare the output array for the result string pointer.
            string[] result = new string[1];

            // 3. Call the native function.
            bool ok = NativeMethods.BNGetTypePrinterTypeStringAfterName(
                this.handle ,
                type.DangerousGetHandle() ,
                platform.DangerousGetHandle() ,
                escaping ,
                result
            );

            // 4. Return the result string or null on failure.
            if (!ok)
            {
                return null;
            }

            return result[0];
        }

        /// <summary>
        /// Gets the portion of a type string that appears before the type name.
        /// </summary>
        /// <param name="type">The type to render.</param>
        /// <param name="platform">The platform for the type context.</param>
        /// <param name="escaping">The token escaping mode.</param>
        /// <returns>The before-name string, or null on failure.</returns>
        public string? GetTypeStringBeforeName(
            BinaryNinja.Type type ,
            Platform platform ,
            TokenEscapingType escaping
        )
        {
            // 1. Validate required parameters.
            if (null == type)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (null == platform)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            // 2. Prepare the output array for the result string pointer.
            string[] result = new string[1];

            // 3. Call the native function.
            bool ok = NativeMethods.BNGetTypePrinterTypeStringBeforeName(
                this.handle ,
                type.DangerousGetHandle() ,
                platform.DangerousGetHandle() ,
                escaping ,
                result
            );

            // 4. Return the result string or null on failure.
            if (!ok)
            {
                return null;
            }

            return result[0];
        }

        /// <summary>
        /// Gets the instruction text tokens for a type through this printer.
        /// </summary>
        /// <param name="type">The type to render.</param>
        /// <param name="platform">The platform for the type context.</param>
        /// <param name="name">The qualified name of the type.</param>
        /// <param name="baseConfidence">The base confidence for the tokens.</param>
        /// <param name="escaping">The token escaping mode.</param>
        /// <returns>An array of instruction text tokens, or an empty array on failure.</returns>
        public unsafe InstructionTextToken[] GetTypeTokens(
            BinaryNinja.Type type ,
            Platform platform ,
            QualifiedName name ,
            byte baseConfidence ,
            TokenEscapingType escaping
        )
        {
            // 1. Validate required parameters.
            if (null == type)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (null == platform)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            // 2. Stack-allocate result pointer and count.
            IntPtr resultPointer = IntPtr.Zero;
            ulong resultCount = 0;

            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                // 3. Call the native function.
                bool ok = NativeMethods.BNGetTypePrinterTypeTokens(
                    this.handle ,
                    type.DangerousGetHandle() ,
                    platform.DangerousGetHandle() ,
                    allocator.AllocStruct<BNQualifiedName>(name.ToNativeEx(allocator)) ,
                    baseConfidence ,
                    escaping ,
                    (IntPtr)(&resultPointer) ,
                    (IntPtr)(&resultCount)
                );

                // 4. If successful, convert the native array to managed objects.
                if (!ok)
                {
                    return Array.Empty<InstructionTextToken>();
                }

                return UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken , InstructionTextToken>(
                    resultPointer ,
                    resultCount ,
                    InstructionTextToken.FromNative ,
                    NativeMethods.BNFreeInstructionText
                );
            }
        }

        /// <summary>
        /// Gets the instruction text tokens that appear after the name for a type.
        /// </summary>
        /// <param name="type">The type to render.</param>
        /// <param name="platform">The platform for the type context.</param>
        /// <param name="baseConfidence">The base confidence for the tokens.</param>
        /// <param name="parentType">The parent type for context, or null.</param>
        /// <param name="escaping">The token escaping mode.</param>
        /// <returns>An array of instruction text tokens, or an empty array on failure.</returns>
        public unsafe InstructionTextToken[] GetTypeTokensAfterName(
            BinaryNinja.Type type ,
            Platform platform ,
            byte baseConfidence ,
            BinaryNinja.Type? parentType ,
            TokenEscapingType escaping
        )
        {
            // 1. Validate required parameters.
            if (null == type)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (null == platform)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            // 2. Stack-allocate result pointer and count.
            IntPtr resultPointer = IntPtr.Zero;
            ulong resultCount = 0;

            // 3. Call the native function.
            bool ok = NativeMethods.BNGetTypePrinterTypeTokensAfterName(
                this.handle ,
                type.DangerousGetHandle() ,
                platform.DangerousGetHandle() ,
                baseConfidence ,
                null == parentType ? IntPtr.Zero : parentType.DangerousGetHandle() ,
                escaping ,
                (IntPtr)(&resultPointer) ,
                (IntPtr)(&resultCount)
            );

            // 4. If successful, convert the native array to managed objects.
            if (!ok)
            {
                return Array.Empty<InstructionTextToken>();
            }

            return UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken , InstructionTextToken>(
                resultPointer ,
                resultCount ,
                InstructionTextToken.FromNative ,
                NativeMethods.BNFreeInstructionText
            );
        }

        /// <summary>
        /// Gets the instruction text tokens that appear before the name for a type.
        /// </summary>
        /// <param name="type">The type to render.</param>
        /// <param name="platform">The platform for the type context.</param>
        /// <param name="baseConfidence">The base confidence for the tokens.</param>
        /// <param name="parentType">The parent type for context, or null.</param>
        /// <param name="escaping">The token escaping mode.</param>
        /// <returns>An array of instruction text tokens, or an empty array on failure.</returns>
        public unsafe InstructionTextToken[] GetTypeTokensBeforeName(
            BinaryNinja.Type type ,
            Platform platform ,
            byte baseConfidence ,
            BinaryNinja.Type? parentType ,
            TokenEscapingType escaping
        )
        {
            // 1. Validate required parameters.
            if (null == type)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (null == platform)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            // 2. Stack-allocate result pointer and count.
            IntPtr resultPointer = IntPtr.Zero;
            ulong resultCount = 0;

            // 3. Call the native function.
            bool ok = NativeMethods.BNGetTypePrinterTypeTokensBeforeName(
                this.handle ,
                type.DangerousGetHandle() ,
                platform.DangerousGetHandle() ,
                baseConfidence ,
                null == parentType ? IntPtr.Zero : parentType.DangerousGetHandle() ,
                escaping ,
                (IntPtr)(&resultPointer) ,
                (IntPtr)(&resultCount)
            );

            // 4. If successful, convert the native array to managed objects.
            if (!ok)
            {
                return Array.Empty<InstructionTextToken>();
            }

            return UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken , InstructionTextToken>(
                resultPointer ,
                resultCount ,
                InstructionTextToken.FromNative ,
                NativeMethods.BNFreeInstructionText
            );
        }

        /// <summary>
        /// Prints all provided types using the default implementation of this printer.
        /// This is the base (non-overridden) version of PrintAllTypes.
        /// </summary>
        /// <param name="names">An array of qualified names for the types.</param>
        /// <param name="types">An array of types to print.</param>
        /// <param name="data">The binary view for context.</param>
        /// <param name="paddingCols">Number of padding columns for indentation.</param>
        /// <param name="escaping">The token escaping mode.</param>
        /// <returns>The printed string, or null on failure.</returns>
        public static string? DefaultPrintAllTypes(
            TypePrinter printer ,
            QualifiedName[] names ,
            BinaryNinja.Type[] types ,
            BinaryView data ,
            int paddingCols ,
            TokenEscapingType escaping
        )
        {
            // 1. Validate required parameters.
            if (null == printer)
            {
                throw new ArgumentNullException(nameof(printer));
            }

            if (null == names || null == types)
            {
                throw new ArgumentNullException(null == names ? nameof(names) : nameof(types));
            }

            if (null == data)
            {
                throw new ArgumentNullException(nameof(data));
            }

            // 2. Marshal the names and types arrays.
            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                // 2.1 Marshal the qualified name array.
                BNQualifiedName[] nativeNames = new BNQualifiedName[names.Length];
                for (int i = 0; i < names.Length; i++)
                {
                    nativeNames[i] = names[i].ToNativeEx(allocator);
                }
                IntPtr namesArray = allocator.AllocStructArray<BNQualifiedName>(nativeNames);

                // 2.2 Marshal the type handle pointer array.
                IntPtr[] typeHandles = new IntPtr[types.Length];
                for (int i = 0; i < types.Length; i++)
                {
                    typeHandles[i] = types[i].DangerousGetHandle();
                }
                IntPtr typesArray = allocator.AllocStructArray<IntPtr>(typeHandles);

                // 3. Prepare the output array for the result string pointer.
                string[] result = new string[1];

                // 4. Call the native function.
                bool ok = NativeMethods.BNTypePrinterDefaultPrintAllTypes(
                    printer.handle ,
                    namesArray ,
                    typesArray ,
                    (ulong)types.Length ,
                    data.DangerousGetHandle() ,
                    paddingCols ,
                    escaping ,
                    result
                );

                // 5. Return the result or null on failure.
                if (!ok)
                {
                    return null;
                }

                return result[0];
            }
        }

        /// <summary>
        /// Prints all provided types using this printer instance.
        /// </summary>
        /// <param name="names">An array of qualified names for the types.</param>
        /// <param name="types">An array of types to print.</param>
        /// <param name="data">The binary view for context.</param>
        /// <param name="paddingCols">Number of padding columns for indentation.</param>
        /// <param name="escaping">The token escaping mode.</param>
        /// <returns>The printed string, or null on failure.</returns>
        public string? PrintAllTypes(
            QualifiedName[] names ,
            BinaryNinja.Type[] types ,
            BinaryView data ,
            int paddingCols ,
            TokenEscapingType escaping
        )
        {
            // 1. Validate required parameters.
            if (null == names || null == types)
            {
                throw new ArgumentNullException(null == names ? nameof(names) : nameof(types));
            }

            if (null == data)
            {
                throw new ArgumentNullException(nameof(data));
            }

            // 2. Marshal the names and types arrays.
            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                // 2.1 Marshal the qualified name array.
                BNQualifiedName[] nativeNames = new BNQualifiedName[names.Length];
                for (int i = 0; i < names.Length; i++)
                {
                    nativeNames[i] = names[i].ToNativeEx(allocator);
                }
                IntPtr namesArray = allocator.AllocStructArray<BNQualifiedName>(nativeNames);

                // 2.2 Marshal the type handle pointer array.
                IntPtr[] typeHandles = new IntPtr[types.Length];
                for (int i = 0; i < types.Length; i++)
                {
                    typeHandles[i] = types[i].DangerousGetHandle();
                }
                IntPtr typesArray = allocator.AllocStructArray<IntPtr>(typeHandles);

                // 3. Prepare the output array for the result string pointer.
                string[] result = new string[1];

                // 4. Call the native function.
                bool ok = NativeMethods.BNTypePrinterPrintAllTypes(
                    this.handle ,
                    namesArray ,
                    typesArray ,
                    (ulong)types.Length ,
                    data.DangerousGetHandle() ,
                    paddingCols ,
                    escaping ,
                    result
                );

                // 5. Return the result or null on failure.
                if (!ok)
                {
                    return null;
                }

                return result[0];
            }
        }

        /// <summary>
        /// Retrieves all registered type printers from the engine.
        /// Each returned printer is a borrowed reference managed by the native engine.
        /// </summary>
        /// <returns>An array of all registered TypePrinter instances.</returns>
        public static unsafe TypePrinter[] GetList()
        {
            // 1. Stack-allocate the count variable.
            ulong count = 0;

            // 2. Retrieve the native array of printer pointers.
            IntPtr arrayPointer = NativeMethods.BNGetTypePrinterList((IntPtr)(&count));

            // 3. Convert to managed array of borrowed handles and free the native pointer array.
            return UnsafeUtils.TakeHandleArray<TypePrinter>(
                arrayPointer ,
                count ,
                TypePrinter.MustBorrowHandle ,
                NativeMethods.BNFreeTypePrinterList
            );
        }
    }
}
