using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNRegisterArchitectureFunctionRecognizer(BNArchitecture* arch, BNFunctionRecognizer* rec)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRegisterArchitectureFunctionRecognizer"
        )]
		internal static extern void BNRegisterArchitectureFunctionRecognizer(
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// BNFunctionRecognizer* rec
		    in BNFunctionRecognizer rec
			
		);
	}
}
