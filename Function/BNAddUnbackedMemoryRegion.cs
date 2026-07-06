using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNAddUnbackedMemoryRegion(BNBinaryView* view, const char* name, uint64_t start, uint64_t length, uint32_t flags, uint8_t fill)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAddUnbackedMemoryRegion"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNAddUnbackedMemoryRegion(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// uint64_t start
		    ulong start  , 
			
			// uint64_t length
		    ulong length  , 
			
			// uint32_t flags
		    uint flags  , 
			
			// uint8_t fill
		    byte fill  
		);
	}
}