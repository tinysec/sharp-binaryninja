namespace BinaryNinja
{
	public sealed class MLILFloatNotEqual : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILFloatNotEqual(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
