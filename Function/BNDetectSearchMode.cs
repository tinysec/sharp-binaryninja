using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{

	public static partial class Core
	{
		public static string DetectSearchMode(string query)
		{
			return UnsafeUtils.TakeAnsiString(
				NativeMethods.BNDetectSearchMode(query)
			);
		}
	}
	
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// const char* BNDetectSearchMode(const char* query)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNDetectSearchMode"
        )]
		internal static extern IntPtr BNDetectSearchMode(
			
			// const char* query
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string query  
		);
	}
}