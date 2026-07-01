using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class SaveSettings : AbstractSafeHandle<SaveSettings>
	{
		public SaveSettings() 
			: this( NativeMethods.BNCreateSaveSettings() , true)
		{
			
		}
		
	    internal SaveSettings(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }
	    
	    internal static SaveSettings? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new SaveSettings(
			    NativeMethods.BNNewSaveSettingsReference(handle) ,
			    true
		    );
	    }
	    
	    internal static SaveSettings MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new SaveSettings(
			    NativeMethods.BNNewSaveSettingsReference(handle) ,
			    true
		    );
	    }
	    
	    internal static SaveSettings? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new SaveSettings(handle, true);
	    }
	    
	    internal static SaveSettings MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new SaveSettings(handle, true);
	    }
	    
	    internal static SaveSettings? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new SaveSettings(handle, false);
	    }
	    
	    internal static SaveSettings MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new SaveSettings(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeSaveSettings(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }

	    public string Name
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNGetSaveSettingsName(this.handle)
			    );
		    }

		    set
		    {
			    NativeMethods.BNSetSaveSettingsName(this.handle, value);
		    }
	    }

	    public bool IsOptionSet(SaveOption option)
	    {
		    return NativeMethods.BNIsSaveSettingsOptionSet(this.handle, option);
	    }

	    public void SetOption(SaveOption option , bool value)
	    {
		    NativeMethods.BNSetSaveSettingsOption(this.handle, option, value);
	    }
	}
}