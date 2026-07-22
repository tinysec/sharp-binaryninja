namespace BinaryNinja
{
	public sealed class MLILAddCarry : AbstractMediumLevelILCarryInstruction
	{
		internal MLILAddCarry(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
		
	}
}
