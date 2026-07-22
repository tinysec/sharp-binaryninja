namespace BinaryNinja
{
	public sealed class MLILUnsignedGreaterOrEqual : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILUnsignedGreaterOrEqual(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
