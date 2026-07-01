using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a relocation handler that applies architecture-specific relocations
    /// during binary loading. Relocation handlers are typically registered by architecture
    /// plugins to translate raw relocation entries into concrete address patches.
    /// </summary>
    public sealed class RelocationHandler : AbstractSafeHandle<RelocationHandler>
    {
        /// <summary>
        /// Initializes a new RelocationHandler wrapper around an existing native handle.
        /// </summary>
        /// <param name="handle">The native pointer to the BNRelocationHandler object.</param>
        /// <param name="owner">True if this wrapper owns the handle and should free it on dispose.</param>
        internal RelocationHandler(IntPtr handle, bool owner)
            : base(handle, owner)
        {
        }

        /// <summary>
        /// Creates an owned reference by incrementing the native reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRelocationHandler pointer.</param>
        /// <returns>A new owned RelocationHandler, or null if the handle is zero.</returns>
        internal static RelocationHandler? NewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new RelocationHandler(
                NativeMethods.BNNewRelocationHandlerReference(handle),
                true
            );
        }

        /// <summary>
        /// Creates an owned reference by incrementing the native reference count.
        /// Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRelocationHandler pointer.</param>
        /// <returns>A new owned RelocationHandler.</returns>
        internal static RelocationHandler MustNewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new RelocationHandler(
                NativeMethods.BNNewRelocationHandlerReference(handle),
                true
            );
        }

        /// <summary>
        /// Takes ownership of an existing handle without incrementing the reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRelocationHandler pointer.</param>
        /// <returns>A new owned RelocationHandler, or null if the handle is zero.</returns>
        internal static RelocationHandler? TakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new RelocationHandler(handle, true);
        }

        /// <summary>
        /// Takes ownership of an existing handle without incrementing the reference count.
        /// Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRelocationHandler pointer.</param>
        /// <returns>A new owned RelocationHandler.</returns>
        internal static RelocationHandler MustTakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new RelocationHandler(handle, true);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRelocationHandler pointer.</param>
        /// <returns>A new RelocationHandler that will not free the handle on dispose.</returns>
        internal static RelocationHandler? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new RelocationHandler(handle, false);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRelocationHandler pointer.</param>
        /// <returns>A new RelocationHandler that will not free the handle on dispose.</returns>
        internal static RelocationHandler MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new RelocationHandler(handle, false);
        }

        /// <summary>
        /// Releases the native BNRelocationHandler handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native handler handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeRelocationHandler(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Applies a relocation to a raw byte buffer at the location described by the relocation entry.
        /// </summary>
        /// <param name="view">The binary view providing context for the relocation.</param>
        /// <param name="arch">The architecture defining the relocation format.</param>
        /// <param name="reloc">The relocation entry describing what to patch and where.</param>
        /// <param name="dest">Pointer to the destination byte buffer to be patched.</param>
        /// <param name="len">Length in bytes of the destination buffer.</param>
        /// <returns>True if the relocation was applied successfully; false on failure.</returns>
        public bool ApplyRelocation(BinaryView view, Architecture arch, Relocation reloc, IntPtr dest, ulong len)
        {
            // 1. Validate required parameters before forwarding to the native API.
            if (null == view)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (null == arch)
            {
                throw new ArgumentNullException(nameof(arch));
            }

            if (null == reloc)
            {
                throw new ArgumentNullException(nameof(reloc));
            }

            // 2. Delegate to the native relocation application API.
            return NativeMethods.BNRelocationHandlerApplyRelocation(
                this.handle,
                view.DangerousGetHandle(),
                arch.DangerousGetHandle(),
                reloc.DangerousGetHandle(),
                dest,
                len
            );
        }

        /// <summary>
        /// Applies a relocation using this handler's default implementation,
        /// bypassing any custom override. Useful when a custom handler wants to
        /// fall back to the base behavior for specific relocation types.
        /// </summary>
        /// <param name="view">The binary view providing context for the relocation.</param>
        /// <param name="arch">The architecture defining the relocation format.</param>
        /// <param name="reloc">The relocation entry describing what to patch and where.</param>
        /// <param name="dest">Pointer to the destination byte buffer to be patched.</param>
        /// <param name="len">Length in bytes of the destination buffer.</param>
        /// <returns>True if the relocation was applied successfully; false on failure.</returns>
        public bool DefaultApplyRelocation(BinaryView view, Architecture arch, Relocation reloc, IntPtr dest, ulong len)
        {
            // 1. Validate required parameters before forwarding to the native API.
            if (null == view)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (null == arch)
            {
                throw new ArgumentNullException(nameof(arch));
            }

            if (null == reloc)
            {
                throw new ArgumentNullException(nameof(reloc));
            }

            // 2. Delegate to the native default relocation application API.
            return NativeMethods.BNRelocationHandlerDefaultApplyRelocation(
                this.handle,
                view.DangerousGetHandle(),
                arch.DangerousGetHandle(),
                reloc.DangerousGetHandle(),
                dest,
                len
            );
        }

        /// <summary>
        /// Computes the operand value for an external relocation at the given address.
        /// This is used during lifting to represent the relocated target as an IL operand.
        /// </summary>
        /// <param name="data">Pointer to the raw bytes at the relocation site.</param>
        /// <param name="addr">The virtual address of the relocation site.</param>
        /// <param name="length">Number of bytes at the relocation site.</param>
        /// <param name="il">The low-level IL function being lifted into.</param>
        /// <param name="relocation">The relocation entry describing the external reference.</param>
        /// <returns>The computed operand value for use in the IL expression.</returns>
        public ulong GetOperandForExternalRelocation(IntPtr data, ulong addr, ulong length, LowLevelILFunction il, Relocation relocation)
        {
            // 1. Validate the required handle parameters.
            if (null == il)
            {
                throw new ArgumentNullException(nameof(il));
            }

            if (null == relocation)
            {
                throw new ArgumentNullException(nameof(relocation));
            }

            // 2. Forward all parameters to the native operand computation API.
            return NativeMethods.BNRelocationHandlerGetOperandForExternalRelocation(
                this.handle,
                data,
                addr,
                length,
                il.DangerousGetHandle(),
                relocation.DangerousGetHandle()
            );
        }
    }
}
