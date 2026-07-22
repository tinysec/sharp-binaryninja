namespace BinaryNinja
{
	public sealed class MLILUnimplementedMemory : AbstractMediumLevelILUnaryInstruction
	{
		internal MLILUnimplementedMemory(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
	}
}
