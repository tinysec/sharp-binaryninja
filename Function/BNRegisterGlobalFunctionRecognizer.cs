using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNRegisterGlobalFunctionRecognizer(BNFunctionRecognizer* rec)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRegisterGlobalFunctionRecognizer"
        )]
		internal static extern void BNRegisterGlobalFunctionRecognizer(
			
			// BNFunctionRecognizer* rec
		    in BNFunctionRecognizer rec
			
		);
	}
}
