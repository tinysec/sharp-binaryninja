using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class DataBuffer : AbstractSafeHandle<DataBuffer> , IEnumerable<byte>
	{
		public DataBuffer()
			:this( Array.Empty<byte>() )
		{
			
		}
		
		public DataBuffer(byte[] data)
			:this( NativeMethods.BNCreateDataBuffer(data , (ulong)data.Length)  , true)
		{
			
		}
		
	    internal DataBuffer(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	       
	    }
	    
	    internal static DataBuffer MustNewFromHandle(IntPtr handle)
	    {
		    return new DataBuffer(
			    NativeMethods.BNDuplicateDataBuffer(handle) ,
			    true
		    );
	    }
	    
	    internal static DataBuffer? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new DataBuffer(handle, true);
	    }
		
	    internal static DataBuffer MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new DataBuffer(handle, true);
	    }
	    
	    internal static DataBuffer? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }

		    return new DataBuffer(handle , false);
	    }

	    internal static DataBuffer MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }

		    return new DataBuffer(handle , false);
	    }
	    
	    public static DataBuffer FromBytes(byte[] data)
	    {
		    return DataBuffer.MustTakeHandle(
			    NativeMethods.BNCreateDataBuffer(data , (ulong)data.Length)
		    );
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeDataBuffer(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }
	    
	    public byte this[int index]
	    {
		    get
		    {
			    return NativeMethods.BNGetDataBufferByte(this.handle, (ulong)index);
		    }

		    set
		    {
			    NativeMethods.BNSetDataBufferByte(this.handle , (ulong)index, value);
		    }
	    }
	    
	    public IEnumerator<byte> GetEnumerator()
	    {
		    List<byte> data = new List<byte>();
		    
		    data.AddRange(this.Contents);
		    
		    return data.GetEnumerator();
	    }
	    
	    IEnumerator IEnumerable.GetEnumerator()
	    {
		    return this.GetEnumerator();
	    }
	    
	    public byte[] Contents
	    {
		    get
		    {
			    IntPtr raw = NativeMethods.BNGetDataBufferContents(this.handle);

			    if (IntPtr.Zero == raw)
			    {
				    return Array.Empty<byte>();
			    }
			    
			    return UnsafeUtils.ReadNumberArray<byte>(raw , this.Length);
		    }

		    set
		    {
			    NativeMethods.BNSetDataBufferContents(this.handle, value, (ulong)value.Length);
		    }
	    }
	    
	    public ulong Length
	    {
		    get
		    {
			    return NativeMethods.BNGetDataBufferLength(this.handle);
		    }

		    set
		    {
			    NativeMethods.BNSetDataBufferLength( this.handle , value);
		    }
	    }
	    
	    public byte[] GetContentsAt(ulong offset)
	    {
		    IntPtr raw = NativeMethods.BNGetDataBufferContentsAt(this.handle , offset);

		    if (IntPtr.Zero == raw)
		    {
			    return Array.Empty<byte>();
		    }
		    
		    return UnsafeUtils.ReadNumberArray<byte>(raw , this.Length - offset);
	    }
	    
	    public void Clear()
	    {
		    NativeMethods.BNClearDataBuffer(this.handle);
	    }

	    /// <summary>
	    /// Convert the data buffer contents to a base64-encoded string.
	    /// </summary>
	    public string ToBase64()
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNDataBufferToBase64(this.handle)
		    );
	    }

	    /// <summary>
	    /// Convert the data buffer contents to an escaped string representation.
	    /// </summary>
	    public string ToEscapedString(bool nullTerminates = false , bool escapePrintable = false)
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNDataBufferToEscapedString(this.handle , nullTerminates , escapePrintable)
		    );
	    }

	    public DataBuffer GetSlice(ulong start , ulong length)
	    {
		    return DataBuffer.MustTakeHandle(
			    NativeMethods.BNGetDataBufferSlice(this.handle , start , length)
		    );
	    }

	    /// <summary>
	    /// Append the contents of another data buffer to this buffer.
	    /// </summary>
	    public void Append(DataBuffer other)
	    {
		    NativeMethods.BNAppendDataBuffer(this.handle , other.DangerousGetHandle());
	    }

	    /// <summary>
	    /// Append raw byte data to this buffer.
	    /// </summary>
	    public void AppendBytes(byte[] data)
	    {
		    NativeMethods.BNAppendDataBufferContents(this.handle , data , (ulong)data.Length);
	    }

	    /// <summary>
	    /// Replace this buffer's contents with the contents of another data buffer.
	    /// </summary>
	    public void Assign(DataBuffer other)
	    {
		    NativeMethods.BNAssignDataBuffer(this.handle , other.DangerousGetHandle());
	    }
	}
}