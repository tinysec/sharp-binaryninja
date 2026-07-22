namespace BinaryNinja
{
	public sealed class MLILFloatAbs : AbstractMediumLevelILUnaryInstruction
	{
		internal MLILFloatAbs(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
	}
}
