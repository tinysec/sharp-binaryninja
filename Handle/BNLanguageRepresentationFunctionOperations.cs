using System;

namespace BinaryNinja
{
    public abstract partial class LanguageRepresentationFunction
    {
        /// <summary>Gets rendered lines for one high-level IL expression.</summary>
        public DisassemblyTextLine[] GetExpressionText(
            HighLevelILInstruction instruction,
            DisassemblySettings? settings = null,
            OperatorPrecedence precedence = OperatorPrecedence.TopLevelOperatorPrecedence,
            bool statement = false
        )
        {
            IntPtr lines = NativeMethods.BNGetLanguageRepresentationFunctionExprText(
                this.handle,
                instruction.ILFunction.DangerousGetHandle(),
                instruction.ExpressionIndex,
                null == settings
                    ? IntPtr.Zero
                    : settings.DangerousGetHandle(),
                instruction.AsFullAst,
                precedence,
                statement,
                out ulong count
            );

            return UnsafeUtils.TakeStructArrayEx<
                BNDisassemblyTextLine,
                DisassemblyTextLine
            >(
                lines,
                count,
                DisassemblyTextLine.FromNative,
                NativeMethods.BNFreeDisassemblyTextLines
            );
        }

        /// <summary>Gets linear-view lines for one high-level IL expression.</summary>
        public DisassemblyTextLine[] GetLinearLines(
            HighLevelILInstruction instruction,
            DisassemblySettings? settings = null
        )
        {
            IntPtr lines =
                NativeMethods.BNGetLanguageRepresentationFunctionLinearLines(
                    this.handle,
                    instruction.ILFunction.DangerousGetHandle(),
                    instruction.ExpressionIndex,
                    null == settings
                        ? IntPtr.Zero
                        : settings.DangerousGetHandle(),
                    instruction.AsFullAst,
                    out ulong count
                );

            return UnsafeUtils.TakeStructArrayEx<
                BNDisassemblyTextLine,
                DisassemblyTextLine
            >(
                lines,
                count,
                DisassemblyTextLine.FromNative,
                NativeMethods.BNFreeDisassemblyTextLines
            );
        }

        /// <summary>Gets rendered lines for one high-level IL basic block.</summary>
        public DisassemblyTextLine[] GetBlockLines(
            HighLevelILBasicBlock block,
            DisassemblySettings? settings = null
        )
        {
            IntPtr lines = NativeMethods.BNGetLanguageRepresentationFunctionBlockLines(
                this.handle,
                block.DangerousGetHandle(),
                null == settings
                    ? IntPtr.Zero
                    : settings.DangerousGetHandle(),
                out ulong count
            );

            return UnsafeUtils.TakeStructArrayEx<
                BNDisassemblyTextLine,
                DisassemblyTextLine
            >(
                lines,
                count,
                DisassemblyTextLine.FromNative,
                NativeMethods.BNFreeDisassemblyTextLines
            );
        }
    }
}
