using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class Remote :  AbstractSafeHandle<Remote>
	{
	    internal Remote(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }

	    internal static Remote? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Remote(
			    NativeMethods.BNNewRemoteReference(handle) ,
			    true
		    );
	    }
	    
	    internal static Remote MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Remote(
			    NativeMethods.BNNewRemoteReference(handle) ,
			    true
		    );
	    }
	    
	    internal static Remote? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Remote(handle, true);
	    }
	    
	    internal static Remote MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Remote(handle, true);
	    }
	    
	    internal static Remote? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Remote(handle, false);
	    }
	    
	    internal static Remote MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Remote(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeRemote(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }
	}
}