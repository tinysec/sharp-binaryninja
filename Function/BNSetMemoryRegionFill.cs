using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSetMemoryRegionFill(BNBinaryView* view, const char* name, uint8_t fill)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetMemoryRegionFill"
        )]
		internal static extern bool BNSetMemoryRegionFill(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// uint8_t fill
		    byte fill  
		);
	}
}