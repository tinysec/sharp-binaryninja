using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNProjectFolderExport(BNProjectFolder* folder, const char* destination, void* ctxt, void** progress)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNProjectFolderExport"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNProjectFolderExport(
			
			// BNProjectFolder* folder
		    IntPtr folder  , 
			
			// const char* destination
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string destination  , 
			
			// void* ctxt
		    IntPtr ctxt  , 
			
			// void** progress
		    IntPtr progress  
			
		);
	}
}