using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class MainThreadAction : AbstractSafeHandle<MainThreadAction>
	{
	    internal MainThreadAction(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }

	    internal static MainThreadAction? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new MainThreadAction(
			    NativeMethods.BNNewMainThreadActionReference(handle) ,
			    true
		    );
	    }
	    
	    internal static MainThreadAction MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new MainThreadAction(
			    NativeMethods.BNNewMainThreadActionReference(handle) ,
			    true
		    );
	    }
	    
	    internal static MainThreadAction? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new MainThreadAction(handle, true);
	    }
	    
	    internal static MainThreadAction MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new MainThreadAction(handle, true);
	    }
	    
	    internal static MainThreadAction? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new MainThreadAction(handle, false);
	    }
	    
	    internal static MainThreadAction MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new MainThreadAction(handle, false);
	    }
	    
        /// <summary>
        /// Releases the native BNMainThreadAction handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native main-thread action handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeMainThreadAction(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Gets whether this main-thread action has finished executing.
        /// Poll this property to determine when it is safe to inspect the result.
        /// </summary>
        public bool IsDone
        {
            get
            {
                // Query the native layer for the completion flag on this action.
                return NativeMethods.BNIsMainThreadActionDone(this.handle);
            }
        }

        /// <summary>
        /// Executes this action immediately on the calling thread.
        /// Use this from the main thread to run the action synchronously without
        /// dispatching it through the Binary Ninja main-thread queue.
        /// </summary>
        public void Execute()
        {
            // Invoke the native action callback directly on the current thread.
            NativeMethods.BNExecuteMainThreadAction(this.handle);
        }

        /// <summary>
        /// Blocks the calling thread until this action has finished executing on the main thread.
        /// Returns immediately if the action is already done.
        /// </summary>
        public void WaitForCompletion()
        {
            // Suspend the caller until the native action signals completion.
            NativeMethods.BNWaitForMainThreadAction(this.handle);
        }

		/// <summary>Blocks until this action has finished executing.</summary>
		public void Wait()
		{
			this.WaitForCompletion();
		}
    }
}
