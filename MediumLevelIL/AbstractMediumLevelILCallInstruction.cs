using System.Collections.Generic;

namespace BinaryNinja
{
	public abstract class AbstractMediumLevelILCallInstruction : MediumLevelILInstruction
	{
		internal AbstractMediumLevelILCallInstruction(
			MediumLevelILFunction ilFunction,
			MediumLevelILExpressionIndex expressionIndex,
			BNMediumLevelILInstruction native)
			: base(ilFunction, expressionIndex, native)
		{
		}

		public abstract MediumLevelILVariable[] Output { get; }

		public abstract MediumLevelILInstruction[] Parameters { get; }

		public override MediumLevelILVariable[] VariablesRead
		{
			get
			{
				List<MediumLevelILVariable> variables = new List<MediumLevelILVariable>();

				foreach (MediumLevelILInstruction parameter in this.Parameters)
				{
					variables.AddRange(parameter.VariablesRead);
				}

				return variables.ToArray();
			}
		}

		public override MediumLevelILVariable[] VariablesWrite
		{
			get
			{
				return this.Output;
			}
		}

		public override MediumLevelILSSAVariable[] SSAVariablesRead
		{
			get
			{
				List<MediumLevelILSSAVariable> variables = new List<MediumLevelILSSAVariable>();

				foreach (MediumLevelILInstruction parameter in this.Parameters)
				{
					variables.AddRange(parameter.SSAVariablesRead);
				}

				return variables.ToArray();
			}
		}
	}
}
