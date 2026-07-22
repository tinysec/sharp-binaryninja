namespace BinaryNinja
{
	public sealed class MLILStoreStructSSA : MediumLevelILInstruction
	{
		internal MLILStoreStructSSA(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		public MediumLevelILInstruction Destination
		{
			get
			{
				return this.GetOperandAsExpression(0);
			}
		}
		
		public ulong Offset
		{
			get
			{
				return this.RawOperands[1];
			}
		}
		
		public ulong DestinationMemory
		{
			get
			{
				return this.RawOperands[2];
			}
		}
		
		public ulong SourceMemory
		{
			get
			{
				return this.RawOperands[3];
			}
		}
		
		public MediumLevelILInstruction Source
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)4);
			}
		}
	}
}
