using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a single snapshot in a Binary Ninja database (.bndb).
    /// Each snapshot records the analysis state at a point in time and may
    /// have parent and child snapshots forming a directed acyclic graph.
    /// </summary>
    public sealed class Snapshot : AbstractSafeHandle<Snapshot>
    {
        /// <summary>
        /// Initializes a new Snapshot wrapper around an existing native handle.
        /// </summary>
        /// <param name="handle">The native pointer to the BNSnapshot object.</param>
        /// <param name="owner">True if this instance owns the handle and should free it on dispose.</param>
        internal Snapshot(IntPtr handle, bool owner)
            : base(handle, owner)
        {
        }

        /// <summary>
        /// Creates a new managed Snapshot by incrementing the reference count on an existing native handle.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNSnapshot pointer.</param>
        /// <returns>A new Snapshot instance, or null if handle is zero.</returns>
        internal static Snapshot? NewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new Snapshot(
                NativeMethods.BNNewSnapshotReference(handle),
                true
            );
        }

        /// <summary>
        /// Creates a new managed Snapshot by incrementing the reference count. Throws if handle is zero.
        /// </summary>
        /// <param name="handle">The native BNSnapshot pointer.</param>
        /// <returns>A new Snapshot instance.</returns>
        internal static Snapshot MustNewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new Snapshot(
                NativeMethods.BNNewSnapshotReference(handle),
                true
            );
        }

        /// <summary>
        /// Takes ownership of an existing native handle without incrementing the reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNSnapshot pointer.</param>
        /// <returns>A new Snapshot instance, or null if handle is zero.</returns>
        internal static Snapshot? TakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new Snapshot(handle, true);
        }

        /// <summary>
        /// Takes ownership of an existing native handle without incrementing the reference count. Throws if zero.
        /// </summary>
        /// <param name="handle">The native BNSnapshot pointer.</param>
        /// <returns>A new Snapshot instance.</returns>
        internal static Snapshot MustTakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new Snapshot(handle, true);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNSnapshot pointer.</param>
        /// <returns>A new Snapshot instance that will not free the handle on dispose.</returns>
        internal static Snapshot? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new Snapshot(handle, false);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNSnapshot pointer.</param>
        /// <returns>A new Snapshot instance that will not free the handle on dispose.</returns>
        internal static Snapshot MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new Snapshot(handle, false);
        }

        /// <summary>
        /// Releases the native BNSnapshot handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native snapshot handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeSnapshot(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Gets the numeric identifier for this snapshot.
        /// Each snapshot in a database has a unique integer ID.
        /// </summary>
        public long Id
        {
            get
            {
                // Retrieve the 64-bit signed integer ID from the native snapshot.
                return NativeMethods.BNGetSnapshotId(this.handle);
            }
        }

        /// <summary>
        /// Gets or sets the human-readable name of this snapshot.
        /// The name is a user-visible label and does not affect snapshot identity.
        /// </summary>
        public string Name
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the snapshot name.
                IntPtr raw = NativeMethods.BNGetSnapshotName(this.handle);

                // 2. Copy the native string to managed memory and free the native allocation.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }

            set
            {
                // Forward the new name to the native API; the string is copied internally.
                NativeMethods.BNSetSnapshotName(this.handle, value);
            }
        }

        /// <summary>
        /// Gets the database that owns this snapshot.
        /// </summary>
        public Database? Database
        {
            get
            {
                // Return a new owned reference to the parent database.
                return BinaryNinja.Database.NewFromHandle(
                    NativeMethods.BNGetSnapshotDatabase(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets all child snapshots of this snapshot in the snapshot graph.
        /// A snapshot may have multiple children when branches diverge.
        /// </summary>
        public Snapshot[] Children
        {
            get
            {
                unsafe
                {
                    // 1. Call native function with a pointer to receive the count.
                    ulong count = 0;
                    IntPtr arrayPointer = NativeMethods.BNGetSnapshotChildren(
                        this.handle,
                        (IntPtr)(&count)
                    );

                    // 2. Convert the native pointer array to managed Snapshot objects and free native memory.
                    return UnsafeUtils.TakeHandleArrayEx<Snapshot>(
                        arrayPointer,
                        count,
                        Snapshot.MustNewFromHandle,
                        NativeMethods.BNFreeSnapshotList
                    );
                }
            }
        }

        /// <summary>
        /// Gets all parent snapshots of this snapshot in the snapshot graph.
        /// Most snapshots have exactly one parent; the initial snapshot has none.
        /// </summary>
        public Snapshot[] Parents
        {
            get
            {
                unsafe
                {
                    // 1. Call native function with a pointer to receive the count.
                    ulong count = 0;
                    IntPtr arrayPointer = NativeMethods.BNGetSnapshotParents(
                        this.handle,
                        (IntPtr)(&count)
                    );

                    // 2. Convert the native pointer array to managed Snapshot objects and free native memory.
                    return UnsafeUtils.TakeHandleArrayEx<Snapshot>(
                        arrayPointer,
                        count,
                        Snapshot.MustNewFromHandle,
                        NativeMethods.BNFreeSnapshotList
                    );
                }
            }
        }

        /// <summary>
        /// Gets the first parent snapshot of this snapshot, or null if this is a root snapshot.
        /// Equivalent to Parents[0] but avoids allocating the full parent array.
        /// </summary>
        public Snapshot? FirstParent
        {
            get
            {
                // The native function returns a borrowed reference; take ownership via TakeHandle.
                return Snapshot.TakeHandle(
                    NativeMethods.BNGetSnapshotFirstParent(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets whether this snapshot was created automatically by Binary Ninja
        /// (for example, during auto-save) rather than by an explicit user action.
        /// </summary>
        public bool IsAutoSave
        {
            get
            {
                // Query the native auto-save flag for this snapshot.
                return NativeMethods.BNIsSnapshotAutoSave(this.handle);
            }
        }

        /// <summary>
        /// Gets whether this snapshot has analysis data stored.
        /// A snapshot may exist in the graph without storing full contents when it has been trimmed.
        /// </summary>
        public bool HasContents
        {
            get
            {
                // Check the native flag indicating stored analysis data presence.
                return NativeMethods.BNSnapshotHasContents(this.handle);
            }
        }

        /// <summary>
        /// Gets whether this snapshot has undo history data stored.
        /// Undo entries allow reverting changes that occurred since the previous snapshot.
        /// </summary>
        public bool HasUndo
        {
            get
            {
                // Check the native flag indicating stored undo data presence.
                return NativeMethods.BNSnapshotHasUndo(this.handle);
            }
        }

        /// <summary>
        /// Gets all undo entries associated with this snapshot.
        /// Each entry describes one user-visible undoable operation.
        /// </summary>
        public UndoEntry[] UndoEntries
        {
            get
            {
                unsafe
                {
                    // 1. Call native function with a pointer to receive the count.
                    ulong count = 0;
                    IntPtr arrayPointer = NativeMethods.BNGetSnapshotUndoEntries(
                        this.handle,
                        (IntPtr)(&count)
                    );

                    // 2. Convert the native pointer array to managed UndoEntry objects and free native memory.
                    return UnsafeUtils.TakeHandleArrayEx<UndoEntry>(
                        arrayPointer,
                        count,
                        UndoEntry.MustNewFromHandle,
                        NativeMethods.BNFreeUndoEntryList
                    );
                }
            }
        }

        /// <summary>
        /// Reads the analysis data stored in this snapshot as a key-value store,
        /// with optional progress reporting.
        /// Returns null if the snapshot contains no data or the read fails.
        /// </summary>
        /// <param name="progress">An optional progress delegate that receives (current, total) progress values.
        /// Pass null for no progress reporting.</param>
        /// <returns>A KeyValueStore containing the snapshot's analysis data, or null on failure.</returns>
        public KeyValueStore? ReadDataWithProgress(ProgressDelegate? progress = null)
        {
            // Delegate to the native read-with-progress function.
            return KeyValueStore.TakeHandle(
                NativeMethods.BNReadSnapshotDataWithProgress(
                    this.handle ,
                    IntPtr.Zero ,
                    null == progress
                        ? IntPtr.Zero
                        : Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(
                            UnsafeUtils.WrapProgressDelegate(progress))
                )
            );
        }

        /// <summary>
        /// Reads the analysis data stored in this snapshot as a key-value store.
        /// Returns null if the snapshot contains no data or the read fails.
        /// </summary>
        /// <returns>A KeyValueStore containing the snapshot's analysis data, or null on failure.</returns>
        public KeyValueStore? ReadData()
        {
            // Delegate to the native read function which allocates a new key-value store.
            return KeyValueStore.TakeHandle(
                NativeMethods.BNReadSnapshotData(this.handle)
            );
        }

        /// <summary>
        /// Gets the raw file contents stored in this snapshot as a data buffer.
        /// Returns null if the snapshot contains no file data.
        /// </summary>
        /// <returns>A DataBuffer with the raw file bytes, or null if unavailable.</returns>
        public DataBuffer? FileContents
        {
            get
            {
                // Retrieve the file contents buffer owned by the native layer.
                return DataBuffer.TakeHandle(
                    NativeMethods.BNGetSnapshotFileContents(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets a hash of the file contents stored in this snapshot.
        /// Useful for comparing snapshots without loading full data.
        /// </summary>
        /// <returns>A DataBuffer containing the hash bytes, or null if unavailable.</returns>
        public DataBuffer? FileContentsHash
        {
            get
            {
                // Retrieve the hash buffer; caller owns the returned allocation.
                return DataBuffer.TakeHandle(
                    NativeMethods.BNGetSnapshotFileContentsHash(this.handle)
                );
            }
        }

        /// <summary>
        /// Checks whether the given snapshot is an ancestor of this snapshot in the graph.
        /// An ancestor is any snapshot reachable by following parent links.
        /// </summary>
        /// <param name="other">The candidate ancestor snapshot to test.</param>
        /// <returns>True if other is an ancestor of this snapshot.</returns>
        public bool HasAncestor(Snapshot other)
        {
            // Forward both handles to the native ancestry check function.
            return NativeMethods.BNSnapshotHasAncestor(
                this.handle,
                other.DangerousGetHandle()
            );
        }

        /// <summary>
        /// Persists the given key-value store as the analysis data for this snapshot.
        /// </summary>
        /// <param name="data">The KeyValueStore to write into this snapshot.</param>
        /// <returns>True if the store operation succeeded.</returns>
        public bool StoreData(KeyValueStore data)
        {
            // Call the native store function passing IntPtr.Zero for ctxt and progress (unused).
            return NativeMethods.BNSnapshotStoreData(
                this.handle,
                data.DangerousGetHandle(),
                IntPtr.Zero,
                IntPtr.Zero
            );
        }

        /// <summary>
        /// Gets all undo entries associated with this snapshot, with progress reporting.
        /// Each entry describes one user-visible undoable operation.
        /// </summary>
        /// <returns>An array of undo entries for this snapshot.</returns>
        public unsafe UndoEntry[] GetUndoEntriesWithProgress()
        {
            // 1. Stack-allocate the count variable and retrieve the native undo entry array.
            ulong count = 0;

            IntPtr arrayPointer = NativeMethods.BNGetSnapshotUndoEntriesWithProgress(
                this.handle ,
                IntPtr.Zero ,
                IntPtr.Zero ,
                (IntPtr)(&count)
            );

            // 2. Convert the native pointer array to managed UndoEntry objects and free native memory.
            return UnsafeUtils.TakeHandleArrayEx<UndoEntry>(
                arrayPointer ,
                count ,
                UndoEntry.MustNewFromHandle ,
                NativeMethods.BNFreeUndoEntryList
            );
        }

        /// <summary>
        /// Gets the undo data associated with this snapshot as a raw DataBuffer.
        /// </summary>
        /// <returns>A DataBuffer containing the undo data, or null if no undo data is available.</returns>
        public DataBuffer? GetUndoData()
        {
            return DataBuffer.TakeHandle(
                NativeMethods.BNGetSnapshotUndoData(this.handle)
            );
        }
    }
}
