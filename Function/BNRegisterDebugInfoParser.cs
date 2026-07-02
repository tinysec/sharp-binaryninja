using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNDebugInfoParser* BNRegisterDebugInfoParser(const char* name, void** isValid, void** parseInfo, void* context)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRegisterDebugInfoParser"
        )]
		internal static extern IntPtr BNRegisterDebugInfoParser(
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// void** isValid
		    IntPtr isValid  , 
			
			// void** parseInfo
		    IntPtr parseInfo  , 
			
			// void* context
		    IntPtr context  
			
		);
	}
}