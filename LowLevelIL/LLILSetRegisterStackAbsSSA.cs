namespace BinaryNinja
{
	public sealed class LLILSetRegisterStackAbsSSA : LowLevelILInstruction
	{
		internal LLILSetRegisterStackAbsSSA(
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
		
		public LowLevelILInstruction Source
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)2);
			}
		}
	}
}
