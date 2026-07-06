using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNWorkflowReplace(BNWorkflow* workflow, const char* activity, const char* newActivity)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNWorkflowReplace"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNWorkflowReplace(
			
			// BNWorkflow* workflow
		    IntPtr workflow  , 
			
			// const char* activity
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string activity  , 
			
			// const char* newActivity
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string newActivity  
			
		);
	}
}