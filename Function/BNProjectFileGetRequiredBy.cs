using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNProjectFile** BNProjectFileGetRequiredBy(BNProjectFile* file, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNProjectFileGetRequiredBy"
        )]
		internal static extern IntPtr BNProjectFileGetRequiredBy(
			
			// BNProjectFile* file
		    IntPtr file   , 
			
			// size_t* count
		    IntPtr count  
		);
	}
}
