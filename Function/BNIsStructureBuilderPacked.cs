using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsStructureBuilderPacked(BNStructureBuilder* s)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsStructureBuilderPacked"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsStructureBuilderPacked(
			
			// BNStructureBuilder* s
		    IntPtr s  
		);
	}
}