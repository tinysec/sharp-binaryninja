namespace BinaryNinja
{
	public sealed class MLILCallOutput : MediumLevelILInstruction
	{
		internal MLILCallOutput(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		public MediumLevelILVariable[] Destination
		{
			get
			{
				return this.GetOperandAsVariableList(0);
			}
		}

		public override MediumLevelILVariable[] VariablesWrite
		{
			get
			{
				return this.Destination;
			}
		}
	}
}
