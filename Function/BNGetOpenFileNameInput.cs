using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public static partial class Core
	{
		public static string? GetOpenFileNameInput(
			string prompt = "Choose a file:" , 
			string ext  = "Executables (*.exe *.dll *.sys);;All Files (*)"
		)
		{
			bool ok = NativeMethods.BNGetOpenFileNameInput(
				out IntPtr result ,
				prompt ,
				ext
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
		/// bool BNGetOpenFileNameInput(const char** result, const char* prompt, const char* ext)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetOpenFileNameInput"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetOpenFileNameInput(
			
			// char** result
		    out IntPtr result  , 
			
			// const char* prompt
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string prompt  , 
			
			// const char* ext
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string ext  
		);
	}
}