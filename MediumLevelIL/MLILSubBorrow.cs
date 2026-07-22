namespace BinaryNinja
{
	public sealed class MLILSubBorrow : AbstractMediumLevelILCarryInstruction
	{
		internal MLILSubBorrow(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
		
	}
}
