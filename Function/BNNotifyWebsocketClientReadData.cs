using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNNotifyWebsocketClientReadData(BNWebsocketClient* client, uint8_t* data, uint64_t len)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNNotifyWebsocketClientReadData"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNNotifyWebsocketClientReadData(
			
			// BNWebsocketClient* client
		    IntPtr client  , 
			
			// uint8_t* data
		    IntPtr data  , 
			
			// uint64_t len
		    ulong len  
			
		);
	}
}