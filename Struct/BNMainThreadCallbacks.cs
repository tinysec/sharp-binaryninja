using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class NativeDelegates
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNMainThreadAddAction(IntPtr context, IntPtr action);
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct BNMainThreadCallbacks
	{
		/// <summary>
		/// void* context
		/// </summary>
		internal IntPtr context;
		
		/// <summary>
		/// void (*addAction)(void* ctxt, BNMainThreadAction* action)
		/// </summary>
		internal IntPtr addAction;
	}
}
