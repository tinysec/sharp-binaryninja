namespace BinaryNinja
{
	public sealed class LLILRegisterStackFreeAbsSSA : LowLevelILInstruction
	{
		internal LLILRegisterStackFreeAbsSSA(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native
		) : base(function , expressionIndex , native)
		{
			
		}
		
		public LLILRegisterStackDestinationSSA Stack
		{
			get
			{
				LLILRegisterStackDestinationSSA? instruction =
					this.GetOperandAsExpression((OperandIndex)0) as LLILRegisterStackDestinationSSA;

				return instruction!;
			}
		}

		public SSARegisterStack DestinationStack
		{
			get
			{
				return this.Stack.Destination;
			}
		}

		public SSARegisterStack SourceStack
		{
			get
			{
				return this.Stack.Source;
			}
		}
		
		public ILRegister Destination
		{
			get
			{
				return this.GetOperandAsRegister((OperandIndex)1);
			}
		}
	}
}
