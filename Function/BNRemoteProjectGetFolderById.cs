using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNRemoteFolder* BNRemoteProjectGetFolderById(BNRemoteProject* project, const char* id)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteProjectGetFolderById"
        )]
		internal static extern IntPtr BNRemoteProjectGetFolderById(
			
			// BNRemoteProject* project
		    IntPtr project  , 
			
			// const char* id
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string id  
			
		);
	}
}