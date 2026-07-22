using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class NativeDelegates
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNRenderLayerApplyToFlowGraph(
			IntPtr context,
			IntPtr graph
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNRenderLayerApplyToLinearViewObject(
			IntPtr context,
			IntPtr linearView,
			IntPtr previous,
			IntPtr next,
			IntPtr inputLines,
			ulong inputLineCount,
			IntPtr outputLines,
			IntPtr outputLineCount
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNRenderLayerFreeLines(
			IntPtr context,
			IntPtr lines,
			ulong count
		);
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct BNRenderLayerCallbacks
	{
		/// <summary>
		/// void* context
		/// </summary>
		internal IntPtr context;
		
		/// <summary>
		/// void** applyToFlowGraph
		/// </summary>
		internal IntPtr applyToFlowGraph;
		
		/// <summary>
		/// void** applyToLinearViewObject
		/// </summary>
		internal IntPtr applyToLinearViewObject;
		
		/// <summary>
		/// void** freeLines
		/// </summary>
		internal IntPtr freeLines;
	}

	/// <summary>
	/// Retained for source compatibility. Custom layers derive from RenderLayer directly.
	/// </summary>
	public class RenderLayerCallbacks
	{
	}
}
