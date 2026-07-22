namespace BinaryNinja
{
	public sealed class MLILSetVariableSplitSSA : MediumLevelILInstruction
	{
		internal MLILSetVariableSplitSSA(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		public MediumLevelILSSAVariable High
		{
			get
			{
				return this.GetOperandAsSSAVariable((OperandIndex)0,(OperandIndex)1);
			}
		}
		
		public MediumLevelILSSAVariable Low
		{
			get
			{
				return this.GetOperandAsSSAVariable((OperandIndex)2,(OperandIndex)3);
			}
		}
	
		public MediumLevelILInstruction Source
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)4);
			}
		}

		public override MediumLevelILSSAVariable[] SSAVariablesRead
		{
			get
			{
				return this.Source.SSAVariablesRead;
			}
		}

		public override MediumLevelILVariable[] VariablesRead
		{
			get
			{
				return this.Source.VariablesRead;
			}
		}
		
		public override MediumLevelILSSAVariable[] SSAVariablesWrite
		{
			get
			{
				return new MediumLevelILSSAVariable[]
				{
					this.High , 
					this.Low
				};
			}
		}
	}
}
