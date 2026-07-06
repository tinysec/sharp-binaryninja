using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNProjectFolderSetParent(BNProjectFolder* folder, BNProjectFolder* parent)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNProjectFolderSetParent"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNProjectFolderSetParent(
			
			// BNProjectFolder* folder
		    IntPtr folder  , 
			
			// BNProjectFolder* parent
		    IntPtr parent  
			
		);
	}
}