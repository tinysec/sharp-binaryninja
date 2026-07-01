using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryNinja
{
	public sealed class HighLevelILBasicBlock : AbstractBasicBlock<HighLevelILBasicBlock>
	{
		internal HighLevelILFunction ILFunction { get; } 

		internal HighLevelILBasicBlock(
			HighLevelILFunction function ,
			IntPtr handle , 
			bool owner
		) : base(handle , owner)
		{
			this.ILFunction = function;
		}
		
		internal static HighLevelILBasicBlock MustNewFromHandleEx(
			HighLevelILFunction function ,
			IntPtr handle 
		)
		{
			return new HighLevelILBasicBlock(
				function,
				NativeMethods.BNNewBasicBlockReference(handle) ,
				true
			);
		}
	    
		internal static HighLevelILBasicBlock? TakeHandleEx(
			HighLevelILFunction function ,
			IntPtr handle ,
			bool ssa = false
		)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new HighLevelILBasicBlock(
				function,
				handle, 
				true
			);
		}

		
		public HighLevelILInstruction this[HighLevelILInstructionIndex index]
		{
			get
			{
				return this.MustGetInstructionByInstructionIndex(index);
			}
		}
		
		public HighLevelILInstruction? GetInstructionByInstructionIndex(HighLevelILInstructionIndex index)
		{
			if ( (ulong)index < this.Start)
			{
				return null;
			}
		    
			if ( (ulong)index >= this.End)
			{
				return null;
			}
		    
			return this.ILFunction.GetInstruction(index);
		}
	    
		public HighLevelILInstruction MustGetInstructionByInstructionIndex(HighLevelILInstructionIndex index)
		{
			if ( (ulong)index < this.Start)
			{
				throw new IndexOutOfRangeException(nameof(index));
			}
		    
			if ( (ulong)index >= this.End)
			{
				throw new IndexOutOfRangeException(nameof(index));
			}
		    
			return this.ILFunction.MustGetInstruction(index);
		}
	    
		public IEnumerable<HighLevelILInstruction> Instructions
		{
			get
			{
				for (ulong index = (ulong)this.Start; index < (ulong)this.End; index++)
				{
					yield return this.MustGetInstructionByInstructionIndex((HighLevelILInstructionIndex)index);
				}
			}
		}
		
		public bool ContainsInstruction(HighLevelILInstructionIndex index)
		{
			return (ulong)index >= (ulong)this.Start && (ulong)index < (ulong)this.End;
		}
		
		public HighLevelILBasicBlock? SourceBlock
		{
			get
			{
				return HighLevelILBasicBlock.TakeHandleEx(
					this.ILFunction ,
					NativeMethods.BNGetBasicBlockSourceBlock(this.handle)
				);
			}
		}
		
	    public HighLevelILBasicBlockEdge[] OutgoingEdges
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockOutgoingEdges(
				    this.handle , 
				    out ulong arrayLength
				);
			    
			    return UnsafeUtils.TakeStructArrayEx<BNBasicBlockEdge,HighLevelILBasicBlockEdge>(
				    arrayPointer ,
				    arrayLength ,
				    (_native) => HighLevelILBasicBlockEdge.FromNativeEx(
					    _native ,
					    this ,
					    true
				    ) ,
				    NativeMethods.BNFreeBasicBlockEdgeList
			    );
		    }
	    }
	    
	    public HighLevelILBasicBlockEdge[] IncomingEdges
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockIncomingEdges(
				    this.handle , 
				    out ulong arrayLength
				);
			
			    return UnsafeUtils.TakeStructArrayEx<BNBasicBlockEdge,HighLevelILBasicBlockEdge>(
				    arrayPointer ,
				    arrayLength ,
				    (_native) => HighLevelILBasicBlockEdge.FromNativeEx(
					    _native ,
					    this ,
					    false
				    ) ,
				    NativeMethods.BNFreeBasicBlockEdgeList
			    );
		    }
	    }
	    
	    public HighLevelILBasicBlock[] GetDominators(bool post)
	    {
		    ulong arrayLength = 0;
			    
		    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockDominators(
			    this.handle , 
			    out arrayLength ,
			    post
		    );
			    
		    return UnsafeUtils.TakeHandleArrayEx<HighLevelILBasicBlock>(
			    arrayPointer ,
			    arrayLength , 
			    (_native) => HighLevelILBasicBlock.MustNewFromHandleEx(
				    this.ILFunction , 
				    _native
				),
			    NativeMethods.BNFreeBasicBlockList
		    );
	    }

	    public HighLevelILBasicBlock[] Dominators
	    {
		    get
		    {
			    return this.GetDominators(false);
		    }
	    }
	    
	    public HighLevelILBasicBlock[] PostDominators
	    {
		    get
		    {
			    return this.GetDominators(true);
		    }
	    }
	    
	    public HighLevelILBasicBlock[] GetStrictDominators(bool post)
	    {
		    ulong arrayLength = 0;
			    
		    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockStrictDominators(
			    this.handle , 
			    out arrayLength ,
			    post
		    );
			    
		    return UnsafeUtils.TakeHandleArrayEx<HighLevelILBasicBlock>(
			    arrayPointer ,
			    arrayLength , 
			    (_native) => HighLevelILBasicBlock.MustNewFromHandleEx(
				    this.ILFunction , 
				    _native
				),
			    NativeMethods.BNFreeBasicBlockList
		    );
	    }
	    
	    public HighLevelILBasicBlock[] StrictDominators
	    {
		    get
		    {
			    return this.GetStrictDominators(false);
		    }
	    }
	    
	    public HighLevelILBasicBlock[] StrictPostDominators
	    {
		    get
		    {
			    return this.GetStrictDominators(true);
		    }
	    }
	    
	    
	    public HighLevelILBasicBlock? GetImmediateDominator(bool post)
	    {
		    return HighLevelILBasicBlock.TakeHandleEx(
			    this.ILFunction,
			    NativeMethods.BNGetBasicBlockImmediateDominator(this.handle , post)
			);
	    }
	    
	    public HighLevelILBasicBlock? ImmediateDominator
	    {
		    get
		    {
			    return this.GetImmediateDominator(false);
		    }
	    }
	    
	    public HighLevelILBasicBlock? ImmediatePostDominator
	    {
		    get
		    {
			    return this.GetImmediateDominator(true);
		    }
	    }
	    
	    public HighLevelILBasicBlock[] GetDominatorTreeChildren(bool post)
	    {
			 ulong arrayLength = 0;
		    
		    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockDominatorTreeChildren(
			    this.handle , 
			    out arrayLength ,
			    post
		    );
		    
		    return UnsafeUtils.TakeHandleArrayEx<HighLevelILBasicBlock>(
			    arrayPointer ,
			    arrayLength , 
			    (_native) => HighLevelILBasicBlock.MustNewFromHandleEx(this.ILFunction , _native),
			    NativeMethods.BNFreeBasicBlockList
		    );
	    }
	    
	    public HighLevelILBasicBlock[] DominatorTreeChildren
	    {
		    get
		    {
			    return this.GetDominatorTreeChildren(false);
		    }
	    }
	    
	    public HighLevelILBasicBlock[] PostDominatorTreeChildren
	    {
		    get
		    {
			    return this.GetDominatorTreeChildren(true);
		    }
	    }
	    
	    
	    public HighLevelILBasicBlock[] GetDominanceFrontier(bool post)
	    {
		    ulong arrayLength = 0;
		    
		    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockDominanceFrontier(
			    this.handle , 
			    out arrayLength ,
			    post
		    );
		    
		    return UnsafeUtils.TakeHandleArrayEx<HighLevelILBasicBlock>(
			    arrayPointer ,
			    arrayLength , 
			    (_native) => HighLevelILBasicBlock.MustNewFromHandleEx(this.ILFunction , _native),
			    NativeMethods.BNFreeBasicBlockList
		    );
	    }
	    
	    public HighLevelILBasicBlock[] DominanceFrontier
	    {
		    get
		    {
			    return this.GetDominanceFrontier(false);
		    }
	    }
	    
	    public HighLevelILBasicBlock[] PostDominanceFrontier
	    {
		    get
		    {
			    return this.GetDominanceFrontier(true);
		    }
	    }
	    
	}
}
