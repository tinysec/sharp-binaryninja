using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNFreeConstantRendererList(BNConstantRenderer** renderers)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNFreeConstantRendererList"
        )]
		internal static extern void BNFreeConstantRendererList(
			
			// BNConstantRenderer** renderers
		    IntPtr renderers  
		);
	}
}
