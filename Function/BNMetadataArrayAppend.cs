using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNMetadataArrayAppend(BNMetadata* data, BNMetadata* md)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNMetadataArrayAppend"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNMetadataArrayAppend(
			
			// BNMetadata* data
		    IntPtr data  , 
			
			// BNMetadata* md
		    IntPtr md  
		);
	}
}