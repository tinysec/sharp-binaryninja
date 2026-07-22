namespace BinaryNinja
{
	public sealed class MLILSetVariableAliased : MediumLevelILInstruction
	{
		internal MLILSetVariableAliased(
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
		
		public MediumLevelILInstruction Source
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)3);
			}
		}

		public override MediumLevelILVariable[] VariablesRead
		{
			get
			{
				return this.Source.VariablesRead;
			}
		}

		public override MediumLevelILSSAVariable[] SSAVariablesRead
		{
			get
			{
				return this.Source.SSAVariablesRead;
			}
		}

		public override MediumLevelILSSAVariable[] SSAVariablesWrite
		{
			get
			{
				return new MediumLevelILSSAVariable[] { this.Destination };
			}
		}
	}
}
