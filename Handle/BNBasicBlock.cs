using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public abstract class AbstractBasicBlock<T_SELF> : AbstractSafeHandle<T_SELF>
		where T_SELF : AbstractBasicBlock<T_SELF>
	{
		internal AbstractBasicBlock(IntPtr handle , bool owner) 
			: base(handle , owner)
	    {
	        
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeBasicBlock(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }

	    public Function? Function
	    {
		    get
		    {
			    return Function.TakeHandle(
				    NativeMethods.BNGetBasicBlockFunction(this.handle)
			    );
		    }
	    }
	    
	    public FunctionGraphType FunctionGraphType
	    {
		    get
		    {
			    return NativeMethods.BNGetBasicBlockFunctionGraphType(this.handle);
		    }
	    }

	    public LowLevelILFunction? LowLevelIlFunction
	    {
		    get
		    {
			    return LowLevelILFunction.TakeHandle(
				    NativeMethods.BNGetBasicBlockLowLevelILFunction(this.handle)
				);
		    }
	    }
	    
	    public MediumLevelILFunction? MediumLevelILFunction
	    {
		    get
		    {
			    return MediumLevelILFunction.TakeHandle(
				    NativeMethods.BNGetBasicBlockMediumLevelILFunction(this.handle)
			    );
		    }
	    }
	    
	    public HighLevelILFunction? HighLevelILFunction
	    {
		    get
		    {
			    return HighLevelILFunction.TakeHandle(
				    NativeMethods.BNGetBasicBlockHighLevelILFunction(this.handle)
			    );
		    }
	    }
	    
	    public BinaryView? View
	    {
		    get
		    {
			    return this.Function?.View;
		    }
	    }

	    public Architecture Architecture
	    {
		    get
		    {
			    IntPtr raw = NativeMethods.BNGetBasicBlockArchitecture(this.handle);

			    if (IntPtr.Zero == raw)
			    {
				    throw new NoNullAllowedException("basic block must have an architecture.");
			    }
			    
			    return new Architecture(raw);
		    }
	    }
		
	    /// <summary>
	    /// address
	    /// </summary>
	    public ulong Start
	    {
		    get
		    {
			    return NativeMethods.BNGetBasicBlockStart(this.handle);
		    }
	    }
	    
	    /// <summary>
	    /// address
	    /// </summary>
	    public ulong End
	    {
		    get
		    {
			    return NativeMethods.BNGetBasicBlockEnd(this.handle);
		    }

		    set
		    {
			    NativeMethods.BNSetBasicBlockEnd(this.handle, value);
		    }
	    }
	    
	    public ulong InstructionCount
	    {
		    get
		    {
			    return (ulong)this.End - (ulong)this.Start;
		    }
	    }
	    
	    public ulong Length
	    {
		    get
		    {
			    return NativeMethods.BNGetBasicBlockLength(this.handle);
		    }
	    }
	    
	    public ulong Index
	    {
		    get
		    {
			    return NativeMethods.BNGetBasicBlockIndex(this.handle);
		    }
	    }
	    
	    public bool HasUndeterminedOutgoingEdges
	    {
		    get
		    {
			    return NativeMethods.BNBasicBlockHasUndeterminedOutgoingEdges(this.handle);
		    }

		    set
		    {
			    NativeMethods.BNBasicBlockSetUndeterminedOutgoingEdges(this.handle, value);
		    }
	    }

	    public bool CanExit
	    {
		    get
		    {
			    return NativeMethods.BNBasicBlockCanExit(this.handle);
		    }
		    
		    set
		    {
			    NativeMethods.BNBasicBlockSetCanExit(this.handle, value);
		    }
	    }

	    public bool HasInvalidInstructions
	    {
		    get
		    {
			    return NativeMethods.BNBasicBlockHasInvalidInstructions(this.handle);
		    }

		    set
		    {
			    NativeMethods.BNBasicBlockSetHasInvalidInstructions(this.handle, value);
		    }
	    }

	    public PendingBasicBlockEdge[] PendingOutgoingEdges
	    {
		    get
		    {
			    ulong arrayLength = 0;
			 
			    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockPendingOutgoingEdges(this.handle , out arrayLength);

			    return UnsafeUtils.TakeStructArray<BNPendingBasicBlockEdge , PendingBasicBlockEdge>(
				    arrayPointer ,
				    arrayLength ,
				    PendingBasicBlockEdge.FromNative ,
				    NativeMethods.BNFreePendingBasicBlockEdgeList
			    );
		    }
	    }

	    public void AddPendingOutgoingEdge(
		    BranchType kind ,
		    ulong address,
		    Architecture arch ,
		    bool fallThrough
		)
	    {
		    NativeMethods.BNBasicBlockAddPendingOutgoingEdge(
			    this.handle,
			    kind,
			    address,
			    arch.DangerousGetHandle(),
			    fallThrough
		    );
	    }

	    public void ClearPendingOutgoingEdges()
	    {
		    NativeMethods.BNClearBasicBlockPendingOutgoingEdges(this.handle);
	    }

	    public byte[] GetInstructionData(ulong address)
	    {
		    ulong size = 0;
		    
		    IntPtr dataPtr = NativeMethods.BNBasicBlockGetInstructionData(this.handle , address , out size );
		    
		    if (IntPtr.Zero == dataPtr || size == 0)
		    {
			    return Array.Empty<byte>();
		    }
		    
		    byte[] data = new byte[size];
		    
		    Marshal.Copy(dataPtr, data, 0, (int)size);
		
		    return data;
	    }
	    
	    public void AddInstructionData(byte[] data)
	    {
		    NativeMethods.BNBasicBlockAddInstructionData(this.handle , data , (ulong)data.Length);
	    }

	    public bool FallThroughToFunction
	    {
		    get
		    {
			    return NativeMethods.BNBasicBlockIsFallThroughToFunction(this.handle);
		    }

		    set
		    {
			    NativeMethods.BNBasicBlockSetFallThroughToFunction(this.handle, value);
		    }
	    }

	    public void SetAutoHighlight(HighlightColor color)
	    {
		    NativeMethods.BNSetAutoBasicBlockHighlight(this.handle, color.ToNative());
	    }
	    
	    public void SetUserHighlight(HighlightColor color)
	    {
		    NativeMethods.BNSetUserBasicBlockHighlight(this.handle, color.ToNative());
	    }

	    public bool IsIL
	    {
		    get
		    {
			    return NativeMethods.BNIsILBasicBlock(this.handle);
		    }
	    }
	    
	    public bool IsLowLevelIL
	    {
		    get
		    {
			    return NativeMethods.BNIsLowLevelILBasicBlock(this.handle);
		    }
	    }
	    
	    public bool IsMediumLevelIL
	    {
		    get
		    {
			    return NativeMethods.BNIsMediumLevelILBasicBlock(this.handle);
		    }
	    }
	    
	    public bool IsHighLevelIL
	    {
		    get
		    {
			    return NativeMethods.BNIsHighLevelILBasicBlock(this.handle);
		    }
	    }

	    public void MarkRecentlyUsed()
	    {
		    NativeMethods.BNMarkBasicBlockAsRecentlyUsed(this.handle);
	    }
	    
	    public DisassemblyTextLine[] GetDisassemblyTextLines(DisassemblySettings? settings = null)
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockDisassemblyText(
			    this.handle,
			    settings == null ? IntPtr.Zero : settings.DangerousGetHandle(),
			    out ulong arrayLength 
		    );
		    
		    return UnsafeUtils.TakeStructArrayEx<BNDisassemblyTextLine,DisassemblyTextLine>(
			    arrayPointer,
			    arrayLength,
			    DisassemblyTextLine.FromNative,
			    NativeMethods.BNFreeDisassemblyTextLines
		    );
	    }

	    public DisassemblyTextLine[] DisassemblyTextLines
	    {
		    get
		    {
			    return this.GetDisassemblyTextLines();
		    }
	    }
	    
	    public ulong? GetInstructionContainingAddress(ulong address )
	    {
		    bool ok = NativeMethods.BNGetBasicBlockInstructionContainingAddress(
			    this.handle , 
			    address ,
			    out ulong start
			);

		    if (!ok)
		    {
			    return null;
		    }

		    return start;
	    }
	    
	    public HighlightColor Highlight
	    {
		    get
		    {
			    BNHighlightColor raw = NativeMethods.BNGetBasicBlockHighlight(this.handle);

			    return HighlightColor.FromNative(raw);
		    }

		    set
		    {
			    this.SetUserHighlight(value);
		    }
	    }
	    
	    public DisassemblyTextLine[] GetLanguageRepresentationLines(
		    DisassemblySettings? settings = null,
		    string language = "Pseudo C"
	    )
	    {
		    Function? function = this.Function;

		    if (null == function)
		    {
			    return Array.Empty<DisassemblyTextLine>();
		    }
		    
		    LanguageRepresentationFunction? pseudo = function.GetLanguageRepresentation(language);

		    if (null == pseudo)
		    {
			    return Array.Empty<DisassemblyTextLine>();
		    }
		    
		    IntPtr arrayPointer = NativeMethods.BNGetLanguageRepresentationFunctionBlockLines(
			    pseudo.DangerousGetHandle() ,
			    this.DangerousGetHandle() ,
			    null == settings ? IntPtr.Zero :  settings.DangerousGetHandle() ,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArrayEx<BNDisassemblyTextLine , DisassemblyTextLine>(
			    arrayPointer ,
			    arrayLength ,
			    DisassemblyTextLine.FromNative ,
			    NativeMethods.BNFreeDisassemblyTextLines
		    );
	    }
	    
	    public DisassemblyTextLine[] PseudoCLines
	    {
		    get
		    {
			    return this.GetLanguageRepresentationLines();
		    }
	    }
	    
	    public string PseudoCText
	    {
		    get
		    {
			    StringBuilder builder = new  StringBuilder();

			    foreach (DisassemblyTextLine line in this.PseudoCLines)
			    {
				    builder.AppendLine(line.ToString());
			    }
			    
			    return builder.ToString();
		    }
	    }
	}
	
	public class BasicBlock : AbstractBasicBlock<BasicBlock>
	{
		internal BasicBlock(IntPtr handle , bool owner) 
			: base(handle , owner)
	    {
	        
	    }
	   
	    public BasicBlock? Source
	    {
		    get
		    {
			    return BasicBlock.TakeHandle(
				    NativeMethods.BNGetBasicBlockSource(this.handle)
			    );
		    }
	    }
	    
		internal static BasicBlock? NewFromHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new BasicBlock(
				NativeMethods.BNNewBasicBlockReference(handle) ,
				true
			);
		}
	    
		internal static BasicBlock MustNewFromHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new BasicBlock(
				NativeMethods.BNNewBasicBlockReference(handle) ,
				true
			);
		}
	    
		internal static BasicBlock? TakeHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new BasicBlock(handle, true);
		}
	    
		internal static BasicBlock MustTakeHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new BasicBlock(handle, true);
		}
	    
		internal static BasicBlock? BorrowHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new BasicBlock(handle, false);
		}
	    
		internal static BasicBlock MustBorrowHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new BasicBlock(handle, false);
		}
	
	    public virtual BasicBlockEdge[] OutgoingEdges
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockOutgoingEdges(
				    this.handle , 
				    out ulong arrayLength
				);

			    return UnsafeUtils.TakeStructArrayEx<BNBasicBlockEdge,BasicBlockEdge>(
				    arrayPointer ,
				    arrayLength ,
				    (_native) => BasicBlockEdge.FromNativeEx(
					    _native ,
					    this ,
					    true
				    ) ,
				    NativeMethods.BNFreeBasicBlockEdgeList
			    );
		    }
	    }
	    
	    public BasicBlockEdge[] IncomingEdges
	    {
		    get
		    {
			    ulong arrayLength = 0;
			    
			    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockIncomingEdges(this.handle , out arrayLength);
			
			    return UnsafeUtils.TakeStructArrayEx<BNBasicBlockEdge,BasicBlockEdge>(
				    arrayPointer ,
				    arrayLength ,
				    (_native) => BasicBlockEdge.FromNativeEx(
					    _native ,
					    this ,
					    false
				    ) ,
				    NativeMethods.BNFreeBasicBlockEdgeList
			    );
		    }
	    }
	    
	    public BasicBlock[] GetDominators(bool post)
	    {
		    ulong arrayLength = 0;
			    
		    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockDominators(
			    this.handle , 
			    out arrayLength ,
			    post
		    );
			    
		    return UnsafeUtils.TakeHandleArrayEx<BasicBlock>(
			    arrayPointer ,
			    arrayLength , 
			    BasicBlock.MustNewFromHandle,
			    NativeMethods.BNFreeBasicBlockList
		    );
	    }

	    public BasicBlock[] Dominators
	    {
		    get
		    {
			    return this.GetDominators(false);
		    }
	    }
	    
	    public BasicBlock[] PostDominators
	    {
		    get
		    {
			    return this.GetDominators(true);
		    }
	    }
	    
	    public BasicBlock[] GetStrictDominators(bool post)
	    {
		    ulong arrayLength = 0;
			    
		    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockStrictDominators(
			    this.handle , 
			    out arrayLength ,
			    post
		    );
			    
		    return UnsafeUtils.TakeHandleArrayEx<BasicBlock>(
			    arrayPointer ,
			    arrayLength , 
			    BasicBlock.MustNewFromHandle,
			    NativeMethods.BNFreeBasicBlockList
		    );
	    }
	    
	    public BasicBlock[] StrictDominators
	    {
		    get
		    {
			    return this.GetStrictDominators(false);
		    }
	    }
	    
	    public BasicBlock[] StrictPostDominators
	    {
		    get
		    {
			    return this.GetStrictDominators(true);
		    }
	    }
	    
	    public BasicBlock? GetImmediateDominator(bool post)
	    {
		    return BasicBlock.TakeHandle(
			    NativeMethods.BNGetBasicBlockImmediateDominator(this.handle , post)
			);
	    }
	    
	    public BasicBlock? ImmediateDominator
	    {
		    get
		    {
			    return this.GetImmediateDominator(false);
		    }
	    }
	    
	    public BasicBlock? ImmediatePostDominator
	    {
		    get
		    {
			    return this.GetImmediateDominator(true);
		    }
	    }
	    
	    public BasicBlock[] GetDominatorTreeChildren(bool post)
	    {
			 ulong arrayLength = 0;
		    
		    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockDominatorTreeChildren(
			    this.handle , 
			    out arrayLength ,
			    post
		    );
		    
		    return UnsafeUtils.TakeHandleArrayEx<BasicBlock>(
			    arrayPointer ,
			    arrayLength , 
			    BasicBlock.MustNewFromHandle,
			    NativeMethods.BNFreeBasicBlockList
		    );
	    }
	    
	    public BasicBlock[] DominatorTreeChildren
	    {
		    get
		    {
			    return this.GetDominatorTreeChildren(false);
		    }
	    }
	    
	    public BasicBlock[] PostDominatorTreeChildren
	    {
		    get
		    {
			    return this.GetDominatorTreeChildren(true);
		    }
	    }
	    
	    
	    public BasicBlock[] GetDominanceFrontier(bool post)
	    {
		    ulong arrayLength = 0;
		    
		    IntPtr arrayPointer = NativeMethods.BNGetBasicBlockDominanceFrontier(
			    this.handle , 
			    out arrayLength ,
			    post
		    );
		    
		    return UnsafeUtils.TakeHandleArrayEx<BasicBlock>(
			    arrayPointer ,
			    arrayLength , 
			    BasicBlock.MustNewFromHandle,
			    NativeMethods.BNFreeBasicBlockList
		    );
	    }
	    
	    public BasicBlock[] DominanceFrontier
	    {
		    get
		    {
			    return this.GetDominanceFrontier(false);
		    }
	    }
	    
	    public BasicBlock[] PostDominanceFrontier
	    {
		    get
		    {
			    return this.GetDominanceFrontier(true);
		    }
	    }

	    public IEnumerable<InstructionTextLine> InstructionTextLines
	    {
		    get
		    {
			    return this.GetInstructionTextLines();
		    }
	    }

	    public IEnumerable<InstructionTextLine> GetInstructionTextLines()
	    {
		    if (null == this.View)
		    {
			    throw new Exception("View is null");
		    }
		   
		    ulong address = this.Start;

		    while (address < this.End)
		    {
			    ulong length = this.End - address;
			    
			    length = Math.Min(this.Architecture.MaxInstructionLength, this.Length - address);

			    byte[] data = this.View.ReadData(address , length);

			    InstructionTextToken[] tokens = this.Architecture.GetInstructionText(
				    data , 
				    address ,
				    ref length
				);

			    if (0 == length)
			    {
				    break;
			    }

			    yield return new InstructionTextLine(tokens);
			    address += length;
		    }
		    
	    }
	}
}