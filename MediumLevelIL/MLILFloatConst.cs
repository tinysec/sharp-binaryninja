using System;

namespace BinaryNinja
{
	public sealed class MLILFloatConst : MediumLevelILInstruction
	{
		internal MLILFloatConst(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		public double Constant
		{
			get
			{
				if (4 == this.Size)
				{
					return this.GetOperandAsFloat(0);
				}

				if (8 == this.Size)
				{
					return this.GetOperandAsDouble(0);
				}

				return this.RawOperands[0];
			}
		}
	}
}
