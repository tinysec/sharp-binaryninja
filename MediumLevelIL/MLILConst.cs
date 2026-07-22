namespace BinaryNinja
{
	public sealed class MLILConst : AbstractMediumLevelILConstInstruction
	{
		internal MLILConst(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
	}
}
