using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class TemporaryFile : AbstractSafeHandle<TemporaryFile>
	{
		public TemporaryFile() 
			: this( NativeMethods.BNCreateTemporaryFile() , true)
		{
			
		}
		
		public TemporaryFile(byte[] data) 
			: this(new DataBuffer(data))
		{
			
		}
		
		public TemporaryFile(DataBuffer buffer) 
			: this( NativeMethods.BNCreateTemporaryFileWithContents(buffer.DangerousGetHandle()) , true)
		{
			
		}
		
	    internal TemporaryFile(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }

	    internal static TemporaryFile? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new TemporaryFile(
			    NativeMethods.BNNewTemporaryFileReference(handle) ,
			    true
		    );
	    }
	    
	    internal static TemporaryFile MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new TemporaryFile(
			    NativeMethods.BNNewTemporaryFileReference(handle) ,
			    true
		    );
	    }
	    
	    internal static TemporaryFile? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new TemporaryFile(handle, true);
	    }
	    
	    internal static TemporaryFile MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new TemporaryFile(handle, true);
	    }
	    
	    internal static TemporaryFile? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new TemporaryFile(handle, false);
	    }
	    
	    internal static TemporaryFile MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new TemporaryFile(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeTemporaryFile(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }

	    public string FilePath
	    {
		    get
		    {
			    return UnsafeUtils.TakeUtf8String(
				    NativeMethods.BNGetTemporaryFilePath(this.handle)
			    );
		    }
	    }
	    
	    public DataBuffer FileContents
	    {
		    get
		    {
			    return DataBuffer.MustTakeHandle(
				    NativeMethods.BNGetTemporaryFileContents(this.handle)
			    );
		    }
	    }
	}
}