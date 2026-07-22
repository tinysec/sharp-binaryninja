using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNMediumLevelILInstruction
	{
		/// <summary>
		/// BNMediumLevelILOperation operation
		/// </summary>
		internal MediumLevelILOperation operation;

		/// <summary>
		/// uint32_t attributes
		/// </summary>
		internal uint attributes;

		/// <summary>
		/// uint32_t sourceOperand
		/// </summary>
		internal uint sourceOperand;

		/// <summary>
		/// uint64_t size
		/// </summary>
		internal ulong size;

		/// <summary>
		/// uint64_t[5] operands
		/// </summary>
		internal fixed ulong operands[5];

		/// <summary>
		/// uint64_t address
		/// </summary>
		internal ulong address;
	}

	public abstract partial class MediumLevelILInstruction
		: INativeWrapper<BNMediumLevelILInstruction>,
		IEquatable<MediumLevelILInstruction>,
		IComparable<MediumLevelILInstruction>
	{
		public MediumLevelILOperation Operation { get;  } = MediumLevelILOperation.MLIL_NOP;

		public uint Attributes { get;  } = 0;

		public ulong Size { get; } = 0;

		public OperandIndex SourceOperand { get; } = 0;
		
		public ulong Address { get;  } = 0;

		/// <summary>
		/// Gets a source-aware location suitable for MLIL translation builders.
		/// </summary>
		public SourceLocation Location
		{
			get
			{
				return SourceLocation.FromInstruction(this);
			}
		}
		
		/// <summary>
		/// Gets all five raw operand slots from the core instruction structure.
		/// </summary>
		/// <remarks>
		/// The native structure and the official Python binding always expose five slots, including
		/// unused trailing zeroes. Logical operand counts cannot be used here because list operands
		/// occupy a count slot and a cache-index slot.
		/// </remarks>
		public ulong[] RawOperands { get; } = Array.Empty<ulong>();

		// extra fields
		internal MediumLevelILFunction ILFunction { get;  }

		/// <summary>
		/// The <see cref="MediumLevelILFunction"/> that owns this instruction.
		/// Mirrors Python <c>MediumLevelILInstruction.function</c>.
		/// </summary>
		public MediumLevelILFunction Function
		{
			get
			{
				return this.ILFunction;
			}
		}

		public MediumLevelILExpressionIndex ExpressionIndex { get;  } = 0;

		internal MediumLevelILInstruction(
			MediumLevelILFunction ilFunction ,
			MediumLevelILExpressionIndex expressionIndex 
		) : this( 
			ilFunction , 
			expressionIndex ,
			NativeMethods.BNGetMediumLevelILByIndex(
				ilFunction.DangerousGetHandle() , 
				expressionIndex
			)
		)
		{
			
		}
		
		internal MediumLevelILInstruction(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		)
		{
			this.ILFunction = ilFunction;
			
			this.ExpressionIndex = expressionIndex;
		
			this.Operation = native.operation ;
			this.Attributes = native.attributes ;
			this.SourceOperand = (OperandIndex)native.sourceOperand;
			this.Size = native.size ;
			this.Address = native.address ;

			this.RawOperands = new ulong[5];

			for (int i = 0; i < this.RawOperands.Length; i++)
			{
				unsafe
			{
					this.RawOperands[i] = native.operands[i];
			}
			}
		}

		internal static MediumLevelILInstruction FromExpressionIndex(
			MediumLevelILFunction ilFunction , 
			MediumLevelILExpressionIndex expressionIndex)
		{
			BNMediumLevelILInstruction native = NativeMethods.BNGetMediumLevelILByIndex(
				ilFunction.DangerousGetHandle() , 
				(MediumLevelILExpressionIndex)expressionIndex
			);

			switch (native.operation)
			{
				case MediumLevelILOperation.MLIL_NOP:
				{
					return new MLILNop(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_SET_VAR:
				{
					return new MLILSetVariable(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_SET_VAR_FIELD:
				{
					return new MLILSetVariableField(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_SET_VAR_SPLIT:
				{
					return new MLILSetVariableSplit(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_ASSERT:
				{
					return new MLILAssert(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_FORCE_VER:
				{
					return new MLILForceVersion(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_LOAD:
				{
					return new MLILLoad(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_LOAD_STRUCT:
				{
					return new MLILLoadStruct(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_STORE:
				{
					return new MLILStore(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_STORE_STRUCT:
				{
					return new MLILStoreStruct(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_VAR:
				{
					return new MLILVariable(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_VAR_FIELD:
				{
					return new MLILVariableField(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_VAR_SPLIT:
				{
					return new MLILVariableSplit(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_ADDRESS_OF:
				{
					return new MLILAddressOf(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_ADDRESS_OF_FIELD:
				{
					return new MLILAddressOfField(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_CONST:
				{
					return new MLILConst(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_CONST_DATA:
				{
					return new MLILConstData(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_CONST_PTR:
				{
					return new MLILConstPointer(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_EXTERN_PTR:
				{
					return new MLILExternPointer(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_FLOAT_CONST:
				{
					return new MLILFloatConst(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_IMPORT:
				{
					return new MLILImport(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_ADD:
				{
					return new MLILAdd(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_ADC:
				{
					return new MLILAddCarry(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_SUB:
				{
					return new MLILSub(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_SBB:
				{
					return new MLILSubBorrow(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_AND:
				{
					return new MLILAnd(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_OR:
				{
					return new MLILOr(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_XOR:
				{
					return new MLILXor(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_LSL:
				{
					return new MLILLogicalShiftLeft(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_LSR:
				{
					return new MLILLogicalShiftRight(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_ASR:
				{
					return new MLILArithmeticShiftRight(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_ROL:
				{
					return new MLILRotateLeft(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_RLC:
				{
					return new MLILRotateLeftCarry(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_ROR:
				{
					return new MLILRotateRight(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_RRC:
				{
					return new MLILRotateRightCarry(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_MUL:
				{
					return new MLILMul(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_MULU_DP:
				{
					return new MLILMulUnsignedDoublePrecision(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_MULS_DP:
				{
					return new MLILMulSignedDoublePrecision(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_DIVU:
				{
					return new MLILDivUnsigned(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_DIVU_DP:
				{
					return new MLILDivUnsignedDoublePrecision(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_DIVS:
				{
					return new MLILDivSigned(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_DIVS_DP:
				{
					return new MLILDivSignedDoublePrecision(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_MODU:
				{
					return new MLILModUnsigned(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_MODU_DP:
				{
					return new MLILModUnsignedDoublePrecision(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_MODS:
				{
					return new MLILModSigned(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_MODS_DP:
				{
					return new MLILModSignedDoublePrecision(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_NEG:
				{
					return new MLILNeg(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_NOT:
				{
					return new MLILNot(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_SX:
				{
					return new MLILSignExtend(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_ZX:
				{
					return new MLILZeroExtend(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_LOW_PART:
				{
					return new MLILLowPart(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_JUMP:
				{
					return new MLILJump(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_JUMP_TO:
				{
					return new MLILJumpTo(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_RET_HINT:
				{
					return new MLILReturnHint(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_CALL:
				{
					return new MLILCall(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_CALL_UNTYPED:
				{
					return new MLILCallUntyped(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_CALL_OUTPUT:
				{
					return new MLILCallOutput(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_CALL_PARAM:
				{
					return new MLILCallParam(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_SEPARATE_PARAM_LIST:
				{
					return new MLILSeparateParamList(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_SHARED_PARAM_SLOT:
				{
					return new MLILSharedParamSlot(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_RET:
				{
					return new MLILReturn(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_NORET:
				{
					return new MLILNoReturn(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_IF:
				{
					return new MLILIf(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_GOTO:
				{
					return new MLILGoto(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_CMP_E:
				{
					return new MLILEqual(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_CMP_NE:
				{
					return new MLILNotEqual(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_CMP_SLT:
				{
					return new MLILSignedLessThan(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_CMP_ULT:
				{
					return new MLILUnsignedLessThan(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_CMP_SLE:
				{
					return new MLILSignedLessOrEqual(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_CMP_ULE:
				{
					return new MLILUnsignedLessOrEqual(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_CMP_SGE:
				{
					return new MLILSignedGreaterOrEqual(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_CMP_UGE:
				{
					return new MLILUnsignedGreaterOrEqual(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_CMP_SGT:
				{
					return new MLILSignedGreaterThan(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_CMP_UGT:
				{
					return new MLILUnsignedGreaterThan(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_TEST_BIT:
				{
					return new MLILTestBit(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_BOOL_TO_INT:
				{
					return new MLILBoolToInt(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_ADD_OVERFLOW:
				{
					return new MLILAddOverflow(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_SYSCALL:
				{
					return new MLILSysCall(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_SYSCALL_UNTYPED:
				{
					return new MLILSysCallUntyped(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_TAILCALL:
				{
					return new MLILTailCall(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_TAILCALL_UNTYPED:
				{
					return new MLILTailCallUntyped(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_INTRINSIC:
				{
					return new MLILIntrinsic(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_FREE_VAR_SLOT:
				{
					return new MLILFreeVariableSlot(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_BP:
				{
					return new MLILBreakpoint(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_TRAP:
				{
					return new MLILTrap(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_UNDEF:
				{
					return new MLILUndefine(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_UNIMPL:
				{
					return new MLILUnimplemented(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_UNIMPL_MEM:
				{
					return new MLILUnimplementedMemory(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_FADD:
				{
					return new MLILFloatAdd(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_FSUB:
				{
					return new MLILFloatSub(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_FMUL:
				{
					return new MLILFloatMul(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_FDIV:
				{
					return new MLILFloatDiv(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_FSQRT:
				{
					return new MLILFloatSquareRoot(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_FNEG:
				{
					return new MLILFloatNeg(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_FABS:
				{
					return new MLILFloatAbs(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_FLOAT_TO_INT:
				{
					return new MLILFloatToInt(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_INT_TO_FLOAT:
				{
					return new MLILIntToFloat(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_FLOAT_CONV:
				{
					return new MLILFloatConvert(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_ROUND_TO_INT:
				{
					return new MLILRoundToInt(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_FLOOR:
				{
					return new MLILFloor(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_CEIL:
				{
					return new MLILCeil(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_FTRUNC:
				{
					return new MLILFloatTruncate(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_FCMP_E:
				{
					return new MLILFloatEqual(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_FCMP_NE:
				{
					return new MLILFloatNotEqual(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_FCMP_LT:
				{
					return new MLILFloatLessThan(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_FCMP_LE:
				{
					return new MLILFloatLessThanOrEqual(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_FCMP_GE:
				{
					return new MLILFloatGreaterOrEqual(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_FCMP_GT:
				{
					return new MLILFloatGreaterThan(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_FCMP_O:
				{
					return new MLILFloatCompareOrdered(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_FCMP_UO:
				{
					return new MLILFloatCompareUnordered(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_SET_VAR_SSA:
				{
					return new MLILSetVariableSSA(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_SET_VAR_SSA_FIELD:
				{
					return new MLILSetVariableSSAField(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_SET_VAR_SPLIT_SSA:
				{
					return new MLILSetVariableSplitSSA(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_SET_VAR_ALIASED:
				{
					return new MLILSetVariableAliased(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_SET_VAR_ALIASED_FIELD:
				{
					return new MLILSetVariableAliasedField(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_VAR_SSA:
				{
					return new MLILVariableSSA(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_VAR_SSA_FIELD:
				{
					return new MLILVariableSSAField(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_VAR_ALIASED:
				{
					return new MLILVariableAliased(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_VAR_ALIASED_FIELD:
				{
					return new MLILVariableAliasedField(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_VAR_SPLIT_SSA:
				{
					return new MLILVariableSplitSSA(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_ASSERT_SSA:
				{
					return new MLILAssertSSA(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_FORCE_VER_SSA:
				{
					return new MLILForceVersionSSA(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_CALL_SSA:
				{
					return new MLILCallSSA(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_CALL_UNTYPED_SSA:
				{
					return new MLILCallUntypedSSA(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_SYSCALL_SSA:
				{
					return new MLILSysCallSSA(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_SYSCALL_UNTYPED_SSA:
				{
					return new MLILSysCallUntypedSSA(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_TAILCALL_SSA:
				{
					return new MLILTailCallSSA(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_TAILCALL_UNTYPED_SSA:
				{
					return new MLILTailCallUntypedSSA(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_CALL_PARAM_SSA:
				{
					return new MLILCallParamSSA(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_CALL_OUTPUT_SSA:
				{
					return new MLILCallOutputSSA(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_MEMORY_INTRINSIC_OUTPUT_SSA:
				{
					return new MLILMemoryIntrinsicOutputSSA(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_LOAD_SSA:
				{
					return new MLILLoadSSA(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_LOAD_STRUCT_SSA:
				{
					return new MLILLoadStructSSA(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_STORE_SSA:
				{
					return new MLILStoreSSA(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_STORE_STRUCT_SSA:
				{
					return new MLILStoreStructSSA(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_INTRINSIC_SSA:
				{
					return new MLILIntrinsicSSA(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_MEMORY_INTRINSIC_SSA:
				{
					return new MLILMemoryIntrinsicSSA(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_FREE_VAR_SLOT_SSA:
				{
					return new MLILFreeVariableSlotSSA(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_VAR_PHI:
				{
					return new MLILVariablePhi(ilFunction ,  expressionIndex , native);
				}
				case MediumLevelILOperation.MLIL_MEM_PHI:
				{
					return new MLILMemoryPhi(ilFunction ,  expressionIndex , native);
				}
				default:
				{
					// Unknown / not-yet-typed operation (e.g. PASS_BY_REF, RETURN_BY_REF,
					// STORE_OUTPUT, VAR_OUTPUT_*, BLOCK_TO_EXPAND, or an op from a newer
					// core). Degrade to a generic wrapper instead of throwing so
					// navigation/iteration stays robust.
					return new MLILGeneric(ilFunction , expressionIndex , native);
				}
			}
		}
		
		public BNMediumLevelILInstruction ToNative()
		{
			BNMediumLevelILInstruction native = new BNMediumLevelILInstruction()
			{
				operation = this.Operation ,
				attributes = this.Attributes ,
				sourceOperand = (uint)this.SourceOperand ,
				size = this.Size ,
				address = this.Address ,
			};
			
			int count = Math.Min(this.RawOperands?.Length ?? 0, 5);

			if (this.RawOperands?.Length >= count)
			{
				for (int i = 0; i < count; i++)
				{
					unsafe
					{
						native.operands[i] = this.RawOperands[i];
					}
				}
			}
			
			return native;
		}
		
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			foreach (InstructionTextToken token in this.Tokens)
			{
				builder.Append(token.Text);
			}
			
			return builder.ToString();
		}
		
		
		public override bool Equals(object? other)
		{
			return Equals(other as MediumLevelILInstruction);
		}

		public bool Equals(MediumLevelILInstruction? other)
		{
			if (other is null)
			{
				return false;
			}

			if (ReferenceEquals(this , other))
			{
				return true;
			}

			if (this.ILFunction != other.ILFunction)
			{
				return false;
			}
			
			return this.ExpressionIndex == other.ExpressionIndex;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine<int,ulong>(
				this.ILFunction.GetHashCode(), 
				(ulong)this.ExpressionIndex
			);
		}

		public static bool operator ==(MediumLevelILInstruction? left, MediumLevelILInstruction? right)
		{
			if (left is null)
			{
				return right is null;
			}
			
			return left.Equals(right);
		}

		public static bool operator !=(MediumLevelILInstruction? left, MediumLevelILInstruction? right)
		{
			return !(left == right);
		}

		public int CompareTo(MediumLevelILInstruction? other)
		{
			if (other is null)
			{
				return 1;
			}
			
			int result = this.ILFunction.CompareTo(other.ILFunction);

			if (0 == result)
			{
				result = this.ExpressionIndex.CompareTo(other.ExpressionIndex);
			}
			
			return result;
		}

		
		public virtual MediumLevelILVariable[] VariablesRead
		{
			get
			{
				return Array.Empty<MediumLevelILVariable>();
			}
		}
		
		public virtual MediumLevelILVariable[] VariablesWrite
		{
			get
			{
				return Array.Empty<MediumLevelILVariable>();
			}
		}
		
		public virtual MediumLevelILSSAVariable[] SSAVariablesRead
		{
			get
			{
				return Array.Empty<MediumLevelILSSAVariable>();
			}
		}
		
		public virtual MediumLevelILSSAVariable[] SSAVariablesWrite
		{
			get
			{
				return Array.Empty<MediumLevelILSSAVariable>();
			}
		}
		
		public T[] GetOperandAsIntegerArray<T>(OperandIndex operand)
			where T : unmanaged
		{
			// BNMediumLevelILGetOperandList takes the operand INDEX (not the value at that slot):
			// the core walks the operation's operand layout to locate the list, so passing the
			// slot value -- as this method did before -- read an unrelated number and returned a
			// garbage list. Mirrors GetOperandAsIntegerMap below.
			IntPtr arrayPointer = NativeMethods.BNMediumLevelILGetOperandList(
				this.ILFunction.DangerousGetHandle() ,
				this.ExpressionIndex ,
				(ulong)operand ,
				out ulong arrayLength
			);

			return UnsafeUtils.TakeNumberArray<T>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNMediumLevelILFreeOperandList
			);
		}
		
		public IDictionary<T,T> GetOperandAsIntegerMap<T>(OperandIndex operand)
			where T : unmanaged
		{
			IntPtr arrayPointer = NativeMethods.BNMediumLevelILGetOperandList(
				this.ILFunction.DangerousGetHandle() ,
				this.ExpressionIndex ,
				(ulong)operand ,
				out ulong arrayLength
			);

			ulong[] keyAndValues = UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNMediumLevelILFreeOperandList
			);

			Dictionary<T,T> target = new Dictionary<T,T>();

			for (int i = 0; i < keyAndValues.Length; i += 2)
			{
				T key = UnsafeUtils.ForceConvert<ulong , T>(
					keyAndValues[i]
				);
				
				T value = UnsafeUtils.ForceConvert<ulong , T>(
					keyAndValues[i+1]
				);
				
				target[key] = value;
			}
			
			return target;
		}
		
		public float GetOperandAsFloat(OperandIndex operand)
		{
			return (float)BitConverter.UInt32BitsToSingle(
				(uint)this.RawOperands[(ulong)operand]
			);
		}
		
		public double GetOperandAsDouble(OperandIndex operand)
		{
			return BitConverter.UInt64BitsToDouble(this.RawOperands[(ulong)operand]);
		}

		/// <summary>
		/// Reads one raw operand slot as a signed integer, mirroring the integer operands exposed by
		/// Python MediumLevelILInstruction detailed_operands (e.g. StructField.offset,
		/// StoreSsa.dest_memory, JumpTo.targets).
		/// </summary>
		public long GetOperandAsInteger(OperandIndex operand)
		{
			return (long)this.RawOperands[(ulong)operand];
		}

		public RegisterValue GetOperandAsConstantData(
			OperandIndex operand1 ,
			OperandIndex operand2
		)
		{
			return new RegisterValue()
			{
				State = (RegisterValueType)this.RawOperands[(ulong)operand1] , 
				Value = (long)this.RawOperands[(ulong)operand2] , 
				Offset = 0 , 
				Size = this.Size
			};
		}
		
		public MediumLevelILVariable GetOperandAsVariable(OperandIndex operand)
		{
			return MediumLevelILVariable.FromIdentifier(
				this.ILFunction , 
				this.RawOperands[(ulong)operand]
			);
		}
		
		public MediumLevelILVariable[] GetOperandAsVariableList(OperandIndex operand1)
		{
			IntPtr arrayPointer = NativeMethods.BNMediumLevelILGetOperandList(
				this.ILFunction.DangerousGetHandle() ,
				this.ExpressionIndex ,
				(ulong)operand1 ,
				out ulong arrayLength
			);

			ulong[] identifiers = UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNMediumLevelILFreeOperandList
			);

			List<MediumLevelILVariable> variables = new List<MediumLevelILVariable>();
			
			foreach (ulong identifier in identifiers)
			{
				variables.Add(
					MediumLevelILVariable.FromIdentifier(this.ILFunction ,identifier)
				);
			}

			return variables.ToArray();
		}
		
		public MediumLevelILSSAVariable GetOperandAsSSAVariable(
			OperandIndex operand1,
			OperandIndex operand2
		)
		{
			MediumLevelILVariable ilVariable = MediumLevelILVariable.FromIdentifier(
				this.ILFunction ,
				this.RawOperands[(ulong)operand1]
			);

			return new MediumLevelILSSAVariable(
				ilVariable ,
				this.RawOperands[(ulong)operand2]
			);
		}

		public MediumLevelILSSAVariable[] GetOperandAsSSAVariableList(OperandIndex operand1)
		{
			IntPtr arrayPointer = NativeMethods.BNMediumLevelILGetOperandList(
				this.ILFunction.DangerousGetHandle() ,
				this.ExpressionIndex ,
				(ulong)operand1 ,
				out ulong arrayLength
			);

			ulong[] identifierAndVersions = UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNMediumLevelILFreeOperandList
			);

			List<MediumLevelILSSAVariable> variables = new List<MediumLevelILSSAVariable>();

			for (int i = 0; i < identifierAndVersions.Length; i += 2)
			{
				ulong identifier = identifierAndVersions[i];
				ulong version = identifierAndVersions[i + 1];
				
				MediumLevelILVariable variable = MediumLevelILVariable.FromIdentifier(this.ILFunction ,identifier);
				
				variables.Add(
					new MediumLevelILSSAVariable(variable ,version )
				);
			}
			
			return variables.ToArray();
		}
		
		public Intrinsic GetOperandAsIntrinsic(OperandIndex operand)
		{
			return new Intrinsic(
				this.ILFunction.OwnerFunction.Architecture , 
				(IntrinsicIndex)this.RawOperands[(ulong)operand]
			);
		}
		
		public PossibleValueSet GetOperandAsPossibleValueSet(OperandIndex operand)
		{
			return PossibleValueSet.TakeNative(
				NativeMethods.BNGetCachedMediumLevelILPossibleValueSet(
					this.ILFunction.DangerousGetHandle() ,
					this.RawOperands[(ulong)operand]
				)
			);
		}
		
		public MediumLevelILInstruction GetOperandAsExpression(OperandIndex operand)
		{
			return MediumLevelILInstruction.FromExpressionIndex(
				this.ILFunction , 
				(MediumLevelILExpressionIndex)this.RawOperands[(ulong)operand]
			);
		}

		public MediumLevelILInstruction[] GetOperandAsExpressionList(
			OperandIndex operand1
		)
		{
			IntPtr arrayPointer = NativeMethods.BNMediumLevelILGetOperandList(
				this.ILFunction.DangerousGetHandle() ,
				this.ExpressionIndex ,
				(ulong)operand1 ,
				out ulong arrayLength
			);

			MediumLevelILExpressionIndex[] expressionIndexes = UnsafeUtils.TakeNumberArray<MediumLevelILExpressionIndex>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNMediumLevelILFreeOperandList
			);

			List<MediumLevelILInstruction> expressions = new List<MediumLevelILInstruction>();
			
			foreach (MediumLevelILExpressionIndex expressionIndex in expressionIndexes)
			{
				expressions.Add(
					MediumLevelILInstruction.FromExpressionIndex(
						this.ILFunction ,
						expressionIndex
					)
				);
			}

			return expressions.ToArray();
		}
		
		public InstructionTextToken[] GetTokens(
			Architecture? arch = null,
			DisassemblySettings? settings = null
		)
		{
			if (null == arch)
			{
				arch = this.ILFunction.OwnerFunction.Architecture;
			}

			bool ok = NativeMethods.BNGetMediumLevelILExprText(
				this.ILFunction.DangerousGetHandle() ,
				null == arch ? IntPtr.Zero : arch.DangerousGetHandle(),
				this.ExpressionIndex,
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

		public MediumLevelILInstructionIndex InstructionIndex
		{
			get
			{
				return NativeMethods.BNGetMediumLevelILInstructionForExpr(
					this.ILFunction.DangerousGetHandle() ,
					this.ExpressionIndex
				);
			}
		}

		public InstructionTextToken[] Tokens
		{
			get
			{
				return this.GetTokens();
			}
		}

		public MediumLevelILBasicBlock? MediumLevelILBasicBlock
		{
			get
			{
				return MediumLevelILBasicBlock.TakeHandleEx(
					this.ILFunction,
					NativeMethods.BNGetMediumLevelILBasicBlockForInstruction(
						this.ILFunction.DangerousGetHandle() ,
						this.InstructionIndex
					)
				);
			}
		}

		public MediumLevelILExpressionIndex SSAExpressionIndex
		{
			get
			{
				return NativeMethods.BNGetMediumLevelILSSAExprIndex(
					this.ILFunction.DangerousGetHandle(),
					this.ExpressionIndex
				);
			}
		}

		public MediumLevelILInstruction SSAForm
		{
			get
			{
				return MediumLevelILInstruction.FromExpressionIndex(
					this.ILFunction.SSAForm ,
					this.SSAExpressionIndex
				);
			}
		}
		
		public MediumLevelILExpressionIndex NonSSAExpressionIndex
		{
			get
			{
				return NativeMethods.BNGetMediumLevelILNonSSAExprIndex(
					this.ILFunction.DangerousGetHandle(),
					this.ExpressionIndex
				);
			}
		}
		
		public MediumLevelILInstruction NonSSAForm
		{
			get
			{
				return MediumLevelILInstruction.FromExpressionIndex(
					this.ILFunction.NonSSAForm ,
					this.NonSSAExpressionIndex
				);
			}
		}

		public RegisterValue Value
		{
			get
			{
				return RegisterValue.FromNative(
					NativeMethods.BNGetMediumLevelILExprValue(
						this.ILFunction.DangerousGetHandle() ,
						this.ExpressionIndex
					)
				);
			}
		}
		
		public PossibleValueSet PossibleValues
		{
			get
			{
				return PossibleValueSet.TakeNative(
					NativeMethods.BNGetMediumLevelILPossibleExprValues(
						this.ILFunction.DangerousGetHandle() ,
						this.ExpressionIndex,
						Array.Empty<DataFlowQueryOption>(),
						0
					)
				);
			}
		}

		public PossibleValueSet GetPossibleValues(DataFlowQueryOption[] options)
		{
			return PossibleValueSet.TakeNative(
				NativeMethods.BNGetMediumLevelILPossibleExprValues(
					this.ILFunction.DangerousGetHandle() ,
					this.ExpressionIndex,
					options,
					(ulong)options.Length
				)
			);
		}
		
		public MediumLevelILLabel? Label
		{
			get
			{
				return MediumLevelILLabel.FromNativePointer(
					NativeMethods.BNGetLabelForMediumLevelILSourceInstruction(
						this.ILFunction.DangerousGetHandle() , 
						this.InstructionIndex
					)
				);
			}
		}

		public LowLevelILInstruction? LowLevelILExpression
		{
			get
			{
				return this.ILFunction.GetLowLevelILExpression(this.ExpressionIndex);
			}
		}
		
		public LowLevelILInstruction[] LowLevelILExpressions
		{
			get
			{
				return this.ILFunction.GetLowLevelILExpressions(this.ExpressionIndex);
			}
		}
		
		public LowLevelILInstruction? LowLevelILInstruction
		{
			get
			{
				return this.ILFunction.GetLowLevelILInstruction(this.InstructionIndex);
			}
		}
		
		// high
		public HighLevelILInstruction? HighLevelILExpression
		{
			get
			{
				return this.ILFunction.GetHighLevelILExpression(this.ExpressionIndex);
			}
		}
		
		public HighLevelILInstruction[] HighLevelILExpressions
		{
			get
			{
				return this.ILFunction.GetHighLevelILExpressions(this.ExpressionIndex);
			}
		}
		
		public HighLevelILInstruction? HighLevelILInstruction
		{
			get
			{
				return this.ILFunction.GetHighLevelILInstruction(this.InstructionIndex);
			}
		}

		/// <summary>
		/// The named, typed operands of this instruction, mirroring Python
		/// MediumLevelILInstruction.detailed_operands (mediumlevelil.py:495). Each entry pairs the
		/// operand name (e.g. "left", "src", "dest"), its value, its kind, and the Python type
		/// string. Operations with no operands return an empty list. This is the foundation for
		/// <see cref="Operands"/> and <see cref="Traverse{T}"/>.
		///
		/// See MediumLevelILDetailedOperandsTable: the call/syscall/intrinsic family's derived
		/// operands (output / params / output_dest_memory / params_src_memory) live inside nested
		/// CallOutput / CallParam / MemoryIntrinsicOutput sub-instructions and are served by the
		/// eight ILOperandKind.CallOutput* / CallParam* / MemoryIntrinsicOutput* variants, which
		/// this reader dispatches below.
		/// </summary>
		public virtual IList<ILOperand> DetailedOperands
		{
			get
			{
				if (false == MediumLevelILDetailedOperandsTable.Table.TryGetValue(
						this.Operation, out ILOperandDescriptor[]? descriptors))
				{
					return new List<ILOperand>();
				}

				List<ILOperand> result = new List<ILOperand>(descriptors.Length);

				foreach (ILOperandDescriptor descriptor in descriptors)
				{
					object? value = this.ReadDetailedOperandByKind(descriptor);
					result.Add(new ILOperand(
						descriptor.Name, value, descriptor.Kind, descriptor.TypeName));
				}

				return result;
			}
		}

		/// <summary>
		/// Reads one operand by its descriptor. SSA variables and constant data each occupy two raw
		/// slots: the identifier/state at <see cref="ILOperandDescriptor.RawIndex"/> and the
		/// version/value at <see cref="ILOperandDescriptor.SecondaryRawIndex"/> (which defaults to
		/// RawIndex + 1 when not set explicitly).
		/// </summary>
		private object? ReadDetailedOperandByKind(ILOperandDescriptor descriptor)
		{
			OperandIndex index = (OperandIndex)(ulong)descriptor.RawIndex;

			int secondary = descriptor.SecondaryRawIndex < 0
				? descriptor.RawIndex + 1
				: descriptor.SecondaryRawIndex;
			OperandIndex secondaryIndex = (OperandIndex)(ulong)secondary;

			switch (descriptor.Kind)
			{
				case ILOperandKind.Expression:
					return this.GetOperandAsExpression(index);

				case ILOperandKind.ExpressionList:
					return this.GetOperandAsExpressionList(index);

				case ILOperandKind.Integer:
					return this.GetOperandAsInteger(index);

				case ILOperandKind.IntegerList:
					return this.GetOperandAsIntegerArray<long>(index);

				case ILOperandKind.Variable:
					return this.GetOperandAsVariable(index);

				case ILOperandKind.VariableList:
					return this.GetOperandAsVariableList(index);

				case ILOperandKind.SSAVariable:
					return this.GetOperandAsSSAVariable(index, secondaryIndex);

				case ILOperandKind.SSAVariableList:
					return this.GetOperandAsSSAVariableList(index);

				case ILOperandKind.Float:
					if (4 == this.Size)
					{
						return (double)this.GetOperandAsFloat(index);
					}

					if (8 == this.Size)
					{
						return this.GetOperandAsDouble(index);
					}

					return (double)this.RawOperands[(ulong)index];

				case ILOperandKind.ConstantData:
					return this.GetOperandAsConstantData(index, secondaryIndex);

				case ILOperandKind.PossibleValueSet:
					return this.GetOperandAsPossibleValueSet(index);

				case ILOperandKind.Intrinsic:
					return this.GetOperandAsIntrinsic(index);

				// Sub-instruction-derived operands: the value lives inside a CallOutput / CallParam
				// / MemoryIntrinsicOutput wrapper expression at RawIndex, not in a flat raw slot.
				// GetOperandAsExpression returns the typed wrapper (FromExpressionIndex maps each
				// opcode to its wrapper class); cast and read the named property, which itself calls
				// the correct flat reader on the sub-instruction at its own slot.
				case ILOperandKind.CallOutputVariables:
					return ((MLILCallOutput)this.GetOperandAsExpression(index)).Destination;

				case ILOperandKind.CallParamExpressions:
					return ((MLILCallParam)this.GetOperandAsExpression(index)).Source;

				case ILOperandKind.CallOutputSsaVariables:
					return ((MLILCallOutputSSA)this.GetOperandAsExpression(index)).Destination;

				case ILOperandKind.CallOutputSsaMemory:
					return ((MLILCallOutputSSA)this.GetOperandAsExpression(index)).DestinationMemory;

				case ILOperandKind.CallParamSsaExpressions:
					return ((MLILCallParamSSA)this.GetOperandAsExpression(index)).Source;

				case ILOperandKind.CallParamSsaMemory:
					return ((MLILCallParamSSA)this.GetOperandAsExpression(index)).SourceMemory;

				case ILOperandKind.MemoryIntrinsicOutputSsaVariables:
					return ((MLILMemoryIntrinsicOutputSSA)this.GetOperandAsExpression(index)).Output;

				case ILOperandKind.MemoryIntrinsicOutputSsaMemory:
					return ((MLILMemoryIntrinsicOutputSSA)this.GetOperandAsExpression(index))
						.DestinationMemory;

				default:
					return null;
			}
		}

		/// <summary>
		/// The operand values of this instruction (the value component of
		/// <see cref="DetailedOperands"/>), mirroring Python MediumLevelILInstruction.operands
		/// (mediumlevelil.py:486). Prefer the named accessors (Source, Left, Condition, ...) when the
		/// operand kind is known at the call site.
		/// </summary>
		public IList<object?> Operands
		{
			get
			{
				IList<ILOperand> detailed = this.DetailedOperands;
				List<object?> result = new List<object?>(detailed.Count);

				foreach (ILOperand operand in detailed)
				{
					result.Add(operand.Value);
				}

				return result;
			}
		}

		/// <summary>
		/// Depth-first traversal of this instruction's operand tree, mirroring Python
		/// MediumLevelILInstruction.traverse (mediumlevelil.py:511). The callback is invoked on this
		/// node and every reachable sub-instruction; each non-null result is yielded. Pass a named
		/// method (method group) as the callback.
		///
		/// Python's MLIL traverse descends into every detailed operand unconditionally -- unlike
		/// HighLevelIL it has no blacklist, because MLIL branch targets are instruction/label indices
		/// rather than sub-instructions, so no operand leaves the enclosing instruction's scope. The
		/// <paramref name="shallow"/> parameter is kept for signature parity with
		/// <see cref="HighLevelILInstruction.Traverse{T}"/>; MLIL has no blacklisted names, so it has
		/// no effect here.
		/// </summary>
		/// <typeparam name="T">The result type; constrained to a reference type so null signals
		/// "do not yield".</typeparam>
		public IEnumerable<T> Traverse<T>(
			Func<MediumLevelILInstruction, T?> callback, bool shallow = true) where T : class
		{
			T? root = callback(this);

			if (null != root)
			{
				yield return root;
			}

			foreach (ILOperand operand in this.DetailedOperands)
			{
				if (shallow && MediumLevelILInstruction.ShallowTraversalBlacklist.Contains(operand.Name))
				{
					continue;
				}

				if (operand.Value is MediumLevelILInstruction subInstruction)
				{
					foreach (T nested in subInstruction.Traverse<T>(callback, shallow))
					{
						yield return nested;
					}
				}
				else if (operand.Value is MediumLevelILInstruction[] subList)
				{
					foreach (MediumLevelILInstruction item in subList)
					{
						foreach (T nested in item.Traverse<T>(callback, shallow))
						{
							yield return nested;
						}
					}
				}
			}
		}

		// Operand names to skip during shallow traversal. Empty for MLIL: Python's
		// MediumLevelILInstruction.traverse (mediumlevelil.py:511) has no blacklist, because MLIL
		// branch targets are indices/labels, not sub-instructions. Kept as the extension point so
		// the signature matches HighLevelILInstruction.Traverse and a future operand that should be
		// skipped can be added in one place.
		private static readonly HashSet<string> ShallowTraversalBlacklist =
			new HashSet<string>();

	}

}
