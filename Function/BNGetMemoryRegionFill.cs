using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// uint8_t BNGetMemoryRegionFill(BNBinaryView* view, const char* name)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetMemoryRegionFill"
        )]
		internal static extern byte BNGetMemoryRegionFill(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  
			
		);
	}
}