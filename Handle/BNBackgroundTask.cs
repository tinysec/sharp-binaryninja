using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class BackgroundTask : AbstractSafeHandle<BackgroundTask>
	{
		public BackgroundTask(string initialText , bool canCancel) 
			: this( NativeMethods.BNBeginBackgroundTask(initialText, canCancel ) , true )
		{
			
		}
		
		internal BackgroundTask(IntPtr handle , bool owner) 
			: base(handle , owner)
	    {
	       
	    }
		
	    internal static BackgroundTask? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new BackgroundTask(
			    NativeMethods.BNNewBackgroundTaskReference(handle) ,
			    true
		    );
	    }
	    
	    internal static BackgroundTask MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new BackgroundTask(
			    NativeMethods.BNNewBackgroundTaskReference(handle) ,
			    true
		    );
	    }
	    
	    internal static BackgroundTask? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new BackgroundTask(handle, true);
	    }
	    
	    internal static BackgroundTask MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new BackgroundTask(handle, true);
	    }
	    
	    internal static BackgroundTask? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new BackgroundTask(handle, false);
	    }
	    
	    internal static BackgroundTask MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new BackgroundTask(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeBackgroundTask(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }

	    public static BackgroundTask[] GetRunningTasks()
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetRunningBackgroundTasks(
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeHandleArrayEx<BackgroundTask>(
			    arrayPointer ,
			    arrayLength,
			    BackgroundTask.MustNewFromHandle,
				NativeMethods.BNFreeBackgroundTaskList
		    );
	    }
	    
	    public bool CanCancel
	    {
		    get
		    {
			    return NativeMethods.BNCanCancelBackgroundTask(this.handle);
		    }
	    }

	    public bool Cancelled
	    {
		    get
		    {
			    return NativeMethods.BNIsBackgroundTaskCancelled(this.handle);
		    }
	    }

	    public bool Finished
	    {
		    get
		    {
			    return NativeMethods.BNIsBackgroundTaskFinished(this.handle);
		    }
	    }

	    public string ProgressText
	    {
		    get
		    {
			    return UnsafeUtils.TakeUtf8String(
				    NativeMethods.BNGetBackgroundTaskProgressText(this.handle)
			    );
		    }

		    set
		    {
			    NativeMethods.BNSetBackgroundTaskProgressText(this.handle, value);
		    }
	    }

	    public ulong RuntimeSeconds
	    {
		    get
		    {
			    return NativeMethods.BNGetBackgroundTaskRuntimeSeconds(this.handle);
		    }
	    }

	    public void Cancel()
	    {
		    NativeMethods.BNCancelBackgroundTask(this.handle);
	    }

	    public void Finish()
	    {
		    NativeMethods.BNFinishBackgroundTask(this.handle);
	    }
	}
}