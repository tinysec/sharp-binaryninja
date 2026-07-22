namespace BinaryNinja
{
	public sealed class LLILSysCallSSA : LowLevelILInstruction
	{
		internal LLILSysCallSSA(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native
		) : base(function , expressionIndex , native)
		{
			
		}

		public LowLevelILSSARegister[] Output
		{
			get
			{
				LLILCallOutputSSA? instruction = this.GetOperandAsExpression(0) as LLILCallOutputSSA;

				return instruction!.Destination;
			}
		}

		public ulong DestinationMemory
		{
			get
			{
				LLILCallOutputSSA? instruction = this.GetOperandAsExpression(0) as LLILCallOutputSSA;

				return instruction!.DestinationMemory;
			}
		}
		
		public LLILCallStackSSA Stack
		{
			get
			{
				LLILCallStackSSA? instruction = this.GetOperandAsExpression((OperandIndex)1) as LLILCallStackSSA;

				return instruction!;
			}
		}

		public LowLevelILSSARegister StackRegister
		{
			get
			{
				return this.Stack.Source;
			}
		}
		
		public ulong StackMemory
		{
			get
			{
				return this.Stack.SourceMemory;
			}
		}

		public ulong SourceMemory
		{
			get
			{
				return this.StackMemory;
			}
		}
		
		public LLILCallParameter Parameter
		{
			get
			{
				LLILCallParameter? instruction = this.GetOperandAsExpression((OperandIndex)2) as LLILCallParameter;

				return instruction!;
			}
		}
		
		public LowLevelILInstruction[] Parameters
		{
			get
			{
				LLILCallParameter? instruction = this.GetOperandAsExpression((OperandIndex)2) as LLILCallParameter;

				return instruction!.Source;
			}
		}
	}
}
