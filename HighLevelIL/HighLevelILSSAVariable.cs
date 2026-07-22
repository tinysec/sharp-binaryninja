using System;
using System.Collections.Generic;

namespace BinaryNinja
{
	public sealed class HighLevelILSSAVariable : AbstractSSAVariable<HighLevelILVariable>,
		IHighLevelILVariable
	{
		public HighLevelILSSAVariable(HighLevelILVariable variable , ulong version) 
			:base(variable, version)
		{
			
		}

		public HighLevelILInstruction? Definition
		{
			get
			{
				return this.Variable.ILFunction.GetExpression(
					NativeMethods.BNGetHighLevelILSSAVarDefinition(
						this.Variable.ILFunction.DangerousGetHandle() ,
						this.Variable.ToNative() ,
						this.Version
					)
				);
			}
		}
		
		public HighLevelILInstruction[] Uses
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetHighLevelILSSAVarUses(
					this.Variable.ILFunction.DangerousGetHandle() ,
					this.Variable.ToNative() ,
					this.Version ,
					out ulong arrayLength
				);

				ulong[] indexes = UnsafeUtils.TakeNumberArray<ulong>(
					arrayPointer ,
					arrayLength ,
					NativeMethods.BNFreeILInstructionList
				);
		    
				List<HighLevelILInstruction>  instructions = new List<HighLevelILInstruction>();

				foreach (HighLevelILExpressionIndex index in indexes)
				{
					instructions.Add(
						this.Variable.ILFunction.MustGetExpression(index)
					);
				}
		    
				return instructions.ToArray();
			}
		}
		
		public bool IsLive
		{
			get
			{
				return NativeMethods.BNIsHighLevelILSSAVarLive(
					this.Variable.ILFunction.DangerousGetHandle() ,
					this.Variable.ToNative() ,
					this.Version
				);
			}
		}
		
		public bool IsLiveAt(HighLevelILInstructionIndex instruction)
		{
			return NativeMethods.BNIsHighLevelILSSAVarLiveAt(
				this.Variable.ILFunction.DangerousGetHandle() ,
				this.Variable.ToNative() ,
				this.Version ,
				instruction
			);
		}
	}
}
