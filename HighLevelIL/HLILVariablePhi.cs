using System.Collections.Generic;

namespace BinaryNinja
{
	public sealed class HLILVariablePhi : HighLevelILInstruction
	{
		internal HLILVariablePhi(
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
		
		public HighLevelILSSAVariable[] Source
		{
			get
			{
				return this.GetOperandAsSSAVariableList((OperandIndex)2);
			}
		}

		/// <summary>
		/// The destination SSA variable, mirroring Python HighLevelILVarPhi.vars_written
		/// (highlevelil.py:1550).
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
