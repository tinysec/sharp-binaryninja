using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class CollaborationChangeset : AbstractSafeHandle<CollaborationChangeset>
	{
	    internal CollaborationChangeset(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }

	    
	    internal static CollaborationChangeset? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new CollaborationChangeset(
			    NativeMethods.BNNewCollaborationChangesetReference(handle) ,
			    true
		    );
	    }
	    
	    internal static CollaborationChangeset MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new CollaborationChangeset(
			    NativeMethods.BNNewCollaborationChangesetReference(handle) ,
			    true
		    );
	    }
	    
	    internal static CollaborationChangeset? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new CollaborationChangeset(handle, true);
	    }
	    
	    internal static CollaborationChangeset MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new CollaborationChangeset(handle, true);
	    }
	    
	    internal static CollaborationChangeset? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new CollaborationChangeset(handle, false);
	    }
	    
	    internal static CollaborationChangeset MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new CollaborationChangeset(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeCollaborationChangeset(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }
	}
}