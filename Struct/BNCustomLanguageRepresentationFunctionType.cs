using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    internal static partial class NativeDelegates
    {
        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate IntPtr BNLanguageRepresentationCreate(
            IntPtr context,
            IntPtr architecture,
            IntPtr owner,
            IntPtr highLevelIL
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal delegate bool BNLanguageRepresentationIsValid(
            IntPtr context,
            IntPtr view
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate IntPtr BNLanguageRepresentationGetObject(IntPtr context);

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate IntPtr BNLanguageRepresentationGetFunctionTypeTokens(
            IntPtr context,
            IntPtr function,
            IntPtr settings,
            IntPtr count
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate void BNLanguageRepresentationFreeLines(
            IntPtr context,
            IntPtr lines,
            UIntPtr count
        );
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BNCustomLanguageRepresentationFunctionType
    {
        internal IntPtr context;

        internal IntPtr create;

        internal IntPtr isValid;

        internal IntPtr getTypePrinter;

        internal IntPtr getTypeParser;

        internal IntPtr getLineFormatter;

        internal IntPtr getFunctionTypeTokens;

        internal IntPtr freeLines;
    }

    /// <summary>
    /// Retained for source compatibility. Custom types use the managed type directly.
    /// </summary>
    public class CustomLanguageRepresentationFunctionType
    {
    }
}
