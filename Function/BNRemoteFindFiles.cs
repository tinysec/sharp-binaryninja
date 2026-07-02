using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNRemoteFileSearchMatch* BNRemoteFindFiles(BNRemote* remote, const char* name, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteFindFiles"
        )]
		internal static extern IntPtr BNRemoteFindFiles(
			
			// BNRemote* remote
		    IntPtr remote   , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name   , 
			
			// size_t* count
		    IntPtr count  
		);
	}
}
