using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNConstantReference 
	{
		/// <summary>
		/// int64_t value
		/// </summary>
		public long value;
		
		/// <summary>
		/// uint64_t size
		/// </summary>
		public ulong size;
		
		/// <summary>
		/// bool pointer
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool pointer;
		
		/// <summary>
		/// bool intermediate
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool intermediate;
	}

    public sealed class ConstantReference 
    {
		public long Value { get; set; } = 0;
		
		public ulong Size { get; set; } = 0;
		
		public bool Pointer { get; set; } = false;
		
		public bool Intermediate { get; set; } = false;
		
		public ConstantReference() 
		{
		    
		}

		internal static ConstantReference FromNative(BNConstantReference native)
		{
			return new ConstantReference()
			{
				Value = native.value , 
				Size = native.size ,
				Pointer = native.pointer ,
				Intermediate = native.intermediate
			};
		}
    }
}