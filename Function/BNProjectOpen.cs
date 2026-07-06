using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNProjectOpen(BNProject* project)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNProjectOpen"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNProjectOpen(
			
			// BNProject* project
		    IntPtr project  
			
		);
	}
}