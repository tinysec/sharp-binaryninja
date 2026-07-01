using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNTransformSessionSetInteractive(BNTransformSession* session, bool interactive)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNTransformSessionSetInteractive"
        )]
		internal static extern void BNTransformSessionSetInteractive(
			
			// BNTransformSession* session
		    IntPtr session   , 
			
			// bool interactive
		    bool interactive  
		);
	}
}
