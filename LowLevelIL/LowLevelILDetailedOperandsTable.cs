using System;
using System.Collections.Generic;

namespace BinaryNinja
{
	// Maps every installed-core LLIL operation to the exact named operands exposed by the
	// official Python binding. Raw indices come from each concrete property's _get_* call,
	// while call and intrinsic operands that live in nested wrapper expressions use dedicated
	// derived kinds. The C++-only ASSERT/FORCE_VER accessors and generic bit/min/max operations
	// are included so all 154 installed-core operations have one authoritative description.
	internal static class LowLevelILDetailedOperandsTable
	{
		private static readonly ILOperandDescriptor[] NoOperands =
			Array.Empty<ILOperandDescriptor>();

		private static readonly ILOperandDescriptor[] UnarySource =
			new ILOperandDescriptor[]
			{
				new ILOperandDescriptor(
					"src", ILOperandKind.Expression, 0, "LowLevelILInstruction"),
			};

		private static readonly ILOperandDescriptor[] BinaryOperands =
			new ILOperandDescriptor[]
			{
				new ILOperandDescriptor(
					"left", ILOperandKind.Expression, 0, "LowLevelILInstruction"),
				new ILOperandDescriptor(
					"right", ILOperandKind.Expression, 1, "LowLevelILInstruction"),
			};

		private static readonly ILOperandDescriptor[] CarryOperands =
			new ILOperandDescriptor[]
			{
				new ILOperandDescriptor(
					"left", ILOperandKind.Expression, 0, "LowLevelILInstruction"),
				new ILOperandDescriptor(
					"right", ILOperandKind.Expression, 1, "LowLevelILInstruction"),
				new ILOperandDescriptor(
					"carry", ILOperandKind.Expression, 2, "LowLevelILInstruction"),
			};

		private static readonly ILOperandDescriptor[] ConstantOperand =
			new ILOperandDescriptor[]
			{
				new ILOperandDescriptor("constant", ILOperandKind.Integer, 0, "int"),
			};

