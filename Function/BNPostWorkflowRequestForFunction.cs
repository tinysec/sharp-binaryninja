using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// const char* BNPostWorkflowRequestForFunction(BNFunction* func, const char* request)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNPostWorkflowRequestForFunction"
        )]
		internal static extern IntPtr BNPostWorkflowRequestForFunction(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// const char* request
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string request  
			
		);
	}
}