using System;
using System.Collections.Generic;

namespace BinaryNinja
{
	public sealed class LowLevelILBasicBlock : AbstractBasicBlock<LowLevelILBasicBlock>
	{
		internal LowLevelILFunction ILFunction { get; }
		
		internal LowLevelILBasicBlock(
			LowLevelILFunction function,
			IntPtr handle , 
			bool owner
		) : base(handle , owner)
		{
			this.ILFunction = function;
		}
		
		internal static LowLevelILBasicBlock MustNewFromHandleEx(
			LowLevelILFunction function,
			IntPtr handle
		)
		{
			return new LowLevelILBasicBlock(
				function,
				NativeMethods.BNNewBasicBlockReference(handle) ,
				true
			);
		}
	    
		internal static LowLevelILBasicBlock? TakeHandleEx(
			LowLevelILFunction function,
			IntPtr handle
		)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new LowLevelILBasicBlock(
				function,
				handle, 
				true
			);
		}
		
		public LowLevelILInstruction this[LowLevelILInstructionIndex index]
		{
			get
			{
				return this.MustGetInstruction(index);
			}
		}
	    
		public LowLevelILInstruction? GetInstruction(LowLevelILInstructionIndex index)
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
	    
		public LowLevelILInstruction MustGetInstruction(LowLevelILInstructionIndex index)
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
	    
		public IEnumerable<LowLevelILInstruction> Instructions
		{
			get
			{
				for (ulong index = (ulong)this.Start; index < (ulong)this.End; index++)
				{
					yield return this.MustGetInstruction((LowLevelILInstructionIndex)index);
				}
			}
		}
		
		public bool ContainsInstruction(LowLevelILInstructionIndex index)
		{
			return (ulong)index >= (ulong)this.Start && (ulong)index < (ulong)this.End;
		}
		
		public LowLevelILBasicBlock? SourceBlock
		{
			get
			{
				return LowLevelILBasicBlock.TakeHandleEx(
					this.ILFunction ,
					NativeMethods.BNGetBasicBlockSourceBlock(this.handle)
				);
			}
		}
		
	    public LowLevelILBasicBlockEdge[] OutgoingEdges
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockOutgoingEdges(
				    this.handle , 
				    out ulong arrayLength
				);
			
