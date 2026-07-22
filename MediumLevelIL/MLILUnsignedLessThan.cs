namespace BinaryNinja
{
	public sealed class MLILUnsignedLessThan : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILUnsignedLessThan(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
