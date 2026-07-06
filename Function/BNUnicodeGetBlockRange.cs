using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNUnicodeGetBlockRange(const char* name, uint32_t* rangeStart, uint32_t* rangeEnd)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNUnicodeGetBlockRange"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNUnicodeGetBlockRange(
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// uint32_t* rangeStart
		    IntPtr rangeStart  , 
			
			// uint32_t* rangeEnd
		    IntPtr rangeEnd  
			
		);
	}
}