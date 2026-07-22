namespace BinaryNinja
{
	public sealed class MLILFloatLessThanOrEqual : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILFloatLessThanOrEqual(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
