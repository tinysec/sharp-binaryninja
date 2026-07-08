namespace BinaryNinja
{
	public sealed class HLILStructField : HighLevelILInstruction
	{
		internal HLILStructField(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
		public HighLevelILInstruction Source
		{
			get
			{
				return this.GetOperandAsExpression(0);
			}
		}
		
		public long Offset
		{
			get
			{
				return (long)this.RawOperands[1];
			}
		}
		
		/// <summary>
		/// The named-member index, or null when there is no named member. Mirrors Python
		/// <c>_get_member_index</c>: the sentinel (1&lt;&lt;63) means no named member.
		/// </summary>
		public long? MemberIndex
		{
			get
			{
				return HighLevelILInstruction.MemberIndexFromRawOperand(this.RawOperands[2]);
			}
		}
	}
}
