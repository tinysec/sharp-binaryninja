using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNProjectFileSetDescription(BNProjectFile* file, const char* description)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNProjectFileSetDescription"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNProjectFileSetDescription(
			
			// BNProjectFile* file
		    IntPtr file  , 
			
			// const char* description
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string description  
			
		);
	}
}