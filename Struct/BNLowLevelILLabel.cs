using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNLowLevelILLabel 
	{
		/// <summary>
		/// bool resolved
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] internal bool resolved;
		
		/// <summary>
		/// uint64_t _ref
		/// </summary>
		internal ulong reference;
		
		/// <summary>
		/// uint64_t operand
		/// </summary>
		internal ulong operand;
	}

    public sealed class LowLevelILLabel : INativeWrapper<BNLowLevelILLabel>
    {
		public bool Resolved { get; set; } = false;
		
		public ulong Reference { get; set; } = 0;

		public ulong Operand { get; set; } = 0;
		
		public LowLevelILLabel()
		{
			BNLowLevelILLabel native = new BNLowLevelILLabel();
			
			NativeMethods.BNLowLevelILInitLabel(ref native);
			
			this.Resolved = native.resolved;
			
			this.Reference = native.reference;
			
			this.Operand = native.operand;
		}

		internal LowLevelILLabel(BNLowLevelILLabel native)
		{
			this.Resolved = native.resolved;
			this.Reference = native.reference;
			this.Operand = native.operand;
		}
		
		internal static LowLevelILLabel? FromNativePointer(IntPtr pointer)
		{
			if (IntPtr.Zero == pointer)
			{
				return null;
			}
			
			return LowLevelILLabel.FromNative(Marshal.PtrToStructure<BNLowLevelILLabel>(pointer));
		}
		
		internal static LowLevelILLabel FromNative(BNLowLevelILLabel native)
		{
			return new LowLevelILLabel()
			{
				Resolved = native.resolved , 
				Reference = native.reference ,
				Operand = native.operand
			};
		}
		
		public BNLowLevelILLabel ToNative()
		{
			return new BNLowLevelILLabel()
			{
				resolved = this.Resolved , 
				reference = this.Reference ,
				operand = this.Operand 
			};
		}
    }
}