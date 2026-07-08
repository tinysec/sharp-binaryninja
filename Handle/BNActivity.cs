using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class Activity : AbstractSafeHandle<Activity>
	{
	    // The core stores the action and eligibility function pointers for the Activity's lifetime.
	    // Rooting the caller-supplied delegates on the instance keeps them alive as long as the
	    // Activity; without this, a temporary delegate passed to Create would be GC-eligible after
	    // Create returns and the next activity callback would dereference freed memory.
	    // Holds the NATIVE wrapper delegate (not the user delegate): the wrapper owns the GC root
	    // for the user callback via its holder context, and its function pointer is what the core
	    // invokes. Rooting the wrapper for the Activity's lifetime keeps the callback reachable.
	    private NativeDelegates.ActivityActionDelegate? m_action = null;

	    private NativeDelegates.ActivityEligibilityDelegate? m_eligibilityHandler = null;

	    internal Activity(IntPtr handle , bool owner)
		    : base(handle , owner)
	    {
	        
	    }
	    
	    internal static Activity? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Activity(
			    NativeMethods.BNNewActivityReference(handle) ,
			    true
		    );
	    }
	    
	    internal static Activity MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Activity(
			    NativeMethods.BNNewActivityReference(handle) ,
			    true
		    );
	    }
	    
	    internal static Activity? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Activity(handle, true);
	    }
	    
	    internal static Activity MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Activity(handle, true);
	    }
	    
	    internal static Activity? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Activity(handle, false);
	    }
	    
	    internal static Activity MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Activity(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeActivity(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }

	    public string Name
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNActivityGetName(this.handle)
			    );
		    }
	    }

	    /// <summary>
	    /// Creates a new Activity with the given configuration name and action callback.
	    /// </summary>
	    /// <param name="configuration">The configuration name for the activity.</param>
	    /// <param name="action">The action callback delegate, or null for no action.</param>
	    /// <returns>A new owned Activity, or null on failure.</returns>
	    public static Activity? Create(string configuration , ActivityAction? action = null)
	    {
		    // 1. Wrap the action into the native (ctxt, analysisContext) callback shape, then take a
		    // function pointer from the wrapper. The wrapper carries the AnalysisContext the core
		    // hands the callback (Action<IntPtr> dropped it, so the action never saw its context).
		    NativeDelegates.ActivityActionDelegate? actionWrapper = null;

		    if (null != action)
		    {
			    actionWrapper = UnsafeUtils.WrapActivityAction(action);
		    }

		    IntPtr actionPtr = null != actionWrapper
			    ? Marshal.GetFunctionPointerForDelegate<NativeDelegates.ActivityActionDelegate>(actionWrapper)
			    : IntPtr.Zero;

		    // 2. Create the activity via the native API.
		    Activity? activity = Activity.TakeHandle(
			    NativeMethods.BNCreateActivity(
				    configuration ?? string.Empty ,
				    IntPtr.Zero ,
				    actionPtr
			    )
		    );

		    // 3. Root the wrapper (not the user delegate) on the instance for the Activity's lifetime;
		    // the wrapper holds the user callback via its context, and its function pointer is what the
		    // core invokes (see m_action).
		    if (null != activity && null != actionWrapper)
		    {
			    activity.m_action = actionWrapper;
		    }

		    return activity;
	    }

	    /// <summary>
	    /// Creates a new Activity with the given configuration name, action callback, and eligibility handler.
	    /// </summary>
	    /// <param name="configuration">The configuration name for the activity.</param>
	    /// <param name="action">The action callback delegate, or null for no action.</param>
	    /// <param name="eligibilityHandler">The eligibility handler delegate, or null for no handler.</param>
	    /// <returns>A new owned Activity, or null on failure.</returns>
	    public static Activity? CreateWithEligibility(
		    string configuration ,
		    ActivityAction? action = null ,
		    ActivityEligibility? eligibilityHandler = null
	    )
	    {
		    // 1. Wrap the action into the native (ctxt, analysisContext) callback shape.
		    NativeDelegates.ActivityActionDelegate? actionWrapper = null;

		    if (null != action)
		    {
			    actionWrapper = UnsafeUtils.WrapActivityAction(action);
		    }

		    IntPtr actionPtr = null != actionWrapper
			    ? Marshal.GetFunctionPointerForDelegate<NativeDelegates.ActivityActionDelegate>(actionWrapper)
			    : IntPtr.Zero;

		    // 2. Wrap the eligibility handler into the native (ctxt, activity, analysisContext) -> bool
		    // callback shape. Action<IntPtr> dropped both handles and returned garbage bool, so the
		    // handler never received its arguments and eligibility was effectively random.
		    NativeDelegates.ActivityEligibilityDelegate? eligibilityWrapper = null;

		    if (null != eligibilityHandler)
		    {
			    eligibilityWrapper = UnsafeUtils.WrapActivityEligibility(eligibilityHandler);
		    }

		    IntPtr eligibilityPtr = null != eligibilityWrapper
			    ? Marshal.GetFunctionPointerForDelegate<NativeDelegates.ActivityEligibilityDelegate>(eligibilityWrapper)
			    : IntPtr.Zero;

		    // 3. Create the activity with eligibility via the native API.
		    Activity? activity = Activity.TakeHandle(
			    NativeMethods.BNCreateActivityWithEligibility(
				    configuration ?? string.Empty ,
				    IntPtr.Zero ,
				    actionPtr ,
				    eligibilityPtr
			    )
		    );

		    // 4. Root both wrappers on the instance for the Activity's lifetime; the wrappers hold the
		    // user callbacks via their contexts, and their function pointers are what the core invokes
		    // (see m_action / m_eligibilityHandler).
		    if (null != activity)
		    {
			    activity.m_action = actionWrapper;

			    activity.m_eligibilityHandler = eligibilityWrapper;
		    }

		    return activity;
	    }
	}
}