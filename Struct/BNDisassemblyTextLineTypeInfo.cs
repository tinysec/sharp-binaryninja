using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNDisassemblyTextLineTypeInfo 
	{
		/// <summary>
		/// bool hasTypeInfo
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] internal bool hasTypeInfo;
		
		/// <summary>
		/// BNType* parentType
		/// </summary>
		internal IntPtr parentType;
		
		/// <summary>
		/// uint64_t fieldIndex
		/// </summary>
		internal ulong fieldIndex;
		
		/// <summary>
		/// uint64_t offset
		/// </summary>
		internal ulong offset;
	}

    public sealed class DisassemblyTextLineTypeInfo 
    {
		public bool HasTypeInfo { get; set; } = false;
		
		public BinaryNinja.Type? ParentType { get; set; } = null;
		
		public ulong FieldIndex { get; set; } = ulong.MaxValue;
		
		public ulong Offset { get; set; } = 0;

		public DisassemblyTextLineTypeInfo()
		{
			
		}
		
		internal DisassemblyTextLineTypeInfo(BNDisassemblyTextLineTypeInfo native)
		{
			this.HasTypeInfo = native.hasTypeInfo;
			this.ParentType =  ( IntPtr.Zero == native.parentType ? null :  BinaryNinja.Type.MustNewFromHandle(native.parentType) );
			this.FieldIndex = native.fieldIndex;
			this.Offset = native.offset;
		}
		
		internal static DisassemblyTextLineTypeInfo FromNative(BNDisassemblyTextLineTypeInfo native)
		{
			return new DisassemblyTextLineTypeInfo(native);
		}

		internal BNDisassemblyTextLineTypeInfo ToNative()
		{
			return new BNDisassemblyTextLineTypeInfo()
			{
				hasTypeInfo = this.HasTypeInfo ,
				parentType = ( null == this.ParentType ? IntPtr.Zero : this.ParentType.DangerousGetHandle() ) ,
				fieldIndex = this.FieldIndex ,
				offset = this.Offset
			};
		}
		
		
    }
}
