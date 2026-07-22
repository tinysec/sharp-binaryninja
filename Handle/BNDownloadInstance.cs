using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed partial class DownloadInstance : AbstractSafeHandle<DownloadInstance>
	{
	    internal DownloadInstance(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }
	    
	    internal static DownloadInstance? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new DownloadInstance(
			    NativeMethods.BNNewDownloadInstanceReference(handle) ,
			    true
		    );
	    }
	    
	    internal static DownloadInstance MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new DownloadInstance(
			    NativeMethods.BNNewDownloadInstanceReference(handle) ,
			    true
		    );
	    }
	    
	    internal static DownloadInstance? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new DownloadInstance(handle, true);
	    }
	    
	    internal static DownloadInstance MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new DownloadInstance(handle, true);
	    }
	    
	    internal static DownloadInstance? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new DownloadInstance(handle, false);
	    }
	    
	    internal static DownloadInstance MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new DownloadInstance(handle, false);
	    }
	    
        /// <summary>
        /// Releases the native BNDownloadInstance handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native download instance and mark it invalid to prevent double-free.
                NativeMethods.BNFreeDownloadInstance(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Gets or sets the last error message recorded by this download instance.
        /// Returns an empty string if no error has occurred.
        /// </summary>
        public string Error
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the error message.
                IntPtr raw = NativeMethods.BNGetErrorForDownloadInstance(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }

            set
            {
                // Forward the error string to the native setter.
                NativeMethods.BNSetErrorForDownloadInstance(
                    this.handle,
                    value ?? string.Empty
                );
            }
        }

        /// <summary>
        /// Notifies the download instance of progress toward completing a transfer.
        /// </summary>
        /// <param name="progress">The number of bytes transferred so far.</param>
        /// <param name="total">The total number of bytes expected; zero if unknown.</param>
        /// <returns>True if the transfer should continue; false if it has been cancelled.</returns>
        public bool NotifyProgress(ulong progress, ulong total)
        {
            // Delegate to the native progress notification API.
            return NativeMethods.BNNotifyProgressForDownloadInstance(this.handle, progress, total);
        }
    }
}
