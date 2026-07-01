using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a Binary Ninja type archive — a persistent store of named types
    /// that can be shared across multiple binary views and kept in sync via snapshots.
    /// Each archive is backed by a file on disk and exposes a versioned snapshot graph.
    /// </summary>
    public sealed class TypeArchive : AbstractSafeHandle<TypeArchive>
    {
        /// <summary>
        /// Initializes a new TypeArchive wrapper around an existing native handle.
        /// </summary>
        /// <param name="handle">The native pointer to the BNTypeArchive object.</param>
        /// <param name="owner">True if this wrapper owns the handle and should free it on dispose.</param>
        internal TypeArchive(IntPtr handle, bool owner)
            : base(handle, owner)
        {
        }

        /// <summary>
        /// Creates an owned reference by incrementing the native reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNTypeArchive pointer.</param>
        /// <returns>A new owned TypeArchive, or null if the handle is zero.</returns>
        internal static TypeArchive? NewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new TypeArchive(
                NativeMethods.BNNewTypeArchiveReference(handle),
                true
            );
        }

        /// <summary>
        /// Creates an owned reference by incrementing the native reference count.
        /// Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNTypeArchive pointer.</param>
        /// <returns>A new owned TypeArchive.</returns>
        internal static TypeArchive MustNewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new TypeArchive(
                NativeMethods.BNNewTypeArchiveReference(handle),
                true
            );
        }

        /// <summary>
        /// Takes ownership of an existing handle without incrementing the reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNTypeArchive pointer.</param>
        /// <returns>A new owned TypeArchive, or null if the handle is zero.</returns>
        internal static TypeArchive? TakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new TypeArchive(handle, true);
        }

        /// <summary>
        /// Takes ownership of an existing handle without incrementing the reference count.
        /// Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNTypeArchive pointer.</param>
        /// <returns>A new owned TypeArchive.</returns>
        internal static TypeArchive MustTakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new TypeArchive(handle, true);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNTypeArchive pointer.</param>
        /// <returns>A new TypeArchive that will not free the handle on dispose.</returns>
        internal static TypeArchive? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new TypeArchive(handle, false);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNTypeArchive pointer.</param>
        /// <returns>A new TypeArchive that will not free the handle on dispose.</returns>
        internal static TypeArchive MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new TypeArchive(handle, false);
        }

        /// <summary>
        /// Releases the native BNTypeArchive handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native archive reference and mark it invalid to prevent double-free.
                NativeMethods.BNFreeTypeArchiveReference(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        // ─── Static factories ────────────────────────────────────────────────────

        /// <summary>
        /// Creates a new type archive on disk at the given path using the specified platform.
        /// The caller is responsible for disposing the returned instance.
        /// Returns null if creation fails.
        /// </summary>
        /// <param name="path">The file path at which to create the archive.</param>
        /// <param name="platform">The platform whose types this archive will store.</param>
        /// <returns>A new owned TypeArchive, or null on failure.</returns>
        public static TypeArchive? Create(string path, Platform platform)
        {
            // 1. Validate required parameters.
            if (null == platform)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            // 2. Create the archive on disk; the returned handle is owned.
            return TypeArchive.TakeHandle(
                NativeMethods.BNCreateTypeArchive(
                    path ?? string.Empty,
                    platform.DangerousGetHandle()
                )
            );
        }

        /// <summary>
        /// Creates a new type archive on disk at the given path using the specified platform
        /// and a caller-provided unique identifier.
        /// The caller is responsible for disposing the returned instance.
        /// Returns null if creation fails.
        /// </summary>
        /// <param name="path">The file path at which to create the archive.</param>
        /// <param name="platform">The platform whose types this archive will store.</param>
        /// <param name="id">The unique identifier to assign to the archive.</param>
        /// <returns>A new owned TypeArchive, or null on failure.</returns>
        public static TypeArchive? CreateWithId(string path, Platform platform, string id)
        {
            // 1. Validate required parameters.
            if (null == platform)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            // 2. Create the archive on disk with the given ID; the returned handle is owned.
            return TypeArchive.TakeHandle(
                NativeMethods.BNCreateTypeArchiveWithId(
                    path ?? string.Empty,
                    platform.DangerousGetHandle(),
                    id ?? string.Empty
                )
            );
        }

        /// <summary>
        /// Opens an existing type archive from the given file path.
        /// Returns null if the file does not exist or cannot be opened.
        /// </summary>
        /// <param name="path">The file path of the archive to open.</param>
        /// <returns>A new owned TypeArchive, or null on failure.</returns>
        public static TypeArchive? Open(string path)
        {
            // Open the archive from disk; the returned handle is owned.
            return TypeArchive.TakeHandle(
                NativeMethods.BNOpenTypeArchive(path ?? string.Empty)
            );
        }

        /// <summary>
        /// Looks up an already-open type archive by its unique identifier.
        /// Returns null if no archive with the given ID is currently open.
        /// </summary>
        /// <param name="id">The unique identifier of the archive to find.</param>
        /// <returns>An addref'd TypeArchive, or null if not found.</returns>
        public static TypeArchive? LookupById(string id)
        {
            // Retrieve an existing open archive by ID; addref the handle.
            return TypeArchive.NewFromHandle(
                NativeMethods.BNLookupTypeArchiveById(id ?? string.Empty)
            );
        }

        // ─── Properties ──────────────────────────────────────────────────────────

        /// <summary>
        /// Gets the filesystem path of this type archive.
        /// </summary>
        public string Path
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the archive path.
                IntPtr raw = NativeMethods.BNGetTypeArchivePath(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the unique identifier string assigned to this type archive.
        /// </summary>
        public string Id
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the archive ID.
                IntPtr raw = NativeMethods.BNGetTypeArchiveId(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the platform associated with this type archive.
        /// Returns null if the native engine cannot resolve the platform.
        /// </summary>
        public Platform? Platform
        {
            get
            {
                // Retrieve a new reference to the platform for this archive.
                return Platform.TakeHandle(
                    NativeMethods.BNGetTypeArchivePlatform(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets or sets the current snapshot ID that is used for type lookups.
        /// Setting this value changes which snapshot's types are visible by default.
        /// </summary>
        public string CurrentSnapshotId
        {
            get
            {
                // Retrieve the ANSI string for the currently active snapshot ID.
                IntPtr raw = NativeMethods.BNGetTypeArchiveCurrentSnapshotId(this.handle);

                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }

            set
            {
                // Apply the given snapshot ID as the current snapshot in the archive.
                NativeMethods.BNSetTypeArchiveCurrentSnapshot(
                    this.handle,
                    value ?? string.Empty
                );
            }
        }

        // ─── Snapshot graph ──────────────────────────────────────────────────────

        /// <summary>
        /// Returns all snapshot IDs present in this archive's snapshot graph.
        /// </summary>
        /// <returns>An array of snapshot ID strings.</returns>
        public unsafe string[] GetAllSnapshotIds()
        {
            // 1. Stack-allocate the count variable and retrieve the native string array.
            ulong count = 0;
            IntPtr ptr = NativeMethods.BNGetTypeArchiveAllSnapshotIds(
                this.handle,
                (IntPtr)(&count)
            );

            // 2. Convert the native char** to managed strings and free the native array.
            return UnsafeUtils.TakeAnsiStringArray(ptr, count, NativeMethods.BNFreeStringList);
        }

        /// <summary>
        /// Returns the IDs of all parent snapshots for the snapshot with the given ID.
        /// </summary>
        /// <param name="id">The snapshot ID whose parents to retrieve.</param>
        /// <returns>An array of parent snapshot ID strings.</returns>
        public unsafe string[] GetSnapshotParentIds(string id)
        {
            // 1. Stack-allocate the count variable and retrieve the native parent ID array.
            ulong count = 0;
            IntPtr ptr = NativeMethods.BNGetTypeArchiveSnapshotParentIds(
                this.handle,
                id ?? string.Empty,
                (IntPtr)(&count)
            );

            // 2. Convert and free the native string array.
            return UnsafeUtils.TakeAnsiStringArray(ptr, count, NativeMethods.BNFreeStringList);
        }

        /// <summary>
        /// Returns the IDs of all child snapshots for the snapshot with the given ID.
        /// </summary>
        /// <param name="id">The snapshot ID whose children to retrieve.</param>
        /// <returns>An array of child snapshot ID strings.</returns>
        public unsafe string[] GetSnapshotChildIds(string id)
        {
            // 1. Stack-allocate the count variable and retrieve the native child ID array.
            ulong count = 0;
            IntPtr ptr = NativeMethods.BNGetTypeArchiveSnapshotChildIds(
                this.handle,
                id ?? string.Empty,
                (IntPtr)(&count)
            );

            // 2. Convert and free the native string array.
            return UnsafeUtils.TakeAnsiStringArray(ptr, count, NativeMethods.BNFreeStringList);
        }

        // ─── Type container ──────────────────────────────────────────────────────

        /// <summary>
        /// Gets the type container view for this archive, which provides a unified
        /// type-manipulation API over the archive's types.
        /// Returns null if the native engine cannot provide a container.
        /// </summary>
        public TypeContainer? GetTypeContainer()
        {
            // Retrieve the type container for this archive; the container is owned.
            return TypeContainer.TakeHandle(
                NativeMethods.BNGetTypeArchiveTypeContainer(this.handle)
            );
        }

        // ─── Type mutations ──────────────────────────────────────────────────────

        /// <summary>
        /// Deletes the type with the given ID from this archive.
        /// </summary>
        /// <param name="id">The unique ID of the type to delete.</param>
        /// <returns>True if the type was deleted successfully; false otherwise.</returns>
        public bool DeleteType(string id)
        {
            // Forward the type ID to the native delete API.
            return NativeMethods.BNDeleteTypeArchiveType(
                this.handle,
                id ?? string.Empty
            );
        }

        /// <summary>
        /// Renames the type with the given ID to a new qualified name in this archive.
        /// </summary>
        /// <param name="id">The unique ID of the type to rename.</param>
        /// <param name="newName">The new qualified name to assign to the type.</param>
        /// <returns>True if the rename succeeded; false otherwise.</returns>
        public bool RenameType(string id, QualifiedName newName)
        {
            // 1. Marshal the qualified name to a native struct on the heap.
            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                // 2. Forward the rename request to the native API.
                return NativeMethods.BNRenameTypeArchiveType(
                    this.handle,
                    id ?? string.Empty,
                    allocator.AllocStruct<BNQualifiedName>(newName.ToNativeEx(allocator))
                );
            }
        }

        // ─── Type lookups ────────────────────────────────────────────────────────

        /// <summary>
        /// Retrieves the type with the given ID as of the specified snapshot.
        /// Returns null if no type with that ID exists in the snapshot.
        /// </summary>
        /// <param name="id">The unique ID of the type to look up.</param>
        /// <param name="snapshot">The snapshot ID to look up the type in.</param>
        /// <returns>A new owned Type, or null if not found.</returns>
        public BinaryNinja.Type? GetTypeById(string id, string snapshot)
        {
            // Forward the type ID and snapshot to the native lookup API.
            return BinaryNinja.Type.NewFromHandle(
                NativeMethods.BNGetTypeArchiveTypeById(
                    this.handle,
                    id ?? string.Empty,
                    snapshot ?? string.Empty
                )
            );
        }

        /// <summary>
        /// Retrieves all type IDs present in the archive as of the specified snapshot.
        /// </summary>
        /// <param name="snapshot">The snapshot ID to query.</param>
        /// <returns>An array of type ID strings.</returns>
        public unsafe string[] GetTypeIds(string snapshot)
        {
            // 1. Stack-allocate the count variable and retrieve the native ID array.
            ulong count = 0;
            IntPtr ptr = NativeMethods.BNGetTypeArchiveTypeIds(
                this.handle,
                snapshot ?? string.Empty,
                (IntPtr)(&count)
            );

            // 2. Convert and free the native string array.
            return UnsafeUtils.TakeAnsiStringArray(ptr, count, NativeMethods.BNFreeStringList);
        }

        // ─── Type reference queries ───────────────────────────────────────────────

        /// <summary>
        /// Returns the IDs of all types that directly reference the type with the given ID,
        /// as of the specified snapshot.
        /// </summary>
        /// <param name="id">The type ID to find incoming direct references for.</param>
        /// <param name="snapshot">The snapshot ID to query.</param>
        /// <returns>An array of type ID strings that directly reference the given type.</returns>
        public unsafe string[] GetIncomingDirectTypeReferences(string id, string snapshot)
        {
            // 1. Stack-allocate the count and retrieve the native reference ID array.
            ulong count = 0;
            IntPtr ptr = NativeMethods.BNGetTypeArchiveIncomingDirectTypeReferences(
                this.handle,
                id ?? string.Empty,
                snapshot ?? string.Empty,
                (IntPtr)(&count)
            );

            // 2. Convert and free the native string array.
            return UnsafeUtils.TakeAnsiStringArray(ptr, count, NativeMethods.BNFreeStringList);
        }

        /// <summary>
        /// Returns the IDs of all types that recursively reference the type with the given ID,
        /// as of the specified snapshot.
        /// </summary>
        /// <param name="id">The type ID to find incoming recursive references for.</param>
        /// <param name="snapshot">The snapshot ID to query.</param>
        /// <returns>An array of type ID strings that recursively reference the given type.</returns>
        public unsafe string[] GetIncomingRecursiveTypeReferences(string id, string snapshot)
        {
            // 1. Stack-allocate the count and retrieve the native recursive reference ID array.
            ulong count = 0;
            IntPtr ptr = NativeMethods.BNGetTypeArchiveIncomingRecursiveTypeReferences(
                this.handle,
                id ?? string.Empty,
                snapshot ?? string.Empty,
                (IntPtr)(&count)
            );

            // 2. Convert and free the native string array.
            return UnsafeUtils.TakeAnsiStringArray(ptr, count, NativeMethods.BNFreeStringList);
        }

        /// <summary>
        /// Returns the IDs of all types that are directly referenced by the type with the given ID,
        /// as of the specified snapshot.
        /// </summary>
        /// <param name="id">The type ID whose outgoing direct references to retrieve.</param>
        /// <param name="snapshot">The snapshot ID to query.</param>
        /// <returns>An array of type ID strings directly referenced by the given type.</returns>
        public unsafe string[] GetOutgoingDirectTypeReferences(string id, string snapshot)
        {
            // 1. Stack-allocate the count and retrieve the native outgoing reference ID array.
            ulong count = 0;
            IntPtr ptr = NativeMethods.BNGetTypeArchiveOutgoingDirectTypeReferences(
                this.handle,
                id ?? string.Empty,
                snapshot ?? string.Empty,
                (IntPtr)(&count)
            );

            // 2. Convert and free the native string array.
            return UnsafeUtils.TakeAnsiStringArray(ptr, count, NativeMethods.BNFreeStringList);
        }

        /// <summary>
        /// Returns the IDs of all types recursively referenced by the type with the given ID,
        /// as of the specified snapshot.
        /// </summary>
        /// <param name="id">The type ID whose outgoing recursive references to retrieve.</param>
        /// <param name="snapshot">The snapshot ID to query.</param>
        /// <returns>An array of type ID strings recursively referenced by the given type.</returns>
        public unsafe string[] GetOutgoingRecursiveTypeReferences(string id, string snapshot)
        {
            // 1. Stack-allocate the count and retrieve the native recursive outgoing reference ID array.
            ulong count = 0;
            IntPtr ptr = NativeMethods.BNGetTypeArchiveOutgoingRecursiveTypeReferences(
                this.handle,
                id ?? string.Empty,
                snapshot ?? string.Empty,
                (IntPtr)(&count)
            );

            // 2. Convert and free the native string array.
            return UnsafeUtils.TakeAnsiStringArray(ptr, count, NativeMethods.BNFreeStringList);
        }

        // ─── Snapshot serialization ───────────────────────────────────────────────

        /// <summary>
        /// Serializes the snapshot with the given ID to a binary data buffer.
        /// </summary>
        /// <param name="snapshot">The snapshot ID to serialize.</param>
        /// <returns>A new owned DataBuffer containing the serialized snapshot, or null on failure.</returns>
        public DataBuffer? SerializeSnapshot(string snapshot)
        {
            // Serialize the snapshot to a native DataBuffer; the handle is owned.
            return DataBuffer.TakeHandle(
                NativeMethods.BNTypeArchiveSerializeSnapshot(
                    this.handle,
                    snapshot ?? string.Empty
                )
            );
        }

        /// <summary>
        /// Deserializes a snapshot from the given binary data buffer and inserts it into this archive.
        /// Returns the ID of the newly created snapshot, or null on failure.
        /// </summary>
        /// <param name="buffer">The DataBuffer containing the serialized snapshot bytes.</param>
        /// <returns>The new snapshot's ID string, or null on failure.</returns>
        public string? DeserializeSnapshot(DataBuffer buffer)
        {
            // 1. Validate the required buffer parameter.
            if (null == buffer)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            // 2. Deserialize and retrieve the new snapshot ID string.
            IntPtr raw = NativeMethods.BNTypeArchiveDeserializeSnapshot(
                this.handle,
                buffer.DangerousGetHandle()
            );

            // 3. Return null if deserialization failed, otherwise copy and free the ID string.
            if (raw == IntPtr.Zero)
            {
                return null;
            }

            return UnsafeUtils.TakeAnsiString(raw);
        }

        // ─── Metadata ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Retrieves the metadata value stored under the given key in this archive.
        /// Returns null if no value exists for the given key.
        /// </summary>
        /// <param name="key">The metadata key to look up.</param>
        /// <returns>A new owned Metadata value, or null if the key is not set.</returns>
        public Metadata? QueryMetadata(string key)
        {
            // Query and return the metadata for the given key; null if not found.
            return Metadata.NewFromHandle(
                NativeMethods.BNTypeArchiveQueryMetadata(
                    this.handle,
                    key ?? string.Empty
                )
            );
        }

        /// <summary>
        /// Stores a metadata value under the given key in this archive.
        /// </summary>
        /// <param name="key">The metadata key to store the value under.</param>
        /// <param name="value">The metadata value to store.</param>
        /// <returns>True if the value was stored successfully; false otherwise.</returns>
        public bool StoreMetadata(string key, Metadata value)
        {
            // 1. Validate the required metadata value parameter.
            if (null == value)
            {
                throw new ArgumentNullException(nameof(value));
            }

            // 2. Forward the key and value to the native store API.
            return NativeMethods.BNTypeArchiveStoreMetadata(
                this.handle,
                key ?? string.Empty,
                value.DangerousGetHandle()
            );
        }

        /// <summary>
        /// Removes the metadata value stored under the given key from this archive.
        /// </summary>
        /// <param name="key">The metadata key whose value to remove.</param>
        /// <returns>True if the value was removed successfully; false otherwise.</returns>
        public bool RemoveMetadata(string key)
        {
            // Delegate to the native remove API with the given key.
            return NativeMethods.BNTypeArchiveRemoveMetadata(
                this.handle,
                key ?? string.Empty
            );
        }

        // ─── Type lookups by name ────────────────────────────────────────────────

        /// <summary>
        /// Retrieves the type with the given qualified name as of the specified snapshot.
        /// Returns null if no type with that name exists in the snapshot.
        /// </summary>
        /// <param name="name">The qualified name of the type to look up.</param>
        /// <param name="snapshot">The snapshot ID to look up the type in. Empty string uses the current snapshot.</param>
        /// <returns>A new owned Type, or null if not found.</returns>
        public BinaryNinja.Type? GetTypeByName(QualifiedName name, string snapshot = "")
        {
            // 1. Marshal the qualified name to a native struct on the heap.
            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                // 2. Forward the name and snapshot to the native lookup API.
                return BinaryNinja.Type.TakeHandle(
                    NativeMethods.BNGetTypeArchiveTypeByName(
                        this.handle,
                        allocator.AllocStruct<BNQualifiedName>(name.ToNativeEx(allocator)),
                        snapshot ?? string.Empty
                    )
                );
            }
        }

        /// <summary>
        /// Retrieves the type ID for the type with the given qualified name
        /// as of the specified snapshot.
        /// </summary>
        /// <param name="name">The qualified name of the type whose ID to look up.</param>
        /// <param name="snapshot">The snapshot ID to look up the type in. Empty string uses the current snapshot.</param>
        /// <returns>The type ID string, or empty string if not found.</returns>
        public string GetTypeId(QualifiedName name, string snapshot = "")
        {
            // 1. Marshal the qualified name to a native struct on the heap.
            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                // 2. Forward the name and snapshot to the native lookup API.
                return UnsafeUtils.TakeAnsiString(
                    NativeMethods.BNGetTypeArchiveTypeId(
                        this.handle,
                        allocator.AllocStruct<BNQualifiedName>(name.ToNativeEx(allocator)),
                        snapshot ?? string.Empty
                    )
                );
            }
        }

        /// <summary>
        /// Retrieves the qualified name for the type with the given ID
        /// as of the specified snapshot.
        /// </summary>
        /// <param name="id">The unique ID of the type whose name to look up.</param>
        /// <param name="snapshot">The snapshot ID to query. Empty string uses the current snapshot.</param>
        /// <returns>The qualified name of the type.</returns>
        public QualifiedName GetTypeName(string id, string snapshot = "")
        {
            // Retrieve the native BNQualifiedName struct and convert to managed.
            return QualifiedName.TakeNative(
                NativeMethods.BNGetTypeArchiveTypeName(
                    this.handle,
                    id ?? string.Empty,
                    snapshot ?? string.Empty
                )
            );
        }

        /// <summary>
        /// Retrieves all type names and their corresponding IDs from the archive
        /// as of the specified snapshot.
        /// </summary>
        /// <param name="snapshot">The snapshot ID to query. Empty string uses the current snapshot.</param>
        /// <returns>An array of QualifiedNameAndId pairs, or an empty array on failure.</returns>
        public unsafe QualifiedNameAndId[] GetTypeNamesAndIds(string snapshot = "")
        {
            // 1. Stack-allocate the output variables for the native call.
            IntPtr namesPointer = IntPtr.Zero;
            IntPtr idsPointer = IntPtr.Zero;
            ulong count = 0;

            // 2. Call the native API to retrieve names and IDs.
            bool ok = NativeMethods.BNGetTypeArchiveTypeNamesAndIds(
                this.handle,
                snapshot ?? string.Empty,
                (IntPtr)(&namesPointer),
                (IntPtr)(&idsPointer),
                (IntPtr)(&count)
            );

            // 3. If the call failed, return an empty array.
            if (!ok)
            {
                return Array.Empty<QualifiedNameAndId>();
            }

            // 4. Convert the native qualified name array to managed QualifiedName objects.
            QualifiedName[] names = UnsafeUtils.TakeStructArrayEx<BNQualifiedName , QualifiedName>(
                namesPointer ,
                count ,
                QualifiedName.FromNative ,
                NativeMethods.BNFreeTypeNameList
            );

            // 5. Convert the native string array to managed strings.
            string[] ids = UnsafeUtils.TakeAnsiStringArray(
                idsPointer ,
                count ,
                NativeMethods.BNFreeStringList
            );

            // 6. Combine names and IDs into QualifiedNameAndId pairs.
            List<QualifiedNameAndId> results = new List<QualifiedNameAndId>();

            for (ulong i = 0; i < count; i++)
            {
                results.Add(new QualifiedNameAndId(names[i], ids[i]));
            }

            return results.ToArray();
        }

        // ─── Close ────────────────────────────────────────────────────────────────

        /// <summary>
        /// Closes this type archive, releasing its internal resources without freeing the handle.
        /// After calling this method the archive is no longer valid for operations.
        /// </summary>
        public new void Close()
        {
            // Delegate to the native close API; does not free the handle (ReleaseHandle does that).
            NativeMethods.BNCloseTypeArchive(this.handle);
        }

        /// <summary>
        /// Adds an array of types to this type archive.
        /// Each element carries a qualified name, a type ID, and the type itself.
        /// </summary>
        /// <param name="types">The array of qualified-name-type-and-id triples to add.</param>
        /// <returns>True if the types were added successfully; false otherwise.</returns>
        public unsafe bool AddTypes(QualifiedNameTypeAndId[] types)
        {
            // 1. Validate the required parameter.
            if (null == types || 0 == types.Length)
            {
                return false;
            }

            // 2. Marshal the managed array into a contiguous native struct array.
            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                // 2.1 Convert each managed item to its native representation.
                BNQualifiedNameTypeAndId[] nativeArray = new BNQualifiedNameTypeAndId[types.Length];

                for (int i = 0; i < types.Length; i++)
                {
                    nativeArray[i] = types[i].ToNativeEx(allocator);
                }

                // 2.2 Pin the native array and call the API.
                IntPtr arrayPointer = allocator.AllocStructArray<BNQualifiedNameTypeAndId>(nativeArray);

                // 3. Forward to the native API.
                return NativeMethods.BNAddTypeArchiveTypes(
                    this.handle ,
                    arrayPointer ,
                    (ulong)types.Length
                );
            }
        }

        /// <summary>
        /// Retrieves all type names stored in this type archive at the given snapshot.
        /// </summary>
        /// <param name="snapshot">The snapshot ID to query, or empty string for the latest.</param>
        /// <returns>An array of qualified names for all types in the archive.</returns>
        public unsafe QualifiedName[] GetTypeNames(string snapshot = "")
        {
            // 1. Stack-allocate the count variable.
            ulong count = 0;

            // 2. Call the native API.
            IntPtr arrayPointer = NativeMethods.BNGetTypeArchiveTypeNames(
                this.handle ,
                snapshot ?? string.Empty ,
                (IntPtr)(&count)
            );

            // 3. Convert the native array to managed objects and free the native memory.
            return UnsafeUtils.TakeStructArrayEx<BNQualifiedName , QualifiedName>(
                arrayPointer ,
                count ,
                QualifiedName.FromNative ,
                NativeMethods.BNFreeQualifiedNameArray
            );
        }

        /// <summary>
        /// Retrieves all types stored in this type archive at the given snapshot,
        /// including their names, IDs, and type objects.
        /// </summary>
        /// <param name="snapshot">The snapshot ID to query, or empty string for the latest.</param>
        /// <returns>An array of QualifiedNameTypeAndId entries for all types in the archive.</returns>
        public unsafe QualifiedNameTypeAndId[] GetTypes(string snapshot = "")
        {
            // 1. Stack-allocate the count variable.
            ulong count = 0;

            // 2. Call the native API.
            IntPtr arrayPointer = NativeMethods.BNGetTypeArchiveTypes(
                this.handle ,
                snapshot ?? string.Empty ,
                (IntPtr)(&count)
            );

            // 3. Convert the native array to managed objects and free the native memory.
            return UnsafeUtils.TakeStructArray<BNQualifiedNameTypeAndId , QualifiedNameTypeAndId>(
                arrayPointer ,
                count ,
                QualifiedNameTypeAndId.FromNative ,
                NativeMethods.BNFreeQualifiedNameTypeAndId
            );
        }
    }
}
