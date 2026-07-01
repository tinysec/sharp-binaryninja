using System;

namespace BinaryNinja
{
	public sealed class LowLevelILVariable : AbstractFunctionVariable<LowLevelILVariable>
	{
		internal LowLevelILFunction ILFunction;

		public LowLevelILVariable(LowLevelILVariable other) 
			: base(
				other.ILFunction.OwnerFunction , 
				other.Type,
				other.Index,
				other.Storage
			)
		{
			this.ILFunction = other.ILFunction;
		}
		
		public LowLevelILVariable(LowLevelILFunction ilFunction , Variable variable) 
			: base(ilFunction.OwnerFunction , variable.ToNative())
		{
			this.ILFunction = ilFunction;
		}
		
		public LowLevelILVariable(LowLevelILFunction ilFunction , CoreVariable variable) 
			: base(ilFunction.OwnerFunction , variable.ToNative())
		{
			this.ILFunction = ilFunction;
		}

		
		internal LowLevelILVariable(LowLevelILFunction ilFunction , BNVariable native) 
			: base(ilFunction.OwnerFunction , native)
		{
			this.ILFunction = ilFunction;
		}
	   
		internal static LowLevelILVariable FromIdentifier(LowLevelILFunction function ,ulong identifier)
		{
			return LowLevelILVariable.FromNative(
				function,
				NativeMethods.BNFromVariableIdentifier(identifier)
			);
		}
	    
		internal static LowLevelILVariable FromNative(
			LowLevelILFunction function , 
			BNVariable native)
		{
			return new LowLevelILVariable(function,  native);
		}
		
		
	}
}
