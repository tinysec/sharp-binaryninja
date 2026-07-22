namespace BinaryNinja
{
	public sealed class MLILSetVariableSSA : MediumLevelILInstruction
	{
		internal MLILSetVariableSSA(
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
				return this.GetOperandAsSSAVariable((OperandIndex)0 , (OperandIndex)1);
			}
		}
		
		public MediumLevelILInstruction Source
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)2);
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
