namespace BinaryNinja
{
	public sealed class HLILVariableDeclare : HighLevelILInstruction
	{
		internal HLILVariableDeclare(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
		/// <summary>The declared variable (Python HighLevelILVarDeclare.var, highlevelil.py).</summary>
		public HighLevelILVariable Variable
		{
			get
			{
				return this.GetOperandAsVariable((OperandIndex)0);
			}
		}
	}
}
