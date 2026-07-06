using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsBackgroundTaskFinished(BNBackgroundTask* task)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsBackgroundTaskFinished"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsBackgroundTaskFinished(
			
			// BNBackgroundTask* task
		    IntPtr task  
		);
	}
}