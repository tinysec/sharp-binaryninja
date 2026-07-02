using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNPerformSearch(const char* query, uint8_t* buffer, uint64_t size, void** callback, void* context)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNPerformSearch"
        )]
		internal static extern bool BNPerformSearch(
			
			// const char* query
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string query  , 
			
			// uint8_t* buffer
		    IntPtr buffer  , 
			
			// uint64_t size
		    ulong size  , 
			
			// void** callback
		    IntPtr callback  , 
			
			// void* context
		    IntPtr context  
		);
	}
}