namespace BinaryNinja
{
	public sealed class HLILFloatConst : HighLevelILInstruction
	{
		internal HLILFloatConst(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
		public double Constant
		{
			get
			{
				if (4 == this.Size)
				{
					return this.GetOperandAsFloat(0);
				}

				if (8 == this.Size)
				{
					return this.GetOperandAsDouble(0);
				}

				return this.RawOperands[0];
			}
		}
	}
}
