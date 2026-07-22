using System;

namespace BinaryNinja
{
	public class IntegerType : BinaryNinja.Type
	{
		public IntegerType(ulong width , BoolWithConfidence sign , string altName) 
			: base( IntegerType.rawCreate(width , sign , altName) , true)
		{
			
		}
		
		public IntegerType(BinaryNinja.Type type) 
			: base(BinaryNinja.Type.NewReferenceHandle(type), true)
		{
			
		}
		
		private static IntPtr rawCreate(ulong width , BoolWithConfidence sign , string altName)
		{
			return NativeMethods.BNCreateIntegerType(
				width , 
				sign.ToNative() , 
				altName 
			);
		}
		
		internal IntegerType(IntPtr handle , bool owner) : base(handle , owner)
		{
			
		}
		
		public IntegerDisplayType DisplayType
		{
			get
			{
				return NativeMethods.BNGetIntegerTypeDisplayType(this.handle);
			}
		}
		
		public BoolWithConfidence IsSigned
		{
			get
			{
				return BoolWithConfidence.FromNative(

					NativeMethods.BNIsTypeSigned(this.handle)
				);
			}
		}
	}
}
