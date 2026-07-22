using System;
using System.Collections.Generic;

namespace BinaryNinja
{
	/// <summary>
	/// Handles one immediate child expression while an MLIL expression is copied.
	/// </summary>
	public delegate MediumLevelILExpressionIndex MediumLevelILSubExpressionHandler(
		MediumLevelILInstruction expression);

	/// <summary>
	/// Deep-copy support for MLIL expressions.
	/// </summary>
	public abstract partial class MediumLevelILInstruction
	{
		private sealed class DefaultCopyContext
		{
			private readonly MediumLevelILFunction destination;

			private readonly SourceLocation? sourceLocation;

			internal DefaultCopyContext(
				MediumLevelILFunction destination,
				SourceLocation? sourceLocation)
			{
				this.destination = destination;
				this.sourceLocation = sourceLocation;
			}

			internal MediumLevelILExpressionIndex Copy(MediumLevelILInstruction expression)
			{
				if (null != this.sourceLocation)
				{
					return expression.CopyTo(this.destination, this.sourceLocation);
				}

				return expression.CopyTo(this.destination);
			}
		}

		/// <summary>
		/// Deep copies this expression and all of its children to a writable MLIL function.
		/// </summary>
		/// <remarks>
		/// This API is intended for lifters and workflow activities while analysis is running.
		/// Branch targets require the destination to be prepared with
		/// <see cref="MediumLevelILFunction.PrepareToCopyFunction"/> and
		/// <see cref="MediumLevelILFunction.PrepareToCopyBasicBlock"/>.
		/// </remarks>
		public MediumLevelILExpressionIndex CopyTo(MediumLevelILFunction destination)
		{
			if (null == destination)
			{
				throw new ArgumentNullException(nameof(destination));
			}

			DefaultCopyContext context = new DefaultCopyContext(destination, null);
			return this.CopyTo(destination, context.Copy, null);
		}

		/// <summary>
		/// Deep copies this expression and applies one source location to the complete copied tree.
		/// </summary>
		public MediumLevelILExpressionIndex CopyTo(
			MediumLevelILFunction destination,
			SourceLocation sourceLocation)
		{
			if (null == destination)
			{
				throw new ArgumentNullException(nameof(destination));
			}

			if (null == sourceLocation)
			{
				throw new ArgumentNullException(nameof(sourceLocation));
			}

			DefaultCopyContext context = new DefaultCopyContext(destination, sourceLocation);
			return this.CopyTo(destination, context.Copy, sourceLocation);
		}

		/// <summary>
		/// Deep copies this expression, delegating every immediate child expression to
		/// <paramref name="subExpressionHandler"/>.
		/// </summary>
		public MediumLevelILExpressionIndex CopyTo(
			MediumLevelILFunction destination,
			MediumLevelILSubExpressionHandler subExpressionHandler)
		{
			return this.CopyTo(destination, subExpressionHandler, null);
		}

		/// <summary>
		/// Deep copies this expression with a child handler and an optional root source location.
		/// </summary>
		public MediumLevelILExpressionIndex CopyTo(
			MediumLevelILFunction destination,
			MediumLevelILSubExpressionHandler subExpressionHandler,
			SourceLocation? sourceLocation)
		{
			if (null == destination)
			{
				throw new ArgumentNullException(nameof(destination));
			}

			if (null == subExpressionHandler)
			{
				throw new ArgumentNullException(nameof(subExpressionHandler));
			}

			SourceLocation location = sourceLocation ?? SourceLocation.FromInstruction(this);
			MediumLevelILExpressionIndex result;

			switch (this.Operation)
			{
				case MediumLevelILOperation.MLIL_JUMP_TO:
					result = this.CopyJumpTo(destination, subExpressionHandler, location);
					break;

				case MediumLevelILOperation.MLIL_GOTO:
					result = this.CopyGoto(destination, location);
					break;

				case MediumLevelILOperation.MLIL_IF:
					result = this.CopyIf(destination, subExpressionHandler, location);
					break;

				default:
					result = this.CopyGeneric(destination, subExpressionHandler, location);
					break;
			}

			destination.SetExpressionAttributes(result, this.Attributes);
			return result;
		}

		private MediumLevelILExpressionIndex CopyGeneric(
			MediumLevelILFunction destination,
			MediumLevelILSubExpressionHandler subExpressionHandler,
			SourceLocation location)
		{
			if (false == MediumLevelILDetailedOperandsTable.Table.TryGetValue(
					this.Operation, out ILOperandDescriptor[]? descriptors))
			{
				throw new NotSupportedException(
					"Cannot deep copy unknown MLIL operation " + this.Operation + ".");
			}

			ulong[] operands = (ulong[])this.RawOperands.Clone();
			HashSet<int> copiedExpressionSlots = new HashSet<int>();

			foreach (ILOperandDescriptor descriptor in descriptors)
			{
				switch (descriptor.Kind)
				{
					case ILOperandKind.Expression:
					case ILOperandKind.CallOutputVariables:
					case ILOperandKind.CallParamExpressions:
					case ILOperandKind.CallOutputSsaVariables:
					case ILOperandKind.CallOutputSsaMemory:
					case ILOperandKind.CallParamSsaExpressions:
					case ILOperandKind.CallParamSsaMemory:
					case ILOperandKind.MemoryIntrinsicOutputSsaVariables:
					case ILOperandKind.MemoryIntrinsicOutputSsaMemory:
						this.CopyExpressionOperand(
							descriptor.RawIndex,
							operands,
							copiedExpressionSlots,
							subExpressionHandler);
						break;

					case ILOperandKind.ExpressionList:
						this.CopyExpressionListOperand(
							descriptor.RawIndex, operands, destination, subExpressionHandler);
						break;

					case ILOperandKind.IntegerList:
					case ILOperandKind.VariableList:
					case ILOperandKind.SSAVariableList:
						this.CopyRawListOperand(descriptor.RawIndex, operands, destination);
						break;

					case ILOperandKind.PossibleValueSet:
						operands[descriptor.RawIndex] = (ulong)destination.CachePossibleValueSet(
							this.GetOperandAsPossibleValueSet((OperandIndex)(ulong)descriptor.RawIndex));
						break;
				}
			}

			return destination.AddExpression(this.Operation, location, this.Size, operands);
		}

