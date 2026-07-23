using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    internal static partial class NativeDelegates
    {
        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate void BNLanguageRepresentationFunctionEvent(
            IntPtr context
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate void BNLanguageRepresentationTokenEmitter(
            IntPtr context,
            IntPtr emitter
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate void BNLanguageRepresentationExpression(
            IntPtr context,
            IntPtr highLevelIL,
            UIntPtr expressionIndex,
            IntPtr emitter,
            IntPtr settings,
            [MarshalAs(UnmanagedType.I1)] bool asFullAst,
            OperatorPrecedence precedence,
            [MarshalAs(UnmanagedType.I1)] bool statement
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate void BNLanguageRepresentationLines(
            IntPtr context,
            IntPtr highLevelIL,
            UIntPtr expressionIndex,
            IntPtr emitter
        );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        internal delegate IntPtr BNLanguageRepresentationString(IntPtr context);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BNCustomLanguageRepresentationFunction
    {
        internal IntPtr context;

        internal IntPtr freeObject;

        internal IntPtr externalRefTaken;

        internal IntPtr externalRefReleased;

        internal IntPtr initTokenEmitter;

        internal IntPtr getExprText;

        internal IntPtr beginLines;

        internal IntPtr endLines;

        internal IntPtr getCommentStartString;

        internal IntPtr getCommentEndString;

        internal IntPtr getAnnotationStartString;

        internal IntPtr getAnnotationEndString;
    }

    /// <summary>
    /// Retained for source compatibility. Custom functions use the managed type directly.
    /// </summary>
    public class CustomLanguageRepresentationFunction
    {
    }
}
