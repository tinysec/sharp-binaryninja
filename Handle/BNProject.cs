using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a Binary Ninja project handle.
    /// Provides local project creation, navigation, metadata, and content management APIs.
    /// </summary>
    public sealed class Project : AbstractSafeHandle<Project>
    {
		private readonly object notificationLock = new object();

		private readonly List<ProjectNotification> notifications =
			new List<ProjectNotification>();

        /// <summary>
        /// Initializes a new Project wrapper around an existing native handle.
        /// </summary>
        /// <param name="handle">The native pointer to the BNProject object.</param>
        /// <param name="owner">True if this instance owns the handle and should free it on dispose.</param>
        internal Project(IntPtr handle, bool owner)
            : base(handle, owner)
        {
        }

        /// <summary>
        /// Creates a new managed Project by incrementing the reference count on an existing native handle.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNProject pointer.</param>
        /// <returns>A new Project instance, or null if handle is zero.</returns>
        internal static Project? NewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new Project(
                NativeMethods.BNNewProjectReference(handle),
                true
            );
        }

        /// <summary>
        /// Creates a new managed Project by incrementing the reference count. Throws if handle is zero.
        /// </summary>
        /// <param name="handle">The native BNProject pointer.</param>
        /// <returns>A new Project instance.</returns>
        internal static Project MustNewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new Project(
                NativeMethods.BNNewProjectReference(handle),
                true
            );
        }

        /// <summary>
        /// Takes ownership of an existing native handle without incrementing the reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNProject pointer.</param>
        /// <returns>A new Project instance, or null if handle is zero.</returns>
        internal static Project? TakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new Project(handle, true);
        }

        /// <summary>
        /// Takes ownership of an existing native handle without incrementing the reference count. Throws if zero.
        /// </summary>
        /// <param name="handle">The native BNProject pointer.</param>
        /// <returns>A new Project instance.</returns>
        internal static Project MustTakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new Project(handle, true);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNProject pointer.</param>
        /// <returns>A new Project instance that will not free the handle on dispose.</returns>
        internal static Project? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new Project(handle, false);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNProject pointer.</param>
        /// <returns>A new Project instance that will not free the handle on dispose.</returns>
        internal static Project MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new Project(handle, false);
        }

        /// <summary>
        /// Releases the native BNProject handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
				lock (this.notificationLock)
				{
					foreach (ProjectNotification notification in this.notifications)
					{
						NativeMethods.BNUnregisterProjectNotification(
							this.handle,
							notification.CallbacksPointer
						);
					}

					this.notifications.Clear();
				}

                // Free the native project handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeProject(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

		/// <summary>Registers a notification receiver for this project.</summary>
		public void RegisterNotification(ProjectNotification notification)
		{
			if (null == notification)
			{
				throw new ArgumentNullException(nameof(notification));
			}

			lock (this.notificationLock)
			{
				if (this.notifications.Contains(notification))
				{
					throw new InvalidOperationException("The notification is already registered.");
				}

				NativeMethods.BNRegisterProjectNotification(
					this.handle,
					notification.CallbacksPointer
				);
				this.notifications.Add(notification);
			}
		}

		/// <summary>Unregisters a notification receiver from this project.</summary>
		public void UnregisterNotification(ProjectNotification notification)
		{
			if (null == notification)
			{
				throw new ArgumentNullException(nameof(notification));
			}

			lock (this.notificationLock)
			{
				if (!this.notifications.Remove(notification))
				{
					return;
				}

				NativeMethods.BNUnregisterProjectNotification(
					this.handle,
					notification.CallbacksPointer
				);
			}
		}

        /// <summary>
        /// Gets all plugin commands that are valid to run in the context of this project.
        /// </summary>
        public PluginCommand[] ValidPluginCommands
        {
            get
            {
                // Retrieve the list of valid plugin commands using an out count parameter.
                IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommandsForProject(
                    this.handle,
                    out ulong arrayLength
                );

                return UnsafeUtils.TakeStructArray<BNPluginCommand, PluginCommand>(
                    arrayPointer,
                    arrayLength,
                    PluginCommand.FromNative,
                    NativeMethods.BNFreePluginCommandList
                );
            }
        }

        // ---------------------------------------------------------------------
        // Static factories (mirror Python Project.open_project / create_project)
        // ---------------------------------------------------------------------

        /// <summary>
        /// Opens an existing project at the given .bnpr directory or .bnpm metadata path.
        /// Returns null on failure.
        /// </summary>
        public static Project? OpenProject(string path)
        {
            return Project.TakeHandle(NativeMethods.BNOpenProject(path ?? string.Empty));
        }

        /// <summary>
        /// Creates a new project in the given .bnpr directory with the given display name.
        /// Returns null on failure.
        /// </summary>
        public static Project? CreateProject(string path, string name)
        {
            return Project.TakeHandle(
                NativeMethods.BNCreateProject(path ?? string.Empty, name ?? string.Empty)
            );
        }

		/// <summary>Gets every project currently open in this process.</summary>
		public static Project[] GetOpenProjects()
		{
			ulong count;
			IntPtr projects = NativeMethods.BNGetOpenProjects(out count);
			return UnsafeUtils.TakeHandleArrayEx<Project>(
				projects,
				count,
				Project.MustNewFromHandle,
				NativeMethods.BNFreeProjectList
			);
		}

        // ---------------------------------------------------------------------
        // Open/close state
        // ---------------------------------------------------------------------

        /// <summary>Whether the project database is currently open.</summary>
        public bool IsOpen
        {
            get { return NativeMethods.BNProjectIsOpen(this.handle); }
        }

        /// <summary>Opens a closed project. Returns true if the project is now open.</summary>
        public bool Open()
        {
            return NativeMethods.BNProjectOpen(this.handle);
        }

        /// <summary>
        /// Closes an open project database (distinct from disposing this wrapper's handle).
        /// Returns true if the project is now closed. Mirrors Python Project.close.
        /// </summary>
        public bool CloseProject()
        {
            return NativeMethods.BNProjectClose(this.handle);
        }

        // ---------------------------------------------------------------------
        // Identity
        // ---------------------------------------------------------------------

        /// <summary>The project's unique identifier.</summary>
        public string Id
        {
            get { return UnsafeUtils.TakeAnsiString(NativeMethods.BNProjectGetId(this.handle)); }
        }

        /// <summary>The project's display name.</summary>
        public string Name
        {
            get { return UnsafeUtils.TakeAnsiString(NativeMethods.BNProjectGetName(this.handle)); }
        }

		/// <summary>Sets the project's display name.</summary>
		public bool SetName(string name)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			return NativeMethods.BNProjectSetName(this.handle, name);
		}

        /// <summary>The project's description.</summary>
        public string Description
        {
            get { return UnsafeUtils.TakeAnsiString(NativeMethods.BNProjectGetDescription(this.handle)); }
        }

		/// <summary>Sets the project's description.</summary>
		public bool SetDescription(string description)
		{
			if (null == description)
			{
				throw new ArgumentNullException(nameof(description));
			}

			return NativeMethods.BNProjectSetDescription(this.handle, description);
		}

        /// <summary>The project's path on disk (the .bnpr directory).</summary>
        public string Path
        {
            get { return UnsafeUtils.TakeAnsiString(NativeMethods.BNProjectGetPath(this.handle)); }
        }

		/// <summary>Gets the in-project path for a file owned by this project.</summary>
		public string GetFilePathInProject(ProjectFile file)
		{
			if (null == file)
			{
				throw new ArgumentNullException(nameof(file));
			}

			return UnsafeUtils.TakeUtf8String(
				NativeMethods.BNProjectGetFilePathInProject(this.handle, file.DangerousGetHandle())
			);
		}

		/// <summary>Queries metadata stored under a project key.</summary>
		public Metadata? QueryMetadata(string key)
		{
			if (null == key)
			{
				throw new ArgumentNullException(nameof(key));
			}

			return Metadata.TakeHandle(NativeMethods.BNProjectQueryMetadata(this.handle, key));
		}

		/// <summary>Stores metadata under a project key.</summary>
		public bool StoreMetadata(string key, Metadata value)
		{
			if (null == key)
			{
				throw new ArgumentNullException(nameof(key));
			}

			if (null == value)
			{
				throw new ArgumentNullException(nameof(value));
			}

			return NativeMethods.BNProjectStoreMetadata(
				this.handle,
				key,
				value.DangerousGetHandle()
			);
		}

		/// <summary>Removes metadata stored under a project key.</summary>
		public bool RemoveMetadata(string key)
		{
			if (null == key)
			{
				throw new ArgumentNullException(nameof(key));
			}

			return NativeMethods.BNProjectRemoveMetadata(this.handle, key);
		}

        // ---------------------------------------------------------------------
        // Navigation to files and folders
        // ---------------------------------------------------------------------

        /// <summary>All files in the project. Mirrors Python Project.files.</summary>
        public unsafe ProjectFile[] Files
        {
            get
            {
                ulong count = 0;
                IntPtr arrayPointer = NativeMethods.BNProjectGetFiles(this.handle, (IntPtr)(&count));

                return UnsafeUtils.TakeHandleArrayEx<ProjectFile>(
                    arrayPointer,
                    count,
                    ProjectFile.MustNewFromHandle,
                    NativeMethods.BNFreeProjectFileList
                );
            }
        }

        /// <summary>All folders in the project. Mirrors Python Project.folders.</summary>
        public unsafe ProjectFolder[] Folders
        {
            get
            {
                ulong count = 0;
                IntPtr arrayPointer = NativeMethods.BNProjectGetFolders(this.handle, (IntPtr)(&count));

                return UnsafeUtils.TakeHandleArrayEx<ProjectFolder>(
                    arrayPointer,
                    count,
                    ProjectFolder.MustNewFromHandle,
                    NativeMethods.BNFreeProjectFolderList
                );
            }
        }

        /// <summary>
        /// The files directly contained in the given folder, or the project root when
        /// folder is null. Mirrors Python Project.get_files_in_folder.
        /// </summary>
        public unsafe ProjectFile[] GetFilesInFolder(ProjectFolder? folder)
        {
            ulong count = 0;
            IntPtr arrayPointer = NativeMethods.BNProjectGetFilesInFolder(
                this.handle,
                null == folder ? IntPtr.Zero : folder.DangerousGetHandle(),
                (IntPtr)(&count)
            );

            return UnsafeUtils.TakeHandleArrayEx<ProjectFile>(
                arrayPointer,
                count,
                ProjectFile.MustNewFromHandle,
                NativeMethods.BNFreeProjectFileList
            );
        }

        /// <summary>The folder with the given id, or null. Mirrors Python Project.get_folder_by_id.</summary>
        public ProjectFolder? GetFolderById(string id)
        {
            return ProjectFolder.TakeHandle(
                NativeMethods.BNProjectGetFolderById(this.handle, id ?? string.Empty)
            );
        }

        /// <summary>The file with the given id, or null. Mirrors Python Project.get_file_by_id.</summary>
        public ProjectFile? GetFileById(string id)
        {
            return ProjectFile.TakeHandle(
                NativeMethods.BNProjectGetFileById(this.handle, id ?? string.Empty)
            );
        }

        /// <summary>
        /// The file whose on-disk path matches, or null. Mirrors Python
        /// Project.get_file_by_path_on_disk.
        /// </summary>
        public ProjectFile? GetFileByPathOnDisk(string path)
        {
            return ProjectFile.TakeHandle(
                NativeMethods.BNProjectGetFileByPathOnDisk(this.handle, path ?? string.Empty)
            );
        }

        /// <summary>
        /// All files whose in-project path matches. Mirrors Python
        /// Project.get_files_by_path_in_project.
        /// </summary>
        public unsafe ProjectFile[] GetFilesByPathInProject(string path)
        {
            ulong count = 0;
            IntPtr arrayPointer = NativeMethods.BNProjectGetFilesByPathInProject(
                this.handle,
                path ?? string.Empty,
                (IntPtr)(&count)
            );

            return UnsafeUtils.TakeHandleArrayEx<ProjectFile>(
                arrayPointer,
                count,
                ProjectFile.MustNewFromHandle,
                NativeMethods.BNFreeProjectFileList
            );
        }

        // ---------------------------------------------------------------------
        // Creation (needed to populate a project; mirrors Python create_folder / create_file_from_path)
        // ---------------------------------------------------------------------

        /// <summary>
        /// Creates a folder under the given parent (or the project root when parent is null).
        /// Returns the new folder, or null on failure.
        /// </summary>
        public ProjectFolder? CreateFolder(ProjectFolder? parent, string name, string description = "")
        {
            return ProjectFolder.TakeHandle(
                NativeMethods.BNProjectCreateFolder(
                    this.handle,
                    null == parent ? IntPtr.Zero : parent.DangerousGetHandle(),
                    name ?? string.Empty,
                    description ?? string.Empty
                )
            );
        }

		/// <summary>Recursively imports a directory into this project.</summary>
		public ProjectFolder? CreateFolderFromPath(
			string path,
			ProjectFolder? parent = null,
			string description = "",
			ProgressDelegate? progress = null
		)
		{
			if (null == path)
			{
				throw new ArgumentNullException(nameof(path));
			}

			ProgressCallbackContext progressContext = new ProgressCallbackContext(progress);
			NativeDelegates.BNProgressFunction nativeProgress = progressContext.Invoke;
			IntPtr result = NativeMethods.BNProjectCreateFolderFromPath(
				this.handle,
				path,
				null == parent ? IntPtr.Zero : parent.DangerousGetHandle(),
				description ?? string.Empty,
				IntPtr.Zero,
				Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(nativeProgress)
			);
			GC.KeepAlive(nativeProgress);
			progressContext.ThrowIfFailed();
			return ProjectFolder.TakeHandle(result);
		}

		/// <summary>Creates a folder with an explicitly supplied stable identifier.</summary>
		public ProjectFolder? CreateFolderUnsafe(
			ProjectFolder? parent,
			string name,
			string description,
			string id
		)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			if (null == id)
			{
				throw new ArgumentNullException(nameof(id));
			}

			return ProjectFolder.TakeHandle(
				NativeMethods.BNProjectCreateFolderUnsafe(
					this.handle,
					null == parent ? IntPtr.Zero : parent.DangerousGetHandle(),
					name,
					description ?? string.Empty,
					id
				)
			);
		}

		/// <summary>Pushes a folder to the project's backing store.</summary>
		public bool PushFolder(ProjectFolder folder)
		{
			if (null == folder)
			{
				throw new ArgumentNullException(nameof(folder));
			}

			return NativeMethods.BNProjectPushFolder(this.handle, folder.DangerousGetHandle());
		}

		/// <summary>Recursively deletes a folder from this project.</summary>
		public bool DeleteFolder(ProjectFolder folder, ProgressDelegate? progress = null)
		{
			if (null == folder)
			{
				throw new ArgumentNullException(nameof(folder));
			}

			ProgressCallbackContext progressContext = new ProgressCallbackContext(progress);
			NativeDelegates.BNProgressFunction nativeProgress = progressContext.Invoke;
			bool result = NativeMethods.BNProjectDeleteFolder(
				this.handle,
				folder.DangerousGetHandle(),
				IntPtr.Zero,
				Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(nativeProgress)
			);
			GC.KeepAlive(nativeProgress);
			progressContext.ThrowIfFailed();
			return result;
		}

        /// <summary>
        /// Imports the file at the given on-disk path into the project under the given folder
        /// (or the project root when folder is null). Returns the new project file, or null.
        /// </summary>
        public ProjectFile? CreateFileFromPath(
            string path,
            ProjectFolder? folder,
            string name,
            string description = "",
			ProgressDelegate? progress = null
        )
        {
			ProgressCallbackContext progressContext = new ProgressCallbackContext(progress);
			NativeDelegates.BNProgressFunction nativeProgress = progressContext.Invoke;
			IntPtr result = NativeMethods.BNProjectCreateFileFromPath(
                    this.handle,
                    path ?? string.Empty,
                    null == folder ? IntPtr.Zero : folder.DangerousGetHandle(),
                    name ?? string.Empty,
                    description ?? string.Empty,
                    IntPtr.Zero,
					Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(nativeProgress)
			);
			GC.KeepAlive(nativeProgress);
			progressContext.ThrowIfFailed();
			return ProjectFile.TakeHandle(result);
        }

		/// <summary>Imports a file with an explicitly supplied identifier and timestamp.</summary>
		public ProjectFile? CreateFileFromPathUnsafe(
			string path,
			ProjectFolder? folder,
			string name,
			string description,
			string id,
			long creationTimestamp,
			ProgressDelegate? progress = null
		)
		{
			ProgressCallbackContext progressContext = new ProgressCallbackContext(progress);
			NativeDelegates.BNProgressFunction nativeProgress = progressContext.Invoke;
			IntPtr result = NativeMethods.BNProjectCreateFileFromPathUnsafe(
				this.handle,
				path ?? string.Empty,
				null == folder ? IntPtr.Zero : folder.DangerousGetHandle(),
				name ?? string.Empty,
				description ?? string.Empty,
				id ?? string.Empty,
				creationTimestamp,
				IntPtr.Zero,
				Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(nativeProgress)
			);
			GC.KeepAlive(nativeProgress);
			progressContext.ThrowIfFailed();
			return ProjectFile.TakeHandle(result);
		}

		/// <summary>Creates a project file from in-memory contents.</summary>
		public unsafe ProjectFile? CreateFile(
			byte[] contents,
			ProjectFolder? folder,
			string name,
			string description = "",
			ProgressDelegate? progress = null
		)
		{
			if (null == contents)
			{
				throw new ArgumentNullException(nameof(contents));
			}

			ProgressCallbackContext progressContext = new ProgressCallbackContext(progress);
			NativeDelegates.BNProgressFunction nativeProgress = progressContext.Invoke;
			IntPtr result;
			fixed (byte* contentsPointer = contents)
			{
				result = NativeMethods.BNProjectCreateFile(
					this.handle,
					(IntPtr)contentsPointer,
					(ulong)contents.Length,
					null == folder ? IntPtr.Zero : folder.DangerousGetHandle(),
					name ?? string.Empty,
					description ?? string.Empty,
					IntPtr.Zero,
					Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(nativeProgress)
				);
			}
			GC.KeepAlive(nativeProgress);
			progressContext.ThrowIfFailed();
			return ProjectFile.TakeHandle(result);
		}

		/// <summary>Creates a project file with an explicitly supplied identifier and timestamp.</summary>
		public unsafe ProjectFile? CreateFileUnsafe(
			byte[] contents,
			ProjectFolder? folder,
			string name,
			string description,
			string id,
			long creationTimestamp,
			ProgressDelegate? progress = null
		)
		{
			if (null == contents)
			{
				throw new ArgumentNullException(nameof(contents));
			}

			ProgressCallbackContext progressContext = new ProgressCallbackContext(progress);
			NativeDelegates.BNProgressFunction nativeProgress = progressContext.Invoke;
			IntPtr result;
			fixed (byte* contentsPointer = contents)
			{
				result = NativeMethods.BNProjectCreateFileUnsafe(
					this.handle,
					(IntPtr)contentsPointer,
					(ulong)contents.Length,
					null == folder ? IntPtr.Zero : folder.DangerousGetHandle(),
					name ?? string.Empty,
					description ?? string.Empty,
					id ?? string.Empty,
					creationTimestamp,
					IntPtr.Zero,
					Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(nativeProgress)
				);
			}
			GC.KeepAlive(nativeProgress);
			progressContext.ThrowIfFailed();
			return ProjectFile.TakeHandle(result);
		}

		/// <summary>Pushes a file to the project's backing store.</summary>
		public bool PushFile(ProjectFile file)
		{
			if (null == file)
			{
				throw new ArgumentNullException(nameof(file));
			}

			return NativeMethods.BNProjectPushFile(this.handle, file.DangerousGetHandle());
		}

		/// <summary>Deletes a file from this project.</summary>
		public bool DeleteFile(ProjectFile file)
		{
			if (null == file)
			{
				throw new ArgumentNullException(nameof(file));
			}

			return NativeMethods.BNProjectDeleteFile(this.handle, file.DangerousGetHandle());
		}

		/// <summary>Begins a bulk project operation.</summary>
		public bool BeginBulkOperation()
		{
			return NativeMethods.BNProjectBeginBulkOperation(this.handle);
		}

		/// <summary>Ends a bulk project operation.</summary>
		public bool EndBulkOperation()
		{
			return NativeMethods.BNProjectEndBulkOperation(this.handle);
		}

		/// <summary>Gets the collaboration project associated with this local project.</summary>
		public RemoteProject? RemoteProject
		{
			get { return RemoteProject.TakeHandle(NativeMethods.BNProjectGetRemoteProject(this.handle)); }
		}
    }
}
