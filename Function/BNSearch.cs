using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSearch(BNBinaryView* view, const char* query, void* context, void** progressCallback, void* matchContext, void** callback)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSearch"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNSearch(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* query
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string query  , 
			
			// void* context
		    IntPtr context  , 
			
			// void* progressCallback
		    IntPtr progressCallback  , 
			
			// void* matchContext
		    IntPtr matchContext  , 
			
			// void* callback
		    IntPtr callback  
		);
	}
}