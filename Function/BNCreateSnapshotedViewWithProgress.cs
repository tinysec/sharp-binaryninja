using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCreateSnapshotedViewWithProgress(BNBinaryView* data, const char* viewName, void* ctxt, void** progress)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateSnapshotedViewWithProgress"
        )]
		internal static extern bool BNCreateSnapshotedViewWithProgress(
			
			// BNBinaryView* data
		    IntPtr data  , 
			
			// const char* viewName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string viewName  , 
			
			// void* ctxt
		    IntPtr ctxt  , 
			
			// void** progress
		    IntPtr progress  
		);
	}
}