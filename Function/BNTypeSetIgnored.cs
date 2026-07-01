using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNType* BNTypeSetIgnored(BNType* type, bool ignored)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNTypeSetIgnored"
        )]
		internal static extern IntPtr BNTypeSetIgnored(
			
			// BNType* type
		    IntPtr type   , 
			
			// bool ignored
		    bool ignored  
		);
	}
}
