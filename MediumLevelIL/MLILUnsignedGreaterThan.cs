namespace BinaryNinja
{
	public sealed class MLILUnsignedGreaterThan : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILUnsignedGreaterThan(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
