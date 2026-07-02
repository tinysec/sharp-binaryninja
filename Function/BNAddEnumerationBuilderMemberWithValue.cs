using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNAddEnumerationBuilderMemberWithValue(BNEnumerationBuilder* e, const char* name, uint64_t value)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAddEnumerationBuilderMemberWithValue"
        )]
		internal static extern void BNAddEnumerationBuilderMemberWithValue(
			
			// BNEnumerationBuilder* e
		    IntPtr e  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// uint64_t _value
		    ulong _value  
		);
	}
}