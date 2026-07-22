namespace BinaryNinja
{
	public sealed partial class HighLevelILFunction
	{
		private HighLevelILExpressionIndex EmitUnaryOperation(
			HighLevelILOperation operation,
			ulong size,
			HighLevelILExpressionIndex source,
			SourceLocation? location)
		{
			return this.AddExpression(operation, location, size, (ulong)source);
		}

		private HighLevelILExpressionIndex EmitBinaryOperation(
			HighLevelILOperation operation,
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location)
		{
			return this.AddExpression(
				operation,
				location,
				size,
				(ulong)left,
				(ulong)right);
		}

		private HighLevelILExpressionIndex EmitTernaryOperation(
			HighLevelILOperation operation,
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			HighLevelILExpressionIndex carry,
			SourceLocation? location)
		{
			return this.AddExpression(
				operation,
				location,
				size,
				(ulong)left,
				(ulong)right,
				(ulong)carry);
		}

		public HighLevelILExpressionIndex EmitAdd(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_ADD,
				size,
				left,
				right,
				location);
		}

		public HighLevelILExpressionIndex EmitAddCarry(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			HighLevelILExpressionIndex carry,
			SourceLocation? location = null)
		{
			return this.EmitTernaryOperation(
				HighLevelILOperation.HLIL_ADC,
				size,
				left,
				right,
				carry,
				location);
		}

		public HighLevelILExpressionIndex EmitSub(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_SUB,
				size,
				left,
				right,
				location);
		}

		public HighLevelILExpressionIndex EmitSubBorrow(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			HighLevelILExpressionIndex carry,
			SourceLocation? location = null)
		{
			return this.EmitTernaryOperation(
				HighLevelILOperation.HLIL_SBB,
				size,
				left,
				right,
				carry,
				location);
		}

		public HighLevelILExpressionIndex EmitAnd(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_AND, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitOr(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_OR, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitXor(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_XOR, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitShiftLeft(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_LSL, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitLogicalShiftRight(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_LSR, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitArithShiftRight(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_ASR, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitRotateLeft(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_ROL, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitRotateLeftCarry(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			HighLevelILExpressionIndex carry,
			SourceLocation? location = null)
		{
			return this.EmitTernaryOperation(
				HighLevelILOperation.HLIL_RLC,
				size,
				left,
				right,
				carry,
				location);
		}

		public HighLevelILExpressionIndex EmitRotateRight(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_ROR, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitRotateRightCarry(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			HighLevelILExpressionIndex carry,
			SourceLocation? location = null)
		{
			return this.EmitTernaryOperation(
				HighLevelILOperation.HLIL_RRC,
				size,
				left,
				right,
				carry,
				location);
		}

		public HighLevelILExpressionIndex EmitMul(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_MUL, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitMulDoublePrecUnsigned(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_MULU_DP, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitMulDoublePrecSigned(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_MULS_DP, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitDivUnsigned(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_DIVU, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitDivDoublePrecUnsigned(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_DIVU_DP, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitDivSigned(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_DIVS, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitDivDoublePrecSigned(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_DIVS_DP, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitModUnsigned(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_MODU, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitModDoublePrecUnsigned(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_MODU_DP, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitModSigned(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_MODS, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitModDoublePrecSigned(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_MODS_DP, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitNeg(
			ulong size,
			HighLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.EmitUnaryOperation(
				HighLevelILOperation.HLIL_NEG, size, source, location);
		}

		public HighLevelILExpressionIndex EmitNot(
			ulong size,
			HighLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.EmitUnaryOperation(
				HighLevelILOperation.HLIL_NOT, size, source, location);
		}

		public HighLevelILExpressionIndex EmitSignExtend(
			ulong size,
			HighLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.EmitUnaryOperation(
				HighLevelILOperation.HLIL_SX, size, source, location);
		}

		public HighLevelILExpressionIndex EmitZeroExtend(
			ulong size,
			HighLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.EmitUnaryOperation(
				HighLevelILOperation.HLIL_ZX, size, source, location);
		}

		public HighLevelILExpressionIndex EmitLowPart(
			ulong size,
			HighLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.EmitUnaryOperation(
				HighLevelILOperation.HLIL_LOW_PART, size, source, location);
		}

		public HighLevelILExpressionIndex EmitEqual(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_CMP_E, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitNotEqual(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_CMP_NE, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitSignedLessThan(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_CMP_SLT, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitUnsignedLessThan(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_CMP_ULT, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitSignedLessThanOrEqual(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_CMP_SLE, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitUnsignedLessThanOrEqual(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_CMP_ULE, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitSignedGreaterThanOrEqual(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_CMP_SGE, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitUnsignedGreaterThanOrEqual(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_CMP_UGE, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitSignedGreaterThan(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_CMP_SGT, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitUnsignedGreaterThan(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_CMP_UGT, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitTestBit(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_TEST_BIT, size, left, right, location);
		}

		public HighLevelILExpressionIndex EmitBoolToInt(
			ulong size,
			HighLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.EmitUnaryOperation(
				HighLevelILOperation.HLIL_BOOL_TO_INT, size, source, location);
		}

		public HighLevelILExpressionIndex EmitAddOverflow(
			ulong size,
			HighLevelILExpressionIndex left,
			HighLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitBinaryOperation(
				HighLevelILOperation.HLIL_ADD_OVERFLOW, size, left, right, location);
		}
	}
}
