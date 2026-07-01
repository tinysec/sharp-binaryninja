using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a single relocation entry within a binary.
    /// Relocations describe patches that the linker or loader must apply to addresses
    /// at load time. Each relocation records its architecture, addressing info, address,
    /// target value, and an optional associated symbol.
    /// </summary>
    public sealed class Relocation : AbstractSafeHandle<Relocation>
    {
        /// <summary>
        /// Initializes a new Relocation wrapper around an existing native handle.
        /// </summary>
        /// <param name="handle">The native pointer to the BNRelocation object.</param>
        /// <param name="owner">True if this instance owns the handle and should free it on dispose.</param>
        public Relocation(IntPtr handle, bool owner)
            : base(handle, owner)
        {
        }

        /// <summary>
        /// Creates a new managed Relocation by incrementing the reference count on an existing native handle.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRelocation pointer.</param>
        /// <returns>A new Relocation instance, or null if handle is zero.</returns>
        internal static Relocation? NewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new Relocation(
                NativeMethods.BNNewRelocationReference(handle),
                true
            );
        }

        /// <summary>
        /// Creates a new managed Relocation by incrementing the reference count. Throws if handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRelocation pointer.</param>
        /// <returns>A new Relocation instance.</returns>
        internal static Relocation MustNewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new Relocation(
                NativeMethods.BNNewRelocationReference(handle),
                true
            );
        }

        /// <summary>
        /// Takes ownership of an existing native handle without incrementing the reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRelocation pointer.</param>
        /// <returns>A new Relocation instance, or null if handle is zero.</returns>
        internal static Relocation? TakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new Relocation(handle, true);
        }

        /// <summary>
        /// Takes ownership of an existing native handle without incrementing the reference count. Throws if zero.
        /// </summary>
        /// <param name="handle">The native BNRelocation pointer.</param>
        /// <returns>A new Relocation instance.</returns>
        internal static Relocation MustTakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new Relocation(handle, true);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRelocation pointer.</param>
        /// <returns>A new Relocation instance that will not free the handle on dispose.</returns>
        internal static Relocation? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new Relocation(handle, false);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRelocation pointer.</param>
        /// <returns>A new Relocation instance that will not free the handle on dispose.</returns>
        internal static Relocation MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new Relocation(handle, false);
        }

        /// <summary>
        /// Releases the native BNRelocation handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native relocation handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeRelocation(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Gets the architecture associated with this relocation.
        /// The architecture determines how the relocation offset and value are interpreted.
        /// </summary>
        public Architecture? Architecture
        {
            get
            {
                // Architecture handles are borrowed (not reference-counted), so use FromHandle.
                return BinaryNinja.Architecture.FromHandle(
                    NativeMethods.BNRelocationGetArchitecture(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets the detailed relocation info struct for this relocation entry.
        /// The struct is returned by value from the native layer and converted to a managed object.
        /// </summary>
        public RelocationInfo Info
        {
            get
            {
                // 1. Retrieve the native struct by value from the native function.
                BNRelocationInfo native = NativeMethods.BNRelocationGetInfo(this.handle);

                // 2. Map each native field to the managed RelocationInfo object.
                RelocationInfo managed = new RelocationInfo();

                // 2.1 Copy the relocation type enumeration value.
                managed.Type = native.type;

                // 2.2 Copy the PC-relative addressing flag.
                managed.PcRelative = native.pcRelative;

                // 2.3 Copy the base-relative addressing flag.
                managed.BaseRelative = native.baseRelative;

                // 2.4 Copy the base address used when base-relative is true.
                managed.Base = native._base;

                // 2.5 Copy the size of the patch in bytes.
                managed.Size = native.size;

                // 2.6 Copy the truncation size for partial-word relocations.
                managed.TruncateSize = native.truncateSize;

                // 2.7 Copy the native/architecture-specific relocation type code.
                managed.NativeType = native.nativeType;

                // 2.8 Copy the addend value (signed offset added to the relocated value).
                managed.Addend = native.addend;

                // 2.9 Copy the flag indicating whether the addend is signed.
                managed.HasSign = native.hasSign;

                // 2.10 Copy the flag indicating whether the addend is encoded implicitly.
                managed.ImplicitAddend = native.implicitAddend;

                // 2.11 Copy the flag indicating whether this is an external relocation.
                managed.External = native.external;

                // 2.12 Copy the index into the symbol table for this relocation.
                managed.SymbolIndex = native.symbolIndex;

                // 2.13 Copy the index of the section this relocation refers to.
                managed.SectionIndex = native.sectionIndex;

                // 2.14 Copy the virtual address of the location to patch.
                managed.Address = native.address;

                // 2.15 Copy the resolved target value (what will be written at Address).
                managed.Target = native.target;

                // 2.16 Copy the flag indicating whether this is a data relocation.
                managed.DataRelocation = native.dataRelocation;

                // 2.17 Prev/Next are chained native struct pointers; leave as null in managed form
                //      since we only have a single struct copy (not a linked list of handles).
                managed.Prev = null;
                managed.Next = null;

                return managed;
            }
        }

        /// <summary>
        /// Gets the virtual address of the location in the binary that this relocation patches.
        /// This is the address within the section where the fix-up is applied.
        /// </summary>
        public ulong Address
        {
            get
            {
                // Retrieve the 64-bit unsigned relocation address from the native layer.
                return NativeMethods.BNRelocationGetReloc(this.handle);
            }
        }

        /// <summary>
        /// Gets the resolved target value for this relocation.
        /// This is the value that will be written (or combined with an addend) at the relocation address.
        /// </summary>
        public ulong Target
        {
            get
            {
                // Retrieve the 64-bit unsigned target value from the native layer.
                return NativeMethods.BNRelocationGetTarget(this.handle);
            }
        }

        /// <summary>
        /// Gets the symbol associated with this relocation, if any.
        /// Returns null if the relocation does not reference a named symbol.
        /// </summary>
        public Symbol? Symbol
        {
            get
            {
                // Retrieve a borrowed reference to the associated symbol; take ownership via TakeHandle.
                return BinaryNinja.Symbol.TakeHandle(
                    NativeMethods.BNRelocationGetSymbol(this.handle)
                );
            }
        }
    }
}
