namespace BinaryNinja
{
	public sealed class MLILSignExtend : AbstractMediumLevelILUnaryInstruction
	{
		internal MLILSignExtend(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
	}
}
