using System;

namespace BinaryNinja
{
	public sealed class VoidType : BinaryNinja.Type
	{
		public VoidType() : base( NativeMethods.BNCreateVoidType() , true)
		{
			
		}
		
		public VoidType(BinaryNinja.Type type) 
			: base(BinaryNinja.Type.NewReferenceHandle(type), true)
		{
			
		}
		
		internal VoidType(IntPtr handle , bool owner) : base(handle , owner)
		{
			
		}
	}
}
