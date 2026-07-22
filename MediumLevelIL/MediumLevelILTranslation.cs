using System;
using System.Collections.Generic;

namespace BinaryNinja
{
    /// <summary>
    /// Managed translation state used to reproduce the official MLIL-to-MLIL mapping pipeline.
    /// </summary>
    public sealed partial class MediumLevelILFunction
    {
        private sealed class ExpressionTranslationTarget
        {
            public MediumLevelILExpressionIndex ExpressionIndex { get; }

            public bool Direct { get; }

            public ExpressionTranslationTarget(
                MediumLevelILExpressionIndex expressionIndex,
                bool direct)
            {
                this.ExpressionIndex = expressionIndex;
                this.Direct = direct;
            }
        }

        private sealed class InstructionTranslationTarget
        {
            public MediumLevelILInstructionIndex InstructionIndex { get; }

            public bool Direct { get; }

            public InstructionTranslationTarget(
                MediumLevelILInstructionIndex instructionIndex,
                bool direct)
            {
                this.InstructionIndex = instructionIndex;
                this.Direct = direct;
            }
        }

        private MediumLevelILFunction? translationSourceFunction;

        private readonly Dictionary<MediumLevelILExpressionIndex, List<ExpressionTranslationTarget>>
            expressionTranslations =
                new Dictionary<MediumLevelILExpressionIndex, List<ExpressionTranslationTarget>>();

        private readonly Dictionary<MediumLevelILInstructionIndex, List<InstructionTranslationTarget>>
            instructionTranslations =
                new Dictionary<MediumLevelILInstructionIndex, List<InstructionTranslationTarget>>();

        private void BeginTranslation(MediumLevelILFunction source)
        {
            this.translationSourceFunction = source;
            this.expressionTranslations.Clear();
            this.instructionTranslations.Clear();
        }

        private void RecordExpressionTranslation(
            MediumLevelILExpressionIndex expressionIndex,
            SourceLocation? location)
        {
            if (null == this.translationSourceFunction || null == location)
            {
                return;
            }

            MediumLevelILInstruction? source = location.SourceMediumLevelILInstruction;
            if (null == source)
            {
                return;
            }

            MediumLevelILExpressionIndex sourceIndex = source.ExpressionIndex;
            if (!this.expressionTranslations.TryGetValue(
                sourceIndex,
                out List<ExpressionTranslationTarget>? targets))
            {
                targets = new List<ExpressionTranslationTarget>();
                this.expressionTranslations.Add(sourceIndex, targets);
            }

            targets.Add(new ExpressionTranslationTarget(expressionIndex, location.ILDirect));
        }

        private void RecordInstructionTranslation(
            MediumLevelILInstructionIndex instructionIndex,
            SourceLocation? location)
        {
            if (null == this.translationSourceFunction || null == location)
            {
                return;
            }

            MediumLevelILInstruction? source = location.SourceMediumLevelILInstruction;
            if (null == source)
            {
                return;
            }

            MediumLevelILInstructionIndex sourceIndex = source.InstructionIndex;
            if (!this.instructionTranslations.TryGetValue(
                sourceIndex,
                out List<InstructionTranslationTarget>? targets))
            {
                targets = new List<InstructionTranslationTarget>();
                this.instructionTranslations.Add(sourceIndex, targets);
            }

            targets.Add(new InstructionTranslationTarget(instructionIndex, location.ILDirect));
        }

        /// <summary>
        /// Gets the LLIL SSA instruction-to-MLIL instruction mapping accumulated by a translation,
        /// or reconstructs the installed mapping from this function when requested.
        /// </summary>
        public Dictionary<LowLevelILInstructionIndex, MediumLevelILInstructionIndex>
            GetLLILSSAToMLILInstructionMap(bool fromTranslation = true)
        {
            Dictionary<LowLevelILInstructionIndex, MediumLevelILInstructionIndex> result =
                new Dictionary<LowLevelILInstructionIndex, MediumLevelILInstructionIndex>();

            if (fromTranslation)
            {
                this.BuildTranslatedInstructionMap(result);
            }
            else
            {
                this.BuildInstalledInstructionMap(result);
            }

            return result;
        }

        private void BuildTranslatedInstructionMap(
            Dictionary<LowLevelILInstructionIndex, MediumLevelILInstructionIndex> result)
        {
            MediumLevelILFunction? sourceFunction = this.translationSourceFunction;
            if (null == sourceFunction)
            {
                return;
            }

            foreach (KeyValuePair<MediumLevelILInstructionIndex, List<InstructionTranslationTarget>> entry
                in this.instructionTranslations)
            {
                LowLevelILInstructionIndex lowerIndex =
                    NativeMethods.BNGetLowLevelILInstructionIndex(
                        sourceFunction.DangerousGetHandle(),
                        entry.Key);

                if (LowLevelILInstructionIndex.Invalid == lowerIndex)
                {
                    continue;
                }

                foreach (InstructionTranslationTarget target in entry.Value)
                {
                    if (target.Direct)
                    {
                        result[lowerIndex] = target.InstructionIndex;
                    }
                }
            }
        }

        private void BuildInstalledInstructionMap(
            Dictionary<LowLevelILInstructionIndex, MediumLevelILInstructionIndex> result)
        {
            for (ulong rawIndex = 0; rawIndex < this.InstructionCount; rawIndex++)
            {
                MediumLevelILInstructionIndex higherIndex =
                    (MediumLevelILInstructionIndex)rawIndex;
                LowLevelILInstructionIndex lowerIndex =
                    NativeMethods.BNGetLowLevelILInstructionIndex(
                        this.DangerousGetHandle(),
                        higherIndex);

                if (LowLevelILInstructionIndex.Invalid != lowerIndex)
                {
                    result[lowerIndex] = higherIndex;
                }
            }
        }

