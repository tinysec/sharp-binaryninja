namespace BinaryNinja
{
	public abstract class AbstractMediumLevelILConstInstruction : MediumLevelILInstruction
	{
		internal AbstractMediumLevelILConstInstruction(
			MediumLevelILFunction ilFunction,
			MediumLevelILExpressionIndex expressionIndex,
			BNMediumLevelILInstruction native)
			: base(ilFunction, expressionIndex, native)
		{
		}

		public ulong Constant
		{
			get
			{
				return this.RawOperands[0];
			}
		}
	}
}
