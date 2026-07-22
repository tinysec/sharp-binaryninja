using System;

namespace BinaryNinja
{
	public sealed class WideCharType : BinaryNinja.Type
	{
		public WideCharType(ulong width = 2 , string altName = "WCHAR") 
			: base( NativeMethods.BNCreateWideCharType(width , altName) , true)
		{
			
		}
		
		public WideCharType(BinaryNinja.Type type) 
			: base(BinaryNinja.Type.NewReferenceHandle(type), true)
		{
			
		}
		
		internal WideCharType(IntPtr handle , bool owner) : base(handle , owner)
		{
			
		}
	}
}
