using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNReplaceEnumerationBuilderMember(BNEnumerationBuilder* e, uint64_t idx, const char* name, uint64_t value)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNReplaceEnumerationBuilderMember"
        )]
		internal static extern void BNReplaceEnumerationBuilderMember(
			
			// BNEnumerationBuilder* e
		    IntPtr e  , 
			
			// uint64_t idx
		    ulong idx  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// uint64_t _value
		    ulong _value  
		);
	}
}