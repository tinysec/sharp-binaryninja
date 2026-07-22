namespace BinaryNinja
{
	public sealed class MLILAssertSSA : MediumLevelILInstruction
	{
		internal MLILAssertSSA(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		public MediumLevelILSSAVariable Source
		{
			get
			{
				return this.GetOperandAsSSAVariable((OperandIndex)0,(OperandIndex)1);
			}
		}
	
		public PossibleValueSet Constraint
		{
			get
			{
				return this.GetOperandAsPossibleValueSet((OperandIndex)2);
			}
		}
	}
}
