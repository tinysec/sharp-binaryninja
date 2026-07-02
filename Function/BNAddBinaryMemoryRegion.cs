using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNAddBinaryMemoryRegion(BNBinaryView* view, const char* name, uint64_t start, BNBinaryView* data, uint32_t flags)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAddBinaryMemoryRegion"
        )]
		internal static extern bool BNAddBinaryMemoryRegion(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// uint64_t start
		    ulong start  , 
			
			// BNBinaryView* data
		    IntPtr data  , 
			
			// uint32_t flags
		    uint flags  
		);
	}
}