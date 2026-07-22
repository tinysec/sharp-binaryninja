using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class NativeDelegates
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNFlowGraphLayout(
			IntPtr context,
			IntPtr graph,
			IntPtr nodes,
			ulong nodeCount
		);
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct BNCustomFlowGraphLayout
	{
		/// <summary>
		/// void* context
		/// </summary>
		public IntPtr context;
		
		/// <summary>
		/// void** layout
		/// </summary>
		public IntPtr layout;
	}

	public class CustomFlowGraphLayout
    {
		public CustomFlowGraphLayout() 
		{
		    
		}
    }
}
