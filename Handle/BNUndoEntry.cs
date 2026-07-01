using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a single entry in Binary Ninja's undo history.
    /// An undo entry records one user-visible operation that can be reversed.
    /// It carries a unique identifier, the Unix timestamp when it was created,
    /// and the list of atomic actions that make up the operation.
    /// </summary>
    public sealed class UndoEntry : AbstractSafeHandle<UndoEntry>
    {
        /// <summary>
        /// Initializes a new UndoEntry wrapper around an existing native handle.
        /// </summary>
        /// <param name="handle">The native pointer to the BNUndoEntry object.</param>
        /// <param name="owner">True if this instance owns the handle and should free it on dispose.</param>
        internal UndoEntry(IntPtr handle, bool owner)
            : base(handle, owner)
        {
        }

        /// <summary>
        /// Creates a new managed UndoEntry by incrementing the reference count on an existing native handle.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNUndoEntry pointer.</param>
        /// <returns>A new UndoEntry instance, or null if handle is zero.</returns>
        internal static UndoEntry? NewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new UndoEntry(
                NativeMethods.BNNewUndoEntryReference(handle),
                true
            );
        }

        /// <summary>
        /// Creates a new managed UndoEntry by incrementing the reference count. Throws if handle is zero.
        /// </summary>
        /// <param name="handle">The native BNUndoEntry pointer.</param>
        /// <returns>A new UndoEntry instance.</returns>
        internal static UndoEntry MustNewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new UndoEntry(
                NativeMethods.BNNewUndoEntryReference(handle),
                true
            );
        }

        /// <summary>
        /// Takes ownership of an existing native handle without incrementing the reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNUndoEntry pointer.</param>
        /// <returns>A new UndoEntry instance, or null if handle is zero.</returns>
        internal static UndoEntry? TakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new UndoEntry(handle, true);
        }

        /// <summary>
        /// Takes ownership of an existing native handle without incrementing the reference count. Throws if zero.
        /// </summary>
        /// <param name="handle">The native BNUndoEntry pointer.</param>
        /// <returns>A new UndoEntry instance.</returns>
        internal static UndoEntry MustTakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new UndoEntry(handle, true);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNUndoEntry pointer.</param>
        /// <returns>A new UndoEntry instance that will not free the handle on dispose.</returns>
        internal static UndoEntry? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new UndoEntry(handle, false);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNUndoEntry pointer.</param>
        /// <returns>A new UndoEntry instance that will not free the handle on dispose.</returns>
        internal static UndoEntry MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new UndoEntry(handle, false);
        }

        /// <summary>
        /// Releases the native BNUndoEntry handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native undo entry handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeUndoEntry(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Gets the unique string identifier for this undo entry.
        /// The ID is stable and can be used to correlate entries across sessions.
        /// </summary>
        public string Id
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the entry ID.
                IntPtr raw = NativeMethods.BNUndoEntryGetId(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the Unix timestamp (seconds since epoch) when this undo entry was recorded.
        /// </summary>
        public ulong Timestamp
        {
            get
            {
                // Retrieve the 64-bit unsigned Unix timestamp from the native entry.
                return NativeMethods.BNUndoEntryGetTimestamp(this.handle);
            }
        }

        /// <summary>
        /// Gets all atomic undo actions that make up this undo entry.
        /// Each action corresponds to one modification within the larger operation.
        /// </summary>
        public UndoAction[] Actions
        {
            get
            {
                // 1. Retrieve the native action array with an out count parameter.
                IntPtr arrayPointer = NativeMethods.BNUndoEntryGetActions(
                    this.handle,
                    out ulong arrayLength
                );

                // 2. Convert the native pointer array to managed UndoAction objects and free native memory.
                return UnsafeUtils.TakeHandleArrayEx<UndoAction>(
                    arrayPointer,
                    arrayLength,
                    UndoAction.MustNewFromHandle,
                    NativeMethods.BNFreeUndoActionList
                );
            }
        }
    }
}
