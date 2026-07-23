using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    internal static partial class NativeDelegates
    {
        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate void BNInteractionReport(
            IntPtr context,
            IntPtr view,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string title,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string contents
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate void BNInteractionRichReport(
            IntPtr context,
            IntPtr view,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string title,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string contents,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string plainText
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate void BNInteractionGraphReport(
            IntPtr context,
            IntPtr view,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string title,
            IntPtr graph
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate void BNInteractionReportCollection(
            IntPtr context,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string title,
            IntPtr reports
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal delegate bool BNInteractionStringInput(
            IntPtr context,
            IntPtr result,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string prompt,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string value
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal delegate bool BNInteractionIntegerInput(
            IntPtr context,
            IntPtr result,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string prompt,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string title
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal delegate bool BNInteractionAddressInput(
            IntPtr context,
            IntPtr result,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string prompt,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string title,
            IntPtr view,
            ulong currentAddress
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal delegate bool BNInteractionChoiceInput(
            IntPtr context,
            IntPtr result,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string prompt,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string title,
            IntPtr choices,
            UIntPtr count
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal delegate bool BNInteractionSaveFileInput(
            IntPtr context,
            IntPtr result,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string prompt,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string extension,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string defaultName
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal delegate bool BNInteractionCheckboxInput(
            IntPtr context,
            IntPtr result,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string prompt,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string title,
            IntPtr defaultChoice
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal delegate bool BNInteractionFormInput(
            IntPtr context,
            IntPtr fields,
            UIntPtr count,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string title
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate MessageBoxButtonResult BNInteractionMessageBox(
            IntPtr context,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string title,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string text,
            MessageBoxButtonSet buttons,
            MessageBoxIcon icon
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal delegate bool BNInteractionOpenUrl(
            IntPtr context,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string url
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal delegate bool BNInteractionProgressDialog(
            IntPtr context,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string title,
            [MarshalAs(UnmanagedType.I1)] bool canCancel,
            IntPtr task,
            IntPtr taskContext
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate void BNInteractionProgressTask(
            IntPtr taskContext,
            IntPtr progress,
            IntPtr progressContext
        );
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BNInteractionHandlerCallbacks
    {
        internal IntPtr context;
        internal IntPtr showPlainTextReport;
        internal IntPtr showMarkdownReport;
        internal IntPtr showHTMLReport;
        internal IntPtr showGraphReport;
        internal IntPtr showReportCollection;
        internal IntPtr getTextLineInput;
        internal IntPtr getIntegerInput;
        internal IntPtr getAddressInput;
        internal IntPtr getChoiceInput;
        internal IntPtr getLargeChoiceInput;
        internal IntPtr getOpenFileNameInput;
        internal IntPtr getSaveFileNameInput;
        internal IntPtr getDirectoryNameInput;
        internal IntPtr getCheckboxInput;
        internal IntPtr getFormInput;
        internal IntPtr showMessageBox;
        internal IntPtr openUrl;
        internal IntPtr runProgressDialog;
    }

    /// <summary>Retained for source compatibility.</summary>
    public class InteractionHandlerCallbacks
    {
    }
}
