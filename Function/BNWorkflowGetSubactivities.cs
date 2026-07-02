using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// char** BNWorkflowGetSubactivities(BNWorkflow* workflow, const char* activity, bool immediate, uint64_t* inoutSize)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNWorkflowGetSubactivities"
        )]
		internal static extern IntPtr BNWorkflowGetSubactivities(
			
			// BNWorkflow* workflow
		    IntPtr workflow  , 
			
			// const char* activity
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string activity  , 
			
			// bool immediate
		    bool immediate  , 
			
			// uint64_t* inoutSize
		    ref ulong inoutSize  
		);
	}
}