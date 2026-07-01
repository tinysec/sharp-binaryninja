using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// size_t BNGetViewDataLength(BNBinaryView* view)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetViewDataLength"
        )]
		internal static extern UIntPtr BNGetViewDataLength(
			
			// BNBinaryView* view
		    IntPtr view  
		);
	}
}
