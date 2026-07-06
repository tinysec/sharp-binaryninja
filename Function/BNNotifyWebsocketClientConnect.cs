using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNNotifyWebsocketClientConnect(BNWebsocketClient* client)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNNotifyWebsocketClientConnect"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNNotifyWebsocketClientConnect(
			
			// BNWebsocketClient* client
		    IntPtr client  
			
		);
	}
}