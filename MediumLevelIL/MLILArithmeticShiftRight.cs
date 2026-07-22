namespace BinaryNinja
{
	public sealed class MLILArithmeticShiftRight : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILArithmeticShiftRight(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
