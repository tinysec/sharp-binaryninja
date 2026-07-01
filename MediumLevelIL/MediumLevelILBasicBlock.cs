using System;
using System.Collections.Generic;

namespace BinaryNinja
{
	public sealed class MediumLevelILBasicBlock : AbstractBasicBlock<MediumLevelILBasicBlock>
	{
		internal MediumLevelILFunction ILFunction { get; }
		
		internal MediumLevelILBasicBlock(
			MediumLevelILFunction function ,
			IntPtr handle , 
			bool owner
		) : base(handle , owner)
		{
			this.ILFunction = function;
		}
		
		internal static MediumLevelILBasicBlock MustNewFromHandleEx(
			MediumLevelILFunction function ,
			IntPtr handle)
		{
			return new MediumLevelILBasicBlock(
				function,
				NativeMethods.BNNewBasicBlockReference(handle) ,
				true
			);
		}
	    
		internal static MediumLevelILBasicBlock? TakeHandleEx(
			MediumLevelILFunction function ,
			IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new MediumLevelILBasicBlock(
				function,
				handle,
				true
			);
		}
	
		public MediumLevelILInstruction this[MediumLevelILInstructionIndex index]
		{
			get
			{
				return this.MustGetInstructionByInstructionIndex(index);
			}
		}
	    
		public MediumLevelILInstruction? GetInstructionByInstructionIndex(MediumLevelILInstructionIndex index)
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
	    
		public MediumLevelILInstruction MustGetInstructionByInstructionIndex(MediumLevelILInstructionIndex index)
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
	    
		public IEnumerable<MediumLevelILInstruction> Instructions
		{
			get
			{
				for (ulong index = (ulong)this.Start; index < (ulong)this.End; index++)
				{
					yield return this.MustGetInstructionByInstructionIndex((MediumLevelILInstructionIndex)index);
				}
			}
		}

		public bool ContainsInstruction(MediumLevelILInstructionIndex index)
		{
			return (ulong)index >= (ulong)this.Start && (ulong)index < (ulong)this.End;
		}
		
		public MediumLevelILBasicBlock? SourceBlock
		{
			get
			{
				return MediumLevelILBasicBlock.TakeHandleEx(
					this.ILFunction ,
					NativeMethods.BNGetBasicBlockSourceBlock(this.handle)
				);
			}
		}
		
	    public MediumLevelILBasicBlockEdge[] OutgoingEdges
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockOutgoingEdges(
				    this.handle , 
				    out ulong arrayLength
				);
			
