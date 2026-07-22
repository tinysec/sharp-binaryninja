using System;
using System.Collections.Generic;

namespace BinaryNinja
{
	public sealed class LowLevelILSSAFlag
	{
		private readonly ulong version;

		public LowLevelILFlag Flag { get;  }
		
		public ulong Version
		{
			get
			{
				return this.version;
			}
		}

		[Obsolete("Use Version.")]
		public ulong Vesion
		{
			get
			{
				return this.version;
			}
		}
		
		public LowLevelILSSAFlag(LowLevelILFunction function , ILFlag flag , ulong version)
			:this( new LowLevelILFlag(function , flag) ,  version )
		{
		
		}
		
		public LowLevelILSSAFlag(LowLevelILFlag flag , ulong version)
		{
			this.Flag = flag;
			this.version = version;
		}
		
		public override string ToString()
		{
			return $"{this.Flag.Name}#{this.Version}";
		}
		
		public LowLevelILInstruction? Definition
		{
			get
			{
				LowLevelILInstructionIndex index = NativeMethods.BNGetLowLevelILSSAFlagDefinition(
					this.Flag.ILFunction.DangerousGetHandle() ,
					this.Flag.Index ,
					this.Version
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
					this.Version,
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
						this.Version
					)
				);
			}
		}

		public ulong[] Versions
		{
			get
			{
				return this.Flag.ILFunction.GetFlagSSAVersions(this.Flag.Index);
			}
		}
	}
}
