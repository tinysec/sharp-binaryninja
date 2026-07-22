namespace BinaryNinja
{
	public sealed class MLILFloatTruncate : AbstractMediumLevelILUnaryInstruction
	{
		internal MLILFloatTruncate(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
	}
}
