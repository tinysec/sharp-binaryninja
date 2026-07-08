using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class BinaryReader : AbstractSafeHandle<BinaryReader>
	{
		public BinaryReader(BinaryView view)
			: this(NativeMethods.BNCreateBinaryReader(view.DangerousGetHandle()) , true)
		{
			
		}
		
	    internal BinaryReader(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }
	   
	    internal static BinaryReader? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new BinaryReader(handle, true);
	    }
	    
	    internal static BinaryReader MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new BinaryReader(handle, true);
	    }
	    
	    internal static BinaryReader? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new BinaryReader(handle, false);
	    }
	    
	    internal static BinaryReader MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new BinaryReader(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeBinaryReader(this.handle);
	            this.SetHandleAsInvalid();
	        }

	        return true;
	    }

	    public Endianness Endianness
	    {
		    get
		    {
			    return NativeMethods.BNGetBinaryReaderEndianness(this.handle);
		    }
		    set
		    {
			    NativeMethods.BNSetBinaryReaderEndianness(this.handle, value);
		    }
	    }

	    public ulong Position
	    {
		    get
		    {
			    return NativeMethods.BNGetReaderPosition(this.handle);
		    }

		    set
		    {
			    if (this.Position != value)
			    {
				    NativeMethods.BNSeekBinaryReader(this.handle, value);
			    }
		    }
	    }

	    
	    public ulong VirtualBase
	    {
		    get
		    {
			    return NativeMethods.BNGetBinaryReaderVirtualBase(this.handle);
		    }

		    set
		    {
			    NativeMethods.BNSetBinaryReaderVirtualBase(this.handle, value);
		    }
	    }

	    public bool IsEOF
	    {
		    get
		    {
			    return NativeMethods.BNIsEndOfFile(this.handle);
		    }
	    }
	    
	
	    public bool ReadData(byte[] buffer)
	    {
		    return NativeMethods.BNReadData(
			    this.handle ,
			    buffer ,
			    (ulong)buffer.Length
			);
	    }

	    #region easy

	    public byte[]? ReadData(ulong length)
	    {
		    if (0 == length)
		    {
			    return null;
		    }
		    
		    byte[] buffer = new byte[length];
		    
		    bool ok = NativeMethods.BNReadData(
			    this.handle ,
			    buffer , 
			    (ulong)buffer.Length
		    );

		    if (!ok)
		    {
			    return null;
		    }

		    return buffer;
	    }
	    
	    public sbyte? ReadInt8()
	    {
		    bool ok = NativeMethods.BNRead8(this.handle, out byte slug);
		    
		    if (!ok)
		    {
			    return null;
		    }
		    
		    return (sbyte)slug;
	    }
	    
	    public short? ReadInt16()
	    {
		    bool ok = NativeMethods.BNRead16(this.handle, out ushort slug);

		    if (!ok)
		    {
			    return null;
		    }
		    
		    return (short)slug;
	    }
	    
	    public short? ReadInt16BE()
	    {
		    bool ok = NativeMethods.BNReadBE16(this.handle, out ushort slug);

		    if (!ok)
		    {
			    return null;
		    }
		    
		    return (short)slug;
	    }
	    
	    public short? ReadInt16LE()
	    {
		    bool ok = NativeMethods.BNReadLE16(this.handle, out ushort slug);

		    if (!ok)
		    {
			    return null;
		    }
		    
		    return (short)slug;
	    }
	    
	    public int? ReadInt32()
	    {
		    bool ok = NativeMethods.BNRead32(this.handle, out uint slug);

		    if (!ok)
		    {
			    return null;
		    }
		    
		    return (int)slug;
	    }
	    
	    public int? ReadInt32BE()
	    {
		    bool ok = NativeMethods.BNReadBE32(this.handle, out uint slug);

		    if (!ok)
		    {
			    return null;
		    }
		    
		    return (int)slug;
	    }
	    
	    public int? ReadInt32LE()
	    {
		    bool ok = NativeMethods.BNReadLE32(this.handle, out uint slug);

		    if (!ok)
		    {
			    return null;
		    }
		    
		    return (int)slug;
	    }
	    
	    public long? ReadInt64()
	    {
		    bool ok = NativeMethods.BNRead64(this.handle, out ulong slug);

		    if (!ok)
		    {
			    return null;
		    }
		    
		    return (long)slug;
	    }
	    
	    public long? ReadInt64BE()
	    {
		    bool ok = NativeMethods.BNReadBE64(this.handle, out ulong slug);

		    if (!ok)
		    {
			    return null;
		    }
		    
		    return (long)slug;
	    }
	    
	    public long? ReadInt64LE()
	    {
		    bool ok = NativeMethods.BNReadLE64(this.handle, out ulong slug);

		    if (!ok)
		    {
			    return null;
		    }
		
		    return (long)slug;
	    }
	    
	    #endregion

	    #region unsigned

	    
	    
	    public byte? ReadUInt8()
	    {
		    bool ok = NativeMethods.BNRead8(this.handle, out byte slug);
		    
		    if (!ok)
		    {
			    return null;
		    }
		    
		    return slug;
	    }
	    
	    
	    
	    public ushort? ReadUInt16()
	    {
		    bool ok = NativeMethods.BNRead16(this.handle, out ushort slug);

		    if (!ok)
		    {
			    return null;
		    }
		    
		    return (ushort)slug;
	    }
	    
	    public ushort? ReadUInt16BE()
	    {
		    bool ok = NativeMethods.BNReadBE16(this.handle, out ushort slug);

		    if (!ok)
		    {
			    return null;
		    }
		    
		    return (ushort)slug;
	    }
	    
	    public ushort? ReadUInt16LE()
	    {
		    bool ok = NativeMethods.BNReadLE16(this.handle, out ushort slug);

		    if (!ok)
		    {
			    return null;
		    }
		    
		    return (ushort)slug;
	    }
	    
	    
	    
	    
	    public uint? ReadUInt32()
	    {
		    bool ok = NativeMethods.BNRead32(this.handle, out uint slug);

		    if (!ok)
		    {
			    return null;
		    }
		    
		    return (uint)slug;
	    }
	    
	    public uint? ReadUInt32BE()
	    {
		    bool ok = NativeMethods.BNReadBE32(this.handle, out uint slug);

		    if (!ok)
		    {
			    return null;
		    }
		    
		    return (uint)slug;
	    }
	    
	    public uint? ReadUInt32LE()
	    {
		    bool ok = NativeMethods.BNReadLE32(this.handle, out uint slug);

		    if (!ok)
		    {
			    return null;
		    }
		    
		    return (uint)slug;
	    }
	    
	    public ulong? ReadUInt64()
	    {
		    bool ok = NativeMethods.BNRead64(this.handle, out ulong slug);

		    if (!ok)
		    {
			    return null;
		    }
		    
		    return (ulong)slug;
	    }
	    
	    public ulong? ReadUInt64BE()
	    {
		    bool ok = NativeMethods.BNReadBE64(this.handle, out ulong slug);

		    if (!ok)
		    {
			    return null;
		    }
		    
		    return (ulong)slug;
	    }
	    
	    public ulong? ReadUInt64LE()
	    {
		    bool ok = NativeMethods.BNReadLE64(this.handle, out ulong slug);

		    if (!ok)
		    {
			    return null;
		    }
		
		    return (ulong)slug;
	    }

	    #endregion
	    
	    // alias
	    public sbyte? ReadSByte()
	    {
		    return this.ReadInt8();
	    }
	    
	    public byte? ReadByte()
	    {
		    return this.ReadUInt8();
	    }

	    public short? ReadShort()
	    {
		    return this.ReadInt16();
	    }
	    
	    public ushort? ReadUShort()
	    {
		    return this.ReadUInt16();
	    }

	    public int? ReadInt()
	    {
		    return this.ReadInt32();
	    }
	    
	    public uint? ReadUInt()
	    {
		    return this.ReadUInt32();
	    }
	    
	    public long? ReadLong()
	    {
		    return this.ReadInt64();
	    }
	    
	    public ulong? ReadULong()
	    {
		    return this.ReadUInt64();
	    }
	    
	    
	    public sbyte? PeekInt8(ulong? offset = null)
	    {
		    ulong old = this.Position;

		    this.Position = offset ?? this.Position;
		    
		    sbyte? value = this.ReadInt8();

		    this.Position = old;

		    return value;
	    }
	    
	    public byte? PeekUInt8(ulong? offset = null)
	    {
		    ulong old = this.Position;

		    this.Position = offset ?? this.Position;
		    
		    byte? value = this.ReadUInt8();

		    this.Position = old;

		    return value;
	    }
	    
	    
	    public short? PeekInt16(ulong? offset = null)
	    {
		    ulong old = this.Position;

		    this.Position = offset ?? this.Position;
		    
		    short? value = this.ReadInt16();

		    this.Position = old;

		    return value;
	    }
	    
	    public short? PeekInt16BE(ulong? offset = null)
	    {
		    ulong old = this.Position;

		    this.Position = offset ?? this.Position;
		    
		    short? value = this.ReadInt16BE();

		    this.Position = old;

		    return value;
	    }
	    
	    public short? PeekInt16LE(ulong? offset = null)
	    {
		    ulong old = this.Position;

		    this.Position = offset ?? this.Position;
		    
		    short? value = this.ReadInt16LE();

		    this.Position = old;

		    return value;
	    }
	    
	    public ushort? PeekUInt16(ulong? offset = null)
	    {
		    ulong old = this.Position;

		    this.Position = offset ?? this.Position;
		    
		    ushort? value = this.ReadUInt16();

		    this.Position = old;

		    return value;
	    }
	    
	    public ushort? PeekUInt16BE(ulong? offset = null)
	    {
		    ulong old = this.Position;

		    this.Position = offset ?? this.Position;
		    
		    ushort? value = this.ReadUInt16BE();

		    this.Position = old;

		    return value;
	    }
	    
	    public ushort? PeekUInt16LE(ulong? offset = null)
	    {
		    ulong old = this.Position;

		    this.Position = offset ?? this.Position;
		    
		    ushort? value = this.ReadUInt16LE();

		    this.Position = old;

		    return value;
	    }
	    
	    //
	    public int? PeekInt32(ulong? offset = null)
	    {
		    ulong old = this.Position;

		    this.Position = offset ?? this.Position;
		    
		    int? value = this.ReadInt32();

		    this.Position = old;

		    return value;
	    }
	    
	    public int? PeekInt32BE(ulong? offset = null)
	    {
		    ulong old = this.Position;

		    this.Position = offset ?? this.Position;
		    
		    int? value = this.ReadInt32BE();

		    this.Position = old;

		    return value;
	    }
	    
	    public int? PeekInt32LE(ulong? offset = null)
	    {
		    ulong old = this.Position;

		    this.Position = offset ?? this.Position;
		    
		    int? value = this.ReadInt32LE();

		    this.Position = old;

		    return value;
	    }
	    
	    public uint? PeekUInt32(ulong? offset = null)
	    {
		    ulong old = this.Position;

		    this.Position = offset ?? this.Position;
		    
		    uint? value = this.ReadUInt32();

		    this.Position = old;

		    return value;
	    }
	    
	    public uint? PeekUInt32BE(ulong? offset = null)
	    {
		    ulong old = this.Position;

		    this.Position = offset ?? this.Position;
		    
		    uint? value = this.ReadUInt32BE();

		    this.Position = old;

		    return value;
	    }
	    
	    public uint? PeekUInt32LE(ulong? offset = null)
	    {
		    ulong old = this.Position;

		    this.Position = offset ?? this.Position;
		    
		    uint? value = this.ReadUInt32LE();

		    this.Position = old;

		    return value;
	    }
	    
	    
	    public long? PeekInt64(ulong? offset = null)
	    {
		    ulong old = this.Position;

		    this.Position = offset ?? this.Position;
		    
		    long? value = this.ReadInt64();

		    this.Position = old;

		    return value;
	    }
	    
	    public long? PeekInt64BE(ulong? offset = null)
	    {
		    ulong old = this.Position;

		    this.Position = offset ?? this.Position;
		    
		    long? value = this.ReadInt64BE();

		    this.Position = old;

		    return value;
	    }
	    
	    
	    public long? PeekInt64LE(ulong? offset = null)
	    {
		    ulong old = this.Position;

		    this.Position = offset ?? this.Position;
		    
		    long? value = this.ReadInt64LE();

		    this.Position = old;

		    return value;
	    }
	    
	    public ulong? PeekUInt64(ulong? offset = null)
	    {
		    ulong old = this.Position;

		    this.Position = offset ?? this.Position;
		    
		    ulong? value = this.ReadUInt64();

		    this.Position = old;

		    return value;
	    }
	    
	    public ulong? PeekUInt64BE(ulong? offset = null)
	    {
		    ulong old = this.Position;

		    this.Position = offset ?? this.Position;
		    
		    ulong? value = this.ReadUInt64BE();

		    this.Position = old;

		    return value;
	    }
	    
	    public ulong? PeekUInt64LE(ulong? offset = null)
	    {
		    ulong old = this.Position;

		    this.Position = offset ?? this.Position;
		    
		    ulong? value = this.ReadUInt64LE();

		    this.Position = old;

		    return value;
	    }
	    
	    
	    // alias peek
	    public sbyte? PeekSByte(ulong? offset = null)
	    {
		    return this.PeekInt8(offset);
	    }
	    
	    public byte? PeekByte(ulong? offset = null)
	    {
		    return this.PeekUInt8(offset);
	    }

	    public short? PeekShort(ulong? offset = null)
	    {
		    return this.PeekInt16(offset);
	    }
	    
	    public ushort? PeekUShort(ulong? offset = null)
	    {
		    return this.PeekUInt16(offset);
	    }

	    public int? PeekInt(ulong? offset = null)
	    {
		    return this.PeekInt32(offset);
	    }
	    
	    public uint? PeekUInt(ulong? offset = null)
	    {
		    return this.PeekUInt32(offset);
	    }
	    
	    public long? PeekLong(ulong? offset = null)
	    {
		    return this.PeekInt64(offset);
	    }
	    
	    public ulong? PeekULong(ulong? offset = null)
	    {
		    return this.PeekUInt64(offset);
	    }

	    public void SeekRelative(long offset)
	    {
		    NativeMethods.BNSeekBinaryReaderRelative(this.handle , offset);
	    }

	    /// <summary>
	    /// Reads a 64-bit unsigned integer from the current position.
	    /// Returns null if the read fails (e.g., end of data).
	    /// </summary>
	    /// <returns>The 64-bit unsigned integer value, or null on failure.</returns>
	    public ulong? Read64()
	    {
		    // Attempt to read a 64-bit value from the current stream position.
		    bool ok = NativeMethods.BNRead64(this.handle , out ulong result);

		    if (!ok)
		    {
			    return null;
		    }

		    return result;
	    }
	}
}