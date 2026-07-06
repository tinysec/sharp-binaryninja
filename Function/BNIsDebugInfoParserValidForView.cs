using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsDebugInfoParserValidForView(BNDebugInfoParser* parser, BNBinaryView* view)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNIsDebugInfoParserValidForView"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsDebugInfoParserValidForView(
			
			// BNDebugInfoParser* parser
		    IntPtr parser  , 
			
			// BNBinaryView* view
		    IntPtr view  
			
		);
	}
}