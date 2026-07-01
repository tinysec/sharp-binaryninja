using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Manages a Binary Ninja database (.bndb) file, providing access to
    /// snapshots, global key-value storage, and analysis cache.
    /// </summary>
    public sealed class Database : AbstractSafeHandle<Database>
    {
        /// <summary>
        /// Initializes a new Database wrapper around an existing native handle.
        /// </summary>
        /// <param name="handle">The native pointer to the BNDatabase object.</param>
        /// <param name="owner">True if this instance owns the handle and should free it on dispose.</param>
        internal Database(IntPtr handle, bool owner)
            : base(handle, owner)
        {
        }

        /// <summary>
        /// Creates a new managed Database by incrementing the reference count on an existing native handle.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNDatabase pointer.</param>
        /// <returns>A new Database instance, or null if handle is zero.</returns>
        internal static Database? NewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new Database(
                NativeMethods.BNNewDatabaseReference(handle),
                true
            );
        }

        /// <summary>
        /// Creates a new managed Database by incrementing the reference count. Throws if handle is zero.
        /// </summary>
        /// <param name="handle">The native BNDatabase pointer.</param>
        /// <returns>A new Database instance.</returns>
        internal static Database MustNewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new Database(
                NativeMethods.BNNewDatabaseReference(handle),
                true
            );
        }

        /// <summary>
        /// Takes ownership of an existing native handle without incrementing the reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNDatabase pointer.</param>
        /// <returns>A new Database instance, or null if handle is zero.</returns>
        internal static Database? TakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new Database(handle, true);
        }

        /// <summary>
        /// Takes ownership of an existing native handle without incrementing the reference count. Throws if zero.
        /// </summary>
        /// <param name="handle">The native BNDatabase pointer.</param>
        /// <returns>A new Database instance.</returns>
        internal static Database MustTakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new Database(handle, true);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNDatabase pointer.</param>
        /// <returns>A new Database instance that will not free the handle on dispose.</returns>
        internal static Database? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new Database(handle, false);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNDatabase pointer.</param>
        /// <returns>A new Database instance that will not free the handle on dispose.</returns>
        internal static Database MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new Database(handle, false);
        }

        /// <summary>
        /// Releases the native BNDatabase handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native database handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeDatabase(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Gets all snapshots stored in this database.
        /// </summary>
        public Snapshot[] Snapshots
        {
            get
            {
                unsafe
                {
                    // 1. Call native function with a pointer to receive the count.
                    ulong count = 0;
                    IntPtr arrayPointer = NativeMethods.BNGetDatabaseSnapshots(
                        this.handle,
                        (IntPtr)(&count)
                    );

                    // 2. Convert the native array to managed Snapshot objects and free native memory.
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
        /// Gets or sets the current (active) snapshot in this database.
        /// Setting this changes which snapshot is loaded when the database is opened.
        /// </summary>
        public Snapshot? CurrentSnapshot
        {
            get
            {
                // Retrieve a borrowed reference to the current snapshot.
                return Snapshot.TakeHandle(
                    NativeMethods.BNGetDatabaseCurrentSnapshot(this.handle)
                );
            }

            set
            {
                if (null == value)
                {
                    return;
                }

                // Update the current snapshot by its numeric ID.
                NativeMethods.BNSetDatabaseCurrentSnapshot(this.handle, value.Id);
            }
        }

        /// <summary>
        /// Gets the file metadata object associated with this database.
        /// </summary>
        public FileMetadata? File
        {
            get
            {
                // Return a borrowed reference to the file metadata backing this database.
                return FileMetadata.BorrowHandle(
                    NativeMethods.BNGetDatabaseFile(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets all global key names stored in this database.
        /// </summary>
        public string[] GlobalKeys
        {
            get
            {
                unsafe
                {
                    // 1. Retrieve the key list with a native count pointer.
                    ulong count = 0;
                    IntPtr arrayPointer = NativeMethods.BNGetDatabaseGlobalKeys(
                        this.handle,
                        (IntPtr)(&count)
                    );

                    // 2. Convert native string array to managed strings and free native memory.
                    return UnsafeUtils.TakeAnsiStringArray(
                        arrayPointer,
                        count,
                        NativeMethods.BNFreeStringList
                    );
                }
            }
        }

        /// <summary>
        /// Retrieves a snapshot by its numeric identifier.
        /// </summary>
        /// <param name="id">The snapshot ID to look up.</param>
        /// <returns>The matching Snapshot, or null if not found.</returns>
        public Snapshot? GetSnapshot(long id)
        {
            // Forward the lookup to the native API which returns a borrowed reference.
            return Snapshot.TakeHandle(
                NativeMethods.BNGetDatabaseSnapshot(this.handle, id)
            );
        }

        /// <summary>
        /// Reads a global string value stored under the given key.
        /// </summary>
        /// <param name="key">The global key to read.</param>
        /// <returns>The stored string value, or null if the key does not exist.</returns>
        public string? ReadGlobal(string key)
        {
            // 1. Retrieve the native string pointer for the key.
            IntPtr raw = NativeMethods.BNReadDatabaseGlobal(this.handle, key);

            if (IntPtr.Zero == raw)
            {
                // Key was not found; return null to signal absence.
                return null;
            }

            // 2. Copy and free the native string.
            return UnsafeUtils.TakeAnsiString(raw);
        }

        /// <summary>
        /// Writes a global string value under the given key.
        /// </summary>
        /// <param name="key">The global key to write.</param>
        /// <param name="value">The string value to store.</param>
        /// <returns>True if the write succeeded.</returns>
        public bool WriteGlobal(string key, string value)
        {
            return NativeMethods.BNWriteDatabaseGlobal(this.handle, key, value);
        }

        /// <summary>
        /// Reads a global binary data buffer stored under the given key.
        /// </summary>
        /// <param name="key">The global key to read.</param>
        /// <returns>A DataBuffer containing the stored bytes, or null if not found.</returns>
        public DataBuffer? ReadGlobalData(string key)
        {
            return DataBuffer.TakeHandle(
                NativeMethods.BNReadDatabaseGlobalData(this.handle, key)
            );
        }

        /// <summary>
        /// Writes a global binary data buffer under the given key.
        /// </summary>
        /// <param name="key">The global key to write.</param>
        /// <param name="value">The DataBuffer to store.</param>
        /// <returns>True if the write succeeded.</returns>
        public bool WriteGlobalData(string key, DataBuffer value)
        {
            return NativeMethods.BNWriteDatabaseGlobalData(
                this.handle,
                key,
                value.DangerousGetHandle()
            );
        }

        /// <summary>
        /// Checks whether a global key exists in this database.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key is present.</returns>
        public bool HasGlobal(string key)
        {
            // The native API returns a non-zero integer when the key exists.
            return 0 != NativeMethods.BNDatabaseHasGlobal(this.handle, key);
        }

        /// <summary>
        /// Reads the analysis cache key-value store from this database.
        /// </summary>
        /// <returns>A KeyValueStore with cached analysis data, or null on failure.</returns>
        public KeyValueStore? ReadAnalysisCache()
        {
            return KeyValueStore.TakeHandle(
                NativeMethods.BNReadDatabaseAnalysisCache(this.handle)
            );
        }

        /// <summary>
        /// Writes an analysis cache key-value store into this database.
        /// </summary>
        /// <param name="value">The KeyValueStore to persist.</param>
        /// <returns>True if the write succeeded.</returns>
        public bool WriteAnalysisCache(KeyValueStore value)
        {
            return NativeMethods.BNWriteDatabaseAnalysisCache(
                this.handle,
                value.DangerousGetHandle()
            );
        }

        /// <summary>
        /// Removes the snapshot with the given ID from this database.
        /// </summary>
        /// <param name="id">The numeric snapshot ID to remove.</param>
        /// <returns>True if removal succeeded.</returns>
        public bool RemoveSnapshot(long id)
        {
            return NativeMethods.BNRemoveDatabaseSnapshot(this.handle, id);
        }

        /// <summary>
        /// Trims a snapshot, removing its stored data but keeping the snapshot entry.
        /// </summary>
        /// <param name="id">The numeric snapshot ID to trim.</param>
        /// <returns>True if the trim succeeded.</returns>
        public bool TrimSnapshot(long id)
        {
            return NativeMethods.BNTrimDatabaseSnapshot(this.handle, id);
        }

        /// <summary>
        /// Reloads the database connection, refreshing the internal state from disk.
        /// </summary>
        public void ReloadConnection()
        {
            NativeMethods.BNDatabaseReloadConnection(this.handle);
        }

        /// <summary>
        /// Checks whether the snapshot with the given ID has stored data in this database.
        /// </summary>
        /// <param name="id">The numeric snapshot ID to check.</param>
        /// <returns>True if the snapshot has data; false if it has been trimmed or never had data.</returns>
        public bool SnapshotHasData(long id)
        {
            return NativeMethods.BNSnapshotHasData(this.handle , id);
        }

        /// <summary>
        /// Writes snapshot data to the database, creating a new snapshot with the specified
        /// parent lineage, binary view contents, name, and optional key-value data.
        /// </summary>
        /// <param name="parents">An array of parent snapshot IDs for lineage tracking.</param>
        /// <param name="file">The binary view to store as the snapshot contents.</param>
        /// <param name="name">A human-readable name for the snapshot.</param>
        /// <param name="data">A key-value store of additional data to persist with the snapshot.</param>
        /// <param name="autoSave">Whether this snapshot should be marked as an auto-save.</param>
        /// <returns>The ID of the newly created snapshot, or a negative value on failure.</returns>
        public unsafe long WriteSnapshotData(
            long[] parents ,
            BinaryView file ,
            string name ,
            KeyValueStore data ,
            bool autoSave
        )
        {
            fixed (long* parentsPtr = parents)
            {
                return NativeMethods.BNWriteDatabaseSnapshotData(
                    this.handle ,
                    (IntPtr)parentsPtr ,
                    (ulong)parents.Length ,
                    file.DangerousGetHandle() ,
                    name ,
                    data.DangerousGetHandle() ,
                    autoSave ,
                    IntPtr.Zero ,
                    IntPtr.Zero
                );
            }
        }
    }
}
