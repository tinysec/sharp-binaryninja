using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNAddDebugType(BNDebugInfo* debugInfo, const char* name, BNType* type, const char** components, uint64_t components_count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAddDebugType"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNAddDebugType(
			
			// BNDebugInfo* debugInfo
		    IntPtr debugInfo  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// BNType* type
		    IntPtr type  , 
			
			// const char** components: caller-built UTF-8 char** block (string[]
			// elements cannot carry LPUTF8Str, so the wrapper builds the block).
		    IntPtr components  ,
			
			// uint64_t components_count
		    ulong components_count  
		);
	}
}