using System;

namespace BinaryNinja
{
	public sealed partial class MediumLevelILFunction
	{
		public MediumLevelILInstruction? GetSSAVariableDefinition(
			MediumLevelILSSAVariable variable)
		{
			return this.GetSSAVariableDefinition(this.ToVariable(variable), variable.Version);
		}

		public MediumLevelILInstruction[] GetSSAVariableUses(
			MediumLevelILSSAVariable variable)
		{
			return this.GetSSAVariableUses(this.ToVariable(variable), variable.Version);
		}

		public bool IsSSAVariableLive(MediumLevelILSSAVariable variable)
		{
			return this.IsSSAVariableLive(this.ToVariable(variable), variable.Version);
		}

		public bool IsSSAVariableLiveAt(
			MediumLevelILSSAVariable variable,
			MediumLevelILInstructionIndex instruction)
		{
			return this.IsSSAVariableLiveAt(
				this.ToVariable(variable),
				variable.Version,
				instruction);
		}

		public RegisterValue GetSSAVariableValue(MediumLevelILSSAVariable variable)
		{
			return this.GetSSAVariableValue(this.ToVariable(variable), variable.Version);
		}

		public PossibleValueSet GetSSAVariablePossibleValues(
			MediumLevelILSSAVariable variable,
			MediumLevelILInstructionIndex instruction,
			DataFlowQueryOption[] options)
		{
			return this.GetSSAVariablePossibleValues(
				this.ToVariable(variable),
				variable.Version,
				instruction,
				options);
		}

		public ulong[] GetVariableSSAVersions(Variable variable)
		{
			IntPtr arrayPointer = NativeMethods.BNGetMediumLevelILVariableSSAVersions(
				this.DangerousGetHandle(),
				variable.ToNative(),
				out ulong arrayLength);

			return UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer,
				arrayLength,
				NativeMethods.BNFreeILInstructionList);
		}

		private Variable ToVariable(MediumLevelILSSAVariable variable)
		{
			return new Variable(this.OwnerFunction, variable.Variable.ToNative());
		}
	}
}
