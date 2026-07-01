using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// char* BNGetStringRecognizerName(BNStringRecognizer* recognizer)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetStringRecognizerName"
        )]
		internal static extern IntPtr BNGetStringRecognizerName(
			
			// BNStringRecognizer* recognizer
		    IntPtr recognizer  
		);
	}
}
