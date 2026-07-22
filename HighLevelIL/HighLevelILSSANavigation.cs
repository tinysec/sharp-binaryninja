using System;

namespace BinaryNinja
{
	public sealed partial class HighLevelILFunction
	{
		public HighLevelILInstruction? GetSSAVariableDefinition(
			HighLevelILSSAVariable variable)
		{
			return this.GetSSAVariableDefinition(this.ToVariable(variable), variable.Version);
		}

		public HighLevelILInstruction[] GetSSAVariableUses(
			HighLevelILSSAVariable variable)
		{
			return this.GetSSAVariableUses(this.ToVariable(variable), variable.Version);
		}

		public bool IsSSAVariableLive(HighLevelILSSAVariable variable)
		{
			return this.IsSSAVariableLive(this.ToVariable(variable), variable.Version);
		}

		public bool IsSSAVariableLiveAt(
			HighLevelILSSAVariable variable,
			HighLevelILInstructionIndex instruction)
		{
			return this.IsSSAVariableLiveAt(
				this.ToVariable(variable),
				variable.Version,
				instruction);
		}

		public ulong[] GetVariableSSAVersions(Variable variable)
		{
			IntPtr arrayPointer = NativeMethods.BNGetHighLevelILVariableSSAVersions(
				this.DangerousGetHandle(),
				variable.ToNative(),
				out ulong arrayLength);

			return UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer,
				arrayLength,
				NativeMethods.BNFreeILInstructionList);
		}

		private Variable ToVariable(HighLevelILSSAVariable variable)
		{
			return new Variable(this.OwnerFunction, variable.Variable.ToNative());
		}
	}
}
