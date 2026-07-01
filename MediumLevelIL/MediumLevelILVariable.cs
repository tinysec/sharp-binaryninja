using System;
using System.Collections.Generic;

namespace BinaryNinja
{
	public sealed class MediumLevelILVariable : AbstractFunctionVariable<MediumLevelILVariable>
	{
		public MediumLevelILFunction ILFunction { get; }

		internal MediumLevelILVariable(MediumLevelILVariable other) 
			: base(
				other.ILFunction.OwnerFunction,
				other.Type,
				other.Index,
				other.Storage
		)
		{
			this.ILFunction = other.ILFunction;
		}
		
		public MediumLevelILVariable(MediumLevelILFunction ilFunction , Variable variable) 
			: base(ilFunction.OwnerFunction , variable.ToNative() )
		{
			this.ILFunction = ilFunction;
		}

		public MediumLevelILVariable(MediumLevelILFunction ilFunction , CoreVariable variable) 
			: base(ilFunction.OwnerFunction , variable.ToNative() )
		{
			this.ILFunction = ilFunction;
		}
		
		internal MediumLevelILVariable(MediumLevelILFunction ilFunction , BNVariable native) 
			: base(ilFunction.OwnerFunction , native)
		{
			this.ILFunction = ilFunction;
		}
	   
		internal static MediumLevelILVariable FromIdentifier(MediumLevelILFunction function ,ulong identifier)
		{
			return MediumLevelILVariable.FromNative(
				function,
				NativeMethods.BNFromVariableIdentifier(identifier)
			);
		}
	    
		internal static MediumLevelILVariable FromNative(
			MediumLevelILFunction function , 
			BNVariable native)
		{
			return new MediumLevelILVariable(function,  native);
		}
	    
		public ulong[] SSAVersions
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetMediumLevelILVariableSSAVersions(
					this.ILFunction.DangerousGetHandle() ,
					this.ToNative() ,
					out ulong arrayLength
				);
		    
				return UnsafeUtils.TakeNumberArray<ulong>(
					arrayPointer,
					arrayLength,
					NativeMethods.BNFreeILInstructionList
				);
			}
		}
		
		public bool IsLiveAt(MediumLevelILInstructionIndex instruction)
		{
			return NativeMethods.BNIsMediumLevelILVarLiveAt(
				this.ILFunction.DangerousGetHandle() ,
				this.ToNative() ,
				instruction
			);
		}
		
		public MediumLevelILInstruction[] Definitions
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetMediumLevelILVariableDefinitions(
					this.ILFunction.DangerousGetHandle() ,
					this.ToNative() ,
					out ulong arrayLength
				);

				ulong[] indexes = UnsafeUtils.TakeNumberArray<ulong>(
					arrayPointer ,
					arrayLength ,
					NativeMethods.BNFreeILInstructionList
				);
		    
				List<MediumLevelILInstruction>  instructions = new List<MediumLevelILInstruction>();

				foreach (MediumLevelILExpressionIndex index in indexes)
				{
					instructions.Add(
						this.ILFunction.MustGetExpression(index)
					);
				}
		    
				return instructions.ToArray();
			}
		}
		
		public MediumLevelILInstruction[] Uses
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetMediumLevelILVariableUses(
					this.ILFunction.DangerousGetHandle() ,
					this.ToNative() ,
					out ulong arrayLength
				);

				ulong[] indexes = UnsafeUtils.TakeNumberArray<ulong>(
					arrayPointer ,
					arrayLength ,
					NativeMethods.BNFreeILInstructionList
				);
		    
				List<MediumLevelILInstruction>  instructions = new List<MediumLevelILInstruction>();

				foreach (MediumLevelILExpressionIndex index in indexes)
				{
					instructions.Add(
						this.ILFunction.MustGetExpression(index)
					);
				}
		    
				return instructions.ToArray();
			}
		}
	}
}
