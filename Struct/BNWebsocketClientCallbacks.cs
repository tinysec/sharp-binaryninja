using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    internal static partial class NativeDelegates
    {
        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate void BNWebsocketDestroyClient(IntPtr context);

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal delegate bool BNWebsocketConnect(
            IntPtr context,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string host,
            ulong headerCount,
            IntPtr headerKeys,
            IntPtr headerValues
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal delegate bool BNWebsocketWrite(
            IntPtr data,
            ulong length,
            IntPtr context
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal delegate bool BNWebsocketDisconnect(IntPtr context);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BNWebsocketClientCallbacks
    {
        internal IntPtr context;
        internal IntPtr destroyClient;
        internal IntPtr connect;
        internal IntPtr write;
        internal IntPtr disconnect;
    }

    /// <summary>Retained for source compatibility.</summary>
    public class WebsocketClientCallbacks
    {
    }
}
