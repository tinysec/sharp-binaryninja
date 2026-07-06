using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNRemoteFileDownload(BNRemoteFile* file, BNProgressFunction progress, void* progressCtxt)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNRemoteFileDownload"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNRemoteFileDownload(
			
			// BNRemoteFile* file
		    IntPtr file   , 
			
			// BNProgressFunction progress
		    IntPtr progress   , 
			
			// void* progressCtxt
		    IntPtr progressCtxt  
		);
	}
}
