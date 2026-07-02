using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCreateSnapshotedView(BNBinaryView* data, const char* viewName)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateSnapshotedView"
        )]
		internal static extern bool BNCreateSnapshotedView(
			
			// BNBinaryView* data
		    IntPtr data  , 
			
			// const char* viewName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string viewName  
		);
	}
}