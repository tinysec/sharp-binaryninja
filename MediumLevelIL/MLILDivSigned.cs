namespace BinaryNinja
{
	public sealed class MLILDivSigned : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILDivSigned(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
