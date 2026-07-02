using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNParsedType 
	{
		/// <summary>
		/// BNQualifiedName name
		/// </summary>
		internal BNQualifiedName name;
		
		/// <summary>
		/// BNType* type
		/// </summary>
		internal IntPtr type;
		
		/// <summary>
		/// bool isUser
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] internal bool isUser;
	}

    public sealed class ParsedType : INativeWrapperEx<BNParsedType>
    {
		public QualifiedName Name { get; set; } = new QualifiedName();
		
		public BinaryNinja.Type? Type { get; set; } = null;
		
		public bool IsUser { get; set; } = false;
		
		public ParsedType() 
		{
		    
		}

		internal static ParsedType FromNative(BNParsedType native)
		{
			return new ParsedType()
			{
				Name = QualifiedName.FromNative(native.name) , 
				Type = BinaryNinja.Type.NewFromHandle(native.type),
				IsUser = native.isUser
			};
		}

		public BNParsedType ToNativeEx(ScopedAllocator allocator)
		{
			return new BNParsedType()
			{
				name = this.Name.ToNativeEx(allocator) , 
				type = ( null == this.Type ? IntPtr.Zero : this.Type.DangerousGetHandle() ) ,
				isUser = this.IsUser
			};
		}
    }
}