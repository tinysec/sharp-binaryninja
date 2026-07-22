namespace BinaryNinja
{
	public sealed class MLILExternPointer : AbstractMediumLevelILConstInstruction
	{
		internal MLILExternPointer(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		
		public ulong Offset
		{
			get
			{
				return this.RawOperands[1];
			}
		}
	}
}
