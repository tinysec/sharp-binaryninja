using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetTypePrinterTypeString(BNTypePrinter* printer, BNType* type, BNPlatform* platform, BNQualifiedName* name, BNTokenEscapingType escaping, const char** result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetTypePrinterTypeString"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetTypePrinterTypeString(
			
			// BNTypePrinter* printer
		    IntPtr printer  , 
			
			// BNType* type
		    IntPtr type  , 
			
			// BNPlatform* platform
		    IntPtr platform  , 
			
			// BNQualifiedName* name
		    IntPtr name  , 
			
			// BNTokenEscapingType escaping
		    TokenEscapingType escaping  , 
			
			// const char** result
		    out IntPtr result  
			
		);
	}
}