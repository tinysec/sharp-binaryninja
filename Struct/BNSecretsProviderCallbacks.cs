using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class NativeDelegates
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNSecretsProviderHasData(
			IntPtr context,
			[MarshalAs(UnmanagedType.LPUTF8Str)] string key
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate IntPtr BNSecretsProviderGetData(
			IntPtr context,
			[MarshalAs(UnmanagedType.LPUTF8Str)] string key
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNSecretsProviderStoreData(
			IntPtr context,
			[MarshalAs(UnmanagedType.LPUTF8Str)] string key,
			[MarshalAs(UnmanagedType.LPUTF8Str)] string data
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNSecretsProviderDeleteData(
			IntPtr context,
			[MarshalAs(UnmanagedType.LPUTF8Str)] string key
		);
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct BNSecretsProviderCallbacks
	{
		internal IntPtr context;

		internal IntPtr hasData;

		internal IntPtr getData;

		internal IntPtr storeData;

		internal IntPtr deleteData;
	}
}
