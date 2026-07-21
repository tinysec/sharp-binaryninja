using System;

namespace BinaryNinja
{
	public sealed class MLILTailCallUntyped : MediumLevelILInstruction
	{
		internal MLILTailCallUntyped(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		// output and params are DERIVED: slots 0 and 2 hold MLILCallOutput / MLILCallParam
		// sub-instruction expressions whose own dest / src hold the variable / expression lists
		// (mirrors MediumLevelILTailcallUntyped in mediumlevelil.py and the sibling MLILCallUntyped
		// / MLILSysCallUntyped wrappers). The earlier flat readers misread those slots as lists.
		public MediumLevelILVariable[] Output
		{
			get
			{
				MLILCallOutput? instruction = this.GetOperandAsExpression(0) as MLILCallOutput;

				if (null == instruction)
				{
					throw new Exception("must be MediumLevelILCallOutput");
				}

				return instruction.Destination;
			}
		}

		public MediumLevelILInstruction Dest
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)1);
			}
		}

		public MediumLevelILInstruction[] Parameters
		{
			get
			{
				MLILCallParam? instruction = this.GetOperandAsExpression((OperandIndex)2) as MLILCallParam;

				if (null == instruction)
				{
					throw new Exception("must be MediumLevelILCallParam");
				}

				return instruction.Source;
			}
		}
		
		public MediumLevelILInstruction Stack
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)3);
			}
		}
	}
}
