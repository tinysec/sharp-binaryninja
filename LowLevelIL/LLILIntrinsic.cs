namespace BinaryNinja
{
	public sealed class LLILIntrinsic : LowLevelILInstruction
	{
		internal LLILIntrinsic(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native
		) : base(function , expressionIndex , native)
		{
			
		}
		
		public FlagOrRegister[] Output
		{
			get
			{
				
				return this.GetOperandAsFlagOrRegisterList(0);
			}
		}

		[System.Obsolete("Use Output instead.")]
		public FlagOrRegister[] output
		{
			get
			{
				return this.Output;
			}
		}
		
		public Intrinsic Intrinsic
		{
			get
			{
				return this.GetOperandAsIntrinsic((OperandIndex)2);
			}
		}
		
		public LLILCallParameter Parameter
		{
			get
			{
				LLILCallParameter? instruction = this.GetOperandAsExpression((OperandIndex)3) as LLILCallParameter;

				return instruction!;
			}
		}
		
		public LowLevelILInstruction[] Parameters
		{
			get
			{
				LLILCallParameter? instruction = this.GetOperandAsExpression((OperandIndex)3) as LLILCallParameter;

				return instruction!.Source;
			}
		}
	}
}
