using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class FieldResolutionInfo : AbstractSafeHandle<FieldResolutionInfo>
	{
	    public FieldResolutionInfo(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }
	    
	    internal static FieldResolutionInfo? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new FieldResolutionInfo(
			    NativeMethods.BNNewFieldResolutionInfoReference(handle) ,
			    true
		    );
	    }
	    
	    internal static FieldResolutionInfo MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new FieldResolutionInfo(
			    NativeMethods.BNNewFieldResolutionInfoReference(handle) ,
			    true
		    );
	    }
	    
	    internal static FieldResolutionInfo? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new FieldResolutionInfo(handle, true);
	    }
	    
	    internal static FieldResolutionInfo MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new FieldResolutionInfo(handle, true);
	    }
	    
	    internal static FieldResolutionInfo? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new FieldResolutionInfo(handle, false);
	    }
	    
	    internal static FieldResolutionInfo MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new FieldResolutionInfo(handle, false);
	    }

	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeFieldResolutionInfo(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }
	}
}