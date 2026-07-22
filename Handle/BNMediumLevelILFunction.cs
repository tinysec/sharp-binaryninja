using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;


namespace BinaryNinja
{
	public sealed partial class MediumLevelILFunction : AbstractSafeHandle<MediumLevelILFunction>
	{
		private readonly bool isSSAForm;

		/// <summary>
		/// Gets whether this function is in either mapped or unmapped SSA form.
		/// </summary>
		/// <remarks>
		/// Form-aware navigation sources propagate this value when creating the wrapper. Keeping
		/// it as captured state avoids querying basic blocks while IL expression indices are in
		/// use, because graph discovery can finalize lazy analysis state and shift those indices.
		/// </remarks>
		public bool IsSSAForm
		{
			get
			{
				return this.isSSAForm;
			}
		}
		
	    internal MediumLevelILFunction(
		    IntPtr handle , 
		    bool owner ,
		    bool ssa = false
		) : base(handle , owner)
	    {
	        this.isSSAForm = ssa;
	    }

	    private static FunctionGraphType GetILForm(IntPtr functionHandle)
	    {
		    IntPtr blocks = NativeMethods.BNGetMediumLevelILBasicBlockList(
			    functionHandle,
			    out ulong count);
		    return ILFunctionNavigation.TakeFunctionGraphType(blocks, count);
	    }

	    internal static MediumLevelILFunction? NewFromHandle(
		    IntPtr handle ,
		    bool ssa = false
		)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new MediumLevelILFunction(
			    NativeMethods.BNNewMediumLevelILFunctionReference(handle) ,
			    true,
			    ssa
		    );
	    }
	    
