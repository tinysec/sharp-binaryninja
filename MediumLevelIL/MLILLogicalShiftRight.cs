namespace BinaryNinja
{
	public sealed class MLILLogicalShiftRight : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILLogicalShiftRight(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
