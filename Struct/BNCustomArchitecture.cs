using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNCustomArchitecture 
	{
		/// <summary>
		/// void* context
		/// </summary>
		internal IntPtr context;

		/// <summary>
		/// void (*init)(void* context, BNArchitecture* obj)
		/// </summary>
		internal IntPtr init;

		/// <summary>
		/// BNEndianness (*getEndianness)(void* ctxt)
		/// </summary>
		internal IntPtr getEndianness;

		/// <summary>
		/// size_t (*getAddressSize)(void* ctxt)
		/// </summary>
		internal IntPtr getAddressSize;

		/// <summary>
		/// size_t (*getDefaultIntegerSize)(void* ctxt)
		/// </summary>
		internal IntPtr getDefaultIntegerSize;

		/// <summary>
		/// size_t (*getInstructionAlignment)(void* ctxt)
		/// </summary>
		internal IntPtr getInstructionAlignment;

		/// <summary>
		/// size_t (*getMaxInstructionLength)(void* ctxt)
		/// </summary>
		internal IntPtr getMaxInstructionLength;

		/// <summary>
		/// size_t (*getOpcodeDisplayLength)(void* ctxt)
		/// </summary>
		internal IntPtr getOpcodeDisplayLength;

		/// <summary>
		/// BNArchitecture* (*getAssociatedArchitectureByAddress)(void* ctxt, uint64_t* addr)
		/// </summary>
		internal IntPtr getAssociatedArchitectureByAddress;

		/// <summary>
		/// bool (*getInstructionInfo)( void* ctxt, const uint8_t* data, uint64_t addr, size_t maxLen, BNInstructionInfo* result)
		/// </summary>
		internal IntPtr getInstructionInfo;

		/// <summary>
		/// bool (*getInstructionText)(void* ctxt, const uint8_t* data, uint64_t addr, size_t* len, BNInstructionTextToken** result, size_t* count)
		/// </summary>
		internal IntPtr getInstructionText;

		/// <summary>
		/// bool (*getInstructionTextWithContext)(void* ctxt, const uint8_t* data, uint64_t addr, size_t* len, void* context, BNInstructionTextToken** result, size_t* count)
		/// </summary>
		internal IntPtr getInstructionTextWithContext;

		/// <summary>
		/// void (*freeInstructionText)(BNInstructionTextToken* tokens, size_t count)
		/// </summary>
		internal IntPtr freeInstructionText;

		/// <summary>
		/// bool (*getInstructionLowLevelIL)( void* ctxt, const uint8_t* data, uint64_t addr, size_t* len, BNLowLevelILFunction* il)
		/// </summary>
		internal IntPtr getInstructionLowLevelIL;

		/// <summary>
		/// void (*analyzeBasicBlocks)(void* ctxt, BNFunction* function, BNBasicBlockAnalysisContext* context)
		/// </summary>
		internal IntPtr analyzeBasicBlocks;

		/// <summary>
		/// bool (*liftFunction)(void *ctext, BNLowLevelILFunction* function, BNFunctionLifterContext* context)
		/// </summary>
		internal IntPtr liftFunction;

		/// <summary>
		/// void (*freeFunctionArchContext)(void *ctxt, void* context)
		/// </summary>
		internal IntPtr freeFunctionArchContext;

		/// <summary>
		/// char* (*getRegisterName)(void* ctxt, uint32_t reg)
		/// </summary>
		internal IntPtr getRegisterName;

		/// <summary>
		/// char* (*getFlagName)(void* ctxt, uint32_t flag)
		/// </summary>
		internal IntPtr getFlagName;

		/// <summary>
		/// char* (*getFlagWriteTypeName)(void* ctxt, uint32_t flags)
		/// </summary>
		internal IntPtr getFlagWriteTypeName;

		/// <summary>
		/// char* (*getSemanticFlagClassName)(void* ctxt, uint32_t semClass)
		/// </summary>
		internal IntPtr getSemanticFlagClassName;

		/// <summary>
		/// char* (*getSemanticFlagGroupName)(void* ctxt, uint32_t semGroup)
		/// </summary>
		internal IntPtr getSemanticFlagGroupName;

		/// <summary>
		/// uint32_t* (*getFullWidthRegisters)(void* ctxt, size_t* count)
		/// </summary>
		internal IntPtr getFullWidthRegisters;

		/// <summary>
		/// uint32_t* (*getAllRegisters)(void* ctxt, size_t* count)
		/// </summary>
		internal IntPtr getAllRegisters;

		/// <summary>
		/// uint32_t* (*getAllFlags)(void* ctxt, size_t* count)
		/// </summary>
		internal IntPtr getAllFlags;

		/// <summary>
		/// uint32_t* (*getAllFlagWriteTypes)(void* ctxt, size_t* count)
		/// </summary>
		internal IntPtr getAllFlagWriteTypes;

		/// <summary>
		/// uint32_t* (*getAllSemanticFlagClasses)(void* ctxt, size_t* count)
		/// </summary>
		internal IntPtr getAllSemanticFlagClasses;

		/// <summary>
		/// uint32_t* (*getAllSemanticFlagGroups)(void* ctxt, size_t* count)
		/// </summary>
		internal IntPtr getAllSemanticFlagGroups;

		/// <summary>
		/// BNFlagRole (*getFlagRole)(void* ctxt, uint32_t flag, uint32_t semClass)
		/// </summary>
		internal IntPtr getFlagRole;

		/// <summary>
		/// uint32_t* (*getFlagsRequiredForFlagCondition)( void* ctxt, BNLowLevelILFlagCondition cond, uint32_t semClass, size_t* count)
		/// </summary>
		internal IntPtr getFlagsRequiredForFlagCondition;

		/// <summary>
		/// uint32_t* (*getFlagsRequiredForSemanticFlagGroup)(void* ctxt, uint32_t semGroup, size_t* count)
		/// </summary>
		internal IntPtr getFlagsRequiredForSemanticFlagGroup;

		/// <summary>
		/// BNFlagConditionForSemanticClass* (*getFlagConditionsForSemanticFlagGroup)( void* ctxt, uint32_t semGroup, size_t* count)
		/// </summary>
		internal IntPtr getFlagConditionsForSemanticFlagGroup;

		/// <summary>
		/// void (*freeFlagConditionsForSemanticFlagGroup)(void* ctxt, BNFlagConditionForSemanticClass* conditions, size_t count)
		/// </summary>
		internal IntPtr freeFlagConditionsForSemanticFlagGroup;

		/// <summary>
		/// uint32_t* (*getFlagsWrittenByFlagWriteType)(void* ctxt, uint32_t writeType, size_t* count)
		/// </summary>
		internal IntPtr getFlagsWrittenByFlagWriteType;

		/// <summary>
		/// uint32_t (*getSemanticClassForFlagWriteType)(void* ctxt, uint32_t writeType)
		/// </summary>
		internal IntPtr getSemanticClassForFlagWriteType;

		/// <summary>
		/// size_t (*getFlagWriteLowLevelIL)(void* ctxt, BNLowLevelILOperation op, size_t size, uint32_t flagWriteType, uint32_t flag, BNRegisterOrConstant* operands, size_t operandCount, BNLowLevelILFunction* il)
		/// </summary>
		internal IntPtr getFlagWriteLowLevelIL;

		/// <summary>
		/// size_t (*getFlagConditionLowLevelIL)( void* ctxt, BNLowLevelILFlagCondition cond, uint32_t semClass, BNLowLevelILFunction* il)
		/// </summary>
		internal IntPtr getFlagConditionLowLevelIL;

		/// <summary>
		/// size_t (*getSemanticFlagGroupLowLevelIL)(void* ctxt, uint32_t semGroup, BNLowLevelILFunction* il)
		/// </summary>
		internal IntPtr getSemanticFlagGroupLowLevelIL;

		/// <summary>
		/// void (*freeRegisterList)(void* ctxt, uint32_t* regs, size_t count)
		/// </summary>
		internal IntPtr freeRegisterList;

		/// <summary>
		/// void (*getRegisterInfo)(void* ctxt, uint32_t reg, BNRegisterInfo* result)
		/// </summary>
		internal IntPtr getRegisterInfo;

		/// <summary>
		/// uint32_t (*getStackPointerRegister)(void* ctxt)
		/// </summary>
		internal IntPtr getStackPointerRegister;

		/// <summary>
		/// uint32_t (*getLinkRegister)(void* ctxt)
		/// </summary>
		internal IntPtr getLinkRegister;

		/// <summary>
		/// uint32_t* (*getGlobalRegisters)(void* ctxt, size_t* count)
		/// </summary>
		internal IntPtr getGlobalRegisters;

		/// <summary>
		/// uint32_t* (*getSystemRegisters)(void* ctxt, size_t* count)
		/// </summary>
		internal IntPtr getSystemRegisters;

		/// <summary>
		/// char* (*getRegisterStackName)(void* ctxt, uint32_t regStack)
		/// </summary>
		internal IntPtr getRegisterStackName;

		/// <summary>
		/// uint32_t* (*getAllRegisterStacks)(void* ctxt, size_t* count)
		/// </summary>
		internal IntPtr getAllRegisterStacks;

		/// <summary>
		/// void (*getRegisterStackInfo)(void* ctxt, uint32_t regStack, BNRegisterStackInfo* result)
		/// </summary>
		internal IntPtr getRegisterStackInfo;

		/// <summary>
		/// BNIntrinsicClass (*getIntrinsicClass)(void* ctxt, uint32_t intrinsic)
		/// </summary>
		internal IntPtr getIntrinsicClass;

		/// <summary>
		/// char* (*getIntrinsicName)(void* ctxt, uint32_t intrinsic)
		/// </summary>
		internal IntPtr getIntrinsicName;

		/// <summary>
		/// uint32_t* (*getAllIntrinsics)(void* ctxt, size_t* count)
		/// </summary>
		internal IntPtr getAllIntrinsics;

		/// <summary>
		/// BNNameAndType* (*getIntrinsicInputs)(void* ctxt, uint32_t intrinsic, size_t* count)
		/// </summary>
		internal IntPtr getIntrinsicInputs;

		/// <summary>
		/// void (*freeNameAndTypeList)(void* ctxt, BNNameAndType* nt, size_t count)
		/// </summary>
		internal IntPtr freeNameAndTypeList;

		/// <summary>
		/// BNTypeWithConfidence* (*getIntrinsicOutputs)(void* ctxt, uint32_t intrinsic, size_t* count)
		/// </summary>
		internal IntPtr getIntrinsicOutputs;

		/// <summary>
		/// void (*freeTypeList)(void* ctxt, BNTypeWithConfidence* types, size_t count)
		/// </summary>
		internal IntPtr freeTypeList;

		/// <summary>
		/// bool (*canAssemble)(void* ctxt)
		/// </summary>
		internal IntPtr canAssemble;

		/// <summary>
		/// bool (*assemble)(void* ctxt, const char* code, uint64_t addr, BNDataBuffer* result, char** errors)
		/// </summary>
		internal IntPtr assemble;

		/// <summary>
		/// bool (*isNeverBranchPatchAvailable)(void* ctxt, const uint8_t* data, uint64_t addr, size_t len)
		/// </summary>
		internal IntPtr isNeverBranchPatchAvailable;

		/// <summary>
		/// bool (*isAlwaysBranchPatchAvailable)(void* ctxt, const uint8_t* data, uint64_t addr, size_t len)
		/// </summary>
		internal IntPtr isAlwaysBranchPatchAvailable;

		/// <summary>
		/// bool (*isInvertBranchPatchAvailable)(void* ctxt, const uint8_t* data, uint64_t addr, size_t len)
		/// </summary>
		internal IntPtr isInvertBranchPatchAvailable;

		/// <summary>
		/// bool (*isSkipAndReturnZeroPatchAvailable)(void* ctxt, const uint8_t* data, uint64_t addr, size_t len)
		/// </summary>
		internal IntPtr isSkipAndReturnZeroPatchAvailable;

		/// <summary>
		/// bool (*isSkipAndReturnValuePatchAvailable)(void* ctxt, const uint8_t* data, uint64_t addr, size_t len)
		/// </summary>
		internal IntPtr isSkipAndReturnValuePatchAvailable;

		/// <summary>
		/// bool (*convertToNop)(void* ctxt, uint8_t* data, uint64_t addr, size_t len)
		/// </summary>
		internal IntPtr convertToNop;

		/// <summary>
		/// bool (*alwaysBranch)(void* ctxt, uint8_t* data, uint64_t addr, size_t len)
		/// </summary>
		internal IntPtr alwaysBranch;

		/// <summary>
		/// bool (*invertBranch)(void* ctxt, uint8_t* data, uint64_t addr, size_t len)
		/// </summary>
		internal IntPtr invertBranch;

		/// <summary>
		/// bool (*skipAndReturnValue)(void* ctxt, uint8_t* data, uint64_t addr, size_t len, uint64_t value)
		/// </summary>
		internal IntPtr skipAndReturnValue;
	}

    public abstract partial class CustomArchitecture
    {
		internal delegate void InitDelegate(
			Architecture arch
		);
		
		internal delegate Endianness GetEndiannessDelegate();
		
		internal delegate ulong GetAddressSizeDelegate();
		
		internal delegate ulong GetDefaultIntegerSizeDelegate();
		
		internal delegate ulong GetInstructionAlignmentDelegate();
		
		internal delegate ulong GetMaxInstructionLengthDelegate();
		
		internal delegate ulong GetOpcodeDisplayLengthDelegate();

		internal delegate Architecture? GetAssociatedArchitectureByAddressDelegate(
			ref ulong address
		);
		
		internal delegate InstructionInfo? GetInstructionInfoDelegate(
			byte[] data,
			ulong address
		);
		
		internal delegate InstructionTextToken[] GetInstructionTextDelegate(
			byte[] data,
			ulong address,
			out ulong length
		);

		internal delegate void FreeInstructionTextDelegate(
			IntPtr tokens,
			ulong count
		);
		
		internal delegate bool GetInstructionLowLevelILDelegate(
			byte[] data,
			ulong address,
			out ulong length,
			LowLevelILFunction il
		);
		
		internal delegate string GetRegisterNameDelegate(
			RegisterIndex reg
		);
		
		internal delegate string getFlagNameDelegate(
			FlagIndex flag
		);
		
		internal delegate string GetFlagWriteTypeNameDelegate(
			uint flags
		);
		
		internal delegate string GetSemanticFlagClassNameDelegate(
			uint semClass
		);
		
		internal delegate string GetSemanticFlagGroupNameDelegate(
			uint  semGroup
		);
		
		internal delegate uint[] GetFullWidthRegistersDelegate();
		
		internal delegate uint[] GetAllRegistersDelegate();
		
		internal delegate uint[] GetAllFlagsDelegate();
		
		internal delegate uint[] GetAllFlagWriteTypesDelegate();
		
		internal delegate uint[] GetAllSemanticFlagClassesDelegate();
		
		internal delegate uint[] GetAllSemanticFlagGroupsDelegate();
		
		internal delegate FlagRole GetFlagRoleDelegate(
			uint flag,
			uint semClass
		);
		
		internal delegate uint[] GetFlagsRequiredForFlagConditionDelegate(
			LowLevelILFlagCondition cond,
			uint semClass
		);
		
		internal delegate uint[] GetFlagsRequiredForSemanticFlagGroupDelegate(
			uint semGroup
		);
		
		internal delegate FlagConditionForSemanticClass[] GetFlagConditionsForSemanticFlagGroupDelegate(
			uint semGroup
		);
		
		internal delegate void FreeFlagConditionsForSemanticFlagGroupDelegate(
			IntPtr conditions
		);
		
		internal delegate uint[] GetFlagsWrittenByFlagWriteTypeDelegate(
			uint writeType
		);
		
		internal delegate uint GetSemanticClassForFlagWriteTypeDelegate(
			uint writeType
		);
		
		internal delegate ulong GetFlagWriteLowLevelILDelegate(
			LowLevelILOperation op,
			ulong size,
			uint flagWriteType,
			uint flag,
			RegisterOrConstant[] operands,
			LowLevelILFunction il
		);
		
		internal delegate ulong GetFlagConditionLowLevelILDelegate(
			LowLevelILFlagCondition cond,
			uint semClass,
			LowLevelILFunction il
		);
		
		internal delegate ulong GetSemanticFlagGroupLowLevelILDelegate(
			uint semGroup,
			LowLevelILFunction il
		);
		
		internal delegate void FreeRegisterListDelegate(
			IntPtr regs
		);
		
		internal delegate RegisterInfo GetRegisterInfoDelegate(
			uint reg
		);
		
		internal delegate uint GetStackPointerRegisterDelegate();
		
		internal delegate uint GetLinkRegisterDelegate();
		
		internal delegate uint[] GetGlobalRegistersDelegate();
		
		internal delegate uint[] GetSystemRegistersDelegate();
		
		internal delegate string GetRegisterStackNameDelegate(
			uint regStack
		);
		
		internal unsafe delegate uint[] GetAllRegisterStacksDelegate();
		
		internal unsafe delegate RegisterStackInfo GetRegisterStackInfoDelegate(
			uint regStack
		);
		
		internal delegate IntrinsicClass GetIntrinsicClassDelegate(
			uint intrinsic
		);
		
		internal delegate string GetIntrinsicNameDelegate(
			uint intrinsic
		);
		
		internal delegate uint[] GetAllIntrinsicsDelegate();
		
		internal delegate NameAndType[] GetIntrinsicInputsDelegate(
			uint intrinsic
		);
		
		internal unsafe delegate void FreeNameAndTypeListDelegate(
			IntPtr nt,
			ulong count
		);
		
		internal delegate TypeWithConfidence[] GetIntrinsicOutputsDelegate(
			uint intrinsic
		);
		
		internal delegate void FreeTypeListDelegate(
			IntPtr types,
			ulong count
		);
		
		internal delegate bool CanAssembleDelegate();
		
		internal delegate byte[] AssembleDelegate(
			string code,
			ulong address
		);
		
		internal delegate bool IsNeverBranchPatchAvailableDelegate(
			byte[] data,
			ulong address
		);
		
		internal delegate bool IsAlwaysBranchPatchAvailableDelegate(
			byte[] data,
			ulong address
		);
		
		internal delegate bool IsInvertBranchPatchAvailableDelegate(
			byte[] data,
			ulong address
		);
		
		internal delegate bool IsSkipAndReturnZeroPatchAvailableDelegate(
			byte[] data,
			ulong address
		);
		
		internal delegate void IsSkipAndReturnValuePatchAvailableDelegate(
			byte[] data,
			ulong address
		);
		
		internal delegate bool ConvertToNopDelegate(
			byte[] data,
			ulong address
		);
		
		internal delegate bool AlwaysBranchDelegate(
			byte[] data,
			ulong address
		);
		
		internal delegate bool InvertBranchDelegate(
			byte[] data,
			ulong address
		);
		
		internal delegate bool SkipAndReturnValueDelegate(
			byte[] data,
			ulong address,
			ulong value
		);
		
		public CustomArchitecture() 
		{
		    
		}

	

		#region method

		public virtual void Init(
			Architecture arch
		)
		{
			
		}

		public virtual Endianness GetEndianness()
		{
			return Endianness.LittleEndian;
		}

		public virtual ulong GetAddressSize()
		{
			return 0;
		}

		public virtual ulong GetDefaultIntegerSize()
		{
			return 0;
		}

		public virtual ulong GetInstructionAlignment()
		{
			return 0;
		}

		public virtual ulong GetMaxInstructionLength()
		{
			return 0;
		}

		public virtual ulong GetOpcodeDisplayLength()
		{
			return 0;
		}

		public virtual Architecture? GetAssociatedArchitectureByAddress(
			ref ulong address
		)
		{
			return null;
		}

		public virtual InstructionInfo? GetInstructionInfo(
			byte[] data ,
			ulong address
		)
		{
			return null;
		}

		public virtual InstructionTextToken[] GetInstructionText(
			byte[] data ,
			ulong address ,
			out ulong length
		)
		{
			length = 0;
			
			return Array.Empty<InstructionTextToken>();
		}

		public virtual void FreeInstructionText(
			IntPtr tokens ,
			ulong count
		)
		{
			
		}

		public virtual ulong? GetInstructionLowLevelIL(
			byte[] data ,
			ulong address ,
			LowLevelILFunction il
		)
		{
			return null;
		}
		
		public virtual string GetRegisterName(
			RegisterIndex reg
		)
		{
			return string.Empty;
		}

		public virtual string getFlagName(
			FlagIndex flag
		)
		{
			return string.Empty;
		}

		public virtual string GetFlagWriteTypeName(
			uint flags
		)
		{
			return string.Empty;
		}

		public virtual string GetSemanticFlagClassName(
			uint semClass
		)
		{
			return string.Empty;
		}

		public virtual string GetSemanticFlagGroupName(
			uint semGroup
		)
		{
			return string.Empty;
		}
		
		public virtual uint[] GetFullWidthRegisters()
		{
			return Array.Empty<uint>();
		}

		
		public virtual uint[] GetAllRegisters()
		{
			return Array.Empty<uint>();
		}

		
		public virtual uint[] GetAllFlags()
		{
			return Array.Empty<uint>();
		}

		
		public virtual uint[] GetAllFlagWriteTypes()
		{
			return Array.Empty<uint>();
		}

		
		public virtual uint[] GetAllSemanticFlagClasses()
		{
			return Array.Empty<uint>();
		}


		public virtual uint[] GetAllSemanticFlagGroups()
		{
			return Array.Empty<uint>();
		}

		public virtual FlagRole GetFlagRole(
			uint flag ,
			uint semClass
		)
		{
			return 0;
		}

		public virtual uint[] GetFlagsRequiredForFlagCondition(
			LowLevelILFlagCondition cond ,
			uint semClass
		)
		{
			return Array.Empty<uint>();
		}

		public virtual uint[] GetFlagsRequiredForSemanticFlagGroup(
			uint semGroup
		)
		{
			return Array.Empty<uint>();
		}

		public virtual FlagConditionForSemanticClass[] GetFlagConditionsForSemanticFlagGroup(
			uint semGroup
		)
		{
			return Array.Empty<FlagConditionForSemanticClass>();
		}

		public virtual void FreeFlagConditionsForSemanticFlagGroup(
			IntPtr conditions
		)
		{
			
		}

		public virtual uint[] GetFlagsWrittenByFlagWriteType(
			uint writeType
		)
		{
			return Array.Empty<uint>();
		}

		public virtual uint GetSemanticClassForFlagWriteType(
			uint writeType
		)
		{
			return 0;
		}

		public virtual ulong GetFlagWriteLowLevelIL(
			LowLevelILOperation op ,
			ulong size ,
			uint flagWriteType ,
			uint flag ,
			RegisterOrConstant[] operands ,
			LowLevelILFunction il
		)
		{
			return 0;
		}

		public virtual ulong GetFlagConditionLowLevelIL(
			LowLevelILFlagCondition cond ,
			uint semClass ,
			LowLevelILFunction il
		)
		{
			return 0;
		}

		public virtual ulong GetSemanticFlagGroupLowLevelIL(
			uint semGroup ,
			LowLevelILFunction il
		)
		{
			return 0;
		}

		public virtual void FreeRegisterList(
			IntPtr regs
		)
		{
			
		}

		public virtual RegisterInfo GetRegisterInfo(
			uint reg
		)
		{
			return new RegisterInfo();
		}

		public virtual uint GetStackPointerRegister()
		{
			return 0;
		}

		public virtual uint GetLinkRegister()
		{
			return 0;
		}

		public virtual uint[] GetGlobalRegisters()
		{
			return Array.Empty<uint>();
		}

		public virtual uint[] GetSystemRegisters()
		{
			return Array.Empty<uint>();
		}

		public virtual string GetRegisterStackName(
			uint regStack
		)
		{
			return string.Empty;
		}
		
		public virtual uint[] GetAllRegisterStacks()
		{
			return Array.Empty<uint>();
		}

		public virtual RegisterStackInfo GetRegisterStackInfo(
			uint regStack
		)
		{
			return new RegisterStackInfo();
		}

		public virtual IntrinsicClass GetIntrinsicClass(
			uint intrinsic
		)
		{
			return 0;
		}

		public virtual string GetIntrinsicName(
			uint intrinsic
		)
		{
			return string.Empty;
		}

		public virtual uint[] GetAllIntrinsics()
		{
			return Array.Empty<uint>();
		}
		
		public virtual NameAndType[] GetIntrinsicInputs(
			uint intrinsic
		)
		{
			return Array.Empty<NameAndType>();
		}

		public virtual void FreeNameAndTypeList(
			IntPtr nt ,
			ulong count
		)
		{
			
		}
		
		public virtual TypeWithConfidence[] GetIntrinsicOutputs(
			uint intrinsic
		)
		{
			return Array.Empty<TypeWithConfidence>();
		}
		
		public virtual void FreeTypeList(
			IntPtr types ,
			ulong count
		)
		{
			
		}

		public virtual bool CanAssemble()
		{
			return false;
		}

		public virtual byte[] Assemble(
			string code ,
			ulong address
		)
		{
			return Array.Empty<byte>();
		}

		public virtual bool IsNeverBranchPatchAvailable(
			byte[] data ,
			ulong address
		)
		{
			return false;
		}

		public virtual bool IsAlwaysBranchPatchAvailable(
			byte[] data ,
			ulong address
		)
		{
			return false;
		}

		public virtual bool IsInvertBranchPatchAvailable(
			byte[] data ,
			ulong address
		)
		{
			return false;
		}

		public virtual bool IsSkipAndReturnZeroPatchAvailable(
			byte[] data ,
			ulong address
		)
		{
			return false;
		}

		public virtual bool IsSkipAndReturnValuePatchAvailable(
			byte[] data ,
			ulong address
		)
		{
			return false;
		}

		public virtual bool ConvertToNop(
			byte[] data ,
			ulong address
		)
		{
			return false;
		}

		public virtual bool AlwaysBranch(
			byte[] data ,
			ulong address
		)
		{
			return false;
		}

		public virtual bool InvertBranch(
			byte[] data ,
			ulong address
		)
		{
			return false;
		}

		public virtual bool SkipAndReturnValue(
			byte[] data ,
			ulong address ,
			ulong value
		)
		{
			return false;
		}
		

		#endregion
    }
}
