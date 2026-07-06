using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSaveAutoSnapshotWithProgress(BNBinaryView* data, void* ctxt, void** progress, BNSaveSettings* settings)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNSaveAutoSnapshotWithProgress"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNSaveAutoSnapshotWithProgress(
			
			// BNBinaryView* data
		    IntPtr data  , 
			
			// void* ctxt
		    IntPtr ctxt  , 
			
			// void* progress
		    IntPtr progress  , 
			
			// BNSaveSettings* settings
		    IntPtr settings  
		);
	}
}