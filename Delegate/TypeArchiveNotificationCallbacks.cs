using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class NativeDelegates
	{
		// void (*typeAdded)(void* ctxt, BNTypeArchive* archive, const char* id, BNType* definition)
		public delegate void TypeArchiveTypeAddedDelegate(
			IntPtr context, IntPtr archive, IntPtr id, IntPtr definition);

		// void (*typeUpdated)(void* ctxt, BNTypeArchive* archive, const char* id, BNType* oldDefinition, BNType* newDefinition)
		public delegate void TypeArchiveTypeUpdatedDelegate(
			IntPtr context, IntPtr archive, IntPtr id, IntPtr oldDefinition, IntPtr newDefinition);

		// void (*typeRenamed)(void* ctxt, BNTypeArchive* archive, const char* id, BNQualifiedName* oldName, BNQualifiedName* newName)
		public delegate void TypeArchiveTypeRenamedDelegate(
			IntPtr context, IntPtr archive, IntPtr id, IntPtr oldName, IntPtr newName);

		// void (*typeDeleted)(void* ctxt, BNTypeArchive* archive, const char* id, BNType* definition)
		public delegate void TypeArchiveTypeDeletedDelegate(
			IntPtr context, IntPtr archive, IntPtr id, IntPtr definition);
	}

	internal static partial class UnsafeUtils
	{
		// Adapts a TypeArchiveNotification into the four native TypeArchive callback shapes. Each
		// method marshals the native arguments into managed objects and forwards to the user's
		// virtual override. The core invokes the function pointers stored on the
		// BNTypeArchiveNotification struct; this context is rooted by those delegates (see
		// TypeArchiveNotificationContext.BuildNative), so it -- and the notification it holds --
		// stays alive for the registration lifetime.
		internal sealed class TypeArchiveNotificationContext
		{
			internal readonly TypeArchiveNotification Notification;

			// The allocated BNTypeArchiveNotification struct (IntPtr to AllocHGlobal memory), kept
			// alive until FreeNative so the core-held registration never sees freed memory.
			internal IntPtr NativeStruct = IntPtr.Zero;

			// Rooting the four wrapper delegates on this context prevents the GC from reclaiming
			// their function pointers while the core holds the registration.
			private NativeDelegates.TypeArchiveTypeAddedDelegate? m_added;
			private NativeDelegates.TypeArchiveTypeUpdatedDelegate? m_updated;
			private NativeDelegates.TypeArchiveTypeRenamedDelegate? m_renamed;
			private NativeDelegates.TypeArchiveTypeDeletedDelegate? m_deleted;

			// The GCHandle stored as the struct's context field; freed on unregistration. The
			// delegates already root this context, but the handle makes the rooting explicit and
			// gives the core a stable context value to pass back.
			private GCHandle m_selfHandle;

			internal TypeArchiveNotificationContext(TypeArchiveNotification notification)
			{
				this.Notification = notification;
			}

			// Marshals the native arguments and forwards to the user override. A throwing override
			// must not abort the core's notification dispatch, so exceptions are swallowed (Python
			// logs and continues).
			internal void OnTypeAdded(IntPtr context, IntPtr archive, IntPtr id, IntPtr definition)
			{
				try
				{
					TypeArchive? archiveObject = TypeArchive.BorrowHandle(archive);
					if (null == archiveObject)
					{
						return;
					}

					string idString = PtrToAnsiString(id);
					Type? definitionType = Type.NewFromHandle(definition);

					this.Notification.OnTypeAdded(archiveObject, idString, definitionType);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnTypeUpdated(
				IntPtr context, IntPtr archive, IntPtr id, IntPtr oldDefinition, IntPtr newDefinition)
			{
				try
				{
					TypeArchive? archiveObject = TypeArchive.BorrowHandle(archive);
					if (null == archiveObject)
					{
						return;
					}

					string idString = PtrToAnsiString(id);
					Type? oldType = Type.NewFromHandle(oldDefinition);
					Type? newType = Type.NewFromHandle(newDefinition);

					this.Notification.OnTypeUpdated(archiveObject, idString, oldType, newType);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnTypeRenamed(
				IntPtr context, IntPtr archive, IntPtr id, IntPtr oldName, IntPtr newName)
			{
				try
				{
					TypeArchive? archiveObject = TypeArchive.BorrowHandle(archive);
					if (null == archiveObject)
					{
						return;
					}

					string idString = PtrToAnsiString(id);
					QualifiedName oldNameValue = QualifiedNameFromPtr(oldName);
					QualifiedName newNameValue = QualifiedNameFromPtr(newName);

					this.Notification.OnTypeRenamed(archiveObject, idString, oldNameValue, newNameValue);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			internal void OnTypeDeleted(IntPtr context, IntPtr archive, IntPtr id, IntPtr definition)
			{
				try
				{
					TypeArchive? archiveObject = TypeArchive.BorrowHandle(archive);
					if (null == archiveObject)
					{
						return;
					}

					string idString = PtrToAnsiString(id);
					Type? definitionType = Type.NewFromHandle(definition);

					this.Notification.OnTypeDeleted(archiveObject, idString, definitionType);
				}
				catch (Exception)
				{
					// Swallowed: a failing callback must not crash the core dispatch.
				}
			}

			// Allocates the BNTypeArchiveNotification struct, sets its context and the four function
			// pointers (taken from the rooted wrapper delegates), and returns the struct pointer.
			// Idempotent: a second call returns the existing struct.
			internal IntPtr BuildNative()
			{
				if (IntPtr.Zero != this.NativeStruct)
				{
					return this.NativeStruct;
				}

				// 1. Create the four wrapper delegates bound to this context and root them as fields.
				this.m_added = new NativeDelegates.TypeArchiveTypeAddedDelegate(this.OnTypeAdded);
				this.m_updated = new NativeDelegates.TypeArchiveTypeUpdatedDelegate(this.OnTypeUpdated);
				this.m_renamed = new NativeDelegates.TypeArchiveTypeRenamedDelegate(this.OnTypeRenamed);
				this.m_deleted = new NativeDelegates.TypeArchiveTypeDeletedDelegate(this.OnTypeDeleted);

				// 2. Allocate a stable context handle so the core has a value to pass back, and so
				// this context is explicitly rooted.
				this.m_selfHandle = GCHandle.Alloc(this, GCHandleType.Normal);

				// 3. Build the struct: context + four function pointers.
				BNTypeArchiveNotification native = new BNTypeArchiveNotification();
				native.context = GCHandle.ToIntPtr(this.m_selfHandle);
				native.typeAdded = Marshal.GetFunctionPointerForDelegate(this.m_added);
				native.typeUpdated = Marshal.GetFunctionPointerForDelegate(this.m_updated);
				native.typeRenamed = Marshal.GetFunctionPointerForDelegate(this.m_renamed);
				native.typeDeleted = Marshal.GetFunctionPointerForDelegate(this.m_deleted);

				this.NativeStruct = Marshal.AllocHGlobal(Marshal.SizeOf<BNTypeArchiveNotification>());
				Marshal.StructureToPtr<BNTypeArchiveNotification>(native, this.NativeStruct, false);

				return this.NativeStruct;
			}

			// Frees the struct and the context handle. Called after the core has unregistered.
			internal void FreeNative()
			{
				if (IntPtr.Zero != this.NativeStruct)
				{
					Marshal.DestroyStructure<BNTypeArchiveNotification>(this.NativeStruct);
					Marshal.FreeHGlobal(this.NativeStruct);
					this.NativeStruct = IntPtr.Zero;
				}

				if (this.m_selfHandle.IsAllocated)
				{
					this.m_selfHandle.Free();
				}
			}

			private static string PtrToAnsiString(IntPtr pointer)
			{
				if (IntPtr.Zero == pointer)
				{
					return string.Empty;
				}

				return Marshal.PtrToStringAnsi(pointer) ?? string.Empty;
			}

			// Reads a BNQualifiedName out of a pointer the core passes (the archive owns the name
			// storage, so it is read without freeing, matching Python's _from_core_struct).
			private static QualifiedName QualifiedNameFromPtr(IntPtr pointer)
			{
				if (IntPtr.Zero == pointer)
				{
					return new QualifiedName(string.Empty);
				}

				BNQualifiedName native = Marshal.PtrToStructure<BNQualifiedName>(pointer);

				return QualifiedName.FromNative(native);
			}
		}
	}
}
