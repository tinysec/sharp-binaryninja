using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// const uint8_t* BNGetViewDataPointer(BNBinaryView* view)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetViewDataPointer"
        )]
		internal static extern IntPtr BNGetViewDataPointer(
			
			// BNBinaryView* view
		    IntPtr view  
		);
	}
}
