namespace BinaryNinja
{
	public sealed class MLILMul : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILMul(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
