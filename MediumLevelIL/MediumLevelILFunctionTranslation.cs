using System;

namespace BinaryNinja
{
	/// <summary>
	/// Translates one top-level MLIL instruction into a writable destination function.
	/// </summary>
	public delegate MediumLevelILExpressionIndex MediumLevelILTranslationHandler(
		MediumLevelILFunction destination,
		MediumLevelILBasicBlock sourceBlock,
		MediumLevelILInstruction sourceInstruction);

	public sealed partial class MediumLevelILFunction
	{
		/// <summary>
		/// Clones this function through a per-instruction translation handler.
		/// </summary>
		/// <remarks>
		/// The returned function remains writable, matching Python
		/// <c>MediumLevelILFunction.translate</c>. The caller can finalize it or install it through an
		/// analysis context. The handler should normally call
		/// <see cref="MediumLevelILInstruction.CopyTo(MediumLevelILFunction)"/> for instructions it
		/// does not replace.
		/// </remarks>
		public MediumLevelILFunction Translate(MediumLevelILTranslationHandler expressionHandler)
		{
			if (null == expressionHandler)
			{
				throw new ArgumentNullException(nameof(expressionHandler));
			}

			LowLevelILFunction? sourceLowLevelIL = this.LowLevelIL;
			if (null == sourceLowLevelIL)
			{
				throw new InvalidOperationException(
					"An MLIL function requires a source LLIL function for translation.");
			}

			using (sourceLowLevelIL)
			{
				MediumLevelILFunction destination = sourceLowLevelIL.CreateMediumLevelIL(
					this.Architecture);

				try
				{
					destination.PrepareToCopyFunction(this);

					foreach (MediumLevelILBasicBlock block in this.BasicBlocks)
					{
						destination.PrepareToCopyBasicBlock(block);

						foreach (MediumLevelILInstruction instruction in block.Instructions)
						{
							destination.SetCurrentAddress(instruction.Address, block.Architecture);
							MediumLevelILExpressionIndex expression = expressionHandler(
								destination,
								block,
								instruction);
							destination.AddInstruction(expression, instruction.Location);
						}
					}

					return destination;
				}
				catch
				{
					destination.Dispose();
					throw;
				}
			}
		}
	}
}
