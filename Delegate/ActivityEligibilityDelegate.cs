using System;

namespace BinaryNinja
{
	// Eligibility check for an Activity: given the Activity and the AnalysisContext it would run
	// under, return whether it may run. Mirrors Python Activity eligibility (workflow.py).
	public delegate bool ActivityEligibility(Activity activity, AnalysisContext analysisContext);

	internal static partial class NativeDelegates
	{
		// bool (*eligibilityCallback)(void* ctxt, BNActivity* activity, BNAnalysisContext* ac)
		public delegate bool ActivityEligibilityDelegate(IntPtr context, IntPtr activity, IntPtr analysisContext);
	}

	internal static partial class UnsafeUtils
	{
		// Adapts a public ActivityEligibility into the native eligibility-callback shape, wrapping
		// the raw BNActivity* and BNAnalysisContext* via their NewFromHandle factories (borrowed
		// handles; each wrapper owns the reference it adds and frees it on dispose).
		internal sealed class ActivityEligibilityContext
		{
			private readonly ActivityEligibility m_callback;

			internal ActivityEligibilityContext(ActivityEligibility callback)
			{
				this.m_callback = callback;
			}

			internal bool OnEligibility(IntPtr context, IntPtr activityHandle, IntPtr analysisContextHandle)
			{
				Activity? activity = Activity.NewFromHandle(activityHandle);
				AnalysisContext? analysisContext = AnalysisContext.NewFromHandle(analysisContextHandle);

				// If either handle is missing the activity is treated as not eligible.
				if (null == activity || null == analysisContext)
				{
					return false;
				}

				return this.m_callback(activity, analysisContext);
			}
		}

		internal static NativeDelegates.ActivityEligibilityDelegate WrapActivityEligibility(ActivityEligibility callback)
		{
			ActivityEligibilityContext context = new ActivityEligibilityContext(callback);

			return new NativeDelegates.ActivityEligibilityDelegate(context.OnEligibility);
		}
	}
}
