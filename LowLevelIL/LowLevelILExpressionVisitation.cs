using System;

namespace BinaryNinja
{
	/// <summary>
	/// Visits one LLIL expression and returns whether its children should also be visited.
	/// </summary>
	public delegate bool LowLevelILExpressionVisitor(LowLevelILInstruction expression);

	public abstract partial class LowLevelILInstruction
	{
		/// <summary>
		/// Visits this expression tree in preorder.
		/// </summary>
		public void VisitExpressions(LowLevelILExpressionVisitor visitor)
		{
			if (null == visitor)
			{
				throw new ArgumentNullException(nameof(visitor));
			}

			if (false == visitor(this))
			{
				return;
			}

			switch (this.Operation)
			{
				case LowLevelILOperation.LLIL_SET_REG:
				case LowLevelILOperation.LLIL_SET_FLAG:
				case LowLevelILOperation.LLIL_REG_STACK_PUSH:
					this.VisitExpressionAt(1, visitor);
					break;

				case LowLevelILOperation.LLIL_SET_REG_SPLIT:
				case LowLevelILOperation.LLIL_SET_REG_SSA:
				case LowLevelILOperation.LLIL_SET_FLAG_SSA:
					this.VisitExpressionAt(2, visitor);
					break;

				case LowLevelILOperation.LLIL_SET_REG_SSA_PARTIAL:
					this.VisitExpressionAt(3, visitor);
					break;

				case LowLevelILOperation.LLIL_SET_REG_SPLIT_SSA:
					((LLILSetRegisterSplitSSA)this).Source.VisitExpressions(visitor);
					break;

				case LowLevelILOperation.LLIL_SET_REG_STACK_REL:
					this.VisitExpressionAt(1, visitor);
					this.VisitExpressionAt(2, visitor);
					break;

				case LowLevelILOperation.LLIL_SET_REG_STACK_REL_SSA:
					this.VisitExpressionAt(1, visitor);
					this.VisitExpressionAt(3, visitor);
					break;

				case LowLevelILOperation.LLIL_SET_REG_STACK_ABS_SSA:
					this.VisitExpressionAt(2, visitor);
					break;

				case LowLevelILOperation.LLIL_REG_STACK_REL:
				case LowLevelILOperation.LLIL_REG_STACK_FREE_REL:
					this.VisitExpressionAt(1, visitor);
					break;

				case LowLevelILOperation.LLIL_REG_STACK_REL_SSA:
					this.VisitExpressionAt(2, visitor);
					break;

				case LowLevelILOperation.LLIL_REG_STACK_FREE_REL_SSA:
					this.VisitExpressionAt(1, visitor);
					break;

				case LowLevelILOperation.LLIL_LOAD:
				case LowLevelILOperation.LLIL_LOAD_SSA:
				case LowLevelILOperation.LLIL_JUMP:
				case LowLevelILOperation.LLIL_JUMP_TO:
				case LowLevelILOperation.LLIL_IF:
				case LowLevelILOperation.LLIL_CALL:
				case LowLevelILOperation.LLIL_CALL_STACK_ADJUST:
				case LowLevelILOperation.LLIL_TAILCALL:
				case LowLevelILOperation.LLIL_RET:
				case LowLevelILOperation.LLIL_PUSH:
				case LowLevelILOperation.LLIL_NEG:
				case LowLevelILOperation.LLIL_NOT:
				case LowLevelILOperation.LLIL_SX:
				case LowLevelILOperation.LLIL_ZX:
				case LowLevelILOperation.LLIL_LOW_PART:
				case LowLevelILOperation.LLIL_BOOL_TO_INT:
				case LowLevelILOperation.LLIL_UNIMPL_MEM:
				case LowLevelILOperation.LLIL_FSQRT:
				case LowLevelILOperation.LLIL_FNEG:
				case LowLevelILOperation.LLIL_FABS:
				case LowLevelILOperation.LLIL_FLOAT_TO_INT:
				case LowLevelILOperation.LLIL_INT_TO_FLOAT:
				case LowLevelILOperation.LLIL_FLOAT_CONV:
				case LowLevelILOperation.LLIL_ROUND_TO_INT:
				case LowLevelILOperation.LLIL_FLOOR:
				case LowLevelILOperation.LLIL_CEIL:
				case LowLevelILOperation.LLIL_FTRUNC:
				case LowLevelILOperation.LLIL_BSWAP:
				case LowLevelILOperation.LLIL_POPCNT:
				case LowLevelILOperation.LLIL_CLZ:
				case LowLevelILOperation.LLIL_CTZ:
				case LowLevelILOperation.LLIL_RBIT:
				case LowLevelILOperation.LLIL_CLS:
				case LowLevelILOperation.LLIL_ABS:
					this.VisitExpressionAt(0, visitor);
					break;

				case LowLevelILOperation.LLIL_STORE:
					this.VisitExpressionAt(0, visitor);
					this.VisitExpressionAt(1, visitor);
					break;

				case LowLevelILOperation.LLIL_STORE_SSA:
					this.VisitExpressionAt(0, visitor);
					this.VisitExpressionAt(3, visitor);
					break;

				case LowLevelILOperation.LLIL_ADD:
				case LowLevelILOperation.LLIL_SUB:
				case LowLevelILOperation.LLIL_AND:
				case LowLevelILOperation.LLIL_OR:
				case LowLevelILOperation.LLIL_XOR:
				case LowLevelILOperation.LLIL_LSL:
				case LowLevelILOperation.LLIL_LSR:
				case LowLevelILOperation.LLIL_ASR:
				case LowLevelILOperation.LLIL_ROL:
				case LowLevelILOperation.LLIL_ROR:
				case LowLevelILOperation.LLIL_MUL:
				case LowLevelILOperation.LLIL_MULU_DP:
				case LowLevelILOperation.LLIL_MULS_DP:
				case LowLevelILOperation.LLIL_DIVU:
				case LowLevelILOperation.LLIL_DIVS:
				case LowLevelILOperation.LLIL_MODU:
				case LowLevelILOperation.LLIL_MODS:
				case LowLevelILOperation.LLIL_DIVU_DP:
				case LowLevelILOperation.LLIL_DIVS_DP:
				case LowLevelILOperation.LLIL_MODU_DP:
				case LowLevelILOperation.LLIL_MODS_DP:
				case LowLevelILOperation.LLIL_CMP_E:
				case LowLevelILOperation.LLIL_CMP_NE:
				case LowLevelILOperation.LLIL_CMP_SLT:
				case LowLevelILOperation.LLIL_CMP_ULT:
				case LowLevelILOperation.LLIL_CMP_SLE:
				case LowLevelILOperation.LLIL_CMP_ULE:
				case LowLevelILOperation.LLIL_CMP_SGE:
				case LowLevelILOperation.LLIL_CMP_UGE:
				case LowLevelILOperation.LLIL_CMP_SGT:
				case LowLevelILOperation.LLIL_CMP_UGT:
				case LowLevelILOperation.LLIL_TEST_BIT:
				case LowLevelILOperation.LLIL_FADD:
				case LowLevelILOperation.LLIL_FSUB:
				case LowLevelILOperation.LLIL_FMUL:
				case LowLevelILOperation.LLIL_FDIV:
				case LowLevelILOperation.LLIL_FCMP_E:
				case LowLevelILOperation.LLIL_FCMP_NE:
				case LowLevelILOperation.LLIL_FCMP_LT:
				case LowLevelILOperation.LLIL_FCMP_LE:
				case LowLevelILOperation.LLIL_FCMP_GE:
				case LowLevelILOperation.LLIL_FCMP_GT:
				case LowLevelILOperation.LLIL_FCMP_O:
				case LowLevelILOperation.LLIL_FCMP_UO:
				case LowLevelILOperation.LLIL_MINS:
				case LowLevelILOperation.LLIL_MAXS:
				case LowLevelILOperation.LLIL_MINU:
				case LowLevelILOperation.LLIL_MAXU:
					this.VisitExpressionAt(0, visitor);
					this.VisitExpressionAt(1, visitor);
					break;

				case LowLevelILOperation.LLIL_ADD_OVERFLOW:
					LLILAddOverflow overflow = (LLILAddOverflow)this;
					overflow.Left.VisitExpressions(visitor);
					overflow.Right.VisitExpressions(visitor);
					break;

				case LowLevelILOperation.LLIL_ADC:
				case LowLevelILOperation.LLIL_SBB:
				case LowLevelILOperation.LLIL_RLC:
				case LowLevelILOperation.LLIL_RRC:
					this.VisitExpressionAt(0, visitor);
					this.VisitExpressionAt(1, visitor);
					this.VisitExpressionAt(2, visitor);
					break;

				case LowLevelILOperation.LLIL_CALL_SSA:
					LLILCallSSA call = (LLILCallSSA)this;
					call.Destination.VisitExpressions(visitor);
					this.VisitExpressionList(call.Parameters, visitor);
					break;

				case LowLevelILOperation.LLIL_SYSCALL_SSA:
					this.VisitExpressionList(((LLILSysCallSSA)this).Parameters, visitor);
					break;

				case LowLevelILOperation.LLIL_TAILCALL_SSA:
					LLILTailCallSSA tailCall = (LLILTailCallSSA)this;
					tailCall.Destination.VisitExpressions(visitor);
					this.VisitExpressionList(tailCall.Parameters, visitor);
					break;

				case LowLevelILOperation.LLIL_INTRINSIC:
					this.VisitExpressionList(((LLILIntrinsic)this).Parameters, visitor);
					break;

				case LowLevelILOperation.LLIL_INTRINSIC_SSA:
					this.VisitExpressionList(((LLILIntrinsicSSA)this).Parameters, visitor);
					break;

				case LowLevelILOperation.LLIL_MEMORY_INTRINSIC_SSA:
					this.VisitExpressionList(((LLILMemoryIntrinsicSSA)this).Parameters, visitor);
					break;

				case LowLevelILOperation.LLIL_CALL_PARAM:
					this.VisitExpressionList(((LLILCallParameter)this).Source, visitor);
					break;

				case LowLevelILOperation.LLIL_SEPARATE_PARAM_LIST_SSA:
					this.VisitExpressionList(((LLILSeparateParamListSSA)this).Source, visitor);
					break;

				case LowLevelILOperation.LLIL_SHARED_PARAM_SLOT_SSA:
					this.VisitExpressionList(((LLILSharedParamSlotSSA)this).Source, visitor);
					break;
			}
		}

		/// <summary>
		/// Compatibility spelling matching the official C++ API.
		/// </summary>
		public void VisitExprs(LowLevelILExpressionVisitor visitor)
		{
			this.VisitExpressions(visitor);
		}

		private void VisitExpressionAt(
			ulong operand,
			LowLevelILExpressionVisitor visitor)
		{
			this.GetOperandAsExpression((OperandIndex)operand).VisitExpressions(visitor);
		}

		private void VisitExpressionList(
			LowLevelILInstruction[] expressions,
			LowLevelILExpressionVisitor visitor)
		{
			foreach (LowLevelILInstruction expression in expressions)
			{
				expression.VisitExpressions(visitor);
			}
		}
	}
}
