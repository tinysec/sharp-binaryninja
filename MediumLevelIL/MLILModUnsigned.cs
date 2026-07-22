namespace BinaryNinja
{
	public sealed class MLILModUnsigned : AbstractMediumLevelILBinaryInstruction
	{
		internal MLILModUnsigned(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
	}
}
