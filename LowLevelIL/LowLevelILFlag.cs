namespace BinaryNinja
{
	public sealed class LowLevelILFlag : ILFlag
	{
		public LowLevelILFunction ILFunction { get; }
		
		public LowLevelILFlag(LowLevelILFunction function , ILFlag flag)
			: base(flag)
		{
			this.ILFunction = function;
		}
		
		public LowLevelILFlag( LowLevelILFlag other)
			: base(other)
		{
			this.ILFunction = other.ILFunction;
		}

		public ulong[] Versions
		{
			get
			{
				return this.ILFunction.SSAForm.GetFlagSSAVersions(this.Index);
			}
		}
	}
}
