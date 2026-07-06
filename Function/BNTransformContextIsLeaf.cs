using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTransformContextIsLeaf(BNTransformContext* context)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNTransformContextIsLeaf"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNTransformContextIsLeaf(
			
			// BNTransformContext* context
		    IntPtr context  
		);
	}
}