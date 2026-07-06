using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsDisassemblySettingsOptionSet(BNDisassemblySettings* settings, BNDisassemblyOption option)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsDisassemblySettingsOptionSet"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsDisassemblySettingsOptionSet(
			
			// BNDisassemblySettings* settings
		    IntPtr settings  , 
			
			// BNDisassemblyOption option
		    DisassemblyOption option  
		);
	}
}