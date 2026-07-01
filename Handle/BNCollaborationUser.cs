using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class CollaborationUser : AbstractSafeHandle<CollaborationUser>
	{
	    internal CollaborationUser(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }
	    
	    internal static CollaborationUser? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new CollaborationUser(
			    NativeMethods.BNNewCollaborationUserReference(handle) ,
			    true
		    );
	    }
	    
	    internal static CollaborationUser MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new CollaborationUser(
			    NativeMethods.BNNewCollaborationUserReference(handle) ,
			    true
		    );
	    }
	    
	    internal static CollaborationUser? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new CollaborationUser(handle, true);
	    }
	    
	    internal static CollaborationUser MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new CollaborationUser(handle, true);
	    }
	    
	    internal static CollaborationUser? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new CollaborationUser(handle, false);
	    }
	    
	    internal static CollaborationUser MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new CollaborationUser(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeCollaborationUser(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }
	}
}