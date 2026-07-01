using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNRemoteFileDownloadContents(BNRemoteFile* file, BNProgressFunction progress, void* progressCtxt, uint8_t** data, size_t* size)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNRemoteFileDownloadContents"
        )]
		internal static extern bool BNRemoteFileDownloadContents(
			
			// BNRemoteFile* file
		    IntPtr file   , 
			
			// BNProgressFunction progress
		    IntPtr progress   , 
			
			// void* progressCtxt
		    IntPtr progressCtxt   , 
			
			// uint8_t** data
		    IntPtr data   , 
			
			// size_t* size
		    IntPtr size  
		);
	}
}
