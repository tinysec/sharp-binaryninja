using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a mapping between a binary view and a type library.
    /// TypeLibraryMapping handles are always borrowed — the mapping lifetime is
    /// managed by the native engine.
    /// </summary>
    public sealed class TypeLibraryMapping : AbstractSafeHandle<TypeLibraryMapping>
    {
        /// <summary>
        /// Initializes a new TypeLibraryMapping wrapper around an existing borrowed handle.
        /// The handle is never owned — the mapping lifetime is managed by the native engine.
        /// </summary>
        /// <param name="handle">The native pointer to the BNTypeLibraryMapping object.</param>
        internal TypeLibraryMapping(IntPtr handle)
            : base(handle, false)
        {
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNTypeLibraryMapping pointer.</param>
        /// <returns>A new TypeLibraryMapping that will not free the handle on dispose.</returns>
        internal static TypeLibraryMapping? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new TypeLibraryMapping(handle);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNTypeLibraryMapping pointer.</param>
        /// <returns>A new TypeLibraryMapping that will not free the handle on dispose.</returns>
        internal static TypeLibraryMapping MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new TypeLibraryMapping(handle);
        }

        /// <summary>
        /// No-op release: type library mapping handles are always borrowed from the native engine
        /// and must not be freed by this wrapper.
        /// </summary>
        /// <returns>True (always, since no deallocation is performed).</returns>
        protected override bool ReleaseHandle()
        {
            // Mapping handles are borrowed from the engine; the native engine owns their lifetime.
            return true;
        }
    }
}
