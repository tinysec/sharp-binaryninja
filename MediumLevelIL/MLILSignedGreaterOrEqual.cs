namespace BinaryNinja
{
	public sealed class MLILSignedGreaterOrEqual : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILSignedGreaterOrEqual(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
