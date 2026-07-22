namespace BinaryNinja
{
	public abstract class AbstractMediumLevelILUnaryInstruction : MediumLevelILInstruction
	{
		internal AbstractMediumLevelILUnaryInstruction(
			MediumLevelILFunction ilFunction,
			MediumLevelILExpressionIndex expressionIndex,
			BNMediumLevelILInstruction native)
			: base(ilFunction, expressionIndex, native)
		{
		}

		public MediumLevelILInstruction Source
		{
			get
			{
				return this.GetOperandAsExpression(0);
			}
		}
	}
}
