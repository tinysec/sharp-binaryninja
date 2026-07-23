using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    public abstract partial class LanguageRepresentationFunction
    {
        /// <summary>Gets the highlight color for a native function basic block.</summary>
        public HighlightColor GetHighlight(BasicBlock block)
        {
            if (null == block)
            {
                throw new ArgumentNullException(nameof(block));
            }

            return HighlightColor.FromNative(
                NativeMethods.BNGetLanguageRepresentationFunctionHighlight(
                    this.handle,
                    block.DangerousGetHandle()
                )
            );
        }

        /// <summary>Gets the highlight color for a high-level IL basic block.</summary>
        public HighlightColor GetHighlight(HighLevelILBasicBlock block)
        {
            if (null == block)
            {
                throw new ArgumentNullException(nameof(block));
            }

            return HighlightColor.FromNative(
                NativeMethods.BNGetLanguageRepresentationFunctionHighlight(
                    this.handle,
                    block.DangerousGetHandle()
                )
            );
        }

        /// <summary>Gets the language type associated with this function.</summary>
        public LanguageRepresentationFunctionType? GetLanguageType()
        {
            return LanguageRepresentationFunctionType.BorrowHandle(
                NativeMethods.BNGetLanguageRepresentationType(this.handle)
            );
        }

        /// <summary>Looks up a registered language type by name.</summary>
        public static LanguageRepresentationFunctionType? GetLanguageTypeByName(
            string name
        )
        {
            return LanguageRepresentationFunctionType.GetByName(name);
        }

        /// <summary>Gets language-specific line formatter settings.</summary>
        public LineFormatterSettings GetLineFormatterSettings(
            DisassemblySettings settings
        )
        {
            if (null == settings)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            IntPtr pointer =
                NativeMethods.BNGetLanguageRepresentationLineFormatterSettings(
                    settings.DangerousGetHandle(),
                    this.handle
                );
            if (IntPtr.Zero == pointer)
            {
                return new LineFormatterSettings();
            }

            try
            {
                BNLineFormatterSettings native =
                    Marshal.PtrToStructure<BNLineFormatterSettings>(pointer);
                LineFormatterSettings result = new LineFormatterSettings();
                result.DesiredLineLength = native.desiredLineLength;
                result.MinimumContentLength = native.minimumContentLength;
                result.TabWidth = native.tabWidth;
                result.MaximumAnnotationLength = native.maximumAnnotationLength;
                result.StringWrappingWidth = native.stringWrappingWidth;
                result.LanguageName = UnsafeUtils.ReadUtf8String(
                    native.languageName
                );
                result.CommentStartString = UnsafeUtils.ReadUtf8String(
                    native.commentStartString
                );
                result.CommentEndString = UnsafeUtils.ReadUtf8String(
                    native.commentEndString
                );
                result.AnnotationStartString = UnsafeUtils.ReadUtf8String(
                    native.annotationStartString
                );
                result.AnnotationEndString = UnsafeUtils.ReadUtf8String(
                    native.annotationEndString
                );
                result.HighLevelIL = HighLevelILFunction.NewFromHandle(
                    native.highLevelIL
                );

                return result;
            }
            finally
            {
                NativeMethods.BNFreeLineFormatterSettings(pointer);
            }
        }
    }
}
