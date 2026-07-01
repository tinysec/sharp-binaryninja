using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNHighLevelILInstruction 
	{
		/// <summary>
		/// BNHighLevelILOperation operation
		/// </summary>
		internal HighLevelILOperation operation;
		
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
		
		/// <summary>
		/// uint64_t parent
		/// </summary>
		internal ulong parent;
	}
	
    public abstract class HighLevelILInstruction : 
	    INativeWrapper<BNHighLevelILInstruction> ,
	    IEquatable<HighLevelILInstruction>,
	    IComparable<HighLevelILInstruction>
    {
	    public HighLevelILOperation Operation { get; set; } = HighLevelILOperation.HLIL_NOP;
		
	    public uint Attributes { get;  } = 0;
	
	    public ulong Size { get;  } = 0;
		
	    public ulong RawParent { get;  } = 0;
		
	    public OperandIndex SourceOperand { get;  } = 0;
		
	    public ulong Address { get;  } = 0;
	
	    public ulong[] RawOperands { get;  } = Array.Empty<ulong>();
	    
		internal HighLevelILFunction ILFunction { get;  }

		public HighLevelILExpressionIndex ExpressionIndex { get;  } = 0;

		private static Dictionary<HighLevelILOperation,int> OperationOperands = new Dictionary<HighLevelILOperation,int> {
			{ HighLevelILOperation.HLIL_NOP, 0 },

			{ HighLevelILOperation.HLIL_BLOCK, 1 },                 // stmtList
			{ HighLevelILOperation.HLIL_IF, 3 },                    // cond, trueBlock, falseBlock
			{ HighLevelILOperation.HLIL_WHILE, 2 },                 // cond, body
			{ HighLevelILOperation.HLIL_DO_WHILE, 2 },              // body, cond
			{ HighLevelILOperation.HLIL_FOR, 4 },                   // init, cond, update, body
			{ HighLevelILOperation.HLIL_SWITCH, 3 },                // expr, caseList, default
			{ HighLevelILOperation.HLIL_CASE, 2 },                  // valueList, body
			{ HighLevelILOperation.HLIL_BREAK, 0 },
			{ HighLevelILOperation.HLIL_CONTINUE, 0 },
			{ HighLevelILOperation.HLIL_JUMP, 1 },                  // dest
			{ HighLevelILOperation.HLIL_RET, 1 },                   // values(list)
			{ HighLevelILOperation.HLIL_NORET, 0 },
			{ HighLevelILOperation.HLIL_GOTO, 1 },                  // label
			{ HighLevelILOperation.HLIL_LABEL, 1 },                 // labelId

			{ HighLevelILOperation.HLIL_VAR_DECLARE, 1 },           // var
			{ HighLevelILOperation.HLIL_VAR_INIT, 2 },              // var, value
			{ HighLevelILOperation.HLIL_ASSIGN, 2 },                // dst, src
			{ HighLevelILOperation.HLIL_ASSIGN_UNPACK, 2 },         // dstList, src
			{ HighLevelILOperation.HLIL_FORCE_VER, 1 },             // var
			{ HighLevelILOperation.HLIL_ASSERT, 1 },                // cond

			{ HighLevelILOperation.HLIL_VAR, 1 },                   // var
			{ HighLevelILOperation.HLIL_STRUCT_FIELD, 2 },          // base, fieldOffset/index
			{ HighLevelILOperation.HLIL_ARRAY_INDEX, 2 },           // base, index
			{ HighLevelILOperation.HLIL_SPLIT, 2 },                 // hi, lo
			{ HighLevelILOperation.HLIL_DEREF, 1 },                 // addr
			{ HighLevelILOperation.HLIL_DEREF_FIELD, 2 },           // addr, fieldOffset/index
			{ HighLevelILOperation.HLIL_ADDRESS_OF, 1 },            // var/expr

			{ HighLevelILOperation.HLIL_CONST, 1 },                 // value
			{ HighLevelILOperation.HLIL_CONST_DATA, 2 },            // state,value (RegisterValue)
			{ HighLevelILOperation.HLIL_CONST_PTR, 1 },             // ptr
			{ HighLevelILOperation.HLIL_EXTERN_PTR, 2 },            // base, offset
			{ HighLevelILOperation.HLIL_FLOAT_CONST, 1 },           // fpBits
			{ HighLevelILOperation.HLIL_IMPORT, 1 },                // symbol/id

			{ HighLevelILOperation.HLIL_ADD, 2 },
			{ HighLevelILOperation.HLIL_ADC, 2 },
			{ HighLevelILOperation.HLIL_SUB, 2 },
			{ HighLevelILOperation.HLIL_SBB, 2 },
			{ HighLevelILOperation.HLIL_AND, 2 },
			{ HighLevelILOperation.HLIL_OR, 2 },
			{ HighLevelILOperation.HLIL_XOR, 2 },
			{ HighLevelILOperation.HLIL_LSL, 2 },
			{ HighLevelILOperation.HLIL_LSR, 2 },
			{ HighLevelILOperation.HLIL_ASR, 2 },
			{ HighLevelILOperation.HLIL_ROL, 2 },
			{ HighLevelILOperation.HLIL_RLC, 2 },
			{ HighLevelILOperation.HLIL_ROR, 2 },
			{ HighLevelILOperation.HLIL_RRC, 2 },
			{ HighLevelILOperation.HLIL_MUL, 2 },
			{ HighLevelILOperation.HLIL_MULU_DP, 2 },
			{ HighLevelILOperation.HLIL_MULS_DP, 2 },
			{ HighLevelILOperation.HLIL_DIVU, 2 },
			{ HighLevelILOperation.HLIL_DIVU_DP, 2 },
			{ HighLevelILOperation.HLIL_DIVS, 2 },
			{ HighLevelILOperation.HLIL_DIVS_DP, 2 },
			{ HighLevelILOperation.HLIL_MODU, 2 },
			{ HighLevelILOperation.HLIL_MODU_DP, 2 },
			{ HighLevelILOperation.HLIL_MODS, 2 },
			{ HighLevelILOperation.HLIL_MODS_DP, 2 },
			{ HighLevelILOperation.HLIL_NEG, 1 },
			{ HighLevelILOperation.HLIL_NOT, 1 },
			{ HighLevelILOperation.HLIL_SX, 2 },                    // value, fromSize/type
			{ HighLevelILOperation.HLIL_ZX, 2 },                    // value, fromSize/type
			{ HighLevelILOperation.HLIL_LOW_PART, 2 },              // value, toSize/type

			{ HighLevelILOperation.HLIL_CALL, 3 },                  // dest, params(list), outputs(list)

			{ HighLevelILOperation.HLIL_CMP_E, 2 },
			{ HighLevelILOperation.HLIL_CMP_NE, 2 },
			{ HighLevelILOperation.HLIL_CMP_SLT, 2 },
			{ HighLevelILOperation.HLIL_CMP_ULT, 2 },
			{ HighLevelILOperation.HLIL_CMP_SLE, 2 },
			{ HighLevelILOperation.HLIL_CMP_ULE, 2 },
			{ HighLevelILOperation.HLIL_CMP_SGE, 2 },
			{ HighLevelILOperation.HLIL_CMP_UGE, 2 },
			{ HighLevelILOperation.HLIL_CMP_SGT, 2 },
			{ HighLevelILOperation.HLIL_CMP_UGT, 2 },
			{ HighLevelILOperation.HLIL_TEST_BIT, 2 },
			{ HighLevelILOperation.HLIL_BOOL_TO_INT, 1 },
			{ HighLevelILOperation.HLIL_ADD_OVERFLOW, 2 },

			{ HighLevelILOperation.HLIL_SYSCALL, 2 },               // params(list), outputs(list)
			{ HighLevelILOperation.HLIL_TAILCALL, 2 },              // dest, params(list)
			{ HighLevelILOperation.HLIL_INTRINSIC, 3 },             // intrinsicId, params(list), outputs(list)
			{ HighLevelILOperation.HLIL_BP, 0 },
			{ HighLevelILOperation.HLIL_TRAP, 1 },                  // code
			{ HighLevelILOperation.HLIL_UNDEF, 0 },
			{ HighLevelILOperation.HLIL_UNIMPL, 0 },
			{ HighLevelILOperation.HLIL_UNIMPL_MEM, 1 },            // addr

			{ HighLevelILOperation.HLIL_FADD, 2 },
			{ HighLevelILOperation.HLIL_FSUB, 2 },
			{ HighLevelILOperation.HLIL_FMUL, 2 },
			{ HighLevelILOperation.HLIL_FDIV, 2 },
			{ HighLevelILOperation.HLIL_FSQRT, 1 },
			{ HighLevelILOperation.HLIL_FNEG, 1 },
			{ HighLevelILOperation.HLIL_FABS, 1 },
			{ HighLevelILOperation.HLIL_FLOAT_TO_INT, 2 },          // value, mode/size
			{ HighLevelILOperation.HLIL_INT_TO_FLOAT, 2 },          // value, mode/size
			{ HighLevelILOperation.HLIL_FLOAT_CONV, 2 },            // value, toType
			{ HighLevelILOperation.HLIL_ROUND_TO_INT, 2 },          // value, mode
			{ HighLevelILOperation.HLIL_FLOOR, 1 },
			{ HighLevelILOperation.HLIL_CEIL, 1 },
			{ HighLevelILOperation.HLIL_FTRUNC, 1 },
			{ HighLevelILOperation.HLIL_FCMP_E, 2 },
			{ HighLevelILOperation.HLIL_FCMP_NE, 2 },
			{ HighLevelILOperation.HLIL_FCMP_LT, 2 },
			{ HighLevelILOperation.HLIL_FCMP_LE, 2 },
			{ HighLevelILOperation.HLIL_FCMP_GE, 2 },
			{ HighLevelILOperation.HLIL_FCMP_GT, 2 },
			{ HighLevelILOperation.HLIL_FCMP_O, 2 },
			{ HighLevelILOperation.HLIL_FCMP_UO, 2 },

			{ HighLevelILOperation.HLIL_UNREACHABLE, 0 },

			{ HighLevelILOperation.HLIL_WHILE_SSA, 2 },             // cond, body
			{ HighLevelILOperation.HLIL_DO_WHILE_SSA, 2 },          // body, cond
			{ HighLevelILOperation.HLIL_FOR_SSA, 4 },               // init, cond, update, body
			{ HighLevelILOperation.HLIL_VAR_INIT_SSA, 2 },          // var(versioned), value
			{ HighLevelILOperation.HLIL_ASSIGN_MEM_SSA, 3 },        // dst(addr), src, dstMem
			{ HighLevelILOperation.HLIL_ASSIGN_UNPACK_MEM_SSA, 3 }, // dstList, src, dstMem
			{ HighLevelILOperation.HLIL_FORCE_VER_SSA, 2 },         // var, newVersion
			{ HighLevelILOperation.HLIL_ASSERT_SSA, 2 },            // cond, mem/reg ver

			{ HighLevelILOperation.HLIL_VAR_SSA, 2 },               // var, version
			{ HighLevelILOperation.HLIL_ARRAY_INDEX_SSA, 3 },       // base, index, srcMem
			{ HighLevelILOperation.HLIL_DEREF_SSA, 2 },             // addr, srcMem
			{ HighLevelILOperation.HLIL_DEREF_FIELD_SSA, 3 },       // addr, field, srcMem

			{ HighLevelILOperation.HLIL_CALL_SSA, 4 },              // dest, params(list), outputs(list), srcMem
			{ HighLevelILOperation.HLIL_SYSCALL_SSA, 3 },           // params(list), outputs(list), srcMem
			{ HighLevelILOperation.HLIL_INTRINSIC_SSA, 4 },         // intrinsicId, params(list), outputs(list), srcMem

			{ HighLevelILOperation.HLIL_VAR_PHI, 1 },               // varVersions(list)
			{ HighLevelILOperation.HLIL_MEM_PHI, 1 },               // memVersions(list)

			{ HighLevelILOperation.HLIL_ABS, 1 },                   // src
			{ HighLevelILOperation.HLIL_BSWAP, 1 },                 // src
			{ HighLevelILOperation.HLIL_CLS, 1 },                   // src
			{ HighLevelILOperation.HLIL_CLZ, 1 },                   // src
			{ HighLevelILOperation.HLIL_CTZ, 1 },                   // src
			{ HighLevelILOperation.HLIL_POPCNT, 1 },                // src
			{ HighLevelILOperation.HLIL_RBIT, 1 },                  // src
			{ HighLevelILOperation.HLIL_MAXS, 2 },                  // left, right
			{ HighLevelILOperation.HLIL_MAXU, 2 },                  // left, right
			{ HighLevelILOperation.HLIL_MINS, 2 },                  // left, right
			{ HighLevelILOperation.HLIL_MINU, 2 },                  // left, right
		};

		
		internal HighLevelILInstruction(
			HighLevelILFunction ilFunction ,
			HighLevelILExpressionIndex expressionIndex 
		) : this( 
			ilFunction , 
			expressionIndex ,
			NativeMethods.BNGetHighLevelILByIndex(
				ilFunction.DangerousGetHandle() , 
				expressionIndex,
				false
			)
		)
		{
			
		}
		
		internal HighLevelILInstruction(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) 
		{
			this.Operation = native.operation ;
			this.Attributes = native.attributes ;
			this.Size = native.size ;
			this.RawParent = native.parent;
			this.Address = native.address ;
			this.SourceOperand = (OperandIndex)native.sourceOperand;
			
			HighLevelILInstruction.OperationOperands.TryGetValue(
				this.Operation ,
				out int operandCount
			);
		
			if (0 == operandCount)
			{
				this.RawOperands = Array.Empty<ulong>();
			}
			else
			{
				this.RawOperands = new ulong[operandCount];
					
				for (int i = 0; i < operandCount; i++)
				{
					unsafe
					{
						this.RawOperands[i] = native.operands[i] ;
					}
				}
			}
			
			this.ILFunction = ilFunction;
			
			this.ExpressionIndex = expressionIndex;
		}
		
		public BNHighLevelILInstruction ToNative()
		{
			BNHighLevelILInstruction native = new BNHighLevelILInstruction()
			{
				operation = this.Operation ,
				attributes = this.Attributes ,
				sourceOperand = (uint)this.SourceOperand ,
				size = this.Size ,
				address = this.Address ,
				parent = this.RawParent 
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
		
		internal static HighLevelILInstruction FromExpressionIndex(
			HighLevelILFunction ilFunction , 
			HighLevelILExpressionIndex expressionIndex,
			bool asFullAst = true
		)
		{
			BNHighLevelILInstruction native = NativeMethods.BNGetHighLevelILByIndex(
				ilFunction.DangerousGetHandle() , 
				expressionIndex,
				asFullAst
			);

			switch (native.operation)
			{
				case HighLevelILOperation.HLIL_NOP:
				{
					return new HLILNop(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_BLOCK:
				{
					return new HLILBlock(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_IF:
				{
					return new HLILIf(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_WHILE:
				{
					return new HLILWhile(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_DO_WHILE:
				{
					return new HLILDoWhile(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_FOR:
				{
					return new HLILFor(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_SWITCH:
				{
					return new HLILSwitch(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_CASE:
				{
					return new HLILCase(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_BREAK:
				{
					return new HLILBreak(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_CONTINUE:
				{
					return new HLILContinue(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_JUMP:
				{
					return new HLILJump(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_RET:
				{
					return new HLILReturn(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_NORET:
				{
					return new HLILNoReturn(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_GOTO:
				{
					return new HLILGoto(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_LABEL:
				{
					return new HLILLabel(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_VAR_DECLARE:
				{
					return new HLILVariableDeclare(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_VAR_INIT:
				{
					return new HLILVariableInit(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_ASSIGN:
				{
					return new HLILAssign(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_ASSIGN_UNPACK:
				{
					return new HLILAssignUnpack(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_FORCE_VER:
				{
					return new HLILForceVersion(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_ASSERT:
				{
					return new HLILAssert(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_VAR:
				{
					return new HLILVariable(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_STRUCT_FIELD:
				{
					return new HLILStructField(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_ARRAY_INDEX:
				{
					return new HLILArrayIndex(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_SPLIT:
				{
					return new HLILSplit(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_DEREF:
				{
					return new HLILDeref(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_DEREF_FIELD:
				{
					return new HLILDerefField(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_ADDRESS_OF:
				{
					return new HLILAddressOf(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_CONST:
				{
					return new HLILConst(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_CONST_DATA:
				{
					return new HLILConstData(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_CONST_PTR:
				{
					return new HLILConstPointer(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_EXTERN_PTR:
				{
					return new HLILExternPointer(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_FLOAT_CONST:
				{
					return new HLILFloatConst(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_IMPORT:
				{
					return new HLILImport(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_ADD:
				{
					return new HLILAdd(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_ADC:
				{
					return new HLILAddCarry(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_SUB:
				{
					return new HLILSub(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_SBB:
				{
					return new HLILSubBorrow(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_AND:
				{
					return new HLILAnd(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_OR:
				{
					return new HLILOr(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_XOR:
				{
					return new HLILXor(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_LSL:
				{
					return new HLILLogicalShiftLef(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_LSR:
				{
					return new HLILLogicalShiftRight(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_ASR:
				{
					return new HlilArithmeticShiftRight(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_ROL:
				{
					return new HLILRotateLeft(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_RLC:
				{
					return new HLILRotateLeftCarry(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_ROR:
				{
					return new HLILRotateRight(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_RRC:
				{
					return new HLILRotateRightCarry(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_MUL:
				{
					return new HLILMul(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_MULU_DP:
				{
					// HLILMulDp
					return new HLILMulUnsignedDoublePrecision(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_MULS_DP:
				{
					return new HLILMulSignedDoublePrecision(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_DIVU:
				{
					return new HLILDivUnsigned(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_DIVU_DP:
				{
					return new HLILDivUnsignedDoublePrecision(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_DIVS:
				{
					return new HLILDivSigned(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_DIVS_DP:
				{
					return new HLILDivSignedDoublePrecision(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_MODU:
				{
					return new HLILModUnsigned(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_MODU_DP:
				{
					return new HLILModUnsignedDoublePrecision(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_MODS:
				{
					return new HlilModSigned(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_MODS_DP:
				{
					return new HLILModSignedDoublePrecision(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_NEG:
				{
					return new HLILNeg(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_NOT:
				{
					return new HLILNot(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_SX:
				{
					return new HLILSignExtend(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_ZX:
				{
					return new HLILZeroExtend(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_LOW_PART:
				{
					return new HLILLowPart(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_CALL:
				{
					return new HLILCall(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_CMP_E:
				{
					return new HLILEqual(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_CMP_NE:
				{
					return new HLILNotEqual(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_CMP_SLT:
				{
					return new HLILSignedLessThan(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_CMP_ULT:
				{
					return new HLILUnsignedLessThan(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_CMP_SLE:
				{
					return new HLILSignedLessEqual(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_CMP_ULE:
				{
					return new HLILUnsignedLessEqual(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_CMP_SGE:
				{
					return new HLILSignedGreaterEqual(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_CMP_UGE:
				{
					return new HLILUnsignedLessEqual(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_CMP_SGT:
				{
					return new HLILSignedGreaterThan(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_CMP_UGT:
				{
					return new HLILUnsignedGreaterThan(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_TEST_BIT:
				{
					return new HLILTestBit(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_BOOL_TO_INT:
				{
					return new HLILBoolToInt(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_ADD_OVERFLOW:
				{
					return new HLILAddOverflow(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_SYSCALL:
				{
					return new HLILSysCall(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_TAILCALL:
				{
					return new HLILTailCall(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_INTRINSIC:
				{
					return new HLILIntrinsic(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_BP:
				{
					return new HLILBreakpoint(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_TRAP:
				{
					return new HLILTrap(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_UNDEF:
				{
					return new HLILUndefined(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_UNIMPL:
				{
					return new HLILUnimplemented(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_UNIMPL_MEM:
				{
					return new HLILUnimplementedMemory(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_FADD:
				{
					return new HLILFloatAdd(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_FSUB:
				{
					return new HLILFloatSub(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_FMUL:
				{
					return new HLILFloatMul(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_FDIV:
				{
					return new HLILFloatDiv(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_FSQRT:
				{
					return new HLILFloatSquareRoot(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_FNEG:
				{
					return new HLILFloatNeg(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_FABS:
				{
					return new HLILFloatAbs(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_FLOAT_TO_INT:
				{
					return new HLILFloatToInt(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_INT_TO_FLOAT:
				{
					return new HLILIntToFloat(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_FLOAT_CONV:
				{
					return new HLILFloatConvert(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_ROUND_TO_INT:
				{
					return new HLILRoundToInt(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_FLOOR:
				{
					return new HLILFloor(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_CEIL:
				{
					return new HLILCeil(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_FTRUNC:
				{
					return new HLILFloatTrunc(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_FCMP_E:
				{
					return new HLILFloatEqual(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_FCMP_NE:
				{
					return new HLILFloatNotEqual(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_FCMP_LT:
				{
					return new HLILFloatLessThan(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_FCMP_LE:
				{
					return new HLILFloatLessEqual(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_FCMP_GE:
				{
					return new HLILFloatGreaterThan(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_FCMP_GT:
				{
					return new HLILFloatGreaterThan(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_FCMP_O:
				{
					return new HLILFloatCompareOrdered(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_FCMP_UO:
				{
					return new HLILFloatCompareUnordered(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_UNREACHABLE:
				{
					return new HLILUnreachable(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_WHILE_SSA:
				{
					return new HLILWhileSSA(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_DO_WHILE_SSA:
				{
					return new HLILDoWhileSSA(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_FOR_SSA:
				{
					return new HLILForSSA(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_VAR_INIT_SSA:
				{
					return new HLILVariableInitSSA(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_ASSIGN_MEM_SSA:
				{
					return new HLILAssignMemorySSA(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_ASSIGN_UNPACK_MEM_SSA:
				{
					return new HLILAssignUnpackMemorySSA(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_FORCE_VER_SSA:
				{
					return new HLILForceVersionSSA(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_ASSERT_SSA:
				{
					return new HLILAssertSSA(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_VAR_SSA:
				{
					return new HLILVariableSSA(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_ARRAY_INDEX_SSA:
				{
					return new HLILArrayIndexSSA(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_DEREF_SSA:
				{
					return new HLILDerefSSA(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_DEREF_FIELD_SSA:
				{
					return new HLILDerefFieldSSA(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_CALL_SSA:
				{
					return new HLILCallSSA(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_SYSCALL_SSA:
				{
					return new HLILSysCallSSA(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_INTRINSIC_SSA:
				{
					return new HLILIntrinsicSSA(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_VAR_PHI:
				{
					return new HLILVariablePhi(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_MEM_PHI:
				{
					return new HLILMemoryPhi(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_ABS:
				case HighLevelILOperation.HLIL_BSWAP:
				case HighLevelILOperation.HLIL_CLS:
				case HighLevelILOperation.HLIL_CLZ:
				case HighLevelILOperation.HLIL_CTZ:
				case HighLevelILOperation.HLIL_POPCNT:
				case HighLevelILOperation.HLIL_RBIT:
				{
					return new HLILGenericUnary(ilFunction, expressionIndex , native);
				}
				case HighLevelILOperation.HLIL_MAXS:
				case HighLevelILOperation.HLIL_MAXU:
				case HighLevelILOperation.HLIL_MINS:
				case HighLevelILOperation.HLIL_MINU:
				{
					return new HLILGenericBinary(ilFunction, expressionIndex , native);
				}
				default:
				{
					// Unknown / not-yet-typed operation (e.g. PASS_BY_REF, RETURN_BY_REF,
					// VAR_SSA_PARTIAL, or an op from a newer core). Degrade to a generic
					// wrapper instead of throwing so navigation/iteration stays robust.
					return new HLILGeneric(ilFunction, expressionIndex , native);
				}
			}
		}

		public override string ToString()
		{
			return this.ExpressionText;
		}
		
		public override bool Equals(object? other)
		{
			return Equals(other as HighLevelILInstruction);
		}

		public bool Equals(HighLevelILInstruction? other)
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

		public static bool operator ==(HighLevelILInstruction? left, HighLevelILInstruction? right)
		{
			if (left is null)
			{
				return right is null;
			}
			
			return left.Equals(right);
		}

		public static bool operator !=(HighLevelILInstruction? left, HighLevelILInstruction? right)
		{
			return !(left == right);
		}

		public int CompareTo(HighLevelILInstruction? other)
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
		
		public T[] GetOperandAsIntegerArray<T>(OperandIndex operand)
			where T : unmanaged
		{
			IntPtr arrayPointer = NativeMethods.BNHighLevelILGetOperandList(
				this.ILFunction.DangerousGetHandle() ,
				this.ExpressionIndex ,
				(ulong)this.RawOperands[(ulong)operand] ,
				out ulong arrayLength
			);

			return UnsafeUtils.TakeNumberArray<T>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNHighLevelILFreeOperandList
			);
		}
		
		public IDictionary<T,T> GetOperandAsIntegerMap<T>(OperandIndex operand)
			where T : unmanaged
		{
			IntPtr arrayPointer = NativeMethods.BNHighLevelILGetOperandList(
				this.ILFunction.DangerousGetHandle() ,
				this.ExpressionIndex ,
				(ulong)operand ,
				out ulong arrayLength
			);

			ulong[] keyAndValues = UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNHighLevelILFreeOperandList
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
		
		public HighLevelILLabel GetOperandAsLabel(OperandIndex operand)
		{
			return new HighLevelILLabel(
				this.ILFunction , 
				this.RawOperands[(ulong)operand]
			);
		}
		
		public HighLevelILVariable GetOperandAsVariable(OperandIndex operand)
		{
			return HighLevelILVariable.FromIdentifierEx(
				this.ILFunction , 
				this.RawOperands[(ulong)operand]
			);
		}
		
		public HighLevelILVariable[] GetOperandAsVariableList(OperandIndex operand1)
		{
			IntPtr arrayPointer = NativeMethods.BNHighLevelILGetOperandList(
				this.ILFunction.DangerousGetHandle() ,
				this.ExpressionIndex ,
				(ulong)operand1 ,
				out ulong arrayLength
			);

			ulong[] identifiers = UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNHighLevelILFreeOperandList
			);

			List<HighLevelILVariable> variables = new List<HighLevelILVariable>();
			
			foreach (ulong identifier in identifiers)
			{
				variables.Add(
					HighLevelILVariable.FromIdentifierEx(this.ILFunction ,identifier)
				);
			}

			return variables.ToArray();
		}
		
		public HighLevelILSSAVariable GetOperandAsSSAVariable(
			OperandIndex operand1,
			OperandIndex operand2
		)
		{
			HighLevelILVariable ilVariable = HighLevelILVariable.FromIdentifierEx(
				this.ILFunction ,
				this.RawOperands[(ulong)operand1]
			);

			return new HighLevelILSSAVariable(
				ilVariable ,
				this.RawOperands[(ulong)operand2]
			);
		}

		public HighLevelILSSAVariable[] GetOperandAsSSAVariableList(OperandIndex operand1)
		{
			IntPtr arrayPointer = NativeMethods.BNHighLevelILGetOperandList(
				this.ILFunction.DangerousGetHandle() ,
				this.ExpressionIndex ,
				(ulong)operand1 ,
				out ulong arrayLength
			);

			ulong[] identifierAndVersions = UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNHighLevelILFreeOperandList
			);

			List<HighLevelILSSAVariable> variables = new List<HighLevelILSSAVariable>();

			for (int i = 0; i < identifierAndVersions.Length; i += 2)
			{
				ulong identifier = identifierAndVersions[i];
				ulong version = identifierAndVersions[i + 1];
				
				HighLevelILVariable variable = HighLevelILVariable.FromIdentifierEx(this.ILFunction ,identifier);
				
				variables.Add(
					new HighLevelILSSAVariable(variable ,version )
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
				NativeMethods.BNGetCachedHighLevelILPossibleValueSet(
					this.ILFunction.DangerousGetHandle() ,
					this.RawOperands[(ulong)operand]
				)
			);
		}
		
		public HighLevelILInstruction GetOperandAsExpression(OperandIndex operand)
		{
			return HighLevelILInstruction.FromExpressionIndex(
				this.ILFunction , 
				(HighLevelILExpressionIndex)this.RawOperands[(ulong)operand]
			);
		}

		public HighLevelILInstruction[] GetOperandAsExpressionList(
			OperandIndex operand
		)
		{
			IntPtr arrayPointer = NativeMethods.BNHighLevelILGetOperandList(
				this.ILFunction.DangerousGetHandle() ,
				this.ExpressionIndex ,
				(ulong)operand ,
				out ulong arrayLength
			);

			HighLevelILExpressionIndex[] expressionIndexes = UnsafeUtils.TakeNumberArray<HighLevelILExpressionIndex>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNHighLevelILFreeOperandList
			);

			List<HighLevelILInstruction> expressions = new List<HighLevelILInstruction>();
			
			foreach (HighLevelILExpressionIndex expressionIndex in expressionIndexes)
			{
				expressions.Add(
					HighLevelILInstruction.FromExpressionIndex(
						this.ILFunction ,
						expressionIndex
					)
				);
			}

			return expressions.ToArray();
		}
		
		public DisassemblyTextLine[] ExpressionLines
		{
			get
			{
				DisassemblySettings settings = DisassemblySettings.DefaultLinear();
				
				settings.SetOption(DisassemblyOption.HighLevelILLinearDisassembly , true);
				settings.SetOption(DisassemblyOption.IndentHLILBody , true);
				
				return this.GetExpressionText(
					false,
					settings
				);
			}
		}
		
		public string ExpressionText
		{
			get
			{
				StringBuilder builder = new StringBuilder();
				
				foreach (DisassemblyTextLine line in this.ExpressionLines)
				{
					builder.Append(line.ToString());
				}
			
				return builder.ToString();
			}
		}
		
		public DisassemblyTextLine[] GetExpressionText(
			bool asFullAst  = false,
			DisassemblySettings? settings = null
		)
		{
			IntPtr arrayPointer = NativeMethods.BNGetHighLevelILExprText(
				this.ILFunction.DangerousGetHandle() ,
				this.ExpressionIndex,
				asFullAst,
				out ulong arrayLength,
				null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			);

			return UnsafeUtils.TakeStructArrayEx<BNDisassemblyTextLine , DisassemblyTextLine>(
				arrayPointer ,
				arrayLength,
				DisassemblyTextLine.FromNative,
				NativeMethods.BNFreeDisassemblyTextLines
			);
		}

		public HighLevelILInstructionIndex InstructionIndex
		{
			get
			{
				return NativeMethods.BNGetHighLevelILInstructionForExpr(
					this.ILFunction.DangerousGetHandle() ,
					this.ExpressionIndex
				);
			}
		}

		public HighLevelILInstruction? SSAExpression
		{
			get
			{
				HighLevelILExpressionIndex index = NativeMethods.BNGetHighLevelILSSAExprIndex(
					this.ILFunction.DangerousGetHandle(),
					this.ExpressionIndex
				);
				
				if ((ulong)index >= this.ILFunction.ExpressionCount)
				{
					return null;
				}
				
				return this.ILFunction.GetExpression((HighLevelILExpressionIndex)index);
			}
		}
		
		public HighLevelILInstruction? NonSSAExpression
		{
			get
			{
				HighLevelILExpressionIndex index = NativeMethods.BNGetHighLevelILNonSSAExprIndex(
					this.ILFunction.DangerousGetHandle(),
					this.ExpressionIndex
				);
				
				if ((ulong)index >= this.ILFunction.ExpressionCount)
				{
					return null;
				}
				
				return this.ILFunction.GetExpression((HighLevelILExpressionIndex)index);
			}
		}
		
		public HighLevelILInstruction? SSAInstruction
		{
			get
			{
				HighLevelILInstructionIndex? index = NativeMethods.BNGetHighLevelILSSAInstructionIndex(
					this.ILFunction.DangerousGetHandle(),
					this.InstructionIndex
				);

				if ((ulong)index >= this.ILFunction.InstructionCount)
				{
					return null;
				}
				
				return this.ILFunction.GetInstruction((HighLevelILInstructionIndex)index);
			}
		}
		
		
		public HighLevelILInstruction? NonSSAInstruction
		{
			get
			{
				HighLevelILInstructionIndex? index = NativeMethods.BNGetHighLevelILNonSSAInstructionIndex(
					this.ILFunction.DangerousGetHandle(),
					this.InstructionIndex
				);

				if ((ulong)index >= this.ILFunction.InstructionCount)
				{
					return null;
				}
				
				return this.ILFunction.GetInstruction((HighLevelILInstructionIndex)index);
			}
		}

		public MediumLevelILInstruction? MediumLevelIL
		{
			get
			{
				MediumLevelILExpressionIndex index = NativeMethods.BNGetMediumLevelILExprIndexFromHighLevelIL(
					this.ILFunction.DangerousGetHandle() ,
					this.ExpressionIndex
				);

				if (null == this.ILFunction.MediumLevelIL)
				{
					return null;
				}
				
				if ((ulong)index >= this.ILFunction.MediumLevelIL.ExpressionCount)
				{
					return null;
				}
				
				return this.ILFunction.MediumLevelIL?.GetExpression(
					(MediumLevelILExpressionIndex)index
				);
			}
		}
		
		public MediumLevelILInstruction[] MediumLevelILs
		{
			get
			{
				MediumLevelILFunction? mediumLevelIl = this.ILFunction.MediumLevelIL;

				if (null == mediumLevelIl)
				{
					return Array.Empty<MediumLevelILInstruction>();
				}
				
				IntPtr arrayPointer = NativeMethods.BNGetMediumLevelILExprIndexesFromHighLevelIL(
					this.ILFunction.DangerousGetHandle() ,
					this.ExpressionIndex,
					out ulong arrayLength
				);

				ulong[] indexes = UnsafeUtils.TakeNumberArray<ulong>(
					arrayPointer ,
					arrayLength ,
					NativeMethods.BNFreeILInstructionList
				);
				
				List<MediumLevelILInstruction> targets = new List<MediumLevelILInstruction>();

				foreach (MediumLevelILExpressionIndex index in indexes)
				{
					targets.Add(mediumLevelIl.MustGetExpression(index));
				}
				
				return targets.ToArray();
			}
		}

		public DisassemblyTextLine[] GetLanguageRepresentationLinearLines(
			DisassemblySettings? settings = null ,
			string language = "Pseudo C",
			bool asFullAst = false
		)
		{
			LanguageRepresentationFunction? pseudo = this.ILFunction.GetLanguageRepresentation(language);

			if (null == pseudo)
			{
				return Array.Empty<DisassemblyTextLine>();
			}
			
			IntPtr arrayPointer = NativeMethods.BNGetLanguageRepresentationFunctionLinearLines(
				pseudo.DangerousGetHandle() ,
				this.ILFunction.DangerousGetHandle() ,
				this.ExpressionIndex ,
				null == settings ? IntPtr.Zero :  settings.DangerousGetHandle() ,
				asFullAst ,
				out ulong arrayLength
			);

			return UnsafeUtils.TakeStructArrayEx<BNDisassemblyTextLine , DisassemblyTextLine>(
				arrayPointer ,
				arrayLength ,
				DisassemblyTextLine.FromNative ,
				NativeMethods.BNFreeDisassemblyTextLines
			);
		}
		
		public DisassemblyTextLine[] PseudoCLinearLines
		{
			get
			{
				return this.GetLanguageRepresentationLinearLines();
			}
		}

		public string PseudoCLinearText
		{
			get
			{
				StringBuilder builder = new StringBuilder();

				foreach (DisassemblyTextLine line in this.PseudoCLinearLines)
				{
					builder.AppendLine(line.ToString());
				}
				
				return builder.ToString();
			}
		}
		
		public DisassemblyTextLine[] GetLanguageRepresentationExpressionLines(
			DisassemblySettings? settings = null,
			bool asFullAst = false,
			OperatorPrecedence precedence = OperatorPrecedence.TopLevelOperatorPrecedence,
			bool statement = false,
			string language = "Pseudo C"
		)
		{
			LanguageRepresentationFunction? pseudo = this.ILFunction.GetLanguageRepresentation(language);

			if (null == pseudo)
			{
				return Array.Empty<DisassemblyTextLine>();
			}
			
			ulong arrayLength = 0;
			
			IntPtr arrayPointer = NativeMethods.BNGetLanguageRepresentationFunctionExprText(
				pseudo.DangerousGetHandle() ,
				this.ILFunction.DangerousGetHandle() ,
				this.ExpressionIndex ,
				null == settings ? IntPtr.Zero :  settings.DangerousGetHandle() ,
				asFullAst ,
				precedence ,
				statement ,
				out arrayLength
			);

			return UnsafeUtils.TakeStructArrayEx<BNDisassemblyTextLine , DisassemblyTextLine>(
				arrayPointer ,
				arrayLength ,
				DisassemblyTextLine.FromNative ,
				NativeMethods.BNFreeDisassemblyTextLines
			);
		}

		public DisassemblyTextLine[] PseudoCExpressionLines
		{
			get
			{
				return this.GetLanguageRepresentationExpressionLines();
			}
		}

		public string PseudoCExpressionText
		{
			get
			{
				StringBuilder builder = new StringBuilder();

				foreach (DisassemblyTextLine line in this.PseudoCExpressionLines)
				{
					builder.AppendLine(line.ToString());
				}
				
				return builder.ToString();
			}
		}

		public HighLevelILBasicBlock? BasicBlock
		{
			get
			{
				return HighLevelILBasicBlock.TakeHandleEx(
					this.ILFunction,
					NativeMethods.BNGetHighLevelILBasicBlockForInstruction(
						this.ILFunction.DangerousGetHandle() ,
						this.InstructionIndex
					)
				);
			}
		}

		public RegisterValue Value
		{
			get
			{
				if (null == this.MediumLevelIL)
				{
					return new RegisterValue();
				}
				
				return this.MediumLevelIL.Value;
			}
		}
		
		public PossibleValueSet PossibleValues
		{
			get
			{
				if (null == this.MediumLevelIL)
				{
					return new PossibleValueSet();
				}
				
				return this.MediumLevelIL.PossibleValues;
			}
		}

		public PossibleValueSet GetPossibleValues(DataFlowQueryOption[] options)
		{
			if (null == this.MediumLevelIL)
			{
				return new PossibleValueSet();
			}
			
			return this.MediumLevelIL.GetPossibleValues(options);
		}
		
		public TypeWithConfidence Type
		{
			get
			{
				return TypeWithConfidence.FromNative(
					NativeMethods.BNGetHighLevelILExprType(
						this.ILFunction.DangerousGetHandle() ,
						this.ExpressionIndex
					)
				);
			}

			set
			{
				NativeMethods.BNSetHighLevelILExprType(
					this.ILFunction.DangerousGetHandle(), 
					this.ExpressionIndex, 
					value.ToNative()
				);
			}
		}

		public ulong SSAMemoryVersion
		{
			get
			{
				return NativeMethods.BNGetHighLevelILSSAMemoryVersionAtILInstruction(
					this.ILFunction.DangerousGetHandle(),
					this.InstructionIndex
				);
			}
		}

		public ulong GetSSAVariableVersion(Variable variable)
		{
			return NativeMethods.BNGetHighLevelILSSAVarVersionAtILInstruction(
				this.ILFunction.DangerousGetHandle() ,
				variable.ToNative() ,
				this.InstructionIndex
			);
		}

		public bool HasSideEffects
		{
			get
			{
				return NativeMethods.BNHighLevelILHasSideEffects(
					this.ILFunction.DangerousGetHandle(),
					this.ExpressionIndex
				);
			}
		}

		/// <summary>
		/// The <see cref="HighLevelILFunction"/> that owns this instruction.
		/// Mirrors Python <c>HighLevelILInstruction.function</c>.
		/// </summary>
		public HighLevelILFunction Function
		{
			get
			{
				return this.ILFunction;
			}
		}

		/// <summary>
		/// The text tokens of this expression (flattened from <see cref="ExpressionLines"/>).
		/// Mirrors Python <c>HighLevelILInstruction.tokens</c>.
		/// </summary>
		public InstructionTextToken[] Tokens
		{
			get
			{
				List<InstructionTextToken> tokens = new List<InstructionTextToken>();

				foreach (DisassemblyTextLine line in this.ExpressionLines)
				{
					tokens.AddRange(line.Tokens);
				}

				return tokens.ToArray();
			}
		}

		/// <summary>
		/// The parent expression of this instruction, or <c>null</c> at the top level.
		/// Mirrors Python <c>HighLevelILInstruction.parent</c>.
		/// </summary>
		public HighLevelILInstruction? Parent
		{
			get
			{
				if (this.RawParent >= this.ILFunction.ExpressionCount)
				{
					return null;
				}

				return this.ILFunction.GetExpression(
					(HighLevelILExpressionIndex)this.RawParent
				);
			}
		}

		/// <summary>
		/// The enclosing statement (instruction) that contains this expression.
		/// Mirrors Python <c>HighLevelILInstruction.instr</c>.
		/// </summary>
		public HighLevelILInstruction Instr
		{
			get
			{
				return this.ILFunction.MustGetInstruction(this.InstructionIndex);
			}
		}

		/// <summary>
		/// SSA form of this expression. Alias of <see cref="SSAExpression"/> to match
		/// the Python/C++ <c>ssa_form</c> naming.
		/// </summary>
		public HighLevelILInstruction? SsaForm
		{
			get
			{
				return this.SSAExpression;
			}
		}

		/// <summary>
		/// Non-SSA form of this expression. Alias of <see cref="NonSSAExpression"/> to
		/// match the Python/C++ <c>non_ssa_form</c> naming.
		/// </summary>
		public HighLevelILInstruction? NonSsaForm
		{
			get
			{
				return this.NonSSAExpression;
			}
		}

		/// <summary>
		/// The corresponding Low Level IL expression (via Medium Level IL), or
		/// <c>null</c>. Mirrors Python <c>HighLevelILInstruction.llil</c>.
		/// </summary>
		public LowLevelILInstruction? LowLevelIL
		{
			get
			{
				return this.MediumLevelIL?.LowLevelILInstruction;
			}
		}

		/// <summary>
		/// Every Low Level IL expression that maps to this High Level IL expression
		/// (via Medium Level IL). Mirrors Python <c>HighLevelILInstruction.llils</c>.
		/// </summary>
		public LowLevelILInstruction[] LowLevelILs
		{
			get
			{
				List<LowLevelILInstruction> targets = new List<LowLevelILInstruction>();

				foreach (MediumLevelILInstruction mediumLevelIL in this.MediumLevelILs)
				{
					LowLevelILInstruction? lowLevelIL = mediumLevelIL.LowLevelILInstruction;

					if (null != lowLevelIL)
					{
						targets.Add(lowLevelIL);
					}
				}

				return targets.ToArray();
			}
		}


    }
}