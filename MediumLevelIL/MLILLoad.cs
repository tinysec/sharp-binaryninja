namespace BinaryNinja
{
	public sealed class MLILLoad : AbstractMediumLevelILUnaryInstruction
	{
		internal MLILLoad(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
	}
}
