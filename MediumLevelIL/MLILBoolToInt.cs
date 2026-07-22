namespace BinaryNinja
{
	public sealed class MLILBoolToInt : AbstractMediumLevelILUnaryInstruction
	{
		internal MLILBoolToInt(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
	}
}
