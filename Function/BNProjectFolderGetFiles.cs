using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNProjectFile** BNProjectFolderGetFiles(BNProjectFolder* folder, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNProjectFolderGetFiles"
        )]
		internal static extern IntPtr BNProjectFolderGetFiles(
			
			// BNProjectFolder* folder
		    IntPtr folder   , 
			
			// size_t* count
		    IntPtr count  
		);
	}
}
