using System;
using System.Collections.Generic;

namespace BinaryNinja
{
	public sealed class LowLevelILSSARegister 
	{
		public ILRegister Register { get; }
		
		public ulong Vesion { get; set; } = 0;
		
		public LowLevelILFunction ILFunction { get; }

		public LowLevelILSSARegister(LowLevelILFunction function , ILRegister register , ulong vesion)
		{
			this.ILFunction = function;
			
			this.Register = register;
			
			this.Vesion = vesion;
		}
		
		public override string ToString()
		{
			return $"{this.Register.Name}#{this.Vesion}";
		}
		
		public LowLevelILInstruction? Definition
		{
			get
			{
				LowLevelILInstructionIndex index = NativeMethods.BNGetLowLevelILSSARegisterDefinition(
					this.ILFunction.DangerousGetHandle() ,
					this.Register.Index ,
					this.Vesion
				);

				if ((ulong)index >= this.ILFunction.InstructionCount)
				{
					return null;
				}

				return this.ILFunction.MustGetInstruction(index);
			}
		}
		
		public LowLevelILInstruction[] Uses
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetLowLevelILSSARegisterUses(
					this.ILFunction.DangerousGetHandle() ,
					this.Register.Index ,
					this.Vesion,
					out ulong arrayLength
				);

				if (0 == arrayLength)
				{
					return Array.Empty<LowLevelILInstruction>();
				}
		    
				ulong[] indices = UnsafeUtils.TakeNumberArray<ulong>(
					arrayPointer ,
					arrayLength ,
					NativeMethods.BNFreeILInstructionList
				);
		    
				List<LowLevelILInstruction> instructions = new List<LowLevelILInstruction>();

				foreach (LowLevelILInstructionIndex index in indices)
				{
					instructions.Add( this.ILFunction.MustGetInstruction( index ) );
				}
		    
				return instructions.ToArray();
			}
		}
		
		public RegisterValue Value
		{
			get
			{
				return RegisterValue.FromNative(
					NativeMethods.BNGetLowLevelILSSARegisterValue(
						this.ILFunction.DangerousGetHandle() , 
						this.Register.Index ,
						this.Vesion
					)
				);
			}
		}
		
		public ulong[] Versions
		{
			get
			{
				return this.ILFunction.SSAForm.GetRegisterSSAVersions(this.Register.Index);
			}
		}
	}
}
