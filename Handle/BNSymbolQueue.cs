using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class SymbolQueue : AbstractSafeHandle<SymbolQueue>
	{
		public SymbolQueue() 
			: this( NativeMethods.BNCreateSymbolQueue() , true)
		{
			
		}
		
	    internal SymbolQueue(IntPtr handle , bool owner)
		    : base(handle , owner)
	    {
	        
	    }
	    
	    internal static SymbolQueue? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new SymbolQueue(handle, true);
	    }
	    
	    internal static SymbolQueue MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new SymbolQueue(handle, true);
	    }
	    
	    internal static SymbolQueue? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new SymbolQueue(handle, false);
	    }
	    
	    internal static SymbolQueue MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new SymbolQueue(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNDestroySymbolQueue(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }
	}
}