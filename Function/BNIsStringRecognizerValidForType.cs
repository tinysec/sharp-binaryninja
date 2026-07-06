using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNIsStringRecognizerValidForType( BNStringRecognizer* recognizer, BNHighLevelILFunction* il, BNType* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsStringRecognizerValidForType"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsStringRecognizerValidForType(
			
			// BNStringRecognizer* recognizer
		    IntPtr recognizer   , 
			
			// BNHighLevelILFunction* il
		    IntPtr il   , 
			
			// BNType* type
		    IntPtr type  
		);
	}
}
