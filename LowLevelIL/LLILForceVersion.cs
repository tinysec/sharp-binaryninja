namespace BinaryNinja
{
	public sealed class LLILForceVersion : LowLevelILInstruction
	{
		internal LLILForceVersion(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native
		) : base(function , expressionIndex , native)
		{
			
		}
		
		public ILRegister Destination
		{
			get
			{
				return this.GetOperandAsRegister((OperandIndex)0 );
			}
		}
	}
}
