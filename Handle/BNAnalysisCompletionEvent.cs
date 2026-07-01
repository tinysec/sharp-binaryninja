using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class AnalysisCompletionEvent : AbstractSafeHandle<AnalysisCompletionEvent>
	{
	    internal AnalysisCompletionEvent(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }
	    
	    internal static AnalysisCompletionEvent? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new AnalysisCompletionEvent(
			    NativeMethods.BNNewAnalysisCompletionEventReference(handle) ,
			    true
		    );
	    }
	    
	    internal static AnalysisCompletionEvent MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new AnalysisCompletionEvent(
			    NativeMethods.BNNewAnalysisCompletionEventReference(handle) ,
			    true
		    );
	    }
	    
	    internal static AnalysisCompletionEvent? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new AnalysisCompletionEvent(handle, true);
	    }
	    
	    internal static AnalysisCompletionEvent MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new AnalysisCompletionEvent(handle, true);
	    }
	    
	    internal static AnalysisCompletionEvent? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new AnalysisCompletionEvent(handle, false);
	    }
	    
	    internal static AnalysisCompletionEvent MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new AnalysisCompletionEvent(handle, false);
	    }

        /// <summary>
        /// Releases the native BNAnalysisCompletionEvent handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native event handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeAnalysisCompletionEvent(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Cancels this analysis completion event, preventing its callback from firing.
        /// Call this to unregister the event before the analysis completes if the callback
        /// is no longer needed.
        /// </summary>
        public void Cancel()
        {
            // Instruct the native layer to remove this event from the completion callback list.
            NativeMethods.BNCancelAnalysisCompletionEvent(this.handle);
        }
    }
}