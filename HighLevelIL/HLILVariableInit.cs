using System.Collections.Generic;

namespace BinaryNinja
{
	public sealed class HLILVariableInit : HighLevelILInstruction
	{
		internal HLILVariableInit(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
		public HighLevelILVariable Destination
		{
			get
			{
				return this.GetOperandAsVariable((OperandIndex)0);
			}
		}
		
		public HighLevelILInstruction Source
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)1);
			}
		}

		/// <summary>
		/// The destination variable, mirroring Python HighLevelILVarInit.vars_written
		/// (highlevelil.py:1376).
		/// </summary>
		public override IList<IHighLevelILVariable> VarsWritten
		{
			get
			{
				List<IHighLevelILVariable> result = new List<IHighLevelILVariable>();
				result.Add(this.Destination);
				return result;
			}
		}
	}
}
