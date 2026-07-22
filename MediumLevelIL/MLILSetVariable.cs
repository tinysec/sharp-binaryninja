namespace BinaryNinja
{
	public sealed class MLILSetVariable : MediumLevelILInstruction
	{
		internal MLILSetVariable(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		public MediumLevelILVariable Destination
		{
			get
			{
				return this.GetOperandAsVariable(0);
			}
		}
		
		public MediumLevelILInstruction Source
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)1);
			}
		}

		public override MediumLevelILVariable[] VariablesRead
		{
			get
			{
				return this.Source.VariablesRead;
			}
		}

		public override MediumLevelILVariable[] VariablesWrite
		{
			get
			{
				return new MediumLevelILVariable[] { this.Destination };
			}
		}

		public override MediumLevelILSSAVariable[] SSAVariablesRead
		{
			get
			{
				return this.Source.SSAVariablesRead;
			}
		}
	}
}
