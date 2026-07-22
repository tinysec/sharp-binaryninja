using System.Collections.Generic;

namespace BinaryNinja
{
	public sealed class HLILAssign : HighLevelILInstruction
	{
		internal HLILAssign(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
		public HighLevelILInstruction Destination
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)0);
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
		/// Variables written by this assignment, mirroring Python HighLevelILAssign.vars_written
		/// (highlevelil.py:1420). When the destination is a leaf-like expression (split, variable,
		/// SSA variable, or struct field) the destination's <see cref="Vars"/> are written; otherwise
		/// the destination's <see cref="VarsWritten"/> are written. The source's
		/// <see cref="VarsWritten"/> are appended in both cases.
		/// </summary>
		public override IList<IHighLevelILVariable> VarsWritten
		{
			get
			{
				HighLevelILInstruction dest = this.Destination;
				HighLevelILInstruction src = this.Source;
				List<IHighLevelILVariable> result = new List<IHighLevelILVariable>();

				bool destIsLeafLike = dest is HLILSplit
					|| dest is HLILVariable
					|| dest is HLILVariableSSA
					|| dest is HLILStructField;

				if (destIsLeafLike)
				{
					result.AddRange(dest.Vars);
				}
				else
				{
					result.AddRange(dest.VarsWritten);
				}

				result.AddRange(src.VarsWritten);

				return result;
			}
		}
	}
}
