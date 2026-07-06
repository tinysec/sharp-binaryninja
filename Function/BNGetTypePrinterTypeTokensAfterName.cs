using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetTypePrinterTypeTokensAfterName(BNTypePrinter* printer, BNType* type, BNPlatform* platform, uint8_t baseConfidence, BNType* parentType, BNTokenEscapingType escaping, BNInstructionTextToken** result, uint64_t* resultCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetTypePrinterTypeTokensAfterName"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetTypePrinterTypeTokensAfterName(
			
			// BNTypePrinter* printer
		    IntPtr printer  , 
			
			// BNType* type
		    IntPtr type  , 
			
			// BNPlatform* platform
		    IntPtr platform  , 
			
			// uint8_t baseConfidence
		    byte baseConfidence  , 
			
			// BNType* parentType
		    IntPtr parentType  , 
			
			// BNTokenEscapingType escaping
		    TokenEscapingType escaping  , 
			
			// BNInstructionTextToken** result
		    IntPtr result  , 
			
			// uint64_t* resultCount
		    IntPtr resultCount  
			
		);
	}
}