using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNBackgroundTask* BNBeginBackgroundTask(const char* initialText, bool canCancel)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNBeginBackgroundTask"
        )]
		internal static extern IntPtr BNBeginBackgroundTask(
			
			// const char* initialText
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string initialText  , 
			
			// bool canCancel
		    bool canCancel  
		);
	}
}