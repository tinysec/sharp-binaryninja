using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public static partial class Core
	{
		public static string? GetDirectoryNameInput(
			string prompt = "Choose a directory:" , 
			string defaultName  = "."
		)
		{
			bool ok = NativeMethods.BNGetDirectoryNameInput(
				out IntPtr result ,
				prompt ,
				defaultName
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
		/// bool BNGetDirectoryNameInput(const char** result, const char* prompt, const char* defaultName)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetDirectoryNameInput"
        )]
		internal static extern bool BNGetDirectoryNameInput(
			
			// char** result
		    out IntPtr result  , 
			
			// const char* prompt
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string prompt  , 
			
			// const char* defaultName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string defaultName  
		);
	}
}