        /// <summary>
        /// Gets the LLIL SSA expression-to-MLIL expression mapping accumulated by a translation,
        /// or reconstructs the installed mapping from this function when requested.
        /// </summary>
        public List<LLILSSAToMLILExpressionMap> GetLLILSSAToMLILExpressionMap(
            bool fromTranslation = true)
        {
            List<LLILSSAToMLILExpressionMap> result =
                new List<LLILSSAToMLILExpressionMap>();

            if (fromTranslation)
            {
                this.BuildTranslatedExpressionMap(result);
            }
            else
            {
                this.BuildInstalledExpressionMap(result);
            }

            return result;
        }

        private void BuildTranslatedExpressionMap(List<LLILSSAToMLILExpressionMap> result)
        {
            MediumLevelILFunction? sourceFunction = this.translationSourceFunction;
            if (null == sourceFunction)
            {
                return;
            }

            LowLevelILFunction? sourceLowLevelIL = sourceFunction.LowLevelIL;
            if (null == sourceLowLevelIL)
            {
                return;
            }

            LowLevelILFunction sourceLowLevelILSSA = sourceLowLevelIL.SSAForm;

            foreach (KeyValuePair<MediumLevelILExpressionIndex, List<ExpressionTranslationTarget>> entry
                in this.expressionTranslations)
            {
                foreach (ExpressionTranslationTarget target in entry.Value)
                {
                    AppendExpressionMappings(
                        sourceFunction,
                        sourceLowLevelILSSA,
                        entry.Key,
                        target.ExpressionIndex,
                        target.Direct,
                        result);
                }
            }
        }

        private void BuildInstalledExpressionMap(List<LLILSSAToMLILExpressionMap> result)
        {
            LowLevelILFunction? sourceLowLevelIL = this.LowLevelIL;
            if (null == sourceLowLevelIL)
            {
                return;
            }

            LowLevelILFunction sourceLowLevelILSSA = sourceLowLevelIL.SSAForm;

            foreach (MediumLevelILInstruction instruction in this.Instructions)
            {
                foreach (MediumLevelILInstruction expression in instruction.Traverse(ReturnInstruction))
                {
                    AppendExpressionMappings(
                        this,
                        sourceLowLevelILSSA,
                        expression.ExpressionIndex,
                        expression.ExpressionIndex,
                        true,
                        result);
                }
            }
        }

        private static MediumLevelILInstruction ReturnInstruction(
            MediumLevelILInstruction instruction)
        {
            return instruction;
        }

        private static void AppendExpressionMappings(
            MediumLevelILFunction mappingFunction,
            LowLevelILFunction lowerSSAFunction,
            MediumLevelILExpressionIndex sourceExpressionIndex,
            MediumLevelILExpressionIndex targetExpressionIndex,
            bool targetIsDirect,
            List<LLILSSAToMLILExpressionMap> result)
        {
            LowLevelILExpressionIndex directLowerIndex = NativeMethods.BNGetLowLevelILExprIndex(
                mappingFunction.DangerousGetHandle(),
                sourceExpressionIndex);
            LowLevelILExpressionIndex[] lowerIndexes = GetLowLevelExpressionIndexes(
                mappingFunction,
                sourceExpressionIndex);

            foreach (LowLevelILExpressionIndex lowerIndex in lowerIndexes)
            {
                MediumLevelILExpressionIndex directHigherIndex =
                    NativeMethods.BNGetMediumLevelILExprIndex(
                        lowerSSAFunction.DangerousGetHandle(),
                        lowerIndex);
                MediumLevelILExpressionIndex[] higherIndexes = GetMediumLevelExpressionIndexes(
                    lowerSSAFunction,
                    lowerIndex);

                bool mapsLowerToHigher = ContainsExpressionIndex(
                    higherIndexes,
                    sourceExpressionIndex);
                bool lowerToHigherDirect = targetIsDirect
                    && directHigherIndex == sourceExpressionIndex;
                bool higherToLowerDirect = targetIsDirect
                    && lowerIndex == directLowerIndex;

                result.Add(new LLILSSAToMLILExpressionMap(
                    lowerIndex,
                    targetExpressionIndex,
                    mapsLowerToHigher,
                    true,
                    lowerToHigherDirect,
                    higherToLowerDirect));
            }
        }

        private static LowLevelILExpressionIndex[] GetLowLevelExpressionIndexes(
            MediumLevelILFunction function,
            MediumLevelILExpressionIndex expressionIndex)
        {
            IntPtr arrayPointer = NativeMethods.BNGetLowLevelILExprIndexes(
                function.DangerousGetHandle(),
                expressionIndex,
                out ulong arrayLength);

            return UnsafeUtils.TakeNumberArray<LowLevelILExpressionIndex>(
                arrayPointer,
                arrayLength,
                NativeMethods.BNFreeILInstructionList);
        }

        private static MediumLevelILExpressionIndex[] GetMediumLevelExpressionIndexes(
            LowLevelILFunction function,
            LowLevelILExpressionIndex expressionIndex)
        {
            IntPtr arrayPointer = NativeMethods.BNGetMediumLevelILExprIndexes(
                function.DangerousGetHandle(),
                expressionIndex,
                out ulong arrayLength);

            return UnsafeUtils.TakeNumberArray<MediumLevelILExpressionIndex>(
                arrayPointer,
                arrayLength,
                NativeMethods.BNFreeILInstructionList);
        }

        private static bool ContainsExpressionIndex(
            MediumLevelILExpressionIndex[] indexes,
            MediumLevelILExpressionIndex expected)
        {
            foreach (MediumLevelILExpressionIndex index in indexes)
            {
                if (expected == index)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
