using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNSetMemoryRegionDisplayName(BNBinaryView* view, const char* name, const char* displayName)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetMemoryRegionDisplayName"
        )]
		internal static extern bool BNSetMemoryRegionDisplayName(
			
			// BNBinaryView* view
		    IntPtr view   , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name   , 
			
			// const char* displayName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string displayName  
		);
	}
}
