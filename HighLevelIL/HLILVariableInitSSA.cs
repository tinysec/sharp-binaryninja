using System.Collections.Generic;

namespace BinaryNinja
{
	public sealed class HLILVariableInitSSA : HighLevelILInstruction
	{
		internal HLILVariableInitSSA(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
		public HighLevelILSSAVariable Destination
		{
			get
			{
				return this.GetOperandAsSSAVariable((OperandIndex)0 , (OperandIndex)1);
			}
		}
		
		public HighLevelILInstruction Source
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)2);
			}
		}

		/// <summary>
		/// The destination SSA variable, mirroring Python HighLevelILVarInitSsa.vars_written
		/// (highlevelil.py:1398).
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
