using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class AnalysisCompletionEvent : AbstractSafeHandle<AnalysisCompletionEvent>
	{
		// Holds the NATIVE wrapper delegate (not the user Action): the wrapper owns the GC root for
		// the user callback via its context, and its function pointer is what the core invokes when
		// analysis completes. Rooting the wrapper for the event's lifetime keeps the callback
		// reachable; the caller must in turn keep this AnalysisCompletionEvent alive (dispose or
		// GC-collecting it cancels the event and frees the delegate).
		private NativeDelegates.AnalysisCompletionEventCallbackDelegate? m_callback = null;

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
		/// Registers a callback that fires once when the next analysis pass on <paramref name="view"/>
		/// completes, mirroring Python <c>BinaryView.add_analysis_completion_event</c>
		/// (binaryview.py:8068). The returned event must be kept alive by the caller (held in a field
		/// or variable) for as long as the callback should remain armed; disposing or GC-collecting it
		/// cancels the event.
		/// </summary>
		/// <param name="view">The view whose analysis completion triggers the callback.</param>
		/// <param name="callback">The action invoked when analysis completes.</param>
		/// <returns>The armed event, or <c>null</c> if the core refused to create it.</returns>
		internal static AnalysisCompletionEvent? Create(BinaryView view, Action callback)
		{
			// 1. Wrap the user Action into the native void(void*) callback shape and take a function
			// pointer from the wrapper.
			NativeDelegates.AnalysisCompletionEventCallbackDelegate wrapper =
				UnsafeUtils.WrapAnalysisCompletionEventCallback(callback);

			IntPtr callbackPtr =
				Marshal.GetFunctionPointerForDelegate<NativeDelegates.AnalysisCompletionEventCallbackDelegate>(
					wrapper);

			// 2. Register with the core. The context pointer is unused (the wrapper captures the
			// callback), so pass null.
			IntPtr handle = NativeMethods.BNAddAnalysisCompletionEvent(
				view.DangerousGetHandle(),
				IntPtr.Zero,
				callbackPtr
			);

			// 3. Take ownership of the returned handle and root the wrapper on the instance so the
			// function pointer stays valid for the event's lifetime (see m_callback).
			AnalysisCompletionEvent? analysisCompletionEvent = AnalysisCompletionEvent.TakeHandle(handle);

			if (null != analysisCompletionEvent)
			{
				analysisCompletionEvent.m_callback = wrapper;
			}

			return analysisCompletionEvent;
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