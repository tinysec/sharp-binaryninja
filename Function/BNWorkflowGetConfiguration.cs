using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// const char* BNWorkflowGetConfiguration(BNWorkflow* workflow, const char* activity)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNWorkflowGetConfiguration"
        )]
		internal static extern IntPtr BNWorkflowGetConfiguration(
			
			// BNWorkflow* workflow
		    IntPtr workflow  , 
			
			// const char* activity
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string activity  
			
		);
	}
}