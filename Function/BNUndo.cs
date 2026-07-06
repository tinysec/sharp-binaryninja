using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNUndo(BNFileMetadata* file)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNUndo"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNUndo(
			
			// BNFileMetadata* file
		    IntPtr file  
		);
	}
}