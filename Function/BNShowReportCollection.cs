using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNShowReportCollection(const char* title, BNReportCollection* reports)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNShowReportCollection"
        )]
		internal static extern void BNShowReportCollection(
			
			// const char* title
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string title  , 
			
			// BNReportCollection* reports
		    IntPtr reports  
		);
	}
}