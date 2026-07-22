using System.Collections.Generic;

namespace BinaryNinja
{
	public sealed class LLILCallStackAdjust : LowLevelILInstruction
	{
		internal LLILCallStackAdjust(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native
		) : base(function , expressionIndex , native)
		{
			
		}

		public LowLevelILInstruction Dest
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)0);
			}
		}

		public LowLevelILInstruction Destination
		{
			get
			{
				return this.Dest;
			}
		}
		
		public long StackAdjustment
		{
			get
			{
				return unchecked((long)this.RawOperands[1]);
			}
		}

		[System.Obsolete("Use StackAdjustment instead.")]
		public ulong StackRdjustment
		{
			get
			{
				return unchecked((ulong)this.StackAdjustment);
			}
		}
		
		public IDictionary<RegisterStackIndex,long> RegisterStackAdjustments
		{
			get
			{
				return this.GetOperandAsRegisterStackDict((OperandIndex)2);
			}
		}
	}
}
