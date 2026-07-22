using System;

namespace BinaryNinja
{
	public sealed partial class LowLevelILFunction
	{
		public LowLevelILInstruction? GetSSARegisterDefinition(
			LowLevelILSSARegister register)
		{
			return this.GetSSARegisterDefinition(register.Register.Index, register.Version);
		}

		public LowLevelILInstruction[] GetSSARegisterUses(
			LowLevelILSSARegister register)
		{
			return this.GetSSARegisterUses(register.Register.Index, register.Version);
		}

		public RegisterValue GetSSARegisterValue(LowLevelILSSARegister register)
		{
			return this.GetSSARegisterValue(register.Register.Index, register.Version);
		}

		public LowLevelILInstruction? GetSSAFlagDefinition(LowLevelILSSAFlag flag)
		{
			return this.GetSSAFlagDefinition(flag.Flag.Index, flag.Version);
		}

		public LowLevelILInstruction[] GetSSAFlagUses(LowLevelILSSAFlag flag)
		{
			return this.GetSSAFlagUses(flag.Flag.Index, flag.Version);
		}

		public RegisterValue GetSSAFlagValue(LowLevelILSSAFlag flag)
		{
			return this.GetSSAFlagValue(flag.Flag.Index, flag.Version);
		}

		public RegisterValue GetRegisterValueAfterInstruction(
			RegisterIndex register,
			LowLevelILInstructionIndex instruction)
		{
			return RegisterValue.FromNative(
				NativeMethods.BNGetLowLevelILRegisterValueAfterInstruction(
					this.DangerousGetHandle(),
					(uint)register,
					instruction));
		}

		public PossibleValueSet GetPossibleRegisterValuesAtInstruction(
			RegisterIndex register,
			LowLevelILInstructionIndex instruction,
			DataFlowQueryOption[]? options = null)
		{
			DataFlowQueryOption[] actualOptions = NormalizeOptions(options);

			return PossibleValueSet.TakeNative(
				NativeMethods.BNGetLowLevelILPossibleRegisterValuesAtInstruction(
					this.DangerousGetHandle(),
					register,
					instruction,
					actualOptions,
					(ulong)actualOptions.Length));
		}

		public PossibleValueSet GetPossibleRegisterValuesAfterInstruction(
			RegisterIndex register,
			LowLevelILInstructionIndex instruction,
			DataFlowQueryOption[]? options = null)
		{
			DataFlowQueryOption[] actualOptions = NormalizeOptions(options);

			return PossibleValueSet.TakeNative(
				NativeMethods.BNGetLowLevelILPossibleRegisterValuesAfterInstruction(
					this.DangerousGetHandle(),
					register,
					instruction,
					actualOptions,
					(ulong)actualOptions.Length));
		}

		public RegisterValue GetFlagValueAtInstruction(
			FlagIndex flag,
			LowLevelILInstructionIndex instruction)
		{
			return RegisterValue.FromNative(
				NativeMethods.BNGetLowLevelILFlagValueAtInstruction(
					this.DangerousGetHandle(),
					flag,
					instruction));
		}

		public RegisterValue GetFlagValueAfterInstruction(
			FlagIndex flag,
			LowLevelILInstructionIndex instruction)
		{
			return RegisterValue.FromNative(
				NativeMethods.BNGetLowLevelILFlagValueAfterInstruction(
					this.DangerousGetHandle(),
					flag,
					instruction));
		}

		public PossibleValueSet GetPossibleFlagValuesAtInstruction(
			FlagIndex flag,
			LowLevelILInstructionIndex instruction,
			DataFlowQueryOption[]? options = null)
		{
			DataFlowQueryOption[] actualOptions = NormalizeOptions(options);

			return PossibleValueSet.TakeNative(
				NativeMethods.BNGetLowLevelILPossibleFlagValuesAtInstruction(
					this.DangerousGetHandle(),
					flag,
					instruction,
					actualOptions,
					(ulong)actualOptions.Length));
		}

		public PossibleValueSet GetPossibleFlagValuesAfterInstruction(
			FlagIndex flag,
			LowLevelILInstructionIndex instruction,
			DataFlowQueryOption[]? options = null)
		{
			DataFlowQueryOption[] actualOptions = NormalizeOptions(options);

			return PossibleValueSet.TakeNative(
				NativeMethods.BNGetLowLevelILPossibleFlagValuesAfterInstruction(
					this.DangerousGetHandle(),
					flag,
					instruction,
					actualOptions,
					(ulong)actualOptions.Length));
		}

		public RegisterValue GetStackContentsAtInstruction(
			long offset,
			ulong length,
			LowLevelILInstructionIndex instruction)
		{
			return RegisterValue.FromNative(
				NativeMethods.BNGetLowLevelILStackContentsAtInstruction(
					this.DangerousGetHandle(),
					offset,
					length,
					instruction));
		}

		public RegisterValue GetStackContentsAfterInstruction(
			long offset,
			ulong length,
			LowLevelILInstructionIndex instruction)
		{
			return RegisterValue.FromNative(
				NativeMethods.BNGetLowLevelILStackContentsAfterInstruction(
					this.DangerousGetHandle(),
					offset,
					length,
					instruction));
		}

		public PossibleValueSet GetPossibleStackContentsAtInstruction(
			long offset,
			ulong length,
			LowLevelILInstructionIndex instruction,
			DataFlowQueryOption[]? options = null)
		{
			DataFlowQueryOption[] actualOptions = NormalizeOptions(options);

			return PossibleValueSet.TakeNative(
				NativeMethods.BNGetLowLevelILPossibleStackContentsAtInstruction(
					this.DangerousGetHandle(),
					offset,
					length,
					instruction,
					actualOptions,
					(ulong)actualOptions.Length));
		}

		public PossibleValueSet GetPossibleStackContentsAfterInstruction(
			long offset,
			ulong length,
			LowLevelILInstructionIndex instruction,
			DataFlowQueryOption[]? options = null)
		{
			DataFlowQueryOption[] actualOptions = NormalizeOptions(options);

			return PossibleValueSet.TakeNative(
				NativeMethods.BNGetLowLevelILPossibleStackContentsAfterInstruction(
					this.DangerousGetHandle(),
					offset,
					length,
					instruction,
					actualOptions,
					(ulong)actualOptions.Length));
		}

		private static DataFlowQueryOption[] NormalizeOptions(DataFlowQueryOption[]? options)
		{
			if (null == options)
			{
				return Array.Empty<DataFlowQueryOption>();
			}

			return options;
		}
	}
}
