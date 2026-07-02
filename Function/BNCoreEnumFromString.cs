using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCoreEnumFromString(const char* enumName, const char* value, uint64_t* result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCoreEnumFromString"
        )]
		internal static extern bool BNCoreEnumFromString(
			
			// const char* enumName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string enumName  , 
			
			// const char* _value
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string _value  , 
			
			// uint64_t* result
		    IntPtr result  
		);
	}
}