namespace BinaryNinja
{
	public sealed class MLILFloatCompareUnordered : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILFloatCompareUnordered(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
