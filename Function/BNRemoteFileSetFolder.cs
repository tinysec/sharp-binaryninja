using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRemoteFileSetFolder(BNRemoteFile* file, BNRemoteFolder* folder)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteFileSetFolder"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNRemoteFileSetFolder(
			
			// BNRemoteFile* file
		    IntPtr file  , 
			
			// BNRemoteFolder* folder
		    IntPtr folder  
			
		);
	}
}