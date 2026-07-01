using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class DisassemblySettings : AbstractSafeHandle<DisassemblySettings>
	{
		public DisassemblySettings() 
			: this( NativeMethods.BNDefaultDisassemblySettings() , true)
		{
			
		}
		
	    internal DisassemblySettings(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	       
	    }

	   
	    internal static DisassemblySettings? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new DisassemblySettings(
			    NativeMethods.BNNewDisassemblySettingsReference(handle) ,
			    true
		    );
	    }
	    
	    internal static DisassemblySettings MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new DisassemblySettings(
			    NativeMethods.BNNewDisassemblySettingsReference(handle) ,
			    true
		    );
	    }
	    
	    internal static DisassemblySettings? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new DisassemblySettings(handle, true);
	    }
	    
	    internal static DisassemblySettings MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new DisassemblySettings(handle, true);
	    }
	    
	    internal static DisassemblySettings? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new DisassemblySettings(handle, false);
	    }
	    
	    internal static DisassemblySettings MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new DisassemblySettings(handle, false);
	    }
	    
	    public static DisassemblySettings Default()
	    {
		    return DisassemblySettings.MustTakeHandle(
			    NativeMethods.BNDefaultDisassemblySettings()
		    );
	    }
	    
	    public static DisassemblySettings DefaultGraph()
	    {
		    return DisassemblySettings.MustTakeHandle(
			    NativeMethods.BNDefaultGraphDisassemblySettings()
		    );
	    }
	    
	    public static DisassemblySettings DefaultLinear()
	    {
		    return DisassemblySettings.MustTakeHandle(
			    NativeMethods.BNDefaultLinearDisassemblySettings()
		    );
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeDisassemblySettings(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }

	    public bool IsOptionSet(DisassemblyOption option)
	    {
		    return NativeMethods.BNIsDisassemblySettingsOptionSet(this.handle , option);
	    }
	    
	    public void SetOption(DisassemblyOption option , bool state)
	    {
		    NativeMethods.BNSetDisassemblySettingsOption(this.handle , option , state);
	    }

	    public ulong Width
	    {
		    get
		    {
			    return NativeMethods.BNGetDisassemblyWidth(this.handle);
		    }

		    set
		    {
			    NativeMethods.BNSetDisassemblyWidth(this.handle, value);
		    }
	    }
	    
	    public ulong MaximumSymbolWidth
	    {
		    get
		    {
			    return NativeMethods.BNGetDisassemblyMaximumSymbolWidth(this.handle);
		    }

		    set
		    {
			    NativeMethods.BNSetDisassemblyMaximumSymbolWidth(this.handle, value);
		    }
	    }
	    
	    public ulong GutterWidth
	    {
		    get
		    {
			    return NativeMethods.BNGetDisassemblyGutterWidth(this.handle);
		    }

		    set
		    {
			    NativeMethods.BNSetDisassemblyGutterWidth(this.handle, value);
		    }
	    }
	    
	    
	    public DisassemblyAddressMode AddressMode
	    {
		    get
		    {
			    return NativeMethods.BNGetDisassemblyAddressMode(this.handle);
		    }

		    set
		    {
			    NativeMethods.BNSetDisassemblyAddressMode(this.handle, value);
		    }
	    }
	    
	    public ulong AddressBaseOffset
	    {
		    get
		    {
			    return NativeMethods.BNGetDisassemblyAddressBaseOffset(this.handle);
		    }

		    set
		    {
			    NativeMethods.BNSetDisassemblyAddressBaseOffset(this.handle, value);
		    }
	    }

	    /// <summary>
	    /// Gets or sets the block label display mode for disassembly output.
	    /// </summary>
	    public DisassemblyBlockLabels BlockLabels
	    {
		    get
		    {
			    return NativeMethods.BNGetDisassemblyBlockLabels(this.handle);
		    }

		    set
		    {
			    NativeMethods.BNSetDisassemblyBlockLabels(this.handle , value);
		    }
	    }

	    /// <summary>
	    /// Gets or sets the call parameter hint display mode for disassembly output.
	    /// </summary>
	    public DisassemblyCallParameterHints CallParameterHints
	    {
		    get
		    {
			    return NativeMethods.BNGetDisassemblyCallParameterHints(this.handle);
		    }

		    set
		    {
			    NativeMethods.BNSetDisassemblyCallParameterHints(this.handle , value);
		    }
	    }

	    /// <summary>
	    /// Creates a new empty disassembly settings instance.
	    /// </summary>
	    /// <returns>A new DisassemblySettings instance.</returns>
	    public static DisassemblySettings Create()
	    {
		    return DisassemblySettings.MustTakeHandle(
			    NativeMethods.BNCreateDisassemblySettings()
		    );
	    }

	    /// <summary>
	    /// Creates a deep copy of this disassembly settings instance.
	    /// </summary>
	    /// <returns>A new DisassemblySettings instance with the same configuration.</returns>
	    public DisassemblySettings Duplicate()
	    {
		    return DisassemblySettings.MustTakeHandle(
			    NativeMethods.BNDuplicateDisassemblySettings(this.handle)
		    );
	    }

	}
}