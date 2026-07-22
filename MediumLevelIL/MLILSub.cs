namespace BinaryNinja
{
	public sealed class MLILSub : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILSub(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
