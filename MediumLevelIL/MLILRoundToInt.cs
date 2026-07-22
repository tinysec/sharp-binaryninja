namespace BinaryNinja
{
	public sealed class MLILRoundToInt : AbstractMediumLevelILUnaryInstruction
	{
		internal MLILRoundToInt(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
	}
}
