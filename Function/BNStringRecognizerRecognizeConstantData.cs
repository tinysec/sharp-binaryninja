using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNStringRecognizerRecognizeConstantData(BNStringRecognizer* recognizer, BNHighLevelILFunction* il, size_t exprIndex, BNDerivedString* out)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNStringRecognizerRecognizeConstantData"
        )]
		internal static extern bool BNStringRecognizerRecognizeConstantData(
			
			// BNStringRecognizer* recognizer
		    IntPtr recognizer   , 
			
			// BNHighLevelILFunction* il
		    IntPtr il   , 
			
			// size_t exprIndex
		    UIntPtr exprIndex   , 
			
			// BNDerivedString* out
		    IntPtr @out  
		);
	}
}
