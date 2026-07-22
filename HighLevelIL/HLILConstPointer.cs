namespace BinaryNinja
{
	public sealed class HLILConstPointer : AbstractHighLevelILConstInstruction
	{
		internal HLILConstPointer(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}

		public StringAnnotation? String
		{
			get
			{
				return this.Function.View.CheckForStringAnnotationType(
					this.Constant,
					true,
					true,
					0);
			}
		}
	}
}
