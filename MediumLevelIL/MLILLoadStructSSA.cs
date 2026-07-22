namespace BinaryNinja
{
	public sealed class MLILLoadStructSSA : MediumLevelILInstruction
	{
		internal MLILLoadStructSSA(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		public MediumLevelILInstruction Source
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)0);
			}
		}
		
		public ulong Offset
		{
			get
			{
				return this.RawOperands[1];
			}
		}
		
		public ulong SourceMemory
		{
			get
			{
				return this.RawOperands[2];
			}
		}
	}
}
