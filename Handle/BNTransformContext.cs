using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class TransformContext : AbstractSafeHandle<TransformContext>
	{
	    internal TransformContext(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	       
	    }

	    internal static TransformContext? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new TransformContext(
			    NativeMethods.BNNewTransformContextReference(handle) ,
			    true
		    );
	    }
	    
	    internal static TransformContext MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new TransformContext(
			    NativeMethods.BNNewTransformContextReference(handle) ,
			    true
		    );
	    }
	    
	    internal static TransformContext? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new TransformContext(handle, true);
	    }
	    
	    internal static TransformContext MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new TransformContext(handle, true);
	    }
	    
	    internal static TransformContext? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new TransformContext(handle, false);
	    }
	    
	    internal static TransformContext MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new TransformContext(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeTransformContext(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }

	    public BinaryView? Input
	    {
		    get
		    {
			    return BinaryView.TakeHandle( 
				    NativeMethods.BNTransformContextGetInput(this.handle)
				);
		    }
	    }

	    public string FileName
	    {
		    get
		    {
			    return UnsafeUtils.TakeUtf8String(
				    NativeMethods.BNTransformContextGetFileName(this.handle)
			    );
		    }
	    }

	    public string TransformName
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNTransformContextGetTransformName(this.handle)
			    );
		    }

	    }

	    public TransformParameter[] Parameters
	    {
		    set
		    {
			    using (ScopedAllocator allocator = new ScopedAllocator())
			    {
				    NativeMethods.BNTransformContextSetTransformParameters(
					    this.handle, 
					    allocator.ConvertToNativeArrayEx<BNTransformParameter,TransformParameter>(
						    value
						),
					    (ulong)value.Length
				    );
			    }
		    }
	    }

	    public void SetParameter(string name , byte[] value)
	    {
		    NativeMethods.BNTransformContextSetTransformParameter(
			    this.handle ,
			    name ,
			    DataBuffer.FromBytes(value).DangerousGetHandle()
		    );
	    }

	    public bool HasParameter(string name)
	    {
		    return NativeMethods.BNTransformContextHasTransformParameter(this.handle, name);
	    }
	    
	    public void ClearParameter(string name)
	    {
		    NativeMethods.BNTransformContextClearTransformParameter(this.handle, name);
	    }
	    
	    public string ExtractionMessage
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNTransformContextGetExtractionMessage(this.handle)
			    );
		    }
	    }
	    
	    public TransformResult ExtractionResult
	    {
		    get
		    {
			    return NativeMethods.BNTransformContextGetExtractionResult(this.handle);
		    }
	    }
	    
	    public TransformResult TransformResult
	    {
		    get
		    {
			    return NativeMethods.BNTransformContextGetTransformResult(this.handle);
		    }
	    }

	    public Metadata? Metadata
	    {
		    get
		    {
			    return Metadata.TakeHandle(
				    NativeMethods.BNTransformContextGetMetadata(this.handle)
			    );
		    }
	    }

	    /// <summary>
	    /// The session-time settings overrides for this transform context. Mirrors Python
	    /// TransformContext.settings / C++ TransformContext::GetSettings().
	    /// </summary>
	    public Settings? Settings
	    {
		    get
		    {
			    return Settings.TakeHandle(
				    NativeMethods.BNTransformContextGetSettings(this.handle)
			    );
		    }
	    }

	    /// <summary>
	    /// The names of the transforms that can decode this context's input. Mirrors Python
	    /// TransformContext.available_transforms / C++ GetAvailableTransforms().
	    /// </summary>
	    public string[] GetAvailableTransforms()
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNTransformContextGetAvailableTransforms(
				    this.handle ,
				    countPtr
			    );

			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    return UnsafeUtils.TakeAnsiStringArray(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeStringList
			    );
		    }
	    }

	    public TransformContext? Parent
	    {
		    get
		    {
			    return TransformContext.TakeHandle(
				    NativeMethods.BNTransformContextGetParent(this.handle)
			    );
		    }
	    }
	    
	    public ulong ChildCount
	    {
		    get
		    {
			    return NativeMethods.BNTransformContextGetChildCount(this.handle);
		    }
	    }
	    
	    public TransformContext[] Children
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNTransformContextGetChildren(
				    this.handle ,
				    out ulong arrayLength
				);

			    return UnsafeUtils.TakeHandleArrayEx<TransformContext>(
				    arrayPointer ,
				    arrayLength ,
				    TransformContext.MustBorrowHandle,
				    NativeMethods.BNFreeTransformContextList
			    );
		    }
	    }

	    public bool IsLeaf
	    {
		    get
		    {
			    return NativeMethods.BNTransformContextIsLeaf(this.handle);
		    }
	    }
	    
	    public bool IsRoot
	    {
		    get
		    {
			    return NativeMethods.BNTransformContextIsRoot(this.handle);
		    }
	    }
	    
	    public bool IsDatabase
	    {
		    get
		    {
			    return NativeMethods.BNTransformContextIsDatabase(this.handle);
		    }
	    }

	    public string[] GetAvailableFiles()
	    {
		    IntPtr arrayPointer = NativeMethods.BNTransformContextGetAvailableFiles(
			    this.handle ,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeAnsiStringArray(
			    arrayPointer ,
			    arrayLength ,
			    NativeMethods.BNFreeStringList
		    );
	    }

	    public void SetAvailableFiles(string[] files)
	    {
		    NativeMethods.BNTransformContextSetAvailableFiles(
			    this.handle ,
			    files ,
			    (ulong)files.Length
		    );
	    }

	    public bool HasAvailableFiles
	    {
		    get
		    {
			    return NativeMethods.BNTransformContextHasAvailableFiles(this.handle);
		    }
	    }

	    public string[] GetRequestedFiles()
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNTransformContextGetRequestedFiles(
				    this.handle ,
				    countPtr
			    );

			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    return UnsafeUtils.TakeAnsiStringArray(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeStringList
			    );
		    }
	    }

	    public void SetRequestedFiles(string[] files)
	    {
		    NativeMethods.BNTransformContextSetRequestedFiles(
			    this.handle ,
			    files ,
			    (ulong)files.Length
		    );
	    }

	    public bool HasRequestedFiles
	    {
		    get
		    {
			    return NativeMethods.BNTransformContextHasRequestedFiles(this.handle);
		    }
	    }

	    public TransformContext? GetChild(string filename)
	    {
		    return TransformContext.TakeHandle(
			    NativeMethods.BNTransformContextGetChild(
				    this.handle ,
				    filename
			    )
		    );
	    }

	    public TransformContext? SetChild(DataBuffer data , string filename , TransformResult result , string message , bool filenameIsDescriptor = false)
	    {
		    return TransformContext.TakeHandle(
			    NativeMethods.BNTransformContextSetChild(
				    this.handle ,
				    data.DangerousGetHandle() ,
				    filename ,
				    result ,
				    message ,
				    filenameIsDescriptor
			    )
		    );
	    }
	}
}