namespace BinaryNinja
{
	public sealed class MLILCeil : AbstractMediumLevelILUnaryInstruction
	{
		internal MLILCeil(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
	}
}
