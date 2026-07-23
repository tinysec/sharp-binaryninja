using System;

namespace BinaryNinja
{
    public sealed partial class HighLevelILTokenEmitter
    {
        /// <summary>Restores a previous expression when disposed.</summary>
        public sealed class CurrentExpressionScope : IDisposable
        {
            private HighLevelILTokenEmitter? emitter;

            private readonly TokenEmitterExpression previous;

            internal CurrentExpressionScope(
                HighLevelILTokenEmitter emitter,
                TokenEmitterExpression expression
            )
            {
                this.emitter = emitter;
                this.previous = emitter.SetCurrentExpr(expression);
            }

            public void Dispose()
            {
                HighLevelILTokenEmitter? current = this.emitter;
                if (null == current)
                {
                    return;
                }

                this.emitter = null;
                current.RestoreCurrentExpr(this.previous);
            }
        }

        /// <summary>Sets the current expression until the scope is disposed.</summary>
        public CurrentExpressionScope SetCurrentExpr(
            HighLevelILInstruction instruction
        )
        {
            return new CurrentExpressionScope(
                this,
                TokenEmitterExpression.FromInstruction(instruction)
            );
        }

        /// <summary>Prepends a blank collapse indicator.</summary>
        public void PrependCollapseIndicator()
        {
            this.PrependCollapseBlankIndicator();
        }

        /// <summary>Prepends a collapse indicator for an instruction region.</summary>
        public void PrependCollapseIndicator(
            Function? function,
            HighLevelILInstruction instruction,
            ulong discriminator = 0
        )
        {
            if (null == instruction)
            {
                throw new ArgumentNullException(nameof(instruction));
            }

            if (!this.HasCollapsableRegions)
            {
                return;
            }

            InstructionTextTokenContext context =
                InstructionTextTokenContext.ContentCollapsiblePadding;
            if (instruction.CanCollapse)
            {
                bool collapsed = null != function && function.IsRegionCollapsed(
                    instruction.GetInstructionHash(discriminator)
                );
                context = collapsed
                    ? InstructionTextTokenContext.ContentCollapsedContext
                    : InstructionTextTokenContext.ContentExpandedContext;
            }

            this.PrependCollapseIndicator(
                context,
                instruction.GetInstructionHash(discriminator)
            );
        }
    }
}
