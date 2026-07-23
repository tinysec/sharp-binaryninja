using System;

namespace BinaryNinja
{
	public sealed partial class LowLevelILFunction
	{
		/// <summary>
		/// Gets the variable-length operand list stored at an expression operand.
		/// </summary>
		public ulong[] GetOperandList(
			LowLevelILExpressionIndex expression,
			ulong listOperand)
		{
			IntPtr arrayPointer = NativeMethods.BNLowLevelILGetOperandList(
				this.handle,
				expression,
				listOperand,
				out ulong arrayLength);

			return UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer,
				arrayLength,
				NativeMethods.BNLowLevelILFreeOperandList);
		}
	}
}
