using System;

namespace BinaryNinja
{
	public sealed class MLILCallUntypedSSA : AbstractMediumLevelILSSACallInstruction
	{
		internal MLILCallUntypedSSA(
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
				MLILCallOutputSSA? instruction = this.GetOperandAsExpression(0) as MLILCallOutputSSA;
		
				if (null == instruction)
				{
					throw new Exception("must be MediumLevelILCallOutput");
				}

				return instruction.Destination;
			}
		}
		
		public ulong OutputDestinationMemory
		{
			get
			{
				MLILCallOutputSSA? instruction = this.GetOperandAsExpression(0) as MLILCallOutputSSA;
				
				if (null == instruction)
				{
					throw new Exception("must be MediumLevelILCallOutput");
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
				MLILCallParamSSA? instruction = this.GetOperandAsExpression((OperandIndex)2) as MLILCallParamSSA;
				
				if (null == instruction)
				{
					throw new Exception("must be MediumLevelILCallParam");
				}

				return instruction.Source;
			}
		}
		
		public ulong ParameterSourceMemory
		{
			get
			{
				MLILCallParamSSA? instruction = this.GetOperandAsExpression((OperandIndex)2) as MLILCallParamSSA;
				
				if (null == instruction)
				{
					throw new Exception("must be MediumLevelILCallParam");
				}

				return instruction.SourceMemory;
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
