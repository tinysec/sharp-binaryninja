namespace BinaryNinja
{
	public sealed class MLILFloor : AbstractMediumLevelILUnaryInstruction
	{
		internal MLILFloor(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
	}
}
