using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class Platform :  AbstractSafeHandle<Platform>
	{
		public Platform(Architecture arch , string name)
			:this( NativeMethods.BNCreatePlatform(arch.DangerousGetHandle() , name) , true)
		{
			
		}
		
	    internal Platform(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }

	    internal static Platform? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Platform(
			    NativeMethods.BNNewPlatformReference(handle) ,
			    true
		    );
	    }
	    
	    internal static Platform MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Platform(
			    NativeMethods.BNNewPlatformReference(handle) ,
			    true
		    );
	    }
	    
	    internal static Platform? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Platform(handle, true);
	    }
	    
	    internal static Platform MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Platform(handle, true);
	    }
	    
	    internal static Platform? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Platform(handle, false);
	    }
	    
	    internal static Platform MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Platform(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreePlatform(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }
		
	    public static Platform[] GetPlatforms()
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetPlatformList(out ulong arrayLength);

		    return UnsafeUtils.TakeHandleArrayEx<Platform>(
			    arrayPointer ,
			    arrayLength ,
			    Platform.MustNewFromHandle,
			    NativeMethods.BNFreePlatformList
		    );
	    }

	    public static Platform? FromName(string name)
	    {
		    return Platform.TakeHandle(
			    NativeMethods.BNGetPlatformByName(name)
		    );
	    }
	    
	    public static Platform[] GetPlatformsByOS(string os)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetPlatformListByOS(
			    os,
			    out ulong arrayLength
			);

		    return UnsafeUtils.TakeHandleArrayEx<Platform>(
			    arrayPointer ,
			    arrayLength ,
			    Platform.MustNewFromHandle,
			    NativeMethods.BNFreePlatformList
		    );
	    }

	    public static string[] GetOSList()
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetPlatformOSList(out ulong arrayLength);

		    return UnsafeUtils.TakeAnsiStringArray(
			    arrayPointer ,
			    arrayLength,
			    NativeMethods.BNFreePlatformOSList
		    );
	    }

	    public void RegisterPlatform(string os)
	    {
		    NativeMethods.BNRegisterPlatform(os , this.handle);
	    }

	    public string Name
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNGetPlatformName(this.handle)
			    );
		    }
	    }
	    
	    public Architecture Architecture
	    {
		    get
		    {
			    return Architecture.MustFromHandle(
				    NativeMethods.BNGetPlatformArchitecture(this.handle)
			    );
		    }
	    }
	    
	    public BinaryNinja.CallingConvention DefaultCallingConvention
	    {
		    get
		    {
			    return BinaryNinja.CallingConvention.MustTakeHandle(
				    NativeMethods.BNGetPlatformDefaultCallingConvention(this.handle)
			    );
		    }
	    }
	    
	    public BinaryNinja.CallingConvention CdeclCallingConvention
	    {
		    get
		    {
			    return BinaryNinja.CallingConvention.MustTakeHandle(
				    NativeMethods.BNGetPlatformCdeclCallingConvention(this.handle)
			    );
		    }
	    }
	    
	    public BinaryNinja.CallingConvention StdcallCallingConvention
	    {
		    get
		    {
			    return BinaryNinja.CallingConvention.MustTakeHandle(
				    NativeMethods.BNGetPlatformStdcallCallingConvention(this.handle)
			    );
		    }
	    }

	    public BinaryNinja.CallingConvention FastcallCallingConvention
	    {
		    get
		    {
			    return BinaryNinja.CallingConvention.MustTakeHandle(
				    NativeMethods.BNGetPlatformFastcallCallingConvention(this.handle)
			    );
		    }
	    }
	    
	    public BinaryNinja.CallingConvention SystemCallConvention
	    {
		    get
		    {
			    return BinaryNinja.CallingConvention.MustTakeHandle(
				    NativeMethods.BNGetPlatformSystemCallConvention(this.handle)
			    );
		    }
	    }
	    
	    
	    public BinaryNinja.CallingConvention[] CallingConventions()
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetPlatformCallingConventions(
			    this.handle,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeHandleArrayEx<CallingConvention>(
			    arrayPointer ,
			    arrayLength ,
			    BinaryNinja.CallingConvention.MustNewFromHandle,
			    NativeMethods.BNFreeCallingConventionList
		    );
	    }
	    
	    public void RegisterCallingConvention(BinaryNinja.CallingConvention callConvention)
	    {
			NativeMethods.BNRegisterPlatformCallingConvention(
				this.handle, 
				callConvention.DangerousGetHandle()
			);
	    }
	    
	    public void RegisterDefaultCallingConvention(BinaryNinja.CallingConvention callConvention)
	    {
		    NativeMethods.BNRegisterPlatformDefaultCallingConvention(
			    this.handle, 
			    callConvention.DangerousGetHandle()
		    );
	    }
	    
	    public void RegisterCdeclCallingConvention(BinaryNinja.CallingConvention callConvention)
	    {
		    NativeMethods.BNRegisterPlatformCdeclCallingConvention(
			    this.handle, 
			    callConvention.DangerousGetHandle()
		    );
	    }
	    
	    public void RegisterStdcallCallingConvention(BinaryNinja.CallingConvention callConvention)
	    {
		    NativeMethods.BNRegisterPlatformStdcallCallingConvention(
			    this.handle, 
			    callConvention.DangerousGetHandle()
		    );
	    }
	    
	    public void RegisterFastcallCallingConvention(BinaryNinja.CallingConvention callConvention)
	    {
		    NativeMethods.BNRegisterPlatformFastcallCallingConvention(
			    this.handle, 
			    callConvention.DangerousGetHandle()
		    );
	    }
	    
	    public void SetSystemCallConvention(BinaryNinja.CallingConvention callConvention)
	    {
		    NativeMethods.BNSetPlatformSystemCallConvention(
			    this.handle, 
			    callConvention.DangerousGetHandle()
		    );
	    }

	    public uint[] GlobalRegisters
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetPlatformGlobalRegisters(
				    this.handle,
				    out ulong arrayLength
				);
			    
			    return UnsafeUtils.TakeNumberArray<uint>(
				    arrayPointer , 
				    arrayLength,
				    NativeMethods.BNFreeRegisterList
				);
		    }
	    }

	    public ulong AddressSize
	    {
		    get
		    {
			    return NativeMethods.BNGetPlatformAddressSize(this.handle);
		    }
	    }

	    public Platform? GetRelatedPlatform(Architecture arch)
	    {
		    return Platform.TakeHandle(
			    NativeMethods.BNGetRelatedPlatform(
				    this.handle, 
				    arch.DangerousGetHandle()
				)
		    );
	    }
	    
	    public void AddRelatedPlatform(Architecture arch , Platform platform)
	    {
		    NativeMethods.BNAddRelatedPlatform(
			    this.handle ,
			    arch.DangerousGetHandle() ,
			    platform.DangerousGetHandle()
		    );
	    }

	    public Platform[] RelatedPlatforms
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetRelatedPlatforms(
				    this.handle ,
				    out ulong arrayLength
			    );

			    return UnsafeUtils.TakeHandleArrayEx<Platform>(
				    arrayPointer ,
				    arrayLength ,
				    Platform.MustNewFromHandle ,
				    NativeMethods.BNFreePlatformList
			    );
		    }
	    }
	    
	    public Platform? GetAssociatedPlatformByAddress(ref ulong address)
	    {
		    return Platform.TakeHandle(
			    NativeMethods.BNGetAssociatedPlatformByAddress(
				    this.handle, 
				    ref address
			    )
		    );
	    }


	    public TypeContainer TypeContainer
	    {
		    get
		    {
			    return TypeContainer.MustTakeHandle(
					NativeMethods.BNGetPlatformTypeContainer(this.handle)
			    );
		    }
	    }
	    
	    public QualifiedNameAndType[] Types
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetPlatformTypes(
				    this.handle,
				    out ulong arrayLength
			    );
			    
			    return UnsafeUtils.TakeStructArrayEx<BNQualifiedNameAndType , QualifiedNameAndType>(
				    arrayPointer ,
				    arrayLength ,
				    QualifiedNameAndType.FromNative ,
				    NativeMethods.BNFreeTypeAndNameList
			    );
		    }
	    }
	    
	    public QualifiedNameAndType[] Variables
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetPlatformVariables(
				    this.handle,
				    out ulong arrayLength
			    );
			    
			    return UnsafeUtils.TakeStructArrayEx<BNQualifiedNameAndType , QualifiedNameAndType>(
				    arrayPointer ,
				    arrayLength ,
				    QualifiedNameAndType.FromNative ,
				    NativeMethods.BNFreeTypeAndNameList
			    );
		    }
	    }
	    
	    public QualifiedNameAndType[] Functions
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetPlatformFunctions(
				    this.handle,
				    out ulong arrayLength
			    );
			    
			    return UnsafeUtils.TakeStructArrayEx<BNQualifiedNameAndType , QualifiedNameAndType>(
				    arrayPointer ,
				    arrayLength ,
				    QualifiedNameAndType.FromNative ,
				    NativeMethods.BNFreeTypeAndNameList
			    );
		    }
	    }
	    
	    public SystemCallInfo[] SystemCalls
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetPlatformSystemCalls(
				    this.handle,
				    out ulong arrayLength
			    );
			    
			    return UnsafeUtils.TakeStructArrayEx<BNSystemCallInfo , SystemCallInfo>(
				    arrayPointer ,
				    arrayLength ,
				    SystemCallInfo.FromNative ,
				    NativeMethods.BNFreeSystemCallList
			    );
		    }
	    }

	    public BinaryNinja.Type? GetTypeByName(QualifiedName name)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    return BinaryNinja.Type.TakeHandle(
				    NativeMethods.BNGetPlatformTypeByName(
					    this.handle ,
					    name.ToNativeEx(allocator)
					) 
			    );
		    }
	    }
	    
	    public BinaryNinja.Type? GetVariableByName(QualifiedName name)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    return BinaryNinja.Type.TakeHandle(
				    NativeMethods.BNGetPlatformVariableByName(
					    this.handle ,
					    name.ToNativeEx(allocator)
				    ) 
			    );
		    }
	    }
	    
	    public BinaryNinja.Type? GetFunctionByName(QualifiedName name , bool exactMatch =false )
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    return BinaryNinja.Type.TakeHandle(
				    NativeMethods.BNGetPlatformFunctionByName(
					    this.handle ,
					    name.ToNativeEx(allocator),
					    exactMatch
				    ) 
			    );
		    }
	    }

	    public string GetSystemCallName(uint number)
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNGetPlatformSystemCallName(this.handle , number)
		    );
	    }
	    
	    public BinaryNinja.Type? GetSystemCallType(uint number)
	    {
		    return BinaryNinja.Type.TakeHandle(
			    NativeMethods.BNGetPlatformSystemCallType(this.handle , number)
		    );
	    }

	    public TypeLibrary[] TypeLibraries
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetPlatformTypeLibraries(
				    this.handle,
				    out ulong arrayLength
				);

			    return UnsafeUtils.TakeHandleArrayEx<TypeLibrary>(
					arrayPointer,
					arrayLength,
					TypeLibrary.MustNewFromHandle,
					NativeMethods.BNFreeTypeLibraryList
			    );
		    }
	    }

	    /// <summary>
	    /// Gets the list of platforms for a given architecture.
	    /// </summary>
	    public static unsafe Platform[] GetListByArchitecture(Architecture arch)
	    {
		    ulong count = 0;

		    IntPtr arrayPointer = NativeMethods.BNGetPlatformListByArchitecture(
			    arch.DangerousGetHandle() ,
			    (IntPtr)(&count)
		    );

		    return UnsafeUtils.TakeHandleArrayEx<Platform>(
			    arrayPointer ,
			    count ,
			    Platform.MustNewFromHandle ,
			    NativeMethods.BNFreePlatformList
		    );
	    }

	    /// <summary>
	    /// Gets the list of platforms for a given OS and architecture.
	    /// </summary>
	    public static unsafe Platform[] GetListByOSAndArchitecture(string os, Architecture arch)
	    {
		    ulong count = 0;

		    IntPtr arrayPointer = NativeMethods.BNGetPlatformListByOSAndArchitecture(
			    os ,
			    arch.DangerousGetHandle() ,
			    (IntPtr)(&count)
		    );

		    return UnsafeUtils.TakeHandleArrayEx<Platform>(
			    arrayPointer ,
			    count ,
			    Platform.MustNewFromHandle ,
			    NativeMethods.BNFreePlatformList
		    );
	    }

	    /// <summary>
	    /// Gets the type of a global register on this platform.
	    /// </summary>
	    public BinaryNinja.Type? GetGlobalRegisterType(uint reg)
	    {
		    return BinaryNinja.Type.TakeHandle(
			    NativeMethods.BNGetPlatformGlobalRegisterType(this.handle , reg)
		    );
	    }

	    /// <summary>
	    /// Gets type libraries matching the given dependency name.
	    /// </summary>
	    public unsafe TypeLibrary[] GetTypeLibrariesByName(string name)
	    {
		    ulong count = 0;

		    IntPtr arrayPointer = NativeMethods.BNGetPlatformTypeLibrariesByName(
			    this.handle ,
			    name ,
			    (IntPtr)(&count)
		    );

		    return UnsafeUtils.TakeHandleArrayEx<TypeLibrary>(
			    arrayPointer ,
			    count ,
			    TypeLibrary.MustNewFromHandle ,
			    NativeMethods.BNFreeTypeLibraryList
		    );
	    }

	    /// <summary>
	    /// Gets the auto platform type ID source string for this platform.
	    /// </summary>
	    public string GetAutoPlatformTypeIdSource()
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNGetAutoPlatformTypeIdSource(this.handle)
		    );
	    }

	    /// <summary>
	    /// Generates an auto platform type ID for the given qualified name.
	    /// </summary>
	    public unsafe string GenerateAutoPlatformTypeId(QualifiedName name)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    BNQualifiedName nativeName = name.ToNativeEx(allocator);

			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNGenerateAutoPlatformTypeId(
					    this.handle ,
					    (IntPtr)(&nativeName)
				    )
			    );
		    }
	    }

	    /// <summary>
	    /// Generates an auto platform NamedTypeReference for the given qualified name and class.
	    /// Mirrors C++ <c>Platform::GenerateAutoPlatformTypeReference</c> / Python
	    /// <c>Platform.generate_auto_platform_type_ref</c>: builds the auto platform type ID for
	    /// the name, then constructs a NamedTypeReference from the class, ID, and name.
	    /// </summary>
	    public NamedTypeReference GenerateAutoPlatformTypeReference(
		    NamedTypeReferenceClass cls ,
		    QualifiedName name
	    )
	    {
		    string typeId = this.GenerateAutoPlatformTypeId(name);

		    return new NamedTypeReference(cls , typeId , name);
	    }

	    /// <summary>
	    /// Creates a new platform with types loaded from a type definition file.
	    /// </summary>
	    /// <param name="arch">The architecture for the new platform.</param>
	    /// <param name="name">The name of the new platform.</param>
	    /// <param name="typeFile">The path to the type definition file.</param>
	    /// <param name="includeDirs">An array of include directory paths for type resolution.</param>
	    /// <returns>A new Platform, or null if creation failed.</returns>
	    /// <summary>
	    /// Adjusts the type parser input arguments and source files for this platform.
	    /// The platform may add platform-specific compiler flags or preprocessor definitions.
	    /// </summary>
	    /// <param name="parser">The type parser that will process the adjusted input.</param>
	    /// <param name="argumentsIn">The input compiler arguments.</param>
	    /// <param name="sourceFileNamesIn">The input source file names.</param>
	    /// <param name="sourceFileValuesIn">The input source file contents.</param>
	    /// <param name="argumentsOut">Receives the adjusted compiler arguments.</param>
	    /// <param name="sourceFileNamesOut">Receives the adjusted source file names.</param>
	    /// <param name="sourceFileValuesOut">Receives the adjusted source file contents.</param>
	    public unsafe void AdjustTypeParserInput(
		    TypeParser parser ,
		    string[] argumentsIn ,
		    string[] sourceFileNamesIn ,
		    string[] sourceFileValuesIn ,
		    out string[] argumentsOut ,
		    out string[] sourceFileNamesOut ,
		    out string[] sourceFileValuesOut
	    )
	    {
		    // 1. Prepare safe arrays.
		    string[] safeArgs = argumentsIn ?? Array.Empty<string>();
		    string[] safeNames = sourceFileNamesIn ?? Array.Empty<string>();
		    string[] safeValues = sourceFileValuesIn ?? Array.Empty<string>();

		    // 2. Stack-allocate output pointers.
		    IntPtr argsOutPtr = IntPtr.Zero;
		    ulong argsOutLen = 0;
		    IntPtr namesOutPtr = IntPtr.Zero;
		    IntPtr valuesOutPtr = IntPtr.Zero;
		    ulong filesOutLen = 0;

		    // 3. Build the const char** input blocks as UTF-8 and call native.
		    //    argumentsIn has its own count; sourceFileNamesIn/sourceFileValuesIn
		    //    are parallel arrays sharing one sourceFilesLenIn count.
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr argsBlock = allocator.AllocUtf8StringArray(safeArgs);
			    IntPtr namesBlock = allocator.AllocUtf8StringArray(safeNames);
			    IntPtr valuesBlock = allocator.AllocUtf8StringArray(safeValues);

			    NativeMethods.BNPlatformAdjustTypeParserInput(
				    this.handle ,
				    parser.DangerousGetHandle() ,
				    argsBlock ,
				    (ulong)safeArgs.Length ,
				    namesBlock ,
				    valuesBlock ,
				    (ulong)safeNames.Length ,
				    (IntPtr)(&argsOutPtr) ,
				    (IntPtr)(&argsOutLen) ,
				    (IntPtr)(&namesOutPtr) ,
				    (IntPtr)(&valuesOutPtr) ,
				    (IntPtr)(&filesOutLen)
			    );
		    }

		    // 4. Marshal the output arrays.
		    argumentsOut = UnsafeUtils.TakeStringArrayEx(argsOutPtr , argsOutLen);
		    sourceFileNamesOut = UnsafeUtils.TakeStringArrayEx(namesOutPtr , filesOutLen);
		    sourceFileValuesOut = UnsafeUtils.TakeStringArrayEx(valuesOutPtr , filesOutLen);
	    }

	    /// <summary>
	    /// Parses C/C++ source code using this platform's type system.
	    /// Returns the parsed types, variables, and functions on success,
	    /// or null with an error message on failure.
	    /// </summary>
	    /// <param name="source">The C/C++ source text to parse.</param>
	    /// <param name="fileName">The filename to use for diagnostics.</param>
	    /// <param name="includeDirs">Include directory paths for header resolution.</param>
	    /// <param name="autoTypeSource">The auto type source identifier string.</param>
	    /// <param name="error">Receives any error message from the parser.</param>
	    /// <returns>A TypeParserResult on success, or null on failure.</returns>
	    public unsafe TypeParserResult? ParseTypesFromSource(
		    string source ,
		    string fileName ,
		    string[] includeDirs ,
		    string autoTypeSource ,
		    out string error
	    )
	    {
		    // 1. Normalize the include-dir input.
		    string[] safeDirs = includeDirs ?? Array.Empty<string>();

		    // 2. Stack-allocate the result struct.
		    BNTypeParserResult rawResult = new BNTypeParserResult();

		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // 3. Build the const char** include-dir block as UTF-8.
			    IntPtr includeDirsBlock = allocator.AllocUtf8StringArray(safeDirs);

			    // 4. Call the native API. `errors` is an out char*: the core allocates
			    //    a single error string on failure and leaves it null on success.
			    IntPtr errorsPointer;
			    bool ok = NativeMethods.BNParseTypesFromSource(
				    this.handle ,
				    source ,
				    fileName ,
				    (IntPtr)(&rawResult) ,
				    out errorsPointer ,
				    includeDirsBlock ,
				    (ulong)safeDirs.Length ,
				    autoTypeSource ?? string.Empty
			    );

			    // 5. Decode + free the core-allocated error string (no-op on null).
			    error = UnsafeUtils.TakeUtf8String(errorsPointer);

			    // 6. On success, convert the result and free the native struct.
			    if (ok)
			    {
				    return TypeParserResult.TakeNative(rawResult);
			    }

			    return null;
		    }
	    }

	    /// <summary>
	    /// Parses types from a C/C++ source file using this platform's type system.
	    /// Returns the parsed types, variables, and functions on success,
	    /// or null with an error message on failure.
	    /// </summary>
	    /// <param name="fileName">The path to the source file to parse.</param>
	    /// <param name="includeDirs">Include directory paths for header resolution.</param>
	    /// <param name="autoTypeSource">The auto type source identifier string.</param>
	    /// <param name="error">Receives any error message from the parser.</param>
	    /// <returns>A TypeParserResult on success, or null on failure.</returns>
	    public unsafe TypeParserResult? ParseTypesFromSourceFile(
		    string fileName ,
		    string[] includeDirs ,
		    string autoTypeSource ,
		    out string error
	    )
	    {
		    // 1. Normalize the include-dir input.
		    string[] safeDirs = includeDirs ?? Array.Empty<string>();

		    // 2. Stack-allocate the result struct.
		    BNTypeParserResult rawResult = new BNTypeParserResult();

		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // 3. Build the const char** include-dir block as UTF-8.
			    IntPtr includeDirsBlock = allocator.AllocUtf8StringArray(safeDirs);

			    // 4. Call the native API. `errors` is an out char*: the core allocates
			    //    a single error string on failure and leaves it null on success.
			    IntPtr errorsPointer;
			    bool ok = NativeMethods.BNParseTypesFromSourceFile(
				    this.handle ,
				    fileName ,
				    (IntPtr)(&rawResult) ,
				    out errorsPointer ,
				    includeDirsBlock ,
				    (ulong)safeDirs.Length ,
				    autoTypeSource ?? string.Empty
			    );

			    // 5. Decode + free the core-allocated error string (no-op on null).
			    error = UnsafeUtils.TakeUtf8String(errorsPointer);

			    // 6. On success, convert the result and free the native struct.
			    if (ok)
			    {
				    return TypeParserResult.TakeNative(rawResult);
			    }

			    return null;
		    }
	    }

	    public static Platform? CreateWithTypes(
		    Architecture arch ,
		    string name ,
		    string typeFile ,
		    string[] includeDirs
	    )
	    {
		    // 1. Prepare the include directories array.
		    string[] safeIncludeDirs = includeDirs ?? Array.Empty<string>();

		    // 2. Build the const char** include-dir block as UTF-8 and call native.
		    IntPtr result;
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr includeDirsBlock = allocator.AllocUtf8StringArray(safeIncludeDirs);

			    result = NativeMethods.BNCreatePlatformWithTypes(
				    arch.DangerousGetHandle() ,
				    name ,
				    typeFile ,
				    includeDirsBlock ,
				    (ulong)safeIncludeDirs.Length
			    );
		    }

		    // 3. Return the wrapped handle, or null on failure.
		    return Platform.TakeHandle(result);
	    }
	}

}