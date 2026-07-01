using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNFlowGraphEdge 
	{
		/// <summary>
		/// BNBranchType type
		/// </summary>
		internal BranchType type;
		
		/// <summary>
		/// BNFlowGraphNode* target
		/// </summary>
		internal IntPtr target;
		
		/// <summary>
		/// BNPoint* points
		/// </summary>
		internal IntPtr points;
		
		/// <summary>
		/// uint64_t pointCount
		/// </summary>
		internal ulong pointCount;
		
		/// <summary>
		/// bool backEdge
		/// </summary>
		internal bool backEdge;
		
		/// <summary>
		/// BNEdgeStyle style
		/// </summary>
		internal BNEdgeStyle style;
	}

    public abstract class AbstractFlowGraphEdge<T_FLOW_GRAPH_NODE>  : INativeWrapperEx<BNFlowGraphEdge>
		where T_FLOW_GRAPH_NODE :  FlowGraphNode
    {
		public BranchType Type { get; set; } = new BranchType();
		
		public T_FLOW_GRAPH_NODE? Target { get; set; } = null;
		
		public Point[] Points { get; set; } = Array.Empty<Point>();
		
		public bool BackEdge { get; set; } = false;
		
		public EdgeStyle Style { get; set; } = new EdgeStyle();
		
		public AbstractFlowGraphEdge(BNFlowGraphEdge native , T_FLOW_GRAPH_NODE? target ) 
		{
		    this.Type = native.type ;
		    
		    this.Target = target ;

		    this.Points = UnsafeUtils.ReadStructArray<BNPoint , Point>(
			    native.points ,
			    native.pointCount ,
			    Point.FromNative
		    );
		    this.BackEdge = native.backEdge;
		    this.Style = EdgeStyle.FromNative(native.style) ;
		}

		public BNFlowGraphEdge ToNativeEx(ScopedAllocator allocator)
		{
			return new BNFlowGraphEdge()
			{
				type = this.Type ,
				target = ( null == this.Target ? IntPtr.Zero : this.Target.DangerousGetHandle() ) ,
				points = (
					0 == this.Points.Length
						? IntPtr.Zero
						: allocator.AllocStructArray<BNPoint>(
							UnsafeUtils.ConvertToNativeArray<BNPoint , Point>(this.Points)
						)
				) ,
				pointCount = (ulong)this.Points.Length,
				backEdge = this.BackEdge,
				style = this.Style.ToNative(),
			};
		}
    }
}