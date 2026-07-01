using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNClearUserGlobalPointerValues(BNBinaryView* view)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNClearUserGlobalPointerValues"
        )]
		internal static extern void BNClearUserGlobalPointerValues(
			
			// BNBinaryView* view
		    IntPtr view  
		);
	}
}
