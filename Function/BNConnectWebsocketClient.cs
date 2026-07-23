using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNConnectWebsocketClient(BNWebsocketClient* client, const char* url, uint64_t headerCount, const char** headerKeys, const char** headerValues, BNWebsocketClientOutputCallbacks* callbacks)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNConnectWebsocketClient"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNConnectWebsocketClient(
			
			// BNWebsocketClient* client
		    IntPtr client  , 
			
			// const char* url
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string url  , 
			
			// uint64_t headerCount
		    ulong headerCount  , 
			
			// const char** headerKeys
		    IntPtr headerKeys  ,
			
			// const char** headerValues
		    IntPtr headerValues  ,
			
			// BNWebsocketClientOutputCallbacks* callbacks
		    in BNWebsocketClientOutputCallbacks callbacks
		);
	}
}
