using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class Section : AbstractSafeHandle<Section>
	{
	    internal Section(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
		    
	    }

	    internal static Section? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Section(
			    NativeMethods.BNNewSectionReference(handle) ,
			    true
		    );
	    }
	    
	    internal static Section MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Section(
			    NativeMethods.BNNewSectionReference(handle) ,
			    true
		    );
	    }
	    
	    internal static Section? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Section(handle, true);
	    }
	    
	    internal static Section MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Section(handle, true);
	    }
	    
	    internal static Section? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Section(handle, false);
	    }
	    
	    internal static Section MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Section(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeSection(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }

	    public string Name
	    {
		    get
		    {
			    IntPtr raw = NativeMethods.BNSectionGetName(this.handle);
		
			    return UnsafeUtils.TakeAnsiString(raw);
		    }
	    }
	    
	    public string Type
	    {
		    get
		    {
			    IntPtr raw = NativeMethods.BNSectionGetType(this.handle);
			
			    return UnsafeUtils.TakeAnsiString(raw);
		    }
	    }
	    
	    public ulong Length
	    {
		    get
		    {
			    return NativeMethods.BNSectionGetLength(this.handle);
		    }
	    }
	    
	    public ulong Start
	    {
		    get
		    {
			    return NativeMethods.BNSectionGetStart(this.handle);
		    }
	    }
	    
	    public ulong End
	    {
		    get
		    {
			    return NativeMethods.BNSectionGetEnd(this.handle);
		    }
	    }
	    
	    public string LinkedSection
	    {
		    get
		    {
			    IntPtr raw = NativeMethods.BNSectionGetLinkedSection(this.handle);
			
			    return UnsafeUtils.TakeAnsiString(raw);
		    }
	    }
	    
	    public string InfoSection
	    {
		    get
		    {
			    IntPtr raw = NativeMethods.BNSectionGetInfoSection(this.handle);
		
			    return UnsafeUtils.TakeAnsiString(raw);
		    }
	    }
	    
	    public ulong InfoData
	    {
		    get
		    {
			    return NativeMethods.BNSectionGetInfoData(this.handle);;
		    }
	    }
	    
	    public ulong Align
	    {
		    get
		    {
			    return NativeMethods.BNSectionGetAlign(this.handle);;
		    }
	    }
	    
	    public ulong EntrySize
	    {
		    get
		    {
			    return NativeMethods.BNSectionGetEntrySize(this.handle);;
		    }
	    }
	    
	    public SectionSemantics Semantics
	    {
		    get
		    {
			    return NativeMethods.BNSectionGetSemantics(this.handle);;
		    }
	    }
	    
	    public bool AutoDefined
	    {
		    get
		    {
			    return NativeMethods.BNSectionIsAutoDefined(this.handle);;
		    }
	    }
	}
}