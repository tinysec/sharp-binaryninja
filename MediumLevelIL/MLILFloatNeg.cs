namespace BinaryNinja
{
	public sealed class MLILFloatNeg : AbstractMediumLevelILUnaryInstruction
	{
		internal MLILFloatNeg(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
	}
}
