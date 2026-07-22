using System.Collections.Generic;
using System.Numerics;

namespace BinaryNinja
{
	public abstract partial class HighLevelILInstruction
	{
		public IList<object?> PrefixOperands
		{
			get
			{
				List<object?> result = new List<object?>();
				result.Add(new HighLevelILOperationAndSize(this.Operation, this.Size));

				foreach (object? operand in this.Operands)
				{
					if (operand is HighLevelILInstruction instruction)
					{
						result.AddRange(instruction.PrefixOperands);
					}
					else
					{
						result.Add(operand);
					}
				}

				return result;
			}
		}

		public IList<object?> PostfixOperands
		{
			get
			{
				List<object?> result = new List<object?>();

				foreach (object? operand in this.Operands)
				{
					if (operand is HighLevelILInstruction instruction)
					{
						result.AddRange(instruction.PostfixOperands);
					}
					else
					{
						result.Add(operand);
					}
				}

				result.Add(new HighLevelILOperationAndSize(this.Operation, this.Size));

				return result;
			}
		}

		public HighLevelILInstruction Ast
		{
			get
			{
				return this.AsAST();
			}
		}

		public HighLevelILInstruction NonAst
		{
			get
			{
				return this.AsNonAST();
			}
		}

		public HighLevelILInstruction AsAST()
		{
			if (this.AsFullAst)
			{
				return this;
			}

			return this.ILFunction.MustGetExpression(this.ExpressionIndex, true);
		}

		public HighLevelILInstruction AsNonAST()
		{
			if (false == this.AsFullAst)
			{
				return this;
			}

			return this.ILFunction.MustGetExpression(this.ExpressionIndex, false);
		}

		public bool CanCollapse
		{
			get
			{
				return HighLevelILInstruction.CanCollapseOperation(this.Operation);
			}
		}

		public static bool CanCollapseOperation(HighLevelILOperation operation)
		{
			switch (operation)
			{
				case HighLevelILOperation.HLIL_IF:
				case HighLevelILOperation.HLIL_WHILE:
				case HighLevelILOperation.HLIL_WHILE_SSA:
				case HighLevelILOperation.HLIL_DO_WHILE:
				case HighLevelILOperation.HLIL_DO_WHILE_SSA:
				case HighLevelILOperation.HLIL_FOR:
				case HighLevelILOperation.HLIL_FOR_SSA:
				case HighLevelILOperation.HLIL_SWITCH:
				case HighLevelILOperation.HLIL_CASE:
				{
					return true;
				}
				default:
				{
					return false;
				}
			}
		}

		public ulong GetInstructionHash(ulong discriminator = 0)
		{
			ulong hash = (ulong)this.Operation;
			hash ^= BitOperations.RotateLeft(this.Address, 23);
			hash ^= BitOperations.RotateLeft(discriminator, 47);

			return hash;
		}
	}
}
