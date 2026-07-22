namespace BinaryNinja
{
	public sealed class MLILVariable : MediumLevelILInstruction
	{
		internal MLILVariable(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		public MediumLevelILVariable Variable
		{
			get
			{
				return this.GetOperandAsVariable(0);
			}
		}

		public MediumLevelILVariable Source
		{
			get
			{
				return this.Variable;
			}
		}
	}
}
