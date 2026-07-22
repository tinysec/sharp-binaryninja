using System;

namespace BinaryNinja
{
	public sealed class MLILTailCallUntypedSSA : MediumLevelILInstruction
	{
		internal MLILTailCallUntypedSSA(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		public MediumLevelILSSAVariable[] Output
		{
			get
			{
				MLILCallOutputSSA? instruction = this.GetOperandAsExpression(0) as MLILCallOutputSSA;
			
				if (null == instruction)
				{
					throw new Exception("must be MediumLevelILCallOutputSsa");
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
					throw new Exception("must be MediumLevelILCallOutputSsa");
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
		
		public MediumLevelILInstruction[] Parameters
		{
			get
			{
				MLILCallParamSSA? instruction = this.GetOperandAsExpression((OperandIndex)2) as MLILCallParamSSA;
				
				if (null == instruction)
				{
					throw new Exception("must be MediumLevelILCallOutputSsa");
				}

				return instruction.Source;
			}
		}

		public ulong ParametersSourceMemory
		{
			get
			{
				MLILCallParamSSA? instruction = this.GetOperandAsExpression((OperandIndex)2) as MLILCallParamSSA;

				if (null == instruction)
				{
					throw new Exception("must be MediumLevelILCallParamSSA");
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
