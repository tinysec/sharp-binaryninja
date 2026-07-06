using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class TypeContainer : AbstractSafeHandle<TypeContainer>
	{
		public TypeContainer() 
			: this( NativeMethods.BNGetEmptyTypeContainer() , true)
		{
			
		}
		
	    internal TypeContainer(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
			
	    }
	    
	    internal static TypeContainer MustDuplicateFromHandle(IntPtr handle)
	    {
		    return new TypeContainer(
			    NativeMethods.BNDuplicateTypeContainer(handle) ,
			    true
		    );
	    }

	    
	    internal static TypeContainer? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new TypeContainer(handle, true);
	    }
	    
	    internal static TypeContainer MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new TypeContainer(handle, true);
	    }
	    
	    internal static TypeContainer? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new TypeContainer(handle, false);
	    }
	    
	    internal static TypeContainer MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new TypeContainer(handle, false);
	    }

	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeTypeContainer(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }

	    public string Id
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(NativeMethods.BNTypeContainerGetId(this.handle));
		    }
	    }
	    
	    public string Name
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(NativeMethods.BNTypeContainerGetName(this.handle));
		    }
	    }
	    
	    public TypeContainerType Type
	    {
		    get
		    {
			    return NativeMethods.BNTypeContainerGetType(this.handle);
		    }
	    }
	    
	    public bool Mutable
	    {
		    get
		    {
			    return NativeMethods.BNTypeContainerIsMutable(this.handle);
		    }
	    }

	    /// <summary>
	    /// The number of types in this container. Mirrors Python TypeContainer.type_count.
	    /// </summary>
	    public ulong TypeCount
	    {
		    get
		    {
			    return (ulong)NativeMethods.BNTypeContainerGetTypeCount(this.handle);
		    }
	    }

	    public Platform Platform
	    {
		    get
		    {
			    IntPtr raw = NativeMethods.BNTypeContainerGetPlatform(this.handle);

			    if (IntPtr.Zero == raw)
			    {
				    throw new Exception("Unable to get platform");
			    }

			    return new Platform(raw , true);
		    }
	    }

	    public QualifiedNameAndId[] AddTypes(
		    QualifiedNameAndType[] types,
		    ProgressDelegate? progress = null
		)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    QualifiedName[] typeNames = QualifiedNameAndType.PickNames(types);
			    
			    bool ok = NativeMethods.BNTypeContainerAddTypes(
				    this.handle,
				    allocator.ConvertToNativeArrayEx<BNQualifiedName,QualifiedName>(typeNames),
				    allocator.AllocHandleArray<BinaryNinja.Type>(QualifiedNameAndType.PickTypes(types)) ,
				    (ulong)typeNames.Length,
				    null == progress ? IntPtr.Zero :
					    Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(
					    UnsafeUtils.WrapProgressDelegate(progress)
					),
				    IntPtr.Zero, 
				    out IntPtr resultNamesPointer,
				    out IntPtr resultIdsPointer,
				    out ulong resultCount  
			    );
			    
			    List<QualifiedNameAndId> results = new List<QualifiedNameAndId>();

			    if (ok)
			    {
				    QualifiedName[] resultNames = UnsafeUtils.TakeStructArrayEx<BNQualifiedName , QualifiedName>(
					    resultNamesPointer ,
					    resultCount ,
					    QualifiedName.FromNative ,
					    NativeMethods.BNFreeTypeNameList
				    );

				    string[] resultIds = UnsafeUtils.TakeAnsiStringArray(
					    resultIdsPointer ,
					    resultCount,
					    NativeMethods.BNFreeStringList
				    );

				    for (ulong i = 0; i < resultCount; i++)
				    {
					    results.Add( new QualifiedNameAndId(
						    resultNames[i],
						    resultIds[i]
						));
				    }
			    }

			    return results.ToArray();
		    }
	    }

	    public bool RenameType(string typeId , QualifiedName newName)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    return NativeMethods.BNTypeContainerRenameType(
				    this.handle, 
				    typeId, 
				    newName.ToNativeEx(allocator) 
				);
		    }
	    }
	    
	    public bool DeleteType(string typeId)
	    {
		    return NativeMethods.BNTypeContainerDeleteType(this.handle, typeId );
	    }
	    
	    public string GetTypeId(QualifiedName typeName)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    bool ok = NativeMethods.BNTypeContainerGetTypeId(
				    this.handle, 
				    typeName.ToNativeEx(allocator) , 
				    out IntPtr typeIdPointer
				);

			    if (!ok)
			    {
				    return String.Empty;
			    }
			    
			    return UnsafeUtils.TakeAnsiString(typeIdPointer);
		    }
	    }
	    
	    public QualifiedName? GetTypeName(string typeId)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    bool ok = NativeMethods.BNTypeContainerGetTypeName(
				    this.handle, 
				    typeId , 
				    out BNQualifiedName rawTypeName
			    );

			    if (!ok)
			    {
				    return null;
			    }
			    
			    return QualifiedName.FromNative(rawTypeName);
		    }
	    }
	    
	    public BinaryNinja.Type? GetTypeById(string typeId)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    bool ok = NativeMethods.BNTypeContainerGetTypeById(
				    this.handle, 
				    typeId , 
				    out IntPtr rawType
			    );

			    if (!ok)
			    {
				    return null;
			    }
			    
			    return new BinaryNinja.Type(rawType , true);
		    }
	    }
	    
	    // QualifiedNameTypeAndId
	    public QualifiedNameTypeAndId[] GetTypes()
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    bool ok = NativeMethods.BNTypeContainerGetTypes(
				    this.handle,
				    out IntPtr typeIdsPointer,
				    out IntPtr typeNamesPointer,
				    out IntPtr typesPointer, 
				    out ulong typeCount  
			    );
	    
			    List<QualifiedNameTypeAndId> results = new List<QualifiedNameTypeAndId>();

			    if (ok)
			    {
				    QualifiedName[] typeNames = UnsafeUtils.TakeStructArrayEx<BNQualifiedName , QualifiedName>(
					    typeNamesPointer ,
					    typeCount ,
					    QualifiedName.FromNative ,
					    NativeMethods.BNFreeTypeNameList
				    );

				    string[] typeIds = UnsafeUtils.TakeAnsiStringArray(
					    typeIdsPointer ,
					    typeCount,
					    NativeMethods.BNFreeStringList
				    );
			    
				    BinaryNinja.Type[] types = UnsafeUtils.TakeHandleArrayEx<BinaryNinja.Type>(
					    typesPointer ,
					    typeCount ,
					    BinaryNinja.Type.MustNewFromHandle,
					    NativeMethods.BNFreeTypeList
				    );

				    for (ulong i = 0; i < typeCount; i++)
				    {
					    results.Add( new QualifiedNameTypeAndId(
						    typeNames[i],
						    types[i],
						    typeIds[i]
						));
				    }
			    }
			    
			    return results.ToArray();
		    }
	    }


	    public BinaryNinja.Type? GetTypeByName(QualifiedName typeName)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    bool ok = NativeMethods.BNTypeContainerGetTypeByName(
				    this.handle,  
				    typeName.ToNativeEx(allocator),
				    out IntPtr pointer
				);

			    if (!ok)
			    {
				    return null;
			    }
			    
			    return BinaryNinja.Type.TakeHandle(pointer);
		    }
	    }

	    public string[] TypeIds
	    {
		    get
		    {
			    bool ok = NativeMethods.BNTypeContainerGetTypeIds(
				    this.handle,
				    out IntPtr typeIdsPointer,
				    out ulong typeCount
				);

			    if (!ok)
			    {
				    return Array.Empty<string>();
			    }

			    return UnsafeUtils.TakeAnsiStringArray(
				    typeIdsPointer ,
				    typeCount,
				    NativeMethods.BNFreeStringList
				    );
		    }
	    }
	    
	    public QualifiedName[] TypeNames
	    {
		    get
		    {
			    bool ok = NativeMethods.BNTypeContainerGetTypeNames(
				    this.handle,
				    out IntPtr typeNamesPointer,
				    out ulong typeCount
			    );

			    if (!ok)
			    {
				    return Array.Empty<QualifiedName>();
			    }

			    return UnsafeUtils.TakeStructArrayEx<BNQualifiedName,QualifiedName>(
				    typeNamesPointer , 
				    typeCount,
				    QualifiedName.FromNative,
				    NativeMethods.BNFreeTypeNameList
				);
		    }
	    }
	    
	    public QualifiedNameAndId[] TypeNameAndIds
	    {
		    get
		    {
			    bool ok = NativeMethods.BNTypeContainerGetTypeNamesAndIds(
				    this.handle,
				    out IntPtr typeIdsPointer,
				    out IntPtr typeNamesPointer,
				    out ulong typeCount
			    );

			    List<QualifiedNameAndId> results = new List<QualifiedNameAndId>();
			    
			    if (ok)
			    {
				    QualifiedName[] typeNames;

				    string[] typeIds;
				    
				    typeIds = UnsafeUtils.TakeAnsiStringArray(
					    typeIdsPointer , 
					    typeCount,
					    NativeMethods.BNFreeStringList
					    );
		    
				    typeNames = UnsafeUtils.TakeStructArrayEx<BNQualifiedName,QualifiedName>(
					    typeNamesPointer , 
					    typeCount,
					    QualifiedName.FromNative,
					    NativeMethods.BNFreeTypeNameList
				    );

				    for (ulong i = 0; i < typeCount; i++)
				    {
					    results.Add( new QualifiedNameAndId(
						    typeNames[i],
						    typeIds[i]
						));
				    }
			    }
				
			    return results.ToArray();
		    }
	    }

	    public QualifiedNameAndType? ParseTypeString(
		    string source ,
		    bool importDepencencies,
		    out TypeParserError[] errors
		)
	    {
		    bool ok =  NativeMethods.BNTypeContainerParseTypeString(
			    this.handle,
			    source,
			    importDepencencies,
			    out BNQualifiedNameAndType rawNameAndType,
			    out IntPtr errorsPointer,
			    out ulong errorCount
			);

		    if (ok)
		    {
			    errors = Array.Empty<TypeParserError>();
			    
			    return QualifiedNameAndType.FromNative(rawNameAndType);
		    }
		    else
		    {
			    errors = UnsafeUtils.TakeStructArrayEx<BNTypeParserError , TypeParserError>(
				    errorsPointer ,
				    errorCount ,
				    TypeParserError.FromNative ,
				    NativeMethods.BNFreeTypeParserErrors
			    );

			    return null;
		    }
	    }

	    /// <summary>
	    /// Parses C/C++ source code within this type container and returns the parsed types,
	    /// variables, and functions. On failure, returns null and populates the errors array.
	    /// </summary>
	    /// <param name="source">The C/C++ source text to parse.</param>
	    /// <param name="fileName">The filename to use for diagnostics.</param>
	    /// <param name="options">Compiler options to pass to the parser.</param>
	    /// <param name="includeDirs">Include directory paths for header resolution.</param>
	    /// <param name="autoTypeSource">The auto type source identifier string.</param>
	    /// <param name="importDependencies">Whether to import type dependencies.</param>
	    /// <param name="errors">Receives any parse errors on failure.</param>
	    /// <returns>A TypeParserResult on success, or null on failure.</returns>
	    public unsafe TypeParserResult? ParseTypesFromSource(
		    string source ,
		    string fileName ,
		    string[] options ,
		    string[] includeDirs ,
		    string autoTypeSource ,
		    bool importDependencies ,
		    out TypeParserError[] errors
	    )
	    {
		    // 1. Prepare safe arrays.
		    string[] safeOptions = options ?? Array.Empty<string>();
		    string[] safeDirs = includeDirs ?? Array.Empty<string>();

		    // 2. Stack-allocate the result struct and error output pointers.
		    BNTypeParserResult rawResult = new BNTypeParserResult();
		    IntPtr errorsPtr = IntPtr.Zero;
		    ulong errorCount = 0;

		    // 3. Call the native API. options/includeDirs are const char** UTF-8
		    // input blocks; build them by hand because .NET cannot apply LPUTF8Str
		    // to string[] array elements (non-ASCII would otherwise corrupt
		    // through the system ANSI code page).
		    bool ok;
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr optionsBlock = allocator.AllocUtf8StringArray(safeOptions);
			    IntPtr dirsBlock = allocator.AllocUtf8StringArray(safeDirs);

			    ok = NativeMethods.BNTypeContainerParseTypesFromSource(
				    this.handle ,
				    source ,
				    fileName ,
				    optionsBlock ,
				    (ulong)safeOptions.Length ,
				    dirsBlock ,
				    (ulong)safeDirs.Length ,
				    autoTypeSource ?? string.Empty ,
				    importDependencies ,
				    (IntPtr)(&rawResult) ,
				    (IntPtr)(&errorsPtr) ,
				    (IntPtr)(&errorCount)
			    );
		    }

		    // 4. On success, convert the result and free the native struct.
		    if (ok)
		    {
			    errors = Array.Empty<TypeParserError>();

			    return TypeParserResult.TakeNative(rawResult);
		    }

		    // 5. On failure, convert the error array.
		    errors = UnsafeUtils.TakeStructArrayEx<BNTypeParserError , TypeParserError>(
			    errorsPtr ,
			    errorCount ,
			    TypeParserError.FromNative ,
			    NativeMethods.BNFreeTypeParserErrors
		    );

		    return null;
	    }

	}
}