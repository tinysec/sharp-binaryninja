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

	    // --- Identity --------------------------------------------------------

	    /// <summary>The file's unique identifier.</summary>
	    public string Id
	    {
		    get { return UnsafeUtils.TakeAnsiString(NativeMethods.BNProjectFileGetId(this.handle)); }
	    }

	    /// <summary>The file's display name within the project.</summary>
	    public string Name
	    {
		    get { return UnsafeUtils.TakeAnsiString(NativeMethods.BNProjectFileGetName(this.handle)); }
	    }

	    /// <summary>The file's description.</summary>
	    public string Description
	    {
		    get { return UnsafeUtils.TakeAnsiString(NativeMethods.BNProjectFileGetDescription(this.handle)); }
	    }

	    /// <summary>The absolute path of the file's backing content on disk.</summary>
	    public string PathOnDisk
	    {
		    get { return UnsafeUtils.TakeAnsiString(NativeMethods.BNProjectFileGetPathOnDisk(this.handle)); }
	    }

	    // --- Navigation ------------------------------------------------------

	    /// <summary>The project that owns this file. Mirrors Python ProjectFile.project.</summary>
	    public Project? Project
	    {
		    get { return Project.TakeHandle(NativeMethods.BNProjectFileGetProject(this.handle)); }
	    }

	    /// <summary>The folder containing this file, or null when it lives at the project root.</summary>
	    public ProjectFolder? Folder
	    {
		    get { return ProjectFolder.TakeHandle(NativeMethods.BNProjectFileGetFolder(this.handle)); }
	    }

	    /// <summary>The project files this file depends on. Mirrors Python ProjectFile.dependencies.</summary>
	    public unsafe ProjectFile[] GetDependencies()
	    {
		    ulong count = 0;
		    IntPtr arrayPointer = NativeMethods.BNProjectFileGetDependencies(this.handle, (IntPtr)(&count));

		    return UnsafeUtils.TakeHandleArrayEx<ProjectFile>(
			    arrayPointer,
			    count,
			    ProjectFile.MustNewFromHandle,
			    NativeMethods.BNFreeProjectFileList
		    );
	    }

	    /// <summary>The project files that depend on this file. Mirrors Python ProjectFile.required_by.</summary>
	    public unsafe ProjectFile[] GetRequiredBy()
	    {
		    ulong count = 0;
		    IntPtr arrayPointer = NativeMethods.BNProjectFileGetRequiredBy(this.handle, (IntPtr)(&count));

		    return UnsafeUtils.TakeHandleArrayEx<ProjectFile>(
			    arrayPointer,
			    count,
			    ProjectFile.MustNewFromHandle,
			    NativeMethods.BNFreeProjectFileList
		    );
	    }
	}
}