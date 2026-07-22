using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class NativeDelegates
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNRelocationHandlerFreeObject(IntPtr context);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNRelocationHandlerGetRelocationInfo(
			IntPtr context,
			IntPtr view,
			IntPtr architecture,
			IntPtr result,
			ulong resultCount
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNRelocationHandlerApplyRelocation(
			IntPtr context,
			IntPtr view,
			IntPtr architecture,
			IntPtr relocation,
			IntPtr destination,
			ulong length
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate ulong BNRelocationHandlerGetOperandForExternalRelocation(
			IntPtr context,
			IntPtr data,
			ulong address,
			ulong length,
			IntPtr lowLevelIl,
			IntPtr relocation
		);
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct BNCustomRelocationHandler
	{
		internal IntPtr context;
		internal IntPtr freeObject;
		internal IntPtr getRelocationInfo;
		internal IntPtr applyRelocation;
		internal IntPtr getOperandForExternalRelocation;
	}

	/// <summary>
	/// Retained for source compatibility. Custom handlers derive from RelocationHandler.
	/// </summary>
	public class CustomRelocationHandler
	{
	}
}
