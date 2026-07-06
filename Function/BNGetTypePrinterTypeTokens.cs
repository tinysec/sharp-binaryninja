using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetTypePrinterTypeTokens(BNTypePrinter* printer, BNType* type, BNPlatform* platform, BNQualifiedName* name, uint8_t baseConfidence, BNTokenEscapingType escaping, BNInstructionTextToken** result, uint64_t* resultCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetTypePrinterTypeTokens"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetTypePrinterTypeTokens(
			
			// BNTypePrinter* printer
		    IntPtr printer  , 
			
			// BNType* type
		    IntPtr type  , 
			
			// BNPlatform* platform
		    IntPtr platform  , 
			
			// BNQualifiedName* name
		    IntPtr name  , 
			
			// uint8_t baseConfidence
		    byte baseConfidence  , 
			
			// BNTokenEscapingType escaping
		    TokenEscapingType escaping  , 
			
			// BNInstructionTextToken** result
		    IntPtr result  , 
			
			// uint64_t* resultCount
		    IntPtr resultCount  
			
		);
	}
}