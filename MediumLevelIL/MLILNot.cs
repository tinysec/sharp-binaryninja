namespace BinaryNinja
{
	public sealed class MLILNot : AbstractMediumLevelILUnaryInstruction
	{
		internal MLILNot(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
	}
}
