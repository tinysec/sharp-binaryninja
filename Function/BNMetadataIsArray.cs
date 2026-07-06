using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNMetadataIsArray(BNMetadata* data)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNMetadataIsArray"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNMetadataIsArray(
			
			// BNMetadata* data
		    IntPtr data  
		);
	}
}