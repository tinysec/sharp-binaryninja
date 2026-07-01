using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNTransformContext* BNTransformContextSetChild(BNTransformContext* context, BNDataBuffer* data, const char* filename, BNTransformResult result, const char* message, bool filenameIsDescriptor)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNTransformContextSetChild"
        )]
		internal static extern IntPtr BNTransformContextSetChild(
			
			// BNTransformContext* context
		    IntPtr context   , 
			
			// BNDataBuffer* data
		    IntPtr data   , 
			
			// const char* filename
		    string filename   , 
			
			// BNTransformResult result
		    TransformResult result   , 
			
			// const char* message
		    string message   , 
			
			// bool filenameIsDescriptor
		    bool filenameIsDescriptor  
		);
	}
}
