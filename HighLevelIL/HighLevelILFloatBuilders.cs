namespace BinaryNinja
{
	public sealed partial class HighLevelILFunction
	{
		public HighLevelILExpressionIndex EmitFloatAdd(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_FADD, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitFloatSub(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_FSUB, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitFloatMul(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_FMUL, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitFloatDiv(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_FDIV, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitFloatSquareRoot(
			ulong size,
			HighLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.EmitUnaryOperation(
				HighLevelILOperation.HLIL_FSQRT, size, source, location);
		}

		public HighLevelILExpressionIndex EmitFloatNeg(
			ulong size,
			HighLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.EmitUnaryOperation(
				HighLevelILOperation.HLIL_FNEG, size, source, location);
		}

		public HighLevelILExpressionIndex EmitFloatAbs(
			ulong size,
			HighLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.EmitUnaryOperation(
				HighLevelILOperation.HLIL_FABS, size, source, location);
		}

		public HighLevelILExpressionIndex EmitFloatToInt(
			ulong size,
			HighLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.EmitUnaryOperation(
				HighLevelILOperation.HLIL_FLOAT_TO_INT, size, source, location);
		}

		public HighLevelILExpressionIndex EmitIntToFloat(
			ulong size,
			HighLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.EmitUnaryOperation(
				HighLevelILOperation.HLIL_INT_TO_FLOAT, size, source, location);
		}

		public HighLevelILExpressionIndex EmitFloatConvert(
			ulong size,
			HighLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.EmitUnaryOperation(
				HighLevelILOperation.HLIL_FLOAT_CONV, size, source, location);
		}

		public HighLevelILExpressionIndex EmitRoundToInt(
			ulong size,
			HighLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.EmitUnaryOperation(
				HighLevelILOperation.HLIL_ROUND_TO_INT, size, source, location);
		}

		public HighLevelILExpressionIndex EmitFloor(
			ulong size,
			HighLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.EmitUnaryOperation(
				HighLevelILOperation.HLIL_FLOOR, size, source, location);
		}

		public HighLevelILExpressionIndex EmitCeil(
			ulong size,
			HighLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.EmitUnaryOperation(
				HighLevelILOperation.HLIL_CEIL, size, source, location);
		}

		public HighLevelILExpressionIndex EmitFloatTrunc(
			ulong size,
			HighLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.EmitUnaryOperation(
				HighLevelILOperation.HLIL_FTRUNC, size, source, location);
		}

		public HighLevelILExpressionIndex EmitFloatEqual(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_FCMP_E, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitFloatNotEqual(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_FCMP_NE, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitFloatLessThan(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_FCMP_LT, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitFloatLessThanOrEqual(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_FCMP_LE, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitFloatGreaterThanOrEqual(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_FCMP_GE, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitFloatGreaterThan(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_FCMP_GT, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitFloatOrdered(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_FCMP_O, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitFloatUnordered(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_FCMP_UO, size, left, right, location);
		}
	}
}
