using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNPluginInstall(BNPlugin* p, const char* versionID)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNPluginInstall"
        )]
		internal static extern bool BNPluginInstall(
			
			// BNPlugin* p
		    IntPtr p   , 
			
			// const char* versionID
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string versionID  
		);
	}
}
