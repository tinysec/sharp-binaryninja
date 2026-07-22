using System;
using System.Collections.Generic;

namespace BinaryNinja
{
	/// <summary>
	/// Handles one immediate child expression while an LLIL expression is copied.
	/// </summary>
	public delegate LowLevelILExpressionIndex LowLevelILSubExpressionHandler(
		LowLevelILInstruction expression);

	public abstract partial class LowLevelILInstruction
	{
		private sealed class DefaultCopyContext
		{
			private readonly LowLevelILFunction destination;

			private readonly SourceLocation? sourceLocation;

			internal DefaultCopyContext(
				LowLevelILFunction destination,
				SourceLocation? sourceLocation)
			{
				this.destination = destination;
				this.sourceLocation = sourceLocation;
			}

			internal LowLevelILExpressionIndex Copy(LowLevelILInstruction expression)
			{
				if (null != this.sourceLocation)
				{
					return expression.CopyTo(this.destination, this.sourceLocation);
				}

				return expression.CopyTo(this.destination);
			}
		}

		/// <summary>
		/// Deep copies this expression and all of its children to a writable LLIL function.
		/// </summary>
		public LowLevelILExpressionIndex CopyTo(LowLevelILFunction destination)
		{
			if (null == destination)
			{
				throw new ArgumentNullException(nameof(destination));
			}

			DefaultCopyContext context = new DefaultCopyContext(destination, null);

			return this.CopyTo(destination, context.Copy, null);
		}

