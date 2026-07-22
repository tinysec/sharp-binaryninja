using System.Collections.Generic;

namespace BinaryNinja
{
	public sealed partial class MediumLevelILFunction
	{
		public MediumLevelILExpressionIndex AddIndexList(ulong[] indexes)
		{
			return this.AddOperandList(indexes);
		}

		public MediumLevelILExpressionIndex AddSSAVariableList(
			MediumLevelILSSAVariable[] variables)
		{
			List<ulong> operands = new List<ulong>(variables.Length * 2);

			foreach (MediumLevelILSSAVariable variable in variables)
			{
				operands.Add(variable.Variable.Identifier);
				operands.Add(variable.Version);
			}

			return this.AddOperandList(operands.ToArray());
		}

		public MediumLevelILExpressionIndex EmitSetVarSSA(
			ulong size,
			MediumLevelILSSAVariable destination,
			MediumLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_SET_VAR_SSA,
				location,
				size,
				destination.Variable.Identifier,
				destination.Version,
				(ulong)source);
		}

		public MediumLevelILExpressionIndex EmitSetVarSSAField(
			ulong size,
			Variable destination,
			ulong newVersion,
			ulong previousVersion,
			ulong offset,
			MediumLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_SET_VAR_SSA_FIELD,
				location,
				size,
				destination.Identifier,
				newVersion,
				previousVersion,
				offset,
				(ulong)source);
		}

		public MediumLevelILExpressionIndex EmitSetVarSplitSSA(
			ulong size,
			MediumLevelILSSAVariable high,
			MediumLevelILSSAVariable low,
			MediumLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_SET_VAR_SPLIT_SSA,
				location,
				size,
				high.Variable.Identifier,
				high.Version,
				low.Variable.Identifier,
				low.Version,
				(ulong)source);
		}

		public MediumLevelILExpressionIndex EmitSetVarAliased(
			ulong size,
			Variable destination,
			ulong newMemoryVersion,
			ulong previousMemoryVersion,
			MediumLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_SET_VAR_ALIASED,
				location,
				size,
				destination.Identifier,
				newMemoryVersion,
				previousMemoryVersion,
				(ulong)source);
		}

		public MediumLevelILExpressionIndex EmitSetVarAliasedField(
			ulong size,
			Variable destination,
			ulong newMemoryVersion,
			ulong previousMemoryVersion,
			ulong offset,
			MediumLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_SET_VAR_ALIASED_FIELD,
				location,
				size,
				destination.Identifier,
				newMemoryVersion,
				previousMemoryVersion,
				offset,
				(ulong)source);
		}

		public MediumLevelILExpressionIndex EmitForceVerSSA(
			ulong size,
			MediumLevelILSSAVariable destination,
			MediumLevelILSSAVariable source,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_FORCE_VER_SSA,
				location,
				size,
				destination.Variable.Identifier,
				destination.Version,
				source.Variable.Identifier,
				source.Version);
		}

		public MediumLevelILExpressionIndex EmitAssertSSA(
			ulong size,
			MediumLevelILSSAVariable source,
			PossibleValueSet constraint,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_ASSERT_SSA,
				location,
				size,
				source.Variable.Identifier,
				source.Version,
				(ulong)this.CachePossibleValueSet(constraint));
		}

		public MediumLevelILExpressionIndex EmitLoadSSA(
			ulong size,
			MediumLevelILExpressionIndex source,
			ulong memoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_LOAD_SSA,
				location,
				size,
				(ulong)source,
				memoryVersion);
		}

		public MediumLevelILExpressionIndex EmitLoadStructSSA(
			ulong size,
			MediumLevelILExpressionIndex source,
			ulong offset,
			ulong memoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_LOAD_STRUCT_SSA,
				location,
				size,
				(ulong)source,
				offset,
				memoryVersion);
		}

		public MediumLevelILExpressionIndex EmitStoreSSA(
			ulong size,
			MediumLevelILExpressionIndex destination,
			ulong newMemoryVersion,
			ulong previousMemoryVersion,
			MediumLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_STORE_SSA,
				location,
				size,
				(ulong)destination,
				newMemoryVersion,
				previousMemoryVersion,
				(ulong)source);
		}

		public MediumLevelILExpressionIndex EmitStoreStructSSA(
			ulong size,
			MediumLevelILExpressionIndex destination,
			ulong offset,
			ulong newMemoryVersion,
			ulong previousMemoryVersion,
			MediumLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_STORE_STRUCT_SSA,
				location,
				size,
				(ulong)destination,
				offset,
				newMemoryVersion,
				previousMemoryVersion,
				(ulong)source);
		}

		public MediumLevelILExpressionIndex EmitVarSSA(
			ulong size,
			MediumLevelILSSAVariable source,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_VAR_SSA,
				location,
				size,
				source.Variable.Identifier,
				source.Version);
		}

		public MediumLevelILExpressionIndex EmitVarSSAField(
			ulong size,
			MediumLevelILSSAVariable source,
			ulong offset,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_VAR_SSA_FIELD,
				location,
				size,
				source.Variable.Identifier,
				source.Version,
				offset);
		}

		public MediumLevelILExpressionIndex EmitVarAliased(
			ulong size,
			Variable source,
			ulong memoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_VAR_ALIASED,
				location,
				size,
				source.Identifier,
				memoryVersion);
		}

		public MediumLevelILExpressionIndex EmitVarAliasedField(
			ulong size,
			Variable source,
			ulong memoryVersion,
			ulong offset,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_VAR_ALIASED_FIELD,
				location,
				size,
				source.Identifier,
				memoryVersion,
				offset);
		}

		public MediumLevelILExpressionIndex EmitVarSplitSSA(
			ulong size,
			MediumLevelILSSAVariable high,
			MediumLevelILSSAVariable low,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_VAR_SPLIT_SSA,
				location,
				size,
				high.Variable.Identifier,
				high.Version,
				low.Variable.Identifier,
				low.Version);
		}

		public MediumLevelILExpressionIndex EmitFreeVarSlotSSA(
			Variable variable,
			ulong newVersion,
			ulong previousVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_FREE_VAR_SLOT_SSA,
				location,
				0,
				variable.Identifier,
				newVersion,
				previousVersion);
		}

		public MediumLevelILExpressionIndex EmitVarPhi(
			MediumLevelILSSAVariable destination,
			MediumLevelILSSAVariable[] sources,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_VAR_PHI,
				location,
				0,
				destination.Variable.Identifier,
				destination.Version,
				(ulong)sources.Length * 2,
				(ulong)this.AddSSAVariableList(sources));
		}

		public MediumLevelILExpressionIndex EmitMemoryPhi(
			ulong destinationMemoryVersion,
			ulong[] sourceMemoryVersions,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_MEM_PHI,
				location,
				0,
				destinationMemoryVersion,
				(ulong)sourceMemoryVersions.Length,
				(ulong)this.AddIndexList(sourceMemoryVersions));
		}
	}
}
