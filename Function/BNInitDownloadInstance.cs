using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNDownloadInstance* BNInitDownloadInstance(BNDownloadProvider* provider, BNDownloadInstanceCallbacks* callbacks)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNInitDownloadInstance"
        )]
		internal static extern IntPtr BNInitDownloadInstance(
			
			// BNDownloadProvider* provider
		    IntPtr provider  , 
			
			// BNDownloadInstanceCallbacks* callbacks
		    in BNDownloadInstanceCallbacks callbacks
			
		);
	}
}
