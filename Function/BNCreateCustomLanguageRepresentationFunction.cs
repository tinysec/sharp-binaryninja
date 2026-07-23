using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNLanguageRepresentationFunction* BNCreateCustomLanguageRepresentationFunction(BNLanguageRepresentationFunctionType* type, BNArchitecture* arch, BNFunction* func, BNHighLevelILFunction* highLevelIL, BNCustomLanguageRepresentationFunction* callbacks)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCreateCustomLanguageRepresentationFunction"
        )]
		internal static extern IntPtr BNCreateCustomLanguageRepresentationFunction(
			
			// BNLanguageRepresentationFunctionType* type
		    IntPtr type  , 
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// BNFunction* func
		    IntPtr func  , 
			
			// BNHighLevelILFunction* highLevelIL
		    IntPtr highLevelIL  , 
			
			// BNCustomLanguageRepresentationFunction* callbacks
		    in BNCustomLanguageRepresentationFunction callbacks
		);
	}
}
