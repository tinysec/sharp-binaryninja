using System;
using System.Collections.Generic;

namespace BinaryNinja
{
	public sealed class HighLevelILVariable : AbstractFunctionVariable<HighLevelILVariable>
	{
		public HighLevelILFunction ILFunction { get; }

		internal HighLevelILVariable(HighLevelILVariable other) 
			: base(
				other.ILFunction.OwnerFunction , 
				other.Type,
				other.Index,
				other.Storage
			)
		{
			this.ILFunction = other.ILFunction;
		}
		
		public HighLevelILVariable(HighLevelILFunction ilFunction , Variable variable) 
			: base(ilFunction.OwnerFunction , variable.ToNative())
		{
			this.ILFunction = ilFunction;
		}
		
		public HighLevelILVariable(HighLevelILFunction ilFunction , CoreVariable variable) 
			: base(ilFunction.OwnerFunction , variable.ToNative())
		{
			this.ILFunction = ilFunction;
		}
		
		internal HighLevelILVariable(HighLevelILFunction ilFunction , BNVariable native) 
			: base(ilFunction.OwnerFunction , native)
		{
			this.ILFunction = ilFunction;
		}
		
		internal static HighLevelILVariable FromIdentifierEx(HighLevelILFunction function ,ulong identifier)
		{
			return HighLevelILVariable.FromNativeEx(
				function,
				NativeMethods.BNFromVariableIdentifier(identifier)
			);
		}
	    
		internal static HighLevelILVariable FromNativeEx(
			HighLevelILFunction function , 
			BNVariable native)
		{
			return new HighLevelILVariable(function,  native);
		}
		
		public bool IsLiveAt(HighLevelILInstructionIndex instruction)
		{
			return NativeMethods.BNIsHighLevelILVarLiveAt(
				this.ILFunction.DangerousGetHandle() ,
				this.ToNative() ,
				instruction
			);
		}
		
		public HighLevelILInstruction[] Definitions
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetHighLevelILVariableDefinitions(
					this.ILFunction.DangerousGetHandle() ,
					this.ToNative() ,
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
						this.ILFunction.MustGetExpression(index)
					);
				}
		    
				return instructions.ToArray();
			}
		}
		
		public HighLevelILInstruction[] Uses
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetHighLevelILVariableUses(
					this.ILFunction.DangerousGetHandle() ,
					this.ToNative() ,
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
						this.ILFunction.MustGetExpression(index)
					);
				}
		    
				return instructions.ToArray();
			}
		}
	}
}
