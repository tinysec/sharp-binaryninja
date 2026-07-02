using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRegisterWorkflow(BNWorkflow* workflow, const char* configuration)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRegisterWorkflow"
        )]
		internal static extern bool BNRegisterWorkflow(
			
			// BNWorkflow* workflow
		    IntPtr workflow  , 
			
			// const char* configuration
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string configuration  
		);
	}
}