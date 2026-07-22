using System;
using System.Collections.Generic;

namespace BinaryNinja
{
	public sealed partial class MediumLevelILFunction
	{
		/// <summary>
		/// Traverses every top-level instruction and sub-expression in this function.
		/// </summary>
		/// <typeparam name="T">
		/// The callback result type. A null result filters the current node from the sequence.
		/// </typeparam>
		public IEnumerable<T> Traverse<T>(
			Func<MediumLevelILInstruction, T?> callback,
			bool shallow = true)
			where T : class
		{
			if (null == callback)
			{
				throw new ArgumentNullException(nameof(callback));
			}

			foreach (MediumLevelILInstruction instruction in this.Instructions)
			{
				foreach (T result in instruction.Traverse(callback, shallow))
				{
					yield return result;
				}
			}
		}
	}
}
