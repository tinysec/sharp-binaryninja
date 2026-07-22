namespace BinaryNinja
{
	public abstract class AbstractHighLevelILConstDataInstruction : HighLevelILInstruction
	{
		internal AbstractHighLevelILConstDataInstruction(
			HighLevelILFunction ilFunction,
			HighLevelILExpressionIndex expressionIndex,
			BNHighLevelILInstruction native)
			: base(ilFunction, expressionIndex, native)
		{
		}

		public RegisterValue Constant
		{
			get
			{
				return this.GetOperandAsConstantData(0, (OperandIndex)1);
			}
		}
	}
}
