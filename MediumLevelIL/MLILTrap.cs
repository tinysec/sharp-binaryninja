namespace BinaryNinja
{
	public sealed class MLILTrap : MediumLevelILInstruction
	{
		internal MLILTrap(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		public ulong Number
		{
			get
			{
				return this.RawOperands[0];
			}
		}

		public long Vector
		{
			get
			{
				return unchecked((long)this.RawOperands[0]);
			}
		}

		public ulong RawVector
		{
			get
			{
				return this.RawOperands[0];
			}
		}
	}
}
