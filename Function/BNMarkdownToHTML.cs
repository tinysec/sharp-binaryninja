using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public static partial class Core
	{
		public static string MarkdownToHTML(string contents)
		{
			return UnsafeUtils.TakeUtf8String(
				NativeMethods.BNMarkdownToHTML(contents)
			);
		}
	}
	
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// char* BNMarkdownToHTML(const char* contents)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNMarkdownToHTML"
        )]
		internal static extern IntPtr BNMarkdownToHTML(
			
			// const char* contents
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string contents  
		);
	}
}