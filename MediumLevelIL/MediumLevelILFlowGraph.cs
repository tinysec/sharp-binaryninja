using System;

namespace BinaryNinja
{
	public sealed class MediumLevelILFlowGraph : AbstractFlowGraph<MediumLevelILFlowGraph>
	{
		internal MediumLevelILFunction ILFunction { get; set; }
		
		internal MediumLevelILFlowGraph(
			MediumLevelILFunction ilFunction,
			IntPtr handle ,
			bool owner
		) : base(handle , owner)
		{
			this.ILFunction = ilFunction;
		}
		
		internal static MediumLevelILFlowGraph? NewFromHandleEx(
			MediumLevelILFunction ilFunction,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new MediumLevelILFlowGraph(
				ilFunction,
				NativeMethods.BNNewFlowGraphReference(handle) ,
				true
			);
		}
	    
		internal static MediumLevelILFlowGraph MustNewFromHandleEx(
			MediumLevelILFunction ilFunction,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new MediumLevelILFlowGraph(
				ilFunction,
				NativeMethods.BNNewFlowGraphReference(handle) ,
				true
			);
		}
	    
		internal static MediumLevelILFlowGraph? TakeHandleEx(
			MediumLevelILFunction ilFunction,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new MediumLevelILFlowGraph(ilFunction , handle, true);
		}
	    
		internal static MediumLevelILFlowGraph MustTakeHandleEx(
			MediumLevelILFunction ilFunction,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new MediumLevelILFlowGraph(ilFunction , handle, true);
		}
	    
		internal static MediumLevelILFlowGraph? BorrowHandleEx(
			MediumLevelILFunction ilFunction,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new MediumLevelILFlowGraph(ilFunction , handle, false);
		}
	    
		internal static MediumLevelILFlowGraph MustBorrowHandleEx(
			MediumLevelILFunction ilFunction,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new MediumLevelILFlowGraph(ilFunction , handle, false);
		}
		
		
		public MediumLevelILFlowGraph? Update()
		{
			return MediumLevelILFlowGraph.TakeHandleEx(
				this.ILFunction,
				NativeMethods.BNUpdateFlowGraph(this.handle )
			);
		}
	    
		public MediumLevelILFlowGraphNode[] Nodes
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetFlowGraphNodes(
					this.handle,
					out ulong arrayLength
				);

				return UnsafeUtils.TakeHandleArrayEx<MediumLevelILFlowGraphNode>(
					arrayPointer ,
					arrayLength ,
					(_native) => MediumLevelILFlowGraphNode.MustNewFromHandleEx(this.ILFunction,_native) ,
					NativeMethods.BNFreeFlowGraphNodeList
				);
			}
		}

		public MediumLevelILFlowGraphNode? GetNode(ulong index)
		{
			return MediumLevelILFlowGraphNode.TakeHandleEx(
				this.ILFunction,
				NativeMethods.BNGetFlowGraphNode(this.handle, index)
			);
		}

	    
		public MediumLevelILFlowGraphNode[] GetNodesInRegion(
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

			return UnsafeUtils.TakeHandleArrayEx<MediumLevelILFlowGraphNode>(
				arrayPointer,
				arrayLength,
				(_native) => MediumLevelILFlowGraphNode.MustNewFromHandleEx(this.ILFunction,_native) ,
				NativeMethods.BNFreeFlowGraphNodeList
			);
		}
	}
}
