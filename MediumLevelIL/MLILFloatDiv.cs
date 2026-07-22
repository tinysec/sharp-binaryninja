namespace BinaryNinja
{
	public sealed class MLILFloatDiv : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILFloatDiv(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
