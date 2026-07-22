namespace BinaryNinja
{
	public sealed class MLILAddOverflow : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILAddOverflow(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
