using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class Architecture : AbstractSafeHandle<Architecture>
	{
		internal Architecture(IntPtr handle)
			:base(handle, false)
	    {
		    
	    }
		
		internal static Architecture? FromHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new Architecture(handle);
		}
	    
		internal static Architecture MustFromHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new Architecture(handle);
		}
		
		public static Architecture? FromName(string name)
		{
			return Architecture.FromHandle(
				NativeMethods.BNGetArchitectureByName(name)
			);
		}
		
		public static Architecture[] GetAllArchitectures()
		{
			IntPtr arrayPointer = NativeMethods.BNGetArchitectureList(
				out ulong arrayLength
			);

			return UnsafeUtils.TakeHandleArray(
				arrayPointer ,
				arrayLength ,
				Architecture.MustFromHandle ,
				NativeMethods.BNFreeArchitectureList
			);
		}
	  
		public static Architecture NativeTypeParserArchitecture()
		{
			return Architecture.MustFromHandle(
				NativeMethods.BNGetNativeTypeParserArchitecture()
			);
		}
		
		public override string ToString()
		{
			return this.Name;
		}

		public string Name
	    {
		    get
		    {
			    IntPtr raw = NativeMethods.BNGetArchitectureName(this.handle);

			    return UnsafeUtils.TakeAnsiString(raw);
		    }
	    }
	    
	    public Endianness Endianness
	    {
		    get
		    {
			    return NativeMethods.BNGetArchitectureEndianness(this.handle);;
		    }
	    }
	    
	    public ulong AddressSize
	    {
		    get
		    {
			    return NativeMethods.BNGetArchitectureAddressSize(this.handle);;
		    }
	    }
	    
	    public ulong DefaultIntegerSize
	    {
		    get
		    {
			    return NativeMethods.BNGetArchitectureDefaultIntegerSize(this.handle);;
		    }
	    }
	    
	    public ulong InstructionAlignment
	    {
		    get
		    {
			    return NativeMethods.BNGetArchitectureInstructionAlignment(this.handle);;
		    }
	    }
	    
	    public ulong MaxInstructionLength
	    {
		    get
		    {
			    return NativeMethods.BNGetArchitectureMaxInstructionLength(this.handle);;
		    }
	    }
	    
	    public ulong OpcodeDisplayLength
	    {
		    get
		    {
			    return NativeMethods.BNGetArchitectureOpcodeDisplayLength(this.handle);;
		    }
	    }
	    
	    public ILRegister[] FullWidthRegisters
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetFullWidthArchitectureRegisters(
				    this.handle ,
				    out ulong arrayLength
			    );

			    uint[] indexes = UnsafeUtils.TakeNumberArray<uint>(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeRegisterList
			    );
			    
			    List<ILRegister> targets = new List<ILRegister>();

			    foreach (RegisterIndex index in indexes)
			    {
				    targets.Add( new ILRegister(this , index) );
			    }

			    return targets.ToArray();
		    }
	    }
	    
	    public ILRegister[] Registers
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetAllArchitectureRegisters(
				    this.handle ,
				    out ulong arrayLength
			    );

			    uint[] indexes = UnsafeUtils.TakeNumberArray<uint>(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeRegisterList
			    );
			    
			    List<ILRegister> targets = new List<ILRegister>();

			    foreach (RegisterIndex index in indexes)
			    {
				    targets.Add( new ILRegister(this , index) );
			    }

			    return targets.ToArray();
		    }
	    }
	    
	    public ILRegister[] GlobalRegister
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetArchitectureGlobalRegisters(
				    this.handle ,
				    out ulong arrayLength
			    );

			    uint[] indexes = UnsafeUtils.TakeNumberArray<uint>(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeRegisterList
			    );
			    
			    List<ILRegister> targets = new List<ILRegister>();

			    foreach (RegisterIndex index in indexes)
			    {
				    targets.Add( new ILRegister(this , index) );
			    }

			    return targets.ToArray();
		    }
	    }
	    
	    public ILRegister[] SystemRegister
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetArchitectureSystemRegisters(
				    this.handle ,
				    out ulong arrayLength
			    );

			    uint[] indexes = UnsafeUtils.TakeNumberArray<uint>(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeRegisterList
			    );
			    
			    List<ILRegister> targets = new List<ILRegister>();

			    foreach (RegisterIndex index in indexes)
			    {
				    targets.Add( new ILRegister(this , index) );
			    }

			    return targets.ToArray();
		    }
	    }
	    
	    public ILFlag[] Flags
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetAllArchitectureFlags(
				    this.handle ,
				    out ulong arrayLength
			    );

			    uint[] indexes = UnsafeUtils.TakeNumberArray<uint>(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeRegisterList
			    );
			    
			    List<ILFlag> targets = new List<ILFlag>();

			    foreach (FlagIndex index in indexes)
			    {
				    targets.Add( new ILFlag(this , index) );
			    }

			    return targets.ToArray();
		    }
	    }
	    
	    public uint[] FlagWriteTypes
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetAllArchitectureFlagWriteTypes(
				    this.handle ,
				    out ulong arrayLength
			    );

			    return UnsafeUtils.TakeNumberArray<uint>(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeRegisterList
			    );
		    }
	    }
	    
	    public SemanticFlagClass[] SemanticFlagClasses
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetAllArchitectureSemanticFlagClasses(
				    this.handle ,
				    out ulong arrayLength
			    );

			    uint[] indexes = UnsafeUtils.TakeNumberArray<uint>(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeRegisterList
			    );
			    
			    List<SemanticFlagClass> targets = new List<SemanticFlagClass>();

			    foreach (SemanticFlagClassIndex index in indexes)
			    {
				    targets.Add( new SemanticFlagClass(this , index) );
			    }

			    return targets.ToArray();
		    }
	    }
	    
	    public SemanticFlagGroup[] SemanticFlagGroups
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetAllArchitectureSemanticFlagGroups(
				    this.handle ,
				    out ulong arrayLength
			    );

			    uint[] indexes = UnsafeUtils.TakeNumberArray<uint>(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeRegisterList
			    );
			    
			    List<SemanticFlagGroup> targets = new List<SemanticFlagGroup>();

			    foreach (SemanticFlagGroupIndex index in indexes)
			    {
				    targets.Add( new SemanticFlagGroup(this , index) );
			    }

			    return targets.ToArray();
		    }
	    }

	    public BinaryNinja.CallingConvention[] CallingConventions
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetArchitectureCallingConventions(
				    this.handle ,
				    out ulong arrayLength
			    );

			    return UnsafeUtils.TakeHandleArrayEx<BinaryNinja.CallingConvention>(
				    arrayPointer ,
				    arrayLength ,
				    BinaryNinja.CallingConvention.MustNewFromHandle ,
				    NativeMethods.BNFreeCallingConventionList
			    );
		    }
	    }

	    public Platform StandalonePlatform
	    {
		    get
		    {
			    return Platform.MustTakeHandle(
				    NativeMethods.BNGetArchitectureStandalonePlatform(this.handle )
			    );
		    }
	    }

	    public TypeLibrary[] TypeLibrares
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetArchitectureTypeLibraries(
				    this.handle ,
				    out ulong arrayLength
			    );

			    return UnsafeUtils.TakeHandleArrayEx<TypeLibrary>(
				    arrayPointer ,
				    arrayLength ,
				    TypeLibrary.MustNewFromHandle,
				    NativeMethods.BNFreeTypeLibraryList
			    );
		    }
	    }
	    
	    public bool CanAssemble
	    {
		    get
		    {
			    return NativeMethods.BNCanArchitectureAssemble(this.handle);
		    }
	    }
	    
	    
	    public string GetFlagName(FlagIndex flag)
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNGetArchitectureFlagName(this.handle, flag)
		    );
	    }
	    
	    public string GetFlagWriteTypeName(uint flags)
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNGetArchitectureFlagWriteTypeName(this.handle, flags)
		    );
	    }
	    
	    public string GetSemanticFlagClassName(SemanticFlagClassIndex semClass)
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNGetArchitectureSemanticFlagClassName(this.handle, semClass)
		    );
	    }
	    
	    public string GetSemanticFlagGroupName(SemanticFlagGroupIndex semGroup)
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNGetArchitectureSemanticFlagGroupName(this.handle, semGroup)
		    );
	    }

	    public FlagRole GetFlagRole(uint flag , uint semClass)
	    {
		    return NativeMethods.BNGetArchitectureFlagRole(this.handle, flag, semClass);
	    }

	    public uint[] GetFlagsRequiredForFlagCondition(
		    LowLevelILFlagCondition condition,
		    uint semClass
		)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetArchitectureFlagsRequiredForFlagCondition(
			    this.handle ,
			    condition ,
			    semClass ,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeNumberArray<uint>(
			    arrayPointer ,
			    arrayLength ,
			    NativeMethods.BNFreeRegisterList
		    );
	    }
	    
	    public uint[] GetFlagsRequiredForSemanticFlagGroup(
		    uint semGroup
	    )
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetArchitectureFlagsRequiredForSemanticFlagGroup(
			    this.handle ,
			    semGroup,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeNumberArray<uint>(
			    arrayPointer ,
			    arrayLength ,
			    NativeMethods.BNFreeRegisterList
		    );
	    }
	    
	    public FlagConditionForSemanticClass[] GetFlagConditionsForSemanticFlagGroup(uint semGroup)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetArchitectureFlagConditionsForSemanticFlagGroup(
			    this.handle ,
			    semGroup,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArray<BNFlagConditionForSemanticClass,FlagConditionForSemanticClass>(
			    arrayPointer ,
			    arrayLength ,
			    FlagConditionForSemanticClass.FromNative,
			    NativeMethods.BNFreeFlagConditionsForSemanticFlagGroup
		    );
	    }

	    public uint[] GetFlagsWrittenByFlagWriteType(uint writeType)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetArchitectureFlagsWrittenByFlagWriteType(
			    this.handle ,
			    writeType,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeNumberArray<uint>(
			    arrayPointer ,
			    arrayLength ,
			    NativeMethods.BNFreeRegisterList
		    );
	    }
	    
	    public uint GetSemanticClassForFlagWriteType(uint writeType)
	    {
		    return NativeMethods.BNGetArchitectureSemanticClassForFlagWriteType(
			    this.handle ,
			    writeType
		    );
	    }
	    
	 
	    

	    public bool GetInstructionInfo(
		    byte[] data ,
		    ulong address ,
		    ulong maxLength ,
		    out InstructionInfo info
	    )
	    {
		    bool ok = false;

		    BNInstructionInfo raw;

		    ok = NativeMethods.BNGetInstructionInfo(
			    this.handle ,
			    data ,
			    address ,
			    maxLength ,
			    out raw
		    );

		    if (ok)
		    {
			    info = InstructionInfo.FromNative(raw);
		    }
		    else
		    {
			    info = new InstructionInfo();
		    }
		    
		    return ok;
	    }
	    
	    public InstructionTextToken[] GetInstructionText(byte[]data , ulong address , ref ulong length )
	    {
		    IntPtr arrayPointer = IntPtr.Zero;

		    ulong arrayLength = 0;

		    bool ok = false;

		    length = (ulong)data.Length;

		    ok = NativeMethods.BNGetInstructionText(
			    this.handle ,
			    data ,
			    address ,
			    ref length ,
			    out arrayPointer ,
			    out arrayLength 
		    );

		    InstructionTextToken[] tokens = Array.Empty<InstructionTextToken>();
		
		    if (ok )
		    {
			    tokens = UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken ,InstructionTextToken>(
				    arrayPointer ,
				    arrayLength,
				    InstructionTextToken.FromNative,
				    NativeMethods.BNFreeInstructionText
			    );
		    }

		    return tokens;
	    }

	    public string GetRegisterName(RegisterIndex reg)
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNGetArchitectureRegisterName(this.handle, reg)
		    );
	    }
	    
	    public RegisterInfo GetRegisterInfo(RegisterIndex reg)
	    {
		    return RegisterInfo.FromNative(
			    NativeMethods.BNGetArchitectureRegisterInfo(this.handle , reg)
		    );
	    }
	    
	    public ILRegister GetRegisterByName(string name)
	    {
		    return new ILRegister(
			    this ,
			    NativeMethods.BNGetArchitectureRegisterByName(this.handle , name)
		    );
	    }
	    
	    public string GetRegisterStackName(RegisterStackIndex regStack)
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNGetArchitectureRegisterStackName(this.handle, regStack)
		    );
	    }
	    
	    public ulong? GetInstructionLowLevelIL(
		    byte[] data , 
		    ulong address ,
		    LowLevelILFunction function
	    )
	    {
		    ulong size = (ulong)data.Length;
		    
		    bool ok = NativeMethods.BNGetInstructionLowLevelIL(
			    this.handle,
			    data ,
			    address,
			    ref size,
			    function.DangerousGetHandle()
		    );

		    if (!ok)
		    {
			    return null;
		    }

		    return size;
	    }

	    public ulong GetFlagWriteLowLevelIL(
		    LowLevelILOperation operation,
		    ulong size ,
		    uint flagWriteType ,
		    uint flag,
		    RegisterOrConstant[] operands,
		    LowLevelILFunction il
		)
	    {
		    return NativeMethods.BNGetArchitectureFlagWriteLowLevelIL(
			    this.handle ,
			    operation ,
			    size ,
			    flagWriteType ,
			    flag ,
			    UnsafeUtils.ConvertToNativeArray<BNRegisterOrConstant , RegisterOrConstant>(operands) ,
			    (uint)operands.Length ,
			    il.DangerousGetHandle()
		    );
	    }
	    
	    public ulong GetDefaultFlagWriteLowLevelIL(
		    LowLevelILOperation operation,
		    ulong size ,
		    FlagRole role ,
		    RegisterOrConstant[] operands,
		    LowLevelILFunction il
	    )
	    {
		    return NativeMethods.BNGetDefaultArchitectureFlagWriteLowLevelIL(
			    this.handle ,
			    operation ,
			    size ,
			    role,
			    UnsafeUtils.ConvertToNativeArray<BNRegisterOrConstant , RegisterOrConstant>(operands) ,
			    (uint)operands.Length ,
			    il.DangerousGetHandle()
		    );
	    }
	    
	    public ulong GetFlagConditionLowLevelIL(
		    LowLevelILFlagCondition condition,
		    uint semClass,
		    LowLevelILFunction il
	    )
	    {
		    return NativeMethods.BNGetArchitectureFlagConditionLowLevelIL(
			    this.handle ,
			    condition,
			    semClass,
			    il.DangerousGetHandle()
		    );
	    }
	    
	    public ulong GetSemanticFlagGroupLowLevelIL(
		    uint semGroup,
		    LowLevelILFunction il
	    )
	    {
		    return NativeMethods.BNGetArchitectureSemanticFlagGroupLowLevelIL(
			    this.handle ,
			    semGroup,
			    il.DangerousGetHandle()
		    );
	    }
	    
	    public uint[] GetModifiedRegistersOnWrite(uint reg)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetModifiedArchitectureRegistersOnWrite(
			    this.handle ,
			    reg,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeNumberArray<uint>(
			    arrayPointer ,
			    arrayLength ,
			    NativeMethods.BNFreeRegisterList
		    );
	    }
	    
	    public Architecture? GetAssociatedArchitectureByAddress(ref ulong address)
	    {
		    return Architecture.FromHandle(
			    NativeMethods.BNGetAssociatedArchitectureByAddress(this.handle , ref address)
		    );
	    }

	    
	    public bool Assemble(
		    string code , 
		    ulong address , 
		    out byte[] data,
		    out string errors
		)
	    {
		    DataBuffer buffer = new DataBuffer( Array.Empty<byte>());
		    
		    IntPtr errorPtr = IntPtr.Zero;

		    bool ok = false;

		    data = Array.Empty<byte>();
		    
		    errors = "";
		    
		    ok = NativeMethods.BNAssemble(
			    this.handle ,
			    code ,
			    address ,
			    buffer.DangerousGetHandle() ,
			    out errorPtr
		    );

		    errors = UnsafeUtils.TakeAnsiString(errorPtr);

		    if (ok)
		    {
			    data = buffer.Contents;
		    }
		    
		    return ok;
	    }

	    public bool IsNeverBranchPatchAvailable(byte[] data , ulong address)
	    {
		    return NativeMethods.BNIsArchitectureNeverBranchPatchAvailable(
			    this.handle ,
			    data ,
			    address ,
			    (ulong)data.Length
		    );
	    }
	    
	    public bool IsAlwaysBranchPatchAvailable(byte[] data , ulong address)
	    {
		    return NativeMethods.BNIsArchitectureAlwaysBranchPatchAvailable(
			    this.handle ,
			    data ,
			    address ,
			    (ulong)data.Length
		    );
	    }
	    
	    public bool IsInvertBranchPatchAvailable(byte[] data , ulong address)
	    {
		    return NativeMethods.BNIsArchitectureInvertBranchPatchAvailable(
			    this.handle ,
			    data ,
			    address ,
			    (ulong)data.Length
		    );
	    }
	    
	    public bool IsSkipAndReturnZeroPatchAvailable(byte[] data , ulong address)
	    {
		    return NativeMethods.BNIsArchitectureSkipAndReturnZeroPatchAvailable(
			    this.handle ,
			    data ,
			    address ,
			    (ulong)data.Length
		    );
	    }
	    
	    public bool IsSkipAndReturnValuePatchAvailable(byte[] data , ulong address)
	    {
		    return NativeMethods.BNIsArchitectureSkipAndReturnValuePatchAvailable(
			    this.handle ,
			    data ,
			    address ,
			    (ulong)data.Length
		    );
	    }
	    
	    public bool ConvertToNop(byte[] data , ulong address)
	    {
		    return NativeMethods.BNArchitectureConvertToNop(
			    this.handle ,
			    data ,
			    address ,
			    (ulong)data.Length
		    );
	    }
	    
	    public bool AlwaysBranch(byte[] data , ulong address)
	    {
		    return NativeMethods.BNArchitectureAlwaysBranch(
			    this.handle ,
			    data ,
			    address ,
			    (ulong)data.Length
		    );
	    }
	    
	    public bool InvertBranch(byte[] data , ulong address)
	    {
		    return NativeMethods.BNArchitectureInvertBranch(
			    this.handle ,
			    data ,
			    address ,
			    (ulong)data.Length
		    );
	    }
	    
	    public bool SkipAndReturnValue(byte[] data , ulong address , ulong _value)
	    {
		    return NativeMethods.BNArchitectureSkipAndReturnValue(
			    this.handle ,
			    data ,
			    address ,
			    (ulong)data.Length,
			    _value
		    );
	    }
	    
	    public TypeLibrary? LookupTypeLibraryByName(string name)
	    {
		    IntPtr raw = NativeMethods.BNLookupTypeLibraryByName(this.handle ,name);

		    if (IntPtr.Zero == raw)
		    {
			    return null;
		    }
		    
		    return new TypeLibrary(raw, true);
	    }
	    
	    public TypeLibrary? LookupTypeLibraryByGuid(string name)
	    {
		    IntPtr raw = NativeMethods.BNLookupTypeLibraryByGuid(this.handle ,name);

		    if (IntPtr.Zero == raw)
		    {
			    return null;
		    }
		    
		    return new TypeLibrary(raw, true);
	    }

	    public Intrinsic[] Intrinsics
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetAllArchitectureIntrinsics(
				    this.handle ,
				    out ulong arrayLength
			    );

			    uint[] indexes = UnsafeUtils.TakeNumberArray<uint>(
				    arrayPointer ,
				    arrayLength ,
				    NativeMethods.BNFreeRegisterList
			    );
			    
			    List<Intrinsic> targets = new List<Intrinsic>();

			    foreach (IntrinsicIndex index in indexes)
			    {
				    targets.Add(
					    new Intrinsic(this , index)
				    );
			    }

			    return targets.ToArray();
		    }
	    }
	    
	    public void FinalizeArchitectureHook()
	    {
		    NativeMethods.BNFinalizeArchitectureHook(this.handle);
	    }

	    public ILRegister StackPointerRegister
	    {
		    get
		    {
			    return new ILRegister(
				    this ,
				    (RegisterIndex)NativeMethods.BNGetArchitectureStackPointerRegister(this.handle)
			    );
		    }
	    }

	    /// <summary>
	    /// Gets the relocation handler registered for this architecture under the given view name.
	    /// Returns null if no handler is registered.
	    /// </summary>
	    /// <param name="viewName">The binary view type name (e.g., "ELF", "PE") to look up the handler for.</param>
	    /// <returns>The relocation handler, or null if none is registered.</returns>
	    public RelocationHandler? GetRelocationHandler(string viewName)
	    {
		    return RelocationHandler.TakeHandle(
			    NativeMethods.BNArchitectureGetRelocationHandler(this.handle, viewName)
		    );
	    }

	    /// <summary>
	    /// Registers a relocation handler for this architecture under the given view name.
	    /// The handler will be used to apply relocations when loading binaries of the specified type.
	    /// </summary>
	    /// <param name="viewName">The binary view type name (e.g., "ELF", "PE") to register the handler for.</param>
	    /// <param name="handler">The relocation handler to register.</param>
	    public void RegisterRelocationHandler(string viewName, RelocationHandler handler)
	    {
		    NativeMethods.BNArchitectureRegisterRelocationHandler(
			    this.handle, viewName, handler.DangerousGetHandle()
		    );
	    }

	    /// <summary>
	    /// Invokes the architecture's basic block analysis callback on a function with the given context.
	    /// This drives the disassembly and basic block discovery for the specified function.
	    /// </summary>
	    /// <param name="function">The function to analyze.</param>
	    /// <param name="context">Pointer to a BNBasicBlockAnalysisContext structure controlling the analysis.</param>
	    public void AnalyzeBasicBlocks(Function function, IntPtr context)
	    {
		    NativeMethods.BNArchitectureAnalyzeBasicBlocks(
			    this.handle, function.DangerousGetHandle(), context
		    );
	    }

	    /// <summary>
	    /// Invokes the default basic block analysis implementation, bypassing any custom architecture override.
	    /// Useful when a custom architecture wants to fall back to the base analysis behavior.
	    /// </summary>
	    /// <param name="function">The function to analyze.</param>
	    /// <param name="context">Pointer to a BNBasicBlockAnalysisContext structure controlling the analysis.</param>
	    public void DefaultAnalyzeBasicBlocks(Function function, IntPtr context)
	    {
		    NativeMethods.BNArchitectureDefaultAnalyzeBasicBlocks(
			    function.DangerousGetHandle(), context
		    );
	    }

	    /// <summary>
	    /// Returns all register stacks defined by this architecture. Register stacks represent
	    /// groups of registers that behave as a stack (e.g., x87 FPU register stack).
	    /// </summary>
	    public unsafe uint[] GetAllRegisterStacks()
	    {
		    // 1. Call the native API to retrieve the register stack index array.
		    ulong count = 0;
		    IntPtr ptr = NativeMethods.BNGetAllArchitectureRegisterStacks(
			    this.handle, (IntPtr)(&count)
		    );

		    // 2. Return empty if no register stacks or null pointer.
		    if (0 == count || IntPtr.Zero == ptr)
		    {
			    return Array.Empty<uint>();
		    }

		    // 3. Marshal the native array into a managed uint[] and free the native buffer.
		    return UnsafeUtils.TakeNumberArray<uint>(ptr, count, NativeMethods.BNFreeRegisterList);
	    }

	    // ===================================================================
	    // Architecture redirection
	    // ===================================================================

	    /// <summary>
	    /// Adds a redirection from one architecture to another through this architecture.
	    /// </summary>
	    /// <param name="from">The source architecture to redirect from.</param>
	    /// <param name="to">The target architecture to redirect to.</param>
	    public void AddRedirection(Architecture from , Architecture to)
	    {
		    NativeMethods.BNAddArchitectureRedirection(
			    this.handle ,
			    from.DangerousGetHandle() ,
			    to.DangerousGetHandle()
		    );
	    }

	    // ===================================================================
	    // Link register
	    // ===================================================================

	    /// <summary>
	    /// Returns the link register index for this architecture.
	    /// </summary>
	    /// <returns>The register index of the link register.</returns>
	    public uint GetLinkRegister()
	    {
		    return NativeMethods.BNGetArchitectureLinkRegister(this.handle);
	    }

	    // ===================================================================
	    // Register stack queries
	    // ===================================================================

	    /// <summary>
	    /// Returns the register stack index that contains the given register.
	    /// </summary>
	    /// <param name="reg">The register index to look up.</param>
	    /// <returns>The register stack index containing this register.</returns>
	    public uint GetRegisterStackForRegister(uint reg)
	    {
		    return NativeMethods.BNGetArchitectureRegisterStackForRegister(this.handle , reg);
	    }

	    /// <summary>
	    /// Returns detailed information about the specified register stack.
	    /// </summary>
	    /// <param name="regStack">The register stack index to query.</param>
	    /// <returns>A RegisterStackInfo describing the register stack layout.</returns>
	    public RegisterStackInfo GetRegisterStackInfo(uint regStack)
	    {
		    return new RegisterStackInfo(
			    NativeMethods.BNGetArchitectureRegisterStackInfo(this.handle , regStack)
		    );
	    }

	    // ===================================================================
	    // Default flag condition lowering
	    // ===================================================================

	    /// <summary>
	    /// Returns the default Low Level IL expression index for a flag condition in this architecture.
	    /// </summary>
	    /// <param name="cond">The flag condition to lower.</param>
	    /// <param name="semClass">The semantic flag class.</param>
	    /// <param name="il">The LLIL function to add the expression to.</param>
	    /// <returns>The expression index of the lowered flag condition.</returns>
	    public ulong GetDefaultFlagConditionLowLevelIL(
		    LowLevelILFlagCondition cond ,
		    uint semClass ,
		    LowLevelILFunction il
	    )
	    {
		    return NativeMethods.BNGetDefaultArchitectureFlagConditionLowLevelIL(
			    this.handle ,
			    cond ,
			    semClass ,
			    il.DangerousGetHandle()
		    );
	    }

	    // ===================================================================
	    // Calling conventions
	    // ===================================================================

	    /// <summary>
	    /// Retrieves a calling convention registered with this architecture by its name.
	    /// </summary>
	    /// <param name="name">The name of the calling convention.</param>
	    /// <returns>The matching CallingConvention, or null if not found.</returns>
	    public CallingConvention? GetCallingConventionByName(string name)
	    {
		    return CallingConvention.NewFromHandle(
			    NativeMethods.BNGetArchitectureCallingConventionByName(this.handle , name)
		    );
	    }

	    /// <summary>
	    /// Gets the cdecl calling convention for this architecture.
	    /// </summary>
	    /// <returns>The cdecl CallingConvention, or null if not set.</returns>
	    public CallingConvention? GetCdeclCallingConvention()
	    {
		    return CallingConvention.NewFromHandle(
			    NativeMethods.BNGetArchitectureCdeclCallingConvention(this.handle)
		    );
	    }

	    /// <summary>
	    /// Sets the cdecl calling convention for this architecture.
	    /// </summary>
	    /// <param name="cc">The calling convention to set as cdecl.</param>
	    public void SetCdeclCallingConvention(CallingConvention cc)
	    {
		    NativeMethods.BNSetArchitectureCdeclCallingConvention(
			    this.handle ,
			    cc.DangerousGetHandle()
		    );
	    }

	    /// <summary>
	    /// Gets the default calling convention for this architecture.
	    /// </summary>
	    /// <returns>The default CallingConvention, or null if not set.</returns>
	    public CallingConvention? GetDefaultCallingConvention()
	    {
		    return CallingConvention.NewFromHandle(
			    NativeMethods.BNGetArchitectureDefaultCallingConvention(this.handle)
		    );
	    }

	    /// <summary>
	    /// Sets the default calling convention for this architecture.
	    /// </summary>
	    /// <param name="cc">The calling convention to set as default.</param>
	    public void SetDefaultCallingConvention(CallingConvention cc)
	    {
		    NativeMethods.BNSetArchitectureDefaultCallingConvention(
			    this.handle ,
			    cc.DangerousGetHandle()
		    );
	    }

	    /// <summary>
	    /// Gets the fastcall calling convention for this architecture.
	    /// </summary>
	    /// <returns>The fastcall CallingConvention, or null if not set.</returns>
	    public CallingConvention? GetFastcallCallingConvention()
	    {
		    return CallingConvention.NewFromHandle(
			    NativeMethods.BNGetArchitectureFastcallCallingConvention(this.handle)
		    );
	    }

	    /// <summary>
	    /// Sets the fastcall calling convention for this architecture.
	    /// </summary>
	    /// <param name="cc">The calling convention to set as fastcall.</param>
	    public void SetFastcallCallingConvention(CallingConvention cc)
	    {
		    NativeMethods.BNSetArchitectureFastcallCallingConvention(
			    this.handle ,
			    cc.DangerousGetHandle()
		    );
	    }

	    /// <summary>
	    /// Gets the stdcall calling convention for this architecture.
	    /// </summary>
	    /// <returns>The stdcall CallingConvention, or null if not set.</returns>
	    public CallingConvention? GetStdcallCallingConvention()
	    {
		    return CallingConvention.NewFromHandle(
			    NativeMethods.BNGetArchitectureStdcallCallingConvention(this.handle)
		    );
	    }

	    /// <summary>
	    /// Sets the stdcall calling convention for this architecture.
	    /// </summary>
	    /// <param name="cc">The calling convention to set as stdcall.</param>
	    public void SetStdcallCallingConvention(CallingConvention cc)
	    {
		    NativeMethods.BNSetArchitectureStdcallCallingConvention(
			    this.handle ,
			    cc.DangerousGetHandle()
		    );
	    }

	}

}