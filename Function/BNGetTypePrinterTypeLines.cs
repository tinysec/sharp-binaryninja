using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetTypePrinterTypeLines(BNTypePrinter* printer, BNType* type, BNTypeContainer* types, BNQualifiedName* name, int32_t paddingCols, bool collapsed, BNTokenEscapingType escaping, BNTypeDefinitionLine** result, uint64_t* resultCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetTypePrinterTypeLines"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetTypePrinterTypeLines(
			
			// BNTypePrinter* printer
		    IntPtr printer  , 
			
			// BNType* type
		    IntPtr type  , 
			
			// BNTypeContainer* types
		    IntPtr types  , 
			
			// BNQualifiedName* name
		    IntPtr name  , 
			
			// int32_t paddingCols
		    int paddingCols  , 
			
			// bool collapsed
		    bool collapsed  , 
			
			// BNTokenEscapingType escaping
		    TokenEscapingType escaping  , 
			
			// BNTypeDefinitionLine** result
		    IntPtr result  , 
			
			// uint64_t* resultCount
		    IntPtr resultCount  
			
		);
	}
}