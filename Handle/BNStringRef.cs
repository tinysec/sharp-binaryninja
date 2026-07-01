using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class StringRef : AbstractSafeHandle<StringRef>
	{
	    internal StringRef(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }
	    
	    internal static StringRef? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new StringRef(handle, true);
	    }
	    
	    internal static StringRef MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new StringRef(handle, true);
	    }
	    
	    internal static StringRef? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new StringRef(handle, false);
	    }
	    
	    internal static StringRef MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new StringRef(handle, false);
	    }
	    
        /// <summary>
        /// Releases the native BNStringRef handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native string-ref handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeStringRef(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Gets the number of bytes stored in this string reference.
        /// This is the raw byte length, not the number of characters.
        /// </summary>
        public ulong Size
        {
            get
            {
                // Retrieve the byte length of the string contents from the native layer.
                return NativeMethods.BNGetStringRefSize(this.handle);
            }
        }

        /// <summary>
        /// Gets the string contents held by this string reference.
        /// Returns null if the native string is absent.
        /// </summary>
        public string? Value
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the string contents.
                IntPtr raw = NativeMethods.BNGetStringRefContents(this.handle);

                // 2. Copy and free the native string; returns null when the pointer is zero.
                return UnsafeUtils.TakeAnsiString(raw);
            }
        }

        /// <summary>
        /// Duplicates this string reference by creating a new reference to the same string data.
        /// The caller owns the returned reference and must dispose it when no longer needed.
        /// </summary>
        /// <returns>A new StringRef backed by the same string, or null if the handle is zero.</returns>
        public StringRef? Duplicate()
        {
            // Ask the native layer for a new reference to the same string data.
            return StringRef.TakeHandle(
                NativeMethods.BNDuplicateStringRef(this.handle)
            );
        }
    }
}