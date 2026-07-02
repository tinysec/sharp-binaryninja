using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNWorkflow* BNWorkflowClone(BNWorkflow* workflow, const char* name, const char* activity)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNWorkflowClone"
        )]
		internal static extern IntPtr BNWorkflowClone(
			
			// BNWorkflow* workflow
		    IntPtr workflow  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// const char* activity
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string activity  
		);
	}
}