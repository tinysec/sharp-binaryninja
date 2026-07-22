using System;
using System.Collections.Generic;

namespace BinaryNinja
{
	public sealed partial class HighLevelILFunction
	{
		public HighLevelILExpressionIndex? GetSSAExpressionIndex(
			HighLevelILExpressionIndex expression)
		{
			HighLevelILExpressionIndex index = NativeMethods.BNGetHighLevelILSSAExprIndex(
				this.DangerousGetHandle(),
				expression);
			using HighLevelILFunction targetFunction = this.SSAForm;

			if ((ulong)index >= targetFunction.ExpressionCount)
			{
				return null;
			}

			return index;
		}

		public HighLevelILExpressionIndex? GetNonSSAExpressionIndex(
			HighLevelILExpressionIndex expression)
		{
			HighLevelILFunction? targetFunction = this.NonSSAForm;
			if (null == targetFunction)
			{
				return null;
			}

			using (targetFunction)
			{
				HighLevelILExpressionIndex index =
					NativeMethods.BNGetHighLevelILNonSSAExprIndex(
						this.DangerousGetHandle(),
						expression);

				if ((ulong)index >= targetFunction.ExpressionCount)
				{
					return null;
				}

				return index;
			}
		}

		public ulong GetSSAVariableVersionAtInstruction(
			Variable variable,
			HighLevelILInstructionIndex instruction)
		{
			return NativeMethods.BNGetHighLevelILSSAVarVersionAtILInstruction(
				this.DangerousGetHandle(),
				variable.ToNative(),
				instruction);
		}

		public ulong GetSSAMemoryVersionAtInstruction(
			HighLevelILInstructionIndex instruction)
		{
			return NativeMethods.BNGetHighLevelILSSAMemoryVersionAtILInstruction(
				this.DangerousGetHandle(),
				instruction);
		}

		public MediumLevelILExpressionIndex? GetMediumLevelILExpressionIndex(
			HighLevelILExpressionIndex expression)
		{
			using MediumLevelILFunction? mediumFunction = this.MediumLevelIL;
			if (null == mediumFunction)
			{
				return null;
			}

			MediumLevelILExpressionIndex index =
				NativeMethods.BNGetMediumLevelILExprIndexFromHighLevelIL(
					this.DangerousGetHandle(),
					expression);

			if ((ulong)index >= mediumFunction.ExpressionCount)
			{
				return null;
			}

			return index;
		}

		public MediumLevelILExpressionIndex[] GetMediumLevelILExpressionIndexes(
			HighLevelILExpressionIndex expression)
		{
			IntPtr arrayPointer = NativeMethods.BNGetMediumLevelILExprIndexesFromHighLevelIL(
				this.DangerousGetHandle(),
				expression,
				out ulong arrayLength);

			return UnsafeUtils.TakeNumberArray<MediumLevelILExpressionIndex>(
				arrayPointer,
				arrayLength,
				NativeMethods.BNFreeILInstructionList);
		}

		public MediumLevelILInstruction? GetMediumLevelILExpression(
			HighLevelILExpressionIndex expression)
		{
			MediumLevelILFunction? mediumFunction = this.MediumLevelIL;
			if (null == mediumFunction)
			{
				return null;
			}

			MediumLevelILExpressionIndex index =
				NativeMethods.BNGetMediumLevelILExprIndexFromHighLevelIL(
					this.DangerousGetHandle(),
					expression);

			if ((ulong)index >= mediumFunction.ExpressionCount)
			{
				mediumFunction.Dispose();

				return null;
			}

			return mediumFunction.GetExpression(index);
		}

		public MediumLevelILInstruction[] GetMediumLevelILExpressions(
			HighLevelILExpressionIndex expression)
		{
			MediumLevelILFunction? mediumFunction = this.MediumLevelIL;
			if (null == mediumFunction)
			{
				return Array.Empty<MediumLevelILInstruction>();
			}

			MediumLevelILExpressionIndex[] indexes =
				this.GetMediumLevelILExpressionIndexes(expression);
			List<MediumLevelILInstruction> instructions =
				new List<MediumLevelILInstruction>(indexes.Length);

			foreach (MediumLevelILExpressionIndex index in indexes)
			{
				MediumLevelILInstruction? instruction = mediumFunction.GetExpression(index);

				if (null != instruction)
				{
					instructions.Add(instruction);
				}
			}

			if (0 == instructions.Count)
			{
				mediumFunction.Dispose();
			}

			return instructions.ToArray();
		}
	}
}
