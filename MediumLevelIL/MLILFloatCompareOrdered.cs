namespace BinaryNinja
{
	public sealed class MLILFloatCompareOrdered : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILFloatCompareOrdered(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
