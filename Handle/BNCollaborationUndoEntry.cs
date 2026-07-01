using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class CollaborationUndoEntry : AbstractSafeHandle<CollaborationUndoEntry>
	{
	    public CollaborationUndoEntry(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	       
	    }
	    
	    internal static CollaborationUndoEntry? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new CollaborationUndoEntry(
			    NativeMethods.BNNewCollaborationUndoEntryReference(handle) ,
			    true
		    );
	    }
	    
	    internal static CollaborationUndoEntry MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new CollaborationUndoEntry(
			    NativeMethods.BNNewCollaborationUndoEntryReference(handle) ,
			    true
		    );
	    }
	    
	    internal static CollaborationUndoEntry? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new CollaborationUndoEntry(handle, true);
	    }
	    
	    internal static CollaborationUndoEntry MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new CollaborationUndoEntry(handle, true);
	    }
	    
	    internal static CollaborationUndoEntry? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new CollaborationUndoEntry(handle, false);
	    }
	    
	    internal static CollaborationUndoEntry MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new CollaborationUndoEntry(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeCollaborationUndoEntry(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }
	}
}