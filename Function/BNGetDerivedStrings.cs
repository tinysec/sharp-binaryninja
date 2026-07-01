using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNDerivedString* BNGetDerivedStrings(BNBinaryView* view, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetDerivedStrings"
        )]
		internal static extern IntPtr BNGetDerivedStrings(
			
			// BNBinaryView* view
		    IntPtr view   , 
			
			// size_t* count
		    IntPtr count  
		);
	}
}
