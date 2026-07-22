using System;
using System.Runtime.ExceptionServices;

namespace BinaryNinja
{
	/// <summary>
	/// Receives a match in an in-memory search buffer.
	/// </summary>
	/// <param name="offset">Byte offset of the match in the buffer.</param>
	/// <param name="length">Length of the match in bytes.</param>
	/// <returns>True to continue searching, or false to cancel.</returns>
	public delegate bool SearchMatchDelegate(ulong offset, ulong length);

	internal sealed class SearchMatchContext
	{
		private readonly SearchMatchDelegate callback;
		private ExceptionDispatchInfo? exception;

		internal SearchMatchContext(SearchMatchDelegate callback)
		{
			this.callback = callback;
		}

		internal bool Invoke(IntPtr context, ulong offset, ulong length)
		{
			if (null != this.exception)
			{
				return false;
			}

			try
			{
				return this.callback(offset, length);
			}
			catch (Exception caught)
			{
				this.exception = ExceptionDispatchInfo.Capture(caught);
				return false;
			}
		}

		internal void ThrowIfFailed()
		{
			if (null != this.exception)
			{
				this.exception.Throw();
			}
		}
	}
}
