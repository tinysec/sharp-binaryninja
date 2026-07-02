using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// const char* BNPostWorkflowRequestForBinaryView(BNBinaryView* view, const char* request)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNPostWorkflowRequestForBinaryView"
        )]
		internal static extern IntPtr BNPostWorkflowRequestForBinaryView(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* request
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string request  
			
		);
	}
}