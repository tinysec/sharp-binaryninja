using System.Collections.Generic;

namespace BinaryNinja
{
	public sealed class HLILAddressOf : HighLevelILInstruction
	{
		internal HLILAddressOf(
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

		/// <summary>
		/// Variables whose address is taken by this instruction, mirroring Python
		/// HighLevelILAddressOf.vars_address_taken (highlevelil.py:1738). A direct
		/// <see cref="HLILVariable"/>/<see cref="HLILVariableSSA"/> source reports its variable; a
		/// <see cref="HLILStructField"/> whose source is a variable reports that variable; otherwise
		/// the source's <see cref="VarsAddressTaken"/> is returned.
		/// </summary>
		public override IList<IHighLevelILVariable> VarsAddressTaken
		{
			get
			{
				HighLevelILInstruction src = this.Source;

				if (src is HLILVariable varInstr)
				{
					List<IHighLevelILVariable> direct = new List<IHighLevelILVariable>();
					direct.Add(varInstr.Variable);
					return direct;
				}

				if (src is HLILVariableSSA varSsaInstr)
				{
					List<IHighLevelILVariable> direct = new List<IHighLevelILVariable>();
					direct.Add(varSsaInstr.Variable);
					return direct;
				}

				if (src is HLILStructField structField)
				{
					HighLevelILInstruction fieldSrc = structField.Source;

					if (fieldSrc is HLILVariable fieldVarInstr)
					{
						List<IHighLevelILVariable> fieldVar = new List<IHighLevelILVariable>();
						fieldVar.Add(fieldVarInstr.Variable);
						return fieldVar;
					}

					if (fieldSrc is HLILVariableSSA fieldVarSsaInstr)
					{
						List<IHighLevelILVariable> fieldVar = new List<IHighLevelILVariable>();
						fieldVar.Add(fieldVarSsaInstr.Variable);
						return fieldVar;
					}
				}

				List<IHighLevelILVariable> result = new List<IHighLevelILVariable>();
				result.AddRange(src.VarsAddressTaken);
				return result;
			}
		}
	}
}
