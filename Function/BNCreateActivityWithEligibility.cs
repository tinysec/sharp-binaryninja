using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNActivity* BNCreateActivityWithEligibility(const char* configuration, void* ctxt, void** action, void** eligibilityHandler)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateActivityWithEligibility"
        )]
		internal static extern IntPtr BNCreateActivityWithEligibility(
			
			// const char* configuration
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string configuration  , 
			
			// void* ctxt
		    IntPtr ctxt  , 
			
			// void** action
		    IntPtr action  , 
			
			// void** eligibilityHandler
		    IntPtr eligibilityHandler  
		);
	}
}