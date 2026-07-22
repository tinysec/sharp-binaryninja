namespace BinaryNinja
{
	public sealed class MLILRotateLeft : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILRotateLeft(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
