using System.Collections.Generic;

namespace BinaryNinja
{
	public sealed class MLILSetVariableSSAField : MediumLevelILInstruction
	{
		internal MLILSetVariableSSAField(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		public MediumLevelILSSAVariable Destination
		{
			get
			{
				return this.GetOperandAsSSAVariable((OperandIndex)0,(OperandIndex)1);
			}
		}
		
		public MediumLevelILSSAVariable Prev
		{
			get
			{
				return this.GetOperandAsSSAVariable((OperandIndex)0,(OperandIndex)2);
			}
		}
		
		public ulong Offset
		{
			get
			{
				return (ulong)this.RawOperands[3];
			}
		}
		
		public MediumLevelILInstruction Source
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)4);
			}
		}

		public override MediumLevelILSSAVariable[] SSAVariablesRead
		{
			get
			{
				List<MediumLevelILSSAVariable> variables = new List<MediumLevelILSSAVariable>();
				
				variables.Add(this.Prev);
				
				variables.AddRange(this.Source.SSAVariablesRead);
				
				return variables.ToArray();
			}
		}

		public override MediumLevelILVariable[] VariablesRead
		{
			get
			{
				return this.Source.VariablesRead;
			}
		}
		
		public override MediumLevelILSSAVariable[]  SSAVariablesWrite
		{
			get
			{
				return new MediumLevelILSSAVariable[]
				{
					this.Destination
				};
			}
		}
	}
}
