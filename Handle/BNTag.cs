using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class Tag : AbstractSafeHandle<Tag>
	{
		public Tag(TagType kind , string data) 
			:this( NativeMethods.BNCreateTag(kind.DangerousGetHandle() , data) ,true)
		{
			
		}
		
	    internal Tag(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }
	    
	    internal static Tag? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Tag(
			    NativeMethods.BNNewTagReference(handle) ,
			    true
		    );
	    }
	    
	    internal static Tag MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Tag(
			    NativeMethods.BNNewTagReference(handle) ,
			    true
		    );
	    }
	    
	    internal static Tag? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Tag(handle, true);
	    }
	    
	    internal static Tag MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Tag(handle, true);
	    }
	    
	    internal static Tag? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Tag(handle, false);
	    }
	    
	    internal static Tag MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Tag(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeTag(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }
	    
	    public string Id
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNTagGetId(this.handle)
			    );
		    }
	    }
	    
	    public TagType Type
	    {
		    get
		    {
			    return TagType.MustTakeHandle(
				    NativeMethods.BNTagGetType(this.handle)
			    );
		    }
	    }
	    
	    public string Data
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNTagGetData(this.handle)
			    );
		    }

		    set
		    {
			    NativeMethods.BNTagSetData(this.handle, value);
		    }
	    }
	}
}