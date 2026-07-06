using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTypePrinterDefaultPrintAllTypes(BNTypePrinter* printer, BNQualifiedName* names, BNType** types, uint64_t typeCount, BNBinaryView* data, int32_t paddingCols, BNTokenEscapingType escaping, const char** result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNTypePrinterDefaultPrintAllTypes"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNTypePrinterDefaultPrintAllTypes(
			
			// BNTypePrinter* printer
		    IntPtr printer  , 
			
			// BNQualifiedName* names
		    IntPtr names  , 
			
			// BNType** types
		    IntPtr types  , 
			
			// uint64_t typeCount
		    ulong typeCount  , 
			
			// BNBinaryView* data
		    IntPtr data  , 
			
			// int32_t paddingCols
		    int paddingCols  , 
			
			// BNTokenEscapingType escaping
		    TokenEscapingType escaping  , 
			
			// const char** result
		    out IntPtr result  
			
		);
	}
}