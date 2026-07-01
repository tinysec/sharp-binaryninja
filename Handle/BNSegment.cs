using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class Segment : AbstractSafeHandle<Segment>
	{
		internal Segment(IntPtr handle , bool owner)
			: base(handle , owner)
	    {

	    }

        /// <summary>
        /// Creates a new Segment with the specified address range, data layout, and flags.
        /// </summary>
        /// <param name="start">The virtual start address of the segment.</param>
        /// <param name="length">The length of the segment in virtual address space.</param>
        /// <param name="dataOffset">The offset into the binary data backing this segment.</param>
        /// <param name="dataLength">The length of the data backing this segment.</param>
        /// <param name="flags">The segment flags (readable, writable, executable, etc.).</param>
        /// <param name="autoDefined">True if this segment was auto-defined by analysis.</param>
        /// <returns>A new owned Segment instance, or null on failure.</returns>
        public static Segment? Create(
            ulong start,
            ulong length,
            ulong dataOffset,
            ulong dataLength,
            SegmentFlag flags,
            bool autoDefined = false
        )
        {
            // Create the segment with the given parameters; the returned handle is owned.
            return Segment.TakeHandle(
                NativeMethods.BNCreateSegment(
                    start,
                    length,
                    dataOffset,
                    dataLength,
                    (uint)flags,
                    autoDefined
                )
            );
        }

		internal static Segment? NewFromHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new Segment(
				NativeMethods.BNNewSegmentReference(handle) ,
				true
			);
		}
	    
		internal static Segment MustNewFromHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new Segment(
				NativeMethods.BNNewSegmentReference(handle) ,
				true
			);
		}
	    
		internal static Segment? TakeHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new Segment(handle, true);
		}
	    
		internal static Segment MustTakeHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new Segment(handle, true);
		}
	    
		internal static Segment? BorrowHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new Segment(handle, false);
		}
	    
		internal static Segment MustBorrowHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new Segment(handle, false);
		}
		
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeSegment(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }
	    
	    public ulong Length
	    {
		    get
		    {
			    return NativeMethods.BNSegmentGetLength(this.handle);
		    }
	    }
	    
	    public ulong Start
	    {
		    get
		    {
			    return NativeMethods.BNSegmentGetStart(this.handle);
		    }
	    } 
	    
	    public ulong End
	    {
		    get
		    {
			    return NativeMethods.BNSegmentGetEnd(this.handle);
		    }
	    } 
	    
	    public ulong DataLength
	    {
		    get
		    {
			    return NativeMethods.BNSegmentGetDataLength(this.handle);
		    }
	    }
	    
	    public ulong DataOffset
	    {
		    get
		    {
			    return NativeMethods.BNSegmentGetDataOffset(this.handle);
		    }
	    }
	    
	    public ulong DataEnd
	    {
		    get
		    {
			    return NativeMethods.BNSegmentGetDataEnd(this.handle);
		    }
	    }
	    
	    public bool AutoDefined
	    {
		    get
		    {
			    return NativeMethods.BNSegmentIsAutoDefined(this.handle);
		    }
	    }
	    
	    public SegmentFlag Flags
	    {
		    get
		    {
			    return (SegmentFlag)NativeMethods.BNSegmentGetFlags(this.handle);
		    }
	    } 
	    
	    public bool Executable
	    {
		    get
		    {
			    return Utils.FlagOn<uint>( (uint)this.Flags, (uint)SegmentFlag.SegmentExecutable );
		    }
	    } 
	    
	    public bool Writable
	    {
		    get
		    {
			    return Utils.FlagOn<uint>((uint)this.Flags, (uint)SegmentFlag.SegmentWritable );
		    }
	    } 
	    
	    public bool Readable
	    {
		    get
		    {
			    return Utils.FlagOn<uint>((uint)this.Flags, (uint)SegmentFlag.SegmentReadable );
		    }
	    } 
	    
	    public bool ContainsData
	    {
		    get
		    {
			    return Utils.FlagOn<uint>((uint)this.Flags, (uint)SegmentFlag.SegmentContainsData );
		    }
	    } 
	    
	    public bool ContainsCode
	    {
		    get
		    {
			    return Utils.FlagOn<uint>((uint)this.Flags, (uint)SegmentFlag.SegmentContainsCode );
		    }
	    } 
	    
	    public bool DenyWrite
	    {
		    get
		    {
			    return Utils.FlagOn<uint>((uint)this.Flags, (uint)SegmentFlag.SegmentDenyWrite );
		    }
	    } 
	    
	    
	}
}