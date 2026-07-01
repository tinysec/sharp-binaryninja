using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class ProjectFolder : AbstractSafeHandle<ProjectFolder>
	{
	    internal ProjectFolder(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }

	    internal static ProjectFolder? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new ProjectFolder(
			    NativeMethods.BNNewProjectFolderReference(handle) ,
			    true
		    );
	    }
	    
	    internal static ProjectFolder MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new ProjectFolder(
			    NativeMethods.BNNewProjectFolderReference(handle) ,
			    true
		    );
	    }
	    
	    internal static ProjectFolder? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new ProjectFolder(handle, true);
	    }
	    
	    internal static ProjectFolder MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new ProjectFolder(handle, true);
	    }
	    
	    internal static ProjectFolder? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new ProjectFolder(handle, false);
	    }
	    
	    internal static ProjectFolder MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new ProjectFolder(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeProjectFolder(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }
	}
}