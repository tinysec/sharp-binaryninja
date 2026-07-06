using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNMetadataIsEqual(BNMetadata* lhs, BNMetadata* rhs)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNMetadataIsEqual"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNMetadataIsEqual(
			
			// BNMetadata* lhs
		    IntPtr lhs  , 
			
			// BNMetadata* rhs
		    IntPtr rhs  
		);
	}
}