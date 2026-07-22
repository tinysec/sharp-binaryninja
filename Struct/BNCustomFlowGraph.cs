using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class NativeDelegates
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNCustomFlowGraphEvent(IntPtr context);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate IntPtr BNCustomFlowGraphUpdate(IntPtr context);
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct BNCustomFlowGraph
	{
		/// <summary>
		/// void* context
		/// </summary>
		internal IntPtr context;
		
		/// <summary>
		/// void** prepareForLayout
		/// </summary>
		internal IntPtr prepareForLayout;
		
		/// <summary>
		/// void** populateNodes
		/// </summary>
		internal IntPtr populateNodes;
		
		/// <summary>
		/// void** completeLayout
		/// </summary>
		internal IntPtr completeLayout;
		
		/// <summary>
		/// void** update
		/// </summary>
		internal IntPtr update;
		
		/// <summary>
		/// void** freeObject
		/// </summary>
		internal IntPtr freeObject;
		
		/// <summary>
		/// void** externalRefTaken
		/// </summary>
		internal IntPtr externalRefTaken;
		
		/// <summary>
		/// void** externalRefReleased
		/// </summary>
		internal IntPtr externalRefReleased;
	}

	/// <summary>
	/// Retained for source compatibility. Custom graphs derive from FlowGraph directly.
	/// </summary>
	public class CustomFlowGraph
	{
	}
}
