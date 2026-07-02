using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNType* BNCreateValueType(const char* value)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateValueType"
        )]
		internal static extern IntPtr BNCreateValueType(
			
			// const char* _value
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string _value  
		);
	}
}