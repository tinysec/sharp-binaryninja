using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNProjectFile** BNProjectFileGetDependencies(BNProjectFile* file, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNProjectFileGetDependencies"
        )]
		internal static extern IntPtr BNProjectFileGetDependencies(
			
			// BNProjectFile* file
		    IntPtr file   , 
			
			// size_t* count
		    IntPtr count  
		);
	}
}
