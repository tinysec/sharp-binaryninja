namespace BinaryNinja
{
	public sealed class MLILLowPart : AbstractMediumLevelILUnaryInstruction
	{
		internal MLILLowPart(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
	}
}
