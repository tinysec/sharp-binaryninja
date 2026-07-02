using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNShowGraphReport(BNBinaryView* view, const char* title, BNFlowGraph* graph)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNShowGraphReport"
        )]
		internal static extern void BNShowGraphReport(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* title
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string title  , 
			
			// BNFlowGraph* graph
		    IntPtr graph  
		);
	}
}