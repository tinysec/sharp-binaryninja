using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNFunctionRemoveMetadata(BNFunction* func, const char* key)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNFunctionRemoveMetadata"
        )]
		internal static extern void BNFunctionRemoveMetadata(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// const char* key
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string key  
			
		);
	}
}