			    return UnsafeUtils.TakeStructArrayEx<BNBasicBlockEdge,MediumLevelILBasicBlockEdge>(
				    arrayPointer ,
				    arrayLength ,
				    (_native) => MediumLevelILBasicBlockEdge.FromNativeEx(
					    _native ,
					    this ,
					    true
				    ) ,
				    NativeMethods.BNFreeBasicBlockEdgeList
			    );
		    }
	    }
	    
	    public MediumLevelILBasicBlockEdge[] IncomingEdges
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockIncomingEdges(
				    this.handle , 
				    out ulong arrayLength
				);
			
			    return UnsafeUtils.TakeStructArrayEx<BNBasicBlockEdge,MediumLevelILBasicBlockEdge>(
				    arrayPointer ,
				    arrayLength ,
				    (_native) => MediumLevelILBasicBlockEdge.FromNativeEx(
					    _native ,
					    this ,
					    false
				    ) ,
				    NativeMethods.BNFreeBasicBlockEdgeList
			    );
		    }
	    }
	    
	    public MediumLevelILBasicBlock[] GetDominators(bool post)
	    {
		    ulong arrayLength = 0;
			    
		    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockDominators(
			    this.handle , 
			    out arrayLength ,
			    post
		    );
			    
		    return UnsafeUtils.TakeHandleArrayEx<MediumLevelILBasicBlock>(
			    arrayPointer ,
			    arrayLength , 
			    (_native) => MediumLevelILBasicBlock.MustNewFromHandleEx(this.ILFunction , _native),
			    NativeMethods.BNFreeBasicBlockList
		    );
	    }

	    public MediumLevelILBasicBlock[] Dominators
	    {
		    get
		    {
			    return this.GetDominators(false);
		    }
	    }
	    
	    public MediumLevelILBasicBlock[] PostDominators
	    {
		    get
		    {
			    return this.GetDominators(true);
		    }
	    }
	    
	    public MediumLevelILBasicBlock[] GetStrictDominators(bool post)
	    {
		    ulong arrayLength = 0;
			    
		    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockStrictDominators(
			    this.handle , 
			    out arrayLength ,
			    post
		    );
			    
		    return UnsafeUtils.TakeHandleArrayEx<MediumLevelILBasicBlock>(
			    arrayPointer ,
			    arrayLength , 
			    (_native) => MediumLevelILBasicBlock.MustNewFromHandleEx(this.ILFunction , _native),
			    NativeMethods.BNFreeBasicBlockList
		    );
	    }
	    
	    public MediumLevelILBasicBlock[] StrictDominators
	    {
		    get
		    {
			    return this.GetStrictDominators(false);
		    }
	    }
	    
	    public MediumLevelILBasicBlock[] StrictPostDominators
	    {
		    get
		    {
			    return this.GetStrictDominators(true);
		    }
	    }
	    
	    public MediumLevelILBasicBlock? GetImmediateDominator(bool post)
	    {
		    return MediumLevelILBasicBlock.TakeHandleEx(
			    this.ILFunction,
			    NativeMethods.BNGetBasicBlockImmediateDominator(this.handle , post)
			);
	    }
	    
	    public MediumLevelILBasicBlock? ImmediateDominator
	    {
		    get
		    {
			    return this.GetImmediateDominator(false);
		    }
	    }
	    
	    public MediumLevelILBasicBlock? ImmediatePostDominator
	    {
		    get
		    {
			    return this.GetImmediateDominator(true);
		    }
	    }
	    
	    public MediumLevelILBasicBlock[] GetDominatorTreeChildren(bool post)
	    {
			 ulong arrayLength = 0;
		    
		    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockDominatorTreeChildren(
			    this.handle , 
			    out arrayLength ,
			    post
		    );
		    
		    return UnsafeUtils.TakeHandleArrayEx<MediumLevelILBasicBlock>(
			    arrayPointer ,
			    arrayLength , 
			    (_native) => MediumLevelILBasicBlock.MustNewFromHandleEx(this.ILFunction , _native),
			    NativeMethods.BNFreeBasicBlockList
		    );
	    }
	    
	    public MediumLevelILBasicBlock[] DominatorTreeChildren
	    {
		    get
		    {
			    return this.GetDominatorTreeChildren(false);
		    }
	    }
	    
	    public MediumLevelILBasicBlock[] PostDominatorTreeChildren
	    {
		    get
		    {
			    return this.GetDominatorTreeChildren(true);
		    }
	    }
	    
	    public MediumLevelILBasicBlock[] GetDominanceFrontier(bool post)
	    {
		    ulong arrayLength = 0;
		    
		    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockDominanceFrontier(
			    this.handle , 
			    out arrayLength ,
			    post
		    );
		    
		    return UnsafeUtils.TakeHandleArrayEx<MediumLevelILBasicBlock>(
			    arrayPointer ,
			    arrayLength , 
			    (_native) => MediumLevelILBasicBlock.MustNewFromHandleEx(this.ILFunction , _native),
			    NativeMethods.BNFreeBasicBlockList
		    );
	    }
	    
	    public MediumLevelILBasicBlock[] DominanceFrontier
	    {
		    get
		    {
			    return this.GetDominanceFrontier(false);
		    }
	    }
	    
	    public MediumLevelILBasicBlock[] PostDominanceFrontier
	    {
		    get
		    {
			    return this.GetDominanceFrontier(true);
		    }
	    }

	    public void PrepareToCopy()
	    {
		    NativeMethods.BNPrepareToCopyMediumLevelILBasicBlock(
			    this.ILFunction.DangerousGetHandle(),
			    this.DangerousGetHandle()
		    );
	    }
	}
}
