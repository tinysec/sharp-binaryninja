using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRemoteProjectCanUserAdmin(BNRemoteProject* project, const char* username)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteProjectCanUserAdmin"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNRemoteProjectCanUserAdmin(
			
			// BNRemoteProject* project
		    IntPtr project  , 
			
			// const char* username
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string username  
			
		);
	}
}