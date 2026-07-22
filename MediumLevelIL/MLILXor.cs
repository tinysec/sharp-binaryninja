namespace BinaryNinja
{
	public sealed class MLILXor : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILXor(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
