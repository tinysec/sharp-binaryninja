namespace BinaryNinja
{
	public sealed class MLILTestBit : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILTestBit(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
