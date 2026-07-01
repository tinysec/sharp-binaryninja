using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class ExternalLibrary : AbstractSafeHandle<ExternalLibrary>
	{
	    internal ExternalLibrary(IntPtr handle , bool owner)
		    : base(handle , owner)
	    {

	    }

	    internal static ExternalLibrary? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }

		    return new ExternalLibrary(
			    NativeMethods.BNNewExternalLibraryReference(handle) ,
			    true
		    );
	    }

	    internal static ExternalLibrary MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }

		    return new ExternalLibrary(
			    NativeMethods.BNNewExternalLibraryReference(handle) ,
			    true
		    );
	    }

	    internal static ExternalLibrary? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }

		    return new ExternalLibrary(handle, true);
	    }

	    internal static ExternalLibrary MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }

		    return new ExternalLibrary(handle, true);
	    }

	    internal static ExternalLibrary? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }

		    return new ExternalLibrary(handle, false);
	    }

	    internal static ExternalLibrary MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }

		    return new ExternalLibrary(handle, false);
	    }

	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeExternalLibrary(this.handle);
	            this.SetHandleAsInvalid();
	        }

	        return true;
	    }

	    public string Name
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNExternalLibraryGetName(this.handle)
			    );
		    }
	    }

	    /// <summary>
	    /// The backing project file for this external library.
	    /// </summary>
	    public ProjectFile? BackingFile
	    {
		    get
		    {
			    return ProjectFile.TakeHandle(
				    NativeMethods.BNExternalLibraryGetBackingFile(this.handle)
			    );
		    }

		    set
		    {
			    NativeMethods.BNExternalLibrarySetBackingFile(
				    this.handle ,
				    null == value ? IntPtr.Zero : value.DangerousGetHandle()
			    );
		    }
	    }
	}
}
