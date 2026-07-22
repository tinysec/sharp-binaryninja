namespace BinaryNinja
{
	public sealed class MLILDivUnsigned : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILDivUnsigned(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
