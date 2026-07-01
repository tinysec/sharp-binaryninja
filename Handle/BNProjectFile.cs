using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class ProjectFile : AbstractSafeHandle<ProjectFile>
	{
	    internal ProjectFile(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	       
	    }

	    internal static ProjectFile? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new ProjectFile(
			    NativeMethods.BNNewProjectFileReference(handle) ,
			    true
		    );
	    }
	    
	    internal static ProjectFile MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new ProjectFile(
			    NativeMethods.BNNewProjectFileReference(handle) ,
			    true
		    );
	    }
	    
	    internal static ProjectFile? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new ProjectFile(handle, true);
	    }
	    
	    internal static ProjectFile MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new ProjectFile(handle, true);
	    }
	    
	    internal static ProjectFile? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new ProjectFile(handle, false);
	    }
	    
	    internal static ProjectFile MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new ProjectFile(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeProjectFile(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }
	}
}