using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNTransformSession* BNCreateTransformSessionFromBinaryView(BNBinaryView* initialView, const char* options)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateTransformSessionFromBinaryView"
        )]
		internal static extern IntPtr BNCreateTransformSessionFromBinaryView(
			
			// BNBinaryView* initialView
		    IntPtr initialView   , 
			
			// const char* options
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string options  
		);
	}
}
