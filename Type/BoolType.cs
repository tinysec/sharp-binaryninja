using System;

namespace BinaryNinja
{
	public sealed class BoolType : BinaryNinja.Type
	{
		public BoolType() : base( NativeMethods.BNCreateBoolType() , true)
		{
			
		}
		
		public BoolType(BinaryNinja.Type type) 
			: base(BinaryNinja.Type.NewReferenceHandle(type), true)
		{
			
		}
		
		internal BoolType(IntPtr handle , bool owner) : base(handle , owner)
		{
			
		}
	}
}
