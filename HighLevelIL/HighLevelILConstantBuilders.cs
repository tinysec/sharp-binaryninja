using System;

namespace BinaryNinja
{
	public sealed partial class HighLevelILFunction
	{
		public HighLevelILExpressionIndex EmitConst(
			ulong size,
			ulong value,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_CONST,
				location,
				size,
				value);
		}

		public HighLevelILExpressionIndex EmitConstPointer(
			ulong size,
			ulong value,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_CONST_PTR,
				location,
				size,
				value);
		}

		public HighLevelILExpressionIndex EmitExternPointer(
			ulong size,
			ulong value,
			ulong offset,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_EXTERN_PTR,
				location,
				size,
				value,
				offset);
		}

		public HighLevelILExpressionIndex EmitFloatConstRaw(
			ulong size,
			ulong value,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_FLOAT_CONST,
				location,
				size,
				value);
		}

		public HighLevelILExpressionIndex EmitFloatConstSingle(
			float value,
			SourceLocation? location = null)
		{
			uint bits = BitConverter.SingleToUInt32Bits(value);

			return this.EmitFloatConstRaw(4, bits, location);
		}

		public HighLevelILExpressionIndex EmitFloatConstDouble(
			double value,
			SourceLocation? location = null)
		{
			ulong bits = BitConverter.DoubleToUInt64Bits(value);

			return this.EmitFloatConstRaw(8, bits, location);
		}

		public HighLevelILExpressionIndex EmitImportedAddress(
			ulong size,
			ulong value,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_IMPORT,
				location,
				size,
				value);
		}

		public HighLevelILExpressionIndex EmitImport(
			ulong size,
			ulong value,
			SourceLocation? location = null)
		{
			return this.EmitImportedAddress(size, value, location);
		}

		public HighLevelILExpressionIndex EmitConstData(
			ulong size,
			RegisterValue data,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_CONST_DATA,
				location,
				size,
				(ulong)data.State,
				(ulong)data.Value);
		}
	}
}
