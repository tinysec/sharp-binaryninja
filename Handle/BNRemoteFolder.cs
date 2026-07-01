using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class RemoteFolder : AbstractSafeHandle<RemoteFolder>
	{
	    internal RemoteFolder(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }

	    internal static RemoteFolder? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new RemoteFolder(
			    NativeMethods.BNNewRemoteFolderReference(handle) ,
			    true
		    );
	    }
	    
	    internal static RemoteFolder MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new RemoteFolder(
			    NativeMethods.BNNewRemoteFolderReference(handle) ,
			    true
		    );
	    }
	    
	    internal static RemoteFolder? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new RemoteFolder(handle, true);
	    }
	    
	    internal static RemoteFolder MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new RemoteFolder(handle, true);
	    }
	    
	    internal static RemoteFolder? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new RemoteFolder(handle, false);
	    }
	    
	    internal static RemoteFolder MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new RemoteFolder(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeRemoteFolder(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }
	}
}