using System.Collections.Generic;

namespace BinaryNinja
{
	public sealed partial class HighLevelILFunction
	{
		public HighLevelILExpressionIndex AddIndexList(ulong[] indexes)
		{
			return this.AddOperandList(indexes);
		}

		public HighLevelILExpressionIndex AddSSAVariableList(
			HighLevelILSSAVariable[] variables)
		{
			List<ulong> operands = new List<ulong>(variables.Length * 2);

			foreach (HighLevelILSSAVariable variable in variables)
			{
				operands.Add(variable.Variable.Identifier);
				operands.Add(variable.Version);
			}

			return this.AddOperandList(operands.ToArray());
		}

		public HighLevelILExpressionIndex EmitWhileSSA(
			HighLevelILExpressionIndex conditionPhi,
			HighLevelILExpressionIndex condition,
			HighLevelILExpressionIndex loop,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_WHILE_SSA,
				location,
				0,
				(ulong)conditionPhi,
				(ulong)condition,
				(ulong)loop);
		}

		public HighLevelILExpressionIndex EmitDoWhileSSA(
			HighLevelILExpressionIndex loop,
			HighLevelILExpressionIndex conditionPhi,
			HighLevelILExpressionIndex condition,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_DO_WHILE_SSA,
				location,
				0,
				(ulong)loop,
				(ulong)conditionPhi,
				(ulong)condition);
		}

		public HighLevelILExpressionIndex EmitForSSA(
			HighLevelILExpressionIndex init,
			HighLevelILExpressionIndex conditionPhi,
			HighLevelILExpressionIndex condition,
			HighLevelILExpressionIndex update,
			HighLevelILExpressionIndex loop,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_FOR_SSA,
				location,
				0,
				(ulong)init,
				(ulong)conditionPhi,
				(ulong)condition,
				(ulong)update,
				(ulong)loop);
		}

		public HighLevelILExpressionIndex EmitVariableInitSSA(
			ulong size,
			HighLevelILSSAVariable destination,
			HighLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_VAR_INIT_SSA,
				location,
				size,
				destination.Variable.Identifier,
				destination.Version,
				(ulong)source);
		}

		public HighLevelILExpressionIndex EmitAssignUnpack(
			HighLevelILExpressionIndex[] output,
			HighLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_ASSIGN_UNPACK,
				location,
				0,
				(ulong)output.Length,
				(ulong)this.AddOperandList(output),
				(ulong)source);
		}

		public HighLevelILExpressionIndex EmitAssignMemorySSA(
			ulong size,
			HighLevelILExpressionIndex destination,
			ulong destinationMemoryVersion,
			HighLevelILExpressionIndex source,
			ulong sourceMemoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_ASSIGN_MEM_SSA,
				location,
				size,
				(ulong)destination,
				destinationMemoryVersion,
				(ulong)source,
				sourceMemoryVersion);
		}

		public HighLevelILExpressionIndex EmitAssignUnpackMemorySSA(
			HighLevelILExpressionIndex[] output,
			ulong destinationMemoryVersion,
			HighLevelILExpressionIndex source,
			ulong sourceMemoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_ASSIGN_UNPACK_MEM_SSA,
				location,
				0,
				(ulong)output.Length,
				(ulong)this.AddOperandList(output),
				destinationMemoryVersion,
				(ulong)source,
				sourceMemoryVersion);
		}

		public HighLevelILExpressionIndex EmitForceVersion(
			ulong size,
			Variable destination,
			Variable source,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_FORCE_VER,
				location,
				size,
				destination.Identifier,
				source.Identifier);
		}

		public HighLevelILExpressionIndex EmitForceVersionSSA(
			ulong size,
			HighLevelILSSAVariable destination,
			HighLevelILSSAVariable source,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_FORCE_VER_SSA,
				location,
				size,
				destination.Variable.Identifier,
				destination.Version,
				source.Variable.Identifier,
				source.Version);
		}

		public HighLevelILExpressionIndex EmitAssert(
			ulong size,
			Variable source,
			PossibleValueSet constraint,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_ASSERT,
				location,
				size,
				source.Identifier,
				(ulong)this.CachePossibleValueSet(constraint));
		}

		public HighLevelILExpressionIndex EmitAssertSSA(
			ulong size,
			HighLevelILSSAVariable source,
			PossibleValueSet constraint,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_ASSERT_SSA,
				location,
				size,
				source.Variable.Identifier,
				source.Version,
				(ulong)this.CachePossibleValueSet(constraint));
		}

		public HighLevelILExpressionIndex EmitVariableSSA(
			ulong size,
			HighLevelILSSAVariable source,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_VAR_SSA,
				location,
				size,
				source.Variable.Identifier,
				source.Version);
		}

		public HighLevelILExpressionIndex EmitVariablePhi(
			HighLevelILSSAVariable destination,
			HighLevelILSSAVariable[] sources,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_VAR_PHI,
				location,
				0,
				destination.Variable.Identifier,
				destination.Version,
				(ulong)sources.Length * 2,
				(ulong)this.AddSSAVariableList(sources));
		}

		public HighLevelILExpressionIndex EmitMemoryPhi(
			ulong destinationMemoryVersion,
			ulong[] sourceMemoryVersions,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_MEM_PHI,
				location,
				0,
				destinationMemoryVersion,
				(ulong)sourceMemoryVersions.Length,
				(ulong)this.AddIndexList(sourceMemoryVersions));
		}

		public HighLevelILExpressionIndex EmitArrayIndexSSA(
			ulong size,
			HighLevelILExpressionIndex source,
			ulong sourceMemoryVersion,
			HighLevelILExpressionIndex index,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_ARRAY_INDEX_SSA,
				location,
				size,
				(ulong)source,
				sourceMemoryVersion,
				(ulong)index);
		}

		public HighLevelILExpressionIndex EmitDerefSSA(
			ulong size,
			HighLevelILExpressionIndex source,
			ulong sourceMemoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_DEREF_SSA,
				location,
				size,
				(ulong)source,
				sourceMemoryVersion);
		}

		public HighLevelILExpressionIndex EmitDerefFieldSSA(
			ulong size,
			HighLevelILExpressionIndex source,
			ulong sourceMemoryVersion,
			ulong offset,
			ulong memberIndex,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_DEREF_FIELD_SSA,
				location,
				size,
				(ulong)source,
				sourceMemoryVersion,
				offset,
				memberIndex);
		}

		public HighLevelILExpressionIndex EmitAddressOf(
			HighLevelILExpressionIndex source,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_ADDRESS_OF,
				location,
				0,
				(ulong)source);
		}

		public HighLevelILExpressionIndex EmitCall(
			HighLevelILExpressionIndex destination,
			HighLevelILExpressionIndex[] parameters,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_CALL,
				location,
				0,
				(ulong)destination,
				(ulong)parameters.Length,
				(ulong)this.AddOperandList(parameters));
		}

		public HighLevelILExpressionIndex EmitSysCall(
			HighLevelILExpressionIndex[] parameters,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_SYSCALL,
				location,
				0,
				(ulong)parameters.Length,
				(ulong)this.AddOperandList(parameters));
		}

		public HighLevelILExpressionIndex EmitTailCall(
			HighLevelILExpressionIndex destination,
			HighLevelILExpressionIndex[] parameters,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_TAILCALL,
				location,
				0,
				(ulong)destination,
				(ulong)parameters.Length,
				(ulong)this.AddOperandList(parameters));
		}

		public HighLevelILExpressionIndex EmitCallSSA(
			HighLevelILExpressionIndex destination,
			HighLevelILExpressionIndex[] parameters,
			ulong destinationMemoryVersion,
			ulong sourceMemoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_CALL_SSA,
				location,
				0,
				(ulong)destination,
				(ulong)parameters.Length,
				(ulong)this.AddOperandList(parameters),
				destinationMemoryVersion,
				sourceMemoryVersion);
		}

		public HighLevelILExpressionIndex EmitSysCallSSA(
			HighLevelILExpressionIndex[] parameters,
			ulong destinationMemoryVersion,
			ulong sourceMemoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_SYSCALL_SSA,
				location,
				0,
				(ulong)parameters.Length,
				(ulong)this.AddOperandList(parameters),
				destinationMemoryVersion,
				sourceMemoryVersion);
		}

		public HighLevelILExpressionIndex EmitIntrinsic(
			IntrinsicIndex intrinsic,
			HighLevelILExpressionIndex[] parameters,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_INTRINSIC,
				location,
				0,
				(ulong)intrinsic,
				(ulong)parameters.Length,
				(ulong)this.AddOperandList(parameters));
		}

		public HighLevelILExpressionIndex EmitIntrinsicSSA(
			IntrinsicIndex intrinsic,
			HighLevelILExpressionIndex[] parameters,
			ulong destinationMemoryVersion,
			ulong sourceMemoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_INTRINSIC_SSA,
				location,
				0,
				(ulong)intrinsic,
				(ulong)parameters.Length,
				(ulong)this.AddOperandList(parameters),
				destinationMemoryVersion,
				sourceMemoryVersion);
		}
	}
}
