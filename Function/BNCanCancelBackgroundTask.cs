using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCanCancelBackgroundTask(BNBackgroundTask* task)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCanCancelBackgroundTask"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCanCancelBackgroundTask(
			
			// BNBackgroundTask* task
		    IntPtr task  
		);
	}
}