		/// <summary>
		/// Deep copies this expression and applies one source location to the copied tree.
		/// </summary>
		public LowLevelILExpressionIndex CopyTo(
			LowLevelILFunction destination,
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
		/// Deep copies this expression while delegating each immediate child to a handler.
		/// </summary>
		public LowLevelILExpressionIndex CopyTo(
			LowLevelILFunction destination,
			LowLevelILSubExpressionHandler subExpressionHandler)
		{
			return this.CopyTo(destination, subExpressionHandler, null);
		}

		/// <summary>
		/// Deep copies this expression with a child handler and optional root source location.
		/// </summary>
		public LowLevelILExpressionIndex CopyTo(
			LowLevelILFunction destination,
			LowLevelILSubExpressionHandler subExpressionHandler,
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
			LowLevelILExpressionIndex result;

			switch (this.Operation)
			{
				case LowLevelILOperation.LLIL_JUMP_TO:
					result = this.CopyJumpTo(destination, subExpressionHandler, location);
					break;

				case LowLevelILOperation.LLIL_GOTO:
					result = this.CopyGoto(destination, location);
					break;

				case LowLevelILOperation.LLIL_IF:
					result = this.CopyIf(destination, subExpressionHandler, location);
					break;

				case LowLevelILOperation.LLIL_ADD_OVERFLOW:
					result = this.CopyAddOverflow(
						destination, subExpressionHandler, location);
					break;

				default:
					result = this.CopyGeneric(destination, subExpressionHandler, location);
					break;
			}

			destination.SetExpressionAttributes(result, this.Attributes);

			return result;
		}

		private LowLevelILExpressionIndex CopyGeneric(
			LowLevelILFunction destination,
			LowLevelILSubExpressionHandler subExpressionHandler,
			SourceLocation location)
		{
			if (false == LowLevelILDetailedOperandsTable.Table.TryGetValue(
					this.Operation, out ILOperandDescriptor[]? descriptors))
			{
				throw new NotSupportedException(
					"Cannot deep copy unknown LLIL operation " + this.Operation + ".");
			}

			ulong[] operands = (ulong[])this.RawOperands.Clone();
			HashSet<int> copiedExpressionSlots = new HashSet<int>();
			HashSet<int> copiedListSlots = new HashSet<int>();

			foreach (ILOperandDescriptor descriptor in descriptors)
			{
				if (ILOperandKind.Expression == descriptor.Kind ||
					IsDerivedExpressionKind(descriptor.Kind))
				{
					this.CopyExpressionOperand(
						descriptor.RawIndex,
						operands,
						copiedExpressionSlots,
						subExpressionHandler);
				}
				else if (ILOperandKind.ExpressionList == descriptor.Kind)
				{
					this.CopyExpressionListOperand(
						descriptor.RawIndex,
						operands,
						copiedListSlots,
						destination,
						subExpressionHandler);
				}
				else if (IsRawListKind(descriptor.Kind))
				{
					this.CopyRawListOperand(
						descriptor.RawIndex,
						operands,
						copiedListSlots,
						destination);
				}
				else if (ILOperandKind.PossibleValueSet == descriptor.Kind)
				{
					operands[descriptor.RawIndex] = (ulong)destination.CachePossibleValueSet(
						this.GetOperandAsPossibleValueSet(
							(OperandIndex)(ulong)descriptor.RawIndex));
				}
			}

			return destination.AddExpression(
				this.Operation,
				location,
				this.Size,
				this.Flags,
				operands);
		}

		private static bool IsDerivedExpressionKind(ILOperandKind kind)
		{
			switch (kind)
			{
				case ILOperandKind.LLILCallOutputSsaRegisters:
				case ILOperandKind.LLILCallStackSsaRegister:
				case ILOperandKind.LLILCallStackSsaMemory:
				case ILOperandKind.LLILCallParamExpressions:
				case ILOperandKind.LLILMemoryIntrinsicOutputSsaRegisters:
				case ILOperandKind.LLILMemoryIntrinsicOutputSsaMemory:
					return true;

				default:
					return false;
			}
		}

		private static bool IsRawListKind(ILOperandKind kind)
		{
			switch (kind)
			{
				case ILOperandKind.IntegerList:
				case ILOperandKind.TargetMap:
				case ILOperandKind.RegisterStackAdjustments:
				case ILOperandKind.RegisterOrFlagList:
				case ILOperandKind.SSARegisterList:
				case ILOperandKind.SSAFlagList:
				case ILOperandKind.SSARegisterStackList:
				case ILOperandKind.SSARegisterOrFlagList:
					return true;

				default:
					return false;
			}
		}

		private void CopyExpressionOperand(
			int rawIndex,
			ulong[] operands,
			HashSet<int> copiedExpressionSlots,
			LowLevelILSubExpressionHandler subExpressionHandler)
		{
			if (false == copiedExpressionSlots.Add(rawIndex))
			{
				return;
			}

			LowLevelILInstruction expression = this.GetOperandAsExpression(
				(OperandIndex)(ulong)rawIndex);
			operands[rawIndex] = (ulong)subExpressionHandler(expression);
		}

		private void CopyExpressionListOperand(
			int rawIndex,
			ulong[] operands,
			HashSet<int> copiedListSlots,
			LowLevelILFunction destination,
			LowLevelILSubExpressionHandler subExpressionHandler)
		{
			if (false == copiedListSlots.Add(rawIndex))
			{
				return;
			}

			LowLevelILInstruction[] expressions = this.GetOperandAsExpressionList(
				(OperandIndex)(ulong)rawIndex);
			LowLevelILExpressionIndex[] copiedExpressions =
				new LowLevelILExpressionIndex[expressions.Length];

			for (int index = 0; index < expressions.Length; index++)
			{
				copiedExpressions[index] = subExpressionHandler(expressions[index]);
			}

			operands[rawIndex + 1] = (ulong)destination.AddOperandList(copiedExpressions);
		}

		private void CopyRawListOperand(
			int rawIndex,
			ulong[] operands,
			HashSet<int> copiedListSlots,
			LowLevelILFunction destination)
		{
			if (false == copiedListSlots.Add(rawIndex))
			{
				return;
			}

			ulong[] list = this.GetOperandAsIntegerArray<ulong>(
				(OperandIndex)(ulong)rawIndex);
			operands[rawIndex + 1] = (ulong)destination.AddOperandList(list);
		}

		private LowLevelILExpressionIndex CopyAddOverflow(
			LowLevelILFunction destination,
			LowLevelILSubExpressionHandler subExpressionHandler,
			SourceLocation location)
		{
			LLILAddOverflow overflow = (LLILAddOverflow)this;
			LowLevelILExpressionIndex left = subExpressionHandler(overflow.Left);
			LowLevelILExpressionIndex right = subExpressionHandler(overflow.Right);

			return destination.EmitAddOverflow(this.Size, left, right, location);
		}

		private LowLevelILExpressionIndex CopyJumpTo(
			LowLevelILFunction destination,
			LowLevelILSubExpressionHandler subExpressionHandler,
			SourceLocation location)
		{
			LLILJumpTo jump = (LLILJumpTo)this;
			Dictionary<ulong, LowLevelILLabel> labels =
				new Dictionary<ulong, LowLevelILLabel>();

			foreach (KeyValuePair<ulong, ulong> target in jump.Targets)
			{
				LowLevelILLabel? label = destination.GetLabelForSourceInstruction(
					(LowLevelILInstructionIndex)target.Value);

				if (null == label)
				{
					return destination.EmitJump(
						subExpressionHandler(jump.Destination), location);
				}

				labels.Add(target.Key, label);
			}

			return destination.EmitJumpTo(
				subExpressionHandler(jump.Destination), labels, location);
		}

		private LowLevelILExpressionIndex CopyGoto(
			LowLevelILFunction destination,
			SourceLocation location)
		{
			LLILGoto goTo = (LLILGoto)this;
			LowLevelILLabel? label = destination.GetLabelForSourceInstruction(goTo.Destination);

			if (null != label)
			{
				return destination.EmitGoto(label, location);
			}

			LowLevelILInstruction target = this.ILFunction.MustGetInstruction(goTo.Destination);
			LowLevelILExpressionIndex address = destination.EmitConstPointer(
				this.ILFunction.Architecture.AddressSize,
				target.Address);

			return destination.EmitJump(address, location);
		}

		private LowLevelILExpressionIndex CopyIf(
			LowLevelILFunction destination,
			LowLevelILSubExpressionHandler subExpressionHandler,
			SourceLocation location)
		{
			LLILIf conditional = (LLILIf)this;
			LowLevelILLabel? trueLabel = destination.GetLabelForSourceInstruction(
				conditional.TrueBranch.InstructionIndex);
			LowLevelILLabel? falseLabel = destination.GetLabelForSourceInstruction(
				conditional.FalseBranch.InstructionIndex);

			if (null == trueLabel || null == falseLabel)
			{
				return destination.EmitUndefined(location);
			}

			return destination.EmitIf(
				subExpressionHandler(conditional.Condition),
				trueLabel,
				falseLabel,
				location);
		}
	}

	public sealed partial class LowLevelILFunction
	{
		/// <summary>
		/// Deep copies an expression owned by this function to another writable LLIL function.
		/// </summary>
		public LowLevelILExpressionIndex CopyExpressionTo(
			LowLevelILInstruction original,
			LowLevelILFunction destination)
		{
			return this.CopyExpressionTo(original, destination, null);
		}

		/// <summary>
		/// Deep copies an expression while optionally delegating each immediate child.
		/// </summary>
		public LowLevelILExpressionIndex CopyExpressionTo(
			LowLevelILInstruction original,
			LowLevelILFunction destination,
			LowLevelILSubExpressionHandler? subExpressionHandler)
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
					"The expression must be owned by the source LLIL function.",
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
