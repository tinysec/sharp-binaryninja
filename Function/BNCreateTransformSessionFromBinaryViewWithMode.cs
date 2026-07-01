using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNTransformSession* BNCreateTransformSessionFromBinaryViewWithMode(BNBinaryView* initialView, BNTransformSessionMode mode, const char* options)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateTransformSessionFromBinaryViewWithMode"
        )]
		internal static extern IntPtr BNCreateTransformSessionFromBinaryViewWithMode(
			
			// BNBinaryView* initialView
		    IntPtr initialView   , 
			
			// BNTransformSessionMode mode
		    TransformSessionMode mode   , 
			
			// const char* options
		    string options  
		);
	}
}
