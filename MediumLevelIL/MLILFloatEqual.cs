namespace BinaryNinja
{
	public sealed class MLILFloatEqual : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILFloatEqual(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
