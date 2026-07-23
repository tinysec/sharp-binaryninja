using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    internal static partial class NativeDelegates
    {
        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate void BNScriptingOutput(
            IntPtr context,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string text
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate void BNScriptingInputReadyStateChanged(
            IntPtr context,
            ScriptingProviderInputReadyState state
        );
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BNScriptingOutputListener
    {
        internal IntPtr context;

        internal IntPtr output;

        internal IntPtr warning;

        internal IntPtr error;

        internal IntPtr inputReadyStateChanged;
    }
}
