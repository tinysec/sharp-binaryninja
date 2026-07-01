using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents an external location: a mapping from a symbol in the current binary
    /// to a target in another (external) library. External locations are used to
    /// document inter-library call targets, typically for static analysis of imported
    /// functions whose implementations live outside the analysed binary.
    /// </summary>
    public sealed class ExternalLocation : AbstractSafeHandle<ExternalLocation>
    {
        /// <summary>
        /// Initializes a new ExternalLocation wrapper around an existing native handle.
        /// </summary>
        /// <param name="handle">The native pointer to the BNExternalLocation object.</param>
        /// <param name="owner">True if this instance owns the handle and should free it on dispose.</param>
        internal ExternalLocation(IntPtr handle, bool owner)
            : base(handle, owner)
        {
        }

        /// <summary>
        /// Creates a new managed ExternalLocation by incrementing the reference count on an existing native handle.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNExternalLocation pointer.</param>
        /// <returns>A new ExternalLocation instance, or null if handle is zero.</returns>
        internal static ExternalLocation? NewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new ExternalLocation(
                NativeMethods.BNNewExternalLocationReference(handle),
                true
            );
        }

        /// <summary>
        /// Creates a new managed ExternalLocation by incrementing the reference count. Throws if handle is zero.
        /// </summary>
        /// <param name="handle">The native BNExternalLocation pointer.</param>
        /// <returns>A new ExternalLocation instance.</returns>
        internal static ExternalLocation MustNewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new ExternalLocation(
                NativeMethods.BNNewExternalLocationReference(handle),
                true
            );
        }

        /// <summary>
        /// Takes ownership of an existing native handle without incrementing the reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNExternalLocation pointer.</param>
        /// <returns>A new ExternalLocation instance, or null if handle is zero.</returns>
        internal static ExternalLocation? TakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new ExternalLocation(handle, true);
        }

        /// <summary>
        /// Takes ownership of an existing native handle without incrementing the reference count. Throws if zero.
        /// </summary>
        /// <param name="handle">The native BNExternalLocation pointer.</param>
        /// <returns>A new ExternalLocation instance.</returns>
        internal static ExternalLocation MustTakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new ExternalLocation(handle, true);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNExternalLocation pointer.</param>
        /// <returns>A new ExternalLocation instance that will not free the handle on dispose.</returns>
        internal static ExternalLocation? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new ExternalLocation(handle, false);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNExternalLocation pointer.</param>
        /// <returns>A new ExternalLocation instance that will not free the handle on dispose.</returns>
        internal static ExternalLocation MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new ExternalLocation(handle, false);
        }

        /// <summary>
        /// Releases the native BNExternalLocation handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native external location handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeExternalLocation(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Gets or sets the external library that this location maps into.
        /// The library identifies which external binary contains the target.
        /// </summary>
        public ExternalLibrary? ExternalLibrary
        {
            get
            {
                // Retrieve an owned reference to the associated external library.
                return BinaryNinja.ExternalLibrary.NewFromHandle(
                    NativeMethods.BNExternalLocationGetExternalLibrary(this.handle)
                );
            }

            set
            {
                // 1. Resolve the library handle; pass zero to clear the association.
                IntPtr libraryHandle = value != null ? value.DangerousGetHandle() : IntPtr.Zero;

                // 2. Forward the new library association to the native API.
                NativeMethods.BNExternalLocationSetExternalLibrary(this.handle, libraryHandle);
            }
        }

        /// <summary>
        /// Gets the source symbol in the current binary that this external location originates from.
        /// The source symbol is the import stub or reference whose true implementation is external.
        /// </summary>
        public Symbol? SourceSymbol
        {
            get
            {
                // Retrieve a borrowed reference to the source symbol; take ownership via TakeHandle.
                return BinaryNinja.Symbol.TakeHandle(
                    NativeMethods.BNExternalLocationGetSourceSymbol(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets whether this external location has a target address (as opposed to a target symbol name).
        /// An external location may specify either a numeric address or a symbol name, but not both.
        /// </summary>
        public bool HasTargetAddress
        {
            get
            {
                // Query the native flag indicating a numeric target address is present.
                return NativeMethods.BNExternalLocationHasTargetAddress(this.handle);
            }
        }

        /// <summary>
        /// Gets whether this external location has a target symbol name (as opposed to a target address).
        /// </summary>
        public bool HasTargetSymbol
        {
            get
            {
                // Query the native flag indicating a named target symbol is present.
                return NativeMethods.BNExternalLocationHasTargetSymbol(this.handle);
            }
        }

        /// <summary>
        /// Gets the numeric target address within the external library, if one has been set.
        /// Check HasTargetAddress before reading; the return value is undefined when false.
        /// </summary>
        public ulong TargetAddress
        {
            get
            {
                // Retrieve the 64-bit unsigned target address from the native layer.
                return NativeMethods.BNExternalLocationGetTargetAddress(this.handle);
            }
        }

        /// <summary>
        /// Gets the target symbol name within the external library, if one has been set.
        /// Check HasTargetSymbol before reading; returns null when no symbol name is set.
        /// </summary>
        public string? TargetSymbol
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the target symbol name.
                IntPtr raw = NativeMethods.BNExternalLocationGetTargetSymbol(this.handle);

                // 2. Copy and free the native string; null means no symbol is set.
                return UnsafeUtils.TakeAnsiString(raw);
            }
        }

        /// <summary>
        /// Sets the numeric target address for this external location, replacing any existing
        /// target address or target symbol. Pass null to clear the target address.
        /// </summary>
        /// <param name="address">The target address to set, or null to clear it.</param>
        /// <returns>True if the operation succeeded.</returns>
        public bool SetTargetAddress(ulong? address)
        {
            unsafe
            {
                if (address.HasValue)
                {
                    // 1. Place the address on the stack so we can take its pointer.
                    ulong value = address.Value;

                    // 2. Pass a pointer to the value to the native API.
                    return NativeMethods.BNExternalLocationSetTargetAddress(
                        this.handle,
                        (IntPtr)(&value)
                    );
                }

                // 3. Pass zero to clear the target address.
                return NativeMethods.BNExternalLocationSetTargetAddress(
                    this.handle,
                    IntPtr.Zero
                );
            }
        }

        /// <summary>
        /// Sets the target symbol name for this external location, replacing any existing
        /// target address or target symbol. Pass null to clear the target symbol.
        /// </summary>
        /// <param name="symbol">The target symbol name to set, or null to clear it.</param>
        /// <returns>True if the operation succeeded.</returns>
        public bool SetTargetSymbol(string? symbol)
        {
            // Forward the symbol string (or null to clear) to the native API.
            return NativeMethods.BNExternalLocationSetTargetSymbol(
                this.handle,
                symbol ?? string.Empty
            );
        }
    }
}
