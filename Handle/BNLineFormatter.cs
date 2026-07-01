using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a registered line formatter that controls how disassembly lines are laid out
    /// and formatted. LineFormatter handles are always borrowed — the formatter lifetime is
    /// managed by the native engine's global registry.
    /// </summary>
    public sealed class LineFormatter : AbstractSafeHandle<LineFormatter>
    {
        /// <summary>
        /// Initializes a new LineFormatter wrapper around an existing borrowed handle.
        /// The handle is never owned — the formatter lifetime is managed by the native engine.
        /// </summary>
        /// <param name="handle">The native pointer to the BNLineFormatter object.</param>
        internal LineFormatter(IntPtr handle)
            : base(handle, false)
        {
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNLineFormatter pointer.</param>
        /// <returns>A new LineFormatter instance that will not free the handle on dispose.</returns>
        internal static LineFormatter? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new LineFormatter(handle);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNLineFormatter pointer.</param>
        /// <returns>A new LineFormatter instance that will not free the handle on dispose.</returns>
        internal static LineFormatter MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new LineFormatter(handle);
        }

        /// <summary>
        /// No-op release: line formatter handles are always borrowed from the global registry
        /// and must not be freed by this wrapper.
        /// </summary>
        /// <returns>True (always, since no deallocation is performed).</returns>
        protected override bool ReleaseHandle()
        {
            // Formatter objects are borrowed from the global registry; the native engine owns their lifetime.
            return true;
        }

        /// <summary>
        /// Gets the registered name that uniquely identifies this line formatter.
        /// </summary>
        public string Name
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the formatter name.
                IntPtr raw = NativeMethods.BNGetLineFormatterName(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        // ===================================================================
        // Static lookup methods
        // ===================================================================

        /// <summary>
        /// Looks up a registered line formatter by its unique name.
        /// Returns null if no formatter with the given name exists.
        /// </summary>
        /// <param name="name">The registered name of the formatter to find.</param>
        /// <returns>A borrowed LineFormatter instance, or null if not found.</returns>
        public static LineFormatter? GetByName(string name)
        {
            // Query the global registry for a formatter with the specified name.
            IntPtr result = NativeMethods.BNGetLineFormatterByName(name);

            // Wrap as a borrowed handle; returns null when the native pointer is zero.
            return LineFormatter.BorrowHandle(result);
        }

        /// <summary>
        /// Gets the default line formatter registered with the engine.
        /// Returns null if no default formatter is configured.
        /// </summary>
        /// <returns>A borrowed LineFormatter instance, or null if unavailable.</returns>
        public static LineFormatter? GetDefault()
        {
            // Query the global registry for the default formatter.
            IntPtr result = NativeMethods.BNGetDefaultLineFormatter();

            // Wrap as a borrowed handle; returns null when the native pointer is zero.
            return LineFormatter.BorrowHandle(result);
        }

        /// <summary>
        /// Gets the default line formatter settings for the given disassembly settings
        /// and optional high-level IL function context. The returned settings struct
        /// describes the line layout parameters the default formatter would use.
        /// </summary>
        /// <param name="settings">The disassembly settings to query defaults for.</param>
        /// <param name="func">Optional HLIL function context; null for defaults without IL context.</param>
        /// <returns>A LineFormatterSettings populated with default values.</returns>
        public static unsafe LineFormatterSettings GetDefaultSettings(
            DisassemblySettings settings ,
            HighLevelILFunction? func = null)
        {
            // 1. Resolve the raw handles for the required and optional parameters.
            IntPtr settingsHandle = settings.DangerousGetHandle();
            IntPtr funcHandle = (func != null) ? func.DangerousGetHandle() : IntPtr.Zero;

            // 2. Call the native function to obtain a heap-allocated BNLineFormatterSettings.
            IntPtr ptr = NativeMethods.BNGetDefaultLineFormatterSettings(settingsHandle , funcHandle);

            if (ptr == IntPtr.Zero)
            {
                return new LineFormatterSettings();
            }

            // 3. Read the native struct from the pointer.
            BNLineFormatterSettings native = Marshal.PtrToStructure<BNLineFormatterSettings>(ptr);

            // 4. Convert to managed type.
            LineFormatterSettings result = new LineFormatterSettings();
            result.DesiredLineLength = native.desiredLineLength;
            result.MinimumContentLength = native.minimumContentLength;
            result.TabWidth = native.tabWidth;
            result.MaximumAnnotationLength = native.maximumAnnotationLength;
            result.StringWrappingWidth = native.stringWrappingWidth;
            result.LanguageName = UnsafeUtils.ReadAnsiString(native.languageName);
            result.CommentStartString = UnsafeUtils.ReadAnsiString(native.commentStartString);
            result.CommentEndString = UnsafeUtils.ReadAnsiString(native.commentEndString);
            result.AnnotationStartString = UnsafeUtils.ReadAnsiString(native.annotationStartString);
            result.AnnotationEndString = UnsafeUtils.ReadAnsiString(native.annotationEndString);

            // 5. The HLIL function is a borrowed pointer inside the struct; wrap it if non-null.
            result.HighLevelIL = (native.highLevelIL != IntPtr.Zero)
                ? HighLevelILFunction.NewFromHandle(native.highLevelIL)
                : null;

            // 6. Free the native struct allocation.
            NativeMethods.BNFreeLineFormatterSettings(ptr);

            return result;
        }

        /// <summary>
        /// Gets all registered line formatters from the engine.
        /// Each returned formatter is a borrowed reference.
        /// </summary>
        /// <returns>An array of all registered LineFormatter instances.</returns>
        public static unsafe LineFormatter[] GetList()
        {
            // 1. Stack-allocate the count variable.
            ulong count = 0;

            // 2. Retrieve the native array of formatter pointers.
            IntPtr arrayPointer = NativeMethods.BNGetLineFormatterList((IntPtr)(&count));

            // 3. Convert to managed array of borrowed handles and free the native pointer array.
            return UnsafeUtils.TakeHandleArray<LineFormatter>(
                arrayPointer ,
                count ,
                LineFormatter.MustBorrowHandle ,
                NativeMethods.BNFreeLineFormatterList
            );
        }

        /// <summary>
        /// Formats an array of disassembly text lines according to the given settings.
        /// The formatter applies line wrapping, indentation, and annotation formatting.
        /// </summary>
        /// <param name="lines">The input lines to format.</param>
        /// <param name="settings">The formatter settings controlling output appearance.</param>
        /// <returns>An array of formatted DisassemblyTextLine objects.</returns>
        public unsafe DisassemblyTextLine[] FormatLines(
            DisassemblyTextLine[] lines ,
            LineFormatterSettings settings
        )
        {
            // 1. Handle empty input.
            if (null == lines || 0 == lines.Length)
            {
                return Array.Empty<DisassemblyTextLine>();
            }

            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                // 2. Marshal the input lines to native structs.
                BNDisassemblyTextLine[] nativeLines = new BNDisassemblyTextLine[lines.Length];
                for (int i = 0; i < lines.Length; i++)
                {
                    nativeLines[i] = lines[i].ToNativeEx(allocator);
                }
                IntPtr linesPtr = allocator.AllocStructArray<BNDisassemblyTextLine>(nativeLines);

                // 3. Marshal the settings struct.
                BNLineFormatterSettings nativeSettings = new BNLineFormatterSettings()
                {
                    highLevelIL = (null != settings.HighLevelIL)
                        ? settings.HighLevelIL.DangerousGetHandle()
                        : IntPtr.Zero ,
                    desiredLineLength = settings.DesiredLineLength ,
                    minimumContentLength = settings.MinimumContentLength ,
                    tabWidth = settings.TabWidth ,
                    maximumAnnotationLength = settings.MaximumAnnotationLength ,
                    stringWrappingWidth = settings.StringWrappingWidth ,
                    languageName = allocator.AllocAnsiString(settings.LanguageName) ,
                    commentStartString = allocator.AllocAnsiString(settings.CommentStartString) ,
                    commentEndString = allocator.AllocAnsiString(settings.CommentEndString) ,
                    annotationStartString = allocator.AllocAnsiString(settings.AnnotationStartString) ,
                    annotationEndString = allocator.AllocAnsiString(settings.AnnotationEndString)
                };
                IntPtr settingsPtr = allocator.AllocStruct<BNLineFormatterSettings>(nativeSettings);

                // 4. Stack-allocate the output count.
                ulong outCount = 0;

                // 5. Call the native API.
                IntPtr resultPointer = NativeMethods.BNFormatLines(
                    this.handle ,
                    linesPtr ,
                    (ulong)lines.Length ,
                    settingsPtr ,
                    (IntPtr)(&outCount)
                );

                // 6. Convert the native result array to managed objects and free the native memory.
                return UnsafeUtils.TakeStructArrayEx<BNDisassemblyTextLine , DisassemblyTextLine>(
                    resultPointer ,
                    outCount ,
                    DisassemblyTextLine.FromNative ,
                    NativeMethods.BNFreeDisassemblyTextLines
                );
            }
        }
    }
}
