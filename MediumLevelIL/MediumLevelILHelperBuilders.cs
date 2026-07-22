using System;

namespace BinaryNinja
{
	public sealed partial class MediumLevelILFunction
	{
		/// <summary>
		/// Creates the address of a field within a variable.
		/// </summary>
		public MediumLevelILExpressionIndex EmitAddressOfField(
			Variable variable,
			ulong offset,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_ADDRESS_OF_FIELD,
				location,
				0,
				variable.Identifier,
				offset);
		}

		/// <summary>
		/// Creates an indirect jump expression. MLIL jumps have no result size.
		/// </summary>
		public MediumLevelILExpressionIndex EmitJump(
			MediumLevelILExpressionIndex destination,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_JUMP,
				location,
				0,
				(ulong)destination);
		}

		/// <summary>
		/// Creates a floating-point constant from its raw binary representation.
		/// </summary>
		public MediumLevelILExpressionIndex EmitFloatConstRaw(
			ulong size,
			ulong value,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_FLOAT_CONST,
				location,
				size,
				value);
		}

		/// <summary>
		/// Creates a single-precision floating-point constant.
		/// </summary>
		public MediumLevelILExpressionIndex EmitFloatConstSingle(
			float value,
			SourceLocation? location = null)
		{
			uint bits = BitConverter.SingleToUInt32Bits(value);

			return this.EmitFloatConstRaw(4, bits, location);
		}

		/// <summary>
		/// Creates a double-precision floating-point constant.
		/// </summary>
		public MediumLevelILExpressionIndex EmitFloatConstDouble(
			double value,
			SourceLocation? location = null)
		{
			ulong bits = BitConverter.DoubleToUInt64Bits(value);

			return this.EmitFloatConstRaw(8, bits, location);
		}

		/// <summary>
		/// Creates a helper expression holding a separately allocated parameter list.
		/// </summary>
		public MediumLevelILExpressionIndex EmitSeparateParamList(
			MediumLevelILExpressionIndex[] parameters,
			SourceLocation? location = null)
		{
			if (null == parameters)
			{
				throw new ArgumentNullException(nameof(parameters));
			}

			return this.AddExpression(
				MediumLevelILOperation.MLIL_SEPARATE_PARAM_LIST,
				location,
				0,
				(ulong)parameters.Length,
				(ulong)this.AddOperandList(parameters));
		}

		/// <summary>
		/// Creates a helper expression holding parameters that share a parameter slot.
		/// </summary>
		public MediumLevelILExpressionIndex EmitSharedParamSlot(
			MediumLevelILExpressionIndex[] parameters,
			SourceLocation? location = null)
		{
			if (null == parameters)
			{
				throw new ArgumentNullException(nameof(parameters));
			}

			return this.AddExpression(
				MediumLevelILOperation.MLIL_SHARED_PARAM_SLOT,
				location,
				0,
				(ulong)parameters.Length,
				(ulong)this.AddOperandList(parameters));
		}
	}
}
