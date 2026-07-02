using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNAddPlainTextReportToCollection(BNReportCollection* reports, BNBinaryView* view, const char* title, const char* contents)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAddPlainTextReportToCollection"
        )]
		internal static extern void BNAddPlainTextReportToCollection(
			
			// BNReportCollection* reports
		    IntPtr reports  , 
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* title
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string title  , 
			
			// const char* contents
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string contents  
		);
	}
}