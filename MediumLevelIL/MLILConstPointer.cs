namespace BinaryNinja
{
	public sealed class MLILConstPointer : MediumLevelILInstruction
	{
		internal MLILConstPointer(
			MediumLevelILFunction ilFunction, 
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) :base(ilFunction, expressionIndex , native)
		{
			
		}
		
		public ulong Constant
		{
			get
			{
				return this.RawOperands[0];
			}
		}

		public StringAnnotation? String
		{
			get
			{
				BinaryView? view = this.Function.SourceView;

				if (null == view)
				{
					return null;
				}

				return view.CheckForStringAnnotationType(
					this.Constant,
					true,
					true,
					0);
			}
		}
	}
}
