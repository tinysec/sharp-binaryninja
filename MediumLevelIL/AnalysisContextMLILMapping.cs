using System;
using System.Collections.Generic;

namespace BinaryNinja
{
    /// <summary>
    /// MLIL mapping support for analysis-context translation activities.
    /// </summary>
    public sealed partial class AnalysisContext
    {
        /// <summary>
        /// Sets the MLIL function and its LLIL SSA instruction/expression mappings.
        /// When either mapping is omitted, both are rebuilt from the source-aware locations recorded
        /// after <see cref="MediumLevelILFunction.PrepareToCopyFunction"/> by
        /// <see cref="MediumLevelILFunction.AddExpression"/> and
        /// <see cref="MediumLevelILFunction.AddInstruction"/>.
        /// </summary>
        public void SetMediumLevelILFunction(
            MediumLevelILFunction mlilFunction,
            IReadOnlyDictionary<LowLevelILInstructionIndex, MediumLevelILInstructionIndex>?
                instructionMap,
            IReadOnlyList<LLILSSAToMLILExpressionMap>? expressionMap)
        {
            if (null == mlilFunction)
            {
                throw new ArgumentNullException(nameof(mlilFunction));
            }

            IReadOnlyDictionary<LowLevelILInstructionIndex, MediumLevelILInstructionIndex>
                effectiveInstructionMap;
            IReadOnlyList<LLILSSAToMLILExpressionMap> effectiveExpressionMap;

            if (null == instructionMap || null == expressionMap)
            {
                effectiveInstructionMap =
                    mlilFunction.GetLLILSSAToMLILInstructionMap(true);
                effectiveExpressionMap =
                    mlilFunction.GetLLILSSAToMLILExpressionMap(true);
            }
            else
            {
                effectiveInstructionMap = instructionMap;
                effectiveExpressionMap = expressionMap;
            }

            ulong[] nativeInstructionMap = BuildInstructionMap(effectiveInstructionMap);
            BNExprMapInfo[] nativeExpressionMap = BuildExpressionMap(effectiveExpressionMap);

            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                IntPtr instructionMapPointer =
                    allocator.AllocStructArray<ulong>(nativeInstructionMap);
                IntPtr expressionMapPointer =
                    allocator.AllocStructArray<BNExprMapInfo>(nativeExpressionMap);

                NativeMethods.BNSetMediumLevelILFunction(
                    this.handle,
                    mlilFunction.DangerousGetHandle(),
                    instructionMapPointer,
                    (ulong)nativeInstructionMap.Length,
                    expressionMapPointer,
                    (ulong)nativeExpressionMap.Length);
            }
        }

        private static ulong[] BuildInstructionMap(
            IReadOnlyDictionary<LowLevelILInstructionIndex, MediumLevelILInstructionIndex> mapping)
        {
            ulong highestIndex = 0;
            bool hasEntries = false;

            foreach (KeyValuePair<LowLevelILInstructionIndex, MediumLevelILInstructionIndex> entry
                in mapping)
            {
                ulong rawIndex = (ulong)entry.Key;
                if (ulong.MaxValue == rawIndex)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(mapping),
                        "The invalid LLIL instruction index cannot be used as a mapping key.");
                }

                if (!hasEntries || highestIndex < rawIndex)
                {
                    highestIndex = rawIndex;
                }

                hasEntries = true;
            }

            if (!hasEntries)
            {
                return Array.Empty<ulong>();
            }

            if ((ulong)(int.MaxValue - 1) < highestIndex)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(mapping),
                    "The LLIL instruction mapping is too large for a managed array.");
            }

            ulong[] result = new ulong[(int)highestIndex + 1];
            for (int index = 0; index < result.Length; index++)
            {
                result[index] = ulong.MaxValue;
            }

            foreach (KeyValuePair<LowLevelILInstructionIndex, MediumLevelILInstructionIndex> entry
                in mapping)
            {
                result[(int)(ulong)entry.Key] = (ulong)entry.Value;
            }

            return result;
        }

        private static BNExprMapInfo[] BuildExpressionMap(
            IReadOnlyList<LLILSSAToMLILExpressionMap> mapping)
        {
            BNExprMapInfo[] result = new BNExprMapInfo[mapping.Count];

            for (int index = 0; index < mapping.Count; index++)
            {
                LLILSSAToMLILExpressionMap? entry = mapping[index];
                if (null == entry)
                {
                    throw new ArgumentException(
                        "The expression mapping cannot contain null entries.",
                        nameof(mapping));
                }

                result[index] = entry.ToNative();
            }

            return result;
        }
    }
}
