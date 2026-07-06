using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCanUndo(BNFileMetadata* file)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCanUndo"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCanUndo(
			
			// BNFileMetadata* file
		    IntPtr file  
		);
	}
}