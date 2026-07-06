using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSetKeyValueStoreBuffer(BNKeyValueStore* store, const char* name, BNDataBuffer* value)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetKeyValueStoreBuffer"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNSetKeyValueStoreBuffer(
			
			// BNKeyValueStore* store
		    IntPtr store  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// BNDataBuffer* _value
		    IntPtr _value  
		);
	}
}