using System;

namespace BinaryNinja
{
	public sealed class MediumLevelILSSAVariable : AbstractSSAVariable<MediumLevelILVariable>
	{
		public MediumLevelILSSAVariable(MediumLevelILVariable variable , ulong version) 
			:base(variable, version)
		{
			
		}

		public MediumLevelILInstruction? Definition
		{
			get
			{
				return this.Variable.ILFunction.GetInstruction(
					NativeMethods.BNGetMediumLevelILSSAVarDefinition(
						this.Variable.ILFunction.DangerousGetHandle() ,
						this.Variable.ToNative() ,
						this.Version
					)
				);
			}
		}
		
		public MediumLevelILInstruction[] Uses
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetMediumLevelILSSAVarUses(
					this.Variable.ILFunction.DangerousGetHandle() ,
					this.Variable.ToNative() ,
					this.Version ,
					out ulong arrayLength
				);

				MediumLevelILInstructionIndex[] indexes =
					UnsafeUtils.TakeNumberArray<MediumLevelILInstructionIndex>(
					arrayPointer ,
					arrayLength ,
					NativeMethods.BNFreeILInstructionList
				);

				return this.Variable.ILFunction.MustGetInstructions(indexes);
			}
		}

		public RegisterValue Value
		{
			get
			{
				return RegisterValue.FromNative(
					NativeMethods.BNGetMediumLevelILSSAVarValue(
						this.Variable.ILFunction.DangerousGetHandle() ,
						this.Variable.ToNative() ,
						this.Version
					)
				);
			}
		}
		
		public PossibleValueSet GetPossibleValues(
			MediumLevelILInstructionIndex instruction ,
			DataFlowQueryOption[] options
		)
		{
			return PossibleValueSet.TakeNative(
				NativeMethods.BNGetMediumLevelILPossibleSSAVarValues(
					this.Variable.ILFunction.DangerousGetHandle() , 
					this.Variable.ToNative(),
					this.Version ,
					instruction,
					options,
					(ulong)options.Length
				)
			);
		}
		
		public bool IsLiveAt(MediumLevelILInstructionIndex instruction)
		{
			return NativeMethods.BNIsMediumLevelILSSAVarLiveAt(
				this.Variable.ILFunction.DangerousGetHandle() ,
				this.Variable.ToNative() ,
				this.Version,
				instruction
			);
		}
	}
}
