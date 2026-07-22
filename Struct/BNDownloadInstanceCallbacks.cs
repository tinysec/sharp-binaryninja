using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class NativeDelegates
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNDownloadInstanceDestroy(IntPtr context);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate int BNDownloadInstancePerformRequest(
			IntPtr context,
			[MarshalAs(UnmanagedType.LPUTF8Str)] string url
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate int BNDownloadInstancePerformCustomRequest(
			IntPtr context,
			[MarshalAs(UnmanagedType.LPUTF8Str)] string method,
			[MarshalAs(UnmanagedType.LPUTF8Str)] string url,
			ulong headerCount,
			IntPtr headerKeys,
			IntPtr headerValues,
			IntPtr response
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNDownloadInstanceFreeResponse(
			IntPtr context,
			IntPtr response
		);
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct BNDownloadInstanceCallbacks
	{
		/// <summary>
		/// void* context
		/// </summary>
		internal IntPtr context;
		
		/// <summary>
		/// void** destroyInstance
		/// </summary>
		internal IntPtr destroyInstance;
		
		/// <summary>
		/// void** performRequest
		/// </summary>
		internal IntPtr performRequest;
		
		/// <summary>
		/// void** performCustomRequest
		/// </summary>
		internal IntPtr performCustomRequest;
		
		/// <summary>
		/// void** freeResponse
		/// </summary>
		internal IntPtr freeResponse;
	}

	/// <summary>
	/// Retained for source compatibility. Custom instances use DownloadInstance directly.
	/// </summary>
	public class DownloadInstanceCallbacks
	{
	}
}
