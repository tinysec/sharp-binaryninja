using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNMetadataIsStringList(BNMetadata* data)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNMetadataIsStringList"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNMetadataIsStringList(
			
			// BNMetadata* data
		    IntPtr data  
		);
	}
}