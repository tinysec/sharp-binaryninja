using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNFunctionStoreMetadata(BNFunction* func, const char* key, BNMetadata* value, bool isAuto)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNFunctionStoreMetadata"
        )]
		internal static extern void BNFunctionStoreMetadata(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// const char* key
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string key  , 
			
			// BNMetadata* _value
		    IntPtr _value  , 
			
			// bool isAuto
		    bool isAuto  
			
		);
	}
}