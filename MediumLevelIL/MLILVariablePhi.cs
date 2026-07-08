namespace BinaryNinja
{
	public sealed class MLILVariablePhi : MediumLevelILInstruction
	{
		internal MLILVariablePhi(
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
		
		public MediumLevelILSSAVariable[] Source
		{
			get
			{
				return this.GetOperandAsSSAVariableList((OperandIndex)2);
			}
		}
	}
}
