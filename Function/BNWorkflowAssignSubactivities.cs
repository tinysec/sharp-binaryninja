using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNWorkflowAssignSubactivities(BNWorkflow* workflow, const char* activity, const char** activities, uint64_t size)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNWorkflowAssignSubactivities"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNWorkflowAssignSubactivities(
			
			// BNWorkflow* workflow
		    IntPtr workflow  , 
			
			// const char* activity
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string activity  , 
			
			// const char** activities: caller-built UTF-8 char** block (string[]
			// elements cannot carry LPUTF8Str, so the wrapper builds the block).
		    IntPtr activities  ,
			
			// uint64_t size
		    ulong size  
			
		);
	}
}