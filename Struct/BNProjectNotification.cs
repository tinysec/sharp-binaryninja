using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class NativeDelegates
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNProjectNotificationBeforeProject(IntPtr context, IntPtr project);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNProjectNotificationAfterProject(IntPtr context, IntPtr project);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNProjectNotificationBeforeMetadata(
			IntPtr context,
			IntPtr project,
			IntPtr key,
			IntPtr value
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNProjectNotificationAfterMetadata(
			IntPtr context,
			IntPtr project,
			IntPtr key,
			IntPtr value
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNProjectNotificationBeforeItem(
			IntPtr context,
			IntPtr project,
			IntPtr item
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNProjectNotificationAfterItem(
			IntPtr context,
			IntPtr project,
			IntPtr item
		);
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct BNProjectNotification
	{
		internal IntPtr context;
		internal IntPtr beforeOpenProject;
		internal IntPtr afterOpenProject;
		internal IntPtr beforeCloseProject;
		internal IntPtr afterCloseProject;
		internal IntPtr beforeProjectMetadataWritten;
		internal IntPtr afterProjectMetadataWritten;
		internal IntPtr beforeProjectFileCreated;
		internal IntPtr afterProjectFileCreated;
		internal IntPtr beforeProjectFileUpdated;
		internal IntPtr afterProjectFileUpdated;
		internal IntPtr beforeProjectFileDeleted;
		internal IntPtr afterProjectFileDeleted;
		internal IntPtr beforeProjectFolderCreated;
		internal IntPtr afterProjectFolderCreated;
		internal IntPtr beforeProjectFolderUpdated;
		internal IntPtr afterProjectFolderUpdated;
		internal IntPtr beforeProjectFolderDeleted;
		internal IntPtr afterProjectFolderDeleted;
	}
}
