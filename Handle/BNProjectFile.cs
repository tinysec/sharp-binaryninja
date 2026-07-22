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

		/// <summary>Sets the file's display name.</summary>
		public bool SetName(string name)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			return NativeMethods.BNProjectFileSetName(this.handle, name);
		}

	    /// <summary>The file's description.</summary>
	    public string Description
	    {
		    get { return UnsafeUtils.TakeAnsiString(NativeMethods.BNProjectFileGetDescription(this.handle)); }
	    }

		/// <summary>Sets the file's description.</summary>
		public bool SetDescription(string description)
		{
			if (null == description)
			{
				throw new ArgumentNullException(nameof(description));
			}

			return NativeMethods.BNProjectFileSetDescription(this.handle, description);
		}

	    /// <summary>The absolute path of the file's backing content on disk.</summary>
	    public string PathOnDisk
	    {
		    get { return UnsafeUtils.TakeAnsiString(NativeMethods.BNProjectFileGetPathOnDisk(this.handle)); }
	    }

		/// <summary>Gets the file's path within its project.</summary>
		public string PathInProject
		{
			get { return UnsafeUtils.TakeUtf8String(NativeMethods.BNProjectFileGetPathInProject(this.handle)); }
		}

		/// <summary>Gets whether the file's backing contents exist on disk.</summary>
		public bool ExistsOnDisk
		{
			get { return NativeMethods.BNProjectFileExistsOnDisk(this.handle); }
		}

		/// <summary>Gets the file's creation timestamp.</summary>
		public long CreationTimestamp
		{
			get { return NativeMethods.BNProjectFileGetCreationTimestamp(this.handle); }
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

		/// <summary>Moves the file to another folder, or to the project root.</summary>
		public bool SetFolder(ProjectFolder? folder)
		{
			return NativeMethods.BNProjectFileSetFolder(
				this.handle,
				null == folder ? IntPtr.Zero : folder.DangerousGetHandle()
			);
		}

		/// <summary>Exports the file's contents to a destination path.</summary>
		public bool Export(string destination)
		{
			if (null == destination)
			{
				throw new ArgumentNullException(nameof(destination));
			}

			return NativeMethods.BNProjectFileExport(this.handle, destination);
		}

		/// <summary>Adds another project file as a dependency.</summary>
		public bool AddDependency(ProjectFile file)
		{
			if (null == file)
			{
				throw new ArgumentNullException(nameof(file));
			}

			return NativeMethods.BNProjectFileAddDependency(
				this.handle,
				file.DangerousGetHandle()
			);
		}

		/// <summary>Removes a project file dependency.</summary>
		public bool RemoveDependency(ProjectFile file)
		{
			if (null == file)
			{
				throw new ArgumentNullException(nameof(file));
			}

			return NativeMethods.BNProjectFileRemoveDependency(
				this.handle,
				file.DangerousGetHandle()
			);
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
