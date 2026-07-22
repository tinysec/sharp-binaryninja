using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNConstantRenderer** BNGetConstantRendererList(size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetConstantRendererList"
        )]
		internal static extern IntPtr BNGetConstantRendererList(
			
			// size_t* count
		    out ulong count
		);
	}
}
