using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNAddGraphReportToCollection(BNReportCollection* reports, BNBinaryView* view, const char* title, BNFlowGraph* graph)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAddGraphReportToCollection"
        )]
		internal static extern void BNAddGraphReportToCollection(
			
			// BNReportCollection* reports
		    IntPtr reports  , 
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* title
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string title  , 
			
			// BNFlowGraph* graph
		    IntPtr graph  
		);
	}
}