namespace BinaryNinja
{
	public sealed class MLILConstData : AbstractMediumLevelILConstDataInstruction
	{
		internal MLILConstData(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		

	}
}
