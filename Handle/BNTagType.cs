using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class TagType : AbstractSafeHandle<TagType>
	{
	    internal TagType(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }
	    
	    internal static TagType? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new TagType(
			    NativeMethods.BNNewTagTypeReference(handle) ,
			    true
		    );
	    }
	    
	    internal static TagType MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new TagType(
			    NativeMethods.BNNewTagTypeReference(handle) ,
			    true
		    );
	    }
	    
	    internal static TagType? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new TagType(handle, true);
	    }
	    
	    internal static TagType MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new TagType(handle, true);
	    }
	    
	    internal static TagType? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new TagType(handle, false);
	    }
	    
	    internal static TagType MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new TagType(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeTagType(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }


	    public string Id
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNTagTypeGetId(this.handle)
			    );
		    }
	    }
	    
	    public string Name
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNTagTypeGetName(this.handle)
			    );
		    }

		    set
		    {
			    NativeMethods.BNTagTypeSetName(this.handle, value);
		    }
	    }
	    
	    public string Icon
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNTagTypeGetIcon(this.handle)
			    );
		    }

		    set
		    {
			    NativeMethods.BNTagTypeSetIcon(this.handle, value);
		    }
	    }

	    public bool Visible
	    {
		    get
		    {
			    return NativeMethods.BNTagTypeGetVisible(this.handle);
		    }

		    set
		    {
			    NativeMethods.BNTagTypeSetVisible(this.handle, value);
		    }
	    }
	    
	    public TagTypeType Type
	    {
		    get
		    {
			    return NativeMethods.BNTagTypeGetType(this.handle);
		    }

		    set
		    {
			    NativeMethods.BNTagTypeSetType(this.handle, value);
		    }
	    }
	    
	    
	    /// <summary>
	    /// Gets the binary view that this tag type belongs to.
	    /// Returns null if no view is associated.
	    /// </summary>
	    public BinaryView? View
	    {
		    get
		    {
			    // Retrieve a new owned reference to the binary view from the native tag type.
			    return BinaryView.TakeHandle(
				    NativeMethods.BNTagTypeGetView(this.handle)
			    );
		    }
	    }

	    public Tag CreateTag(string data)
	    {
		    return Tag.MustTakeHandle(
			    NativeMethods.BNCreateTag(this.handle , data)
		    );
	    }
	}
}