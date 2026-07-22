namespace BinaryNinja
{
	public sealed class MLILLogicalShiftLeft : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILLogicalShiftLeft(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
