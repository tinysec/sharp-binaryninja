using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class CollaborationSnapshot : AbstractSafeHandle<CollaborationSnapshot>
	{
	    public CollaborationSnapshot(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }

	    
	    internal static CollaborationSnapshot? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new CollaborationSnapshot(
			    NativeMethods.BNNewCollaborationSnapshotReference(handle) ,
			    true
		    );
	    }
	    
	    internal static CollaborationSnapshot MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new CollaborationSnapshot(
			    NativeMethods.BNNewCollaborationSnapshotReference(handle) ,
			    true
		    );
	    }
	    
	    internal static CollaborationSnapshot? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new CollaborationSnapshot(handle, true);
	    }
	    
	    internal static CollaborationSnapshot MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new CollaborationSnapshot(handle, true);
	    }
	    
	    internal static CollaborationSnapshot? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new CollaborationSnapshot(handle, false);
	    }
	    
	    internal static CollaborationSnapshot MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new CollaborationSnapshot(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeCollaborationSnapshot(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }
	}
}