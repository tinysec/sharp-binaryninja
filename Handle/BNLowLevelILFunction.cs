using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed partial class LowLevelILFunction : AbstractSafeHandle<LowLevelILFunction>
	{
		private readonly bool isSSAForm;
		private readonly Architecture? architecture;

		/// <summary>
		/// Gets whether this function is in SSA form.
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
		
	    internal LowLevelILFunction(
		    IntPtr handle ,
		    bool owner ,
		    bool ssa = false,
		    Architecture? architecture = null
		) : base(handle , owner)
	    {
	       this.isSSAForm = ssa;
	       this.architecture = null == architecture
		       ? null
		       : BinaryNinja.Architecture.MustFromHandle(architecture.DangerousGetHandle());
	    }

	    private static FunctionGraphType GetILForm(IntPtr functionHandle)
	    {
		    IntPtr blocks = NativeMethods.BNGetLowLevelILBasicBlockList(
			    functionHandle,
			    out ulong count);
		    return ILFunctionNavigation.TakeFunctionGraphType(blocks, count);
	    }
	    
	    internal static LowLevelILFunction? NewFromHandle(
		    IntPtr handle,
		    bool ssa = false,
		    Architecture? architecture = null
		)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new LowLevelILFunction(
			    NativeMethods.BNNewLowLevelILFunctionReference(handle) ,
			    true,
			    ssa,
			    architecture
		    );
	    }
	    
	    internal static LowLevelILFunction MustNewFromHandle(
		    IntPtr handle,
		    bool ssa = false,
		    Architecture? architecture = null
		)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new LowLevelILFunction(
			    NativeMethods.BNNewLowLevelILFunctionReference(handle) ,
			    true,
			    ssa,
			    architecture
		    );
	    }
	    
	    internal static LowLevelILFunction? TakeHandle(
		    IntPtr handle,
		    bool ssa = false,
		    Architecture? architecture = null
		)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new LowLevelILFunction(handle, true, ssa, architecture);
	    }
	    
	    internal static LowLevelILFunction MustTakeHandle(
		    IntPtr handle,
		    bool ssa = false,
		    Architecture? architecture = null
		)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new LowLevelILFunction(handle, true, ssa, architecture);
	    }
	    
	    internal static LowLevelILFunction? BorrowHandle(
		    IntPtr handle,
		    bool ssa = false,
		    Architecture? architecture = null
		)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new LowLevelILFunction(handle, false, ssa, architecture);
	    }
	    
	    internal static LowLevelILFunction MustBorrowHandle(
		    IntPtr handle,
		    bool ssa = false,
		    Architecture? architecture = null
		)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new LowLevelILFunction(handle, false, ssa, architecture);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeLowLevelILFunction(this.handle);
	            this.SetHandleAsInvalid();
	        }

	        return true;
	    }

        // ===================================================================
        // Static factory methods
        // ===================================================================

        /// <summary>
        /// Creates a new low-level IL function for the given architecture and optional owner function.
        /// </summary>
        /// <param name="arch">The architecture this LLIL function targets.</param>
        /// <param name="owner">Optional owning function; null for a standalone LLIL function.</param>
        /// <returns>A new owned LowLevelILFunction instance.</returns>
        public static LowLevelILFunction Create(Architecture arch , Function? owner = null)
        {
            // 1. Resolve the optional function handle.
            IntPtr ownerHandle = (owner != null) ? owner.DangerousGetHandle() : IntPtr.Zero;

            // 2. Call the native factory to create the LLIL function.
            IntPtr result = NativeMethods.BNCreateLowLevelILFunction(
                arch.DangerousGetHandle() ,
                ownerHandle
            );

            // 3. Wrap as a new owned handle (non-SSA by default).
		    return LowLevelILFunction.MustTakeHandle(result, false, arch);
        }

        // ===================================================================
        // Instance properties and methods
        // ===================================================================

	    public Function? SourceFunction
	    {
		    get
		    {
			    return Function.TakeHandle(
				    NativeMethods.BNGetLowLevelILOwnerFunction(this.handle) 
			    );
		    }
	    }

	    public Function OwnerFunction
	    {
		    get
		    {
			    Function? function = this.SourceFunction;

			    if (null == function)
			    {
				    throw new InvalidOperationException(
					    "Standalone low-level IL functions do not have an owner function.");
			    }

			    return function;
		    }
	    }

	    public Architecture Architecture
	    {
		    get
		    {
			    if (null != this.architecture)
			    {
				    return this.architecture;
			    }

			    using Function? function = this.SourceFunction;
			    if (null == function)
			    {
				    throw new InvalidOperationException(
					    "The architecture is unavailable for this low-level IL function.");
			    }

			    return function.Architecture;
		    }
	    }
	    
	    public ulong InstructionCount
	    {
		    get
		    {
			    return NativeMethods.BNGetLowLevelILInstructionCount(this.handle);
		    }
	    }
	    
	    public LowLevelILInstruction? GetExpression(LowLevelILExpressionIndex expr)
	    {
		    if ( (ulong)expr >= this.ExpressionCount)
		    {
			    return null;
		    }
		    
		    return LowLevelILInstruction.FromExpressionIndex(
			    this,
			    expr
		    );
	    }
	    
	    public LowLevelILInstruction MustGetExpression(LowLevelILExpressionIndex expr)
	    {
		    if ( (ulong)expr >= this.ExpressionCount)
		    {
			    throw new IndexOutOfRangeException(nameof(expr));
		    }
		    
		    return LowLevelILInstruction.FromExpressionIndex(
			    this,
			    expr
		    );
	    }
	    
	    public LowLevelILInstruction? GetInstruction(LowLevelILInstructionIndex instr)
	    {
		    if ( (ulong)instr >= this.InstructionCount)
		    {
			    return null;
		    }
		    
		    LowLevelILExpressionIndex expr = NativeMethods.BNGetLowLevelILIndexForInstruction(
			    this.handle,
			    instr
		    );
		    
		    return this.GetExpression(expr);
	    }
	    
	    public LowLevelILInstruction MustGetInstruction(LowLevelILInstructionIndex instr)
	    {
		    if ( (ulong)instr >= this.InstructionCount)
		    {
			    throw new IndexOutOfRangeException(nameof(instr));
		    }
		    
		    LowLevelILExpressionIndex expr = NativeMethods.BNGetLowLevelILIndexForInstruction(
			    this.handle,
			    instr
		    );
		    
		    return this.MustGetExpression(expr);
	    }
	    
	    public LowLevelILInstruction[] MustGetExpressions(LowLevelILExpressionIndex[] indexes)
	    {
		    List<LowLevelILInstruction> instructions = new List<LowLevelILInstruction>();

		    foreach (LowLevelILExpressionIndex index in indexes)
		    {
			    instructions.Add( this.MustGetExpression(index) );
		    }

		    return instructions.ToArray();
	    }
	    
	    public LowLevelILInstruction[] MustGetInstructions(LowLevelILInstructionIndex[] indexes)
	    {
		    List<LowLevelILInstruction> instructions = new List<LowLevelILInstruction>();

		    foreach (LowLevelILInstructionIndex index in indexes)
		    {
			    instructions.Add( this.MustGetInstruction(index) );
		    }

		    return instructions.ToArray();
	    }

	    
	    public LowLevelILInstruction this[LowLevelILInstructionIndex index]
	    {
		    get
		    {
			    return this.MustGetInstruction(index);
		    }
	    }
	    
	    public IEnumerable<LowLevelILInstruction> Instructions
	    {
		    get
		    {
			    foreach (LowLevelILBasicBlock basicBlock in BasicBlocks)
			    {
				    foreach (LowLevelILInstruction instruction in basicBlock.Instructions)
				    {
					    yield return instruction;
				    }
			    }
		    }
	    }
	    
	    public ulong ExpressionCount
	    {
		    get
		    {
			    return NativeMethods.BNGetLowLevelILExprCount(this.handle);
		    }
	    }
	    
	    public LowLevelILBasicBlock[] BasicBlocks
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetLowLevelILBasicBlockList(
				    this.handle,
				    out ulong arrayLength
				);

			    return UnsafeUtils.TakeHandleArrayEx<LowLevelILBasicBlock>(
				    arrayPointer ,
				    arrayLength ,
				    (_native) => LowLevelILBasicBlock.MustNewFromHandleEx(this, _native) ,
				    NativeMethods.BNFreeBasicBlockList
			    );
		    }
	    }

	    public ulong CurrentAddress
	    {
		    get
		    {
			    return NativeMethods.BNLowLevelILGetCurrentAddress(this.handle);
		    }

		    set
		    {
			    this.SetCurrentAddress(value);
		    }
	    }

	    public void SetCurrentAddress(ulong address, Architecture? arch = null)
	    {
		    if (null == arch)
		    {
			    arch = this.Architecture;
		    }
		    
		    NativeMethods.BNLowLevelILSetCurrentAddress(
			    this.handle, 
			    null == arch ? IntPtr.Zero : arch.DangerousGetHandle(),
			    address
			);
	    }

	    public void SetCurrentSourceBlock(LowLevelILBasicBlock block)
	    {
		    NativeMethods.BNLowLevelILSetCurrentSourceBlock(
			    this.handle ,
			    block.DangerousGetHandle()
		    );
	    }
	    
	    public uint TemporaryRegisterCount
	    {
		    get
		    {
			    return NativeMethods.BNGetLowLevelILTemporaryRegisterCount(this.handle);
		    }
	    }

	    public uint TemporaryFlagCount
	    {
		    get
		    {
			    return NativeMethods.BNGetLowLevelILTemporaryFlagCount(this.handle);
		    }
	    }

	    public LowLevelILBasicBlock? GetBasicBlockForInstruction(LowLevelILInstructionIndex instruction)
	    {
		    return LowLevelILBasicBlock.TakeHandleEx(
			    this,
			    NativeMethods.BNGetLowLevelILBasicBlockForInstruction(this.handle , instruction)
		    );
	    }

	    public LowLevelILFunction SSAForm
	    {
		    get
		    {
			    if (this.IsSSAForm)
			    {
				    return this;
			    }
			    
			    return LowLevelILFunction.MustTakeHandle(
				    NativeMethods.BNGetLowLevelILSSAForm(this.handle),
				    true,
				    this.Architecture
			    );
		    }
	    }
	    
	    public LowLevelILFunction NonSSAForm
	    {
		    get
		    {
			    if (this.IsSSAForm)
			    {
				    return LowLevelILFunction.MustTakeHandle(
					    NativeMethods.BNGetLowLevelILNonSSAForm(this.handle),
					    false,
					    this.Architecture
				    );
			    }
			    else
			    {
				    return this;
			    }
		    }
	    }
	    
	    public MediumLevelILFunction? MediumLevelIL
	    {
		    get
		    {
			    return MediumLevelILFunction.TakeHandle(
				    NativeMethods.BNGetMediumLevelILForLowLevelIL(this.handle),
				    this.IsSSAForm
			    );
		    }
	    }
	    
	    public MediumLevelILFunction? MappedMediumLevelIL
	    {
		    get
		    {
			    return MediumLevelILFunction.TakeHandle(
				    NativeMethods.BNGetMappedMediumLevelIL(this.handle),
				    this.IsSSAForm
			    );
		    }
	    }

	    public MediumLevelILFunction CreateMediumLevelIL(Architecture? arch = null)
	    {
		    if (null == arch)
		    {
			    arch = this.Architecture;
		    }

		    using Function? sourceFunction = this.SourceFunction;

		    return MediumLevelILFunction.MustTakeHandle(

			    NativeMethods.BNCreateMediumLevelILFunction(
				    null == arch ? IntPtr.Zero : arch.DangerousGetHandle() ,
				    null == sourceFunction
					    ? IntPtr.Zero
					    : sourceFunction.DangerousGetHandle(),
				    this.handle
			    )
		    );
	    }

	    public BinaryView? SourceView
	    {
		    get
		    {
			    using Function? function = this.SourceFunction;

			    return null == function ? null : function.View;
		    }
	    }

	    public BinaryView View
	    {
		    get
		    {
			    BinaryView? view = this.SourceView;

			    if (null == view)
			    {
				    throw new InvalidOperationException(
					    "Standalone low-level IL functions do not have a binary view.");
			    }

			    return view;
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
	    /// Gets the native graph type. Use <see cref="ILForm"/> for parity with the official
	    /// bindings.
	    /// </summary>
	    public FunctionGraphType GraphType
	    {
		    get
		    {
			    return this.ILForm;
		    }
	    }
	    
	    public ILRegister[] Registers
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetLowLevelRegisters(
				    this.handle ,
				    out ulong arrayLength 
			    );

			    uint[] indexes = UnsafeUtils.TakeNumberArray<uint>(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeLLILVariablesList
			    );
			    
			    List<ILRegister> targets = new List<ILRegister>();

			    foreach (RegisterIndex index in indexes)
			    {
				    targets.Add( new ILRegister(this.Architecture, index) );
			    }
			    
			    return targets.ToArray();
		    }
	    }
	    
	    public LowLevelILSSARegister[] SSARegisters
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetLowLevelSSARegistersWithoutVersions(
				    this.handle ,
				    out ulong arrayLength 
			    );

			    uint[] indexes = UnsafeUtils.TakeNumberArray<uint>(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeLLILVariablesList
			    );
			    
			    List<LowLevelILSSARegister> targets = new List<LowLevelILSSARegister>();

			    foreach (RegisterIndex index in indexes)
			    {
				    ulong[] versions = this.GetRegisterSSAVersions(index);

				    foreach (ulong version in versions)
				    {
					    ILRegister register = new ILRegister(this.Architecture, index);
					    
					    targets.Add( 
						    new LowLevelILSSARegister(this, register, version)
						);
				    }
			    }
			    
			    return targets.ToArray();
		    }
	    }

	    public RegisterStack[] RegisterStacks
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetLowLevelRegisterStacks(
				    this.handle ,
				    out ulong arrayLength 
			    );

			    uint[] indexes = UnsafeUtils.TakeNumberArray<uint>(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeLLILVariablesList
			    );
			    
			    List<RegisterStack> targets = new List<RegisterStack>();

			    foreach (RegisterStackIndex index in indexes)
			    {
				    targets.Add( new RegisterStack(this.Architecture, index) );
			    }
			    
			    return targets.ToArray();
		    }
	    }
	    
	    public SSARegisterStack[] SSARegisterStacks
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetLowLevelSSARegisterStacksWithoutVersions(
				    this.handle ,
				    out ulong arrayLength 
			    );

			    uint[] indexes = UnsafeUtils.TakeNumberArray<uint>(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeLLILVariablesList
			    );
			    
			    List<SSARegisterStack> targets = new List<SSARegisterStack>();

			    foreach (RegisterStackIndex index in indexes)
			    {
				    ulong[] versions = this.GetRegisterStackSSAVersions(index);

				    foreach (ulong version in versions)
				    {
					    RegisterStack registerStack = new RegisterStack(this.Architecture, index);
					    
					    targets.Add( 
						    new SSARegisterStack(registerStack, version)
					    );
				    }
			    }
			    
			    return targets.ToArray();
		    }
	    }
	    
	    public ILFlag[] Flags
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetLowLevelFlags(
				    this.handle ,
				    out ulong arrayLength 
			    );

			    uint[] indexes = UnsafeUtils.TakeNumberArray<uint>(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeLLILVariablesList
			    );
			    
			    List<ILFlag> targets = new List<ILFlag>();

			    foreach (FlagIndex index in indexes)
			    {
				    targets.Add( new ILFlag(this.Architecture, index) );
			    }
			    
			    return targets.ToArray();
		    }
	    }
	    
	    public LowLevelILSSAFlag[] SSAFlags
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetLowLevelSSAFlagsWithoutVersions(
				    this.handle ,
				    out ulong arrayLength 
			    );

			    uint[] indexes = UnsafeUtils.TakeNumberArray<uint>(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeLLILVariablesList
			    );
			    
			    List<LowLevelILSSAFlag> targets = new List<LowLevelILSSAFlag>();

			    foreach (FlagIndex index in indexes)
			    {
				    ulong[] versions = this.GetFlagSSAVersions(index);

				    foreach (ulong version in versions)
				    {
					    ILFlag flag = new ILFlag(this.Architecture, index);
					    
					    targets.Add( 
						    new LowLevelILSSAFlag(this , flag, version)
					    );
				    }
			    }
			    
			    return targets.ToArray();
		    }
	    }
	 
	    public ulong[] GetRegisterSSAVersions(RegisterIndex register)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetLowLevelRegisterSSAVersions(
			    this.handle ,
			    register,
			    out ulong arrayLength 
		    );

		    return UnsafeUtils.TakeNumberArray<ulong>(
			    arrayPointer ,
			    arrayLength ,
			    NativeMethods.BNFreeLLILVariableVersionList
		    );
	    }
	    
	    public ulong[] GetRegisterStackSSAVersions(RegisterStackIndex registerStack)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetLowLevelRegisterStackSSAVersions(
			    this.handle ,
			    registerStack,
			    out ulong arrayLength 
		    );

		    return UnsafeUtils.TakeNumberArray<ulong>(
			    arrayPointer ,
			    arrayLength ,
			    NativeMethods.BNFreeLLILVariableVersionList
		    );
	    }
	    
	    public ulong[] GetFlagSSAVersions(FlagIndex flag)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetLowLevelFlagSSAVersions(
			    this.handle ,
			    flag,
			    out ulong arrayLength 
		    );

		    return UnsafeUtils.TakeNumberArray<ulong>(
			    arrayPointer ,
			    arrayLength ,
			    NativeMethods.BNFreeLLILVariableVersionList
		    );
	    }
	    
	    public ulong[] MemoryVersions
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetLowLevelMemoryVersions(
				    this.handle ,
				    out ulong arrayLength 
			    );

			    return UnsafeUtils.TakeNumberArray<ulong>(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeLLILVariableVersionList
			    );
		    }
	    }
	    
	    public LowLevelILInstruction? GetInstructionStart(ulong address, Architecture? arch = null)
	    {
		    if (null == arch)
		    {
			    arch = this.Architecture;
		    }
		    
		    LowLevelILInstructionIndex index = NativeMethods.BNLowLevelILGetInstructionStart(
			    this.handle ,
			    null == arch ? IntPtr.Zero :  arch.DangerousGetHandle(),
			    address
		    );

		    if ( (ulong)index >= this.InstructionCount)
		    {
			    return null;
		    }

		    return this.MustGetInstruction(index);
	    }
	    
	    public LowLevelILInstruction[] GetInstructionsAt(ulong address, Architecture? arch = null)
	    {
		    if (null == arch)
		    {
			    arch = this.Architecture;
		    }
		    
		    IntPtr arrayPointer = NativeMethods.BNLowLevelILGetInstructionsAt(
			    this.handle ,
			    null == arch ? IntPtr.Zero :  arch.DangerousGetHandle(),
			    address,
			    out ulong arrayLength 
		    );

		    LowLevelILInstructionIndex[] indexes =  UnsafeUtils.TakeNumberArray<LowLevelILInstructionIndex>(
			    arrayPointer ,
			    arrayLength ,
			    NativeMethods.BNFreeILInstructionList
		    );
		    
		    return this.MustGetInstructions(indexes);
	    }
	    
	    /// <summary>
	    /// LLIL exit instructions for the instruction at the given index (for example the
	    /// instructions a call may return to). Mirrors Python
	    /// <c>LowLevelILFunction.get_exits_for_instr</c> /
	    /// C++ <c>LowLevelILFunction::GetExitsForInstruction</c>.
	    /// </summary>
	    public LowLevelILInstruction[] GetExitsForInstruction(LowLevelILInstructionIndex instruction)
	    {
		    IntPtr arrayPointer = NativeMethods.BNLowLevelILGetExitsForInstruction(
			    this.handle ,
			    instruction ,
			    out ulong arrayLength
		    );

		    LowLevelILInstructionIndex[] indexes = UnsafeUtils.TakeNumberArray<LowLevelILInstructionIndex>(
			    arrayPointer ,
			    arrayLength ,
			    NativeMethods.BNFreeILInstructionList
		    );

		    return this.MustGetInstructions(indexes);
	    }

	    public LowLevelILInstruction? CurrentInstruction
	    {
		    get
		    {
			    return this.GetInstructionStart(this.CurrentAddress);
		    }
	    }
	  
	    public void ClearIndirectBranches()
	    {
		    NativeMethods.BNLowLevelILClearIndirectBranches(this.handle);
	    }

	    public void SetIndirectBranches(ArchitectureAndAddress[] branches)
	    {
		    NativeMethods.BNLowLevelILSetIndirectBranches(
			    this.handle ,
			    UnsafeUtils.ConvertToNativeArray<BNArchitectureAndAddress,ArchitectureAndAddress>(
				    branches
				),
			    (ulong)branches.Length
			);
	    }

	    public LowLevelILExpressionIndex CopyExpression(LowLevelILInstruction original)
	    {
		    return this.AddExpression(
			    original.Operation ,
			    new SourceLocation(original.Address , original.SourceOperand) ,
			    original.Size ,
			    original.Flags ,
			    original.RawOperands
		    );
	    }
	    
	    public void UpdateExpression(
		    LowLevelILInstructionIndex instruction,
		    OperandIndex operand,
		    ulong value
		    )
	    {
		    NativeMethods.BNUpdateLowLevelILOperand(
			    this.handle ,
			    instruction ,
			    operand,
			    value
		    );
	    }
	    
	    public void ReplaceExpression(LowLevelILExpressionIndex oldExpr , LowLevelILExpressionIndex newExpr)
	    {
		    NativeMethods.BNReplaceLowLevelILExpr(this.handle , oldExpr , newExpr);
	    }

	    public void ReplaceExpression(LowLevelILInstruction oldExpr , LowLevelILInstruction newExpr)
	    {
		    NativeMethods.BNReplaceLowLevelILExpr(
			    this.handle ,
			    oldExpr.ExpressionIndex ,
			    newExpr.ExpressionIndex
			);
	    }
	 
	    public void ReplaceExpression(LowLevelILExpressionIndex oldExpr , LowLevelILInstruction newExpr)
	    {
		    NativeMethods.BNReplaceLowLevelILExpr(
			    this.handle ,
			    oldExpr ,
			    newExpr.ExpressionIndex
		    );
	    }
	    
	    public void ReplaceExpression(LowLevelILInstruction oldExpr , LowLevelILExpressionIndex newExpr)
	    {
		    NativeMethods.BNReplaceLowLevelILExpr(
			    this.handle ,
			    oldExpr.ExpressionIndex ,
			    newExpr
		    );
	    }
	    
	    
	    
	    public void SetExpressionAttributes(LowLevelILExpressionIndex expression , uint attributes )
	    {
		    NativeMethods.BNSetLowLevelILExprAttributes(
			    this.handle,
			    expression,
			    attributes 
			);
	    }
	    
	    public LowLevelILInstructionIndex AddInstruction(LowLevelILExpressionIndex expression )
	    {
		    return NativeMethods.BNLowLevelILAddInstruction(
			    this.handle,
			    expression
		    );
	    }

	    public void MarkLabel(LowLevelILLabel label)
	    {
		    NativeMethods.BNLowLevelILMarkLabel(this.handle, label.ToNative());
	    }
	    
	    public LowLevelILExpressionIndex AddLabelMap(IDictionary<ulong, LowLevelILLabel> labelMap)
	    {
		    return NativeMethods.BNLowLevelILAddLabelMap(
			    this.handle ,
			    labelMap.Keys.ToArray(),
			    UnsafeUtils.ConvertToNativeArray<BNLowLevelILLabel,LowLevelILLabel>(
				    labelMap.Values.ToArray()
			    ) ,
			    (ulong)labelMap.Count
		    );
	    }

	    public LowLevelILExpressionIndex AddOperandList(ulong[] operands)
	    {
		    return NativeMethods.BNLowLevelILAddOperandList(
			    this.handle ,
			    operands ,
			    (ulong)operands.Length
		    );
	    }
	    
	    public LowLevelILExpressionIndex AddOperandList(LowLevelILExpressionIndex[] operands)
	    {
		    List<ulong> targets = new List<ulong>();

		    foreach (LowLevelILExpressionIndex operand in operands)
		    {
			    targets.Add( (ulong)operand);
		    }
		    
		    return NativeMethods.BNLowLevelILAddOperandList(
			    this.handle ,
			    targets.ToArray() ,
			    (ulong)targets.Count
		    );
	    }
	    
	    public LowLevelILPossibleValueSetCacheIndex CachePossibleValueSet(PossibleValueSet pvs)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    return NativeMethods.BNCacheLowLevelILPossibleValueSet(
				    this.handle , 
				    pvs.ToNativeEx(allocator)
			    );
		    }
	    }

	    public PossibleValueSet GetCachedPossibleValueSet(LowLevelILPossibleValueSetCacheIndex index)
	    {
		    return PossibleValueSet.TakeNative(
			    NativeMethods.BNGetCachedLowLevelILPossibleValueSet(
				    this.DangerousGetHandle() ,
				    index
			    )
		    );
	    }

	    public void SetExpressionSourceOperand(LowLevelILExpressionIndex expression , uint operand)
	    {
		    NativeMethods.BNLowLevelILSetExprSourceOperand(
			    this.handle,
			    expression,
			    operand
			);
	    }

	    public void FinalizeLowLevelILFunction()
	    {
		    NativeMethods.BNFinalizeLowLevelILFunction(this.handle);
	    }

	    public void GenerateSSAForm()
	    {
		    NativeMethods.BNGenerateLowLevelILSSAForm(this.handle);
	    }

	    public InstructionTextToken[] GetExpressionText(
		    LowLevelILExpressionIndex expression,
		    Architecture? arch = null,
		    DisassemblySettings? settings = null
		)
	    {
		    if (null == arch)
		    {
			    arch = this.Architecture;
		    }

		    NativeMethods.BNGetLowLevelILExprText(
			    this.handle,
			    null == arch ? IntPtr.Zero :  arch.DangerousGetHandle(),
			    expression,
			    null == settings ? IntPtr.Zero :  settings.DangerousGetHandle(),
			    out IntPtr arrayPointer,
			    out ulong arrayLength
			);

		    return UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken , InstructionTextToken>(
			    arrayPointer ,
			    arrayLength ,
			    InstructionTextToken.FromNative ,
			    NativeMethods.BNFreeInstructionText
		    );
	    }

	    public InstructionTextToken[] GetInstructionText(
		    LowLevelILInstructionIndex instruction,
		    Architecture? arch = null,
		    DisassemblySettings? settings = null
	    )
	    {
		    if (null == arch)
		    {
			    arch = this.Architecture;
		    }

		    using Function? sourceFunction = this.SourceFunction;

		    NativeMethods.BNGetLowLevelILInstructionText(
			    this.handle,
			    null == sourceFunction
				    ? IntPtr.Zero
				    : sourceFunction.DangerousGetHandle(),
			    null == arch ? IntPtr.Zero :  arch.DangerousGetHandle(),
			    instruction,
			    null == settings ? IntPtr.Zero :  settings.DangerousGetHandle(),
			    out IntPtr arrayPointer,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken , InstructionTextToken>(
			    arrayPointer ,
			    arrayLength ,
			    InstructionTextToken.FromNative ,
			    NativeMethods.BNFreeInstructionText
		    );
	    }
	    
	    public void PrepareToCopyFunction(LowLevelILFunction il)
	    {
		    NativeMethods.BNPrepareToCopyLowLevelILFunction(
			    this.handle,
			    il.DangerousGetHandle()
			);
	    }
	    
	    public void PrepareToCopyBasicBlock(LowLevelILBasicBlock basicBlock)
	    {
		    NativeMethods.BNPrepareToCopyLowLevelILBasicBlock(
			    this.handle,
			    basicBlock.DangerousGetHandle()
		    );
	    }

	    public LowLevelILLabel? GetLabelForSourceInstruction(LowLevelILInstructionIndex instr)
	    {
		    return LowLevelILLabel.FromNativePointer(
			    NativeMethods.BNGetLabelForLowLevelILSourceInstruction(
				    this.DangerousGetHandle() ,
				    instr
			    )
		    );
	    }

	    public void AddLabelForAddress(ulong address , Architecture? arch = null)
	    {
		    if (null == arch)
		    {
			    arch = this.Architecture;
		    }

		    NativeMethods.BNAddLowLevelILLabelForAddress(
			    this.DangerousGetHandle() ,
			    null == arch ? IntPtr.Zero : arch.DangerousGetHandle() ,
			    address
		    );
	    }
	    
	    public LowLevelILLabel? GetLabelForAddress(ulong address , Architecture? arch = null)
	    {
		    if (null == arch)
		    {
			    arch = this.Architecture;
		    }
		    
		    return LowLevelILLabel.FromNativePointer(

			    NativeMethods.BNGetLowLevelILLabelForAddress(
				    this.DangerousGetHandle() ,
				    null == arch ? IntPtr.Zero : arch.DangerousGetHandle() ,
				    address
			    )
		    );
	    }
	    
	    public LowLevelILInstructionIndex GetSSAInstructionIndex(LowLevelILInstructionIndex instr )
	    {
		    return NativeMethods.BNGetLowLevelILSSAInstructionIndex(
			    this.handle ,
			    instr
		    );
	    }
	    
	    public LowLevelILInstructionIndex GetNonSSAInstructionIndex(LowLevelILInstructionIndex instr )
	    {
		    return NativeMethods.BNGetLowLevelILNonSSAInstructionIndex(
			    this.handle ,
			    instr
		    );
	    }
	    
	    public LowLevelILExpressionIndex GetSSAExpressionIndex(LowLevelILExpressionIndex expression )
	    {
		    return NativeMethods.BNGetLowLevelILSSAExprIndex(
			    this.handle ,
			    expression
		    );
	    }
	    
	    public LowLevelILExpressionIndex GetNonSSAExpressionIndex(LowLevelILExpressionIndex expression )
	    {
		    return NativeMethods.BNGetLowLevelILNonSSAExprIndex(
			    this.handle ,
			    expression
		    );
	    }
	    
	    public LowLevelILInstruction? GetSSARegisterDefinition(RegisterIndex register , ulong vesion )
	    {
		    LowLevelILInstructionIndex index = NativeMethods.BNGetLowLevelILSSARegisterDefinition(
			    this.handle ,
			    register ,
			    vesion
		    );

		    if ((ulong)index >= this.InstructionCount)
		    {
			    return null;
		    }

		    return this.MustGetInstruction(index);
	    }
	    
	    public LowLevelILInstruction? GetSSAFlagDefinition(FlagIndex flag , ulong vesion )
	    {
		    LowLevelILInstructionIndex index = NativeMethods.BNGetLowLevelILSSAFlagDefinition(
			    this.handle ,
			    flag ,
			    vesion
		    );

		    if ((ulong)index >= this.InstructionCount)
		    {
			    return null;
		    }

		    return this.MustGetInstruction(index);
	    }
	    
	    public LowLevelILInstruction? GetSSAMemoryDefinition(ulong vesion )
	    {
		    LowLevelILInstructionIndex index = NativeMethods.BNGetLowLevelILSSAMemoryDefinition(
			    this.handle ,
			    vesion
		    );

		    if ((ulong)index >= this.InstructionCount)
		    {
			    return null;
		    }

		    return this.MustGetInstruction(index);
	    }
	    
	    public LowLevelILInstruction[] GetSSARegisterUses(RegisterIndex register , ulong vesion )
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetLowLevelILSSARegisterUses(
			    this.handle ,
			    register ,
			    vesion,
			    out ulong arrayLength
		    );

		    if (0 == arrayLength)
		    {
			    return Array.Empty<LowLevelILInstruction>();
		    }
		    
		    ulong[] indices = UnsafeUtils.TakeNumberArray<ulong>(
			    arrayPointer ,
			    arrayLength ,
			    NativeMethods.BNFreeILInstructionList
		    );
		    
		    List<LowLevelILInstruction> instructions = new List<LowLevelILInstruction>();

		    foreach (LowLevelILInstructionIndex index in indices)
		    {
			    instructions.Add( this.MustGetInstruction( index ) );
		    }
		    
		    return instructions.ToArray();
	    }
	    
	    public LowLevelILInstruction[] GetSSAFlagUses(FlagIndex flag , ulong vesion )
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetLowLevelILSSAFlagUses(
			    this.handle ,
			    flag ,
			    vesion,
			    out ulong arrayLength
		    );

		    if (0 == arrayLength)
		    {
			    return Array.Empty<LowLevelILInstruction>();
		    }
		    
		    ulong[] indices = UnsafeUtils.TakeNumberArray<ulong>(
			    arrayPointer ,
			    arrayLength ,
			    NativeMethods.BNFreeILInstructionList
		    );
		    
		    List<LowLevelILInstruction> instructions = new List<LowLevelILInstruction>();

		    foreach (LowLevelILInstructionIndex index in indices)
		    {
			    instructions.Add( this.MustGetInstruction( index ) );
		    }
		    
		    return instructions.ToArray();
	    }
	    
	    public LowLevelILInstruction[] GetSSAMemoryUses(ulong vesion )
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetLowLevelILSSAMemoryUses(
			    this.handle ,
			    vesion,
			    out ulong arrayLength
		    );

		    if (0 == arrayLength)
		    {
			    return Array.Empty<LowLevelILInstruction>();
		    }
		    
		    ulong[] indices = UnsafeUtils.TakeNumberArray<ulong>(
			    arrayPointer ,
			    arrayLength ,
			    NativeMethods.BNFreeILInstructionList
		    );
		    
		    List<LowLevelILInstruction> instructions = new List<LowLevelILInstruction>();

		    foreach (LowLevelILInstructionIndex index in indices)
		    {
			    instructions.Add( this.MustGetInstruction( index ) );
		    }
		    
		    return instructions.ToArray();
	    }
	    
	    public RegisterValue GetSSARegisterValue(RegisterIndex register , ulong vesion)
	    {
		    return RegisterValue.FromNative(
			    NativeMethods.BNGetLowLevelILSSARegisterValue(
				    this.handle , 
				    register,
				    vesion
			    )
		    );
	    }
	    
	    public RegisterValue GetSSAFlagValue(FlagIndex flag , ulong vesion)
	    {
		    return RegisterValue.FromNative(
			    NativeMethods.BNGetLowLevelILSSAFlagValue(
				    this.handle , 
				    flag,
				    vesion
			    )
		    );
	    }
	    
	    public RegisterValue GetExpressionValue(LowLevelILExpressionIndex expressionIndex)
	    {
		    return RegisterValue.FromNative(
			    NativeMethods.BNGetLowLevelILExprValue(this.handle , expressionIndex)
			);
	    }
	    
	    public RegisterValue GetRegisterValueAtInstruction(
		    RegisterIndex register,
		    LowLevelILInstructionIndex instruction
		)
	    {
		    return RegisterValue.FromNative(
			    NativeMethods.BNGetLowLevelILRegisterValueAtInstruction(
				    this.handle , 
				    register,
				    instruction
				)
		    );
	    }
	    
	    [Obsolete("Use GetRegisterValueAfterInstruction.")]
	    public RegisterValue GetRegisterValueAfteInstruction(
		    uint registerIndex,
		    LowLevelILInstructionIndex instruction
	    )
	    {
		    return this.GetRegisterValueAfterInstruction(
			    (RegisterIndex)registerIndex,
			    instruction);
	    }
	    
	    public LowLevelILInstructionIndex? GetInstructionIndexForExpressionIndex(LowLevelILExpressionIndex expressionIndex)
	    {
		    LowLevelILInstructionIndex index = NativeMethods.BNGetLowLevelILInstructionForExpr(this.handle , expressionIndex);

		    if ((ulong)index >= this.InstructionCount)
		    {
			    return null;
		    }
		    
		    return index;
	    }
	    
	    public LowLevelILExpressionIndex? GetExpressionIndexForInstructionIndex(LowLevelILInstructionIndex instructionIndex)
	    {
		    LowLevelILExpressionIndex index = NativeMethods.BNGetLowLevelILIndexForInstruction(this.handle , instructionIndex);

		    if ((ulong)index >= this.ExpressionCount)
		    {
			    return null;
		    }
		    
		    return index;
	    }
	    
	    // medium
	    public MediumLevelILInstruction? GetMediumLevelILExpression(
		    LowLevelILExpressionIndex lowExpr
	    )
	    {
		    MediumLevelILFunction? mlil = this.MediumLevelIL;

		    if (null == mlil)
		    {
			    return null;
		    }

		    MediumLevelILExpressionIndex mediumExpr = NativeMethods.BNGetMediumLevelILExprIndex(
			    this.handle ,
			    lowExpr
		    );

		    if ((ulong)mediumExpr >= mlil.ExpressionCount)
		    {
			    return null;
		    }
		    
		    return mlil.GetExpression(mediumExpr);
	    }
	    
	    public MediumLevelILInstruction? GetMediumLevelILInstruction(
		    LowLevelILInstructionIndex lowInstr
		)
	    {
		    MediumLevelILFunction? mlil = this.MediumLevelIL;

		    if (null == mlil)
		    {
			    return null;
		    }

		    MediumLevelILInstructionIndex mediumInstr = NativeMethods.BNGetMediumLevelILInstructionIndex(
			    this.handle ,
			    lowInstr
		    );

		    if ((ulong)mediumInstr >= mlil.InstructionCount)
		    {
			    return null;
		    }
		    
		    return mlil.GetInstruction(mediumInstr);
	    }

	    public MediumLevelILInstruction[] GetMediumLevelILExpressions(
		    LowLevelILExpressionIndex lowExpr)
	    {
		    MediumLevelILFunction? mlil = this.MediumLevelIL;

		    if (null == mlil)
		    {
			    return Array.Empty<MediumLevelILInstruction>();
		    }
		    
		    IntPtr arrayPointer = NativeMethods.BNGetMediumLevelILExprIndexes(
			    this.handle , 
			    lowExpr,
			    out ulong arrayLength
			);
	
		    MediumLevelILExpressionIndex[] mediumExprs = UnsafeUtils.TakeNumberArray<MediumLevelILExpressionIndex>(
			    arrayPointer ,
			    arrayLength ,
			    NativeMethods.BNFreeILInstructionList
		    );

		    return mlil.MustGetExpressions(mediumExprs);
	    }

	    // mapped
	    public MediumLevelILInstruction? GetMappedMediumLevelILExpression(
		    LowLevelILExpressionIndex lowExpr
	    )
	    {
		    MediumLevelILFunction? mlil = this.MappedMediumLevelIL;

		    if (null == mlil)
		    {
			    return null;
		    }

		    MediumLevelILExpressionIndex mediumExpr = NativeMethods.BNGetMappedMediumLevelILExprIndex(
			    this.handle ,
			    lowExpr
		    );

		    if ((ulong)mediumExpr >= mlil.ExpressionCount)
		    {
			    return null;
		    }
		    
		    return mlil.GetExpression(mediumExpr);
	    }
	    
	    public MediumLevelILInstruction? GetMappedMediumLevelILInstruction(
		    LowLevelILInstructionIndex lowInstr
		)
	    {
		    MediumLevelILFunction? mlil = this.MappedMediumLevelIL;

		    if (null == mlil)
		    {
			    return null;
		    }

		    MediumLevelILInstructionIndex mediumInstr = NativeMethods.BNGetMappedMediumLevelILInstructionIndex(
			    this.handle ,
			    lowInstr
		    );

		    if ((ulong)mediumInstr >= mlil.InstructionCount)
		    {
			    return null;
		    }
		    
		    return mlil.GetInstruction(mediumInstr);
	    }

	    // high
	    public HighLevelILInstruction? GetHighLevelILExpression(
		    LowLevelILExpressionIndex lowExpr
		)
	    {
		    MediumLevelILFunction? mlil = this.MediumLevelIL;

		    if (null == mlil)
		    {
			    return null;
		    }
		    
		    MediumLevelILExpressionIndex mediumExpr = NativeMethods.BNGetMediumLevelILExprIndex(
			    this.handle , 
			    lowExpr
			);

		    if ((ulong)mediumExpr >= mlil.ExpressionCount)
		    {
			    return null;
		    }

		    return mlil.GetHighLevelILExpression(mediumExpr);
	    }

	    public HighLevelILInstruction[] GetHighLevelILExpressions(
		    LowLevelILExpressionIndex lowExpr
	    )
	    {
		    MediumLevelILFunction? mlil = this.MediumLevelIL;

		    if (null == mlil)
		    {
			    return Array.Empty<HighLevelILInstruction>();
		    }
		    
		    HashSet<HighLevelILInstruction> highExprs  = new HashSet<HighLevelILInstruction>();

		    MediumLevelILInstruction[] mediumExprs = this.GetMediumLevelILExpressions(lowExpr);

		    foreach (MediumLevelILInstruction mediumExpr in mediumExprs)
		    {
			    foreach (HighLevelILInstruction highExpr in mediumExpr.HighLevelILExpressions)
			    {
				    highExprs.Add(highExpr);
			    }
		    }

		    return highExprs.ToArray();
	    }
	    
	    public HighLevelILInstruction? GetHighLevelILInstruction(
		    LowLevelILInstructionIndex lowInst
	    )
	    {
		    MediumLevelILFunction? mlil = this.MediumLevelIL;

		    if (null == mlil)
		    {
			    return null;
		    }
		    
		    MediumLevelILInstructionIndex mediumInst = NativeMethods.BNGetMediumLevelILInstructionIndex(
			    this.handle , 
			    lowInst
		    );

		    if ((ulong)mediumInst >= mlil.InstructionCount)
		    {
			    return null;
		    }

		    return mlil.GetHighLevelILInstruction(mediumInst);
	    }
	    
	    public LowLevelILFlowGraph? CreateGraph(DisassemblySettings? settings = null)
	    {
		    return LowLevelILFlowGraph.TakeHandleEx(
			    this,
			    NativeMethods.BNCreateLowLevelILFunctionGraph(
				    this.handle ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public LowLevelILFlowGraph? CreateImmediateGraph(DisassemblySettings? settings = null)
	    {
		    return LowLevelILFlowGraph.TakeHandleEx(
			    this,
			    NativeMethods.BNCreateLowLevelILImmediateFunctionGraph(
				    this.handle ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    
	    public LowLevelILExpressionIndex AddExpression(
		    LowLevelILOperation operation ,
		    SourceLocation? location = null,
		    ulong size = 0 ,
		    uint flags = 0,
		    params ulong[] operands
		)
	    {
		    ulong a = 0;
		    ulong b = 0;
		    ulong c = 0;
		    ulong d = 0;
	
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
		    
		    if (null == location)
		    {
			    return NativeMethods.BNLowLevelILAddExpr(
				    this.handle ,
				    operation,
				    size ,
				    flags,
				    a,
				    b ,
				    c ,
				    d 
			    );
		    }
		    else
		    {
			    return NativeMethods.BNLowLevelILAddExprWithLocation(
				    this.handle ,
				    location.Address,
				    location.Operand,
				    operation,
				    size ,
				    flags,
				    a,
				    b ,
				    c ,
				    d 
			    );
		    }
	    }

	    public LowLevelILExpressionIndex EmitNop(SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_NOP ,
			    location
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitSetRegister(
		    ulong size ,
		    RegisterIndex register,
		    LowLevelILExpressionIndex value ,
		    FlagIndex? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_SET_REG ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag,
			    (ulong)register,
			    (ulong)value
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitSetRegisterSplit(
		    ulong size ,
		    RegisterIndex high,
		    RegisterIndex low,
		    LowLevelILExpressionIndex value ,
		    FlagIndex? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_SET_REG_SPLIT ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag,
			    (ulong)high,
			    (ulong)low,
			    (ulong)value
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitSetRegisterStackRelative(
		    ulong size ,
		    RegisterStackIndex registerStack,
		    LowLevelILExpressionIndex entry ,
		    LowLevelILExpressionIndex value ,
		    FlagIndex? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_SET_REG_STACK_REL ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag,
			    (ulong)registerStack,
			    (ulong)entry,
			    (ulong)value
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitSetRegisterStackPush(
		    ulong size ,
		    RegisterStackIndex registerStack,
		    LowLevelILExpressionIndex value ,
		    FlagIndex? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_REG_STACK_PUSH ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag,
			    (ulong)registerStack,
			    (ulong)value
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitSetFlag(
		    ulong size ,
		    FlagIndex flag,
		    LowLevelILExpressionIndex value ,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_SET_FLAG ,
			    location,
			    size,
			    0,
			    (ulong)flag,
			    (ulong)value
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitAssert(
		    ulong size ,
		    RegisterIndex source,
		    PossibleValueSet constraint,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_ASSERT ,
			    location,
			    size,
			    0,
			    (ulong)source,
			    (ulong)this.CachePossibleValueSet(constraint)
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitForceVersion(
		    ulong size ,
		    RegisterIndex destination,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FORCE_VER ,
			    location,
			    size,
			    (uint)0,
			    (ulong)destination
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitLoad(
		    ulong size ,
		    LowLevelILExpressionIndex address,
		    FlagIndex? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_LOAD ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag,
			    (ulong)address
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitStore(
		    ulong size ,
		    LowLevelILExpressionIndex address,
		    LowLevelILExpressionIndex value,
		    FlagIndex? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_STORE ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag,
			    (ulong)address,
			    (ulong)value
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitPush(
		    ulong size ,
		    LowLevelILExpressionIndex value,
		    FlagIndex? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_PUSH ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag,
			    (ulong)value
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitPop(
		    ulong size ,
		    FlagIndex? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_POP ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitRegister(
		    ulong size ,
		    RegisterIndex register,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_REG ,
			    location,
			    size,
			    0,
			    (ulong)register
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitRegisterSplit(
		    ulong size ,
		    RegisterIndex high,
		    RegisterIndex low,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_REG_SPLIT ,
			    location,
			    size,
			    0,
			    (ulong)high,
			    (ulong)low
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitRegisterStackRelative(
		    ulong size ,
		    RegisterStackIndex registerStack,
		    LowLevelILInstructionIndex entry,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_REG_STACK_REL ,
			    location,
			    size,
			    0,
			    (ulong)registerStack,
			    (ulong)entry
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitRegisterStackPop(
		    ulong size ,
		    RegisterStackIndex registerStack,
		    FlagIndex? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_REG_STACK_POP ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag,
			    (ulong)registerStack
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitRegisterStackFreeRegister(
		    ulong size ,
		    RegisterIndex register,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_REG_STACK_FREE_REG ,
			    location,
			    size,
			    0,
			    (ulong)register
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitRegisterStackFreeRelative(
		    ulong size ,
		    RegisterStackIndex registerStack,
		    LowLevelILInstructionIndex entry,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_REG_STACK_FREE_REL ,
			    location,
			    size,
			    0,
			    (ulong)registerStack,
			    (ulong)entry
		    );
	    }
	    
	    
	    public LowLevelILExpressionIndex EmitConst(
		    ulong size ,
		    ulong value,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_CONST ,
			    location,
			    size,
			    0,
			    value
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitConstPointer(
		    ulong size ,
		    ulong value,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_CONST_PTR ,
			    location,
			    size,
			    0,
			    value
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitExternPointer(
		    ulong size ,
		    ulong value,
		    ulong offset,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_EXTERN_PTR ,
			    location,
			    size,
			    0,
			    value,
			    offset
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitRawConstFloat(
		    ulong size ,
		    ulong value,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FLOAT_CONST ,
			    location,
			    size,
			    0,
			    value
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitConstFloat(
		    ulong size ,
		    float value,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FLOAT_CONST ,
			    location,
			    size,
			    0,
			    (ulong)BitConverter.SingleToUInt32Bits(value)
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitConstDouble(
		    ulong size ,
		    double value,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FLOAT_CONST ,
			    location,
			    size,
			    0,
			    (ulong)BitConverter.DoubleToUInt64Bits(value)
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitFlag(
		    ulong size ,
		    FlagIndex flag,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FLAG ,
			    location,
			    size,
			    0,
			    (ulong)flag
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitFlagBit(
		    ulong size ,
		    FlagIndex flag,
		    ulong bit,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FLAG_BIT ,
			    location,
			    size,
			    0,
			    (ulong)flag,
			    bit
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitAdd(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_ADD ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitAddCarray(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    LowLevelILExpressionIndex carray,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_ADC ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b,
			    (ulong)carray
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitSub(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_SUB ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitSubBorrow(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    LowLevelILExpressionIndex carray,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_SBB ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b,
			    (ulong)carray
		    );
	    }
	    
	    
	    public LowLevelILExpressionIndex EmitAnd(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_AND ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitOr(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_OR ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitXor(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_XOR ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitLogicalShiftLeft(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_LSL ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitLogicalShiftRight(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_LSR ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitArithmeticShiftRight(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_ASR ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitRotateLeft(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_ROL ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitRotateLeftCarray(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    LowLevelILExpressionIndex carray,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_RLC ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b,
			    (ulong)carray
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitRotateRight(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_ROR ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitRotateRightCarray(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    LowLevelILExpressionIndex carray,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_RRC ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b,
			    (ulong)carray
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitMul(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_MUL ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitMulSignedDoublePrecision(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_MULS_DP ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitMulUnsignedDoublePrecision(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_MULU_DP ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitDivSigned(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_DIVS ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitDivSignedDoublePrecision(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_DIVS_DP ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitDivUnsigned(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_DIVU ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitDivUnsignedDoublePrecision(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_DIVU_DP ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b
		    );
	    }
	    
	   
	    
	    public LowLevelILExpressionIndex EmitModSigned(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_MODS ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitModSignedDoublePrecision(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_MODS_DP ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitModUnsigned(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_MODU ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitModUnsignedDoublePrecision(
		    ulong size ,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex b,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_MODU_DP ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)a,
			    (ulong)b
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitNeg(
		    ulong size ,
		    LowLevelILExpressionIndex value,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_NEG ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)value
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitNot(
		    ulong size ,
		    LowLevelILExpressionIndex value,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_NOT ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)value
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitSignExtend(
		    ulong size ,
		    LowLevelILExpressionIndex value,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_SX ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)value
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitZeroExtend(
		    ulong size ,
		    LowLevelILExpressionIndex value,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_ZX ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)value
		    );
	    }

	    public LowLevelILExpressionIndex EmitLowPart(
		    ulong size ,
		    LowLevelILExpressionIndex value,
		    ILFlag? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_LOW_PART ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag.Index,
			    (ulong)value
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitJump(
		    LowLevelILExpressionIndex dest,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_JUMP ,
			    location,
			    0,
			   0,
			    (ulong)dest
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitJumpTo(
		    LowLevelILExpressionIndex dest,
		    IDictionary<ulong,LowLevelILLabel> targets,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_JUMP_TO ,
			    location,
			    0,
			    0,
			    (ulong)dest,
			    (ulong) targets.Count * 2,
			    (ulong)this.AddLabelMap(targets)
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitCall(
		    LowLevelILExpressionIndex dest,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_CALL ,
			    location,
			    0,
			    0,
			    (ulong)dest
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitCallStackAdjust(
		    LowLevelILExpressionIndex dest,
		    long stackAdjust,
		    IDictionary<RegisterStackIndex,long>? registerStackAdjustments = null,
		    SourceLocation? location = null)
	    {
		    List<ulong> pairs = new List<ulong>();

		    if (null != registerStackAdjustments)
		    {
			    foreach (KeyValuePair<RegisterStackIndex , long> item in registerStackAdjustments)
			    {
				    pairs.Add( (ulong)item.Key );
				    pairs.Add( (ulong)item.Value );
			    }
		    }
		    
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_CALL_STACK_ADJUST ,
			    location,
			    0,
			    0,
			    (ulong)dest,
			    (ulong)stackAdjust,
			    (ulong)pairs.Count,
			    (ulong)this.AddOperandList(pairs.ToArray())
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitTailCall(
		    LowLevelILExpressionIndex dest,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_TAILCALL ,
			    location,
			    0,
			    0,
			    (ulong)dest
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitRet(
		    LowLevelILExpressionIndex dest,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_RET ,
			    location,
			    0,
			    0,
			    (ulong)dest
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitNoRet(
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_NORET ,
			    location,
			    0,
			    0
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitFlagCondition(
		    LowLevelILFlagCondition condition,
		    SemanticFlagClassIndex semanticFlagClass = 0,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FLAG_COND ,
			    location,
			    0,
			    0,
			    (ulong)condition,
			    (ulong)semanticFlagClass
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitFlagGroup(
		    SemanticFlagGroupIndex semanticFlagGroup,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FLAG_GROUP ,
			    location,
			    0,
			    0,
			    (ulong)semanticFlagGroup
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitEqual(
		    ulong size,
		    LowLevelILExpressionIndex left,
		    LowLevelILExpressionIndex right,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_CMP_E ,
			    location,
			    size,
			    0,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitNotEqual(
		    ulong size,
		    LowLevelILExpressionIndex left,
		    LowLevelILExpressionIndex right,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_CMP_NE ,
			    location,
			    size,
			    0,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitSignedLessThan(
		    ulong size,
		    LowLevelILExpressionIndex left,
		    LowLevelILExpressionIndex right,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_CMP_SLT ,
			    location,
			    size,
			    0,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitUnsignedLessThan(
		    ulong size,
		    LowLevelILExpressionIndex left,
		    LowLevelILExpressionIndex right,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_CMP_ULT ,
			    location,
			    size,
			    0,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitSignedLessEqual(
		    ulong size,
		    LowLevelILExpressionIndex left,
		    LowLevelILExpressionIndex right,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_CMP_SLE ,
			    location,
			    size,
			    0,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitUnsignedLessEqual(
		    ulong size,
		    LowLevelILExpressionIndex left,
		    LowLevelILExpressionIndex right,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_CMP_ULE ,
			    location,
			    size,
			    0,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitSignedGreaterEqual(
		    ulong size,
		    LowLevelILExpressionIndex left,
		    LowLevelILExpressionIndex right,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_CMP_SGE ,
			    location,
			    size,
			    0,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitUnsignedGreaterEqual(
		    ulong size,
		    LowLevelILExpressionIndex left,
		    LowLevelILExpressionIndex right,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_CMP_UGE ,
			    location,
			    size,
			    0,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitSignedGreaterThan(
		    ulong size,
		    LowLevelILExpressionIndex left,
		    LowLevelILExpressionIndex right,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_CMP_SGT ,
			    location,
			    size,
			    0,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitUnsignedGreaterThan(
		    ulong size,
		    LowLevelILExpressionIndex left,
		    LowLevelILExpressionIndex right,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_CMP_UGT ,
			    location,
			    size,
			    0,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitTestBit(
		    ulong size,
		    LowLevelILExpressionIndex a,
		    LowLevelILExpressionIndex bit,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_TEST_BIT ,
			    location,
			    size,
			    0,
			    (ulong)a,
			    (ulong)bit
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitBoolToInt(
		    ulong size,
		    LowLevelILExpressionIndex operand,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_BOOL_TO_INT ,
			    location,
			    size,
			    0,
			    (ulong)operand
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitSysCall(
		    ulong size,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_SYSCALL ,
			    location,
			    size,
			    0
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitIntrinsic(
		    ulong size,
		    ulong[] outputFlagOrRegisters,
		    IntrinsicIndex index,
		    LowLevelILExpressionIndex[] parameters,
		    FlagIndex? flag = null,
		    SourceLocation? location = null)
	    {
		    LowLevelILExpressionIndex callParam = this.AddExpression(
			    LowLevelILOperation.LLIL_CALL_PARAM ,
			    location,
			    size,
			    0,
			    (ulong)parameters.Length,
			    (ulong)this.AddOperandList(parameters)
		    );
		    
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_INTRINSIC ,
			    location,
			    size,
			    null == flag ? 0 : (uint)flag,
			    (ulong)outputFlagOrRegisters.Length,
			    (ulong)this.AddOperandList(outputFlagOrRegisters),
			    (ulong)index,
			    (ulong)callParam
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitBreakPoint(
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_BP ,
			    location,
			    0,
			    0
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitTrap(
		    ulong value,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_TRAP ,
			    location,
			    0,
			    0,
			    value
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitUndefined(
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_UNDEF ,
			    location,
			    0,
			    0
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitUnimplemented(
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_UNIMPL ,
			    location,
			    0,
			    0
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitUnimplementedMemeory(
		    ulong size,
		    LowLevelILExpressionIndex address,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_UNIMPL_MEM ,
			    location,
			    size,
			    0,
			    (ulong)address
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitFloatAdd(
		    ulong size,
		    LowLevelILExpressionIndex left,
		    LowLevelILExpressionIndex right,
		    FlagIndex? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FADD ,
			    location,
			    size,
			    null == flag  ? 0 : (uint)flag,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitFloatSub(
		    ulong size,
		    LowLevelILExpressionIndex left,
		    LowLevelILExpressionIndex right,
		    FlagIndex? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FSUB ,
			    location,
			    size,
			    null == flag  ? 0 : (uint)flag,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitFloatMul(
		    ulong size,
		    LowLevelILExpressionIndex left,
		    LowLevelILExpressionIndex right,
		    FlagIndex? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FMUL ,
			    location,
			    size,
			    null == flag  ? 0 : (uint)flag,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitFloatDiv(
		    ulong size,
		    LowLevelILExpressionIndex left,
		    LowLevelILExpressionIndex right,
		    FlagIndex? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FDIV ,
			    location,
			    size,
			    null == flag  ? 0 : (uint)flag,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitFloatSquareRoot(
		    ulong size,
		    LowLevelILExpressionIndex operand,
		    FlagIndex? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FSQRT ,
			    location,
			    size,
			    null == flag  ? 0 : (uint)flag,
			    (ulong)operand
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitFloatNeg(
		    ulong size,
		    LowLevelILExpressionIndex operand,
		    FlagIndex? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FNEG ,
			    location,
			    size,
			    null == flag  ? 0 : (uint)flag,
			    (ulong)operand
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitFloatAbs(
		    ulong size,
		    LowLevelILExpressionIndex operand,
		    FlagIndex? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FABS ,
			    location,
			    size,
			    null == flag  ? 0 : (uint)flag,
			    (ulong)operand
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitFloatToInt(
		    ulong size,
		    LowLevelILExpressionIndex operand,
		    FlagIndex? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FLOAT_TO_INT ,
			    location,
			    size,
			    null == flag  ? 0 : (uint)flag,
			    (ulong)operand
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitIntToFloat(
		    ulong size,
		    LowLevelILExpressionIndex operand,
		    FlagIndex? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_INT_TO_FLOAT ,
			    location,
			    size,
			    null == flag  ? 0 : (uint)flag,
			    (ulong)operand
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitFloatConvert(
		    ulong size,
		    LowLevelILExpressionIndex operand,
		    FlagIndex? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FLOAT_CONV ,
			    location,
			    size,
			    null == flag  ? 0 : (uint)flag,
			    (ulong)operand
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitRoundToInt(
		    ulong size,
		    LowLevelILExpressionIndex operand,
		    FlagIndex? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_ROUND_TO_INT ,
			    location,
			    size,
			    null == flag  ? 0 : (uint)flag,
			    (ulong)operand
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitFloor(
		    ulong size,
		    LowLevelILExpressionIndex operand,
		    FlagIndex? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FLOOR ,
			    location,
			    size,
			    null == flag  ? 0 : (uint)flag,
			    (ulong)operand
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitCeil(
		    ulong size,
		    LowLevelILExpressionIndex operand,
		    FlagIndex? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_CEIL ,
			    location,
			    size,
			    null == flag  ? 0 : (uint)flag,
			    (ulong)operand
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitFloatTrunc(
		    ulong size,
		    LowLevelILExpressionIndex operand,
		    FlagIndex? flag = null,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FTRUNC ,
			    location,
			    size,
			    null == flag  ? 0 : (uint)flag,
			    (ulong)operand
		    );
	    }

	    public LowLevelILExpressionIndex EmitFloatEqual(
		    ulong size,
		    LowLevelILExpressionIndex left,
		    LowLevelILExpressionIndex right,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FCMP_E ,
			    location,
			    size,
			    0,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitFloatNotEqual(
		    ulong size,
		    LowLevelILExpressionIndex left,
		    LowLevelILExpressionIndex right,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FCMP_NE ,
			    location,
			    size,
			    0,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitFloatLessThan(
		    ulong size,
		    LowLevelILExpressionIndex left,
		    LowLevelILExpressionIndex right,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FCMP_LT ,
			    location,
			    size,
			    0,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitFloatLessEqual(
		    ulong size,
		    LowLevelILExpressionIndex left,
		    LowLevelILExpressionIndex right,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FCMP_LE ,
			    location,
			    size,
			    0,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitFloatGreaterEqual(
		    ulong size,
		    LowLevelILExpressionIndex left,
		    LowLevelILExpressionIndex right,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FCMP_GE ,
			    location,
			    size,
			    0,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitFloatGreaterThan(
		    ulong size,
		    LowLevelILExpressionIndex left,
		    LowLevelILExpressionIndex right,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FCMP_GT ,
			    location,
			    size,
			    0,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitFloatCompareOrder(
		    ulong size,
		    LowLevelILExpressionIndex left,
		    LowLevelILExpressionIndex right,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FCMP_O ,
			    location,
			    size,
			    0,
			    (ulong)left,
			    (ulong)right
		    );
	    }

	    public LowLevelILExpressionIndex EmitFloatCompareUnorder(
		    ulong size,
		    LowLevelILExpressionIndex left,
		    LowLevelILExpressionIndex right,
		    SourceLocation? location = null)
	    {
		    return this.AddExpression(
			    LowLevelILOperation.LLIL_FCMP_UO ,
			    location,
			    size,
			    0,
			    (ulong)left,
			    (ulong)right
		    );
	    }
	    
	    public LowLevelILExpressionIndex EmitGoto(
		    ulong size,
		    LowLevelILLabel label,
		    SourceLocation? location = null)
	    {
		    if (null == location)
		    {
			    return NativeMethods.BNLowLevelILGoto(
				    this.handle ,
				    label.ToNative()
			    );
		    }
		    else
		    {
			    return NativeMethods.BNLowLevelILGotoWithLocation(
				    this.handle ,
				    label.ToNative(),
				    location.Address,
				    (uint)location.Operand
			    );
		    }
	    }
	    
	    public LowLevelILExpressionIndex EmitIf(
		    ulong size,
		    LowLevelILExpressionIndex condition,
		    LowLevelILLabel trueBranch,
		    LowLevelILLabel falseBranch,
		    SourceLocation? location = null)
	    {
		    
		    if (null == location)
		    {
			    return NativeMethods.BNLowLevelILIf(
				    this.handle ,
				    condition,
				    trueBranch.ToNative(),
				    falseBranch.ToNative()
			    );
		    }
		    else
		    {
			    return NativeMethods.BNLowLevelILIfWithLocation(
				    this.handle ,
				    condition,
				    trueBranch.ToNative(),
				    falseBranch.ToNative(),
				    location.Address,
				    (uint)location.Operand
			    );
		    }
	    }
	    
	    
	}
	
	
}
