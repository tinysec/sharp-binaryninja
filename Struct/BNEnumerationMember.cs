using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNEnumerationMember 
	{
		/// <summary>
		/// const char* name
		/// </summary>
		public IntPtr name;
		
		/// <summary>
		/// uint64_t value
		/// </summary>
		public ulong value;
		
		/// <summary>
		/// bool isDefault
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool isDefault;
	}

    public class EnumerationMember 
    {
		public string Name { get; set; } = string.Empty;
		
		public ulong Value { get; set; } = 0;
		
		public bool IsDefault { get; set; } = false;
	
		public EnumerationMember(string name, ulong value , bool isDefault = false) 
		{
		    this.Name = name;
		    this.Value = value;
		    this.IsDefault = isDefault;
		}

		internal static EnumerationMember FromNative(BNEnumerationMember raw)
		{
			return new EnumerationMember(
				UnsafeUtils.ReadAnsiString(raw.name),
				raw.value,
				raw.isDefault
			);
		}
		
		internal BNEnumerationMember ToNative(ScopedAllocator allocator)
		{
			return new BNEnumerationMember()
			{
				name =  allocator.AllocAnsiString(this.Name),
				value = this.Value,
				isDefault = this.IsDefault
			};
		}

		public override string ToString()
		{
			return $"{this.Name} = {this.Value}";
		}
		
    }
}