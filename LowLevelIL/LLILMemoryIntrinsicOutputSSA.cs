namespace BinaryNinja
{
	public sealed class LLILMemoryIntrinsicOutputSSA : LowLevelILInstruction
	{
		internal LLILMemoryIntrinsicOutputSSA(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native
		) : base(function , expressionIndex , native)
		{
			
		}
		
		public ulong DestinationMemory
		{
			get
			{

				return this.RawOperands[0];
			}
		}
		
		public SSAFlagOrRegister[] Output
		{
			get
			{
				return this.GetOperandAsSSAFlagOrRegisterList((OperandIndex)1);
			}
		}
	}
}
