using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNTransformSession* BNCreateTransformSession(const char* filename, const char* options)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateTransformSession"
        )]
		internal static extern IntPtr BNCreateTransformSession(
			
			// const char* filename
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string filename   , 
			
			// const char* options
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string options  
		);
	}
}
