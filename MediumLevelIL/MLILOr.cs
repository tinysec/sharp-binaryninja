namespace BinaryNinja
{
	public sealed class MLILOr : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILOr(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
