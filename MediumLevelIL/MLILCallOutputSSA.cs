namespace BinaryNinja
{
	public sealed class MLILCallOutputSSA : MediumLevelILInstruction
	{
		internal MLILCallOutputSSA(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		public ulong DestinationMemory
		{
			get
			{
				return this.RawOperands[0];
			}
		}
		
		public MediumLevelILSSAVariable[] Destination
		{
			get
			{
				return this.GetOperandAsSSAVariableList((OperandIndex)1);
			}
		}

		public override MediumLevelILSSAVariable[] SSAVariablesWrite
		{
			get
			{
				return this.Destination;
			}
		}
	}
}