		private void CopyExpressionOperand(
			int rawIndex,
			ulong[] operands,
			HashSet<int> copiedExpressionSlots,
			MediumLevelILSubExpressionHandler subExpressionHandler)
		{
			if (false == copiedExpressionSlots.Add(rawIndex))
			{
				return;
			}

			MediumLevelILInstruction expression = this.GetOperandAsExpression(
				(OperandIndex)(ulong)rawIndex);
			operands[rawIndex] = (ulong)subExpressionHandler(expression);
		}

		private void CopyExpressionListOperand(
			int rawIndex,
			ulong[] operands,
			MediumLevelILFunction destination,
			MediumLevelILSubExpressionHandler subExpressionHandler)
		{
			MediumLevelILInstruction[] expressions = this.GetOperandAsExpressionList(
				(OperandIndex)(ulong)rawIndex);
			MediumLevelILExpressionIndex[] copiedExpressions =
				new MediumLevelILExpressionIndex[expressions.Length];

			for (int i = 0; i < expressions.Length; i++)
			{
				copiedExpressions[i] = subExpressionHandler(expressions[i]);
			}

			operands[rawIndex + 1] = (ulong)destination.AddOperandList(copiedExpressions);
		}

		private void CopyRawListOperand(
			int rawIndex,
			ulong[] operands,
			MediumLevelILFunction destination)
		{
			ulong[] list = this.GetOperandAsIntegerArray<ulong>((OperandIndex)(ulong)rawIndex);
			operands[rawIndex + 1] = (ulong)destination.AddOperandList(list);
		}

		private MediumLevelILExpressionIndex CopyJumpTo(
			MediumLevelILFunction destination,
			MediumLevelILSubExpressionHandler subExpressionHandler,
			SourceLocation location)
		{
			MLILJumpTo jump = (MLILJumpTo)this;
			Dictionary<ulong, MediumLevelILLabel> labels =
				new Dictionary<ulong, MediumLevelILLabel>();

			foreach (KeyValuePair<ulong, ulong> target in jump.Targets)
			{
				MediumLevelILLabel? label = destination.GetLabelForSourceInstruction(
					(MediumLevelILInstructionIndex)target.Value);
				if (null == label)
				{
					MediumLevelILExpressionIndex copiedDestination =
						subExpressionHandler(jump.Destination);
					return destination.EmitJump(this.Size, copiedDestination, location);
				}

				labels.Add(target.Key, label);
			}

			return destination.EmitJumpTo(
				subExpressionHandler(jump.Destination), labels, location);
		}

		private MediumLevelILExpressionIndex CopyGoto(
			MediumLevelILFunction destination,
			SourceLocation location)
		{
			MLILGoto goTo = (MLILGoto)this;
			MediumLevelILLabel? label = destination.GetLabelForSourceInstruction(goTo.Destination);

			if (null != label)
			{
				return destination.EmitGoto(label, location);
			}

			MediumLevelILInstruction target = this.ILFunction.MustGetInstruction(goTo.Destination);
			MediumLevelILExpressionIndex address = destination.EmitConstPointer(
				this.ILFunction.Architecture.AddressSize, target.Address);
			return destination.EmitJump(0, address, location);
		}

		private MediumLevelILExpressionIndex CopyIf(
			MediumLevelILFunction destination,
			MediumLevelILSubExpressionHandler subExpressionHandler,
			SourceLocation location)
		{
			MLILIf conditional = (MLILIf)this;
			MediumLevelILLabel? trueLabel = destination.GetLabelForSourceInstruction(
				conditional.TrueBranch);
			MediumLevelILLabel? falseLabel = destination.GetLabelForSourceInstruction(
				conditional.FalseBranch);

			if ((null == trueLabel) || (null == falseLabel))
			{
				return destination.EmitUndefined(location);
			}

			return destination.EmitIf(
				subExpressionHandler(conditional.Condition), trueLabel, falseLabel, location);
		}
	}

	public sealed partial class MediumLevelILFunction
	{
		/// <summary>
		/// Deep copies an expression owned by this function to a writable MLIL function.
		/// </summary>
		public MediumLevelILExpressionIndex CopyExpressionTo(
			MediumLevelILInstruction original,
			MediumLevelILFunction destination)
		{
			return this.CopyExpressionTo(original, destination, null);
		}

		/// <summary>
		/// Deep copies an expression owned by this function, delegating every immediate child
		/// expression to <paramref name="subExpressionHandler"/> when it is provided.
		/// </summary>
		public MediumLevelILExpressionIndex CopyExpressionTo(
			MediumLevelILInstruction original,
			MediumLevelILFunction destination,
			MediumLevelILSubExpressionHandler? subExpressionHandler)
		{
			if (null == original)
			{
				throw new ArgumentNullException(nameof(original));
			}

			if (null == destination)
			{
				throw new ArgumentNullException(nameof(destination));
			}

			if (this.DangerousGetHandle() != original.ILFunction.DangerousGetHandle())
			{
				throw new ArgumentException(
					"The expression must be owned by the source MLIL function.",
					nameof(original));
			}

			if (null == subExpressionHandler)
			{
				return original.CopyTo(destination);
			}

			return original.CopyTo(destination, subExpressionHandler);
		}
	}
}
