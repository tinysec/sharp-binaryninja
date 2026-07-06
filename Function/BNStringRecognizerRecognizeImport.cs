using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNStringRecognizerRecognizeImport(BNStringRecognizer* recognizer, BNHighLevelILFunction* il, size_t exprIndex, BNType* type, int64_t val, BNDerivedString* out)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNStringRecognizerRecognizeImport"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNStringRecognizerRecognizeImport(
			
			// BNStringRecognizer* recognizer
		    IntPtr recognizer   , 
			
			// BNHighLevelILFunction* il
		    IntPtr il   , 
			
			// size_t exprIndex
		    UIntPtr exprIndex   , 
			
			// BNType* type
		    IntPtr type   , 
			
			// int64_t val
		    long val   , 
			
			// BNDerivedString* out
		    IntPtr @out  
		);
	}
}
