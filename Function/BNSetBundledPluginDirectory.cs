using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public static partial class Core
	{
		public static void SetBundledPluginDirectory( string path  )
		{
			NativeMethods.BNSetBundledPluginDirectory(path);
		}
	}
	
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNSetBundledPluginDirectory(const char* path)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetBundledPluginDirectory"
        )]
		public static extern void BNSetBundledPluginDirectory(
			
			// const char* path
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string path  
		);
	}
}