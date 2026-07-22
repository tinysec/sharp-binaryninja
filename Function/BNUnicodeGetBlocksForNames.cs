using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNUnicodeGetBlocksForNames(const char** names, uint64_t nameCount, uint32_t*** starts, uint32_t*** ends, uint64_t** blockListCounts, uint64_t* blockCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNUnicodeGetBlocksForNames"
        )]
		internal static extern void BNUnicodeGetBlocksForNames(
			
			// const char** names
		    IntPtr names,
			
			// uint64_t nameCount
		    ulong nameCount  , 
			
			// uint32_t*** starts
		    IntPtr starts  , 
			
			// uint32_t*** ends
		    IntPtr ends  , 
			
			// uint64_t** blockListCounts
		    IntPtr blockListCounts  , 
			
			// uint64_t* blockCount
		    IntPtr blockCount  
			
		);
	}
}
