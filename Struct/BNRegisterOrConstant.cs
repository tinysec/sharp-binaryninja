using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNRegisterOrConstant 
	{
		/// <summary>
		/// bool constant
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] internal bool constant;
		
		/// <summary>
		/// uint32_t reg
		/// </summary>
		internal uint reg;
		
		/// <summary>
		/// uint64_t value
		/// </summary>
		internal ulong value;
	}

    public sealed class RegisterOrConstant : INativeWrapper<BNRegisterOrConstant>
    {
		public bool Constant { get; set; } = false;
	
		public uint Register { get; set; } = 0;
		
		public ulong Value { get; set; } = 0;
		
		public RegisterOrConstant() 
		{
		    
		}

		internal static RegisterOrConstant FromNative(BNRegisterOrConstant native)
		{
			return new RegisterOrConstant()
			{
				Constant = native.constant ,
				Register = native.reg , 
				Value = native.value
			};
		}

		public BNRegisterOrConstant ToNative()
		{
			return new BNRegisterOrConstant()
			{
				constant = this.Constant , 
				reg = this.Register ,
				value = this.Value
			};
		}
    }
}