namespace BinaryNinja
{
	public sealed class MLILFloatMul : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILFloatMul(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
