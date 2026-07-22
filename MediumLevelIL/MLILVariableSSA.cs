namespace BinaryNinja
{
	public sealed class MLILVariableSSA : MediumLevelILInstruction
	{
		internal MLILVariableSSA(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		public MediumLevelILSSAVariable Variable
		{
			get
			{
				return this.GetOperandAsSSAVariable((OperandIndex)0,(OperandIndex)1);
			}
		}

		public MediumLevelILSSAVariable Source
		{
			get
			{
				return this.Variable;
			}
		}
	}
}
