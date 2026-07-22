namespace BinaryNinja
{
	public sealed class LLILIntrinsicSSA : LowLevelILInstruction
	{
		internal LLILIntrinsicSSA(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native
		) : base(function , expressionIndex , native)
		{
			
		}
		
		public SSAFlagOrRegister[] Output
		{
			get
			{
				return this.GetOperandAsSSAFlagOrRegisterList(0);
			}
		}

		[System.Obsolete("Use Output instead.")]
		public SSAFlagOrRegister[] output
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
