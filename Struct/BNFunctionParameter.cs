using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNFunctionParameter 
	{
		/// <summary>
		/// const char* name
		/// </summary>
		internal IntPtr name;
		
		/// <summary>
		/// BNType* type
		/// </summary>
		internal IntPtr type;
		
		/// <summary>
		/// uint8_t typeConfidence
		/// </summary>
		internal byte typeConfidence;
		
		/// <summary>
		/// bool defaultLocation
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] internal bool defaultLocation;
		
		/// <summary>
		/// BNVariable location
		/// </summary>
		internal BNVariable location;
	}
	
    public class FunctionParameter : INativeWrapperEx<BNFunctionParameter>
    {
		public string Name { get; set; } = string.Empty;
		
		public BinaryNinja.Type Type { get; set; }
		
		public byte TypeConfidence { get; set; } = 0;
		
		public bool DefaultLocation { get; set; } = false;
		
		public CoreVariable Location { get; set; }
	
		internal FunctionParameter(BNFunctionParameter native) 
		{
			this.Name = UnsafeUtils.ReadAnsiString(native.name);
			
			this.Type = new BinaryNinja.Type(
				NativeMethods.BNNewTypeReference(native.type ) , 
				true
			);
			
			this.TypeConfidence = native.typeConfidence;
			
			this.DefaultLocation = native.defaultLocation;
			
			this.Location = new CoreVariable( native.location);
		}

		internal static FunctionParameter FromNative(BNFunctionParameter native)
		{
			return new FunctionParameter(native);
		}
		
		public BNFunctionParameter ToNativeEx(ScopedAllocator allocator)
		{
			return new BNFunctionParameter()
			{
				name = allocator.AllocAnsiString(this.Name),
				type = this.Type.DangerousGetHandle(),
				typeConfidence = this.TypeConfidence,
				defaultLocation = this.DefaultLocation,
				location = this.Location.ToNative()
			};
		}
    }
}