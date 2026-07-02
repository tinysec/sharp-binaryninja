using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNTransformSession* BNCreateTransformSessionWithMode(const char* filename, BNTransformSessionMode mode, const char* options)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateTransformSessionWithMode"
        )]
		internal static extern IntPtr BNCreateTransformSessionWithMode(
			
			// const char* filename
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string filename   , 
			
			// BNTransformSessionMode mode
		    TransformSessionMode mode   , 
			
			// const char* options
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string options  
		);
	}
}
