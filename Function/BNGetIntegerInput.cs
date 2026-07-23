using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public static partial class Core
	{
		public static long? GetIntegerInput(string prompt , string title )
		{
			bool ok = NativeMethods.BNGetIntegerInput(
				out long result ,
				prompt ,
				title
			);

			if (!ok)
			{
				return null;
			}

			return result;
		}
	}
	
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetIntegerInput(int64_t* result, const char* prompt, const char* title)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetIntegerInput"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetIntegerInput(
			
			// int64_t* result
		    out long result  ,
			
			// const char* prompt
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string prompt  , 
			
			// const char* title
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string title  
		);
	}
}
