using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// uint64_t BNFuzzyMatchSingle(const char* target, const char* query)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNFuzzyMatchSingle"
        )]
		internal static extern ulong BNFuzzyMatchSingle(
			
			// const char* target
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string target  , 
			
			// const char* query
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string query  
			
		);
	}
}