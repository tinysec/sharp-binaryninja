using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNMetadataIsUnsignedInteger(BNMetadata* data)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNMetadataIsUnsignedInteger"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNMetadataIsUnsignedInteger(
			
			// BNMetadata* data
		    IntPtr data  
		);
	}
}