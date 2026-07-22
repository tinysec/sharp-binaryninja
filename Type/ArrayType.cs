using System;

namespace BinaryNinja
{
	public sealed class ArrayType : BinaryNinja.Type
	{
		public ArrayType(
			TypeWithConfidence elementType,
			ulong elementCount 
		) : base( ArrayType.rawCreate(elementType , elementCount) , true)
		{
			
		}
		
		public ArrayType(BinaryNinja.Type type) 
			: base(BinaryNinja.Type.NewReferenceHandle(type), true)
		{
			
		}
		
		internal ArrayType(IntPtr handle , bool owner) : base(handle , owner)
		{
			
		}

		private static IntPtr rawCreate(
			TypeWithConfidence elementType,
			ulong elementCount 
		)
		{
			return NativeMethods.BNCreateArrayType(
				elementType.ToNative() , 
				elementCount
			);
		}
		
		public ulong ElementCount
		{
			get
			{
				return NativeMethods.BNGetTypeElementCount(this.handle);
			}
		}

		public TypeWithConfidence ElementType
		{
			get
			{
				return TypeWithConfidence.FromNative( NativeMethods.BNGetChildType(this.handle) );
			}
		}
	}
}
