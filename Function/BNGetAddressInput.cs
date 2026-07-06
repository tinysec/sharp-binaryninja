using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public static partial class Core
	{
		public static ulong? GetAddressInput(string prompt = "PROMPT>" , string title = "get_address" )
		{
			bool ok = NativeMethods.BNGetAddressInput(
				out ulong result ,
				prompt ,
				title,
				IntPtr.Zero ,
				0
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
		/// bool BNGetAddressInput(uint64_t* result, const char* prompt, const char* title, BNBinaryView* view, uint64_t currentAddr)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetAddressInput"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetAddressInput(
			
			// uint64_t* result
		    out ulong result  , 
			
			// const char* prompt
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string prompt  , 
			
			// const char* title
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string title  , 
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// uint64_t currentAddr
		    ulong currentAddr  
		);
	}
}