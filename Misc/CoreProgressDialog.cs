using System;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	/// <summary>
	/// Performs work while reporting progress through a native progress dialog.
	/// </summary>
	/// <param name="progress">Callback used to update progress and detect cancellation.</param>
	public delegate void ProgressTaskDelegate(ProgressDelegate progress);

	public static partial class Core
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate void NativeProgressTask(
			IntPtr taskContext,
			IntPtr progress,
			IntPtr progressContext);

		private sealed class ProgressTaskContext
		{
			private readonly ProgressTaskDelegate task;
			private ExceptionDispatchInfo? exception;

			internal ProgressTaskContext(ProgressTaskDelegate task)
			{
				this.task = task;
			}

			internal void Invoke(
				IntPtr taskContext,
				IntPtr progress,
				IntPtr progressContext)
			{
				try
				{
					NativeDelegates.BNProgressFunction nativeProgress =
						Marshal.GetDelegateForFunctionPointer<NativeDelegates.BNProgressFunction>(progress);
					ProgressCallbackProxy proxy = new ProgressCallbackProxy(
						nativeProgress,
						progressContext);
					this.task(proxy.Invoke);
					GC.KeepAlive(nativeProgress);
				}
				catch (Exception caught)
				{
					this.exception = ExceptionDispatchInfo.Capture(caught);
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

		private sealed class ProgressCallbackProxy
		{
			private readonly NativeDelegates.BNProgressFunction progress;
			private readonly IntPtr context;

			internal ProgressCallbackProxy(
				NativeDelegates.BNProgressFunction progress,
				IntPtr context)
			{
				this.progress = progress;
				this.context = context;
			}

			internal bool Invoke(ulong current, ulong total)
			{
				return this.progress(this.context, current, total);
			}
		}

		/// <summary>
		/// Runs a task while the active interaction handler displays progress and permits cancellation.
		/// </summary>
		/// <param name="title">Progress dialog title.</param>
		/// <param name="canCancel">Whether the interaction handler may cancel the task.</param>
		/// <param name="task">Task that receives a progress-reporting callback.</param>
		/// <returns>True when the task completed without cancellation.</returns>
		public static bool RunProgressDialog(
			string title,
			bool canCancel,
			ProgressTaskDelegate task)
		{
			if (null == title)
			{
				throw new ArgumentNullException(nameof(title));
			}

			if (null == task)
			{
				throw new ArgumentNullException(nameof(task));
			}

			ProgressTaskContext taskContext = new ProgressTaskContext(task);
			NativeProgressTask nativeTask = taskContext.Invoke;
			bool result;
			try
			{
				result = NativeMethods.BNRunProgressDialog(
					title,
					canCancel,
					Marshal.GetFunctionPointerForDelegate<NativeProgressTask>(nativeTask),
					IntPtr.Zero);
			}
			finally
			{
				GC.KeepAlive(nativeTask);
			}

			taskContext.ThrowIfFailed();
			return result;
		}
	}
}
