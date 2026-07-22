namespace BinaryNinja
{
	public sealed class MLILSignedGreaterThan : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILSignedGreaterThan(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
