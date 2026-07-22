using System;
using System.Collections.Generic;

namespace BinaryNinja
{
	public abstract partial class LowLevelILInstruction
	{
		/// <summary>
		/// Gets every named, typed operand exposed by the official LLIL bindings.
		/// </summary>
		public virtual IList<ILOperand> DetailedOperands
		{
			get
			{
				if (false == LowLevelILDetailedOperandsTable.Table.TryGetValue(
						this.Operation, out ILOperandDescriptor[]? descriptors))
				{
					return new List<ILOperand>();
				}

				List<ILOperand> result = new List<ILOperand>(descriptors.Length);

				foreach (ILOperandDescriptor descriptor in descriptors)
				{
					object? value = this.ReadDetailedOperand(descriptor);
					result.Add(new ILOperand(
						descriptor.Name, value, descriptor.Kind, descriptor.TypeName));
				}

				return result;
			}
		}

		/// <summary>
		/// Gets the values from <see cref="DetailedOperands"/> in official operand order.
		/// </summary>
		public IList<object?> Operands
		{
			get
			{
				IList<ILOperand> detailed = this.DetailedOperands;
				List<object?> result = new List<object?>(detailed.Count);

				foreach (ILOperand operand in detailed)
				{
					result.Add(operand.Value);
				}

				return result;
			}
		}

		/// <summary>
		/// Traverses this instruction and every expression-valued detailed operand depth first.
		/// </summary>
		public IEnumerable<T> Traverse<T>(
			Func<LowLevelILInstruction, T?> callback) where T : class
		{
			if (null == callback)
			{
				throw new ArgumentNullException(nameof(callback));
			}

			T? root = callback(this);

			if (null != root)
			{
				yield return root;
			}

			foreach (ILOperand operand in this.DetailedOperands)
			{
				if (operand.Value is LowLevelILInstruction child)
				{
					foreach (T nested in child.Traverse<T>(callback))
					{
						yield return nested;
					}
				}
				else if (operand.Value is LowLevelILInstruction[] children)
				{
					foreach (LowLevelILInstruction listChild in children)
					{
						foreach (T nested in listChild.Traverse<T>(callback))
						{
							yield return nested;
						}
					}
				}
			}
		}

		private object? ReadDetailedOperand(ILOperandDescriptor descriptor)
		{
			OperandIndex index = (OperandIndex)(ulong)descriptor.RawIndex;
			int secondary = descriptor.SecondaryRawIndex < 0
				? descriptor.RawIndex + 1
				: descriptor.SecondaryRawIndex;
			OperandIndex secondaryIndex = (OperandIndex)(ulong)secondary;

			switch (descriptor.Kind)
			{
				case ILOperandKind.Expression:
					return this.GetOperandAsExpression(index);

				case ILOperandKind.ExpressionList:
					return this.GetOperandAsExpressionList(index);

				case ILOperandKind.Integer:
					return unchecked((long)this.RawOperands[(ulong)index]);

				case ILOperandKind.IntegerList:
					return this.GetOperandAsIndexList(index);

				case ILOperandKind.Float:
					if (4 == this.Size)
					{
						return (double)this.GetOperandAsFloat(index);
					}

					if (8 == this.Size)
					{
						return this.GetOperandAsDouble(index);
					}

					return (double)this.RawOperands[(ulong)index];

				case ILOperandKind.PossibleValueSet:
					return this.GetOperandAsPossibleValueSet(index);

				case ILOperandKind.Intrinsic:
					return this.GetOperandAsIntrinsic(index);

				case ILOperandKind.Register:
					return this.GetOperandAsRegister(index);

				case ILOperandKind.Flag:
					return this.GetOperandAsFlag(index);

				case ILOperandKind.RegisterStack:
					return this.GetOperandAsRegisterStack(index);

				case ILOperandKind.SSARegister:
					return this.GetOperandAsSSARegister(index, secondaryIndex);

				case ILOperandKind.SSAFlag:
					return this.GetOperandAsSSAFlag(index, secondaryIndex);

				case ILOperandKind.SSARegisterStack:
					return this.GetOperandAsSSARegisterStack(index, secondaryIndex);

				case ILOperandKind.FlagCondition:
					return this.GetOperandAsFlagCondition(index);

				case ILOperandKind.SemanticFlagClass:
					return this.GetOperandAsSemanticFlagClass(index);

				case ILOperandKind.SemanticFlagGroup:
					return this.GetOperandAsSemanticFlagGroup(index);

				case ILOperandKind.TargetMap:
					return this.GetOperandAsIntegerDict<ulong>(index);

				case ILOperandKind.RegisterStackAdjustments:
					return this.GetOperandAsRegisterStackDict(index);

				case ILOperandKind.RegisterOrFlagList:
					return this.GetOperandAsFlagOrRegisterList(index);

				case ILOperandKind.SSARegisterList:
					return this.GetOperandAsSSARegisterList(index);

				case ILOperandKind.SSAFlagList:
					return this.GetOperandAsSSAFlagList(index);

				case ILOperandKind.SSARegisterStackList:
					return this.GetOperandAsSSARegisterStackList(index);

				case ILOperandKind.SSARegisterOrFlagList:
					return this.GetOperandAsSSAFlagOrRegisterList(index);

				case ILOperandKind.LLILCallOutputSsaRegisters:
					return ((LLILCallOutputSSA)this.GetOperandAsExpression(index)).Destination;

				case ILOperandKind.LLILCallStackSsaRegister:
					return ((LLILCallStackSSA)this.GetOperandAsExpression(index)).Source;

				case ILOperandKind.LLILCallStackSsaMemory:
					return unchecked((long)((LLILCallStackSSA)this.GetOperandAsExpression(index))
						.SourceMemory);

				case ILOperandKind.LLILCallParamExpressions:
					return ((LLILCallParameter)this.GetOperandAsExpression(index)).Source;

				case ILOperandKind.LLILMemoryIntrinsicOutputSsaRegisters:
					return ((LLILMemoryIntrinsicOutputSSA)this.GetOperandAsExpression(index)).Output;

				case ILOperandKind.LLILMemoryIntrinsicOutputSsaMemory:
					return unchecked((long)((LLILMemoryIntrinsicOutputSSA)this.GetOperandAsExpression(index))
						.DestinationMemory);

				case ILOperandKind.LLILAddOverflowLeft:
					return ((LLILAddOverflow)this).Left;

				case ILOperandKind.LLILAddOverflowRight:
					return ((LLILAddOverflow)this).Right;

				default:
					return null;
			}
		}
	}
}
