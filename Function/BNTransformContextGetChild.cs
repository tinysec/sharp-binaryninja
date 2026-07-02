using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNTransformContext* BNTransformContextGetChild(BNTransformContext* context, const char* filename)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNTransformContextGetChild"
        )]
		internal static extern IntPtr BNTransformContextGetChild(
			
			// BNTransformContext* context
		    IntPtr context  , 
			
			// const char* filename
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string filename  
			
		);
	}
}