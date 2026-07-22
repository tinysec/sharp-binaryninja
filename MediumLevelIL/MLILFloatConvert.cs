namespace BinaryNinja
{
	public sealed class MLILFloatConvert : AbstractMediumLevelILUnaryInstruction
	{
		internal MLILFloatConvert(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
	}
}
