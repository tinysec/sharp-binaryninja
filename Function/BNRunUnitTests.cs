using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// int BNRunUnitTests(int argc, char** argv)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNRunUnitTests"
        )]
		internal static extern int BNRunUnitTests(
			
			// int argc
		    int argc   , 
			
			// char** argv
		    IntPtr argv  
		);
	}
}
