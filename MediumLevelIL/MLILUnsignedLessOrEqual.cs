namespace BinaryNinja
{
	public sealed class MLILUnsignedLessOrEqual : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILUnsignedLessOrEqual(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
