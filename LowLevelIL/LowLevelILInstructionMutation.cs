using System;
using System.Collections.Generic;

namespace BinaryNinja
{
	public abstract partial class LowLevelILInstruction
	{
		public ISet<ILInstructionAttribute> AttributeSet
		{
			get
			{
				HashSet<ILInstructionAttribute> result =
					new HashSet<ILInstructionAttribute>();

				foreach (ILInstructionAttribute attribute in
					Enum.GetValues<ILInstructionAttribute>())
				{
					uint mask = (uint)attribute;

					if (mask == (this.Attributes & mask))
					{
						result.Add(attribute);
					}
				}

				return result;
			}
		}

		public void Replace(LowLevelILExpressionIndex expression)
		{
			this.ILFunction.ReplaceExpression(this.ExpressionIndex, expression);
		}

		public void Replace(LowLevelILInstruction instruction)
		{
			if (null == instruction)
			{
				throw new ArgumentNullException(nameof(instruction));
			}

			this.Replace(instruction.ExpressionIndex);
		}

		public void SetAttributes(uint attributes)
		{
			this.ILFunction.SetExpressionAttributes(this.ExpressionIndex, attributes);
		}

		public void SetAttribute(
			ILInstructionAttribute attribute,
			bool state = true)
		{
			uint mask = (uint)attribute;
			uint attributes = this.ILFunction.MustGetExpression(
				this.ExpressionIndex).Attributes;

			if (state)
			{
				attributes |= mask;

				if (ILInstructionAttribute.ILAllowDeadStoreElimination == attribute)
				{
					attributes &=
						~(uint)ILInstructionAttribute.ILPreventDeadStoreElimination;
				}
				else if (ILInstructionAttribute.ILPreventDeadStoreElimination == attribute)
				{
					attributes &=
						~(uint)ILInstructionAttribute.ILAllowDeadStoreElimination;
				}
			}
			else
			{
				attributes &= ~mask;
			}

			this.SetAttributes(attributes);
		}

		public void ClearAttribute(ILInstructionAttribute attribute)
		{
			this.SetAttribute(attribute, false);
		}
	}
}
