using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents the Firmware Ninja analysis engine attached to a binary view.
    /// Provides operations for querying and managing firmware relationships, memory
    /// access patterns, and hardware device interactions discovered during analysis.
    /// </summary>
    public sealed class FirmwareNinja : AbstractSafeHandle<FirmwareNinja>
    {
        /// <summary>
        /// Initializes a new FirmwareNinja wrapper around an existing native handle.
        /// </summary>
        /// <param name="handle">The native pointer to the BNFirmwareNinja object.</param>
        /// <param name="owner">True if this wrapper owns the handle and should free it on dispose.</param>
        public FirmwareNinja(IntPtr handle, bool owner)
            : base(handle, owner)
        {
        }

        /// <summary>
        /// Creates a new FirmwareNinja analysis engine attached to the given binary view.
        /// The caller is responsible for disposing the returned instance.
        /// </summary>
        /// <param name="view">The binary view to attach the analysis engine to.</param>
        /// <returns>A new owned FirmwareNinja instance, or null on failure.</returns>
        public static FirmwareNinja? Create(BinaryView view)
        {
            // 1. Validate the required binary view parameter.
            if (null == view)
            {
                throw new ArgumentNullException(nameof(view));
            }

            // 2. Create a new FirmwareNinja engine for the given view; the returned handle is owned.
            return FirmwareNinja.TakeHandle(
                NativeMethods.BNCreateFirmwareNinja(view.DangerousGetHandle())
            );
        }

        /// <summary>
        /// Takes ownership of an existing handle without incrementing the reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNFirmwareNinja pointer.</param>
        /// <returns>A new owned FirmwareNinja, or null if the handle is zero.</returns>
        internal static FirmwareNinja? TakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new FirmwareNinja(handle, true);
        }

        /// <summary>
        /// Takes ownership of an existing handle without incrementing the reference count.
        /// Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNFirmwareNinja pointer.</param>
        /// <returns>A new owned FirmwareNinja.</returns>
        internal static FirmwareNinja MustTakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new FirmwareNinja(handle, true);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNFirmwareNinja pointer.</param>
        /// <returns>A new FirmwareNinja that will not free the handle on dispose.</returns>
        internal static FirmwareNinja? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new FirmwareNinja(handle, false);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNFirmwareNinja pointer.</param>
        /// <returns>A new FirmwareNinja that will not free the handle on dispose.</returns>
        internal static FirmwareNinja MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new FirmwareNinja(handle, false);
        }

        /// <summary>
        /// Releases the native BNFirmwareNinja handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native engine handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeFirmwareNinja(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Retrieves all firmware relationships currently stored in this analysis engine.
        /// Each returned relationship is a new owned reference.
        /// </summary>
        /// <returns>An array of all known FirmwareNinjaRelationship instances.</returns>
        public unsafe FirmwareNinjaRelationship[] QueryRelationships()
        {
            // 1. Stack-allocate the count variable and retrieve the native relationship array.
            ulong count = 0;
            IntPtr ptr = NativeMethods.BNFirmwareNinjaQueryRelationships(
                this.handle,
                (IntPtr)(&count)
            );

            // 2. Convert the pointer array to owned managed wrappers and free the native list.
            return UnsafeUtils.TakeHandleArrayEx<FirmwareNinjaRelationship>(
                ptr,
                count,
                FirmwareNinjaRelationship.MustNewFromHandle,
                NativeMethods.BNFirmwareNinjaFreeRelationships
            );
        }

        /// <summary>
        /// Looks up a stored firmware relationship by its GUID.
        /// Returns null if no relationship with the given GUID exists.
        /// </summary>
        /// <param name="guid">The GUID string that uniquely identifies the relationship.</param>
        /// <returns>The matching FirmwareNinjaRelationship as a new owned reference, or null.</returns>
        public FirmwareNinjaRelationship? GetRelationshipByGuid(string guid)
        {
            // 1. Query the engine for a relationship matching the given GUID string.
            IntPtr raw = NativeMethods.BNFirmwareNinjaGetRelationshipByGuid(
                this.handle,
                guid ?? string.Empty
            );

            // 2. Addref and wrap the returned pointer; null on not-found (zero pointer).
            return FirmwareNinjaRelationship.NewFromHandle(raw);
        }

        /// <summary>
        /// Adds a firmware relationship to this analysis engine's relationship store.
        /// The relationship is referenced (addref'd) by the engine after this call.
        /// </summary>
        /// <param name="relationship">The relationship to add.</param>
        public void AddRelationship(FirmwareNinjaRelationship relationship)
        {
            // 1. Validate the required relationship parameter.
            if (null == relationship)
            {
                throw new ArgumentNullException(nameof(relationship));
            }

            // 2. Forward the relationship handle to the native add API.
            NativeMethods.BNFirmwareNinjaAddRelationship(
                this.handle,
                relationship.DangerousGetHandle()
            );
        }

        /// <summary>
        /// Removes the firmware relationship identified by the given GUID from this engine.
        /// Has no effect if no relationship with the given GUID exists.
        /// </summary>
        /// <param name="guid">The GUID string of the relationship to remove.</param>
        public void RemoveRelationshipByGuid(string guid)
        {
            // Delegate to the native remove API using the given GUID string.
            NativeMethods.BNFirmwareNinjaRemoveRelationshipByGuid(
                this.handle,
                guid ?? string.Empty
            );
        }

        /// <summary>
        /// Removes a previously stored custom device by name.
        /// </summary>
        /// <param name="name">The name of the custom device to remove.</param>
        /// <returns>True if the device was found and removed; false otherwise.</returns>
        public bool RemoveCustomDevice(string name)
        {
            // Forward the device name to the native remove API and return the result.
            return NativeMethods.BNFirmwareNinjaRemoveCustomDevice(
                this.handle,
                name ?? string.Empty
            );
        }

        /// <summary>
        /// Stores a custom device definition with the given name, address range, and info string.
        /// </summary>
        /// <param name="name">The human-readable name of the custom device.</param>
        /// <param name="start">The start address of the device memory region.</param>
        /// <param name="end">The end address of the device memory region.</param>
        /// <param name="info">A description or additional information about the device.</param>
        /// <returns>True if the device was stored successfully; false otherwise.</returns>
        public bool StoreCustomDevice(string name, ulong start, ulong end, string info)
        {
            // Forward all parameters to the native store API and return the result.
            return NativeMethods.BNFirmwareNinjaStoreCustomDevice(
                this.handle,
                name ?? string.Empty,
                start,
                end,
                info ?? string.Empty
            );
        }

        /// <summary>
        /// Analyzes all functions in the binary view and collects their memory access patterns.
        /// The returned pointer refers to a native array of BNFirmwareNinjaFunctionMemoryAccesses
        /// structs whose lifetime is managed by the caller.
        /// Free with BNFirmwareNinjaFreeFunctionMemoryAccesses when done.
        /// </summary>
        /// <param name="fmaPointer">Receives a pointer to the native array of function memory accesses.</param>
        /// <param name="progress">Optional progress callback invoked during analysis.</param>
        /// <returns>The number of elements in the returned array.</returns>
        public unsafe int GetFunctionMemoryAccesses(out IntPtr fmaPointer, ProgressDelegate? progress = null)
        {
            // 1. Stack-allocate a local pointer for the native out parameter.
            IntPtr fma = IntPtr.Zero;

            // 2. Call the native analysis function with optional progress callback.
            int count = NativeMethods.BNFirmwareNinjaGetFunctionMemoryAccesses(
                this.handle,
                (IntPtr)(&fma),
                null == progress
                    ? IntPtr.Zero
                    : Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(
                        UnsafeUtils.WrapProgressDelegate(progress)),
                IntPtr.Zero
            );

            // 3. Copy the resulting pointer to the out parameter.
            fmaPointer = fma;

            return count;
        }

        /// <summary>
        /// Queries previously stored function memory accesses from the binary view metadata.
        /// The returned pointer refers to a native array of BNFirmwareNinjaFunctionMemoryAccesses
        /// structs whose lifetime is managed by the caller.
        /// Free with BNFirmwareNinjaFreeFunctionMemoryAccesses when done.
        /// </summary>
        /// <param name="fmaPointer">Receives a pointer to the native array of function memory accesses.</param>
        /// <returns>The number of elements in the returned array.</returns>
        public unsafe int QueryFunctionMemoryAccessesFromMetadata(out IntPtr fmaPointer)
        {
            // 1. Stack-allocate a local pointer for the native out parameter.
            IntPtr fma = IntPtr.Zero;

            // 2. Call the native metadata query function.
            int count = NativeMethods.BNFirmwareNinjaQueryFunctionMemoryAccessesFromMetadata(
                this.handle,
                (IntPtr)(&fma)
            );

            // 3. Copy the resulting pointer to the out parameter.
            fmaPointer = fma;

            return count;
        }

        /// <summary>
        /// Stores the given function memory accesses array to the binary view metadata
        /// for later retrieval via QueryFunctionMemoryAccessesFromMetadata.
        /// </summary>
        /// <param name="fmaPointer">Pointer to the native array of BNFirmwareNinjaFunctionMemoryAccesses.</param>
        /// <param name="size">The number of elements in the array.</param>
        public void StoreFunctionMemoryAccessesToMetadata(IntPtr fmaPointer, int size)
        {
            // Forward the native array pointer and its size to the metadata store API.
            NativeMethods.BNFirmwareNinjaStoreFunctionMemoryAccessesToMetadata(
                this.handle,
                fmaPointer,
                size
            );
        }

        /// <summary>
        /// Performs entropy-based analysis to find code and data sections in the binary.
        /// The returned pointer refers to a native array of BNFirmwareNinjaSection structs.
        /// Free with BNFirmwareNinjaFreeSections when done.
        /// </summary>
        /// <param name="sectionsPointer">Receives a pointer to the native array of discovered sections.</param>
        /// <param name="highThreshold">The upper entropy threshold for code detection (default 0.9).</param>
        /// <param name="lowThreshold">The lower entropy threshold for code detection (default 0.5).</param>
        /// <param name="blockSize">The block size in bytes used for entropy calculation (default 4096).</param>
        /// <param name="mode">The section analysis mode to use.</param>
        /// <returns>The number of sections found.</returns>
        public unsafe int FindSectionsWithEntropy(
            out IntPtr sectionsPointer,
            float highThreshold = 0.9f,
            float lowThreshold = 0.5f,
            ulong blockSize = 4096,
            FirmwareNinjaSectionAnalysisMode mode = default)
        {
            // 1. Stack-allocate a local pointer for the native out parameter.
            IntPtr sections = IntPtr.Zero;

            // 2. Call the native entropy analysis function with the given thresholds.
            int count = NativeMethods.BNFirmwareNinjaFindSectionsWithEntropy(
                this.handle,
                (IntPtr)(&sections),
                highThreshold,
                lowThreshold,
                blockSize,
                mode
            );

            // 3. Copy the resulting pointer to the out parameter.
            sectionsPointer = sections;

            return count;
        }

        /// <summary>
        /// Queries the names of all known boards for the given CPU architecture.
        /// </summary>
        /// <param name="arch">The architecture to query board names for.</param>
        /// <returns>An array of board name strings; empty if none are found.</returns>
        public unsafe string[] QueryBoardNamesForArchitecture(Architecture arch)
        {
            // 1. Stack-allocate a local pointer for the native out parameter.
            IntPtr boards = IntPtr.Zero;

            // 2. Call the native query; returns the number of board names found.
            int count = NativeMethods.BNFirmwareNinjaQueryBoardNamesForArchitecture(
                this.handle,
                arch.DangerousGetHandle(),
                (IntPtr)(&boards)
            );

            // 3. Return empty array when no boards are found.
            if (count <= 0)
            {
                return Array.Empty<string>();
            }

            // 4. Convert the native string array to managed strings and free the native array.
            return UnsafeUtils.TakeAnsiStringArray(
                boards,
                (ulong)count,
                NativeMethods.BNFreeStringList
            );
        }

        /// <summary>
        /// Queries the devices present on a specific board for the given architecture.
        /// The returned pointer refers to a native array of BNFirmwareNinjaDevice structs.
        /// Free with BNFirmwareNinjaFreeDevices when done.
        /// </summary>
        /// <param name="arch">The architecture the board belongs to.</param>
        /// <param name="board">The board name to query devices for.</param>
        /// <param name="devicesPointer">Receives a pointer to the native array of devices.</param>
        /// <returns>The number of devices found on the board.</returns>
        public unsafe int QueryBoardDevices(Architecture arch, string board, out IntPtr devicesPointer)
        {
            // 1. Stack-allocate a local pointer for the native out parameter.
            IntPtr devices = IntPtr.Zero;

            // 2. Call the native query with the architecture handle and board name.
            int count = NativeMethods.BNFirmwareNinjaQueryBoardDevices(
                this.handle,
                arch.DangerousGetHandle(),
                board ?? string.Empty,
                (IntPtr)(&devices)
            );

            // 3. Copy the resulting pointer to the out parameter.
            devicesPointer = devices;

            return count;
        }

        /// <summary>
        /// Queries all custom devices that have been stored in this analysis engine.
        /// The returned pointer refers to a native array of BNFirmwareNinjaDevice structs.
        /// Free with BNFirmwareNinjaFreeDevices when done.
        /// </summary>
        /// <param name="devicesPointer">Receives a pointer to the native array of custom devices.</param>
        /// <returns>The number of custom devices stored.</returns>
        public unsafe int QueryCustomDevices(out IntPtr devicesPointer)
        {
            // 1. Stack-allocate a local pointer for the native out parameter.
            IntPtr devices = IntPtr.Zero;

            // 2. Call the native query for custom devices.
            int count = NativeMethods.BNFirmwareNinjaQueryCustomDevices(
                this.handle,
                (IntPtr)(&devices)
            );

            // 3. Copy the resulting pointer to the out parameter.
            devicesPointer = devices;

            return count;
        }

        /// <summary>
        /// Cross-references function memory accesses with known board devices to determine
        /// which functions access which hardware peripherals.
        /// The returned pointer refers to a native array of BNFirmwareNinjaDeviceAccesses structs.
        /// Free with BNFirmwareNinjaFreeBoardDeviceAccesses when done.
        /// </summary>
        /// <param name="fma">Pointer to the function memory accesses array (from GetFunctionMemoryAccesses).</param>
        /// <param name="size">The number of elements in the fma array.</param>
        /// <param name="accessesPointer">Receives a pointer to the native array of device accesses.</param>
        /// <param name="arch">The architecture to use for device lookup.</param>
        /// <returns>The number of device access entries found.</returns>
        public unsafe int GetBoardDeviceAccesses(
            IntPtr fma,
            int size,
            out IntPtr accessesPointer,
            Architecture arch)
        {
            // 1. Stack-allocate a local pointer for the native out parameter.
            IntPtr accesses = IntPtr.Zero;

            // 2. Call the native analysis function with the fma array and architecture.
            int count = NativeMethods.BNFirmwareNinjaGetBoardDeviceAccesses(
                this.handle,
                fma,
                size,
                (IntPtr)(&accesses),
                arch.DangerousGetHandle()
            );

            // 3. Copy the resulting pointer to the out parameter.
            accessesPointer = accesses;

            return count;
        }

        /// <summary>
        /// Builds a reference tree showing how a specific address is accessed by functions
        /// in the analyzed binary. Returns a root FirmwareNinjaReferenceNode for tree traversal.
        /// </summary>
        /// <param name="address">The address to build the reference tree for.</param>
        /// <param name="fma">Pointer to the function memory accesses array.</param>
        /// <param name="size">The number of elements in the fma array.</param>
        /// <param name="value">Receives the resolved value at the address, if applicable.</param>
        /// <returns>The root reference node of the tree, or null on failure.</returns>
        public unsafe FirmwareNinjaReferenceNode? GetAddressReferenceTree(
            ulong address,
            IntPtr fma,
            int size,
            out ulong value)
        {
            // 1. Stack-allocate the value output slot.
            ulong outValue = 0;

            // 2. Call the native function to build the reference tree.
            IntPtr raw = NativeMethods.BNFirmwareNinjaGetAddressReferenceTree(
                this.handle,
                address,
                fma,
                size,
                (IntPtr)(&outValue)
            );

            // 3. Copy the resolved value to the out parameter.
            value = outValue;

            // 4. Wrap the returned node pointer; null if the native call returned zero.
            return FirmwareNinjaReferenceNode.TakeHandle(raw);
        }

        /// <summary>
        /// Builds a reference tree showing how a memory region (start to end) is accessed
        /// by functions in the analyzed binary. Returns a root FirmwareNinjaReferenceNode.
        /// </summary>
        /// <param name="start">The start address of the memory region.</param>
        /// <param name="end">The end address of the memory region.</param>
        /// <param name="fma">Pointer to the function memory accesses array.</param>
        /// <param name="size">The number of elements in the fma array.</param>
        /// <param name="value">Receives the resolved value for the region, if applicable.</param>
        /// <returns>The root reference node of the tree, or null on failure.</returns>
        public unsafe FirmwareNinjaReferenceNode? GetMemoryRegionReferenceTree(
            ulong start,
            ulong end,
            IntPtr fma,
            int size,
            out ulong value)
        {
            // 1. Stack-allocate the value output slot.
            ulong outValue = 0;

            // 2. Call the native function to build the memory region reference tree.
            IntPtr raw = NativeMethods.BNFirmwareNinjaGetMemoryRegionReferenceTree(
                this.handle,
                start,
                end,
                fma,
                size,
                (IntPtr)(&outValue)
            );

            // 3. Copy the resolved value to the out parameter.
            value = outValue;

            // 4. Wrap the returned node pointer; null if the native call returned zero.
            return FirmwareNinjaReferenceNode.TakeHandle(raw);
        }
    }
}
