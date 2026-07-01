using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNSetStructureBuilder(BNTypeBuilder* type, BNStructureBuilder* s)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNSetStructureBuilder"
        )]
		internal static extern void BNSetStructureBuilder(
			
			// BNTypeBuilder* type
		    IntPtr type   , 
			
			// BNStructureBuilder* s
		    IntPtr s  
		);
	}
}
