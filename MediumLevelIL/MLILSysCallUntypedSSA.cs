using System;

namespace BinaryNinja
{
	public sealed class MLILSysCallUntypedSSA : AbstractMediumLevelILSSACallInstruction
	{
		internal MLILSysCallUntypedSSA(
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
					throw new Exception("must MediumLevelILCallOutputSSA");
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
					throw new Exception("must MediumLevelILCallOutputSSA");
				}

				return instruction.DestinationMemory;
			}
		}
		
		public override MediumLevelILInstruction[] Parameters
		{
			get
			{
				MLILCallParamSSA? instruction = this.GetOperandAsExpression((OperandIndex)1) as MLILCallParamSSA;

				if (null == instruction)
				{
					throw new Exception("must MediumLevelILCallParamSSA");
				}

				return instruction.Source;
			}
		}
		
		public ulong ParametersSourceMemory
		{
			get
			{
				MLILCallParamSSA? instruction = this.GetOperandAsExpression((OperandIndex)1) as MLILCallParamSSA;

				if (null == instruction)
				{
					throw new Exception("must MediumLevelILCallParamSSA");
				}

				return instruction.SourceMemory;
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
