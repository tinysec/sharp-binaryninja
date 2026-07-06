using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCreateDatabaseWithProgress(BNBinaryView* data, const char* path, void* ctxt, void** progress, BNSaveSettings* settings)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateDatabaseWithProgress"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCreateDatabaseWithProgress(
			
			// BNBinaryView* data
		    IntPtr data  , 
			
			// const char* path
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string path  , 
			
			// void* ctxt
		    IntPtr ctxt  , 
			
			// void* progress
		    IntPtr progress  , 
			
			// BNSaveSettings* settings
		    IntPtr settings  
		);
	}
}