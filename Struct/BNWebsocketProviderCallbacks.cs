using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    internal static partial class NativeDelegates
    {
        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate IntPtr BNWebsocketCreateClient(IntPtr context);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BNWebsocketProviderCallbacks
    {
        internal IntPtr context;
        internal IntPtr createClient;
    }

    /// <summary>Retained for source compatibility.</summary>
    public class WebsocketProviderCallbacks
    {
    }
}
