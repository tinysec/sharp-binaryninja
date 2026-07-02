using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRemoteFileSetName(BNRemoteFile* file, const char* name)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteFileSetName"
        )]
		internal static extern bool BNRemoteFileSetName(
			
			// BNRemoteFile* file
		    IntPtr file  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  
			
		);
	}
}