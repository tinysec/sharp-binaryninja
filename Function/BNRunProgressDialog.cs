using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRunProgressDialog(const char* title, bool canCancel, void** task, void* taskCtxt)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRunProgressDialog"
        )]
		internal static extern bool BNRunProgressDialog(
			
			// const char* title
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string title  , 
			
			// bool canCancel
		    bool canCancel  , 
			
			// void** task
		    IntPtr task  , 
			
			// void* taskCtxt
		    IntPtr taskCtxt  
			
		);
	}
}