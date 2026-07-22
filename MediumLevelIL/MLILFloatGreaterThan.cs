namespace BinaryNinja
{
	public sealed class MLILFloatGreaterThan : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILFloatGreaterThan(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
