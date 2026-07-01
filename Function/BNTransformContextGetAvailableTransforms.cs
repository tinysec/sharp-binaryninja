using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// char** BNTransformContextGetAvailableTransforms(BNTransformContext* context, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNTransformContextGetAvailableTransforms"
        )]
		internal static extern IntPtr BNTransformContextGetAvailableTransforms(
			
			// BNTransformContext* context
		    IntPtr context   , 
			
			// size_t* count
		    IntPtr count  
		);
	}
}
