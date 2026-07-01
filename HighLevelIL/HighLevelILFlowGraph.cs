using System;

namespace BinaryNinja
{
	public sealed class HighLevelILFlowGraph : AbstractFlowGraph<HighLevelILFlowGraph>
	{
		internal HighLevelILFunction ILFunction { get; set; }
		
		internal HighLevelILFlowGraph(
			HighLevelILFunction ilFunction,
			IntPtr handle ,
			bool owner
		) : base(handle , owner)
		{
			this.ILFunction = ilFunction;
		}
		
		internal static HighLevelILFlowGraph? NewFromHandleEx(
			HighLevelILFunction ilFunction,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new HighLevelILFlowGraph(
				ilFunction,
				NativeMethods.BNNewFlowGraphReference(handle) ,
				true
			);
		}
	    
		internal static HighLevelILFlowGraph MustNewFromHandleEx(
			HighLevelILFunction ilFunction,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new HighLevelILFlowGraph(
				ilFunction,
				NativeMethods.BNNewFlowGraphReference(handle) ,
				true
			);
		}
	    
		internal static HighLevelILFlowGraph? TakeHandleEx(
			HighLevelILFunction ilFunction,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new HighLevelILFlowGraph(ilFunction , handle, true);
		}
	    
		internal static HighLevelILFlowGraph MustTakeHandleEx(
			HighLevelILFunction ilFunction,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new HighLevelILFlowGraph(ilFunction , handle, true);
		}
	    
		internal static HighLevelILFlowGraph? BorrowHandleEx(
			HighLevelILFunction ilFunction,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new HighLevelILFlowGraph(ilFunction , handle, false);
		}
	    
		internal static HighLevelILFlowGraph MustBorrowHandleEx(
			HighLevelILFunction ilFunction,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new HighLevelILFlowGraph(ilFunction , handle, false);
		}
		
		
		
		public HighLevelILFlowGraph? Update()
		{
			return HighLevelILFlowGraph.TakeHandleEx(
				this.ILFunction,
				NativeMethods.BNUpdateFlowGraph(this.handle )
			);
		}
	    
		public HighLevelILFlowGraphNode[] Nodes
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetFlowGraphNodes(
					this.handle,
					out ulong arrayLength
				);

				return UnsafeUtils.TakeHandleArrayEx<HighLevelILFlowGraphNode>(
					arrayPointer ,
					arrayLength ,
					(_native) => HighLevelILFlowGraphNode.MustNewFromHandleEx(this.ILFunction,_native) ,
					NativeMethods.BNFreeFlowGraphNodeList
				);
			}
		}

		public HighLevelILFlowGraphNode? GetNode(ulong index)
		{
			return HighLevelILFlowGraphNode.TakeHandleEx(
				this.ILFunction,
				NativeMethods.BNGetFlowGraphNode(this.handle, index)
			);
		}

	    
		public HighLevelILFlowGraphNode[] GetNodesInRegion(
			int left  , 
			int top  , 
			int right  , 
			int bottom
		)
		{
			IntPtr arrayPointer = NativeMethods.BNGetFlowGraphNodesInRegion(
				this.handle,
				left,
				top,
				right,
				bottom,
				out ulong arrayLength
			);

			return UnsafeUtils.TakeHandleArrayEx<HighLevelILFlowGraphNode>(
				arrayPointer,
				arrayLength,
				(_native) => HighLevelILFlowGraphNode.MustNewFromHandleEx(this.ILFunction,_native) ,
				NativeMethods.BNFreeFlowGraphNodeList
			);
		}
	}
}
