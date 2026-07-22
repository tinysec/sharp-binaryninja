namespace BinaryNinja
{
	public sealed class MLILIntToFloat : AbstractMediumLevelILUnaryInstruction
	{
		internal MLILIntToFloat(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
	}
}
