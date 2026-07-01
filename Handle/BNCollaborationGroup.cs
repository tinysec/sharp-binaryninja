using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class CollaborationGroup :  AbstractSafeHandle<CollaborationGroup>
	{
	    internal CollaborationGroup(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }
	    
	    internal static CollaborationGroup? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new CollaborationGroup(
			    NativeMethods.BNNewCollaborationGroupReference(handle) ,
			    true
		    );
	    }
	    
	    internal static CollaborationGroup MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new CollaborationGroup(
			    NativeMethods.BNNewCollaborationGroupReference(handle) ,
			    true
		    );
	    }
	    
	    internal static CollaborationGroup? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new CollaborationGroup(handle, true);
	    }
	    
	    internal static CollaborationGroup MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new CollaborationGroup(handle, true);
	    }
	    
	    internal static CollaborationGroup? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new CollaborationGroup(handle, false);
	    }
	    
	    internal static CollaborationGroup MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new CollaborationGroup(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeCollaborationGroup(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }
	}
}