using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetTypePrinterTypeStringBeforeName(BNTypePrinter* printer, BNType* type, BNPlatform* platform, BNTokenEscapingType escaping, const char** result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetTypePrinterTypeStringBeforeName"
        )]
		internal static extern bool BNGetTypePrinterTypeStringBeforeName(
			
			// BNTypePrinter* printer
		    IntPtr printer  , 
			
			// BNType* type
		    IntPtr type  , 
			
			// BNPlatform* platform
		    IntPtr platform  , 
			
			// BNTokenEscapingType escaping
		    TokenEscapingType escaping  , 
			
			// const char** result
		    out IntPtr result  
			
		);
	}
}