			    return UnsafeUtils.TakeStructArrayEx<BNBasicBlockEdge,LowLevelILBasicBlockEdge>(
				    arrayPointer ,
				    arrayLength ,
				    (_native) => LowLevelILBasicBlockEdge.FromNativeEx(
					    _native ,
					    this ,
					    true
				    ) ,
				    NativeMethods.BNFreeBasicBlockEdgeList
			    );
		    }
	    }
	    
	    public LowLevelILBasicBlockEdge[] IncomingEdges
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockIncomingEdges(
				    this.handle , 
				    out ulong arrayLength
				);
			
			    return UnsafeUtils.TakeStructArrayEx<BNBasicBlockEdge,LowLevelILBasicBlockEdge>(
				    arrayPointer ,
				    arrayLength ,
				    (_native) => LowLevelILBasicBlockEdge.FromNativeEx(
					    _native ,
					    this ,
					    false
				    ) ,
				    NativeMethods.BNFreeBasicBlockEdgeList
			    );
		    }
	    }
	    
	    public LowLevelILBasicBlock[] GetDominators(bool post)
	    {
		    ulong arrayLength = 0;
			    
		    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockDominators(
			    this.handle , 
			    out arrayLength ,
			    post
		    );
			    
		    return UnsafeUtils.TakeHandleArrayEx<LowLevelILBasicBlock>(
			    arrayPointer ,
			    arrayLength , 
			    (_native) => LowLevelILBasicBlock.MustNewFromHandleEx(this.ILFunction , _native),
			    NativeMethods.BNFreeBasicBlockList
		    );
	    }

	    public LowLevelILBasicBlock[] Dominators
	    {
		    get
		    {
			    return this.GetDominators(false);
		    }
	    }
	    
	    public LowLevelILBasicBlock[] PostDominators
	    {
		    get
		    {
			    return this.GetDominators(true);
		    }
	    }
	    
	    public LowLevelILBasicBlock[] GetStrictDominators(bool post)
	    {
		    ulong arrayLength = 0;
			    
		    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockStrictDominators(
			    this.handle , 
			    out arrayLength ,
			    post
		    );
			    
		    return UnsafeUtils.TakeHandleArrayEx<LowLevelILBasicBlock>(
			    arrayPointer ,
			    arrayLength , 
			    (_native) => LowLevelILBasicBlock.MustNewFromHandleEx(this.ILFunction , _native),
			    NativeMethods.BNFreeBasicBlockList
		    );
	    }
	    
	    public LowLevelILBasicBlock[] StrictDominators
	    {
		    get
		    {
			    return this.GetStrictDominators(false);
		    }
	    }
	    
	    public LowLevelILBasicBlock[] StrictPostDominators
	    {
		    get
		    {
			    return this.GetStrictDominators(true);
		    }
	    }
	    
	    public LowLevelILBasicBlock? GetImmediateDominator(bool post)
	    {
		    return LowLevelILBasicBlock.TakeHandleEx(
			    this.ILFunction,
			    NativeMethods.BNGetBasicBlockImmediateDominator(this.handle , post)
			);
	    }
	    
	    public LowLevelILBasicBlock? ImmediateDominator
	    {
		    get
		    {
			    return this.GetImmediateDominator(false);
		    }
	    }
	    
	    public LowLevelILBasicBlock? ImmediatePostDominator
	    {
		    get
		    {
			    return this.GetImmediateDominator(true);
		    }
	    }
	    
	    public LowLevelILBasicBlock[] GetDominatorTreeChildren(bool post)
	    {
			 ulong arrayLength = 0;
		    
		    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockDominatorTreeChildren(
			    this.handle , 
			    out arrayLength ,
			    post
		    );
		    
		    return UnsafeUtils.TakeHandleArrayEx<LowLevelILBasicBlock>(
			    arrayPointer ,
			    arrayLength , 
			    (_native) => LowLevelILBasicBlock.MustNewFromHandleEx(this.ILFunction , _native),
			    NativeMethods.BNFreeBasicBlockList
		    );
	    }
	    
	    public LowLevelILBasicBlock[] DominatorTreeChildren
	    {
		    get
		    {
			    return this.GetDominatorTreeChildren(false);
		    }
	    }
	    
	    public LowLevelILBasicBlock[] PostDominatorTreeChildren
	    {
		    get
		    {
			    return this.GetDominatorTreeChildren(true);
		    }
	    }
	    
	    public LowLevelILBasicBlock[] GetDominanceFrontier(bool post)
	    {
		    ulong arrayLength = 0;
		    
		    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockDominanceFrontier(
			    this.handle , 
			    out arrayLength ,
			    post
		    );
		    
		    return UnsafeUtils.TakeHandleArrayEx<LowLevelILBasicBlock>(
			    arrayPointer ,
			    arrayLength , 
			    (_native) => LowLevelILBasicBlock.MustNewFromHandleEx(this.ILFunction , _native),
			    NativeMethods.BNFreeBasicBlockList
		    );
	    }
	    
	    public LowLevelILBasicBlock[] DominanceFrontier
	    {
		    get
		    {
			    return this.GetDominanceFrontier(false);
		    }
	    }
	    
	    public LowLevelILBasicBlock[] PostDominanceFrontier
	    {
		    get
		    {
			    return this.GetDominanceFrontier(true);
		    }
	    }

	    public void PrepareToCopy()
	    {
		    NativeMethods.BNPrepareToCopyLowLevelILBasicBlock(
			    this.ILFunction.DangerousGetHandle(),
			    this.DangerousGetHandle()
		    );
	    }
	    
	}
}
