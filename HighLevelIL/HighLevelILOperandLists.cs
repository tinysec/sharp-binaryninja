using System;

namespace BinaryNinja
{
	public sealed partial class HighLevelILFunction
	{
		/// <summary>
		/// Gets the variable-length operand list stored at an expression operand.
		/// </summary>
		public ulong[] GetOperandList(
			HighLevelILExpressionIndex expression,
			ulong listOperand)
		{
			IntPtr arrayPointer = NativeMethods.BNHighLevelILGetOperandList(
				this.handle,
				expression,
				listOperand,
				out ulong arrayLength);

			return UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer,
				arrayLength,
				NativeMethods.BNHighLevelILFreeOperandList);
		}
	}
}
