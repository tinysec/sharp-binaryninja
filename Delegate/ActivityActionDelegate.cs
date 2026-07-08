using System;

namespace BinaryNinja
{
	// Action invoked when an Activity runs. The core hands the action the AnalysisContext the
	// activity is executing under. Mirrors Python Activity.action (workflow.py): the raw
	// BNAnalysisContext* is wrapped before reaching managed code.
	public delegate void ActivityAction(AnalysisContext analysisContext);

	internal static partial class NativeDelegates
	{
		// void (*actionCallback)(void* ctxt, BNAnalysisContext* ac)
		public delegate void ActivityActionDelegate(IntPtr context, IntPtr analysisContext);
	}

	internal static partial class UnsafeUtils
	{
		// Adapts a public ActivityAction into the native action-callback shape, wrapping the raw
		// BNAnalysisContext* via AnalysisContext.NewFromHandle. The core passes a borrowed handle;
		// NewFromHandle adds a reference so the wrapper owns its own and frees it on dispose.
		internal sealed class ActivityActionContext
		{
			private readonly ActivityAction m_callback;

			internal ActivityActionContext(ActivityAction callback)
			{
				this.m_callback = callback;
			}

			internal void OnAction(IntPtr context, IntPtr analysisContextHandle)
			{
				AnalysisContext? analysisContext = AnalysisContext.NewFromHandle(analysisContextHandle);

				// The core always provides a non-null AnalysisContext to a running activity; skip
				// defensively if it does not.
				if (null == analysisContext)
				{
					return;
				}

				this.m_callback(analysisContext);
			}
		}

		internal static NativeDelegates.ActivityActionDelegate WrapActivityAction(ActivityAction callback)
		{
			ActivityActionContext context = new ActivityActionContext(callback);

			return new NativeDelegates.ActivityActionDelegate(context.OnAction);
		}
	}
}
