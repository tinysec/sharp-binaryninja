namespace BinaryNinja
{
	public sealed class MLILNeg : AbstractMediumLevelILUnaryInstruction
	{
		internal MLILNeg(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
	}
}
