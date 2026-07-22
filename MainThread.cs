using System;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace BinaryNinja
{
	internal static partial class NativeDelegates
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNMainThreadActionCallback(IntPtr context);
	}

	/// <summary>
	/// Integrates an application event loop with Binary Ninja main-thread actions.
	/// </summary>
	public abstract class MainThreadActionHandler
	{
		private static MainThreadActionHandler? registeredHandler;

		private readonly NativeDelegates.BNMainThreadAddAction addActionCallback;

		protected MainThreadActionHandler()
		{
			this.addActionCallback = new NativeDelegates.BNMainThreadAddAction(
				this.InvokeAddAction
			);
		}

		/// <summary>Registers this handler as the application's main-thread dispatcher.</summary>
		public void Register()
		{
			BNMainThreadCallbacks callbacks = new BNMainThreadCallbacks();
			callbacks.context = IntPtr.Zero;
			callbacks.addAction = Marshal.GetFunctionPointerForDelegate(
				this.addActionCallback
			);

			MainThreadActionHandler.registeredHandler = this;
			NativeMethods.BNRegisterMainThread(in callbacks);
		}

		/// <summary>Adds an action to the application's main-thread event queue.</summary>
		public abstract void AddMainThreadAction(MainThreadAction action);

		private void InvokeAddAction(IntPtr context, IntPtr actionHandle)
		{
			try
			{
				MainThreadAction action = MainThreadAction.MustTakeHandle(actionHandle);
				this.AddMainThreadAction(action);
			}
			catch (Exception exception)
			{
				Core.LogError("Unhandled exception in main-thread action handler: {0}", exception);
			}
		}
	}

	public static partial class Core
	{
		private sealed class MainThreadInvocation
		{
			internal MainThreadInvocation(Action action, bool waitsForCompletion)
			{
				this.Action = action;
				this.WaitsForCompletion = waitsForCompletion;
			}

			internal Action Action { get; }

			internal bool WaitsForCompletion { get; }

			internal Exception? Exception { get; set; }

			internal int ContextReleased;
		}

		private static readonly NativeDelegates.BNMainThreadActionCallback
			mainThreadActionCallback = new NativeDelegates.BNMainThreadActionCallback(
				Core.InvokeMainThreadAction
			);

		/// <summary>Schedules an action on the Binary Ninja main thread.</summary>
		public static MainThreadAction? ExecuteOnMainThread(Action action)
		{
			if (null == action)
			{
				throw new ArgumentNullException(nameof(action));
			}

			MainThreadInvocation invocation = new MainThreadInvocation(action, false);
			GCHandle context = GCHandle.Alloc(invocation);
			IntPtr contextPointer = GCHandle.ToIntPtr(context);
			IntPtr callbackPointer = Marshal.GetFunctionPointerForDelegate(
				Core.mainThreadActionCallback
			);

			try
			{
				return MainThreadAction.TakeHandle(
					NativeMethods.BNExecuteOnMainThread(contextPointer, callbackPointer)
				);
			}
			catch
			{
				Core.ReleaseMainThreadInvocation(invocation, context);
				throw;
			}
		}

		/// <summary>Runs an action on the Binary Ninja main thread and waits for it.</summary>
		public static void ExecuteOnMainThreadAndWait(Action action)
		{
			if (null == action)
			{
				throw new ArgumentNullException(nameof(action));
			}

			MainThreadInvocation invocation = new MainThreadInvocation(action, true);
			GCHandle context = GCHandle.Alloc(invocation);
			try
			{
				NativeMethods.BNExecuteOnMainThreadAndWait(
					GCHandle.ToIntPtr(context),
					Marshal.GetFunctionPointerForDelegate(Core.mainThreadActionCallback)
				);
			}
			finally
			{
				Core.ReleaseMainThreadInvocation(invocation, context);
			}

			if (null != invocation.Exception)
			{
				ExceptionDispatchInfo.Capture(invocation.Exception).Throw();
			}
		}

		private static void InvokeMainThreadAction(IntPtr contextPointer)
		{
			GCHandle context = GCHandle.FromIntPtr(contextPointer);
			MainThreadInvocation? invocation = context.Target as MainThreadInvocation;
			if (null == invocation)
			{
				return;
			}

			try
			{
				invocation.Action();
			}
			catch (Exception exception)
			{
				if (invocation.WaitsForCompletion)
				{
					invocation.Exception = exception;
				}
				else
				{
					Core.LogError("Unhandled exception in main-thread action: {0}", exception);
				}
			}
			finally
			{
				if (!invocation.WaitsForCompletion)
				{
					Core.ReleaseMainThreadInvocation(invocation, context);
				}
			}
		}

		private static void ReleaseMainThreadInvocation(
			MainThreadInvocation invocation,
			GCHandle context
		)
		{
			if (0 == Interlocked.Exchange(ref invocation.ContextReleased, 1))
			{
				context.Free();
			}
		}
	}
}
