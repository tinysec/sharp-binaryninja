using System;

namespace BinaryNinja
{
	public sealed class LowLevelILFlowGraph : AbstractFlowGraph<LowLevelILFlowGraph>
	{
		internal LowLevelILFunction ILFunction { get; set; }
		
		internal LowLevelILFlowGraph(
			LowLevelILFunction ilFunction,
			IntPtr handle ,
			bool owner
		) : base(handle , owner)
		{
			this.ILFunction = ilFunction;
		}
		
		internal static LowLevelILFlowGraph? NewFromHandleEx(
			LowLevelILFunction ilFunction,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new LowLevelILFlowGraph(
				ilFunction,
				NativeMethods.BNNewFlowGraphReference(handle) ,
				true
			);
		}
	    
		internal static LowLevelILFlowGraph MustNewFromHandleEx(
			LowLevelILFunction ilFunction,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new LowLevelILFlowGraph(
				ilFunction,
				NativeMethods.BNNewFlowGraphReference(handle) ,
				true
			);
		}
	    
		internal static LowLevelILFlowGraph? TakeHandleEx(
			LowLevelILFunction ilFunction,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new LowLevelILFlowGraph(ilFunction , handle, true);
		}
	    
		internal static LowLevelILFlowGraph MustTakeHandleEx(
			LowLevelILFunction ilFunction,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new LowLevelILFlowGraph(ilFunction , handle, true);
		}
	    
		internal static LowLevelILFlowGraph? BorrowHandleEx(
			LowLevelILFunction ilFunction,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new LowLevelILFlowGraph(ilFunction , handle, false);
		}
	    
		internal static LowLevelILFlowGraph MustBorrowHandleEx(
			LowLevelILFunction ilFunction,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new LowLevelILFlowGraph(ilFunction , handle, false);
		}
		
		
		public LowLevelILFlowGraph? Update()
		{
			return LowLevelILFlowGraph.TakeHandleEx(
				this.ILFunction,
				NativeMethods.BNUpdateFlowGraph(this.handle )
			);
		}
	    
		public LowLevelILFlowGraphNode[] Nodes
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetFlowGraphNodes(
					this.handle,
					out ulong arrayLength
				);

				return UnsafeUtils.TakeHandleArrayEx<LowLevelILFlowGraphNode>(
					arrayPointer ,
					arrayLength ,
					(_native) => LowLevelILFlowGraphNode.MustNewFromHandleEx(this.ILFunction,_native) ,
					NativeMethods.BNFreeFlowGraphNodeList
				);
			}
		}

		public LowLevelILFlowGraphNode? GetNode(ulong index)
		{
			return LowLevelILFlowGraphNode.TakeHandleEx(
				this.ILFunction,
				NativeMethods.BNGetFlowGraphNode(this.handle, index)
			);
		}

	    
		public LowLevelILFlowGraphNode[] GetNodesInRegion(
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

			return UnsafeUtils.TakeHandleArrayEx<LowLevelILFlowGraphNode>(
				arrayPointer,
				arrayLength,
				(_native) => LowLevelILFlowGraphNode.MustNewFromHandleEx(this.ILFunction,_native) ,
				NativeMethods.BNFreeFlowGraphNodeList
			);
		}
	}
}
