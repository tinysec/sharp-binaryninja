using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsSnapshotDataAppliedWithoutError(BNFileMetadata* view)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsSnapshotDataAppliedWithoutError"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsSnapshotDataAppliedWithoutError(
			
			// BNFileMetadata* view
		    IntPtr view  
		);
	}
}