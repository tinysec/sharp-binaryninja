namespace BinaryNinja
{
	public sealed class MLILFloatLessThan : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILFloatLessThan(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
