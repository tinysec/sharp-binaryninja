namespace BinaryNinja
{
	public abstract class AbstractMediumLevelILCarryInstruction
		: AbstractMediumLevelILBinaryInstruction
	{
		internal AbstractMediumLevelILCarryInstruction(
			MediumLevelILFunction ilFunction,
			MediumLevelILExpressionIndex expressionIndex,
			BNMediumLevelILInstruction native)
			: base(ilFunction, expressionIndex, native)
		{
		}

		public MediumLevelILInstruction Carry
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)2);
			}
		}
	}
}
