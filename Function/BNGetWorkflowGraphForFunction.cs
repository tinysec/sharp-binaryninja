using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNFlowGraph* BNGetWorkflowGraphForFunction(BNFunction* func, const char* name, bool sequential)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetWorkflowGraphForFunction"
        )]
		internal static extern IntPtr BNGetWorkflowGraphForFunction(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// bool sequential
		    bool sequential  
			
		);
	}
}