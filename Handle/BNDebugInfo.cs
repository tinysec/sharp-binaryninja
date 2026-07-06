using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Wraps a BNDebugInfo native handle, providing access to debug functions, types,
    /// and data variables aggregated by debug info parsers.
    /// </summary>
    public sealed class DebugInfo : AbstractSafeHandle<DebugInfo>
    {
        /// <summary>
        /// Initializes a new DebugInfo wrapper around an existing native handle.
        /// </summary>
        /// <param name="handle">The native BNDebugInfo pointer.</param>
        /// <param name="owner">True if this instance owns the handle and should free it on dispose.</param>
        internal DebugInfo(IntPtr handle, bool owner)
            : base(handle, owner)
        {
        }

        /// <summary>
        /// Creates a managed wrapper by taking a new reference on the given handle.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNDebugInfo pointer.</param>
        /// <returns>A new DebugInfo instance that owns an addref'd reference, or null.</returns>
        internal static DebugInfo? NewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new DebugInfo(
                NativeMethods.BNNewDebugInfoReference(handle),
                true
            );
        }

        /// <summary>
        /// Creates a managed wrapper by taking a new reference on the given handle.
        /// Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNDebugInfo pointer.</param>
        /// <returns>A new DebugInfo instance that owns an addref'd reference.</returns>
        internal static DebugInfo MustNewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new DebugInfo(
                NativeMethods.BNNewDebugInfoReference(handle),
                true
            );
        }

        /// <summary>
        /// Takes ownership of an already-owned native handle without addref'ing.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNDebugInfo pointer (already owned by caller).</param>
        /// <returns>A new DebugInfo instance that owns the handle, or null.</returns>
        internal static DebugInfo? TakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new DebugInfo(handle, true);
        }

        /// <summary>
        /// Takes ownership of an already-owned native handle without addref'ing.
        /// Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNDebugInfo pointer (already owned by caller).</param>
        /// <returns>A new DebugInfo instance that owns the handle.</returns>
        internal static DebugInfo MustTakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new DebugInfo(handle, true);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNDebugInfo pointer.</param>
        /// <returns>A new DebugInfo instance that will not free the handle on dispose, or null.</returns>
        internal static DebugInfo? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new DebugInfo(handle, false);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNDebugInfo pointer.</param>
        /// <returns>A new DebugInfo instance that will not free the handle on dispose.</returns>
        internal static DebugInfo MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new DebugInfo(handle, false);
        }

        /// <summary>
        /// Releases the native BNDebugInfo handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native debug info reference and mark it invalid to prevent double-free.
                NativeMethods.BNFreeDebugInfoReference(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Gets the names of all parsers that have contributed data to this DebugInfo object.
        /// </summary>
        public unsafe string[] ParserNames
        {
            get
            {
                // 1. Stack-allocate the count and retrieve the native string-pointer array.
                ulong count = 0;

                IntPtr ptr = NativeMethods.BNGetDebugParserNames(
                    this.handle,
                    (IntPtr)(&count)
                );

                // 2. Read all ANSI strings from the native array.
                string[] names = UnsafeUtils.ReadAnsiStringArray(ptr, count);

                // 3. Free the native string array (BNFreeStringList takes ptr + count).
                if (ptr != IntPtr.Zero)
                {
                    NativeMethods.BNFreeStringList(ptr, count);
                }

                return names;
            }
        }

        /// <summary>
        /// Retrieves all debug functions, optionally filtered by name.
        /// Pass null to get every function from all parsers.
        /// </summary>
        /// <param name="name">Optional name filter; null returns all functions.</param>
        /// <returns>Array of DebugFunctionInfo records.</returns>
        public unsafe DebugFunctionInfo[] GetFunctions(string? name = null)
        {
            // 1. Stack-allocate the count variable and call the native API.
            ulong count = 0;

            IntPtr ptr = NativeMethods.BNGetDebugFunctions(
                this.handle,
                name ?? string.Empty,
                (IntPtr)(&count)
            );

            // 2. Convert each native BNDebugFunctionInfo to a managed DebugFunctionInfo,
            //    then free the native array (count is passed for bookkeeping).
            return UnsafeUtils.TakeStructArrayEx<BNDebugFunctionInfo, DebugFunctionInfo>(
                ptr,
                count,
                ConvertDebugFunctionInfo,
                NativeMethods.BNFreeDebugFunctions
            );
        }

        /// <summary>
        /// Retrieves all named debug types, optionally filtered by name.
        /// Pass null to get every type from all parsers.
        /// </summary>
        /// <param name="name">Optional name filter; null returns all types.</param>
        /// <returns>Array of NameAndType records.</returns>
        public unsafe NameAndType[] GetTypes(string? name = null)
        {
            // 1. Stack-allocate the count variable and call the native API.
            ulong count = 0;

            IntPtr ptr = NativeMethods.BNGetDebugTypes(
                this.handle,
                name ?? string.Empty,
                (IntPtr)(&count)
            );

            // 2. Convert each native BNNameAndType to a managed NameAndType,
            //    then free the native array.
            return UnsafeUtils.TakeStructArrayEx<BNNameAndType, NameAndType>(
                ptr,
                count,
                NameAndType.FromNative,
                NativeMethods.BNFreeDebugTypes
            );
        }

        /// <summary>
        /// Retrieves all named debug data variables, optionally filtered by name.
        /// Pass null to get every data variable from all parsers.
        /// </summary>
        /// <param name="name">Optional name filter; null returns all data variables.</param>
        /// <returns>Array of DataVariableAndName records.</returns>
        public unsafe DataVariableAndName[] GetDataVariables(string? name = null)
        {
            // 1. Stack-allocate the count variable and call the native API.
            ulong count = 0;

            IntPtr ptr = NativeMethods.BNGetDebugDataVariables(
                this.handle,
                name ?? string.Empty,
                (IntPtr)(&count)
            );

            // 2. Convert each native BNDataVariableAndName to a managed DataVariableAndName,
            //    then free the native array.
            return UnsafeUtils.TakeStructArrayEx<BNDataVariableAndName, DataVariableAndName>(
                ptr,
                count,
                DataVariableAndName.FromNative,
                NativeMethods.BNFreeDataVariablesAndName
            );
        }

        /// <summary>
        /// Returns the type container that provides type information contributed by the
        /// given debug info parser.
        /// </summary>
        /// <param name="parserName">The name of the contributing parser.</param>
        /// <returns>An owned TypeContainer for the specified parser, or null.</returns>
        public TypeContainer? GetTypeContainer(string parserName)
        {
            // BNGetDebugInfoTypeContainer returns an OWNED handle (the C++ wrapper adopts it
            // with no addref, matching TypeArchive.GetTypeContainer); take it directly.
            return TypeContainer.TakeHandle(
                NativeMethods.BNGetDebugInfoTypeContainer(
                    this.handle,
                    parserName ?? string.Empty
                )
            );
        }

        /// <summary>
        /// Adds a function record to this debug info.
        /// </summary>
        /// <param name="info">The debug function information to add.</param>
        /// <returns>True if the function was added successfully.</returns>
        public unsafe bool AddFunction(DebugFunctionInfo info)
        {
            // 1. Convert the managed DebugFunctionInfo to a native BNDebugFunctionInfo
            //    using a scoped allocator to manage native string lifetimes.
            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                BNDebugFunctionInfo native = ToNativeDebugFunctionInfo(info, allocator);

                // 2. Pass a pointer to the native struct to the native API.
                return NativeMethods.BNAddDebugFunction(
                    this.handle,
                    (IntPtr)(&native)
                );
            }
        }

        /// <summary>
        /// Adds a named type to this debug info.
        /// </summary>
        /// <param name="name">The name of the type.</param>
        /// <param name="type">The type object to register.</param>
        /// <param name="components">An array of component (namespace) strings.</param>
        /// <returns>True if the type was added successfully.</returns>
        public bool AddType(string name, BinaryNinja.Type type, string[] components)
        {
            // Retrieve the raw type handle.
            IntPtr typeHandle = (type != null) ? type.DangerousGetHandle() : IntPtr.Zero;

            string[] safeComponents = components ?? Array.Empty<string>();

            // components is a const char** UTF-8 input block; build it by hand
            // because .NET cannot apply LPUTF8Str to string[] array elements.
            bool ok;
            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                IntPtr componentsBlock = allocator.AllocUtf8StringArray(safeComponents);

                ok = NativeMethods.BNAddDebugType(
                    this.handle,
                    name ?? string.Empty,
                    typeHandle,
                    componentsBlock,
                    (ulong)safeComponents.Length
                );
            }

            return ok;
        }

        /// <summary>
        /// Adds a data variable to this debug info.
        /// </summary>
        /// <param name="address">The virtual address of the data variable.</param>
        /// <param name="type">The type of the data variable.</param>
        /// <param name="name">The name of the data variable.</param>
        /// <param name="components">An array of component (namespace) strings.</param>
        /// <returns>True if the data variable was added successfully.</returns>
        public bool AddDataVariable(
            ulong address,
            BinaryNinja.Type type,
            string name,
            string[] components)
        {
            // Retrieve the raw type handle.
            IntPtr typeHandle = (type != null) ? type.DangerousGetHandle() : IntPtr.Zero;

            string[] safeComponents = components ?? Array.Empty<string>();

            // components is a const char** UTF-8 input block; build it by hand
            // because .NET cannot apply LPUTF8Str to string[] array elements.
            bool ok;
            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                IntPtr componentsBlock = allocator.AllocUtf8StringArray(safeComponents);

                ok = NativeMethods.BNAddDebugDataVariable(
                    this.handle,
                    address,
                    typeHandle,
                    name ?? string.Empty,
                    componentsBlock,
                    (ulong)safeComponents.Length
                );
            }

            return ok;
        }

        /// <summary>
        /// Removes all functions contributed by the named parser from this debug info.
        /// </summary>
        /// <param name="parserName">The name of the parser whose functions to remove.</param>
        /// <returns>True if the removal succeeded.</returns>
        public bool RemoveParserFunctions(string parserName)
        {
            // Delegate to the native removal API.
            return NativeMethods.BNRemoveDebugParserFunctions(
                this.handle,
                parserName ?? string.Empty
            );
        }

        /// <summary>
        /// Removes all types contributed by the named parser from this debug info.
        /// </summary>
        /// <param name="parserName">The name of the parser whose types to remove.</param>
        /// <returns>True if the removal succeeded.</returns>
        public bool RemoveParserTypes(string parserName)
        {
            // Delegate to the native removal API.
            return NativeMethods.BNRemoveDebugParserTypes(
                this.handle,
                parserName ?? string.Empty
            );
        }

        /// <summary>
        /// Removes all data variables contributed by the named parser from this debug info.
        /// </summary>
        /// <param name="parserName">The name of the parser whose data variables to remove.</param>
        /// <returns>True if the removal succeeded.</returns>
        public bool RemoveParserDataVariables(string parserName)
        {
            // Delegate to the native removal API.
            return NativeMethods.BNRemoveDebugParserDataVariables(
                this.handle,
                parserName ?? string.Empty
            );
        }

        /// <summary>
        /// Removes all data (functions, types, data variables) contributed by the named parser.
        /// </summary>
        /// <param name="parserName">The name of the parser whose data to remove.</param>
        /// <returns>True if the removal succeeded.</returns>
        public bool RemoveParserInfo(string parserName)
        {
            // Delegate to the native removal API.
            return NativeMethods.BNRemoveDebugParserInfo(
                this.handle,
                parserName ?? string.Empty
            );
        }

        /// <summary>
        /// Removes a single function from the named parser's function list by its index.
        /// </summary>
        /// <param name="parserName">The name of the contributing parser.</param>
        /// <param name="index">The zero-based index of the function to remove.</param>
        /// <returns>True if the removal succeeded.</returns>
        public bool RemoveFunctionByIndex(string parserName, ulong index)
        {
            // Delegate to the native removal API.
            return NativeMethods.BNRemoveDebugFunctionByIndex(
                this.handle,
                parserName ?? string.Empty,
                index
            );
        }

        /// <summary>
        /// Removes a data variable at the given address from the named parser's data variable list.
        /// </summary>
        /// <param name="parserName">The name of the contributing parser.</param>
        /// <param name="address">The virtual address of the data variable to remove.</param>
        /// <returns>True if the removal succeeded.</returns>
        public bool RemoveDataVariableByAddress(string parserName, ulong address)
        {
            // Delegate to the native removal API.
            return NativeMethods.BNRemoveDebugDataVariableByAddress(
                this.handle,
                parserName ?? string.Empty,
                address
            );
        }

        /// <summary>
        /// Converts a native BNDebugFunctionInfo value-type struct to a managed DebugFunctionInfo object.
        /// Reads all ANSI string fields, the type handle, and the components string array.
        /// </summary>
        /// <param name="native">The native debug function struct to convert.</param>
        /// <returns>A fully populated DebugFunctionInfo instance.</returns>
        private static DebugFunctionInfo ConvertDebugFunctionInfo(BNDebugFunctionInfo native)
        {
            // 1. Build the managed object, converting each field individually.
            DebugFunctionInfo info = new DebugFunctionInfo();

            // 1.1 Read the three name strings from native ANSI pointers.
            info.ShortName = UnsafeUtils.ReadAnsiString(native.shortName);
            info.FullName = UnsafeUtils.ReadAnsiString(native.fullName);
            info.RawName = UnsafeUtils.ReadAnsiString(native.rawName);

            // 1.2 Copy the address directly.
            info.Address = native.address;

            // 1.3 Addref the type handle if non-null; leave null otherwise.
            info.Type = BinaryNinja.Type.NewFromHandle(native.type);

            // 1.4 Borrow the platform handle if non-null.
            info.Platform = BinaryNinja.Platform.NewFromHandle(native.platform);

            // 1.5 Read the components char** array.
            info.Components = UnsafeUtils.ReadAnsiStringArray(native.components, native.componentN);

            // 1.6 Read the local variables array.
            info.LocalVariables = UnsafeUtils.ReadStructArray<BNVariableNameAndType, VariableNameAndType>(
                native.localVariables,
                native.localVariableN,
                VariableNameAndType.FromNative
            );

            return info;
        }

        /// <summary>
        /// Converts a managed DebugFunctionInfo to a native BNDebugFunctionInfo, using the
        /// provided allocator to ensure native string memory remains valid for the call duration.
        /// The caller must keep the allocator alive until the native call completes.
        /// </summary>
        /// <param name="info">The managed debug function information to convert.</param>
        /// <param name="allocator">The scoped allocator that owns native string memory.</param>
        /// <returns>A native BNDebugFunctionInfo struct suitable for P/Invoke.</returns>
        private static unsafe BNDebugFunctionInfo ToNativeDebugFunctionInfo(
            DebugFunctionInfo info,
            ScopedAllocator allocator)
        {
            // 1. Allocate ANSI strings for the three name fields.
            IntPtr shortNamePtr = allocator.AllocAnsiString(info.ShortName ?? string.Empty);
            IntPtr fullNamePtr = allocator.AllocAnsiString(info.FullName ?? string.Empty);
            IntPtr rawNamePtr = allocator.AllocAnsiString(info.RawName ?? string.Empty);

            // 2. Retrieve raw handles for the type and platform (may be null).
            IntPtr typeHandle = (info.Type != null) ? info.Type.DangerousGetHandle() : IntPtr.Zero;
            IntPtr platformHandle = (info.Platform != null) ? info.Platform.DangerousGetHandle() : IntPtr.Zero;

            // 3. Populate the native struct (components and localVariables are not set here;
            //    the native AddFunction API accepts them via separate parameters if needed).
            BNDebugFunctionInfo native = new BNDebugFunctionInfo
            {
                shortName = shortNamePtr,
                fullName = fullNamePtr,
                rawName = rawNamePtr,
                address = info.Address,
                type = typeHandle,
                platform = platformHandle,
                components = IntPtr.Zero,
                componentN = 0,
                localVariables = IntPtr.Zero,
                localVariableN = 0
            };

            return native;
        }

        /// <summary>
        /// Adds a data variable info record (with name) to this debug info.
        /// </summary>
        /// <param name="var">The data variable and name to add.</param>
        /// <returns>True if the data variable info was added successfully.</returns>
        public unsafe bool AddDataVariableInfo(DataVariableAndName var)
        {
            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                BNDataVariableAndName native = var.ToNative(allocator);

                return NativeMethods.BNAddDebugDataVariableInfo(
                    this.handle,
                    (IntPtr)(&native)
                );
            }
        }

        /// <summary>
        /// Retrieves a data variable by its address from the given parser.
        /// </summary>
        /// <param name="parserName">The name of the contributing parser.</param>
        /// <param name="address">The virtual address of the data variable.</param>
        /// <returns>A DataVariableAndName if found, or null.</returns>
        public unsafe DataVariableAndName? GetDataVariableByAddress(string parserName, ulong address)
        {
            BNDataVariableAndName native = new BNDataVariableAndName();

            bool ok = NativeMethods.BNGetDebugDataVariableByAddress(
                this.handle,
                parserName,
                address,
                (IntPtr)(&native)
            );

            if (!ok)
            {
                return null;
            }

            return DataVariableAndName.FromNative(native);
        }

        /// <summary>
        /// Retrieves a data variable by its name from the given parser.
        /// </summary>
        /// <param name="parserName">The name of the contributing parser.</param>
        /// <param name="variableName">The name of the data variable.</param>
        /// <returns>A DataVariableAndName if found, or null.</returns>
        public unsafe DataVariableAndName? GetDataVariableByName(string parserName, string variableName)
        {
            BNDataVariableAndName native = new BNDataVariableAndName();

            bool ok = NativeMethods.BNGetDebugDataVariableByName(
                this.handle,
                parserName,
                variableName,
                (IntPtr)(&native)
            );

            if (!ok)
            {
                return null;
            }

            return DataVariableAndName.FromNative(native);
        }

        /// <summary>
        /// Removes a type by name from the given parser's type list.
        /// </summary>
        /// <param name="parserName">The name of the contributing parser.</param>
        /// <param name="typeName">The name of the type to remove.</param>
        /// <returns>True if the removal succeeded.</returns>
        public bool RemoveDebugTypeByName(string parserName, string typeName)
        {
            return NativeMethods.BNRemoveDebugTypeByName(
                this.handle,
                parserName ?? string.Empty,
                typeName ?? string.Empty
            );
        }

        /// <summary>
        /// Looks up a debug type by parser name and type name.
        /// Returns a single Type or null if not found.
        /// </summary>
        /// <param name="parserName">The name of the debug info parser that contributed the type.</param>
        /// <param name="typeName">The name of the type to look up.</param>
        /// <returns>The matching Type, or null if not found.</returns>
        public BinaryNinja.Type? GetDebugTypeByName(string parserName , string typeName)
        {
            // Forward to the native API and wrap the returned handle.
            IntPtr raw = NativeMethods.BNGetDebugTypeByName(
                this.handle ,
                parserName ?? string.Empty ,
                typeName ?? string.Empty
            );

            // Return null if the native API returned zero (type not found).
            return BinaryNinja.Type.TakeHandle(raw);
        }

        /// <summary>
        /// Retrieves all debug types matching the given type name across all parsers.
        /// Each result includes the contributing parser name and the type.
        /// </summary>
        /// <param name="typeName">The type name to search for.</param>
        /// <returns>An array of NameAndType entries matching the given name.</returns>
        public unsafe NameAndType[] GetDebugTypesByName(string typeName)
        {
            // 1. Stack-allocate the count variable.
            ulong count = 0;

            // 2. Call the native API.
            IntPtr arrayPointer = NativeMethods.BNGetDebugTypesByName(
                this.handle ,
                typeName ?? string.Empty ,
                (IntPtr)(&count)
            );

            // 3. Convert the native array to managed objects and free the native memory.
            return UnsafeUtils.TakeStructArrayEx<BNNameAndType , NameAndType>(
                arrayPointer ,
                count ,
                NameAndType.FromNative ,
                NativeMethods.BNFreeNameAndTypeList
            );
        }

        /// <summary>
        /// Retrieves all debug data variables at a specific address across all parsers.
        /// </summary>
        /// <param name="address">The address to query for data variables.</param>
        /// <returns>An array of data variables found at the given address.</returns>
        public unsafe DataVariableAndNameAndDebugParser[] GetDataVariablesByAddress(ulong address)
        {
            // 1. Stack-allocate the count variable.
            ulong count = 0;

            // 2. Call the native API.
            IntPtr arrayPointer = NativeMethods.BNGetDebugDataVariablesByAddress(
                this.handle ,
                address ,
                (IntPtr)(&count)
            );

            // 3. If no results, return empty.
            if (IntPtr.Zero == arrayPointer || 0 == count)
            {
                return Array.Empty<DataVariableAndNameAndDebugParser>();
            }

            // 4. Marshal each native struct into a managed object.
            BNDataVariableAndNameAndDebugParser* rawArray = (BNDataVariableAndNameAndDebugParser*)arrayPointer;

            DataVariableAndNameAndDebugParser[] result = new DataVariableAndNameAndDebugParser[(int)count];

            for (ulong i = 0; i < count; i++)
            {
                DataVariableAndNameAndDebugParser item = new DataVariableAndNameAndDebugParser();

                item.Address = rawArray[i].address;
                item.Type = (rawArray[i].type != IntPtr.Zero)
                    ? BinaryNinja.Type.NewFromHandle(rawArray[i].type)
                    : null;
                item.Name = UnsafeUtils.ReadAnsiString(rawArray[i].name);
                item.Parser = UnsafeUtils.ReadAnsiString(rawArray[i].parser);
                item.AutoDiscovered = rawArray[i].autoDiscovered;
                item.TypeConfidence = rawArray[i].typeConfidence;

                result[i] = item;
            }

            // 5. Free the native array.
            NativeMethods.BNFreeDataVariableAndNameAndDebugParserList(arrayPointer , count);

            return result;
        }

        /// <summary>
        /// Retrieves all debug data variables matching the given variable name across all parsers.
        /// </summary>
        /// <param name="name">The variable name to search for.</param>
        /// <returns>An array of DataVariableAndName entries matching the given name.</returns>
        public unsafe DataVariableAndName[] GetDataVariablesByName(string name)
        {
            // 1. Stack-allocate the count variable.
            ulong count = 0;

            // 2. Call the native API.
            IntPtr arrayPointer = NativeMethods.BNGetDebugDataVariablesByName(
                this.handle ,
                name ?? string.Empty ,
                (IntPtr)(&count)
            );

            // 3. If no results, return empty.
            if (IntPtr.Zero == arrayPointer || 0 == count)
            {
                return Array.Empty<DataVariableAndName>();
            }

            // 4. Marshal each native struct into a managed object.
            BNDataVariableAndName* rawArray = (BNDataVariableAndName*)arrayPointer;

            DataVariableAndName[] result = new DataVariableAndName[(int)count];

            for (ulong i = 0; i < count; i++)
            {
                result[i] = DataVariableAndName.FromNative(rawArray[i]);
            }

            // 5. Free the native array.
            // BNFreeDataVariableAndName frees a single item; use loop for array.
            // Actually the list uses BNFreeNameAndTypeList-style pattern per the P/Invoke.
            // Since BNGetDebugDataVariablesByName returns count-based array, free the whole block.
            NativeMethods.BNFreeDataVariableAndName(arrayPointer);

            return result;
        }
    }
}
