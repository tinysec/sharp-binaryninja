using System;
using System.Runtime.ExceptionServices;

namespace BinaryNinja
{
	internal sealed class ProgressCallbackContext
	{
		private readonly ProgressDelegate? progress;
		private ExceptionDispatchInfo? failure;

		internal ProgressCallbackContext(ProgressDelegate? progress)
		{
			this.progress = progress;
		}

		internal bool Invoke(IntPtr context, ulong progress, ulong total)
		{
			try
			{
				if (null == this.progress)
				{
					return true;
				}

				return this.progress(progress, total);
			}
			catch (Exception exception)
			{
				this.failure = ExceptionDispatchInfo.Capture(exception);
				return false;
			}
		}

		internal void ThrowIfFailed()
		{
			if (null != this.failure)
			{
				this.failure.Throw();
			}
		}
	}
}
