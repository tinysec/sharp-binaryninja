namespace BinaryNinja
{
	public sealed class MLILFloatAdd : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILFloatAdd(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
