using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class FileMetadata : AbstractSafeHandle<FileMetadata>
	{
		private NavigationHandler? navigationHandler;

		public FileMetadata()
			: this(NativeMethods.BNCreateFileMetadata() , true)
		{
			
		}

		public FileMetadata(string filename)
			: this(NativeMethods.BNCreateFileMetadata() , true)
		{
			this.Filename = filename;
		}

		internal FileMetadata(IntPtr handle , bool owner) 
			: base(handle , owner)
		{
			
		}
		
		internal static FileMetadata? TakeHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new FileMetadata(handle, true);
		}
	    
		internal static FileMetadata MustTakeHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new FileMetadata(handle, true);
		}
	    
		internal static FileMetadata? BorrowHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new FileMetadata(handle, false);
		}
	    
		internal static FileMetadata MustBorrowHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new FileMetadata(handle, false);
		}
		

		public static FileMetadata FromFile(string filename)
		{
			FileMetadata file = new FileMetadata();

			file.Filename = filename;

			return file;
		}

		protected override bool ReleaseHandle()
		{
			if (!this.IsInvalid)
			{
				if (null != this.navigationHandler)
				{
					NativeMethods.BNSetFileMetadataNavigationHandler(this.handle, IntPtr.Zero);
					this.navigationHandler = null;
				}

				NativeMethods.BNCloseFile(this.handle);
				NativeMethods.BNFreeFileMetadata(this.handle);
				this.SetHandleAsInvalid();
			}

			return true;
		}


		public ulong SessionId
		{
			get
			{
				return NativeMethods.BNFileMetadataGetSessionId(this.handle);
			}
		}

		public string Filename
		{
			get
			{
				return UnsafeUtils.TakeUtf8String(
					NativeMethods.BNGetFilename(this.handle)
				);
			}

			set
			{
				NativeMethods.BNSetFilename(this.handle , value);
			}
		}

		/// <summary>Gets or sets the user-facing display name for this file.</summary>
		public string DisplayName
		{
			get
			{
				return UnsafeUtils.TakeUtf8String(NativeMethods.BNGetDisplayName(this.handle));
			}
			set
			{
				if (null == value)
				{
					throw new ArgumentNullException(nameof(value));
				}

				NativeMethods.BNSetDisplayName(this.handle, value);
			}
		}

		public string OriginalFilename
		{
			get
			{
				return UnsafeUtils.TakeUtf8String(
					NativeMethods.BNGetOriginalFilename(this.handle)
				);
			}

			set
			{
				NativeMethods.BNSetOriginalFilename(this.handle , value);
			}
		}

		public string GetVirtualPath()
		{
			return UnsafeUtils.TakeUtf8String(
				NativeMethods.BNGetVirtualPath(this.handle)
			);
		}

		public void SetVirtualPath(string value)
		{
			NativeMethods.BNSetVirtualPath(this.handle , value);
		}


		public bool Modified
		{
			get
			{
				return NativeMethods.BNIsFileModified(this.handle);
			}

			set
			{
				if (value)
				{
					NativeMethods.BNMarkFileModified(this.handle);
				}
				else
				{
					NativeMethods.BNMarkFileSaved(this.handle);
				}
			}
		}

		public bool AnalysisChanged
		{
			get
			{
				return NativeMethods.BNIsAnalysisChanged(this.handle);
			}
		}

		public bool IsBackedByDatabase(string binaryViewType = "")
		{
			return NativeMethods.BNIsBackedByDatabase(this.handle , binaryViewType);
		}

		public string View
		{
			get
			{
				return UnsafeUtils.TakeAnsiString(NativeMethods.BNGetCurrentView(this.handle));
			}

			set
			{
				this.Navigate(value , this.Offset);
			}
		}

		public ulong Offset
		{
			get
			{
				return NativeMethods.BNGetCurrentOffset(this.handle);
			}

			set
			{
				this.Navigate(this.View , value);
			}
		}

		public bool Navigate(string binaryViewType , ulong offset)
		{
			return NativeMethods.BNNavigate(
				this.handle ,
				binaryViewType ,
				offset
			);
		}

		/// <summary>
		/// Gets or sets the handler used by the core to query and update the current navigation state.
		/// </summary>
		public NavigationHandler? Navigation
		{
			get
			{
				return this.navigationHandler;
			}

			set
			{
				this.SetNavigationHandler(value);
			}
		}

		/// <summary>
		/// Gets or sets the navigation handler. This is an alias for <see cref="Navigation"/>.
		/// </summary>
		public NavigationHandler? Nav
		{
			get
			{
				return this.Navigation;
			}

			set
			{
				this.Navigation = value;
			}
		}

		/// <summary>
		/// Registers a navigation handler, or unregisters the current handler when null is supplied.
		/// </summary>
		public void SetNavigationHandler(NavigationHandler? handler)
		{
			NativeMethods.BNSetFileMetadataNavigationHandler(
				this.handle,
				null == handler ? IntPtr.Zero : handler.Callbacks);
			this.navigationHandler = handler;
		}

		public BinaryView? GetFileViewOfType(string binaryViewType)
		{
			return BinaryView.TakeHandle(
				NativeMethods.BNGetFileViewOfType(this.handle , binaryViewType)
			);
		}

		public BinaryView? RawBinaryView
		{
			get
			{
				return this.GetFileViewOfType("Raw");
			}
		}

		public Database? Database
		{
			get
			{
				return Database.TakeHandle(
					NativeMethods.BNGetFileMetadataDatabase(this.handle)
				);
			}
		}

		public bool SnapshotDataAppliedWithoutError
		{
			get
			{
				return NativeMethods.BNIsSnapshotDataAppliedWithoutError(this.handle);
			}
		}


		public ProjectFile? ProjectFile
		{
			get
			{
				return ProjectFile.TakeHandle(
					NativeMethods.BNGetProjectFile(this.handle)
				);
			}

			set
			{
				NativeMethods.BNSetProjectFile(
					this.handle ,
					null == value ? IntPtr.Zero : value.DangerousGetHandle()
				);
			}
		}

		/// <summary>
		/// The project that owns this file, navigating file -> ProjectFile -> Project, or null
		/// when the file is not part of a project. Mirrors Python FileMetadata.project.
		/// </summary>
		public Project? Project
		{
			get
			{
				using (ProjectFile? file = this.ProjectFile)
				{
					if (null == file)
					{
						return null;
					}

					return file.Project;
				}
			}
		}

		public void CloseFile()
		{
			NativeMethods.BNCloseFile(this.handle);
		}

		public string BeginUndoActions(bool anonymousAllowed  )
	    {
		    return UnsafeUtils.TakeUtf8String(
			    NativeMethods.BNBeginUndoActions(this.handle , anonymousAllowed)
		    );
	    }

	    public void CommitUndoActions(string id)
	    {
		    NativeMethods.BNCommitUndoActions(this.handle , id);
	    }
	    
	    public void ForgetUndoActions(string id)
	    {
		    NativeMethods.BNForgetUndoActions(this.handle , id);
	    }
	    
	    public void RevertUndoActions(string id)
	    {
		    NativeMethods.BNRevertUndoActions(this.handle , id);
	    }

	    public bool CanUndo
	    {
		    get
		    {
			    return NativeMethods.BNCanUndo(this.handle);
		    }
	    }
	    
	    public bool Undo()
	    {
		    return NativeMethods.BNUndo(this.handle);
	    }
	    
	    public bool CanRedo
	    {
		    get
		    {
			    return NativeMethods.BNCanRedo(this.handle);
		    }
	    }
	    
	    public bool Redo()
	    {
		    return NativeMethods.BNRedo(this.handle);
	    }

	    public UndoEntry[] UndoEntries
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetUndoEntries(
				    this.handle ,
				    out ulong arrayLength
			    );

			    return UnsafeUtils.TakeHandleArrayEx<UndoEntry>(
				    arrayPointer ,
				    arrayLength ,
				    UndoEntry.MustNewFromHandle ,
				    NativeMethods.BNFreeUndoEntryList
			    );
		    }
	    }
	    
	    public UndoEntry[] RedoEntrie
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetRedoEntries(
				    this.handle ,
				    out ulong arrayLength
			    );

			    return UnsafeUtils.TakeHandleArrayEx<UndoEntry>(
				    arrayPointer ,
				    arrayLength ,
				    UndoEntry.MustNewFromHandle ,
				    NativeMethods.BNFreeUndoEntryList
			    );
		    }
	    }

	    public string[] ExistingViews
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetExistingViews(
				    this.handle ,
				    out ulong arrayLength
			    );
			    
			    return UnsafeUtils.TakeAnsiStringArray(
				    arrayPointer,
				    arrayLength ,
				    NativeMethods.BNFreeStringList
			    );
		    }
	    }


	    public void UnregisterViewOfType(string viewType , BinaryView view)
	    {
		    NativeMethods.BNUnregisterViewOfType(
			    this.handle , 
			    viewType , 
			    view.DangerousGetHandle()
			);
	    }
	    
	    public Logger? GetLogger(string name)
	    {
		    return Logger.GetLogger(name , this.SessionId);
	    }
	    
	    public Logger CreateLogger(string name)
	    {
		    return Logger.CreateLogger(name , this.SessionId);
	    }
	    
	    public Logger GetOrCreateLogger(string name)
	    {
		    return Logger.GetOrCreateLogger(name , this.SessionId);
	    }

	    public BinaryView? OpenExistingDatabase(string filename , ProgressDelegate? progress = null)
	    {
		    if (null == progress)
		    {
			    return BinaryView.TakeHandle(
				    NativeMethods.BNOpenExistingDatabase(this.handle , filename)
			    );
		    }
		    else
		    {
			    NativeDelegates.BNProgressFunction progressWrapper = UnsafeUtils.WrapProgressDelegate(progress);

			    BinaryView? result = BinaryView.TakeHandle(
				    NativeMethods.BNOpenExistingDatabaseWithProgress(
					    this.handle ,
					    filename,
					    IntPtr.Zero,
					    Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(progressWrapper)
					)
			    );

			    GC.KeepAlive(progressWrapper);

			    return result;
		    }
	    }
	    
	    public BinaryView? OpenDatabaseForConfiguration(string filename )
	    {
		    return BinaryView.TakeHandle(
			    NativeMethods.BNOpenDatabaseForConfiguration(this.handle , filename)
		    );
	    }

	    /// <summary>
	    /// Clears all undo and redo entries from this file's undo history.
	    /// </summary>
	    public void ClearUndoEntries()
	    {
		    NativeMethods.BNClearUndoEntries(this.handle);
	    }

	    /// <summary>
	    /// Gets the last redo entry from the undo history.
	    /// Returns null if there is no redo entry available.
	    /// </summary>
	    /// <returns>An owned UndoEntry instance, or null if the redo stack is empty.</returns>
	    public UndoEntry? GetLastRedoEntry()
	    {
		    // BNGetLastRedoEntry returns an OWNED handle (the C++ wrapper adopts it with no
		    // addref), so take it directly instead of bumping the reference count.
		    return UndoEntry.TakeHandle(
			    NativeMethods.BNGetLastRedoEntry(this.handle)
		    );
	    }

	    /// <summary>
	    /// Gets the last undo entry from the undo history.
	    /// Returns null if there is no undo entry available.
	    /// </summary>
	    /// <returns>An owned UndoEntry instance, or null if the undo stack is empty.</returns>
	    public UndoEntry? GetLastUndoEntry()
	    {
		    // BNGetLastUndoEntry returns an OWNED handle (the C++ wrapper adopts it with no
		    // addref), so take it directly instead of bumping the reference count.
		    return UndoEntry.TakeHandle(
			    NativeMethods.BNGetLastUndoEntry(this.handle)
		    );
	    }

	    /// <summary>
	    /// Gets the title of the last redo entry in the undo history.
	    /// </summary>
	    /// <returns>The title string of the last redo entry.</returns>
	    public string GetLastRedoEntryTitle()
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNGetLastRedoEntryTitle(this.handle)
		    );
	    }

	    /// <summary>
	    /// Gets the title of the last undo entry in the undo history.
	    /// </summary>
	    /// <returns>The title string of the last undo entry.</returns>
	    public string GetLastUndoEntryTitle()
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNGetLastUndoEntryTitle(this.handle)
		    );
	    }

	    /// <summary>
	    /// Gets the list of users who have contributed to this file.
	    /// </summary>
	    /// <returns>An array of User objects.</returns>
	    public unsafe User[] GetUsers()
	    {
		    // 1. Stack-allocate the count variable.
		    ulong count = 0;

		    // 2. Call the native function to retrieve the user handle array.
		    IntPtr arrayPointer = NativeMethods.BNGetUsers(
			    this.handle ,
			    (IntPtr)(&count)
		    );

		    // 3. Convert the handle array to managed User objects and free.
		    return UnsafeUtils.TakeHandleArrayEx<User>(
			    arrayPointer ,
			    count ,
			    User.MustNewFromHandle ,
			    NativeMethods.BNFreeUserList
		    );
	    }

	    /// <summary>
	    /// Applies snapshot data to this file from a key-value store with optional progress reporting.
	    /// </summary>
	    /// <param name="view">The binary view to apply the snapshot to.</param>
	    /// <param name="data">The key-value store containing snapshot data.</param>
	    /// <param name="cache">The key-value store containing cached data.</param>
	    /// <param name="openForConfiguration">Whether to open the view in configuration mode.</param>
	    /// <param name="restoreRawView">Whether to restore the raw binary view.</param>
	    /// <param name="progress">Optional progress callback, or null for no progress reporting.</param>
	    public void ApplySnapshotData(
		    BinaryView view ,
		    KeyValueStore data ,
		    KeyValueStore cache ,
		    bool openForConfiguration ,
		    bool restoreRawView ,
		    ProgressDelegate? progress = null
	    )
	    {
		    NativeDelegates.BNProgressFunction? progressWrapper =
			    null == progress ? null : UnsafeUtils.WrapProgressDelegate(progress);

		    NativeMethods.BNApplySnapshotData(
			    this.handle ,
			    view.DangerousGetHandle() ,
			    data.DangerousGetHandle() ,
			    cache.DangerousGetHandle() ,
			    IntPtr.Zero ,
			    null == progressWrapper
				    ? IntPtr.Zero
				    : Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(progressWrapper) ,
			    openForConfiguration ,
			    restoreRawView
		    );

		    GC.KeepAlive(progressWrapper);
	    }

	    /// <summary>
	    /// Retrieves snapshot data from this file into a key-value store with optional progress reporting.
	    /// </summary>
	    /// <param name="data">The key-value store to receive snapshot data.</param>
	    /// <param name="cache">The key-value store to receive cached data.</param>
	    /// <param name="progress">Optional progress callback, or null for no progress reporting.</param>
	    public void GetSnapshotData(
		    KeyValueStore data ,
		    KeyValueStore cache ,
		    ProgressDelegate? progress = null
	    )
	    {
		    NativeDelegates.BNProgressFunction? progressWrapper =
			    null == progress ? null : UnsafeUtils.WrapProgressDelegate(progress);

		    NativeMethods.BNGetSnapshotData(
			    this.handle ,
			    data.DangerousGetHandle() ,
			    cache.DangerousGetHandle() ,
			    IntPtr.Zero ,
			    null == progressWrapper
				    ? IntPtr.Zero
				    : Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(progressWrapper)
		    );

		    GC.KeepAlive(progressWrapper);
	    }

	}
}
