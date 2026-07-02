using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNProjectFileExport(BNProjectFile* file, const char* destination)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNProjectFileExport"
        )]
		internal static extern bool BNProjectFileExport(
			
			// BNProjectFile* file
		    IntPtr file  , 
			
			// const char* destination
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string destination  
			
		);
	}
}