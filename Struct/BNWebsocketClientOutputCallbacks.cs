using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    internal static partial class NativeDelegates
    {
        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal delegate bool BNWebsocketConnected(IntPtr context);

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate void BNWebsocketDisconnected(IntPtr context);

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate void BNWebsocketError(
            [MarshalAs(UnmanagedType.LPUTF8Str)] string message,
            IntPtr context
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal delegate bool BNWebsocketRead(
            IntPtr data,
            ulong length,
            IntPtr context
        );
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BNWebsocketClientOutputCallbacks
    {
        internal IntPtr context;
        internal IntPtr connectedCallback;
        internal IntPtr disconnectedCallback;
        internal IntPtr errorCallback;
        internal IntPtr readCallback;
    }

    /// <summary>Retained for source compatibility.</summary>
    public class WebsocketClientOutputCallbacks
    {
    }
}
