using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNWorkflowIsRegistered(BNWorkflow* workflow)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNWorkflowIsRegistered"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNWorkflowIsRegistered(
			
			// BNWorkflow* workflow
		    IntPtr workflow  
		);
	}
}