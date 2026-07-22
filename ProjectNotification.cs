using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	/// <summary>Receives lifecycle and content change events from a project.</summary>
	public class ProjectNotification
	{
		private enum ItemKind
		{
			FileCreated,
			FileUpdated,
			FileDeleted,
			FolderCreated,
			FolderUpdated,
			FolderDeleted
		}

		private readonly List<Delegate> callbackRoots = new List<Delegate>();

		private GCHandle callbacksHandle;

		public ProjectNotification()
		{
			BNProjectNotification callbacks = new BNProjectNotification();
			callbacks.context = IntPtr.Zero;
			callbacks.beforeOpenProject = this.RootCallback(
				new NativeDelegates.BNProjectNotificationBeforeProject(this.InvokeBeforeOpenProject)
			);
			callbacks.afterOpenProject = this.RootCallback(
				new NativeDelegates.BNProjectNotificationAfterProject(this.InvokeAfterOpenProject)
			);
			callbacks.beforeCloseProject = this.RootCallback(
				new NativeDelegates.BNProjectNotificationBeforeProject(this.InvokeBeforeCloseProject)
			);
			callbacks.afterCloseProject = this.RootCallback(
				new NativeDelegates.BNProjectNotificationAfterProject(this.InvokeAfterCloseProject)
			);
			callbacks.beforeProjectMetadataWritten = this.RootCallback(
				new NativeDelegates.BNProjectNotificationBeforeMetadata(this.InvokeBeforeMetadataWritten)
			);
			callbacks.afterProjectMetadataWritten = this.RootCallback(
				new NativeDelegates.BNProjectNotificationAfterMetadata(this.InvokeAfterMetadataWritten)
			);
			callbacks.beforeProjectFileCreated = this.RootCallback(
				new NativeDelegates.BNProjectNotificationBeforeItem(this.InvokeBeforeFileCreated)
			);
			callbacks.afterProjectFileCreated = this.RootCallback(
				new NativeDelegates.BNProjectNotificationAfterItem(this.InvokeAfterFileCreated)
			);
			callbacks.beforeProjectFileUpdated = this.RootCallback(
				new NativeDelegates.BNProjectNotificationBeforeItem(this.InvokeBeforeFileUpdated)
			);
			callbacks.afterProjectFileUpdated = this.RootCallback(
				new NativeDelegates.BNProjectNotificationAfterItem(this.InvokeAfterFileUpdated)
			);
			callbacks.beforeProjectFileDeleted = this.RootCallback(
				new NativeDelegates.BNProjectNotificationBeforeItem(this.InvokeBeforeFileDeleted)
			);
			callbacks.afterProjectFileDeleted = this.RootCallback(
				new NativeDelegates.BNProjectNotificationAfterItem(this.InvokeAfterFileDeleted)
			);
			callbacks.beforeProjectFolderCreated = this.RootCallback(
				new NativeDelegates.BNProjectNotificationBeforeItem(this.InvokeBeforeFolderCreated)
			);
			callbacks.afterProjectFolderCreated = this.RootCallback(
				new NativeDelegates.BNProjectNotificationAfterItem(this.InvokeAfterFolderCreated)
			);
			callbacks.beforeProjectFolderUpdated = this.RootCallback(
				new NativeDelegates.BNProjectNotificationBeforeItem(this.InvokeBeforeFolderUpdated)
			);
			callbacks.afterProjectFolderUpdated = this.RootCallback(
				new NativeDelegates.BNProjectNotificationAfterItem(this.InvokeAfterFolderUpdated)
			);
			callbacks.beforeProjectFolderDeleted = this.RootCallback(
				new NativeDelegates.BNProjectNotificationBeforeItem(this.InvokeBeforeFolderDeleted)
			);
			callbacks.afterProjectFolderDeleted = this.RootCallback(
				new NativeDelegates.BNProjectNotificationAfterItem(this.InvokeAfterFolderDeleted)
			);
			this.callbacksHandle = GCHandle.Alloc(callbacks, GCHandleType.Pinned);
		}

		~ProjectNotification()
		{
			if (this.callbacksHandle.IsAllocated)
			{
				this.callbacksHandle.Free();
			}
		}

		internal IntPtr CallbacksPointer
		{
			get { return this.callbacksHandle.AddrOfPinnedObject(); }
		}

		public virtual bool OnBeforeOpenProject(Project project) { return true; }
		public virtual void OnAfterOpenProject(Project project) { }
		public virtual bool OnBeforeCloseProject(Project project) { return true; }
		public virtual void OnAfterCloseProject(Project project) { }
		public virtual bool OnBeforeProjectMetadataWritten(Project project, string key, Metadata value) { return true; }
		public virtual void OnAfterProjectMetadataWritten(Project project, string key, Metadata value) { }
		public virtual bool OnBeforeProjectFileCreated(Project project, ProjectFile file) { return true; }
		public virtual void OnAfterProjectFileCreated(Project project, ProjectFile file) { }
		public virtual bool OnBeforeProjectFileUpdated(Project project, ProjectFile file) { return true; }
		public virtual void OnAfterProjectFileUpdated(Project project, ProjectFile file) { }
		public virtual bool OnBeforeProjectFileDeleted(Project project, ProjectFile file) { return true; }
		public virtual void OnAfterProjectFileDeleted(Project project, ProjectFile file) { }
		public virtual bool OnBeforeProjectFolderCreated(Project project, ProjectFolder folder) { return true; }
		public virtual void OnAfterProjectFolderCreated(Project project, ProjectFolder folder) { }
		public virtual bool OnBeforeProjectFolderUpdated(Project project, ProjectFolder folder) { return true; }
		public virtual void OnAfterProjectFolderUpdated(Project project, ProjectFolder folder) { }
		public virtual bool OnBeforeProjectFolderDeleted(Project project, ProjectFolder folder) { return true; }
		public virtual void OnAfterProjectFolderDeleted(Project project, ProjectFolder folder) { }

		private IntPtr RootCallback<TDelegate>(TDelegate callback)
			where TDelegate : Delegate
		{
			this.callbackRoots.Add(callback);
			return Marshal.GetFunctionPointerForDelegate<TDelegate>(callback);
		}

		private bool InvokeBeforeOpenProject(IntPtr context, IntPtr project)
		{
			return this.InvokeBeforeProject(project, true);
		}

		private bool InvokeBeforeCloseProject(IntPtr context, IntPtr project)
		{
			return this.InvokeBeforeProject(project, false);
		}

		private bool InvokeBeforeProject(IntPtr projectHandle, bool open)
		{
			try
			{
				using (Project project = Project.MustNewFromHandle(projectHandle))
				{
					return open ? this.OnBeforeOpenProject(project) : this.OnBeforeCloseProject(project);
				}
			}
			catch (Exception exception)
			{
				Core.LogError("Unhandled exception in project notification: {0}", exception);
				return false;
			}
		}

		private void InvokeAfterOpenProject(IntPtr context, IntPtr project)
		{
			this.InvokeAfterProject(project, true);
		}

		private void InvokeAfterCloseProject(IntPtr context, IntPtr project)
		{
			this.InvokeAfterProject(project, false);
		}

		private void InvokeAfterProject(IntPtr projectHandle, bool open)
		{
			try
			{
				using (Project project = Project.MustNewFromHandle(projectHandle))
				{
					if (open) { this.OnAfterOpenProject(project); }
					else { this.OnAfterCloseProject(project); }
				}
			}
			catch (Exception exception)
			{
				Core.LogError("Unhandled exception in project notification: {0}", exception);
			}
		}

		private bool InvokeBeforeMetadataWritten(IntPtr context, IntPtr projectHandle, IntPtr key, IntPtr value)
		{
			string managedKey = UnsafeUtils.TakeUtf8String(key);
			try
			{
				using (Project project = Project.MustNewFromHandle(projectHandle))
				using (Metadata metadata = Metadata.MustNewFromHandle(value))
				{
					return this.OnBeforeProjectMetadataWritten(project, managedKey, metadata);
				}
			}
			catch (Exception exception)
			{
				Core.LogError("Unhandled exception in project metadata notification: {0}", exception);
				return false;
			}
		}

		private void InvokeAfterMetadataWritten(IntPtr context, IntPtr projectHandle, IntPtr key, IntPtr value)
		{
			string managedKey = UnsafeUtils.TakeUtf8String(key);
			try
			{
				using (Project project = Project.MustNewFromHandle(projectHandle))
				using (Metadata metadata = Metadata.MustNewFromHandle(value))
				{
					this.OnAfterProjectMetadataWritten(project, managedKey, metadata);
				}
			}
			catch (Exception exception)
			{
				Core.LogError("Unhandled exception in project metadata notification: {0}", exception);
			}
		}

		private bool InvokeBeforeFileCreated(IntPtr c, IntPtr p, IntPtr i) { return this.InvokeBeforeItem(p, i, ItemKind.FileCreated); }
		private bool InvokeBeforeFileUpdated(IntPtr c, IntPtr p, IntPtr i) { return this.InvokeBeforeItem(p, i, ItemKind.FileUpdated); }
		private bool InvokeBeforeFileDeleted(IntPtr c, IntPtr p, IntPtr i) { return this.InvokeBeforeItem(p, i, ItemKind.FileDeleted); }
		private bool InvokeBeforeFolderCreated(IntPtr c, IntPtr p, IntPtr i) { return this.InvokeBeforeItem(p, i, ItemKind.FolderCreated); }
		private bool InvokeBeforeFolderUpdated(IntPtr c, IntPtr p, IntPtr i) { return this.InvokeBeforeItem(p, i, ItemKind.FolderUpdated); }
		private bool InvokeBeforeFolderDeleted(IntPtr c, IntPtr p, IntPtr i) { return this.InvokeBeforeItem(p, i, ItemKind.FolderDeleted); }
		private void InvokeAfterFileCreated(IntPtr c, IntPtr p, IntPtr i) { this.InvokeAfterItem(p, i, ItemKind.FileCreated); }
		private void InvokeAfterFileUpdated(IntPtr c, IntPtr p, IntPtr i) { this.InvokeAfterItem(p, i, ItemKind.FileUpdated); }
		private void InvokeAfterFileDeleted(IntPtr c, IntPtr p, IntPtr i) { this.InvokeAfterItem(p, i, ItemKind.FileDeleted); }
		private void InvokeAfterFolderCreated(IntPtr c, IntPtr p, IntPtr i) { this.InvokeAfterItem(p, i, ItemKind.FolderCreated); }
		private void InvokeAfterFolderUpdated(IntPtr c, IntPtr p, IntPtr i) { this.InvokeAfterItem(p, i, ItemKind.FolderUpdated); }
		private void InvokeAfterFolderDeleted(IntPtr c, IntPtr p, IntPtr i) { this.InvokeAfterItem(p, i, ItemKind.FolderDeleted); }

		private bool InvokeBeforeItem(IntPtr projectHandle, IntPtr itemHandle, ItemKind kind)
		{
			try
			{
				using (Project project = Project.MustNewFromHandle(projectHandle))
				{
					if (ItemKind.FileCreated == kind || ItemKind.FileUpdated == kind || ItemKind.FileDeleted == kind)
					{
						using (ProjectFile file = ProjectFile.MustNewFromHandle(itemHandle))
						{
							if (ItemKind.FileCreated == kind) { return this.OnBeforeProjectFileCreated(project, file); }
							if (ItemKind.FileUpdated == kind) { return this.OnBeforeProjectFileUpdated(project, file); }
							return this.OnBeforeProjectFileDeleted(project, file);
						}
					}

					using (ProjectFolder folder = ProjectFolder.MustNewFromHandle(itemHandle))
					{
						if (ItemKind.FolderCreated == kind) { return this.OnBeforeProjectFolderCreated(project, folder); }
						if (ItemKind.FolderUpdated == kind) { return this.OnBeforeProjectFolderUpdated(project, folder); }
						return this.OnBeforeProjectFolderDeleted(project, folder);
					}
				}
			}
			catch (Exception exception)
			{
				Core.LogError("Unhandled exception in project item notification: {0}", exception);
				return false;
			}
		}

		private void InvokeAfterItem(IntPtr projectHandle, IntPtr itemHandle, ItemKind kind)
		{
			try
			{
				using (Project project = Project.MustNewFromHandle(projectHandle))
				{
					if (ItemKind.FileCreated == kind || ItemKind.FileUpdated == kind || ItemKind.FileDeleted == kind)
					{
						using (ProjectFile file = ProjectFile.MustNewFromHandle(itemHandle))
						{
							if (ItemKind.FileCreated == kind) { this.OnAfterProjectFileCreated(project, file); }
							else if (ItemKind.FileUpdated == kind) { this.OnAfterProjectFileUpdated(project, file); }
							else { this.OnAfterProjectFileDeleted(project, file); }
							return;
						}
					}

					using (ProjectFolder folder = ProjectFolder.MustNewFromHandle(itemHandle))
					{
						if (ItemKind.FolderCreated == kind) { this.OnAfterProjectFolderCreated(project, folder); }
						else if (ItemKind.FolderUpdated == kind) { this.OnAfterProjectFolderUpdated(project, folder); }
						else { this.OnAfterProjectFolderDeleted(project, folder); }
					}
				}
			}
			catch (Exception exception)
			{
				Core.LogError("Unhandled exception in project item notification: {0}", exception);
			}
		}
	}
}
