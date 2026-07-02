using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNLinearViewObject* BNCreateLinearViewLanguageRepresentation(BNBinaryView* view, BNDisassemblySettings* settings, const char* language)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateLinearViewLanguageRepresentation"
        )]
		internal static extern IntPtr BNCreateLinearViewLanguageRepresentation(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// BNDisassemblySettings* settings
		    IntPtr settings  , 
			
			// const char* language
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string language  
		);
	}
}