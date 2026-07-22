namespace BinaryNinja
{
	public sealed class MLILMulSignedDoublePrecision : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILMulSignedDoublePrecision(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
