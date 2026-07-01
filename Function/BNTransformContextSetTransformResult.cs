using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNTransformContextSetTransformResult(BNTransformContext* context, BNTransformResult result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNTransformContextSetTransformResult"
        )]
		internal static extern void BNTransformContextSetTransformResult(
			
			// BNTransformContext* context
		    IntPtr context   , 
			
			// BNTransformResult result
		    TransformResult result  
		);
	}
}
