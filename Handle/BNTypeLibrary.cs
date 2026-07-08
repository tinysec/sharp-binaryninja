using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class TypeLibrary :  AbstractSafeHandle<TypeLibrary>
	{
		public TypeLibrary(Architecture arch , string name) 
			:this( NativeMethods.BNNewTypeLibrary(arch.DangerousGetHandle(), name) , true)
		{
			
		}
		
	    internal TypeLibrary(IntPtr handle , bool owner)
		    : base(handle , owner)
	    {
	        
	    }
	    
	    internal static TypeLibrary? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new TypeLibrary(
			    NativeMethods.BNNewTypeLibraryReference(handle) ,
			    true
		    );
	    }
	    
	    internal static TypeLibrary MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new TypeLibrary(
			    NativeMethods.BNNewTypeLibraryReference(handle) ,
			    true
		    );
	    }
	    
	    internal static TypeLibrary? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new TypeLibrary(handle, true);
	    }
	    
	    internal static TypeLibrary MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new TypeLibrary(handle, true);
	    }
	    
	    internal static TypeLibrary? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new TypeLibrary(handle, false);
	    }
	    
	    internal static TypeLibrary MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new TypeLibrary(handle, false);
	    }
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeTypeLibrary(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }

	    public static bool DecompressToFile(string filename , string outdir)
	    {
		    return NativeMethods.BNTypeLibraryDecompressToFile(filename , outdir);
	    }

	    public static TypeLibrary? LoadFromFile(string filename)
	    {
		    IntPtr raw = NativeMethods.BNLoadTypeLibraryFromFile(filename);

		    if (IntPtr.Zero == raw)
		    {
			    return null;
		    }
		    
		    return new TypeLibrary(raw, true);
	    }

	    public static TypeLibrary? LookupByName(Architecture arch ,string name)
	    {
		    IntPtr raw = NativeMethods.BNLookupTypeLibraryByName(arch.DangerousGetHandle() ,name);

		    if (IntPtr.Zero == raw)
		    {
			    return null;
		    }

		    return new TypeLibrary(raw, true);
	    }

	    /// <summary>
	    /// Looks up a registered type library by its GUID on the given architecture, mirroring
	    /// Python <c>TypeLibrary.lookup_by_guid</c>. Returns <c>null</c> when no library with that
	    /// GUID is registered.
	    /// </summary>
	    /// <param name="arch">The architecture whose type libraries to search.</param>
	    /// <param name="guid">The GUID string to match.</param>
	    /// <returns>The matching TypeLibrary, or <c>null</c> if none is registered.</returns>
	    public static TypeLibrary? LookupByGuid(Architecture arch, string guid)
	    {
		    IntPtr raw = NativeMethods.BNLookupTypeLibraryByGuid(arch.DangerousGetHandle(), guid);

		    if (IntPtr.Zero == raw)
		    {
			    return null;
		    }

		    return new TypeLibrary(raw, true);
	    }

	    /// <summary>
	    /// Creates a new type library instance with a fresh GUID and the same data as this one,
	    /// mirroring Python <c>TypeLibrary.duplicate</c> (typelibrary.py:142).
	    /// </summary>
	    /// <returns>An independent copy of this type library.</returns>
	    public TypeLibrary Duplicate()
	    {
		    return TypeLibrary.MustTakeHandle(
			    NativeMethods.BNDuplicateTypeLibrary(this.handle)
		    );
	    }

	    /// <summary>
	    /// Makes this created or loaded type library available for platforms to use when loading
	    /// binaries, mirroring Python <c>TypeLibrary.register</c> (typelibrary.py:244). Python exposes
	    /// this as a property; the C# binding exposes it as a method since it performs an action with
	    /// no meaningful return value.
	    /// </summary>
	    public void Register()
	    {
		    NativeMethods.BNRegisterTypeLibrary(this.handle);
	    }
	    
	    public bool WriteToFile(string filename)
	    {
		    return NativeMethods.BNWriteTypeLibraryToFile(this.handle , filename);
	    }

	    public Architecture Architecture
	    {
		    get
		    {
			    return Architecture.MustFromHandle(
				    NativeMethods.BNGetTypeLibraryArchitecture(this.handle)
			    );
		    }
	    }

	    public string Name
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(NativeMethods.BNGetTypeLibraryName(this.handle));
		    }

		    set
		    {
			    NativeMethods.BNSetTypeLibraryName(this.handle, value);
		    }
	    }
	    
	    public string DependencyName
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(NativeMethods.BNGetTypeLibraryDependencyName(this.handle));
		    }

		    set
		    {
			    NativeMethods.BNSetTypeLibraryDependencyName(this.handle, value);
		    }
	    }
	    
	    public string Guid
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(NativeMethods.BNGetTypeLibraryGuid(this.handle));
		    }

		    set
		    {
			    NativeMethods.BNSetTypeLibraryGuid(this.handle, value);
		    }
	    }
	    
	    public string[] AlternateNames
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetTypeLibraryAlternateNames(
				    this.handle,
				    out ulong arrayLength
				 );
			    
			    return UnsafeUtils.TakeAnsiStringArray(
				    arrayPointer,
				    arrayLength,
				    NativeMethods.BNFreeStringList
				    );
		    }
	    }

	    public void AddAlternateName(string alternateName)
	    {
		    NativeMethods.BNAddTypeLibraryAlternateName(this.handle, alternateName);
	    }
	    
	    public string[] Platforms
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetTypeLibraryPlatforms(
				    this.handle,
				    out ulong arrayLength
			    );
			    
			    return UnsafeUtils.TakeAnsiStringArray(
				    arrayPointer,
				    arrayLength,
				    NativeMethods.BNFreeStringList
				    );
		    }
	    }
	    
	    public void AddPlatform(Platform platform)
	    {
		    NativeMethods.BNAddTypeLibraryPlatform(this.handle, platform.DangerousGetHandle());
	    }

	    public void ClearPlatforms(Platform platform)
	    {
		    NativeMethods.BNClearTypeLibraryPlatforms(this.handle);
	    }
	    
	    public bool Build()
	    {
		    return NativeMethods.BNFinalizeTypeLibrary(this.handle);
	    }

	    public Metadata? Metadata
	    {
		    get
		    {
			    IntPtr raw = NativeMethods.BNTypeLibraryGetMetadata(this.handle);

			    if (IntPtr.Zero == raw)
			    {
				    return null;
			    }
		    
			    return new Metadata(raw, true);
		    }
	    }
	    
	    public Metadata? QueryMetadata(string key)
	    {
		    IntPtr raw = NativeMethods.BNTypeLibraryQueryMetadata(this.handle , key);

		    if (IntPtr.Zero == raw)
		    {
			    return null;
		    }
		    
		    return new Metadata(raw, true);
	    }
	    
	    public void StoreMetadata(string key , Metadata data)
	    {
		    NativeMethods.BNTypeLibraryStoreMetadata(this.handle , key , data.DangerousGetHandle() );
	    }
	    
	    public void RemoveMetadata(string key )
	    {
		    NativeMethods.BNTypeLibraryRemoveMetadata(this.handle , key );
	    }
	    
	    public TypeContainer TypeContainer
	    {
		    get
		    {
			    IntPtr raw = NativeMethods.BNGetTypeLibraryTypeContainer(this.handle);

			    if (IntPtr.Zero == raw)
			    {
				    throw new  Exception("Failed to get TypeContainer");
			    }
		    
			    return new TypeContainer(raw, true);
		    }
	    }

	    public void AddNamedObject(QualifiedName name , BinaryNinja.Type kind)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    NativeMethods.BNAddTypeLibraryNamedObject(
				    this.handle , 
				    name.ToNativeEx(allocator) , 
				    kind.DangerousGetHandle()
				);
		    }
	    }
	    
	    public void AddNamedType(QualifiedName name , BinaryNinja.Type kind)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    NativeMethods.BNAddTypeLibraryNamedType(
				    this.handle ,
				    name.ToNativeEx(allocator) ,
				    kind.DangerousGetHandle()
			    );
		    }
	    }

	    /// <summary>
	    /// Removes a named object from this type library's object store, mirroring Python
	    /// <c>TypeLibrary.remove_named_object</c> (typelibrary.py:383). Only the object is removed;
	    /// any types it references are left in place.
	    /// </summary>
	    /// <param name="name">The qualified name of the object to remove.</param>
	    public void RemoveNamedObject(QualifiedName name)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // The Remove P/Invoke takes the name as a pointer (BNQualifiedName*), unlike Add which
			    // takes it by reference, so pin the converted struct and pass its address.
			    IntPtr namePointer = allocator.AllocStruct(name.ToNativeEx(allocator));

			    NativeMethods.BNRemoveTypeLibraryNamedObject(
				    this.handle ,
				    namePointer
			    );
		    }
	    }

	    /// <summary>
	    /// Removes a named type from this type library's type store, mirroring Python
	    /// <c>TypeLibrary.remove_named_type</c> (typelibrary.py:416). Only the type is removed; any
	    /// objects that reference it are left in place.
	    /// </summary>
	    /// <param name="name">The qualified name of the type to remove.</param>
	    public void RemoveNamedType(QualifiedName name)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    // The Remove P/Invoke takes the name as a pointer (BNQualifiedName*), unlike Add which
			    // takes it by reference, so pin the converted struct and pass its address.
			    IntPtr namePointer = allocator.AllocStruct(name.ToNativeEx(allocator));

			    NativeMethods.BNRemoveTypeLibraryNamedType(
				    this.handle ,
				    namePointer
			    );
		    }
	    }

	    public void AddTypeSource(QualifiedName name , string source)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    NativeMethods.BNAddTypeLibraryNamedTypeSource(
				    this.handle , 
				    name.ToNativeEx(allocator) , 
				    source
			    );
		    }
	    }
	    
	    public BinaryNinja.Type? GetNamedObject(QualifiedName name)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr raw = NativeMethods.BNGetTypeLibraryNamedObject(
				    this.handle , 
				    name.ToNativeEx(allocator) 
			    );

			    if (IntPtr.Zero == raw)
			    {
				    return null;
			    }

			    return new BinaryNinja.Type(raw , true);
		    }
	    }
	    
	    public BinaryNinja.Type? GetNamedType(QualifiedName name)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr raw = NativeMethods.BNGetTypeLibraryNamedType(
				    this.handle , 
				    name.ToNativeEx(allocator) 
			    );

			    if (IntPtr.Zero == raw)
			    {
				    return null;
			    }

			    return new BinaryNinja.Type(raw , true);
		    }
	    }
	    
	    public QualifiedNameAndType[] NamedObjects
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetTypeLibraryNamedObjects(
				    this.handle ,
				    out ulong arrayLength
				);

			    return UnsafeUtils.TakeStructArrayEx<BNQualifiedNameAndType , QualifiedNameAndType>(
				    arrayPointer ,
				    arrayLength ,
				    QualifiedNameAndType.FromNative ,
				    NativeMethods.BNFreeQualifiedNameAndTypeArray
			    );
		    }
	    }
	    
	    public QualifiedNameAndType[] NamedTypes
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetTypeLibraryNamedTypes(
				    this.handle ,
				    out ulong arrayLength
			    );

			    return UnsafeUtils.TakeStructArrayEx<BNQualifiedNameAndType , QualifiedNameAndType>(
				    arrayPointer ,
				    arrayLength ,
				    QualifiedNameAndType.FromNative ,
				    NativeMethods.BNFreeQualifiedNameAndTypeArray
			    );
		    }
	    }
	    
	}
}