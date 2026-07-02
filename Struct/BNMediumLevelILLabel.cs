using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNMediumLevelILLabel 
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

    public sealed class MediumLevelILLabel : INativeWrapper<BNMediumLevelILLabel>
    {
		public bool Resolved { get; set; } = false;
		
		public ulong Reference { get; set; } = 0;
		
		public ulong Operand { get; set; } = 0;
		
		public MediumLevelILLabel() 
		{
			BNMediumLevelILLabel native = new BNMediumLevelILLabel();
			
			NativeMethods.BNMediumLevelILInitLabel(ref native);
			
			this.Resolved = native.resolved;
			this.Reference = native.reference;
			this.Operand = native.operand;
		}
		
		internal MediumLevelILLabel(BNMediumLevelILLabel native) 
		{
		    this.Resolved = native.resolved;
		    this.Reference = native.reference;
		    this.Operand = native.operand;
		}

		internal static MediumLevelILLabel FromNative(BNMediumLevelILLabel native)
		{
			return new MediumLevelILLabel(native);
		}

		internal static MediumLevelILLabel? FromNativePointer(IntPtr pointer)
		{
			if (IntPtr.Zero == pointer)
			{
				return null;
			}
			
			return MediumLevelILLabel.FromNative(Marshal.PtrToStructure<BNMediumLevelILLabel>(pointer));
		}

		public BNMediumLevelILLabel ToNative()
		{
			return new BNMediumLevelILLabel()
			{
				resolved = this.Resolved , 
				reference = this.Reference ,
				operand = this.Operand ,
			};
		}
    }
}