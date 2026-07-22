namespace BinaryNinja
{
	public sealed class MLILSignedLessThan : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILSignedLessThan(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
