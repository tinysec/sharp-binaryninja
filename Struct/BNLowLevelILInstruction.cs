using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNLowLevelILInstruction 
	{
		/// <summary>
		/// BNLowLevelILOperation operation
		/// </summary>
		internal LowLevelILOperation operation;
		
		/// <summary>
		/// uint32_t attributes
		/// </summary>
		internal uint attributes;
		
		/// <summary>
		/// uint64_t size
		/// </summary>
		internal ulong size;
		
		/// <summary>
		/// uint32_t flags
		/// </summary>
		internal uint flags;
		
		/// <summary>
		/// uint32_t sourceOperand
		/// </summary>
		internal uint sourceOperand;
		
		/// <summary>
		/// uint64_t[4] operands
		/// </summary>
		internal fixed ulong operands[4];
		
		/// <summary>
		/// uint64_t address
		/// </summary>
		internal ulong address;
	}

    public abstract class LowLevelILInstruction 
	    : INativeWrapper<BNLowLevelILInstruction>,
	    IEquatable<LowLevelILInstruction>,
	    IComparable<LowLevelILInstruction>
    {
	    public LowLevelILOperation Operation { get;  } = LowLevelILOperation.LLIL_NOP;
		
		public uint Attributes { get;  } = 0;
		
		public ulong Size { get;  } = 0;
		
		public uint Flags { get;  } = 0;
		
		public OperandIndex SourceOperand { get;  } = 0;
		
		public ulong Address { get;  } = 0;
		
		public ulong[] RawOperands { get;  } = Array.Empty<ulong>();
		
		// related
		internal LowLevelILFunction ILFunction  { get;  }

		/// <summary>
		/// The <see cref="LowLevelILFunction"/> that owns this instruction.
		/// Mirrors Python <c>LowLevelILInstruction.function</c>.
		/// </summary>
		public LowLevelILFunction Function
		{
			get
			{
				return this.ILFunction;
			}
		}

		public LowLevelILExpressionIndex ExpressionIndex { get; } = 0;

		private static Dictionary<LowLevelILOperation,int> OperationOperands = new Dictionary<LowLevelILOperation,int> {
			{ LowLevelILOperation.LLIL_NOP, 0 },

			{ LowLevelILOperation.LLIL_SET_REG, 2 },                 // dstReg, src
			{ LowLevelILOperation.LLIL_SET_REG_SPLIT, 3 },           // dstHi, dstLo, src
			{ LowLevelILOperation.LLIL_SET_FLAG, 2 },                // flag, src
			{ LowLevelILOperation.LLIL_SET_REG_STACK_REL, 3 },       // dstRegStack, rel, src
			{ LowLevelILOperation.LLIL_REG_STACK_PUSH, 2 },          // regStack, src

			{ LowLevelILOperation.LLIL_ASSERT, 2 },  // sourceRegister, constraint
			{ LowLevelILOperation.LLIL_FORCE_VER, 1 },               // var/version

			{ LowLevelILOperation.LLIL_LOAD, 2 },                    // size, addr
			{ LowLevelILOperation.LLIL_STORE, 3 },                   // size, addr, value

			{ LowLevelILOperation.LLIL_PUSH, 2 },                    // size, src
			{ LowLevelILOperation.LLIL_POP, 1 },                     // size

			{ LowLevelILOperation.LLIL_REG, 1 },                     // reg
			{ LowLevelILOperation.LLIL_REG_SPLIT, 2 },               // regHi, regLo
			{ LowLevelILOperation.LLIL_REG_STACK_REL, 2 },           // regStack, rel
			{ LowLevelILOperation.LLIL_REG_STACK_POP, 2 },           // regStack, size
			{ LowLevelILOperation.LLIL_REG_STACK_FREE_REG, 1 },      // reg
			{ LowLevelILOperation.LLIL_REG_STACK_FREE_REL, 2 },      // regStack, rel

			{ LowLevelILOperation.LLIL_CONST, 2 },                   // size, value
			{ LowLevelILOperation.LLIL_CONST_PTR, 2 },               // size, addr
			{ LowLevelILOperation.LLIL_EXTERN_PTR, 3 },              // size, base, offset
			{ LowLevelILOperation.LLIL_FLOAT_CONST, 2 },             // size, fpBits

			{ LowLevelILOperation.LLIL_FLAG, 1 },                    // flag
			{ LowLevelILOperation.LLIL_FLAG_BIT, 2 },  // sourceFlag, bitIndex

			{ LowLevelILOperation.LLIL_ADD, 3 },                     // size, left, right
			{ LowLevelILOperation.LLIL_ADC, 3 },                     // size, left, right
			{ LowLevelILOperation.LLIL_SUB, 3 },                     // size, left, right
			{ LowLevelILOperation.LLIL_SBB, 3 },                     // size, left, right
			{ LowLevelILOperation.LLIL_AND, 3 },                     // size, left, right
			{ LowLevelILOperation.LLIL_OR, 3 },                      // size, left, right
			{ LowLevelILOperation.LLIL_XOR, 3 },                     // size, left, right
			{ LowLevelILOperation.LLIL_LSL, 3 },                     // size, left, right
			{ LowLevelILOperation.LLIL_LSR, 3 },                     // size, left, right
			{ LowLevelILOperation.LLIL_ASR, 3 },                     // size, left, right
			{ LowLevelILOperation.LLIL_ROL, 3 },                     // size, left, right
			{ LowLevelILOperation.LLIL_RLC, 3 },                     // size, left, right
			{ LowLevelILOperation.LLIL_ROR, 3 },                     // size, left, right
			{ LowLevelILOperation.LLIL_RRC, 3 },                     // size, left, right
			{ LowLevelILOperation.LLIL_MUL, 3 },                     // size, left, right
			{ LowLevelILOperation.LLIL_MULU_DP, 3 },                 // size, left, right
			{ LowLevelILOperation.LLIL_MULS_DP, 3 },                 // size, left, right
			{ LowLevelILOperation.LLIL_DIVU, 3 },                    // size, left, right
			{ LowLevelILOperation.LLIL_DIVU_DP, 3 },                 // size, left, right
			{ LowLevelILOperation.LLIL_DIVS, 3 },                    // size, left, right
			{ LowLevelILOperation.LLIL_DIVS_DP, 3 },                 // size, left, right
			{ LowLevelILOperation.LLIL_MODU, 3 },                    // size, left, right
			{ LowLevelILOperation.LLIL_MODU_DP, 3 },                 // size, left, right
			{ LowLevelILOperation.LLIL_MODS, 3 },                    // size, left, right
			{ LowLevelILOperation.LLIL_MODS_DP, 3 },                 // size, left, right

			{ LowLevelILOperation.LLIL_NEG, 2 },                     // size, src
			{ LowLevelILOperation.LLIL_NOT, 2 },                     // size, src
			{ LowLevelILOperation.LLIL_SX, 3 },                      // toSize, fromSize, src
			{ LowLevelILOperation.LLIL_ZX, 3 },                      // toSize, fromSize, src
			{ LowLevelILOperation.LLIL_LOW_PART, 2 },                // toSize, src

			{ LowLevelILOperation.LLIL_JUMP, 1 },                    // dest
			{ LowLevelILOperation.LLIL_JUMP_TO, 3 },                 // dest, table, targetReg
			{ LowLevelILOperation.LLIL_CALL, 1 },                    // dest
			{ LowLevelILOperation.LLIL_CALL_STACK_ADJUST, 2 },       // adjust, callExpr
			{ LowLevelILOperation.LLIL_TAILCALL, 1 },                // dest
			{ LowLevelILOperation.LLIL_RET, 1 },                     // src(list)
			{ LowLevelILOperation.LLIL_NORET, 0 },                   // noreturn

			{ LowLevelILOperation.LLIL_IF, 3 },                      // cond, true, false (blocks)
			{ LowLevelILOperation.LLIL_GOTO, 1 },                    // target block
			{ LowLevelILOperation.LLIL_FLAG_COND, 2 },  // flagCondition, semanticFlagClass
			{ LowLevelILOperation.LLIL_FLAG_GROUP, 1 },              // group

			{ LowLevelILOperation.LLIL_CMP_E, 3 },                   // size, left, right
			{ LowLevelILOperation.LLIL_CMP_NE, 3 },                  // size, left, right
			{ LowLevelILOperation.LLIL_CMP_SLT, 3 },                 // size, left, right
			{ LowLevelILOperation.LLIL_CMP_ULT, 3 },                 // size, left, right
			{ LowLevelILOperation.LLIL_CMP_SLE, 3 },                 // size, left, right
			{ LowLevelILOperation.LLIL_CMP_ULE, 3 },                 // size, left, right
			{ LowLevelILOperation.LLIL_CMP_SGE, 3 },                 // size, left, right
			{ LowLevelILOperation.LLIL_CMP_UGE, 3 },                 // size, left, right
			{ LowLevelILOperation.LLIL_CMP_SGT, 3 },                 // size, left, right
			{ LowLevelILOperation.LLIL_CMP_UGT, 3 },                 // size, left, right
			{ LowLevelILOperation.LLIL_TEST_BIT, 3 },                // size, left, right(bit)
			{ LowLevelILOperation.LLIL_BOOL_TO_INT, 2 },             // size, src
			{ LowLevelILOperation.LLIL_ADD_OVERFLOW, 3 },            // size, left, right

			{ LowLevelILOperation.LLIL_SYSCALL, 1 },                 // params(list)
			{ LowLevelILOperation.LLIL_BP, 0 },                      // breakpoint
			{ LowLevelILOperation.LLIL_TRAP, 1 },                    // code
			{ LowLevelILOperation.LLIL_INTRINSIC, 4 },  // outputRegisterOrFlagList(+1), intrinsic, parameterExprs
			{ LowLevelILOperation.LLIL_UNDEF, 0 },                   // undefined
			{ LowLevelILOperation.LLIL_UNIMPL, 0 },                  // unimplemented
			{ LowLevelILOperation.LLIL_UNIMPL_MEM, 1 },              // addr

			{ LowLevelILOperation.LLIL_FADD, 3 },                    // size, left, right
			{ LowLevelILOperation.LLIL_FSUB, 3 },                    // size, left, right
			{ LowLevelILOperation.LLIL_FMUL, 3 },                    // size, left, right
			{ LowLevelILOperation.LLIL_FDIV, 3 },                    // size, left, right
			{ LowLevelILOperation.LLIL_FSQRT, 2 },                   // size, src
			{ LowLevelILOperation.LLIL_FNEG, 2 },                    // size, src
			{ LowLevelILOperation.LLIL_FABS, 2 },                    // size, src
			{ LowLevelILOperation.LLIL_FLOAT_TO_INT, 3 },            // toSize, fromSize, src
			{ LowLevelILOperation.LLIL_INT_TO_FLOAT, 3 },            // toSize, fromSize, src
			{ LowLevelILOperation.LLIL_FLOAT_CONV, 3 },              // toSize, fromSize, src
			{ LowLevelILOperation.LLIL_ROUND_TO_INT, 2 },            // size, src
			{ LowLevelILOperation.LLIL_FLOOR, 2 },                   // size, src
			{ LowLevelILOperation.LLIL_CEIL, 2 },                    // size, src
			{ LowLevelILOperation.LLIL_FTRUNC, 2 },                  // size, src
			{ LowLevelILOperation.LLIL_FCMP_E, 3 },                  // size, left, right
			{ LowLevelILOperation.LLIL_FCMP_NE, 3 },                 // size, left, right
			{ LowLevelILOperation.LLIL_FCMP_LT, 3 },                 // size, left, right
			{ LowLevelILOperation.LLIL_FCMP_LE, 3 },                 // size, left, right
			{ LowLevelILOperation.LLIL_FCMP_GE, 3 },                 // size, left, right
			{ LowLevelILOperation.LLIL_FCMP_GT, 3 },                 // size, left, right
			{ LowLevelILOperation.LLIL_FCMP_O, 3 },                  // size, left, right
			{ LowLevelILOperation.LLIL_FCMP_UO, 3 },                 // size, left, right

			{ LowLevelILOperation.LLIL_SET_REG_SSA, 3 },             // dstReg, version, src
			{ LowLevelILOperation.LLIL_SET_REG_SSA_PARTIAL, 4 },  // destSSARegister(var,version), partialRegister, sourceExpr
			{ LowLevelILOperation.LLIL_SET_REG_SPLIT_SSA, 4 },       // dstHi, verHi, dstLo, verLo, src
			{ LowLevelILOperation.LLIL_SET_REG_STACK_REL_SSA, 4 },   // regStack, version, rel, src
			{ LowLevelILOperation.LLIL_SET_REG_STACK_ABS_SSA, 3 },   // regStack, index, src
			{ LowLevelILOperation.LLIL_REG_SPLIT_DEST_SSA, 4 },      // regHi, verHi, regLo, verLo
			{ LowLevelILOperation.LLIL_REG_STACK_DEST_SSA, 3 },      // regStack, index, size
			{ LowLevelILOperation.LLIL_REG_SSA, 2 },                 // reg, version
			{ LowLevelILOperation.LLIL_REG_SSA_PARTIAL, 3 },  // sourceSSARegister(var,version), partialRegister
			{ LowLevelILOperation.LLIL_REG_SPLIT_SSA, 4 },           // regHi, verHi, regLo, verLo
			{ LowLevelILOperation.LLIL_REG_STACK_REL_SSA, 4 },  // sourceSSARegisterStack(var,version), sourceExpr, topSSARegister
			{ LowLevelILOperation.LLIL_REG_STACK_ABS_SSA, 3 },  // sourceSSARegisterStack(var,version), sourceRegister
			{ LowLevelILOperation.LLIL_REG_STACK_FREE_REL_SSA, 3 },  // regStack, version, rel
			{ LowLevelILOperation.LLIL_REG_STACK_FREE_ABS_SSA, 2 },  // regStack, index
			{ LowLevelILOperation.LLIL_SET_FLAG_SSA, 3 },            // flag, version, src
			{ LowLevelILOperation.LLIL_ASSERT_SSA, 3 },  // sourceSSARegister(var,version), constraint
			{ LowLevelILOperation.LLIL_FORCE_VER_SSA, 4 },  // destSSARegister(var,version), sourceSSARegister(var,version)
			{ LowLevelILOperation.LLIL_FLAG_SSA, 2 },                // flag, version
			{ LowLevelILOperation.LLIL_FLAG_BIT_SSA, 3 },  // sourceSSAFlag(var,version), bitIndex

			{ LowLevelILOperation.LLIL_CALL_SSA, 4 },                // dest, params(list), outputs(list), srcMem
			{ LowLevelILOperation.LLIL_SYSCALL_SSA, 3 },             // params(list), outputs(list), srcMem
			{ LowLevelILOperation.LLIL_TAILCALL_SSA, 4 },  // outputSSARegisters, destExpr, stackSSARegister, parameterExprs
			{ LowLevelILOperation.LLIL_CALL_PARAM, 2 },              // paramLoc, src
			{ LowLevelILOperation.LLIL_CALL_STACK_SSA, 3 },  // stackSSARegister(var,version), prevMemoryVersion
			{ LowLevelILOperation.LLIL_CALL_OUTPUT_SSA, 3 },         // outputLoc, srcExpr, dstMem
			{ LowLevelILOperation.LLIL_SEPARATE_PARAM_LIST_SSA, 2 }, // intParams(list), floatParams(list)
			{ LowLevelILOperation.LLIL_SHARED_PARAM_SLOT_SSA, 2 },   // slot, size
			{ LowLevelILOperation.LLIL_MEMORY_INTRINSIC_OUTPUT_SSA, 3 }, // dest, size, dstMem
			{ LowLevelILOperation.LLIL_LOAD_SSA, 2 },                // addr, srcMem
			{ LowLevelILOperation.LLIL_STORE_SSA, 4 },  // destExpr, destMemoryVersion, sourceMemoryVersion, sourceExpr
			{ LowLevelILOperation.LLIL_INTRINSIC_SSA, 4 },           // intrinsicId, params(list), outputs(list), srcMem
			{ LowLevelILOperation.LLIL_MEMORY_INTRINSIC_SSA, 4 },    // dest, src, size, dstMem

			{ LowLevelILOperation.LLIL_REG_PHI, 3 },  // destSSARegister(var,version), sourceSSARegisters(list)
			{ LowLevelILOperation.LLIL_REG_STACK_PHI, 3 },  // destSSARegisterStack(var,version), sourceSSARegisterStacks(list)
			{ LowLevelILOperation.LLIL_FLAG_PHI, 3 },  // destSSAFlag(var,version), sourceSSAFlags(list)
			{ LowLevelILOperation.LLIL_MEM_PHI, 1 },                 // memVersions(list)

			{ LowLevelILOperation.LLIL_ABS, 1 },                     // src
			{ LowLevelILOperation.LLIL_BSWAP, 1 },                   // src
			{ LowLevelILOperation.LLIL_CLS, 1 },                     // src
			{ LowLevelILOperation.LLIL_CLZ, 1 },                     // src
			{ LowLevelILOperation.LLIL_CTZ, 1 },                     // src
			{ LowLevelILOperation.LLIL_POPCNT, 1 },                  // src
			{ LowLevelILOperation.LLIL_RBIT, 1 },                    // src
			{ LowLevelILOperation.LLIL_MAXS, 2 },                    // left, right
			{ LowLevelILOperation.LLIL_MAXU, 2 },                    // left, right
			{ LowLevelILOperation.LLIL_MINS, 2 },                    // left, right
			{ LowLevelILOperation.LLIL_MINU, 2 },                    // left, right
		};
		
	
		
		internal LowLevelILInstruction(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex 
		) : this( 
			function , 
			expressionIndex ,
			NativeMethods.BNGetLowLevelILByIndex(
				function.DangerousGetHandle() , 
				expressionIndex
			)
		)
		{
			
		}
		
		internal LowLevelILInstruction(
			LowLevelILFunction function, 
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native
		) 
		{
			this.Operation = native.operation ;
			this.Attributes = native.attributes ;
			this.SourceOperand = (OperandIndex)native.sourceOperand;
			this.Size = native.size ;
			this.Address = native.address ;

			LowLevelILInstruction.OperationOperands.TryGetValue(
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
			
			this.ILFunction = function;
			
			this.ExpressionIndex = expressionIndex;
		}
	
		public BNLowLevelILInstruction ToNative()
		{
			BNLowLevelILInstruction native = new BNLowLevelILInstruction()
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
		
		
		public override bool Equals(object? other)
		{
			return Equals(other as LowLevelILInstruction);
		}

		public bool Equals(LowLevelILInstruction? other)
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

		public static bool operator ==(LowLevelILInstruction? left, LowLevelILInstruction? right)
		{
			if (left is null)
			{
				return right is null;
			}
			
			return left.Equals(right);
		}

		public static bool operator !=(LowLevelILInstruction? left, LowLevelILInstruction? right)
		{
			return !(left == right);
		}

		public int CompareTo(LowLevelILInstruction? other)
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

		public SourceLocation Location
		{
			get
			{
				return new SourceLocation(this.Address , this.SourceOperand);
			}
		}
		
		public static LowLevelILInstruction FromExpressionIndex(
			LowLevelILFunction ilFunction , 
			LowLevelILExpressionIndex expression
		)
		{
			BNLowLevelILInstruction native = NativeMethods.BNGetLowLevelILByIndex(
				ilFunction.DangerousGetHandle() , 
				(LowLevelILExpressionIndex)expression
			);

			switch (native.operation)
			{
				case LowLevelILOperation.LLIL_NOP:
				{
					return new LLILNop(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_SET_REG:
				{
					return new LLILSetRegister(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_SET_REG_SPLIT:
				{
					return new LLILSetRegisterSplit(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_SET_FLAG:
				{
					return new LLILSetFlag(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_SET_REG_STACK_REL:
				{
					return new LLILSetRegisterStackRel(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_REG_STACK_PUSH:
				{
					return new LLILRegisterStackPush(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_ASSERT:
				{
					return new LLILAssert(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FORCE_VER:
				{
					return new LLILForceVersion(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_LOAD:
				{
					return new LLILLoad(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_STORE:
				{
					return new LLILStore(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_PUSH:
				{
					return new LLILPush(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_POP:
				{
					return new LLILPop(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_REG:
				{
					return new LLILRegister(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_REG_SPLIT:
				{
					return new LLILRegisterSplit(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_REG_STACK_REL:
				{
					return new LLILRegisterStackRelative(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_REG_STACK_POP:
				{
					return new LLILRegisterStackPop(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_REG_STACK_FREE_REG:
				{
					return new LLILRegisterStackFreeRegister(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_REG_STACK_FREE_REL:
				{
					return new LLILRegisterStackFreeRel(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_CONST:
				{
					return new LLILConst(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_CONST_PTR:
				{
					return new LLILConstPointer(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_EXTERN_PTR:
				{
					return new LLILExternPointer(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FLOAT_CONST:
				{
					return new LLILFloatConst(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FLAG:
				{
					return new LLILFlag(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FLAG_BIT:
				{
					return new LLILFlagBit(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_ADD:
				{
					return new LLILAdd(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_ADC:
				{
					return new LLILAddCarry(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_SUB:
				{
					return new LLILSub(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_SBB:
				{
					return new LLILSubBorrow(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_AND:
				{
					return new LLILAnd(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_OR:
				{
					return new LLILOr(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_XOR:
				{
					return new LLILXor(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_LSL:
				{
					return new LLILLogicalShiftLeft(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_LSR:
				{
					return new LLILLogicalShiftRight(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_ASR:
				{
					return new LLILArithmeticShiftRight(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_ROL:
				{
					return new LLILRotateLeft(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_RLC:
				{
					return new LLILRotateLeftCarry(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_ROR:
				{
					return new LLILRotateRight(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_RRC:
				{
					return new LLILRotateRightCarry(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_MUL:
				{
					return new LLILMul(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_MULU_DP:
				{
					return new LLILMulUnsignedDoublePrecision(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_MULS_DP:
				{
					return new LLILMulSignedDoublePrecision(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_DIVU:
				{
					return new LLILDivUnsigned(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_DIVU_DP:
				{
					return new LLILDivUnsignedDoublePrecision(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_DIVS:
				{
					return new LLILDivSigned(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_DIVS_DP:
				{
					return new LLILDivSignedDoublePrecision(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_MODU:
				{
					return new LLILModUnsigned(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_MODU_DP:
				{
					return new LLILModUnsignedDoublePrecision(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_MODS:
				{
					return new LLILModSigned(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_MODS_DP:
				{
					return new LLILModSignedDoublePrecision(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_NEG:
				{
					return new LLILNeg(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_NOT:
				{
					return new LLILNot(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_SX:
				{
					return new LLILSignExtend(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_ZX:
				{
					return new LLILZeroExtend(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_LOW_PART:
				{
					return new LLILLowPart(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_JUMP:
				{
					return new LLILJump(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_JUMP_TO:
				{
					return new LLILJumpTo(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_CALL:
				{
					return new LLILCall(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_CALL_STACK_ADJUST:
				{
					return new LLILCallStackAdjust(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_TAILCALL:
				{
					return new LLILTailCall(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_RET:
				{
					return new LLILReturn(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_NORET:
				{
					return new LLILNoReturn(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_IF:
				{
					return new LLILIf(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_GOTO:
				{
					return new LLILGoto(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FLAG_COND:
				{
					return new LLILFlagCond(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FLAG_GROUP:
				{
					return new LLILFlagGroup(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_CMP_E:
				{
					return new LLILEqual(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_CMP_NE:
				{
					return new LLILNotEqual(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_CMP_SLT:
				{
					return new LLILSignedLessThan(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_CMP_ULT:
				{
					return new LLILUnsignedLessThan(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_CMP_SLE:
				{
					return new LLILSignedLessEqual(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_CMP_ULE:
				{
					return new LLILUnsignedLessEqual(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_CMP_SGE:
				{
					return new LLILSignedGreaterEqual(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_CMP_UGE:
				{
					return new LLILUnsignedGreaterEqual(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_CMP_SGT:
				{
					return new LLILSignedGreaterThan(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_CMP_UGT:
				{
					return new LLILUnsignedGreaterThan(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_TEST_BIT:
				{
					return new LLILTestBit(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_BOOL_TO_INT:
				{
					return new LLILBoolToInt(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_ADD_OVERFLOW:
				{
					return new LLILAddOverflow(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_SYSCALL:
				{
					return new LLILSysCall(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_BP:
				{
					return new LLILBreakpoint(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_TRAP:
				{
					return new LLILTrap(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_INTRINSIC:
				{
					return new LLILIntrinsic(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_UNDEF:
				{
					return new LLILUndefined(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_UNIMPL:
				{
					return new LLILUnimplemented(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_UNIMPL_MEM:
				{
					return new LLILUnimplementedMemory(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FADD:
				{
					return new LLILFloatAdd(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FSUB:
				{
					return new LLILFloatSub(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FMUL:
				{
					return new LLILFloatMul(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FDIV:
				{
					return new LLILFloatDiv(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FSQRT:
				{
					return new LLILFloatSquareRoot(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FNEG:
				{
					return new LLILFloatNeg(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FABS:
				{
					return new LLILFloatAbs(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FLOAT_TO_INT:
				{
					return new LLILFloatToInt(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_INT_TO_FLOAT:
				{
					return new LLILIntToFloat(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FLOAT_CONV:
				{
					return new LLILFloatConvert(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_ROUND_TO_INT:
				{
					return new LLILRoundToInt(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FLOOR:
				{
					return new LLILFloor(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_CEIL:
				{
					return new LLILCeil(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FTRUNC:
				{
					return new LLILFloatTrunc(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FCMP_E:
				{
					return new LLILFloatEqual(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FCMP_NE:
				{
					return new LLILFloatNotEqual(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FCMP_LT:
				{
					return new LLILFloatLessThan(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FCMP_LE:
				{
					return new LLILFloatLessEqual(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FCMP_GE:
				{
					return new LLILFloatGreaterEqual(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FCMP_GT:
				{
					return new LLILFloatGreaterThan(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FCMP_O:
				{
					return new LLILFloatCompareOrdered(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FCMP_UO:
				{
					return new LLILFloatCompareUnordered(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_SET_REG_SSA:
				{
					return new LLILSetRegisterSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_SET_REG_SSA_PARTIAL:
				{
					return new LLILSetRegisterSSAPartial(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_SET_REG_SPLIT_SSA:
				{
					return new LLILSetRegisterSplitSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_SET_REG_STACK_REL_SSA:
				{
					return new LLILSetRegisterStackRelativeSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_SET_REG_STACK_ABS_SSA:
				{
					return new LLILSetRegisterStackAbsSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_REG_SPLIT_DEST_SSA:
				{
					return new LLILRegisterSplitDestinationSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_REG_STACK_DEST_SSA:
				{
					return new LLILRegisterStackDestinationSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_REG_SSA:
				{
					return new LLILRegisterSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_REG_SSA_PARTIAL:
				{
					return new LLILRegisterSSAPartial(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_REG_SPLIT_SSA:
				{
					return new LLILRegisterSplitSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_REG_STACK_REL_SSA:
				{
					return new LLILRegisterStackRelativeSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_REG_STACK_ABS_SSA:
				{
					return new LLILRegisterStackAbsSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_REG_STACK_FREE_REL_SSA:
				{
					return new LLILRegisterStackFreeRelativeSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_REG_STACK_FREE_ABS_SSA:
				{
					return new LLILRegisterStackFreeAbsSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_SET_FLAG_SSA:
				{
					return new LLILSetFlagSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_ASSERT_SSA:
				{
					return new LLILAssertSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FORCE_VER_SSA:
				{
					return new LLILForceVersionSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FLAG_SSA:
				{
					return new LLILFlagSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FLAG_BIT_SSA:
				{
					return new LLILFlagBitSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_CALL_SSA:
				{
					return new LLILCallSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_SYSCALL_SSA:
				{
					return new LLILSysCallSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_TAILCALL_SSA:
				{
					return new LLILTailCallSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_CALL_PARAM:
				{
					return new LLILCallParameter(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_CALL_STACK_SSA:
				{
					return new LLILCallStackSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_CALL_OUTPUT_SSA:
				{
					return new LLILCallOutputSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_SEPARATE_PARAM_LIST_SSA:
				{
					return new LLILSeparateParamListSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_SHARED_PARAM_SLOT_SSA:
				{
					return new LLILSharedParamSlotSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_MEMORY_INTRINSIC_OUTPUT_SSA:
				{
					return new LLILMemoryIntrinsicOutputSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_LOAD_SSA:
				{
					return new LLILLoadSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_STORE_SSA:
				{
					return new LLILStoreSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_INTRINSIC_SSA:
				{
					return new LLILIntrinsicSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_MEMORY_INTRINSIC_SSA:
				{
					return new LLILMemoryIntrinsicSSA(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_REG_PHI:
				{
					return new LLILRegisterPhi(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_REG_STACK_PHI:
				{
					return new LLILRegisterStackPhi(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_FLAG_PHI:
				{
					return new LLILFlagPhi(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_MEM_PHI:
				{
					return new LLILMemoryPhi(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_ABS:
				case LowLevelILOperation.LLIL_BSWAP:
				case LowLevelILOperation.LLIL_CLS:
				case LowLevelILOperation.LLIL_CLZ:
				case LowLevelILOperation.LLIL_CTZ:
				case LowLevelILOperation.LLIL_POPCNT:
				case LowLevelILOperation.LLIL_RBIT:
				{
					return new LLILGenericUnary(ilFunction , expression , native );
				}
				case LowLevelILOperation.LLIL_MAXS:
				case LowLevelILOperation.LLIL_MAXU:
				case LowLevelILOperation.LLIL_MINS:
				case LowLevelILOperation.LLIL_MINU:
				{
					return new LLILGenericBinary(ilFunction , expression , native );
				}
				default:
				{
					// Unknown / not-yet-typed operation (or an op from a newer core).
					// Degrade to a generic wrapper instead of throwing so
					// navigation/iteration stays robust.
					return new LLILGeneric(ilFunction , expression , native );
				}
			}
		}
		
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			foreach (InstructionTextToken token in this.ExpressionTextTokens)
			{
				builder.Append(token.Text);
			}
			
			return builder.ToString();
		}
		
		public T[] GetOperandAsIntegerArray<T>(OperandIndex operand)
			where T : unmanaged
		{
			IntPtr arrayPointer = NativeMethods.BNLowLevelILGetOperandList(
				this.ILFunction.DangerousGetHandle() ,
				this.ExpressionIndex ,
				this.RawOperands[(ulong)operand] ,
				out ulong arrayLength
			);

			return UnsafeUtils.TakeNumberArray<T>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNLowLevelILFreeOperandList
			);
		}
		
		public IDictionary<T,T> GetOperandAsIntegerDict<T>(OperandIndex operand)
			where T : unmanaged
		{
			IntPtr arrayPointer = NativeMethods.BNLowLevelILGetOperandList(
				this.ILFunction.DangerousGetHandle() ,
				this.ExpressionIndex ,
				(ulong)operand ,
				out ulong arrayLength
			);

			ulong[] keyAndValues = UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNLowLevelILFreeOperandList
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
		
		public LowLevelILVariable GetOperandAsVariable(OperandIndex operand)
		{
			return LowLevelILVariable.FromIdentifier(
				this.ILFunction , 
				this.RawOperands[(ulong)operand]
			);
		}
		
		public LowLevelILVariable[] GetOperandAsVariableList(OperandIndex operand1)
		{
			IntPtr arrayPointer = NativeMethods.BNLowLevelILGetOperandList(
				this.ILFunction.DangerousGetHandle() ,
				this.ExpressionIndex ,
				(ulong)operand1 ,
				out ulong arrayLength
			);

			ulong[] identifiers = UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNLowLevelILFreeOperandList
			);

			List<LowLevelILVariable> variables = new List<LowLevelILVariable>();
			
			foreach (ulong identifier in identifiers)
			{
				variables.Add(
					LowLevelILVariable.FromIdentifier(this.ILFunction ,identifier)
				);
			}

			return variables.ToArray();
		}
		
		public LowLevelILSSAVariable GetOperandAsSSAVariable(
			OperandIndex operand1,
			OperandIndex operand2
		)
		{
			LowLevelILVariable ilVariable = LowLevelILVariable.FromIdentifier(
				this.ILFunction ,
				this.RawOperands[(ulong)operand1]
			);

			return new LowLevelILSSAVariable(
				ilVariable ,
				this.RawOperands[(ulong)operand2]
			);
		}

		public LowLevelILSSAVariable[] GetOperandAsSSAVariableList(OperandIndex operand1)
		{
			IntPtr arrayPointer = NativeMethods.BNLowLevelILGetOperandList(
				this.ILFunction.DangerousGetHandle() ,
				this.ExpressionIndex ,
				(ulong)operand1 ,
				out ulong arrayLength
			);

			ulong[] identifierAndVersions = UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNLowLevelILFreeOperandList
			);

			List<LowLevelILSSAVariable> variables = new List<LowLevelILSSAVariable>();

			for (int i = 0; i < identifierAndVersions.Length; i += 2)
			{
				ulong identifier = identifierAndVersions[i];
				ulong version = identifierAndVersions[i + 1];
				
				LowLevelILVariable variable = LowLevelILVariable.FromIdentifier(this.ILFunction ,identifier);
				
				variables.Add(
					new LowLevelILSSAVariable(variable ,version )
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
		
		public LowLevelILFlagCondition GetOperandAsFlagCondition(OperandIndex operand)
		{
			return (LowLevelILFlagCondition)(this.RawOperands[(ulong)operand]);
		}
		
		public SemanticFlagClass GetOperandAsSemanticFlagClass(OperandIndex operand)
		{
			return new SemanticFlagClass(
				this.ILFunction.OwnerFunction.Architecture , 
				(SemanticFlagClassIndex)this.RawOperands[(ulong)operand]
			);
		}
		
		public SemanticFlagGroup GetOperandAsSemanticFlagGroup(OperandIndex operand)
		{
			return new SemanticFlagGroup(
				this.ILFunction.OwnerFunction.Architecture , 
				(SemanticFlagGroupIndex)this.RawOperands[(ulong)operand]
			);
		}
		
		public ILFlag GetOperandAsFlag(OperandIndex operand)
		{
			return new ILFlag(
				this.ILFunction.OwnerFunction.Architecture , 
				(FlagIndex)this.RawOperands[(ulong)operand]
			);
		}
		
		public LowLevelILSSAFlag GetOperandAsSSAFlag(
			OperandIndex operand1,
			OperandIndex operand2
		)
		{
			return new LowLevelILSSAFlag(
				this.ILFunction,
				this.GetOperandAsFlag(operand1) ,
				this.RawOperands[(ulong)operand2]
			);
		}
		
		public LowLevelILSSAFlag[] GetOperandAsSSAFlagList(OperandIndex operand)
		{
			IntPtr arrayPointer = NativeMethods.BNLowLevelILGetOperandList(
				this.ILFunction.DangerousGetHandle() ,
				this.ExpressionIndex ,
				(ulong)operand ,
				out ulong arrayLength
			);

			ulong[] indexAndVersions = UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNLowLevelILFreeOperandList
			);

			List<LowLevelILSSAFlag> flags = new List<LowLevelILSSAFlag>();

			for (int i = 0; i < indexAndVersions.Length; i += 2)
			{
				FlagIndex index = (FlagIndex)indexAndVersions[i];
				ulong version = indexAndVersions[i + 1];
				
				ILFlag register = new ILFlag(
					this.ILFunction.OwnerFunction.Architecture ,
					index
				);
				
				flags.Add(
					new LowLevelILSSAFlag(this.ILFunction, register ,version )
				);
			}
			
			return flags.ToArray();
		}
		
		public ILRegister GetOperandAsRegister(OperandIndex operand)
		{
			return new ILRegister(
				this.ILFunction.OwnerFunction.Architecture , 
				(RegisterIndex)this.RawOperands[(ulong)operand]
			);
		}
		
		public LowLevelILSSARegister GetOperandAsSSARegister(
			OperandIndex operand1,
			OperandIndex operand2
		)
		{
			return new LowLevelILSSARegister(
				this.ILFunction,
				this.GetOperandAsRegister(operand1) ,
				this.RawOperands[(ulong)operand2]
			);
		}
		
		public LowLevelILSSARegister[] GetOperandAsSSARegisterList(OperandIndex operand)
		{
			IntPtr arrayPointer = NativeMethods.BNLowLevelILGetOperandList(
				this.ILFunction.DangerousGetHandle() ,
				this.ExpressionIndex ,
				(ulong)operand ,
				out ulong arrayLength
			);

			ulong[] indexAndVersions = UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNLowLevelILFreeOperandList
			);

			List<LowLevelILSSARegister> registers = new List<LowLevelILSSARegister>();

			for (int i = 0; i < indexAndVersions.Length; i += 2)
			{
				RegisterIndex index = (RegisterIndex)indexAndVersions[i];
				ulong version = indexAndVersions[i + 1];
				
				ILRegister register = new ILRegister(
					this.ILFunction.OwnerFunction.Architecture ,
					index
				);
				
				registers.Add(
					new LowLevelILSSARegister(this.ILFunction, register ,version )
				);
			}
			
			return registers.ToArray();
		}
		
		public RegisterStack GetOperandAsRegisterStack(OperandIndex operand)
		{
			return new RegisterStack(
				this.ILFunction.OwnerFunction.Architecture , 
				(RegisterStackIndex)this.RawOperands[(ulong)operand]
			);
		}
		
		public SSARegisterStack GetOperandAsSSARegisterStack(
			OperandIndex operand1,
			OperandIndex operand2
		)
		{
			return new SSARegisterStack(
				this.GetOperandAsRegisterStack(operand1) ,
				this.RawOperands[(ulong)operand2]
			);
		}
		
		public SSARegisterStack[] GetOperandAsSSARegisterStackList(OperandIndex operand)
		{
			IntPtr arrayPointer = NativeMethods.BNLowLevelILGetOperandList(
				this.ILFunction.DangerousGetHandle() ,
				this.ExpressionIndex ,
				(ulong)operand ,
				out ulong arrayLength
			);

			ulong[] indexAndVersions = UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNLowLevelILFreeOperandList
			);

			List<SSARegisterStack> regStacks = new List<SSARegisterStack>();

			for (int i = 0; i < indexAndVersions.Length; i += 2)
			{
				RegisterStackIndex index = (RegisterStackIndex)indexAndVersions[i];
				
				ulong version = indexAndVersions[i + 1];
				
				RegisterStack regStack = new RegisterStack(
					this.ILFunction.OwnerFunction.Architecture ,
					index
				);
				
				regStacks.Add(
					new SSARegisterStack(regStack ,version )
				);
			}
			
			return regStacks.ToArray();
		}
		
		public IDictionary<RegisterStackIndex,ulong> GetOperandAsRegisterStackDict(OperandIndex operand)
		{
			IntPtr arrayPointer = NativeMethods.BNLowLevelILGetOperandList(
				this.ILFunction.DangerousGetHandle() ,
				this.ExpressionIndex ,
				(ulong)operand ,
				out ulong arrayLength
			);

			ulong[] paires = UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNLowLevelILFreeOperandList
			);

			Dictionary<RegisterStackIndex,ulong> target = new Dictionary<RegisterStackIndex,ulong>();

			for (int i = 0; i < paires.Length; i += 2)
			{
				RegisterStackIndex key = (RegisterStackIndex)( paires[i] );
				
				ulong adjust = paires[i+1];

				if ( 0 != (adjust & 0x80000000) )
				{
					adjust |= ~0x80000000;
				}
				
				target[key] = adjust;
			}
			
			return target;
		}
		
		public PossibleValueSet GetOperandAsPossibleValueSet(OperandIndex operand)
		{
			return PossibleValueSet.TakeNative(
				NativeMethods.BNGetCachedLowLevelILPossibleValueSet(
					this.ILFunction.DangerousGetHandle() ,
					(LowLevelILPossibleValueSetCacheIndex)this.RawOperands[(ulong)operand]
				)
			);
		}
		
		public LowLevelILInstruction GetOperandAsExpression(OperandIndex operand)
		{
			return LowLevelILInstruction.FromExpressionIndex(
				this.ILFunction , 
				(LowLevelILExpressionIndex)this.RawOperands[(ulong)operand]
			);
		}

		public LowLevelILInstruction[] GetOperandAsExpressionList(OperandIndex operand1)
		{
			IntPtr arrayPointer = NativeMethods.BNLowLevelILGetOperandList(
				this.ILFunction.DangerousGetHandle() ,
				this.ExpressionIndex ,
				(ulong)operand1 ,
				out ulong arrayLength
			);

			LowLevelILExpressionIndex[] expressionIndexes = UnsafeUtils.TakeNumberArray<LowLevelILExpressionIndex>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNLowLevelILFreeOperandList
			);

			List<LowLevelILInstruction> expressions = new List<LowLevelILInstruction>();
			
			foreach (LowLevelILExpressionIndex expressionIndex in expressionIndexes)
			{
				expressions.Add(
					LowLevelILInstruction.FromExpressionIndex(
						this.ILFunction ,
						expressionIndex
					)
				);
			}

			return expressions.ToArray();
		}
		
		
		public FlagOrRegister[] GetOperandAsFlagOrRegisterList(OperandIndex operand1)
		{
			IntPtr arrayPointer = NativeMethods.BNLowLevelILGetOperandList(
				this.ILFunction.DangerousGetHandle() ,
				this.ExpressionIndex ,
				(ulong)operand1 ,
				out ulong arrayLength
			);

			ulong[] values = UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNLowLevelILFreeOperandList
			);

			List<FlagOrRegister> targets = new List<FlagOrRegister>();
			
			foreach (ulong value in values)
			{
				if (0 != ( value & ( 1UL << 32 ) ))
				{
					ILFlag flag = new ILFlag(
						this.ILFunction.OwnerFunction.Architecture ,
						(FlagIndex)( value & 0xffffffff )
					);
					
					targets.Add( new FlagOrRegister(flag) );
				}
				else
				{
					ILRegister register = new ILRegister(
						this.ILFunction.OwnerFunction.Architecture ,
						(RegisterIndex)( value & 0xffffffff )
					);
					
					targets.Add( new FlagOrRegister(register) );
				}
			}

			return targets.ToArray();
		}
		
		public SSAFlagOrRegister[] GetOperandAsSSAFlagOrRegisterList(OperandIndex operand1)
		{
			IntPtr arrayPointer = NativeMethods.BNLowLevelILGetOperandList(
				this.ILFunction.DangerousGetHandle() ,
				this.ExpressionIndex ,
				(ulong)operand1 ,
				out ulong arrayLength
			);

			ulong[] paires = UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer ,
				arrayLength ,
				NativeMethods.BNLowLevelILFreeOperandList
			);

			List<SSAFlagOrRegister> targets = new List<SSAFlagOrRegister>();
			
			for (int i = 0; i < paires.Length; i += 2)
			{
				RegisterStackIndex key = (RegisterStackIndex)( paires[i] );
				
				ulong version = paires[i+1];

				if (0 != ( paires[i] & ( 1UL << 32 ) ))
				{
					ILFlag flag = new ILFlag(
						this.ILFunction.OwnerFunction.Architecture ,
						(FlagIndex)( paires[i] & 0xffffffff )
					);

					FlagOrRegister item = new FlagOrRegister(flag);
					
					targets.Add( new SSAFlagOrRegister(item , version) );
				}
				else
				{
					ILRegister register = new ILRegister(
						this.ILFunction.OwnerFunction.Architecture ,
						(RegisterIndex)( paires[i] & 0xffffffff )
					);

					FlagOrRegister item = new FlagOrRegister(register);
					
					targets.Add( new SSAFlagOrRegister(item , version) );
				}
			}
			
			return targets.ToArray();
		}
		
		public InstructionTextToken[] ExpressionTextTokens
		{
			get
			{
				return this.GetExpressionTextTokens();
			}
		}

		/// <summary>
		/// The text tokens of this expression. Alias of
		/// <see cref="ExpressionTextTokens"/> for parity with Python
		/// <c>LowLevelILInstruction.tokens</c> and the MLIL/HLIL <c>Tokens</c> members.
		/// </summary>
		public InstructionTextToken[] Tokens
		{
			get
			{
				return this.ExpressionTextTokens;
			}
		}

		public InstructionTextToken[] GetExpressionTextTokens(
			Architecture? arch = null,
			DisassemblySettings? settings = null
		)
		{
			if (null == arch)
			{
				arch = this.ILFunction.OwnerFunction.Architecture;
			}

			if (null == settings)
			{
				settings = DisassemblySettings.DefaultLinear();
			}

			bool ok = NativeMethods.BNGetLowLevelILExprText(
				this.ILFunction.DangerousGetHandle() ,
				null == arch ? IntPtr.Zero : arch.DangerousGetHandle(),
				this.ExpressionIndex,
				null == settings ? IntPtr.Zero : settings.DangerousGetHandle(),
				out IntPtr arrayPointer,
				out ulong arrayLength
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
		
		public InstructionTextToken[] InstructionTextTokens
		{
			get
			{
				return this.GetInstructionTextTokens();
			}
		}
		
		public InstructionTextToken[] GetInstructionTextTokens(
			Architecture? arch = null,
			DisassemblySettings? settings = null
		)
		{
			if (null == arch)
			{
				arch = this.ILFunction.OwnerFunction.Architecture;
			}

			if (null == settings)
			{
				settings = DisassemblySettings.DefaultLinear();
			}
			
			bool ok = NativeMethods.BNGetLowLevelILInstructionText(
				this.ILFunction.DangerousGetHandle() ,
				this.ILFunction.OwnerFunction.DangerousGetHandle(),
				null == arch ? IntPtr.Zero : arch.DangerousGetHandle(),
				this.InstructionIndex,
				null == settings ? IntPtr.Zero : settings.DangerousGetHandle(),
				out IntPtr arrayPointer,
				out ulong arrayLength
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
		
		public LowLevelILInstructionIndex InstructionIndex
		{
			get
			{
				return NativeMethods.BNGetLowLevelILInstructionForExpr(
					this.ILFunction.DangerousGetHandle() ,
					this.ExpressionIndex
				);
			}
		}
		
		public LowLevelILInstructionIndex SSAInstructionIndex
		{
			get
			{
				if (this.ILFunction.IsSSAForm)
				{
					return this.InstructionIndex;
				}
				else
				{
					return NativeMethods.BNGetLowLevelILSSAInstructionIndex(
						this.ILFunction.DangerousGetHandle() ,
						this.InstructionIndex
					);
				}
			}
		}
		
		public LowLevelILInstructionIndex NonSSAInstructionIndex
		{
			get
			{
				if (this.ILFunction.IsSSAForm)
				{
					return NativeMethods.BNGetLowLevelILNonSSAInstructionIndex(
						this.ILFunction.DangerousGetHandle() ,
						this.InstructionIndex
					);
				}
				else
				{
					return this.InstructionIndex;
				}
			}
		}
		
		
		public LowLevelILBasicBlock? BasicBlock
		{
			get
			{
				return LowLevelILBasicBlock.TakeHandleEx(
					this.ILFunction,
					NativeMethods.BNGetLowLevelILBasicBlockForInstruction(
						this.ILFunction.DangerousGetHandle() ,
						this.InstructionIndex
					)
				);
			}
		}

		
		public LowLevelILInstruction? SSAForm
		{
			get
			{
				if (this.ILFunction.IsSSAForm)
				{
					return this;
				}
				
				LowLevelILExpressionIndex index = NativeMethods.BNGetLowLevelILSSAExprIndex(
					this.ILFunction.DangerousGetHandle(),
					this.ExpressionIndex
				);

				if ((ulong)index != this.ILFunction.ExpressionCount)
				{
					return null;
				}
				
				return LowLevelILInstruction.FromExpressionIndex(
					this.ILFunction.SSAForm ,
					index
				);
			}
		}
		
		public LowLevelILInstruction? NonSSAForm
		{
			get
			{
				if (this.ILFunction.IsSSAForm)
				{
					LowLevelILExpressionIndex index = NativeMethods.BNGetLowLevelILNonSSAExprIndex(
						this.ILFunction.DangerousGetHandle(),
						this.ExpressionIndex
					);

					if ((ulong)index != this.ILFunction.ExpressionCount)
					{
						return null;
					}
				
					return LowLevelILInstruction.FromExpressionIndex(
						this.ILFunction.NonSSAForm ,
						index
					);
				}
				else
				{
					return this;
				}
			}
		}

		public RegisterValue Value
		{
			get
			{
				return RegisterValue.FromNative(
					NativeMethods.BNGetLowLevelILExprValue(
						this.ILFunction.DangerousGetHandle() ,
						this.ExpressionIndex
					)
				);
			}
		}
		
		public PossibleValueSet GetPossibleValues(DataFlowQueryOption[] options)
		{
			return PossibleValueSet.TakeNative(
				NativeMethods.BNGetLowLevelILPossibleExprValues(
					this.ILFunction.DangerousGetHandle() ,
					this.ExpressionIndex,
					options,
					(ulong)options.Length
				)
			);
		}
		
		public PossibleValueSet PossibleValues
		{
			get
			{
				return this.GetPossibleValues(Array.Empty<DataFlowQueryOption>());
			}
		}
		
		public MediumLevelILInstruction? MediumLevelILExpression
		{
			get
			{
				return this.ILFunction.GetMediumLevelILExpression(
					this.ExpressionIndex
				);
			}
		}
		
		public MediumLevelILInstruction? MediumLevelILInstruction
		{
			get
			{
				return this.ILFunction.GetMediumLevelILInstruction(
					this.InstructionIndex
				);
			}
		}

		public MediumLevelILInstruction[] MediumLevelILExpressions
		{
			get
			{
				return this.ILFunction.GetMediumLevelILExpressions(this.ExpressionIndex);
			}
		}
		
		public MediumLevelILInstruction? MappedMediumLevelILExpression
		{
			get
			{
				return this.ILFunction.GetMappedMediumLevelILExpression(
					this.ExpressionIndex
				);
			}
		}
		
		public MediumLevelILInstruction? MappedMediumLevelILInstruction
		{
			get
			{
				return this.ILFunction.GetMappedMediumLevelILInstruction(
					this.InstructionIndex
				);
			}
		}
	
		public HighLevelILInstruction? HighLevelILExpression
		{
			get
			{
				return this.ILFunction.GetHighLevelILExpression(
					this.ExpressionIndex
				);
			}
		}
		
		public HighLevelILInstruction[] HighLevelILExpressions
		{
			get
			{
				return this.ILFunction.GetHighLevelILExpressions(
					this.ExpressionIndex
				);
			}
		}
		
		public HighLevelILInstruction? HighLevelILInstruction
		{
			get
			{
				return this.ILFunction.GetHighLevelILInstruction(
					this.InstructionIndex
				);
			}
		}
		
		public RegisterValue GetRegisterValue(RegisterIndex register)
		{
			return RegisterValue.FromNative(
				NativeMethods.BNGetLowLevelILRegisterValueAtInstruction(
					this.ILFunction.DangerousGetHandle() ,
					register,
					this.InstructionIndex
				)
			);
		}
		
		public RegisterValue GetRegisterValueAfter(uint registerIndex)
		{
			return RegisterValue.FromNative(
				NativeMethods.BNGetLowLevelILRegisterValueAfterInstruction(
					this.ILFunction.DangerousGetHandle() ,
					registerIndex,
					this.InstructionIndex
				)
			);
		}
		
		public PossibleValueSet GetRegisterPossibleValues(
			RegisterIndex register ,
			DataFlowQueryOption[] options
		)
		{
			return PossibleValueSet.TakeNative(
				NativeMethods.BNGetLowLevelILPossibleRegisterValuesAtInstruction(
					this.ILFunction.DangerousGetHandle() ,
					register,
					this.InstructionIndex,
					options,
					(ulong)options.Length
				)
			);
		}
		
		public PossibleValueSet GetRegisterPossibleValuesAfter(
			RegisterIndex register ,
			DataFlowQueryOption[] options
		)
		{
			return PossibleValueSet.TakeNative(
				NativeMethods.BNGetLowLevelILPossibleRegisterValuesAfterInstruction(
					this.ILFunction.DangerousGetHandle() ,
					register,
					this.InstructionIndex,
					options,
					(ulong)options.Length
				)
			);
		}
		
		public RegisterValue GetFlagValue(FlagIndex flag)
		{
			return RegisterValue.FromNative(
				NativeMethods.BNGetLowLevelILFlagValueAtInstruction(
					this.ILFunction.DangerousGetHandle() ,
					flag,
					this.InstructionIndex
				)
			);
		}
		
		public RegisterValue GetFlagValueAfter(FlagIndex flag)
		{
			return RegisterValue.FromNative(
				NativeMethods.BNGetLowLevelILFlagValueAfterInstruction(
					this.ILFunction.DangerousGetHandle() ,
					flag,
					this.InstructionIndex
				)
			);
		}
		
		public PossibleValueSet GetPossibleFlagValues(
			FlagIndex flag ,
			DataFlowQueryOption[] options
		)
		{
			return PossibleValueSet.TakeNative(
				NativeMethods.BNGetLowLevelILPossibleFlagValuesAtInstruction(
					this.ILFunction.DangerousGetHandle() ,
					flag,
					this.InstructionIndex,
					options,
					(ulong)options.Length
				)
			);
		}
		
		public PossibleValueSet GetPossibleFlagValuesAfter(
			FlagIndex flag ,
			DataFlowQueryOption[] options
		)
		{
			return PossibleValueSet.TakeNative(
				NativeMethods.BNGetLowLevelILPossibleFlagValuesAfterInstruction(
					this.ILFunction.DangerousGetHandle() ,
					flag,
					this.InstructionIndex,
					options,
					(ulong)options.Length
				)
			);
		}
		
		public RegisterValue GetStackContents(long offset , ulong length)
		{
			return RegisterValue.FromNative(
				NativeMethods.BNGetLowLevelILStackContentsAtInstruction(
					this.ILFunction.DangerousGetHandle() ,
					offset,
					length,
					this.InstructionIndex
				)
			);
		}
		
		public RegisterValue GetStackContentsAfter(long offset , ulong length)
		{
			return RegisterValue.FromNative(
				NativeMethods.BNGetLowLevelILStackContentsAfterInstruction(
					this.ILFunction.DangerousGetHandle() ,
					offset,
					length,
					this.InstructionIndex
				)
			);
		}
		
		public PossibleValueSet GetPossibleStackContents(
			long offset ,
			ulong length,
			DataFlowQueryOption[]? options = null
		)
		{
			if (null == options)
			{
				options = Array.Empty<DataFlowQueryOption>();
			}
			
			return PossibleValueSet.TakeNative(
				NativeMethods.BNGetLowLevelILPossibleStackContentsAtInstruction(
					this.ILFunction.DangerousGetHandle() ,
					offset,
					length,
					this.InstructionIndex,
					options,
					(ulong)options.Length
				)
			);
		}
		
		public PossibleValueSet GetPossibleStackContentsAfter(
			long offset ,
			ulong length,
			DataFlowQueryOption[]? options = null
		)
		{
			if (null == options)
			{
				options = Array.Empty<DataFlowQueryOption>();
			}
			
			return PossibleValueSet.TakeNative(
				NativeMethods.BNGetLowLevelILPossibleStackContentsAfterInstruction(
					this.ILFunction.DangerousGetHandle() ,
					offset,
					length,
					this.InstructionIndex,
					options,
					(ulong)options.Length
				)
			);
		}
		
		public LowLevelILInstruction[] Exits
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNLowLevelILGetExitsForInstruction(
					this.ILFunction.DangerousGetHandle() ,
					this.InstructionIndex,
					out ulong arrayLength 
				);

				ulong[] indexes = UnsafeUtils.TakeNumberArray<ulong>(
					arrayPointer ,
					arrayLength ,
					NativeMethods.BNFreeILInstructionList
				);
				
				List<LowLevelILInstruction> instructions = new List<LowLevelILInstruction>();

				foreach (LowLevelILInstructionIndex index in indexes)
				{
					instructions.Add(this.ILFunction.MustGetInstruction(index));
				}

				return instructions.ToArray();
			}
		}
		
		public LowLevelILLabel? Label
		{
			get
			{
				return LowLevelILLabel.FromNativePointer(
					NativeMethods.BNGetLabelForLowLevelILSourceInstruction(
						this.ILFunction.DangerousGetHandle() ,
						this.InstructionIndex
					)
				);
			}
		}
		
		public void SetSourceOperand(uint operand)
		{
			NativeMethods.BNLowLevelILSetExprSourceOperand(
				this.ILFunction.DangerousGetHandle(),
				this.ExpressionIndex,
				operand
			);
		}
    }
}