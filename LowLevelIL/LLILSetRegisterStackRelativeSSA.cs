namespace BinaryNinja
{
	public sealed class LLILSetRegisterStackRelativeSSA : LowLevelILInstruction
	{
		internal LLILSetRegisterStackRelativeSSA(
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
					this.GetOperandAsExpression(0) as LLILRegisterStackDestinationSSA;

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
		
		public LowLevelILInstruction Destination
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)1);
			}
		}
		
		public LLILRegisterSSA Top
		{
			get
			{
				LLILRegisterSSA? instruction =
					this.GetOperandAsExpression((OperandIndex)2) as LLILRegisterSSA;

				return instruction!;
			}
		}

		public LowLevelILSSARegister TopRegister
		{
			get
			{
				return this.Top.Source;
			}
		}
		
		public LowLevelILInstruction Source
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)3);
			}
		}
	}
}
