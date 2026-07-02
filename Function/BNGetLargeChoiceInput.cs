using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public static partial class Core
	{
		public static int? GetLargeChoiceInput(
			string prompt  , 
			string title ,
			string[] choices 
		)
		{
			bool ok = NativeMethods.BNGetLargeChoiceInput(
				out ulong result ,
				prompt ,
				title,
				choices ,
				(ulong)choices.Length
			);

			if (!ok)
			{
				return null;
			}

			return (int)result;
		}
	}
	
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetLargeChoiceInput(uint64_t* result, const char* prompt, const char* title, const char** choices, uint64_t count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetLargeChoiceInput"
        )]
		internal static extern bool BNGetLargeChoiceInput(
			
			// uint64_t* result
		    out ulong result  , 
			
			// const char* prompt
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string prompt  , 
			
			// const char* title
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string title  , 
			
			// const char** choices
		    string[] choices  , 
			
			// uint64_t count
		    ulong count  
		);
	}
}