using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSaveAutoSnapshot(BNBinaryView* data, BNSaveSettings* settings)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNSaveAutoSnapshot"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNSaveAutoSnapshot(
			
			// BNBinaryView* data
		    IntPtr data  , 
			
			// BNSaveSettings* settings
		    IntPtr settings  
		);
	}
}