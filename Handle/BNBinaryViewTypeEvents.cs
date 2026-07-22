using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class NativeDelegates
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNBinaryViewEventCallback(
			IntPtr context,
			IntPtr view
		);
	}

	public sealed partial class BinaryViewType
	{
		private static readonly object eventLock = new object();

		private static readonly List<BinaryViewEventContext> eventContexts =
			new List<BinaryViewEventContext>();

		/// <summary>Registers a callback invoked when a binary view is finalized.</summary>
		public static void RegisterBinaryViewFinalizationEvent(
			Action<BinaryView> callback
		)
		{
			BinaryViewType.RegisterBinaryViewEvent(
				BinaryViewEventType.BinaryViewFinalizationEvent,
				callback
			);
		}

		/// <summary>
		/// Registers a callback invoked when a binary view completes initial analysis.
		/// </summary>
		public static void RegisterBinaryViewInitialAnalysisCompletionEvent(
			Action<BinaryView> callback
		)
		{
			BinaryViewType.RegisterBinaryViewEvent(
				BinaryViewEventType.BinaryViewInitialAnalysisCompletionEvent,
				callback
			);
		}

		internal static void RegisterBinaryViewEvent(
			BinaryViewEventType type,
			Action<BinaryView> callback
		)
		{
			if (null == callback)
			{
				throw new ArgumentNullException(nameof(callback));
			}

			BinaryViewEventContext context = new BinaryViewEventContext(callback);
			lock (BinaryViewType.eventLock)
			{
				BinaryViewType.eventContexts.Add(context);
			}

			NativeMethods.BNRegisterBinaryViewEvent(
				type,
				Marshal.GetFunctionPointerForDelegate(context.NativeCallback),
				IntPtr.Zero
			);
		}
	}

	internal sealed class BinaryViewEventContext
	{
		private readonly Action<BinaryView> callback;

		internal NativeDelegates.BNBinaryViewEventCallback NativeCallback { get; }

		internal BinaryViewEventContext(Action<BinaryView> callback)
		{
			this.callback = callback;
			this.NativeCallback = new NativeDelegates.BNBinaryViewEventCallback(
				this.Invoke
			);
		}

		private void Invoke(IntPtr context, IntPtr view)
		{
			try
			{
				BinaryView managedView = BinaryView.MustTakeHandle(
					NativeMethods.BNNewViewReference(view)
				);
				this.callback(managedView);
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in a BinaryView event callback: {0}",
					exception
				);
			}
		}
	}
}
