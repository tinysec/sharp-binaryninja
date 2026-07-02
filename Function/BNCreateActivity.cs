using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNActivity* BNCreateActivity(const char* configuration, void* ctxt, void** action)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateActivity"
        )]
		internal static extern IntPtr BNCreateActivity(
			
			// const char* configuration
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string configuration  , 
			
			// void* ctxt
		    IntPtr ctxt  , 
			
			// void** action
		    IntPtr action  
		);
	}
}