using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNSetBackgroundTaskProgressText(BNBackgroundTask* task, const char* text)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetBackgroundTaskProgressText"
        )]
		internal static extern void BNSetBackgroundTaskProgressText(
			
			// BNBackgroundTask* task
		    IntPtr task  , 
			
			// const char* text
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string text  
		);
	}
}