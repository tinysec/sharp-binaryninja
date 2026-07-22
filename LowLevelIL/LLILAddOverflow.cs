namespace BinaryNinja
{
	public sealed class LLILAddOverflow : LowLevelILInstruction
	{
		internal LLILAddOverflow(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native
		) : base(function , expressionIndex , native)
		{
			
		}
		
		public LowLevelILInstruction Left
		{
			get
			{
				return LowLevelILInstruction.FromExpressionIndex(
					this.ILFunction,
					(LowLevelILExpressionIndex)this.Flags);
			}
		}
		
		public LowLevelILInstruction Right
		{
			get
			{
				return this.GetOperandAsExpression(0);
			}
		}
	}
}
