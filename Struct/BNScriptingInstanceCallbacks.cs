using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    internal static partial class NativeDelegates
    {
        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate void BNScriptingInstanceEvent(IntPtr context);

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate ScriptingProviderExecuteResult
            BNScriptingInstanceExecuteInput(
                IntPtr context,
                [MarshalAs(UnmanagedType.LPUTF8Str)] string input
            );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate void BNScriptingInstanceSetObject(
            IntPtr context,
            IntPtr value
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate void BNScriptingInstanceSetAddress(
            IntPtr context,
            ulong address
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate void BNScriptingInstanceSetSelection(
            IntPtr context,
            ulong begin,
            ulong end
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate IntPtr BNScriptingInstanceCompleteInput(
            IntPtr context,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string text,
            ulong state
        );
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BNScriptingInstanceCallbacks
    {
        internal IntPtr context;

        internal IntPtr destroyInstance;

        internal IntPtr externalRefTaken;

        internal IntPtr externalRefReleased;

        internal IntPtr executeScriptInput;

        internal IntPtr executeScriptInputFromFilename;

        internal IntPtr cancelScriptInput;

        internal IntPtr releaseBinaryView;

        internal IntPtr setCurrentBinaryView;

        internal IntPtr setCurrentFunction;

        internal IntPtr setCurrentBasicBlock;

        internal IntPtr setCurrentAddress;

        internal IntPtr setCurrentSelection;

        internal IntPtr completeInput;

        internal IntPtr stop;
    }

    /// <summary>
    /// Retained for source compatibility. Custom instances use ScriptingInstance directly.
    /// </summary>
    public class ScriptingInstanceCallbacks
    {
    }
}
