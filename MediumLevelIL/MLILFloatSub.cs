namespace BinaryNinja
{
	public sealed class MLILFloatSub : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILFloatSub(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
