using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNRegisterValueWithConfidenceAndRegister
	{
		/// <summary>
		/// uint32_t reg
		/// </summary>
		internal uint reg;

		/// <summary>
		/// BNRegisterValueWithConfidence value
		/// </summary>
		internal BNRegisterValueWithConfidence value;
	}

    public sealed class RegisterValueWithConfidenceAndRegister : INativeWrapper<BNRegisterValueWithConfidenceAndRegister>
    {
		public uint Register { get; set; } = 0;

		public RegisterValueWithConfidence Value { get; set; } = new RegisterValueWithConfidence();

		public RegisterValueWithConfidenceAndRegister()
		{

		}

		internal static RegisterValueWithConfidenceAndRegister FromNative(BNRegisterValueWithConfidenceAndRegister native)
		{
			return new RegisterValueWithConfidenceAndRegister()
			{
				Register = native.reg,
				Value = RegisterValueWithConfidence.FromNative(native.value)
			};
		}

		public BNRegisterValueWithConfidenceAndRegister ToNative()
		{
			return new BNRegisterValueWithConfidenceAndRegister()
			{
				reg = this.Register,
				value = this.Value.ToNative()
			};
		}
    }
}