		internal static readonly Dictionary<LowLevelILOperation, ILOperandDescriptor[]> Table =
			new Dictionary<LowLevelILOperation, ILOperandDescriptor[]>
			{
			{ LowLevelILOperation.LLIL_NOP, NoOperands },
			{ LowLevelILOperation.LLIL_SET_REG,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("dest", ILOperandKind.Register, 0, "ILRegister"),
					new ILOperandDescriptor("src", ILOperandKind.Expression, 1, "LowLevelILInstruction"),
				}
			},
			{ LowLevelILOperation.LLIL_SET_REG_SPLIT,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("hi", ILOperandKind.Register, 0, "ILRegister"),
					new ILOperandDescriptor("lo", ILOperandKind.Register, 1, "ILRegister"),
					new ILOperandDescriptor("src", ILOperandKind.Expression, 2, "LowLevelILInstruction"),
				}
			},
			{ LowLevelILOperation.LLIL_SET_FLAG,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("dest", ILOperandKind.Flag, 0, "ILFlag"),
					new ILOperandDescriptor("src", ILOperandKind.Expression, 1, "LowLevelILInstruction"),
				}
			},
			{ LowLevelILOperation.LLIL_SET_REG_STACK_REL,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("stack", ILOperandKind.RegisterStack, 0, "ILRegisterStack"),
					new ILOperandDescriptor("dest", ILOperandKind.Expression, 1, "LowLevelILInstruction"),
					new ILOperandDescriptor("src", ILOperandKind.Expression, 2, "LowLevelILInstruction"),
				}
			},
			{ LowLevelILOperation.LLIL_REG_STACK_PUSH,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("stack", ILOperandKind.RegisterStack, 0, "ILRegisterStack"),
					new ILOperandDescriptor("src", ILOperandKind.Expression, 1, "LowLevelILInstruction"),
				}
			},
			{ LowLevelILOperation.LLIL_ASSERT,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("src", ILOperandKind.Register, 0, "ILRegister"),
					new ILOperandDescriptor("constraint", ILOperandKind.PossibleValueSet, 1, "PossibleValueSet"),
				}
			},
			{ LowLevelILOperation.LLIL_FORCE_VER,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("dest", ILOperandKind.Register, 0, "ILRegister"),
				}
			},
			{ LowLevelILOperation.LLIL_LOAD, UnarySource },
			{ LowLevelILOperation.LLIL_STORE,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("dest", ILOperandKind.Expression, 0, "LowLevelILInstruction"),
					new ILOperandDescriptor("src", ILOperandKind.Expression, 1, "LowLevelILInstruction"),
				}
			},
			{ LowLevelILOperation.LLIL_PUSH, UnarySource },
			{ LowLevelILOperation.LLIL_POP, NoOperands },
			{ LowLevelILOperation.LLIL_REG,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("src", ILOperandKind.Register, 0, "ILRegister"),
				}
			},
			{ LowLevelILOperation.LLIL_REG_SPLIT,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("hi", ILOperandKind.Register, 0, "ILRegister"),
					new ILOperandDescriptor("lo", ILOperandKind.Register, 1, "ILRegister"),
				}
			},
			{ LowLevelILOperation.LLIL_REG_STACK_REL,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("stack", ILOperandKind.RegisterStack, 0, "ILRegisterStack"),
					new ILOperandDescriptor("src", ILOperandKind.Expression, 1, "LowLevelILInstruction"),
				}
			},
			{ LowLevelILOperation.LLIL_REG_STACK_POP,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("stack", ILOperandKind.RegisterStack, 0, "ILRegisterStack"),
				}
			},
			{ LowLevelILOperation.LLIL_REG_STACK_FREE_REG,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("dest", ILOperandKind.Register, 0, "ILRegister"),
				}
			},
			{ LowLevelILOperation.LLIL_REG_STACK_FREE_REL,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("stack", ILOperandKind.RegisterStack, 0, "ILRegisterStack"),
					new ILOperandDescriptor("dest", ILOperandKind.Expression, 1, "LowLevelILInstruction"),
				}
			},
			{ LowLevelILOperation.LLIL_CONST, ConstantOperand },
			{ LowLevelILOperation.LLIL_CONST_PTR, ConstantOperand },
			{ LowLevelILOperation.LLIL_EXTERN_PTR,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("constant", ILOperandKind.Integer, 0, "int"),
					new ILOperandDescriptor("offset", ILOperandKind.Integer, 1, "int"),
				}
			},
			{ LowLevelILOperation.LLIL_FLOAT_CONST,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("constant", ILOperandKind.Float, 0, "Union[int, float]"),
				}
			},
			{ LowLevelILOperation.LLIL_FLAG,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("src", ILOperandKind.Flag, 0, "ILFlag"),
				}
			},
			{ LowLevelILOperation.LLIL_FLAG_BIT,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("src", ILOperandKind.Flag, 0, "ILFlag"),
					new ILOperandDescriptor("bit", ILOperandKind.Integer, 1, "int"),
				}
			},
			{ LowLevelILOperation.LLIL_ADD, BinaryOperands },
			{ LowLevelILOperation.LLIL_ADC, CarryOperands },
			{ LowLevelILOperation.LLIL_SUB, BinaryOperands },
			{ LowLevelILOperation.LLIL_SBB, CarryOperands },
			{ LowLevelILOperation.LLIL_AND, BinaryOperands },
			{ LowLevelILOperation.LLIL_OR, BinaryOperands },
			{ LowLevelILOperation.LLIL_XOR, BinaryOperands },
			{ LowLevelILOperation.LLIL_LSL, BinaryOperands },
			{ LowLevelILOperation.LLIL_LSR, BinaryOperands },
			{ LowLevelILOperation.LLIL_ASR, BinaryOperands },
			{ LowLevelILOperation.LLIL_ROL, BinaryOperands },
			{ LowLevelILOperation.LLIL_RLC, CarryOperands },
			{ LowLevelILOperation.LLIL_ROR, BinaryOperands },
			{ LowLevelILOperation.LLIL_RRC, CarryOperands },
			{ LowLevelILOperation.LLIL_MUL, BinaryOperands },
			{ LowLevelILOperation.LLIL_MULU_DP, BinaryOperands },
			{ LowLevelILOperation.LLIL_MULS_DP, BinaryOperands },
			{ LowLevelILOperation.LLIL_DIVU, BinaryOperands },
			{ LowLevelILOperation.LLIL_DIVU_DP, BinaryOperands },
			{ LowLevelILOperation.LLIL_DIVS, BinaryOperands },
			{ LowLevelILOperation.LLIL_DIVS_DP, BinaryOperands },
			{ LowLevelILOperation.LLIL_MODU, BinaryOperands },
			{ LowLevelILOperation.LLIL_MODU_DP, BinaryOperands },
			{ LowLevelILOperation.LLIL_MODS, BinaryOperands },
			{ LowLevelILOperation.LLIL_MODS_DP, BinaryOperands },
			{ LowLevelILOperation.LLIL_NEG, UnarySource },
			{ LowLevelILOperation.LLIL_NOT, UnarySource },
			{ LowLevelILOperation.LLIL_SX, UnarySource },
			{ LowLevelILOperation.LLIL_ZX, UnarySource },
			{ LowLevelILOperation.LLIL_LOW_PART, UnarySource },
			{ LowLevelILOperation.LLIL_JUMP,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("dest", ILOperandKind.Expression, 0, "LowLevelILInstruction"),
				}
			},
			{ LowLevelILOperation.LLIL_JUMP_TO,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("dest", ILOperandKind.Expression, 0, "LowLevelILInstruction"),
					new ILOperandDescriptor("targets", ILOperandKind.TargetMap, 1, "Dict[int, int]"),
				}
			},
			{ LowLevelILOperation.LLIL_CALL,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("dest", ILOperandKind.Expression, 0, "LowLevelILInstruction"),
				}
			},
			{ LowLevelILOperation.LLIL_CALL_STACK_ADJUST,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("dest", ILOperandKind.Expression, 0, "LowLevelILInstruction"),
					new ILOperandDescriptor("stack_adjustment", ILOperandKind.Integer, 1, "int"),
					new ILOperandDescriptor("reg_stack_adjustments", ILOperandKind.RegisterStackAdjustments, 2, "Dict[RegisterStackName, int]"),
				}
			},
			{ LowLevelILOperation.LLIL_TAILCALL,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("dest", ILOperandKind.Expression, 0, "LowLevelILInstruction"),
				}
			},
			{ LowLevelILOperation.LLIL_RET,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("dest", ILOperandKind.Expression, 0, "LowLevelILInstruction"),
				}
			},
			{ LowLevelILOperation.LLIL_NORET, NoOperands },
			{ LowLevelILOperation.LLIL_IF,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("condition", ILOperandKind.Expression, 0, "LowLevelILInstruction"),
					new ILOperandDescriptor("true", ILOperandKind.Integer, 1, "InstructionIndex"),
					new ILOperandDescriptor("false", ILOperandKind.Integer, 2, "InstructionIndex"),
				}
			},
			{ LowLevelILOperation.LLIL_GOTO,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("dest", ILOperandKind.Integer, 0, "InstructionIndex"),
				}
			},
			{ LowLevelILOperation.LLIL_FLAG_COND,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("condition", ILOperandKind.FlagCondition, 0, "LowLevelILFlagCondition"),
					new ILOperandDescriptor("semantic_class", ILOperandKind.SemanticFlagClass, 1, "Optional[ILSemanticFlagClass]"),
				}
			},
			{ LowLevelILOperation.LLIL_FLAG_GROUP,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("semantic_group", ILOperandKind.SemanticFlagGroup, 0, "ILSemanticFlagGroup"),
				}
			},
			{ LowLevelILOperation.LLIL_CMP_E, BinaryOperands },
			{ LowLevelILOperation.LLIL_CMP_NE, BinaryOperands },
			{ LowLevelILOperation.LLIL_CMP_SLT, BinaryOperands },
			{ LowLevelILOperation.LLIL_CMP_ULT, BinaryOperands },
			{ LowLevelILOperation.LLIL_CMP_SLE, BinaryOperands },
			{ LowLevelILOperation.LLIL_CMP_ULE, BinaryOperands },
			{ LowLevelILOperation.LLIL_CMP_SGE, BinaryOperands },
			{ LowLevelILOperation.LLIL_CMP_UGE, BinaryOperands },
			{ LowLevelILOperation.LLIL_CMP_SGT, BinaryOperands },
			{ LowLevelILOperation.LLIL_CMP_UGT, BinaryOperands },
			{ LowLevelILOperation.LLIL_TEST_BIT, BinaryOperands },
			{ LowLevelILOperation.LLIL_BOOL_TO_INT, UnarySource },
			{ LowLevelILOperation.LLIL_ADD_OVERFLOW,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("left", ILOperandKind.LLILAddOverflowLeft, 0, "LowLevelILInstruction"),
					new ILOperandDescriptor("right", ILOperandKind.LLILAddOverflowRight, 0, "LowLevelILInstruction"),
				}
			},
			{ LowLevelILOperation.LLIL_SYSCALL, NoOperands },
			{ LowLevelILOperation.LLIL_BP, NoOperands },
			{ LowLevelILOperation.LLIL_TRAP,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("vector", ILOperandKind.Integer, 0, "int"),
				}
			},
			{ LowLevelILOperation.LLIL_INTRINSIC,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("output", ILOperandKind.RegisterOrFlagList, 0, "List[Union[ILFlag, ILRegister]]"),
					new ILOperandDescriptor("intrinsic", ILOperandKind.Intrinsic, 2, "ILIntrinsic"),
					new ILOperandDescriptor("params", ILOperandKind.LLILCallParamExpressions, 3, "List[LowLevelILInstruction]"),
				}
			},
			{ LowLevelILOperation.LLIL_UNDEF, NoOperands },
			{ LowLevelILOperation.LLIL_UNIMPL, NoOperands },
			{ LowLevelILOperation.LLIL_UNIMPL_MEM, UnarySource },
			{ LowLevelILOperation.LLIL_FADD, BinaryOperands },
			{ LowLevelILOperation.LLIL_FSUB, BinaryOperands },
			{ LowLevelILOperation.LLIL_FMUL, BinaryOperands },
			{ LowLevelILOperation.LLIL_FDIV, BinaryOperands },
			{ LowLevelILOperation.LLIL_FSQRT, UnarySource },
			{ LowLevelILOperation.LLIL_FNEG, UnarySource },
			{ LowLevelILOperation.LLIL_FABS, UnarySource },
			{ LowLevelILOperation.LLIL_FLOAT_TO_INT, UnarySource },
			{ LowLevelILOperation.LLIL_INT_TO_FLOAT, UnarySource },
			{ LowLevelILOperation.LLIL_FLOAT_CONV, UnarySource },
			{ LowLevelILOperation.LLIL_ROUND_TO_INT, UnarySource },
			{ LowLevelILOperation.LLIL_FLOOR, UnarySource },
			{ LowLevelILOperation.LLIL_CEIL, UnarySource },
			{ LowLevelILOperation.LLIL_FTRUNC, UnarySource },
			{ LowLevelILOperation.LLIL_FCMP_E, BinaryOperands },
			{ LowLevelILOperation.LLIL_FCMP_NE, BinaryOperands },
			{ LowLevelILOperation.LLIL_FCMP_LT, BinaryOperands },
			{ LowLevelILOperation.LLIL_FCMP_LE, BinaryOperands },
			{ LowLevelILOperation.LLIL_FCMP_GE, BinaryOperands },
			{ LowLevelILOperation.LLIL_FCMP_GT, BinaryOperands },
			{ LowLevelILOperation.LLIL_FCMP_O, BinaryOperands },
			{ LowLevelILOperation.LLIL_FCMP_UO, BinaryOperands },
			{ LowLevelILOperation.LLIL_SET_REG_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("dest", ILOperandKind.SSARegister, 0, 1, "SSARegister"),
					new ILOperandDescriptor("src", ILOperandKind.Expression, 2, "LowLevelILInstruction"),
				}
			},
			{ LowLevelILOperation.LLIL_SET_REG_SSA_PARTIAL,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("full_reg", ILOperandKind.SSARegister, 0, 1, "SSARegister"),
					new ILOperandDescriptor("dest", ILOperandKind.Register, 2, "ILRegister"),
					new ILOperandDescriptor("src", ILOperandKind.Expression, 3, "LowLevelILInstruction"),
				}
			},
			{ LowLevelILOperation.LLIL_SET_REG_SPLIT_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("hi", ILOperandKind.Expression, 0, "LowLevelILInstruction"),
					new ILOperandDescriptor("lo", ILOperandKind.Expression, 1, "LowLevelILInstruction"),
					new ILOperandDescriptor("src", ILOperandKind.Expression, 2, "LowLevelILInstruction"),
				}
			},
			{ LowLevelILOperation.LLIL_SET_REG_STACK_REL_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("stack", ILOperandKind.Expression, 0, "LowLevelILInstruction"),
					new ILOperandDescriptor("dest", ILOperandKind.Expression, 1, "LowLevelILInstruction"),
					new ILOperandDescriptor("top", ILOperandKind.Expression, 2, "LowLevelILInstruction"),
					new ILOperandDescriptor("src", ILOperandKind.Expression, 3, "LowLevelILInstruction"),
				}
			},
			{ LowLevelILOperation.LLIL_SET_REG_STACK_ABS_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("stack", ILOperandKind.Expression, 0, "LowLevelILInstruction"),
					new ILOperandDescriptor("dest", ILOperandKind.Register, 1, "ILRegister"),
					new ILOperandDescriptor("src", ILOperandKind.Expression, 2, "LowLevelILInstruction"),
				}
			},
			{ LowLevelILOperation.LLIL_REG_SPLIT_DEST_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("dest", ILOperandKind.SSARegister, 0, 1, "SSARegister"),
				}
			},
			{ LowLevelILOperation.LLIL_REG_STACK_DEST_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("dest", ILOperandKind.SSARegisterStack, 0, 1, "SSARegisterStack"),
					new ILOperandDescriptor("src", ILOperandKind.SSARegisterStack, 0, 2, "SSARegisterStack"),
				}
			},
			{ LowLevelILOperation.LLIL_REG_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("src", ILOperandKind.SSARegister, 0, 1, "SSARegister"),
				}
			},
			{ LowLevelILOperation.LLIL_REG_SSA_PARTIAL,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("full_reg", ILOperandKind.SSARegister, 0, 1, "SSARegister"),
					new ILOperandDescriptor("src", ILOperandKind.Register, 2, "ILRegister"),
				}
			},
			{ LowLevelILOperation.LLIL_REG_SPLIT_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("hi", ILOperandKind.SSARegister, 0, 1, "SSARegister"),
					new ILOperandDescriptor("lo", ILOperandKind.SSARegister, 2, 3, "SSARegister"),
				}
			},
			{ LowLevelILOperation.LLIL_REG_STACK_REL_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("stack", ILOperandKind.SSARegisterStack, 0, 1, "SSARegisterStack"),
					new ILOperandDescriptor("src", ILOperandKind.Expression, 2, "LowLevelILInstruction"),
					new ILOperandDescriptor("top", ILOperandKind.Expression, 3, "LowLevelILInstruction"),
				}
			},
			{ LowLevelILOperation.LLIL_REG_STACK_ABS_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("stack", ILOperandKind.SSARegisterStack, 0, 1, "SSARegisterStack"),
					new ILOperandDescriptor("src", ILOperandKind.Register, 2, "ILRegister"),
				}
			},
			{ LowLevelILOperation.LLIL_REG_STACK_FREE_REL_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("stack", ILOperandKind.Expression, 0, "LowLevelILInstruction"),
					new ILOperandDescriptor("dest", ILOperandKind.Expression, 1, "LowLevelILInstruction"),
					new ILOperandDescriptor("top", ILOperandKind.Expression, 2, "LowLevelILInstruction"),
				}
			},
			{ LowLevelILOperation.LLIL_REG_STACK_FREE_ABS_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("stack", ILOperandKind.Expression, 0, "LowLevelILInstruction"),
					new ILOperandDescriptor("dest", ILOperandKind.Register, 1, "ILRegister"),
				}
			},
			{ LowLevelILOperation.LLIL_SET_FLAG_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("dest", ILOperandKind.SSAFlag, 0, 1, "SSAFlag"),
					new ILOperandDescriptor("src", ILOperandKind.Expression, 2, "LowLevelILInstruction"),
				}
			},
			{ LowLevelILOperation.LLIL_ASSERT_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("src", ILOperandKind.SSARegister, 0, 1, "SSARegister"),
					new ILOperandDescriptor("constraint", ILOperandKind.PossibleValueSet, 2, "PossibleValueSet"),
				}
			},
			{ LowLevelILOperation.LLIL_FORCE_VER_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("dest", ILOperandKind.SSARegister, 0, 1, "SSARegister"),
					new ILOperandDescriptor("src", ILOperandKind.SSARegister, 2, 3, "SSARegister"),
				}
			},
			{ LowLevelILOperation.LLIL_FLAG_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("src", ILOperandKind.SSAFlag, 0, 1, "SSAFlag"),
				}
			},
			{ LowLevelILOperation.LLIL_FLAG_BIT_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("src", ILOperandKind.SSAFlag, 0, 1, "SSAFlag"),
					new ILOperandDescriptor("bit", ILOperandKind.Integer, 2, "int"),
				}
			},
			{ LowLevelILOperation.LLIL_CALL_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("output", ILOperandKind.LLILCallOutputSsaRegisters, 0, "List[SSARegister]"),
					new ILOperandDescriptor("dest", ILOperandKind.Expression, 1, "LowLevelILInstruction"),
					new ILOperandDescriptor("stack", ILOperandKind.Expression, 2, "LowLevelILInstruction"),
					new ILOperandDescriptor("params", ILOperandKind.LLILCallParamExpressions, 3, "List[LowLevelILInstruction]"),
				}
			},
			{ LowLevelILOperation.LLIL_SYSCALL_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("output", ILOperandKind.LLILCallOutputSsaRegisters, 0, "List[SSARegister]"),
					new ILOperandDescriptor("stack_reg", ILOperandKind.LLILCallStackSsaRegister, 1, "SSARegister"),
					new ILOperandDescriptor("stack_memory", ILOperandKind.LLILCallStackSsaMemory, 1, "int"),
					new ILOperandDescriptor("params", ILOperandKind.LLILCallParamExpressions, 2, "List[LowLevelILInstruction]"),
				}
			},
			{ LowLevelILOperation.LLIL_TAILCALL_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("output", ILOperandKind.LLILCallOutputSsaRegisters, 0, "List[SSARegister]"),
					new ILOperandDescriptor("dest", ILOperandKind.Expression, 1, "LowLevelILInstruction"),
					new ILOperandDescriptor("stack", ILOperandKind.Expression, 2, "LowLevelILInstruction"),
					new ILOperandDescriptor("params", ILOperandKind.LLILCallParamExpressions, 3, "List[LowLevelILInstruction]"),
				}
			},
			{ LowLevelILOperation.LLIL_CALL_PARAM,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("src", ILOperandKind.ExpressionList, 0, "List[LowLevelILInstruction]"),
				}
			},
			{ LowLevelILOperation.LLIL_CALL_STACK_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("src", ILOperandKind.SSARegister, 0, 1, "SSARegister"),
					new ILOperandDescriptor("src_memory", ILOperandKind.Integer, 2, "int"),
				}
			},
			{ LowLevelILOperation.LLIL_CALL_OUTPUT_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("dest_memory", ILOperandKind.Integer, 0, "int"),
					new ILOperandDescriptor("dest", ILOperandKind.SSARegisterList, 1, "List[SSARegister]"),
				}
			},
			{ LowLevelILOperation.LLIL_SEPARATE_PARAM_LIST_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("src", ILOperandKind.ExpressionList, 0, "List[LowLevelILInstruction]"),
				}
			},
			{ LowLevelILOperation.LLIL_SHARED_PARAM_SLOT_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("src", ILOperandKind.ExpressionList, 0, "List[LowLevelILInstruction]"),
				}
			},
			{ LowLevelILOperation.LLIL_MEMORY_INTRINSIC_OUTPUT_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("dest_memory", ILOperandKind.Integer, 0, "int"),
					new ILOperandDescriptor("output", ILOperandKind.SSARegisterOrFlagList, 1, "List[SSARegisterOrFlag]"),
				}
			},
			{ LowLevelILOperation.LLIL_LOAD_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("src", ILOperandKind.Expression, 0, "LowLevelILInstruction"),
					new ILOperandDescriptor("src_memory", ILOperandKind.Integer, 1, "int"),
				}
			},
			{ LowLevelILOperation.LLIL_STORE_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("dest", ILOperandKind.Expression, 0, "LowLevelILInstruction"),
					new ILOperandDescriptor("dest_memory", ILOperandKind.Integer, 1, "int"),
					new ILOperandDescriptor("src_memory", ILOperandKind.Integer, 2, "int"),
					new ILOperandDescriptor("src", ILOperandKind.Expression, 3, "LowLevelILInstruction"),
				}
			},
			{ LowLevelILOperation.LLIL_INTRINSIC_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("output", ILOperandKind.SSARegisterOrFlagList, 0, "List[SSARegisterOrFlag]"),
					new ILOperandDescriptor("intrinsic", ILOperandKind.Intrinsic, 2, "ILIntrinsic"),
					new ILOperandDescriptor("params", ILOperandKind.LLILCallParamExpressions, 3, "List[LowLevelILInstruction]"),
				}
			},
			{ LowLevelILOperation.LLIL_MEMORY_INTRINSIC_SSA,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("output", ILOperandKind.LLILMemoryIntrinsicOutputSsaRegisters, 0, "List[SSARegisterOrFlag]"),
					new ILOperandDescriptor("intrinsic", ILOperandKind.Intrinsic, 1, "ILIntrinsic"),
					new ILOperandDescriptor("params", ILOperandKind.LLILCallParamExpressions, 2, "List[LowLevelILInstruction]"),
					new ILOperandDescriptor("dest_memory", ILOperandKind.LLILMemoryIntrinsicOutputSsaMemory, 0, "int"),
					new ILOperandDescriptor("src_memory", ILOperandKind.Integer, 3, "int"),
				}
			},
			{ LowLevelILOperation.LLIL_REG_PHI,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("dest", ILOperandKind.SSARegister, 0, 1, "SSARegister"),
					new ILOperandDescriptor("src", ILOperandKind.SSARegisterList, 2, "List[SSARegister]"),
				}
			},
			{ LowLevelILOperation.LLIL_REG_STACK_PHI,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("dest", ILOperandKind.SSARegisterStack, 0, 1, "SSARegisterStack"),
					new ILOperandDescriptor("src", ILOperandKind.SSARegisterStackList, 2, "List[SSARegisterStack]"),
				}
			},
			{ LowLevelILOperation.LLIL_FLAG_PHI,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("dest", ILOperandKind.SSAFlag, 0, 1, "SSAFlag"),
					new ILOperandDescriptor("src", ILOperandKind.SSAFlagList, 2, "List[SSAFlag]"),
				}
			},
			{ LowLevelILOperation.LLIL_MEM_PHI,
				new ILOperandDescriptor[]
				{
					new ILOperandDescriptor("dest_memory", ILOperandKind.Integer, 0, "int"),
					new ILOperandDescriptor("src_memory", ILOperandKind.IntegerList, 1, "List[int]"),
				}
			},
			{ LowLevelILOperation.LLIL_BSWAP, UnarySource },
			{ LowLevelILOperation.LLIL_POPCNT, UnarySource },
			{ LowLevelILOperation.LLIL_CLZ, UnarySource },
			{ LowLevelILOperation.LLIL_CTZ, UnarySource },
			{ LowLevelILOperation.LLIL_RBIT, UnarySource },
			{ LowLevelILOperation.LLIL_CLS, UnarySource },
			{ LowLevelILOperation.LLIL_MINS, BinaryOperands },
			{ LowLevelILOperation.LLIL_MAXS, BinaryOperands },
			{ LowLevelILOperation.LLIL_MINU, BinaryOperands },
			{ LowLevelILOperation.LLIL_MAXU, BinaryOperands },
			{ LowLevelILOperation.LLIL_ABS, UnarySource },
			};
	}
}
