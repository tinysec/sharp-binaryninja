namespace BinaryNinja
{
	public sealed class MLILNotEqual : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILNotEqual(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
