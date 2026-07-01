using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class RemoteProject : AbstractSafeHandle<RemoteProject>
	{
	    internal RemoteProject(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
		    
	    }

	    internal static RemoteProject? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new RemoteProject(
			    NativeMethods.BNNewRemoteProjectReference(handle) ,
			    true
		    );
	    }
	    
	    internal static RemoteProject MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new RemoteProject(
			    NativeMethods.BNNewRemoteProjectReference(handle) ,
			    true
		    );
	    }
	    
	    internal static RemoteProject? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new RemoteProject(handle, true);
	    }
	    
	    internal static RemoteProject MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new RemoteProject(handle, true);
	    }
	    
	    internal static RemoteProject? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new RemoteProject(handle, false);
	    }
	    
	    internal static RemoteProject MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new RemoteProject(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeRemoteProject(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }
	}
}