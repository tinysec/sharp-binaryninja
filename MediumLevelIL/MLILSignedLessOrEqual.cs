namespace BinaryNinja
{
	public sealed class MLILSignedLessOrEqual : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILSignedLessOrEqual(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
