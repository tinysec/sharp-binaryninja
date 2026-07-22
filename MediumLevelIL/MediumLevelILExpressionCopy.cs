using System;

namespace BinaryNinja
{
    /// <summary>
    /// Expression-copy support for writable MLIL functions.
    /// </summary>
    public sealed partial class MediumLevelILFunction
    {
        /// <summary>
        /// Adds a shallow copy of an expression from this function.
        /// Child-expression indexes remain shared, and expression metadata is not copied.
        /// </summary>
        public MediumLevelILExpressionIndex CopyExpression(MediumLevelILInstruction original)
        {
            if (null == original)
            {
                throw new ArgumentNullException(nameof(original));
            }

            if (this.DangerousGetHandle() != original.ILFunction.DangerousGetHandle())
            {
                throw new ArgumentException(
                    "A shallow expression copy must remain in the source MLIL function.",
                    nameof(original));
            }

            SourceLocation location = new SourceLocation(
                original.Address,
                original.SourceOperand);

            return this.AddExpression(
                original.Operation,
                location,
                original.Size,
                original.RawOperands);
        }
    }
}
