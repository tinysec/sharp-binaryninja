using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNStringRecognizer** BNGetStringRecognizerList(size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetStringRecognizerList"
        )]
		internal static extern IntPtr BNGetStringRecognizerList(
			
			// size_t* count
		    IntPtr count  
		);
	}
}
