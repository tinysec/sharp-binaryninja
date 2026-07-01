using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class BinaryWriter : AbstractSafeHandle<BinaryWriter>
	{
		public BinaryWriter(BinaryView view)
			: this(NativeMethods.BNCreateBinaryWriter(view.DangerousGetHandle()) , true)
		{
			
		}
		
	    internal BinaryWriter(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	       
	    }

	    internal static BinaryWriter? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new BinaryWriter(handle, true);
	    }
	    
	    internal static BinaryWriter MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new BinaryWriter(handle, true);
	    }
	    
	    internal static BinaryWriter? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new BinaryWriter(handle, false);
	    }
	    
	    internal static BinaryWriter MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new BinaryWriter(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeBinaryWriter(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }
	    
	    public Endianness Endianness
	    {
		    get
		    {
			    return NativeMethods.BNGetBinaryWriterEndianness(this.handle);
		    }
		    set
		    {
			    NativeMethods.BNSetBinaryWriterEndianness(this.handle, value);
		    }
	    }
	    
	    public ulong Position
	    {
		    get
		    {
			    return NativeMethods.BNGetWriterPosition(this.handle);
		    }

		    set
		    {
			    if (this.Position != value)
			    {
				    NativeMethods.BNSeekBinaryWriter(this.handle, value);
			    }
		    }
	    }
	    
	    public bool WriteData(byte[] data)
	    {
		    return NativeMethods.BNWriteData(
			    this.handle ,
			    data , 
			    (ulong)data.Length
		    );
	    }
	    
	    public bool WriteByte(byte value)
	    {
		    return NativeMethods.BNWrite8(this.handle ,value);
	    }
	    
	    public bool WriteInt8(sbyte value)
	    {
		    return NativeMethods.BNWrite8(this.handle ,(byte)value);
	    }
	    
	    public bool WriteUInt8(byte value)
	    {
		    return NativeMethods.BNWrite8(this.handle ,value);
	    }
	    
	    // 16
	    public bool WriteInt16(short value)
	    {
		    return NativeMethods.BNWrite16(this.handle , (ushort)value);
	    }
	    
	    public bool WriteInt16BE(short value)
	    {
		    return NativeMethods.BNWriteBE16(this.handle , (ushort)value);
	    }
	    
	    public bool WriteInt16LE(short value)
	    {
		    return NativeMethods.BNWriteLE16(this.handle , (ushort)value);
	    }
	    
	    public bool WriteUInt16(ushort value)
	    {
		    return NativeMethods.BNWrite16(this.handle ,value);
	    }
	    
	    public bool WriteUInt16BE(ushort value)
	    {
		    return NativeMethods.BNWriteBE16(this.handle ,value);
	    }
	    
	    public bool WriteUInt16LE(ushort value)
	    {
		    return NativeMethods.BNWriteLE16(this.handle ,value);
	    }
	    
	    // 32
	    public bool WriteInt32(int value)
	    {
		    return NativeMethods.BNWrite32(this.handle , (uint)value);
	    }
	    
	    public bool WriteInt32BE(int value)
	    {
		    return NativeMethods.BNWriteBE32(this.handle , (uint)value);
	    }
	    
	    public bool WriteInt32LE(int value)
	    {
		    return NativeMethods.BNWriteLE32(this.handle , (uint)value);
	    }
	    
	    public bool WriteUInt32(uint value)
	    {
		    return NativeMethods.BNWrite32(this.handle ,value);
	    }
	    
	    public bool WriteUInt32BE(uint value)
	    {
		    return NativeMethods.BNWriteBE32(this.handle ,value);
	    }
	    
	    public bool WriteUInt32LE(uint value)
	    {
		    return NativeMethods.BNWriteLE32(this.handle ,value);
	    }
	    
	    // 64
	    public bool WriteInt64(long value)
	    {
		    return NativeMethods.BNWrite64(this.handle , (ulong)value);
	    }
	    
	    public bool WriteInt64BE(long value)
	    {
		    return NativeMethods.BNWriteBE64(this.handle , (ulong)value);
	    }
	    
	    public bool WriteInt64LE(long value)
	    {
		    return NativeMethods.BNWriteLE64(this.handle , (ulong)value);
	    }
	    
	    public bool WriteUInt64(ulong value)
	    {
		    return NativeMethods.BNWrite64(this.handle ,value);
	    }
	    
	    public bool WriteUInt64BE(ulong value)
	    {
		    return NativeMethods.BNWriteBE64(this.handle ,value);
	    }
	    
	    public bool WriteUInt64LE(ulong value)
	    {
		    return NativeMethods.BNWriteLE64(this.handle ,value);
	    }

	    public bool WriteString(string text , Encoding? encoding = null)
	    {
		    if (null == encoding)
		    {
			    encoding = Encoding.UTF8;
		    }

		    byte[] data = encoding.GetBytes(text);

		    return NativeMethods.BNWriteData(
			    this.handle ,
			    data ,
			    (ulong)data.Length
		    );
	    }

	    public void SeekRelative(long offset)
	    {
		    NativeMethods.BNSeekBinaryWriterRelative(this.handle , offset);
	    }

	}
}