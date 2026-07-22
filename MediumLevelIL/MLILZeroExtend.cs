namespace BinaryNinja
{
	public sealed class MLILZeroExtend : AbstractMediumLevelILUnaryInstruction
	{
		internal MLILZeroExtend(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
	}
}
