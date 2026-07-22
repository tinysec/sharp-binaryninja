using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class NativeDelegates
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate IntPtr BNDownloadProviderCreateInstance(IntPtr context);
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct BNDownloadProviderCallbacks
	{
		/// <summary>
		/// void* context
		/// </summary>
		internal IntPtr context;
		
		/// <summary>
		/// void** createInstance
		/// </summary>
		internal IntPtr createInstance;
	}

	/// <summary>
	/// Retained for source compatibility. Custom providers use DownloadProvider directly.
	/// </summary>
	public class DownloadProviderCallbacks
	{
	}
}
