using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCoreEnumToString(const char* enumName, uint64_t value, const char** result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCoreEnumToString"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCoreEnumToString(
			
			// const char* enumName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string enumName  , 
			
			// uint64_t _value
		    ulong _value  , 
			
			// const char** result
		    out IntPtr result  
		);
	}
}