using System;

namespace BinaryNinja
{
	public sealed class MLILSysCallUntyped : AbstractMediumLevelILCallInstruction
	{
		internal MLILSysCallUntyped(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		public override MediumLevelILVariable[] Output
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
		
		public override MediumLevelILInstruction[] Parameters
		{
			get
			{
				MLILCallParam? instruction = this.GetOperandAsExpression((OperandIndex)1) as MLILCallParam;

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
				return this.GetOperandAsExpression((OperandIndex)2);
			}
		}
	}
}
