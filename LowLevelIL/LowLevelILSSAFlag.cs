using System;
using System.Collections.Generic;

namespace BinaryNinja
{
	public sealed class LowLevelILSSAFlag
	{
		public LowLevelILFlag Flag { get;  }
		
		public ulong Vesion { get;  } = 0;
		
		public LowLevelILSSAFlag(LowLevelILFunction function , ILFlag flag , ulong vesion)
			:this( new LowLevelILFlag(function , flag) ,  vesion )
		{
		
		}
		
		public LowLevelILSSAFlag(LowLevelILFlag flag , ulong vesion)
		{
			this.Flag = flag;
			this.Vesion = vesion;
		}
		
		public override string ToString()
		{
			return $"{this.Flag.Name}#{this.Vesion}";
		}
		
		public LowLevelILInstruction? Definition
		{
			get
			{
				LowLevelILInstructionIndex index = NativeMethods.BNGetLowLevelILSSAFlagDefinition(
					this.Flag.ILFunction.DangerousGetHandle() ,
					this.Flag.Index ,
					this.Vesion
				);

				if ((ulong)index >= this.Flag.ILFunction.InstructionCount)
				{
					return null;
				}

				return this.Flag.ILFunction.MustGetInstruction(index);
			}
		}
		
		public LowLevelILInstruction[] Uses
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetLowLevelILSSAFlagUses(
					this.Flag.ILFunction.DangerousGetHandle() ,
					this.Flag.Index ,
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
					instructions.Add( this.Flag.ILFunction.MustGetInstruction( index ) );
				}
		    
				return instructions.ToArray();
			}
		}
		
		public RegisterValue Value
		{
			get
			{
				return RegisterValue.FromNative(
					NativeMethods.BNGetLowLevelILSSAFlagValue(
						this.Flag.ILFunction.DangerousGetHandle() , 
						this.Flag.Index ,
						this.Vesion
					)
				);
			}
		}
	}
}
