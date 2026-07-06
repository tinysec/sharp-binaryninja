using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetFormInput(BNFormInputField* fields, uint64_t count, const char* title)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetFormInput"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetFormInput(
			
			// BNFormInputField* fields
			BNFormInputField[] fields  , 
			
			// uint64_t count
		    ulong count  , 
			
			// const char* title
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string title  
		);
	}
}