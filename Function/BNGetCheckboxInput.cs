using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public static partial class Core
	{
		public static bool? GetCheckboxInput(
			string prompt  , 
			string title  ,
			bool   defaultChoice = false
		)
		{
			long defaltChoiceValue = ( defaultChoice ? 1 : 0 );
			
			bool ok = NativeMethods.BNGetCheckboxInput(
				out long result ,
				prompt ,
				title,
				ref defaltChoiceValue
			);

			if (!ok)
			{
				return null;
			}

			return result != 0;
		}
	}
	
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetCheckboxInput(int64_t* result, const char* prompt, const char* title, int64_t* defaultChoice)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetCheckboxInput"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetCheckboxInput(
			
			// int64_t* result
		    out long result  , 
			
			// const char* prompt
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string prompt  , 
			
			// const char* title
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string title  , 
			
			// int64_t* defaultChoice
		    ref long defaultChoice  
		);
	}
}