using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRemoteProjectCanUserEdit(BNRemoteProject* project, const char* username)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteProjectCanUserEdit"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNRemoteProjectCanUserEdit(
			
			// BNRemoteProject* project
		    IntPtr project  , 
			
			// const char* username
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string username  
			
		);
	}
}