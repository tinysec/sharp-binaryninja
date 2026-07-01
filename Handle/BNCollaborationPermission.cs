using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class CollaborationPermission : AbstractSafeHandle<CollaborationPermission>
	{
	    public CollaborationPermission(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	       
	    }

	    
	    internal static CollaborationPermission? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new CollaborationPermission(
			    NativeMethods.BNNewCollaborationPermissionReference(handle) ,
			    true
		    );
	    }
	    
	    internal static CollaborationPermission MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new CollaborationPermission(
			    NativeMethods.BNNewCollaborationPermissionReference(handle) ,
			    true
		    );
	    }
	    
	    internal static CollaborationPermission? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new CollaborationPermission(handle, true);
	    }
	    
	    internal static CollaborationPermission MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new CollaborationPermission(handle, true);
	    }
	    
	    internal static CollaborationPermission? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new CollaborationPermission(handle, false);
	    }
	    
	    internal static CollaborationPermission MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new CollaborationPermission(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeCollaborationPermission(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }
	}
}