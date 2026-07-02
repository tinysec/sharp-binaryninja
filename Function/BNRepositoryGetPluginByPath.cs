using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNRepoPlugin* BNRepositoryGetPluginByPath(BNRepository* r, const char* pluginPath)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRepositoryGetPluginByPath"
        )]
		internal static extern IntPtr BNRepositoryGetPluginByPath(
			
			// BNRepository* r
		    IntPtr r  , 
			
			// const char* pluginPath
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string pluginPath  
			
		);
	}
}