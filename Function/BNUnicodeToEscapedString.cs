using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// const char* BNUnicodeToEscapedString(uint32_t** starts, uint32_t** ends, uint64_t* blockListCounts, uint64_t blockCount, bool utf8Enabled, void* data, uint64_t dataLen)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNUnicodeToEscapedString"
        )]
		internal static extern IntPtr BNUnicodeToEscapedString(
			
			// uint32_t** starts
		    IntPtr starts  , 
			
			// uint32_t** ends
		    IntPtr ends  , 
			
			// uint64_t* blockListCounts
		    IntPtr blockListCounts  , 
			
			// uint64_t blockCount
		    ulong blockCount  , 
			
			// bool utf8Enabled
		    [MarshalAs(UnmanagedType.I1)] bool utf8Enabled,
			
			// void* data
		    IntPtr data  , 
			
			// uint64_t dataLen
		    ulong dataLen  
			
		);
	}
}
