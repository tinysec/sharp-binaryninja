using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNAddMarkdownReportToCollection(BNReportCollection* reports, BNBinaryView* view, const char* title, const char* contents, const char* plaintext)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAddMarkdownReportToCollection"
        )]
		internal static extern void BNAddMarkdownReportToCollection(
			
			// BNReportCollection* reports
		    IntPtr reports  , 
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* title
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string title  , 
			
			// const char* contents
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string contents  , 
			
			// const char* plaintext
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string plaintext  
		);
	}
}