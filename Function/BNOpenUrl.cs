using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public static partial class Core
	{
		public static bool OpenUrl(string url)
		{
			return NativeMethods.BNOpenUrl(url);
		}
	}
	
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNOpenUrl(const char* url)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNOpenUrl"
        )]
		internal static extern bool BNOpenUrl(
			
			// const char* url
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string url  
		);
	}
}