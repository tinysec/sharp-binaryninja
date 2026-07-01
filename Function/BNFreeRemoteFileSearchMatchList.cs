using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNFreeRemoteFileSearchMatchList(BNRemoteFileSearchMatch* matches, size_t count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNFreeRemoteFileSearchMatchList"
        )]
		internal static extern void BNFreeRemoteFileSearchMatchList(
			
			// BNRemoteFileSearchMatch* matches
		    IntPtr matches   , 
			
			// size_t count
		    UIntPtr count  
		);
	}
}
