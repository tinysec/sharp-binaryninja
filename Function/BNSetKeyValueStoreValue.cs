using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSetKeyValueStoreValue(BNKeyValueStore* store, const char* name, const char* value)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetKeyValueStoreValue"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNSetKeyValueStoreValue(
			
			// BNKeyValueStore* store
		    IntPtr store  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// const char* _value
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string _value  
		);
	}
}