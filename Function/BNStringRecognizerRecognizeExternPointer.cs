using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNStringRecognizerRecognizeExternPointer(BNStringRecognizer* recognizer, BNHighLevelILFunction* il, size_t exprIndex, BNType* type, int64_t val, uint64_t offset, BNDerivedString* out)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNStringRecognizerRecognizeExternPointer"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNStringRecognizerRecognizeExternPointer(
			
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
			
			// uint64_t offset
		    ulong offset   , 
			
			// BNDerivedString* out
		    out BNDerivedString @out
		);
	}
}
