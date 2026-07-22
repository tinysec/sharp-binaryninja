namespace BinaryNinja
{
	public sealed class MLILAnd : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILAnd(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
