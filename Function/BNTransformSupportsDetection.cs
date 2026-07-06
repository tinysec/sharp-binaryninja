using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTransformSupportsDetection(BNTransform* xform)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNTransformSupportsDetection"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNTransformSupportsDetection(
			
			// BNTransform* xform
		    IntPtr xform  
		);
	}
}