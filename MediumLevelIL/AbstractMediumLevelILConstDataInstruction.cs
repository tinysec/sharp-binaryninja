namespace BinaryNinja
{
	public abstract class AbstractMediumLevelILConstDataInstruction : MediumLevelILInstruction
	{
		internal AbstractMediumLevelILConstDataInstruction(
			MediumLevelILFunction ilFunction,
			MediumLevelILExpressionIndex expressionIndex,
			BNMediumLevelILInstruction native)
			: base(ilFunction, expressionIndex, native)
		{
		}

		public RegisterValue ConstantData
		{
			get
			{
				return this.GetOperandAsConstantData((OperandIndex)0, (OperandIndex)1);
			}
		}

		public RegisterValue Constant
		{
			get
			{
				return this.ConstantData;
			}
		}
	}
}
