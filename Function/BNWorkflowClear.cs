using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNWorkflowClear(BNWorkflow* workflow)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNWorkflowClear"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNWorkflowClear(
			
			// BNWorkflow* workflow
		    IntPtr workflow  
			
		);
	}
}