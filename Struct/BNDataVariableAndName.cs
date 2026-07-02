using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNDataVariableAndName 
	{
		/// <summary>
		/// uint64_t address
		/// </summary>
		internal ulong address;
		
		/// <summary>
		/// BNType* type
		/// </summary>
		internal IntPtr type;
		
		/// <summary>
		/// const char* name
		/// </summary>
		internal IntPtr name;
		
		/// <summary>
		/// bool autoDiscovered
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] internal bool autoDiscovered;
		
		/// <summary>
		/// uint8_t typeConfidence
		/// </summary>
		internal byte typeConfidence;
	}

    public class DataVariableAndName 
    {
		public ulong Address { get; set; } = 0;
		
		public BinaryNinja.Type Type { get; set; }
		
		public string Name { get; set; } = string.Empty;
		
		public bool AutoDiscovered { get; set; } = false;
		
		public byte TypeConfidence { get; set; } = 0;
		
		public DataVariableAndName(
			ulong address,
			BinaryNinja.Type type,
			string name,
			bool autoDiscovered,
			byte typeConfidence
		) 
		{
		    this.Address = address;
		    this.Type = type;
		    this.Name = name;
		    this.AutoDiscovered = autoDiscovered;
		    this.TypeConfidence = typeConfidence;
		}
		
		public DataVariableAndName(BNDataVariableAndName native)
		{
			this.Address = native.address;
			this.Type = BinaryNinja.Type.MustNewFromHandle(native.type);
			this.Name = UnsafeUtils.ReadAnsiString(native.name);
			this.AutoDiscovered = native.autoDiscovered;
			this.TypeConfidence = native.typeConfidence;
		}
		
		internal static DataVariableAndName FromNative(BNDataVariableAndName native)
		{
			return new DataVariableAndName(native);
		}

		internal BNDataVariableAndName ToNative(ScopedAllocator allocator)
		{
			return new BNDataVariableAndName()
			{
				address = this.Address ,
				type = this.Type.DangerousGetHandle() ,
				name = allocator.AllocAnsiString(this.Name) ,
				autoDiscovered = this.AutoDiscovered ,
				typeConfidence = this.TypeConfidence
			};
		}
    }
}