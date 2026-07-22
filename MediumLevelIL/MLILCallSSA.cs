using System;

namespace BinaryNinja
{
	public sealed class MLILCallSSA : AbstractMediumLevelILSSACallInstruction
	{
		internal MLILCallSSA(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		public override MediumLevelILSSAVariable[] Output
		{
			get
			{
				MLILCallOutputSSA? instruction = this.GetOperandAsExpression(0) as  MLILCallOutputSSA;

				if (null == instruction)
				{
					throw new Exception("must be MediumLevelILCallOutputSSA");
				}

				return instruction.Destination;
			}
		}
		
		public ulong OutputDestMemory
		{
			get
			{
				MLILCallOutputSSA? instruction = this.GetOperandAsExpression(0) as  MLILCallOutputSSA;

				if (null == instruction)
				{
					throw new Exception("must be MediumLevelILCallOutputSSA");
				}

				return instruction.DestinationMemory;
			}
		}
		
		public MediumLevelILInstruction Destination
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)1);
			}
		}
		
		public override MediumLevelILInstruction[] Parameters
		{
			get
			{
				return this.GetOperandAsExpressionList((OperandIndex)2);
			}
		}
		
		public ulong SourceMemory
		{
			get
			{
				return this.RawOperands[4];
			}
		}
	}
}
