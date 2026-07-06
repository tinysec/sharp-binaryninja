using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNPluginUpdate(BNPlugin* p, const char* versionID)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNPluginUpdate"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNPluginUpdate(
			
			// BNPlugin* p
		    IntPtr p   , 
			
			// const char* versionID
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string versionID  
		);
	}
}
