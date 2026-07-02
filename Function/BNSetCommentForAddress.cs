using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNSetCommentForAddress(BNFunction* func, uint64_t addr, const char* comment)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetCommentForAddress"
        )]
		internal static extern void BNSetCommentForAddress(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// uint64_t addr
		    ulong addr  , 
			
			// const char* comment
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string comment  
		);
	}
}