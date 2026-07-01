using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a registered language representation function type, which defines a
    /// decompilation language (e.g., C, C++, LLIL). Handles are always borrowed from
    /// the native engine's global registry and must not be freed by this wrapper.
    /// </summary>
    public sealed class LanguageRepresentationFunctionType : AbstractSafeHandle<LanguageRepresentationFunctionType>
    {
        /// <summary>
        /// Initializes a new LanguageRepresentationFunctionType wrapper around an existing
        /// borrowed handle. The handle is never owned by this instance.
        /// </summary>
        /// <param name="handle">The native pointer to the BNLanguageRepresentationFunctionType object.</param>
        internal LanguageRepresentationFunctionType(IntPtr handle)
            : base(handle, false)
        {
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNLanguageRepresentationFunctionType pointer.</param>
        /// <returns>A new LanguageRepresentationFunctionType instance that will not free the handle on dispose.</returns>
        internal static LanguageRepresentationFunctionType? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new LanguageRepresentationFunctionType(handle);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNLanguageRepresentationFunctionType pointer.</param>
        /// <returns>A new LanguageRepresentationFunctionType instance that will not free the handle on dispose.</returns>
        internal static LanguageRepresentationFunctionType MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new LanguageRepresentationFunctionType(handle);
        }

        /// <summary>
        /// No-op release: language representation function type handles are always borrowed
        /// from the global registry and must not be freed by this wrapper.
        /// </summary>
        /// <returns>True (always, since no deallocation is performed).</returns>
        protected override bool ReleaseHandle()
        {
            // Objects are borrowed from the global registry; the native engine owns their lifetime.
            return true;
        }

        /// <summary>
        /// Gets the registered name that uniquely identifies this language representation type.
        /// </summary>
        public string Name
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the language type name.
                IntPtr raw = NativeMethods.BNGetLanguageRepresentationFunctionTypeName(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the line formatter associated with this language representation type.
        /// Returns null if no line formatter is registered.
        /// </summary>
        public LineFormatter? Formatter
        {
            get
            {
                // Borrow the native line formatter handle; it is owned by the global registry.
                return LineFormatter.BorrowHandle(
                    NativeMethods.BNGetLanguageRepresentationFunctionTypeLineFormatter(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets the type parser associated with this language representation type.
        /// Returns null if no parser is registered.
        /// </summary>
        public TypeParser? Parser
        {
            get
            {
                // Borrow the native type parser handle; it is owned by the global registry.
                return TypeParser.BorrowHandle(
                    NativeMethods.BNGetLanguageRepresentationFunctionTypeParser(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets the type printer associated with this language representation type.
        /// Returns null if no printer is registered.
        /// </summary>
        public TypePrinter? Printer
        {
            get
            {
                // Borrow the native type printer handle; it is owned by the global registry.
                return TypePrinter.BorrowHandle(
                    NativeMethods.BNGetLanguageRepresentationFunctionTypePrinter(this.handle)
                );
            }
        }

        /// <summary>
        /// Determines whether this language representation type is valid for the given binary view.
        /// </summary>
        /// <param name="view">The binary view to check validity against.</param>
        /// <returns>True if this language representation type is applicable to the given view.</returns>
        public bool IsValidFor(BinaryView view)
        {
            // Retrieve the raw handle for the view; pass zero if null (although view should not be null).
            IntPtr viewHandle = (view != null) ? view.DangerousGetHandle() : IntPtr.Zero;

            // Delegate to the native validity check.
            return NativeMethods.BNIsLanguageRepresentationFunctionTypeValid(this.handle, viewHandle);
        }

        /// <summary>
        /// Retrieves all registered language representation function types from the engine.
        /// Each returned instance is a borrowed reference managed by the native engine.
        /// </summary>
        /// <returns>An array of all registered LanguageRepresentationFunctionType instances.</returns>
        /// <summary>
        /// Gets the function type declaration tokens for the given function using this language
        /// representation type. Returns the declaration as an array of disassembly text lines.
        /// </summary>
        /// <param name="func">The function to get type tokens for.</param>
        /// <param name="settings">Optional disassembly settings. Pass null for defaults.</param>
        /// <returns>An array of DisassemblyTextLine containing the function type tokens.</returns>
        public unsafe DisassemblyTextLine[] GetFunctionTypeTokens(
            Function func ,
            DisassemblySettings? settings = null
        )
        {
            // 1. Stack-allocate the count variable.
            ulong count = 0;

            // 2. Call the native API.
            IntPtr arrayPointer = NativeMethods.BNGetLanguageRepresentationFunctionTypeFunctionTypeTokens(
                this.handle ,
                func.DangerousGetHandle() ,
                (null != settings) ? settings.DangerousGetHandle() : IntPtr.Zero ,
                (IntPtr)(&count)
            );

            // 3. Convert the native array to managed objects and free the native memory.
            return UnsafeUtils.TakeStructArrayEx<BNDisassemblyTextLine , DisassemblyTextLine>(
                arrayPointer ,
                count ,
                DisassemblyTextLine.FromNative ,
                NativeMethods.BNFreeDisassemblyTextLines
            );
        }

        public static LanguageRepresentationFunctionType[] GetList()
        {
            // 1. Call the native API to get the array of language representation function type pointers.
            IntPtr arrayPointer = NativeMethods.BNGetLanguageRepresentationFunctionTypeList(
                out ulong count
            );

            // 2. Convert to managed array of borrowed handles and free the native pointer array.
            return UnsafeUtils.TakeHandleArray<LanguageRepresentationFunctionType>(
                arrayPointer ,
                count ,
                LanguageRepresentationFunctionType.MustBorrowHandle ,
                NativeMethods.BNFreeLanguageRepresentationFunctionTypeList
            );
        }
    }
}
