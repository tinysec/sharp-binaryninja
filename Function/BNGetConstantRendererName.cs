using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// char* BNGetConstantRendererName(BNConstantRenderer* renderer)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetConstantRendererName"
        )]
		internal static extern IntPtr BNGetConstantRendererName(
			
			// BNConstantRenderer* renderer
		    IntPtr renderer  
		);
	}
}
