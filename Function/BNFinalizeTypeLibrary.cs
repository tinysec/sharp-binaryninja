using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNFinalizeTypeLibrary(BNTypeLibrary* lib)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNFinalizeTypeLibrary"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNFinalizeTypeLibrary(
			
			// BNTypeLibrary* lib
		    IntPtr lib  
		);
	}
}