using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNAddDebugDataVariable(BNDebugInfo* debugInfo, uint64_t address, BNType* type, const char* name, const char** components, uint64_t components_count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAddDebugDataVariable"
        )]
		internal static extern bool BNAddDebugDataVariable(
			
			// BNDebugInfo* debugInfo
		    IntPtr debugInfo  , 
			
			// uint64_t address
		    ulong address  , 
			
			// BNType* type
		    IntPtr type  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// const char** components
		    string[] components  , 
			
			// uint64_t components_count
		    ulong components_count  
		);
	}
}