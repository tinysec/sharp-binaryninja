using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNBinaryView* BNOpenExistingDatabaseWithProgress(BNFileMetadata* file, const char* path, void* ctxt, void** progress)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNOpenExistingDatabaseWithProgress"
        )]
		internal static extern IntPtr BNOpenExistingDatabaseWithProgress(
			
			// BNFileMetadata* file
		    IntPtr file  , 
			
			// const char* path
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string path  , 
			
			// void* ctxt
		    IntPtr ctxt  , 
			
			// void* progress
		    IntPtr progress  
		);
	}
}