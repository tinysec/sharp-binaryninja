namespace BinaryNinja
{
	public sealed class MLILImport : AbstractMediumLevelILConstInstruction
	{
		internal MLILImport(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}

	}
}
