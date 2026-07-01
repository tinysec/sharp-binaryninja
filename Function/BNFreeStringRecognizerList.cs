using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNFreeStringRecognizerList(BNStringRecognizer** recognizers)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNFreeStringRecognizerList"
        )]
		internal static extern void BNFreeStringRecognizerList(
			
			// BNStringRecognizer** recognizers
		    IntPtr recognizers  
		);
	}
}
