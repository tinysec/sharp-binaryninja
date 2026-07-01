using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNTransformContextSetTransformName(BNTransformContext* context, const char* transformName)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNTransformContextSetTransformName"
        )]
		internal static extern void BNTransformContextSetTransformName(
			
			// BNTransformContext* context
		    IntPtr context   , 
			
			// const char* transformName
		    string transformName  
		);
	}
}
