using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNMetadataIsBooleanList(BNMetadata* data)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNMetadataIsBooleanList"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNMetadataIsBooleanList(
			
			// BNMetadata* data
		    IntPtr data  
		);
	}
}