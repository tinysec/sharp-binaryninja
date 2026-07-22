using System;

namespace BinaryNinja
{
	public sealed partial class MediumLevelILFunction
	{
		/// <summary>
		/// Compatibility alias for the former misspelled rotate-right builder.
		/// </summary>
		[Obsolete("Use EmitRotateRight instead.")]
		public MediumLevelILExpressionIndex EmitRorateRight(
			ulong size,
			MediumLevelILExpressionIndex left,
			MediumLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitRotateRight(size, left, right, location);
		}

		/// <summary>
		/// Compatibility alias for the former all-capitals multiply builder.
		/// </summary>
		[Obsolete("Use EmitMul instead.")]
		public MediumLevelILExpressionIndex EmitMUL(
			ulong size,
			MediumLevelILExpressionIndex left,
			MediumLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.EmitMul(size, left, right, location);
		}

		/// <summary>
		/// Compatibility alias for the former all-capitals jump builder.
		/// </summary>
		[Obsolete("Use EmitJump instead.")]
		public MediumLevelILExpressionIndex EmitJUMP(
			ulong size,
			MediumLevelILExpressionIndex destination,
			SourceLocation? location = null)
		{
			return this.EmitJump(size, destination, location);
		}

		/// <summary>
		/// Compatibility alias for the former all-capitals untyped-call builder.
		/// </summary>
		[Obsolete("Use EmitCallUntyped instead.")]
		public MediumLevelILExpressionIndex EmitCALL_UNTYPED(
			Variable[] outputs,
			MediumLevelILExpressionIndex destination,
			MediumLevelILExpressionIndex[] parameters,
			MediumLevelILExpressionIndex stack,
			SourceLocation? location = null)
		{
			return this.EmitCallUntyped(outputs, destination, parameters, stack, location);
		}
	}
}
