namespace BinaryNinja
{
	public sealed class MLILAddressOf : MediumLevelILInstruction
	{
		internal MLILAddressOf(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		public MediumLevelILVariable Source
		{
			get
			{
				return this.GetOperandAsVariable(0);
			}
		}

		public override MediumLevelILVariable[] VariablesAddressTaken
		{
			get
			{
				return new MediumLevelILVariable[] { this.Source };
			}
		}
	}
}
