using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class NativeDelegates
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNWorkerAction(IntPtr context);
	}

	public static partial class Core
	{
		private enum WorkerQueueKind
		{
			Default,
			Priority,
			Interactive
		}

		private static readonly NativeDelegates.BNWorkerAction workerActionCallback =
			new NativeDelegates.BNWorkerAction(Core.InvokeWorkerAction);

		/// <summary>Enqueues a named action on the default analysis worker queue.</summary>
		public static void WorkerEnqueue(Action action, string name = "")
		{
			Core.EnqueueWorker(WorkerQueueKind.Default, action, name);
		}

		/// <summary>Enqueues a named action ahead of normal analysis worker work.</summary>
		public static void WorkerPriorityEnqueue(Action action, string name = "")
		{
			Core.EnqueueWorker(WorkerQueueKind.Priority, action, name);
		}

		/// <summary>Enqueues a named action on the interactive worker queue.</summary>
		public static void WorkerInteractiveEnqueue(Action action, string name = "")
		{
			Core.EnqueueWorker(WorkerQueueKind.Interactive, action, name);
		}

		private static void EnqueueWorker(WorkerQueueKind kind, Action action, string name)
		{
			if (null == action)
			{
				throw new ArgumentNullException(nameof(action));
			}

			GCHandle context = GCHandle.Alloc(action);
			IntPtr contextPointer = GCHandle.ToIntPtr(context);
			IntPtr callback = Marshal.GetFunctionPointerForDelegate(
				Core.workerActionCallback
			);

			try
			{
				switch (kind)
				{
					case WorkerQueueKind.Default:
						NativeMethods.BNWorkerEnqueueNamed(
							contextPointer,
							callback,
							name ?? string.Empty
						);
						break;
					case WorkerQueueKind.Priority:
						NativeMethods.BNWorkerPriorityEnqueueNamed(
							contextPointer,
							callback,
							name ?? string.Empty
						);
						break;
					case WorkerQueueKind.Interactive:
						NativeMethods.BNWorkerInteractiveEnqueueNamed(
							contextPointer,
							callback,
							name ?? string.Empty
						);
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(kind));
				}
			}
			catch
			{
				if (context.IsAllocated)
				{
					context.Free();
				}

				throw;
			}
		}

		private static void InvokeWorkerAction(IntPtr contextPointer)
		{
			GCHandle context = GCHandle.FromIntPtr(contextPointer);
			try
			{
				Action? action = context.Target as Action;
				if (null != action)
				{
					action();
				}
			}
			catch (Exception exception)
			{
				Core.LogError("Unhandled exception in worker action: {0}", exception);
			}
			finally
			{
				if (context.IsAllocated)
				{
					context.Free();
				}
			}
		}
	}
}
