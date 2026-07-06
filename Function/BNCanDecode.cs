using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCanDecode(BNTransform* xform, BNBinaryView* input)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCanDecode"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCanDecode(
			
			// BNTransform* xform
		    IntPtr xform  , 
			
			// BNBinaryView* input
		    IntPtr input  
		);
	}
}