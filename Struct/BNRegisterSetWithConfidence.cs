using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNRegisterSetWithConfidence 
	{
		/// <summary>
		/// uint32_t* regs
		/// </summary>
		internal IntPtr regs;
		
		/// <summary>
		/// uint64_t count
		/// </summary>
		internal ulong count;
		
		/// <summary>
		/// uint8_t confidence
		/// </summary>
		internal byte confidence;
	}

    public sealed class RegisterSetWithConfidence : INativeWrapperEx<BNRegisterSetWithConfidence>
    {
		public uint[] Registers { get; set; } = Array.Empty<uint>();
		
		public byte Confidence { get; set; } = 0;
		
		public RegisterSetWithConfidence() 
		{
			
		}
		
		public RegisterSetWithConfidence(uint[] regs , byte confidence) 
		{
		    this.Registers = regs;
		    this.Confidence = confidence;
		}

		internal static RegisterSetWithConfidence FromNative(BNRegisterSetWithConfidence raw)
		{
			return new RegisterSetWithConfidence(
				UnsafeUtils.ReadNumberArray<uint>(raw.regs , raw.count) ,
				raw.confidence
			);
		}
		
		public BNRegisterSetWithConfidence ToNativeEx(ScopedAllocator allocator)
		{
			return new BNRegisterSetWithConfidence()
			{
				regs = allocator.AllocIntegerArray<uint>(this.Registers),
				count = (ulong)this.Registers.Length,
				confidence = this.Confidence
			};
		}
    }
}
