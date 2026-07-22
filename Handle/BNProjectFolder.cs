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

	    // --- Identity --------------------------------------------------------

	    /// <summary>The folder's unique identifier.</summary>
	    public string Id
	    {
		    get { return UnsafeUtils.TakeAnsiString(NativeMethods.BNProjectFolderGetId(this.handle)); }
	    }

	    /// <summary>The folder's display name.</summary>
	    public string Name
	    {
		    get { return UnsafeUtils.TakeAnsiString(NativeMethods.BNProjectFolderGetName(this.handle)); }
	    }

		/// <summary>Sets the folder's display name.</summary>
		public bool SetName(string name)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			return NativeMethods.BNProjectFolderSetName(this.handle, name);
		}

	    /// <summary>The folder's description.</summary>
	    public string Description
	    {
		    get { return UnsafeUtils.TakeAnsiString(NativeMethods.BNProjectFolderGetDescription(this.handle)); }
	    }

		/// <summary>Sets the folder's description.</summary>
		public bool SetDescription(string description)
		{
			if (null == description)
			{
				throw new ArgumentNullException(nameof(description));
			}

			return NativeMethods.BNProjectFolderSetDescription(this.handle, description);
		}

	    // --- Navigation ------------------------------------------------------

	    /// <summary>The project that owns this folder. Mirrors Python ProjectFolder.project.</summary>
	    public Project? Project
	    {
		    get { return Project.TakeHandle(NativeMethods.BNProjectFolderGetProject(this.handle)); }
	    }

	    /// <summary>The parent folder, or null when this folder is at the project root.</summary>
	    public ProjectFolder? Parent
	    {
		    get { return ProjectFolder.TakeHandle(NativeMethods.BNProjectFolderGetParent(this.handle)); }
	    }

		/// <summary>Moves the folder under another parent, or to the project root.</summary>
		public bool SetParent(ProjectFolder? parent)
		{
			return NativeMethods.BNProjectFolderSetParent(
				this.handle,
				null == parent ? IntPtr.Zero : parent.DangerousGetHandle()
			);
		}

		/// <summary>Recursively exports this folder to a destination path.</summary>
		public bool Export(string destination, ProgressDelegate? progress = null)
		{
			if (null == destination)
			{
				throw new ArgumentNullException(nameof(destination));
			}

			ProgressCallbackContext progressContext = new ProgressCallbackContext(progress);
			NativeDelegates.BNProgressFunction nativeProgress = progressContext.Invoke;
			bool result = NativeMethods.BNProjectFolderExport(
				this.handle,
				destination,
				IntPtr.Zero,
				Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(nativeProgress)
			);
			GC.KeepAlive(nativeProgress);
			progressContext.ThrowIfFailed();
			return result;
		}

	    /// <summary>The files directly contained in this folder. Mirrors Python ProjectFolder navigation.</summary>
	    public unsafe ProjectFile[] Files
	    {
		    get
		    {
			    ulong count = 0;
			    IntPtr arrayPointer = NativeMethods.BNProjectFolderGetFiles(this.handle, (IntPtr)(&count));

			    return UnsafeUtils.TakeHandleArrayEx<ProjectFile>(
				    arrayPointer,
				    count,
				    ProjectFile.MustNewFromHandle,
				    NativeMethods.BNFreeProjectFileList
			    );
		    }
	    }
	}
}
