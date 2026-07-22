namespace BinaryNinja
{
	public sealed class MLILEqual : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILEqual(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
