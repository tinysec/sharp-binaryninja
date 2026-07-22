using System;

namespace BinaryNinja
{
	public sealed class FloatType : BinaryNinja.Type
	{
		public FloatType(ulong width ,  string altName) 
			: base( NativeMethods.BNCreateFloatType(width , altName) , true)
		{
			
		}
	
		public FloatType(BinaryNinja.Type type) 
			: base(BinaryNinja.Type.NewReferenceHandle(type), true)
		{
			
		}
		
		internal FloatType(IntPtr handle , bool owner) : base(handle , owner)
		{
			
		}
	}
}
