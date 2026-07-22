using System.Collections;
using System.Collections.Generic;

namespace BinaryNinja
{
	public abstract partial class HighLevelILInstruction
	{
		/// <summary>
		/// The direct sub-instruction operands of this instruction, mirroring Python
		/// HighLevelILInstruction.instruction_operands. List operands are flattened and only their
		/// instruction elements are kept; scalar operands are dropped.
		/// </summary>
		public IList<HighLevelILInstruction> InstructionOperands
		{
			get
			{
				List<HighLevelILInstruction> result = new List<HighLevelILInstruction>();

				foreach (object? operand in this.Operands)
				{
					if (operand is HighLevelILInstruction instruction)
					{
						result.Add(instruction);
					}
					else if (operand is IList list)
					{
						foreach (object? element in list)
						{
							if (element is HighLevelILInstruction elementInstruction)
							{
								result.Add(elementInstruction);
							}
						}
					}
				}

				return result;
			}
		}

		/// <summary>
		/// Non-unique list of variables used by this instruction, mirroring Python
		/// HighLevelILInstruction.vars. <see cref="IHighLevelILVariable"/> preserves the official
		/// binding's union of regular and SSA variables without losing static type safety.
		/// </summary>
		public virtual IList<IHighLevelILVariable> Vars
		{
			get
			{
				List<IHighLevelILVariable> result = new List<IHighLevelILVariable>();

				foreach (object? operand in this.Operands)
				{
					CollectVars(operand, result);
				}

				return result;
			}
		}

		/// <summary>
		/// Variables written by this instruction. The base implementation recurses over direct
		/// sub-instruction operands; assignment-shaped instructions override it for destinations.
		/// </summary>
		public virtual IList<IHighLevelILVariable> VarsWritten
		{
			get
			{
				List<IHighLevelILVariable> result = new List<IHighLevelILVariable>();

				foreach (object? operand in this.Operands)
				{
					if (operand is HighLevelILInstruction instruction)
					{
						result.AddRange(instruction.VarsWritten);
					}
				}

				return result;
			}
		}

		/// <summary>
		/// Non-unique list of variables whose address is taken by this instruction.
		/// </summary>
		public virtual IList<IHighLevelILVariable> VarsAddressTaken
		{
			get
			{
				List<IHighLevelILVariable> result = new List<IHighLevelILVariable>();

				foreach (HighLevelILInstruction instruction in this.InstructionOperands)
				{
					result.AddRange(instruction.VarsAddressTaken);
				}

				return result;
			}
		}

		/// <summary>
		/// Non-unique list of variables whose value is read by this instruction. Computed by
		/// multiset subtraction of written and address-taken variables from <see cref="Vars"/>.
		/// </summary>
		public IList<IHighLevelILVariable> VarsRead
		{
			get
			{
				List<IHighLevelILVariable> nonRead = new List<IHighLevelILVariable>();
				nonRead.AddRange(this.VarsWritten);
				nonRead.AddRange(this.VarsAddressTaken);

				List<IHighLevelILVariable> result = new List<IHighLevelILVariable>();
				foreach (IHighLevelILVariable variable in this.Vars)
				{
					if (nonRead.Remove(variable))
					{
						continue;
					}

					result.Add(variable);
				}

				return result;
			}
		}

		private static void CollectVars(
			object? operand,
			List<IHighLevelILVariable> result
		)
		{
			if (operand is HighLevelILInstruction instruction)
			{
				result.AddRange(instruction.Vars);
			}
			else if (operand is HighLevelILVariable variable)
			{
				result.Add(variable);
			}
			else if (operand is HighLevelILSSAVariable ssaVariable)
			{
				result.Add(ssaVariable);
			}
			else if (operand is IList list)
			{
				foreach (object? element in list)
				{
					CollectVars(element, result);
				}
			}
		}
	}
}
