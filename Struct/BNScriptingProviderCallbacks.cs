using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    internal static partial class NativeDelegates
    {
        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate IntPtr BNScriptingProviderCreateInstance(
            IntPtr context
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal delegate bool BNScriptingProviderLoadModule(
            IntPtr context,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string repository,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string module,
            [MarshalAs(UnmanagedType.I1)] bool force
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal delegate bool BNScriptingProviderInstallModules(
            IntPtr context,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string modules
        );
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BNScriptingProviderCallbacks
    {
        internal IntPtr context;

        internal IntPtr createInstance;

        internal IntPtr loadModule;

        internal IntPtr installModules;
    }

    /// <summary>
    /// Retained for source compatibility. Custom providers use ScriptingProvider directly.
    /// </summary>
    public class ScriptingProviderCallbacks
    {
    }
}
