using System;

namespace BinaryNinja
{
	public sealed partial class MediumLevelILFunction
	{
		/// <summary>
		/// Gets the variable-length operand list stored at an expression operand.
		/// </summary>
		public ulong[] GetOperandList(
			MediumLevelILExpressionIndex expression,
			ulong listOperand)
		{
			IntPtr arrayPointer = NativeMethods.BNMediumLevelILGetOperandList(
				this.handle,
				expression,
				listOperand,
				out ulong arrayLength);

			return UnsafeUtils.TakeNumberArray<ulong>(
				arrayPointer,
				arrayLength,
				NativeMethods.BNMediumLevelILFreeOperandList);
		}
	}
}
