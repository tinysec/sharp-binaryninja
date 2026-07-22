namespace BinaryNinja
{
	public sealed class MLILAdd : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILAdd(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
