using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNSetDisplayName(BNFileMetadata* file, const char* name)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetDisplayName"
        )]
		internal static extern void BNSetDisplayName(
			
			// BNFileMetadata* file
		    IntPtr file   , 
			
			// const char* name
		    string name  
		);
	}
}
