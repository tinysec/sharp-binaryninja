using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNTransformSession* BNCreateTransformSessionFromTransformContextWithMode(BNTransformContext* context, BNTransformSessionMode mode, const char* options)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateTransformSessionFromTransformContextWithMode"
        )]
		internal static extern IntPtr BNCreateTransformSessionFromTransformContextWithMode(
			
			// BNTransformContext* context
		    IntPtr context   , 
			
			// BNTransformSessionMode mode
		    TransformSessionMode mode   , 
			
			// const char* options
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string options  
		);
	}
}
