namespace BinaryNinja
{
	public sealed class MLILFloatGreaterOrEqual : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILFloatGreaterOrEqual(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
