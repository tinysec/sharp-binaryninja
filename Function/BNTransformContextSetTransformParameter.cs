using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNTransformContextSetTransformParameter(BNTransformContext* context, const char* name, BNDataBuffer* data)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNTransformContextSetTransformParameter"
        )]
		internal static extern void BNTransformContextSetTransformParameter(
			
			// BNTransformContext* context
		    IntPtr context  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// BNDataBuffer* data
		    IntPtr data  
		);
	}
}