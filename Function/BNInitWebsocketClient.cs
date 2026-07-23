using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNWebsocketClient* BNInitWebsocketClient(BNWebsocketProvider* provider, BNWebsocketClientCallbacks* callbacks)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNInitWebsocketClient"
        )]
		internal static extern IntPtr BNInitWebsocketClient(
			
			// BNWebsocketProvider* provider
		    IntPtr provider  , 
			
			// BNWebsocketClientCallbacks* callbacks
		    in BNWebsocketClientCallbacks callbacks
			
		);
	}
}
