namespace BinaryNinja
{
	public sealed class MLILFloatSquareRoot : AbstractMediumLevelILUnaryInstruction
	{
		internal MLILFloatSquareRoot(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
	}
}
