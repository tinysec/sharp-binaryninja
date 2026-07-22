namespace BinaryNinja
{
	public sealed class MLILModSigned : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILModSigned(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
