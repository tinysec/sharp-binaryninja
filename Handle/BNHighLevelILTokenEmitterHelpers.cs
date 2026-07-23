using System;

namespace BinaryNinja
{
    public sealed partial class HighLevelILTokenEmitter
    {
        /// <summary>Appends each token in order.</summary>
        public void Append(InstructionTextToken[] tokens)
        {
            if (null == tokens)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            foreach (InstructionTextToken token in tokens)
            {
                this.Append(token);
            }
        }

        /// <summary>Appends an integer size token.</summary>
        public void AppendSizeToken(
            ulong size,
            InstructionTextTokenType type
        )
        {
            NativeMethods.BNAddHighLevelILSizeToken(size, type, this.handle);
        }

        /// <summary>Appends a floating-point size token.</summary>
        public void AppendFloatSizeToken(
            ulong size,
            InstructionTextTokenType type
        )
        {
            NativeMethods.BNAddHighLevelILFloatSizeToken(size, type, this.handle);
        }

        /// <summary>Appends tokens for a variable access.</summary>
        public unsafe void AppendVarTextToken(
            CoreVariable variable,
            HighLevelILInstruction instruction,
            ulong size
        )
        {
            if (null == variable)
            {
                throw new ArgumentNullException(nameof(variable));
            }

            BNVariable native = variable.ToNative();
            NativeMethods.BNAddHighLevelILVarTextToken(
                GetInstructionFunctionHandle(instruction),
                (IntPtr)(&native),
                this.handle,
                (ulong)instruction.ExpressionIndex,
                size
            );
        }

        /// <summary>Appends tokens for an integer value.</summary>
        public void AppendIntegerTextToken(
            HighLevelILInstruction instruction,
            long value,
            ulong size
        )
        {
            NativeMethods.BNAddHighLevelILIntegerTextToken(
                GetInstructionFunctionHandle(instruction),
                (ulong)instruction.ExpressionIndex,
                value,
                size,
                this.handle
            );
        }

        /// <summary>Appends tokens for an array index.</summary>
        public void AppendArrayIndexToken(
            HighLevelILInstruction instruction,
            long value,
            ulong size,
            ulong address = 0
        )
        {
            NativeMethods.BNAddHighLevelILArrayIndexToken(
                GetInstructionFunctionHandle(instruction),
                (ulong)instruction.ExpressionIndex,
                value,
                size,
                this.handle,
                address
            );
        }

        /// <summary>Appends tokens for a pointer value.</summary>
        public SymbolDisplayResult AppendPointerTextToken(
            HighLevelILInstruction instruction,
            long value,
            DisassemblySettings? settings,
            SymbolDisplayType symbolDisplay,
            OperatorPrecedence precedence,
            bool allowShortString = false
        )
        {
            return NativeMethods.BNAddHighLevelILPointerTextToken(
                GetInstructionFunctionHandle(instruction),
                (ulong)instruction.ExpressionIndex,
                value,
                this.handle,
                null == settings
                    ? IntPtr.Zero
                    : settings.DangerousGetHandle(),
                symbolDisplay,
                precedence,
                allowShortString
            );
        }

        /// <summary>Appends tokens for a constant value.</summary>
        public void AppendConstantTextToken(
            HighLevelILInstruction instruction,
            long value,
            ulong size,
            DisassemblySettings? settings,
            OperatorPrecedence precedence
        )
        {
            NativeMethods.BNAddHighLevelILConstantTextToken(
                GetInstructionFunctionHandle(instruction),
                (ulong)instruction.ExpressionIndex,
                value,
                size,
                this.handle,
                null == settings
                    ? IntPtr.Zero
                    : settings.DangerousGetHandle(),
                precedence
            );
        }

        /// <summary>Gets names for outer structure members.</summary>
        public static unsafe string[] GetNamesForOuterStructureMembers(
            BinaryView view,
            BinaryNinja.Type type,
            HighLevelILInstruction variable
        )
        {
            if (null == view)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (null == type)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (null == variable)
            {
                throw new ArgumentNullException(nameof(variable));
            }

            ulong count = 0;
            IntPtr names = NativeMethods.BNAddNamesForOuterStructureMembers(
                view.DangerousGetHandle(),
                type.DangerousGetHandle(),
                GetInstructionFunctionHandle(variable),
                (ulong)variable.ExpressionIndex,
                (IntPtr)(&count)
            );

            return UnsafeUtils.TakeUtf8StringArray(
                names,
                count,
                NativeMethods.BNFreeStringList
            );
        }

        /// <summary>Gets names for outer structure members.</summary>
        public static string[] AddNamesForOuterStructureMembers(
            BinaryView view,
            BinaryNinja.Type type,
            HighLevelILInstruction variable
        )
        {
            return HighLevelILTokenEmitter.GetNamesForOuterStructureMembers(
                view,
                type,
                variable
            );
        }

        private static IntPtr GetInstructionFunctionHandle(
            HighLevelILInstruction instruction
        )
        {
            if (null == instruction)
            {
                throw new ArgumentNullException(nameof(instruction));
            }

            return instruction.ILFunction.DangerousGetHandle();
        }
    }
}
