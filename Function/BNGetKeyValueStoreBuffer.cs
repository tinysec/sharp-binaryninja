using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNDataBuffer* BNGetKeyValueStoreBuffer(BNKeyValueStore* store, const char* name)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetKeyValueStoreBuffer"
        )]
		internal static extern IntPtr BNGetKeyValueStoreBuffer(
			
			// BNKeyValueStore* store
		    IntPtr store  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  
		);
	}
}