	    internal static MediumLevelILFunction MustNewFromHandle(
		    IntPtr handle,
		    bool ssa = false
		)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new MediumLevelILFunction(
			    NativeMethods.BNNewMediumLevelILFunctionReference(handle) ,
			    true,
			    ssa
		    );
	    }
	    
	    internal static MediumLevelILFunction? TakeHandle(
		    IntPtr handle,
		    bool ssa = false
		)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new MediumLevelILFunction(handle, true , ssa);
	    }
	    
	    internal static MediumLevelILFunction MustTakeHandle(
		    IntPtr handle,
		    bool ssa = false
		)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new MediumLevelILFunction(handle, true , ssa);
	    }
	    
	    internal static MediumLevelILFunction? BorrowHandle(
		    IntPtr handle,
		    bool ssa = false
		)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new MediumLevelILFunction(handle, false , ssa);
	    }
	    
	    internal static MediumLevelILFunction MustBorrowHandle(
		    IntPtr handle,
		    bool ssa = false
		)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new MediumLevelILFunction(handle, false , ssa);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeMediumLevelILFunction(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }

	    public Function OwnerFunction
	    {
		    get
		    {
			    return Function.MustTakeHandle(
				    NativeMethods.BNGetMediumLevelILOwnerFunction(this.handle) 
			    );
		    }
	    }

	    public Architecture Architecture
	    {
		    get
		    {
			    return this.OwnerFunction.Architecture;
		    }
	    }

	    /// <summary>
	    /// BinaryView that contains the owning function. Matches the LLIL/HLIL <c>View</c> accessors.
	    /// </summary>
	    public BinaryView View
	    {
		    get
		    {
			    return this.OwnerFunction.View;
		    }
	    }

	    public ulong ExpressionCount
	    {
		    get
		    {
			    return NativeMethods.BNGetMediumLevelILExprCount(this.handle);
		    }
	    }

	    public ulong InstructionCount
	    {
		    get
		    {
			    return NativeMethods.BNGetMediumLevelILInstructionCount(this.handle);
		    }
	    }

	    public MediumLevelILInstruction? GetExpression(
		    MediumLevelILExpressionIndex expression
	    )
	    {
		    if ( (ulong)expression >= this.ExpressionCount)
		    {
			    return null;
		    }
		    
		    return MediumLevelILInstruction.FromExpressionIndex(
			    this,
			    expression
		    );
	    }
	    
	    public MediumLevelILInstruction MustGetExpression(
		    MediumLevelILExpressionIndex expression
		)
	    {
		    if ( (ulong)expression >= this.ExpressionCount)
		    {
			    throw new IndexOutOfRangeException(nameof(expression));
		    }
		    
		    return MediumLevelILInstruction.FromExpressionIndex(
			    this,
			    expression
		    );
	    }
	    
	    
	    public MediumLevelILInstruction this[MediumLevelILInstructionIndex index]
	    {
		    get
		    {
			    return this.MustGetInstruction(index);
		    }
	    }
	    
	    public IEnumerable<MediumLevelILInstruction> Instructions
	    {
		    get
		    {
			    for (ulong index = 0; index < this.InstructionCount; index++)
			    {
				    yield return this.MustGetInstruction( (MediumLevelILInstructionIndex)index);
			    }
		    }
	    }
	 
	    public MediumLevelILInstruction? GetInstruction(MediumLevelILInstructionIndex instruction)
	    {
		    if ( (ulong)instruction >= this.InstructionCount)
		    {
			    return null;
		    }
	
		    return this.MustGetInstruction(instruction);
	    }
	    
	    public MediumLevelILInstruction MustGetInstruction(MediumLevelILInstructionIndex instruction)
	    {
		    if ( (ulong)instruction >= this.InstructionCount)
		    {
			    throw new IndexOutOfRangeException(nameof(instruction));
		    }
		    
		    MediumLevelILExpressionIndex expressionIndex = NativeMethods.BNGetMediumLevelILIndexForInstruction(
			    this.handle,
			    instruction
		    );
		    
		    return this.MustGetExpression(expressionIndex);
	    }
	    
	    
	    public MediumLevelILInstruction[] MustGetExpressions(MediumLevelILExpressionIndex[] indexes)
	    {
		    List<MediumLevelILInstruction> instructions = new List<MediumLevelILInstruction>();

		    foreach (MediumLevelILExpressionIndex index in indexes)
		    {
			    instructions.Add( this.MustGetExpression(index) );
		    }

		    return instructions.ToArray();
	    }

	    public MediumLevelILInstruction[] MustGetInstructions(MediumLevelILInstructionIndex[] indexes)
	    {
		    List<MediumLevelILInstruction> instructions = new List<MediumLevelILInstruction>();

		    foreach (MediumLevelILInstructionIndex index in indexes)
		    {
			    instructions.Add( this.MustGetInstruction(index) );
		    }

		    return instructions.ToArray();
	    }

	    public ulong CurrentAddress
	    {
		    get
		    {
			    return NativeMethods.BNMediumLevelILGetCurrentAddress(this.handle);
		    }

		    set
		    {
			    NativeMethods.BNMediumLevelILSetCurrentAddress(
				    this.handle, 
				    this.OwnerFunction.Architecture.DangerousGetHandle(),
				    value
				);
		    }
	    }

	    public MediumLevelILInstruction? CurrentInstruction
	    {
		    get
		    {
			    return this.GetInstructionStart(this.CurrentAddress);
		    }
	    }

	    public void SetCurrentAddress(ulong address , Architecture? arch = null)
	    {
		    NativeMethods.BNMediumLevelILSetCurrentAddress(
			    this.handle, 
			    null == arch ? this.OwnerFunction.Architecture.DangerousGetHandle() : arch.DangerousGetHandle(),
			    address
		    );
	    }
	    
	    public MediumLevelILBasicBlock[] BasicBlocks
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetMediumLevelILBasicBlockList(
				    this.handle ,
				    out ulong arrayLength
			    );
			    
			    return UnsafeUtils.TakeHandleArrayEx<MediumLevelILBasicBlock>(
				    arrayPointer,
				    arrayLength,
				    (native) => { return MediumLevelILBasicBlock.MustNewFromHandleEx(this , native);},
				    NativeMethods.BNFreeBasicBlockList
			    );
		    }
	    }

	    /// <summary>
	    /// Gets the native IL form represented by this function.
	    /// </summary>
	    public FunctionGraphType ILForm
	    {
		    get
		    {
			    return GetILForm(this.handle);
		    }
	    }

	    /// <summary>
	    /// Gets the native graph type. This alias keeps the MLIL API consistent with the
	    /// existing LLIL and HLIL navigation properties.
	    /// </summary>
	    public FunctionGraphType FunctionGraphType
	    {
		    get
		    {
			    return this.ILForm;
		    }
	    }

	    public MediumLevelILBasicBlock? GetBasicBlockForInstruction(MediumLevelILInstructionIndex index)
	    {
		    return MediumLevelILBasicBlock.TakeHandleEx(
			    this,
			    NativeMethods.BNGetMediumLevelILBasicBlockForInstruction(
				    this.handle ,
				    index)
		    );
	    }
	    
	    public MediumLevelILFunction SSAForm
	    {
		    get
		    {
			    if (this.IsSSAForm)
			    {
				    return this;
			    }
			    else
			    {
				    return MediumLevelILFunction.MustTakeHandle(
					    NativeMethods.BNGetMediumLevelILSSAForm(this.handle),
					    true
				    );
			    }
		    }
	    }
	    
	    public MediumLevelILFunction NonSSAForm
	    {
		    get
		    {
			    if (this.IsSSAForm)
			    {
				    return MediumLevelILFunction.MustTakeHandle(
					    NativeMethods.BNGetMediumLevelILNonSSAForm(this.handle),
					    false
				    );
			    }
			    else
			    {
				    return this;
			    }
		    }
	    }
	    
	    public LowLevelILFunction? LowLevelIL
	    {
		    get
		    {
			    return LowLevelILFunction.TakeHandle(
				    NativeMethods.BNGetLowLevelILForMediumLevelIL(this.handle),
				    this.IsSSAForm
			    );
		    }
	    }
	    
	    public HighLevelILFunction? HighLevelIL
	    {
		    get
		    {
			    return HighLevelILFunction.TakeHandle(
				    NativeMethods.BNGetHighLevelILForMediumLevelIL(this.handle),
				    this.IsSSAForm
			    );
		    }
	    }
	
	    public MediumLevelILInstruction? GetInstructionStart( ulong address ,  Architecture? arch = null )
	    {
		    if (null == arch)
		    {
			    arch = this.Architecture;
		    }
		    
		    return this.GetInstruction(
				    NativeMethods.BNMediumLevelILGetInstructionStart(
				    this.handle,
				    null == arch ? this.OwnerFunction.Architecture.DangerousGetHandle() : arch.DangerousGetHandle(),
				    address
				)
		    );
	    }
	    
	    public void MarkLabel(MediumLevelILLabel label)
	    {
		    NativeMethods.BNMediumLevelILMarkLabel(this.handle, label.ToNative());
	    }
	    
	    public MediumLevelILExpressionIndex AddLabelMap(IDictionary<ulong, MediumLevelILLabel> labelMap)
	    {
		    ulong[] values = new ulong[labelMap.Count];
		    IntPtr[] labelPointers = new IntPtr[labelMap.Count];
		    int index = 0;

		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    foreach (KeyValuePair<ulong, MediumLevelILLabel> entry in labelMap)
			    {
				    values[index] = entry.Key;
				    labelPointers[index] = allocator.AllocStruct(entry.Value.ToNative());
				    index++;
			    }

			    return NativeMethods.BNMediumLevelILAddLabelMap(
				    this.handle,
				    values,
				    labelPointers,
				    (ulong)labelMap.Count);
		    }
	    }

	    public MediumLevelILExpressionIndex AddOperandList(ulong[] operands)
	    {
		    return NativeMethods.BNMediumLevelILAddOperandList(
			    this.handle ,
			    operands ,
			    (ulong)operands.Length
		    );
	    }
	    
	    public MediumLevelILExpressionIndex AddOperandList(MediumLevelILExpressionIndex[] operands)
	    {
		    List<ulong> targets = new List<ulong>();

		    foreach (MediumLevelILExpressionIndex operand in operands)
		    {
			    targets.Add( (ulong)operand);
		    }
		    
		    return NativeMethods.BNMediumLevelILAddOperandList(
			    this.handle ,
			    targets.ToArray() ,
			    (ulong)operands.Length
		    );
	    }
	    
	    public MediumLevelILExpressionIndex AddVariableList(Variable[] variables)
	    {
		    List<ulong> operands = new List<ulong>();

		    foreach (Variable variable in variables)
		    {
			    operands.Add(variable.Identifier);
		    }
		    
		    return this.AddOperandList(operands.ToArray());
	    }
	    
	    public MediumLevelILPossibleValueSetCacheIndex CachePossibleValueSet(PossibleValueSet pvs)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    return NativeMethods.BNCacheMediumLevelILPossibleValueSet(
				    this.handle, 
				    pvs.ToNativeEx(allocator)
			    );
		    }
	    }

	    public void FinalizeILFunction()
	    {
		    NativeMethods.BNFinalizeMediumLevelILFunction(this.handle);
	    }

	    public void GenerateSSAForm(
		    bool analyzeConditionals,
		    bool handleAliases ,
		    Variable[] knownNotAliases,
		    Variable[] knownAliases
		)
	    {
		    NativeMethods.BNGenerateMediumLevelILSSAForm(
			    this.handle,
			    analyzeConditionals,
			    handleAliases,
			    UnsafeUtils.ConvertToNativeArray<BNVariable,Variable>(
				    knownNotAliases
				    ),
			    (ulong)knownNotAliases.Length,
			    UnsafeUtils.ConvertToNativeArray<BNVariable,Variable>(
				    knownAliases
			    ),
			    (ulong)knownAliases.Length
			);
	    }

	    public void PrepareToCopyFunction(MediumLevelILFunction source)
	    {
		    if (null == source)
		    {
			    throw new ArgumentNullException(nameof(source));
		    }

		    this.BeginTranslation(source);
		    NativeMethods.BNPrepareToCopyMediumLevelILFunction(this.handle , source.DangerousGetHandle());
	    }
	    
	    public void PrepareToCopyBasicBlock(MediumLevelILBasicBlock block)
	    {
		    NativeMethods.BNPrepareToCopyMediumLevelILBasicBlock(this.handle , block.DangerousGetHandle());
	    }
	    
	    public MediumLevelILLabel? GetLabelForSourceInstruction(MediumLevelILInstructionIndex index)
	    {
		    return MediumLevelILLabel.FromNativePointer(
			    NativeMethods.BNGetLabelForMediumLevelILSourceInstruction(this.handle , index)
		    );
	    }
	    
	    public MediumLevelILInstructionIndex GetSSAInstructionIndex(MediumLevelILInstructionIndex instr)
	    {
		    return NativeMethods.BNGetMediumLevelILSSAInstructionIndex(this.handle , instr);
	    }

	    public MediumLevelILInstructionIndex GetNonSSAInstructionIndex(MediumLevelILInstructionIndex instr)
	    {
		    return NativeMethods.BNGetMediumLevelILNonSSAInstructionIndex(this.handle , instr);
	    }

	    public MediumLevelILInstruction? GetSSAVariableDefinition(Variable variable , ulong version)
	    {
		    return this.GetExpression(
			    NativeMethods.BNGetMediumLevelILSSAVarDefinition(
				    this.handle ,
				    variable.ToNative() ,
				    version
			    )
		    );
	    }
	    
	    public MediumLevelILInstruction? GetSSAMemoryDefinition(ulong version)
	    {
		    return this.GetInstruction(
			    NativeMethods.BNGetMediumLevelILSSAMemoryDefinition(
			    this.handle , 
			    version
		    ));
	    }
	    
	    public MediumLevelILInstruction[] GetSSAVariableUses(Variable variable , ulong version)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetMediumLevelILSSAVarUses(
			    this.handle , 
			    variable.ToNative() ,
			    version,
			    out ulong arrayLength
			);
		    
		    MediumLevelILInstructionIndex[] indexs = UnsafeUtils.TakeNumberArray<MediumLevelILInstructionIndex>(
			    arrayPointer ,
			    arrayLength ,
			    NativeMethods.BNFreeILInstructionList
		    );

		    return this.MustGetInstructions(indexs);
	    }
	    
	    public MediumLevelILInstruction[] GetSSAMemoryUses( ulong version)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetMediumLevelILSSAMemoryUses(
			    this.handle , 
			    version,
			    out ulong arrayLength
		    );

		    MediumLevelILInstructionIndex[] indexs = UnsafeUtils.TakeNumberArray<MediumLevelILInstructionIndex>(
			    arrayPointer ,
			    arrayLength ,
			    NativeMethods.BNFreeILInstructionList
		    );
		    
		    return this.MustGetInstructions(indexs);
	    }
	    
	    public bool IsSSAVariableLive(Variable variable , ulong version)
	    {
		    return NativeMethods.BNIsMediumLevelILSSAVarLive(
			    this.handle , 
			    variable.ToNative() ,
			    version
			);
	    }
	    
	    public bool IsSSAVariableLiveAt(Variable variable , ulong version , MediumLevelILInstructionIndex instr)
	    {
		    return NativeMethods.BNIsMediumLevelILSSAVarLiveAt(
			    this.handle , 
			    variable.ToNative() ,
			    version,
			    instr
		    );
	    }
	    
	    public bool IsVariableLiveAt(Variable variable , MediumLevelILInstructionIndex instr)
	    {
		    return NativeMethods.BNIsMediumLevelILVarLiveAt(
			    this.handle , 
			    variable.ToNative() ,
			    instr
		    );
	    }

	    public MediumLevelILInstruction[] GetVariableDefinitions(Variable variable )
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetMediumLevelILVariableDefinitions(
			    this.handle , 
			    variable.ToNative(),
			    out ulong arrayLength
		    );

		    MediumLevelILInstructionIndex[] indexs = UnsafeUtils.TakeNumberArray<MediumLevelILInstructionIndex>(
			    arrayPointer ,
			    arrayLength ,
			    NativeMethods.BNFreeILInstructionList
		    );
		    
		    return this.MustGetInstructions(indexs);
	    }
	    
	    public MediumLevelILInstruction[] GetVariableUses(Variable variable )
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetMediumLevelILVariableUses(
			    this.handle , 
			    variable.ToNative(),
			    out ulong arrayLength
		    );

		    MediumLevelILInstructionIndex[] indexs = UnsafeUtils.TakeNumberArray<MediumLevelILInstructionIndex>(
			    arrayPointer ,
			    arrayLength ,
			    NativeMethods.BNFreeILInstructionList
		    );
		    
		    return this.MustGetInstructions(indexs);
	    }
	    
	    public MediumLevelILInstruction[] GetLiveInstructionsForVariable(
		    Variable variable , 
		    bool includeLastUse = true
		)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetMediumLevelILLiveInstructionsForVariable(
			    this.handle , 
			    variable.ToNative(),
			    includeLastUse,
			    out ulong arrayLength
		    );

		    MediumLevelILInstructionIndex[] indexs = UnsafeUtils.TakeNumberArray<MediumLevelILInstructionIndex>(
			    arrayPointer ,
			    arrayLength ,
			    NativeMethods.BNFreeILInstructionList
		    );
		    
		    return this.MustGetInstructions(indexs);
	    }
	    
	    public RegisterValue GetSSAVariableValue(Variable variable ,  ulong version)
	    {
		    return RegisterValue.FromNative(
			    NativeMethods.BNGetMediumLevelILSSAVarValue(
				    this.handle ,
				    variable.ToNative() ,
				    version
			    )
		    );
	    }
	    
	    public MediumLevelILInstructionIndex? GetInstructionIndexForExpressionIndex(
		    MediumLevelILExpressionIndex expressionIndex
	    )
	    {
		    MediumLevelILInstructionIndex index =  NativeMethods.BNGetMediumLevelILInstructionForExpr(
			    this.handle,
			    expressionIndex
		    );

		    if ((ulong)index >= this.InstructionCount)
		    {
			    return null;
		    }
		    
		    return index;
	    }
	    
	    public MediumLevelILExpressionIndex? GetExpressionIndexForInstructionIndex(
		    MediumLevelILInstructionIndex instruction
	    )
	    {
		    MediumLevelILExpressionIndex index = NativeMethods.BNGetMediumLevelILIndexForInstruction(
			    this.handle,
			    instruction
			);
		    
		    if ((ulong)index >= this.ExpressionCount)
		    {
			    return null;
		    }
		    
		    return index;
	    }
	    
	    public LowLevelILInstruction? GetLowLevelILExpression(
		    MediumLevelILExpressionIndex mediumExpr
	    )
	    {
		    LowLevelILFunction? llil = this.LowLevelIL;

		    if (null == llil)
		    {
			    return null;
		    }

		    LowLevelILFunction llilSSA = llil.SSAForm;
		    
		    LowLevelILExpressionIndex lowExpr = NativeMethods.BNGetLowLevelILExprIndex(
			    this.handle, 
			    mediumExpr
		    );

		    if ((ulong)lowExpr >= llilSSA.ExpressionCount)
		    {
			    return null;
		    }

		    return llilSSA.GetExpression(lowExpr);
	    }
	    
	    public LowLevelILInstruction? GetLowLevelILInstruction(
		    MediumLevelILInstructionIndex mediumInstr
	    )
	    {
		    LowLevelILFunction? llil = this.LowLevelIL;

		    if (null == llil)
		    {
			    return null;
		    }

		    LowLevelILFunction llilSSA = llil.SSAForm;
		    
		    LowLevelILInstructionIndex lowInstr = NativeMethods.BNGetLowLevelILInstructionIndex(
			    this.handle, 
			    mediumInstr
			);

		    if ((ulong)lowInstr >= llilSSA.InstructionCount)
		    {
			    return null;
		    }

		    return llilSSA.GetInstruction(lowInstr);
	    }
	    
	    public LowLevelILInstruction[] GetLowLevelILExpressions(
		    MediumLevelILExpressionIndex mediumExpr
	    )
	    {
		    LowLevelILFunction? llil = this.LowLevelIL;

		    if (null == llil)
		    {
			    return Array.Empty<LowLevelILInstruction>();
		    }

		    LowLevelILFunction llilSSA = llil.SSAForm;
		    
		    IntPtr arrayPointer = NativeMethods.BNGetLowLevelILExprIndexes(
			    this.handle, 
			    mediumExpr ,
			    out ulong arrayLength
			);
		    
		    ulong[] lowExprs = UnsafeUtils.TakeNumberArray<ulong>(
			    arrayPointer,
			    arrayLength ,
			    NativeMethods.BNFreeILInstructionList
		    );
		    
		    List<LowLevelILInstruction> expressions = new List<LowLevelILInstruction>();

		    foreach (LowLevelILExpressionIndex lowExpr in lowExprs)
		    {
			    expressions.Add(
				    llilSSA.MustGetExpression(lowExpr)
				);
		    }
		    
		    return expressions.ToArray();
	    }
	    
	    // high
	    public HighLevelILInstruction? GetHighLevelILExpression(
		    MediumLevelILExpressionIndex mediumExpr
	    )
	    {
		    HighLevelILFunction? hlil = this.HighLevelIL;

		    if (null == hlil)
		    {
			    return null;
		    }
		    
		    HighLevelILExpressionIndex highExpr = NativeMethods.BNGetHighLevelILExprIndex(
			    this.handle, 
			    mediumExpr
		    );

		    if ((ulong)highExpr >= hlil.ExpressionCount)
		    {
			    return null;
		    }

		    return hlil.GetExpression(highExpr);
	    }
	    
	    public HighLevelILInstruction[] GetHighLevelILExpressions(
		    MediumLevelILExpressionIndex mediumExpr
	    )
	    {
		    HighLevelILFunction? hlil = this.HighLevelIL;

		    if (null == hlil)
		    {
			    return Array.Empty<HighLevelILInstruction>();
		    }
		    
		    IntPtr arrayPointer = NativeMethods.BNGetHighLevelILExprIndexes(
			    this.handle, 
			    mediumExpr ,
			    out ulong arrayLength
		    );
		    
		    ulong[] highExprs = UnsafeUtils.TakeNumberArray<ulong>(
			    arrayPointer,
			    arrayLength ,
			    NativeMethods.BNFreeILInstructionList
		    );
		    
		    List<HighLevelILInstruction> instructions = new List<HighLevelILInstruction>();

		    foreach (HighLevelILExpressionIndex index in highExprs)
		    {
			    instructions.Add(
				    hlil.MustGetExpression(index)
			    );
		    }
		    
		    return instructions.ToArray();
	    }
	    
	    public HighLevelILInstruction? GetHighLevelILInstruction(
		    MediumLevelILInstructionIndex mediumInstr ,
		    bool asFullAST = false
	    )
	    {
		    HighLevelILFunction? hlil = this.HighLevelIL;

		    if (null == hlil)
		    {
			    return null;
		    }
		    
		    HighLevelILInstructionIndex highInstr = NativeMethods.BNGetHighLevelILInstructionIndex(
			    this.handle, 
			    mediumInstr
		    );

		    if ((ulong)highInstr >= hlil.InstructionCount)
		    {
			    return null;
		    }

		    return hlil.GetInstruction(highInstr , asFullAST);
	    }
	    
	    
	    
	    public MediumLevelILExpressionIndex GetSSAExpressionIndex(MediumLevelILExpressionIndex expression)
	    {
		    return NativeMethods.BNGetMediumLevelILSSAExprIndex(this.handle , expression);
	    }
	    
	    public MediumLevelILExpressionIndex GetNonSSAExpressionIndex(MediumLevelILExpressionIndex expression)
	    {
		    return NativeMethods.BNGetMediumLevelILNonSSAExprIndex(this.handle , expression);
	    }
	    
	    public ulong GetSSAMemoryVersionAtInstruction(MediumLevelILInstructionIndex instruction)
	    {
		    return NativeMethods.BNGetMediumLevelILSSAMemoryVersionAtILInstruction(this.handle , instruction);
	    }
	    
	    public ulong GetSSAMemoryVersionAfterILInstruction(MediumLevelILInstructionIndex instruction)
	    {
		    return NativeMethods.BNGetMediumLevelILSSAMemoryVersionAfterILInstruction(this.handle , instruction);
	    }

	    public MediumLevelILFlowGraph CreateGraph(DisassemblySettings? settings = null)
	    {
		    return MediumLevelILFlowGraph.MustTakeHandleEx(
			    this,
			    NativeMethods.BNCreateMediumLevelILFunctionGraph(
				    this.handle ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public MediumLevelILFlowGraph CreateImmediateGraph(DisassemblySettings? settings = null)
	    {
		    return MediumLevelILFlowGraph.MustTakeHandleEx(
			    this,
			    NativeMethods.BNCreateMediumLevelILImmediateFunctionGraph(
				    this.handle ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }

	    public MediumLevelILVariable[] Variables
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetMediumLevelILVariables(
				    this.handle ,
				    out ulong arrayLength
			    );

			    return UnsafeUtils.TakeStructArray<BNVariable , MediumLevelILVariable>(
					arrayPointer,
					arrayLength,
					(native) => { return new MediumLevelILVariable(this , native);},
					NativeMethods.BNFreeVariableList
			    );
		    }
	    }
	    
	    public MediumLevelILVariable[] AliasedVariables
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetMediumLevelILAliasedVariables(
				    this.handle ,
				    out ulong arrayLength
			    );

			    return UnsafeUtils.TakeStructArray<BNVariable , MediumLevelILVariable>(
				    arrayPointer,
				    arrayLength,
				    (native) => { return new MediumLevelILVariable(this , native);},
				    NativeMethods.BNFreeVariableList
			    );
		    }
	    }
	    
	    public MediumLevelILSSAVariable[] SSAVariables
	    {
		    get
		    {
			    List<MediumLevelILSSAVariable> targets = new List<MediumLevelILSSAVariable>();
			    
			    foreach (MediumLevelILVariable variable in this.Variables)
			    {
				    ulong[] versions = this.GetVariableSSAVersions(variable);

				    foreach (ulong version in versions)
				    {
					    targets.Add(new MediumLevelILSSAVariable(variable, version));
				    }
			    }
			    
			    return targets.ToArray();
		    }
	    }

	    public ulong[] GetVariableSSAVersions(MediumLevelILVariable variable)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetMediumLevelILVariableSSAVersions(
			    this.handle ,
			    variable.ToNative() ,
			    out ulong arrayLength
		    );
		    
		    return UnsafeUtils.TakeNumberArray<ulong>(
			    arrayPointer,
			    arrayLength,
			    NativeMethods.BNFreeILInstructionList
			);
	    }

	    public TypeWithConfidence GetExpressionType(MediumLevelILExpressionIndex expression)
	    {
		    return TypeWithConfidence.FromNative(
			    NativeMethods.BNGetMediumLevelILExprType(this.handle , expression)
		    );
	    }

	    public RegisterValue GetExpressionValue(MediumLevelILExpressionIndex expression)
	    {
		    return RegisterValue.FromNative(
			    NativeMethods.BNGetMediumLevelILExprValue(this.handle , expression)
		    );
	    }
	    
	    public PossibleValueSet GetExpressionPossibleValues(MediumLevelILExpressionIndex expression , DataFlowQueryOption[] options)
	    {
		    return PossibleValueSet.TakeNative(
			    NativeMethods.BNGetMediumLevelILPossibleExprValues(
				    this.handle , 
				    expression,
				    options,
				    (ulong)options.Length
				)
		    );
	    }
	    
	    
	    public InstructionTextToken[] GetExpressionTokens(
		    MediumLevelILExpressionIndex expression ,
		    Architecture? arch = null,
		    DisassemblySettings? settings = null
		)
	    {
		    bool ok = NativeMethods.BNGetMediumLevelILExprText(
			    this.handle ,
			    null == arch ? this.OwnerFunction.Architecture.DangerousGetHandle() : arch.DangerousGetHandle(),
			    expression,
			    out IntPtr arrayPointer,
			    out ulong arrayLength,
			    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
		    );

		    if (!ok)
		    {
			    return Array.Empty<InstructionTextToken>();
		    }

		    return UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken , InstructionTextToken>(
			    arrayPointer ,
			    arrayLength,
			    InstructionTextToken.FromNative,
			    NativeMethods.BNFreeInstructionText
		    );
	    }

	    public void SetExpressionType(MediumLevelILExpressionIndex expr , TypeWithConfidence type)
	    {
		    NativeMethods.BNSetMediumLevelILExprType(this.handle , expr , type.ToNative());
	    }

	    public void ReplaceExpression(MediumLevelILExpressionIndex oldExpr , MediumLevelILExpressionIndex newExpr)
	    {
		    NativeMethods.BNReplaceMediumLevelILExpr(this.handle , oldExpr , newExpr);
	    }

	    public void SetExpressionAttributes(MediumLevelILExpressionIndex expr , uint attributes)
	    {
		    NativeMethods.BNSetMediumLevelILExprAttributes(this.handle , expr , attributes);
	    }
	    
	    public MediumLevelILInstructionIndex AddInstruction(
		    MediumLevelILExpressionIndex expr,
		    SourceLocation? location = null)
	    {
		    MediumLevelILInstructionIndex result =
			    NativeMethods.BNMediumLevelILAddInstruction(this.handle , expr);
		    this.RecordInstructionTranslation(result, location);

		    return result;
	    }
	    
	    public PossibleValueSet GetSSAVariablePossibleValues(
		    Variable variable ,
		    ulong version ,
		    MediumLevelILInstructionIndex instruction ,
		    DataFlowQueryOption[] options
		 )
	    {
		    return PossibleValueSet.TakeNative(
			    NativeMethods.BNGetMediumLevelILPossibleSSAVarValues(
				    this.handle , 
				    variable.ToNative(),
				    version ,
				    instruction,
				    options,
				    (ulong)options.Length
			    )
		    );
	    }

	    public ulong GetSSAVariableVersionAtInstruction(
		    Variable variable,
		    MediumLevelILInstructionIndex instruction
		)
	    {
		    return NativeMethods.BNGetMediumLevelILSSAVarVersionAtILInstruction(
			    this.handle ,
			    variable.ToNative() ,
			    instruction
		    );
	    }
	    
	    public ulong GetSSAVariableVersionAfterInstruction(
		    Variable variable,
		    MediumLevelILInstructionIndex instruction
	    )
	    {
		    return NativeMethods.BNGetMediumLevelILSSAVarVersionAfterILInstruction(
			    this.handle ,
			    variable.ToNative() ,
			    instruction
		    );
	    }

	    public MediumLevelILVariable GetVariableForRegisterAtInstruction(
		    uint reg,
		    MediumLevelILInstructionIndex instruction
		)
	    {
		    return new MediumLevelILVariable(
			    this,
			    NativeMethods.BNGetMediumLevelILVariableForRegisterAtInstruction(
					this.handle ,
					reg,
					instruction
			    )
		    );
	    }
	    
	    public MediumLevelILVariable GetVariableForRegisterAfterInstruction(
		    RegisterIndex reg,
		    MediumLevelILInstructionIndex instruction
	    )
	    {
		    return new MediumLevelILVariable(
			    this,
			    NativeMethods.BNGetMediumLevelILVariableForRegisterAfterInstruction(
				    this.handle ,
				    reg,
				    instruction
			    )
		    );
	    }
	    
	    public MediumLevelILVariable GetVariableForFlagAtInstruction(
		    FlagIndex flag,
		    MediumLevelILInstructionIndex instruction
	    )
	    {
		    return new MediumLevelILVariable(
			    this,
			    NativeMethods.BNGetMediumLevelILVariableForFlagAtInstruction(
				    this.handle ,
				    flag,
				    instruction
			    )
		    );
	    }
	    
	    public MediumLevelILVariable GetVariableForFlagAfterInstruction(
		    FlagIndex flag,
		    MediumLevelILInstructionIndex instruction
	    )
	    {
		    return new MediumLevelILVariable(
			    this,
			    NativeMethods.BNGetMediumLevelILVariableForFlagAfterInstruction(
				    this.handle ,
				    flag,
				    instruction
			    )
		    );
	    }
	    
	    public MediumLevelILVariable GetVariableForStackLocationAtInstruction(
		    long offset,
		    MediumLevelILInstructionIndex instruction
	    )
	    {
		    return new MediumLevelILVariable(
			    this,
			    NativeMethods.BNGetMediumLevelILVariableForStackLocationAtInstruction(
				    this.handle ,
				    offset,
				    instruction
			    )
		    );
	    }
	    
	    public MediumLevelILVariable GetVariableForStackLocationAfterInstruction(
		    long offset,
		    MediumLevelILInstructionIndex instruction
	    )
	    {
		    return new MediumLevelILVariable(
			    this,
			    NativeMethods.BNGetMediumLevelILVariableForStackLocationAfterInstruction(
				    this.handle ,
				    offset,
				    instruction
			    )
		    );
	    }
	    
	    public RegisterValue GetRegisterValueAtInstruction(
		    uint reg,
		    MediumLevelILInstructionIndex instruction
	    )
	    {
		    return RegisterValue.FromNative(
			    NativeMethods.BNGetMediumLevelILRegisterValueAtInstruction(
				    this.handle ,
				    reg,
				    instruction
			    )
		    );
	    }
	    
	    public RegisterValue GetRegisterValueAfterInstruction(
		    RegisterIndex reg,
		    MediumLevelILInstructionIndex instruction
	    )
	    {
		    return RegisterValue.FromNative(
			    NativeMethods.BNGetMediumLevelILRegisterValueAfterInstruction(
				    this.handle ,
				    reg,
				    instruction
			    )
		    );
	    }
	    
	    public PossibleValueSet GetRegisterPossibleValuesAtInstruction(
		    RegisterIndex reg ,
		    MediumLevelILInstructionIndex instruction ,
		    DataFlowQueryOption[] options
	    )
	    {
		    return PossibleValueSet.TakeNative(
			    NativeMethods.BNGetMediumLevelILPossibleRegisterValuesAtInstruction(
				    this.handle , 
				    reg,
				    instruction,
				    options,
				    (ulong)options.Length
			    )
		    );
	    }
	    
	    public PossibleValueSet GetRegisterPossibleValuesAfterInstruction(
		    RegisterIndex reg ,
		    MediumLevelILInstructionIndex instruction ,
		    DataFlowQueryOption[] options
	    )
	    {
		    return PossibleValueSet.TakeNative(
			    NativeMethods.BNGetMediumLevelILPossibleRegisterValuesAfterInstruction(
				    this.handle , 
				    reg,
				    instruction,
				    options,
				    (ulong)options.Length
			    )
		    );
	    }
	    
	    public RegisterValue GetFlagValueAtInstruction(
		    FlagIndex flag,
		    MediumLevelILInstructionIndex instruction
	    )
	    {
		    return RegisterValue.FromNative(
			    NativeMethods.BNGetMediumLevelILFlagValueAtInstruction(
				    this.handle ,
				    flag,
				    instruction
			    )
		    );
	    }
	    
	    public RegisterValue GetFlagValueAfterInstruction(
		    FlagIndex flag,
		    MediumLevelILInstructionIndex instruction
	    )
	    {
		    return RegisterValue.FromNative(
			    NativeMethods.BNGetMediumLevelILFlagValueAfterInstruction(
				    this.handle ,
				    flag,
				    instruction
			    )
		    );
	    }
	    
	    public PossibleValueSet GetFlagPossibleValuesAtInstruction(
		    FlagIndex flag ,
		    MediumLevelILInstructionIndex instruction ,
		    DataFlowQueryOption[] options
	    )
	    {
		    return PossibleValueSet.TakeNative(
			    NativeMethods.BNGetMediumLevelILPossibleFlagValuesAtInstruction(
				    this.handle , 
				    flag,
				    instruction,
				    options,
				    (ulong)options.Length
			    )
		    );
	    }
	    
	    public PossibleValueSet GetFlagPossibleValuesAfterInstruction(
		    FlagIndex flag ,
		    MediumLevelILInstructionIndex instruction ,
		    DataFlowQueryOption[] options
	    )
	    {
		    return PossibleValueSet.TakeNative(
			    NativeMethods.BNGetMediumLevelILPossibleFlagValuesAfterInstruction(
				    this.handle , 
				    flag,
				    instruction,
				    options,
				    (ulong)options.Length
			    )
		    );
	    }
	    
	    public RegisterValue StackContentsAtInstruction(
		    long offset,
		    ulong length,
		    MediumLevelILInstructionIndex instruction
	    )
	    {
		    return RegisterValue.FromNative(
			    NativeMethods.BNGetMediumLevelILStackContentsAtInstruction(
				    this.handle ,
				    offset,
				    length,
				    instruction
			    )
		    );
	    }
	    
	    public RegisterValue GetStackContentsAfterInstruction(
		    long offset,
		    ulong length,
		    MediumLevelILInstructionIndex instruction
	    )
	    {
		    return RegisterValue.FromNative(
			    NativeMethods.BNGetMediumLevelILStackContentsAfterInstruction(
				    this.handle ,
				    offset,
				    length,
				    instruction
			    )
		    );
	    }

	    public PossibleValueSet GetStackContentsPossibleValuesAtInstruction(
		    long offset,
		    ulong length,
		    MediumLevelILInstructionIndex instruction ,
		    DataFlowQueryOption[] options
	    )
	    {
		    return PossibleValueSet.TakeNative(
			    NativeMethods.BNGetMediumLevelILPossibleStackContentsAtInstruction(
				    this.handle , 
				    offset,
				    length,
				    instruction,
				    options,
				    (ulong)options.Length
			    )
		    );
	    }
	    
	    public PossibleValueSet GetStackContentsPossibleValuesAfterInstruction(
		    long offset,
		    ulong length,
		    MediumLevelILInstructionIndex instruction ,
		    DataFlowQueryOption[] options
	    )
	    {
		    return PossibleValueSet.TakeNative(
			    NativeMethods.BNGetMediumLevelILPossibleStackContentsAfterInstruction(
				    this.handle , 
				    offset,
				    length,
				    instruction,
				    options,
				    (ulong)options.Length
			    )
		    );
	    }
	    
	    public ILBranchInstructionAndDependence[] GetAllBranchDependence(MediumLevelILInstructionIndex instruction)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetAllMediumLevelILBranchDependence(
			    this.handle ,
			    instruction ,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArray<BNILBranchInstructionAndDependence , ILBranchInstructionAndDependence>(
			    arrayPointer,
			    arrayLength ,
			    ILBranchInstructionAndDependence.FromNative,
			    NativeMethods.BNFreeILBranchDependenceList
		    );
	    }
	    
	    public ILBranchDependence GetBranchDependence(
		    MediumLevelILInstructionIndex instruction,
		    MediumLevelILInstructionIndex branch
	    )
	    {
		    return NativeMethods.BNGetMediumLevelILBranchDependence(
			    this.handle ,
			    instruction ,
			    branch
		    );
	    }

	    public uint GetDefaultIndexForVariableDefinition(
		    Variable variable,
		    MediumLevelILInstructionIndex instruction
		)
	    {
		    return NativeMethods.BNGetDefaultIndexForMediumLevelILVariableDefinition(
			    this.handle ,
			    variable.ToNative() ,
			    instruction
		    );
	    }

	    public ulong[] GetExpressionOperands(
		    MediumLevelILExpressionIndex expression, 
		    ulong operand
		)
	    {
		    IntPtr arrayPointer = NativeMethods.BNMediumLevelILGetOperandList(
			    this.handle ,
			    expression ,
			    operand ,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeNumberArray<ulong>(
			    arrayPointer ,
			    arrayLength ,
			    NativeMethods.BNMediumLevelILFreeOperandList
		    );
	    }

	    public MediumLevelILVariable[] GetExpressionVariables(
		    MediumLevelILExpressionIndex expression , 
		    ulong operand
		)
	    {
		    List<MediumLevelILVariable>  variables = new List<MediumLevelILVariable>();
		    
		    ulong[] identifiers = this.GetExpressionOperands(expression, operand);

		    foreach (ulong identifier in identifiers)
		    {
			    variables.Add( 
				    MediumLevelILVariable.FromIdentifier(this , identifier)
				);
		    }
		    
		    return variables.ToArray();
	    }
	    
	    public PluginCommand[] ValidPluginCommands
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommandsForMediumLevelILFunction(
				    this.OwnerFunction.View.DangerousGetHandle(),
				    this.DangerousGetHandle(),
				    out ulong arrayLength
			    );

			    return UnsafeUtils.TakeStructArray<BNPluginCommand , PluginCommand>(
				    arrayPointer ,
				    arrayLength ,
				    PluginCommand.FromNative ,
				    NativeMethods.BNFreePluginCommandList
			    );
		    }
	    }
	    
	    public PluginCommand[] GetValidPluginCommandsForInstruction(ulong instruction)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommandsForMediumLevelILInstruction(
			    this.OwnerFunction.View.DangerousGetHandle(),
			    this.DangerousGetHandle(),
			    instruction,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArray<BNPluginCommand , PluginCommand>(
			    arrayPointer ,
			    arrayLength ,
			    PluginCommand.FromNative ,
			    NativeMethods.BNFreePluginCommandList
		    );
	    }
	    
	    public MediumLevelILExpressionIndex AddExpression(
		    MediumLevelILOperation operation,
		    SourceLocation? location = null,
		    ulong size = 0, 
		    params ulong[] operands
	    )
	    {
		    MediumLevelILExpressionIndex result;
		    ulong a = 0;
		    ulong b = 0;
		    ulong c = 0;
		    ulong d = 0;
		    ulong e = 0;

		    if (operands.Length > 0)
		    {
			    a = operands[0];
		    }
		    
		    if (operands.Length > 1)
		    {
			    b = operands[1];
		    }
		    
		    if (operands.Length > 2)
		    {
			    c = operands[2];
		    }
		    
		    if (operands.Length > 3)
		    {
			    d = operands[3];
		    }
		    
		    if (operands.Length > 4)
		    {
			    e = operands[4];
		    }
		    
		    if (null == location)
		    {
			    result = NativeMethods.BNMediumLevelILAddExpr(
				    this.handle ,
				    operation,
				    size ,
				    a,
				    b ,
				    c ,
				    d ,
				    e
			    );
		    }
		    else
		    {
			    result = NativeMethods.BNMediumLevelILAddExprWithLocation(
				    this.handle ,
				    operation,
				    location.Address,
				    location.Operand,
				    size ,
				    a,
				    b ,
				    c ,
				    d ,
				    e
			    );
		    }

		    this.RecordExpressionTranslation(result, location);

		    return result;
	    }

	    public MediumLevelILExpressionIndex EmitNop(SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_NOP ,
			    location
			);
	    }
	    
	    public MediumLevelILExpressionIndex EmitSetVar(
		    ulong size,
		    Variable dest,
		    MediumLevelILExpressionIndex source,
		    SourceLocation? location = null
		)
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_SET_VAR,
			    location,
			    size,
			    dest.Identifier,
			    (ulong)source
			);
	    }
	    
	    public MediumLevelILExpressionIndex EmitSetVarField(
		    ulong size,
		    Variable dest,
		    ulong offset,
		    MediumLevelILExpressionIndex source,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_SET_VAR_FIELD,
			    location,
			    size,
			    dest.Identifier,
			    offset,
			    (ulong)source
		    );
	    }
	    
	  
	    public MediumLevelILExpressionIndex EmitSetVarSplit(
		    ulong size,
		    Variable destHigh,
		    Variable destLow,
		    MediumLevelILExpressionIndex source,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_SET_VAR_SPLIT,
			    location,
			    size,
			    destHigh.Identifier,
			    destLow.Identifier,
			    (ulong)source
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitAssert(
		    ulong size,
		    Variable source,
		    PossibleValueSet constraint,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_ASSERT,
			    location,
			    size,
			    source.Identifier,
			    (ulong)this.CachePossibleValueSet(constraint)
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitForceVer(
		    ulong size,
		    Variable dest,
		    Variable source,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_FORCE_VER,
			    location,
			    size,
			    dest.Identifier,
			    source.Identifier
		    );
	    }
	
	    public MediumLevelILExpressionIndex EmitLoad(
		    ulong size,
		    MediumLevelILExpressionIndex source,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_LOAD,
			    location,
			    size,
			    (ulong)source
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitLoadStruct(
		    ulong size,
		    MediumLevelILExpressionIndex source,
		    ulong offset,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_LOAD_STRUCT,
			    location,
			    size,
			    (ulong)source,
			    offset
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitStore(
		    ulong size,
		    MediumLevelILExpressionIndex dest,
		    MediumLevelILExpressionIndex source,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_STORE,
			    location,
			    size,
			    (ulong)dest,
			    (ulong)source
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitStoreStruct(
		    ulong size,
		    MediumLevelILExpressionIndex dest,
		    ulong offset,
		    MediumLevelILExpressionIndex source,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_STORE_STRUCT,
			    location,
			    size,
			    (ulong)dest,
			    (ulong)offset,
			    (ulong)source
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitVar(
		    ulong size,
		    Variable source,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_VAR,
			    location,
			    size,
			    (ulong)source.Identifier
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitVarField(
		    ulong size,
		    Variable source,
		    ulong offset,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_VAR_FIELD,
			    location,
			    size,
			    (ulong)source.Identifier,
			    offset
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitVarSplit(
		    ulong size,
		    Variable high,
		    Variable low,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_VAR_SPLIT,
			    location,
			    size,
			    (ulong)high.Identifier,
			    (ulong)low.Identifier
		    );
	    }
	    
	    
	    public MediumLevelILExpressionIndex EmitAddressOf(
		    Variable variable,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_ADDRESS_OF,
			    location,
			    0,
			    (ulong)variable.Identifier
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitConst(
		    ulong size,
		    ulong value ,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_CONST,
			    location,
			    size,
			    value
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitConstData(
		    ulong size,
		    RegisterValue data, // state must ConstantDataValue
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_CONST_DATA,
			    location,
			    size,
			    (ulong)data.State,
			    (ulong)data.Value
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitConstPointer(
		    ulong size,
		    ulong value,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_CONST_PTR,
			    location,
			    size,
			    value
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitExternPointer(
		    ulong size,
		    ulong value,
		    ulong offset,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_EXTERN_PTR,
			    location,
			    size,
			    value,
			    offset
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitFloatConst(
		    ulong size,
		    ulong value,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_FLOAT_CONST,
			    location,
			    size,
			    value
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitImport(
		    ulong size,
		    ulong value,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_IMPORT,
			    location,
			    size,
			    value
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitAdd(
		    ulong size,
		    MediumLevelILExpressionIndex a,
		    MediumLevelILExpressionIndex b,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_ADD,
			    location,
			    size,
			    (ulong)a,
			    (ulong)b
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitAddCarry(
		    ulong size,
		    MediumLevelILExpressionIndex a,
		    MediumLevelILExpressionIndex b,
		    MediumLevelILExpressionIndex carry,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_ADC,
			    location,
			    size,
			    (ulong)a,
			    (ulong)b,
			    (ulong) carry
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitSub(
		    ulong size,
		    MediumLevelILExpressionIndex a,
		    MediumLevelILExpressionIndex b,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_SUB,
			    location,
			    size,
			    (ulong)a,
			    (ulong) b
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitSubBorrow(
		    ulong size,
		    MediumLevelILExpressionIndex a,
		    MediumLevelILExpressionIndex b,
		    MediumLevelILExpressionIndex carry,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_SBB,
			    location,
			    size,
			    (ulong)a,
			    (ulong)b,
			    (ulong)carry
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitAnd(
		    ulong size,
		    MediumLevelILExpressionIndex a,
		    MediumLevelILExpressionIndex b,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_AND,
			    location,
			    size,
			    (ulong)a,
			    (ulong)b
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitOr(
		    ulong size,
		    MediumLevelILExpressionIndex a,
		    MediumLevelILExpressionIndex b,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_OR,
			    location,
			    size,
			    (ulong)a,
			    (ulong) b
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitXor(
		    ulong size,
		    MediumLevelILExpressionIndex a,
		    MediumLevelILExpressionIndex b,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_XOR,
			    location,
			    size,
			    (ulong)a,
			    (ulong)b
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitShiftLeft(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_LSL,
			    location,
			    size,
			    (ulong)left,
			    (ulong) right
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitLogicalShiftRight(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_LSR,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitArithShiftRight(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_ASR,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitRotateLeft(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_ROL,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitRotateLeftCarry(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_RLC,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitRorateRight(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_ROR,
			    location,
			    size,
			    (ulong)left,
			    (ulong) right
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitRotateRightCarry(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    MediumLevelILExpressionIndex carry,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_RRC,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right,
			    (ulong) carry
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitMUL(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_MUL,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitMulDoublePrecUnsigned(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_MULU_DP,
			    location,
			    size,
			    (ulong)left,
			    (ulong) right
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitMulDoublePrecSigned(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_MULS_DP,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitDivUnsigned(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_DIVU,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitDivDoublePrecUnsigned(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_DIVU_DP,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	  
	    public MediumLevelILExpressionIndex EmitDivSigned(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_DIVS,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	
	    public MediumLevelILExpressionIndex EmitDivDoublePrecSigned(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_DIVS_DP,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	  
	    public MediumLevelILExpressionIndex EmitModUnsigned(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_MODU,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitModDoublePrecUnsigned(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_MODU_DP,
			    location,
			    size,
			    (ulong)left,
			    (ulong) right
		    );
	    }
	    
	 
	    public MediumLevelILExpressionIndex EmitModSigned(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_MODS,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    
	    public MediumLevelILExpressionIndex EmitModDoublePrecSigned(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_MODS_DP,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	
	    public MediumLevelILExpressionIndex EmitNeg(
		    ulong size,
		    MediumLevelILExpressionIndex value,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_NEG,
			    location,
			    size,
			    (ulong)value
		    );
	    }
	   
	    public MediumLevelILExpressionIndex EmitNot(
		    ulong size,
		    MediumLevelILExpressionIndex value,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_NOT,
			    location,
			    size,
			    (ulong)value
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitSignExtend(
		    ulong size,
		    MediumLevelILExpressionIndex value,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_SX,
			    location,
			    size,
			    (ulong)value
		    );
	    }
	  
	    public MediumLevelILExpressionIndex EmitZeroExtend(
		    ulong size,
		    MediumLevelILExpressionIndex value,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_ZX,
			    location,
			    size,
			    (ulong)value
		    );
	    }
	
	    public MediumLevelILExpressionIndex EmitLowPart(
		    ulong size,
		    MediumLevelILExpressionIndex value,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_LOW_PART,
			    location,
			    size,
			    (ulong)value
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitJUMP(
		    ulong size,
		    MediumLevelILExpressionIndex dest,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_JUMP,
			    location,
			    size,
			    (ulong)dest
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitJumpTo(
		    MediumLevelILExpressionIndex dest,
		    IDictionary<ulong, MediumLevelILLabel> targets,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_JUMP_TO,
			    location,
			    0,
			    (ulong)dest,
			    (ulong)targets.Count * 2,
			    (ulong)this.AddLabelMap(targets)
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitCall(
		    Variable[] outputs,
		    MediumLevelILExpressionIndex dest,
		    MediumLevelILExpressionIndex[] parameters,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_CALL,
			    location,
			    0,
			    (ulong)outputs.Length,
			    (ulong)this.AddVariableList(outputs),
			    (ulong)dest,
			    (ulong)parameters.Length,
			    (ulong)this.AddOperandList(parameters)
		    );
	    }
	
	    public MediumLevelILExpressionIndex EmitCALL_UNTYPED(
		    Variable[] outputs,
		    MediumLevelILExpressionIndex dest,
		    MediumLevelILExpressionIndex[] parameters,
		    MediumLevelILExpressionIndex stack,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_CALL_UNTYPED,
			    location,
			    0,
			    (ulong)this.EmitCallOutput(outputs, location),
			    (ulong)dest,
			    (ulong)this.EmitCallParam(parameters, location),
			    (ulong)stack
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitCallOutput(
		    Variable[] outputs,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_CALL_OUTPUT,
			    location,
			    0,
			    (ulong)outputs.Length,
			    (ulong)this.AddVariableList(outputs)
		    );
	    }
	    
	   
	    public MediumLevelILExpressionIndex EmitCallParam(
		    MediumLevelILExpressionIndex[] parameters,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_CALL_PARAM,
			    location,
			    0,
			    (ulong)parameters.Length,
			    (ulong)this.AddOperandList(parameters)
		    );
	    }
	    
	 
	    public MediumLevelILExpressionIndex EmitRet(
		    MediumLevelILExpressionIndex[] sources,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_RET,
			    location,
			    0,
			    (ulong)sources.Length,
			    (ulong)this.AddOperandList(sources)
		    );
	    }
	 
	    public MediumLevelILExpressionIndex EmitNoRet(
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_NORET,
			    location,
			    0
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitIf(
		    MediumLevelILExpressionIndex condition,
		    MediumLevelILLabel tureLabel,
		    MediumLevelILLabel falseLabel,
		    SourceLocation? location = null
	    )
	    {
		    MediumLevelILExpressionIndex result;

		    if (null == location)
		    {
			    result = NativeMethods.BNMediumLevelILIf(
				    this.handle ,
				    condition ,
				    tureLabel.ToNative() ,
				    falseLabel.ToNative() 
			    );
		    }
		    else
		    {
			    result = NativeMethods.BNMediumLevelILIfWithLocation(
				    this.handle ,
				    condition ,
				    tureLabel.ToNative() ,
				    falseLabel.ToNative() ,
				    location.Address ,
				    (uint)location.Operand
			    );
		    }

		    this.RecordExpressionTranslation(result, location);
		    return result;
	    }
	
	    public MediumLevelILExpressionIndex EmitGoto(
		    MediumLevelILLabel label,
		    SourceLocation? location = null
	    )
	    {
		    MediumLevelILExpressionIndex result;

		    if (null == location)
		    {
			    result = NativeMethods.BNMediumLevelILGoto(
				    this.handle,
				    label.ToNative()
			    );
		    }
		    else
		    {
			    result = NativeMethods.BNMediumLevelILGotoWithLocation(
				    this.handle,
				    label.ToNative(),
				    location.Address,
				    location.Operand
			    );
		    }

		    this.RecordExpressionTranslation(result, location);
		    return result;
	    }
	    
	    public MediumLevelILExpressionIndex EmitEqual(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_CMP_E,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	 
	    public MediumLevelILExpressionIndex EmitNotEqual(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_CMP_NE,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitSignedLessThan(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_CMP_SLT,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitUnsignedLessThan(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_CMP_ULT,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	
	    public MediumLevelILExpressionIndex EmitSignedLessThanOrEqual(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_CMP_SLE,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	
	    public MediumLevelILExpressionIndex EmitUnsignedLessThanOrEqual(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_CMP_ULE,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitSignedGreaterThanOrEqual(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_CMP_SGE,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitUnsignedGreaterThanOrEqual(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_CMP_UGE,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	 
	    public MediumLevelILExpressionIndex EmitSignedGreaterThan(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_CMP_SGT,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitUnsignedGreaterThan(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_CMP_UGT,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitTestBit(
		    ulong size,
		    MediumLevelILExpressionIndex a,
		    MediumLevelILExpressionIndex b,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_TEST_BIT,
			    location,
			    size,
			    (ulong)a,
			    (ulong) b
		    );
	    }

	    public MediumLevelILExpressionIndex EmitBoolToInt(
		    ulong size,
		    MediumLevelILExpressionIndex a,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_BOOL_TO_INT,
			    location,
			    size,
			    (ulong)a
		    );
	    }
	   
	    public MediumLevelILExpressionIndex EmitSysCall(
		    Variable[] outputs,
		    MediumLevelILExpressionIndex[] parameters,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_SYSCALL,
			    location,
			    0,
			    (ulong)outputs.Length,
			    (ulong)this.AddVariableList(outputs),
			    (ulong)parameters.Length,
			    (ulong)this.AddOperandList(parameters)
		    );
	    }
	    
	  
	    public MediumLevelILExpressionIndex EmitSysCallUntyped(
		    Variable[] outputs,
		    MediumLevelILExpressionIndex[] parameters,
		    MediumLevelILExpressionIndex stack,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_SYSCALL_UNTYPED,
			    location,
			    0,
			    (ulong)this.EmitCallOutput(outputs, location),
			    (ulong)this.EmitCallParam(parameters , location),
			    (ulong)stack
		    );
	    }
	 
	    public MediumLevelILExpressionIndex EmitTailCall(
		    Variable[] outputs,
		    MediumLevelILExpressionIndex dest,
		    MediumLevelILExpressionIndex[] parameters,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_TAILCALL,
			    location,
			    0,
			    (ulong)outputs.Length,
			    (ulong)this.AddVariableList(outputs),
			    (ulong)dest,
			    (ulong)parameters.Length,
			    (ulong)this.AddOperandList(parameters)
		    );
	    }
	
	    public MediumLevelILExpressionIndex EmitTailCallUntyped(
		    Variable[] outputs,
		    MediumLevelILExpressionIndex dest,
		    MediumLevelILExpressionIndex[] parameters,
		    MediumLevelILExpressionIndex stack,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_TAILCALL_UNTYPED,
			    location,
			    0,
			    (ulong)this.EmitCallOutput(outputs, location),
			    (ulong)dest,
			    (ulong)this.EmitCallParam(parameters , location),
			    (ulong)stack
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitIntrinsic(
		    Variable[] outputs,
		    IntrinsicIndex  intrinsic,
		    MediumLevelILExpressionIndex[] parameters,
		    MediumLevelILExpressionIndex stack,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_INTRINSIC,
			    location,
			    0,
			    (ulong)outputs.Length,
			    (ulong)this.AddVariableList(outputs),
			    (ulong)intrinsic,
			  (ulong)parameters.Length,
			  (ulong)this.AddOperandList(parameters)
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitFreeVarSlot(
		    ulong size,
		    Variable variable,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_FREE_VAR_SLOT,
			    location,
			    size,
			    variable.Identifier
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitBreakpoint(
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_BP,
			    location,
			    0
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitTrap(
		    ulong value,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_TRAP,
			    location,
			    0,
			    value
		    );
	    }

	    public MediumLevelILExpressionIndex EmitUndefined(
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_UNDEF,
			    location,
			    0
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitUnimplemented(
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_UNIMPL,
			    location,
			    0
		    );
	    }
	  
	    public MediumLevelILExpressionIndex EmitUnimplementedMemoryRef(
		    ulong size,
		    MediumLevelILExpressionIndex source,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_UNIMPL_MEM,
			    location,
			    size,
			    (ulong)source
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitFloatAdd(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_FADD,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	 
	    public MediumLevelILExpressionIndex EmitFloatSub(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_FSUB,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	
	    public MediumLevelILExpressionIndex EmitFloatMul(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_FMUL,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitFloatDiv(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_FDIV,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitFloatSquareRoot(
		    ulong size,
		    MediumLevelILExpressionIndex operand,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_FSQRT,
			    location,
			    size,
			    (ulong)operand
		    );
	    }
	    
	  
	    public MediumLevelILExpressionIndex EmitFloatNeg(
		    ulong size,
		    MediumLevelILExpressionIndex operand,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_FNEG,
			    location,
			    size,
			    (ulong)operand
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitFloatAbs(
		    ulong size,
		    MediumLevelILExpressionIndex operand,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_FABS,
			    location,
			    size,
			    (ulong)operand
		    );
	    }
	
	    public MediumLevelILExpressionIndex EmitFloatToInt(
		    ulong size,
		    MediumLevelILExpressionIndex operand,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_FLOAT_TO_INT,
			    location,
			    size,
			    (ulong)operand
		    );
	    }
	   
	    public MediumLevelILExpressionIndex EmitIntToFloat(
		    ulong size,
		    MediumLevelILExpressionIndex operand,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_INT_TO_FLOAT,
			    location,
			    size,
			    (ulong)operand
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitFloatConvert(
		    ulong size,
		    MediumLevelILExpressionIndex operand,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_FLOAT_CONV,
			    location,
			    size,
			    (ulong)operand
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitRoundToInt(
		    ulong size,
		    MediumLevelILExpressionIndex operand,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_ROUND_TO_INT,
			    location,
			    size,
			    (ulong)operand
		    );
	    }
	
	    public MediumLevelILExpressionIndex EmitFloor(
		    ulong size,
		    MediumLevelILExpressionIndex operand,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_FLOOR,
			    location,
			    size,
			    (ulong)operand
		    );
	    }
	
	    public MediumLevelILExpressionIndex EmitCeil(
		    ulong size,
		    MediumLevelILExpressionIndex operand,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_CEIL,
			    location,
			    size,
			    (ulong)operand
		    );
	    }
	    

	    public MediumLevelILExpressionIndex EmitFloatTrunc(
		    ulong size,
		    MediumLevelILExpressionIndex operand,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_FTRUNC,
			    location,
			    size,
			    (ulong)operand
		    );
	    }
	    
	 
	    public MediumLevelILExpressionIndex EmitFloatEqual(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_FCMP_E,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	  
	    public MediumLevelILExpressionIndex EmitFloatNotEqual(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_FCMP_NE,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	
	    public MediumLevelILExpressionIndex EmitFloatLessThan(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_FCMP_LT,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	   
	    public MediumLevelILExpressionIndex EmitFloatLessThanOrEqual(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_FCMP_LE,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	 
	    public MediumLevelILExpressionIndex EmitFloatGreaterThanOrEqual(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_FCMP_GE,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitFloatGreaterThan(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_FCMP_GT,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitFloatOrdered(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_FCMP_O,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public MediumLevelILExpressionIndex EmitFloatUnordered(
		    ulong size,
		    MediumLevelILExpressionIndex left,
		    MediumLevelILExpressionIndex right,
		    SourceLocation? location = null
	    )
	    {
		    return this.AddExpression(
			    MediumLevelILOperation.MLIL_FCMP_UO,
			    location,
			    size,
			    (ulong)left,
			    (ulong)right
		    );
	    }

	    // ===================================================================
	    // Instruction text and mutation
	    // ===================================================================

	    /// <summary>
	    /// Retrieves the text tokens for the specified MLIL instruction index.
	    /// </summary>
	    /// <param name="instruction">The instruction index to get text for.</param>
	    /// <param name="arch">Optional architecture override. Uses the owner function's architecture when null.</param>
	    /// <param name="settings">Optional disassembly settings controlling display.</param>
	    /// <returns>An array of InstructionTextToken objects, or empty if retrieval failed.</returns>
	    public unsafe InstructionTextToken[] GetInstructionText(
		    ulong instruction ,
		    Architecture? arch = null ,
		    DisassemblySettings? settings = null
	    )
	    {
		    IntPtr tokensPtr = IntPtr.Zero;
		    ulong count = 0;

		    bool ok = NativeMethods.BNGetMediumLevelILInstructionText(
			    this.handle ,
			    this.OwnerFunction.DangerousGetHandle() ,
			    null == arch ? this.OwnerFunction.Architecture.DangerousGetHandle() : arch.DangerousGetHandle() ,
			    instruction ,
			    (IntPtr)(&tokensPtr) ,
			    (IntPtr)(&count) ,
			    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
		    );

		    if (!ok)
		    {
			    return Array.Empty<InstructionTextToken>();
		    }

		    return UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken , InstructionTextToken>(
			    tokensPtr ,
			    count ,
			    InstructionTextToken.FromNative ,
			    NativeMethods.BNFreeInstructionText
		    );
	    }

	    /// <summary>
	    /// Marks the specified MLIL instruction for removal during finalization.
	    /// </summary>
	    /// <param name="instr">The instruction index to mark for removal.</param>
	    public void MarkInstructionForRemoval(ulong instr)
	    {
		    NativeMethods.BNMarkMediumLevelILInstructionForRemoval(this.handle , instr);
	    }

	    /// <summary>
	    /// Replaces the specified MLIL instruction with a different expression.
	    /// </summary>
	    /// <param name="instr">The instruction index to replace.</param>
	    /// <param name="expr">The expression index to use as the replacement.</param>
	    public void ReplaceInstruction(ulong instr , ulong expr)
	    {
		    NativeMethods.BNReplaceMediumLevelILInstruction(this.handle , instr , expr);
	    }

	    /// <summary>
	    /// Updates a specific operand of an MLIL instruction to a new value.
	    /// </summary>
	    /// <param name="instr">The instruction index to modify.</param>
	    /// <param name="operandIndex">The operand index within the instruction.</param>
	    /// <param name="value">The new value for the operand.</param>
	    public void UpdateOperand(ulong instr , ulong operandIndex , ulong value)
	    {
		    NativeMethods.BNUpdateMediumLevelILOperand(this.handle , instr , operandIndex , value);
	    }
	}

}
