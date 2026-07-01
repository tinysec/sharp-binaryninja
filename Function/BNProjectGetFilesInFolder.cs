using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNProjectFile** BNProjectGetFilesInFolder(BNProject* project, BNProjectFolder* folder, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNProjectGetFilesInFolder"
        )]
		internal static extern IntPtr BNProjectGetFilesInFolder(
			
			// BNProject* project
		    IntPtr project   , 
			
			// BNProjectFolder* folder
		    IntPtr folder   , 
			
			// size_t* count
		    IntPtr count  
		);
	}
}
