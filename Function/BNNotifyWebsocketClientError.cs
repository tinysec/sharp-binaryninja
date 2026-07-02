using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNNotifyWebsocketClientError(BNWebsocketClient* client, const char* msg)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNNotifyWebsocketClientError"
        )]
		internal static extern void BNNotifyWebsocketClientError(
			
			// BNWebsocketClient* client
		    IntPtr client  , 
			
			// const char* msg
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string msg  
			
		);
	}
}