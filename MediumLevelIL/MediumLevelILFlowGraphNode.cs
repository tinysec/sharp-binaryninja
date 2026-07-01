using System;

namespace BinaryNinja
{
	public sealed class MediumLevelILFlowGraphNode : FlowGraphNode
	{
		internal MediumLevelILFunction ILFunction { get; set; }

		internal MediumLevelILFlowGraphNode(
			MediumLevelILFunction ilFunction ,
			IntPtr handle ,
			bool owner)
			: base(handle , owner)
		{
			this.ILFunction = ilFunction;
		}

		internal static MediumLevelILFlowGraphNode? NewFromHandleEx(
			MediumLevelILFunction ilFunction ,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}

			return new MediumLevelILFlowGraphNode(
				ilFunction ,
				NativeMethods.BNNewFlowGraphNodeReference(handle) ,
				true
			);
		}

		internal static MediumLevelILFlowGraphNode MustNewFromHandleEx(
			MediumLevelILFunction ilFunction ,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}

			return new MediumLevelILFlowGraphNode(
				ilFunction ,
				NativeMethods.BNNewFlowGraphNodeReference(handle) ,
				true
			);
		}

		internal static MediumLevelILFlowGraphNode? TakeHandleEx(
			MediumLevelILFunction ilFunction ,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}

			return new MediumLevelILFlowGraphNode(
				ilFunction ,
				handle ,
				true
			);
		}

		internal static MediumLevelILFlowGraphNode MustTakeHandleEx(
			MediumLevelILFunction ilFunction ,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}

			return new MediumLevelILFlowGraphNode(
				ilFunction ,
				handle ,
				true
			);
		}

		internal static MediumLevelILFlowGraphNode? BorrowHandleEx(
			MediumLevelILFunction ilFunction ,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}

			return new MediumLevelILFlowGraphNode(
				ilFunction ,
				handle ,
				false
			);
		}

		internal static MediumLevelILFlowGraphNode MustBorrowHandleEx(
			MediumLevelILFunction ilFunction ,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}

			return new MediumLevelILFlowGraphNode(
				ilFunction ,
				handle ,
				false
			);
		}


		public MediumLevelILBasicBlock? BasicBlock
		{
			get
			{
				return MediumLevelILBasicBlock.TakeHandleEx(
					this.ILFunction ,
					NativeMethods.BNGetFlowGraphBasicBlock(this.handle)
				);
			}

			set
			{
				NativeMethods.BNSetFlowGraphBasicBlock(
					this.handle ,
					null == value ? IntPtr.Zero : value.DangerousGetHandle()
				);
			}
		}

		public MediumLevelILFlowGraphEdge[] IncomingEdges
		{
			get
			{
				ulong arrayLength = 0;

				IntPtr arrayPointer = NativeMethods.BNGetFlowGraphNodeIncomingEdges(
					this.handle ,
					out arrayLength
				);

				return UnsafeUtils.TakeStructArrayEx<BNFlowGraphEdge , MediumLevelILFlowGraphEdge>(
					arrayPointer ,
					arrayLength ,
					(_native) => MediumLevelILFlowGraphEdge.FromNativeEx(this.ILFunction , _native) ,
					NativeMethods.BNFreeFlowGraphNodeEdgeList
				);
			}
		}

		public MediumLevelILFlowGraphEdge[] OutgoingEdges
		{
			get
			{
				ulong arrayLength = 0;

				IntPtr arrayPointer = NativeMethods.BNGetFlowGraphNodeOutgoingEdges(
					this.handle ,
					out arrayLength
				);

				return UnsafeUtils.TakeStructArrayEx<BNFlowGraphEdge , MediumLevelILFlowGraphEdge>(
					arrayPointer ,
					arrayLength ,
					(_native) => MediumLevelILFlowGraphEdge.FromNativeEx(this.ILFunction , _native) ,
					NativeMethods.BNFreeFlowGraphNodeEdgeList
				);
			}
		}
	}
}
