using System.Collections.Generic;

namespace BinaryNinja
{
	public sealed class HLILAssignUnpack : HighLevelILInstruction
	{
		internal HLILAssignUnpack(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
		public HighLevelILInstruction[] Destination
		{
			get
			{
				return this.GetOperandAsExpressionList((OperandIndex)0);
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
		/// Variables written by this unpack assignment, mirroring Python
		/// HighLevelILAssignUnpack.vars_written (highlevelil.py:1447). For each destination
		/// expression, a variable operand contributes its <see cref="HLILVariable.Variable"/> (or
		/// <see cref="HLILVariableSSA.Variable"/>); any other expression contributes its
		/// <see cref="VarsWritten"/>.
		/// </summary>
		public override IList<IHighLevelILVariable> VarsWritten
		{
			get
			{
				List<IHighLevelILVariable> result = new List<IHighLevelILVariable>();

				foreach (HighLevelILInstruction destExpr in this.Destination)
				{
					if (destExpr is HLILVariable varInstr)
					{
						result.Add(varInstr.Variable);
					}
					else if (destExpr is HLILVariableSSA varSsaInstr)
					{
						result.Add(varSsaInstr.Variable);
					}
					else
					{
						result.AddRange(destExpr.VarsWritten);
					}
				}

				return result;
			}
		}
	}
}
