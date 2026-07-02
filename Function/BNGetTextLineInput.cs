using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public static partial class Core
	{
		public static string? GetTextLineInput(string prompt , string title )
		{
			bool ok = NativeMethods.BNGetTextLineInput(
				out IntPtr result ,
				prompt ,
				title
			);

			if (!ok)
			{
				return null;
			}

			return UnsafeUtils.TakeUtf8String(result);
		}
	}
	
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetTextLineInput(const char** result, const char* prompt, const char* title)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetTextLineInput"
        )]
		internal static extern bool BNGetTextLineInput(
			
			// char** result
		    out IntPtr result  , 
			
			// const char* prompt
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string prompt  , 
			
			// const char* title
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string title  
		);
	}
}