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
	    private Action<IntPtr>? m_action = null;

	    private Action<IntPtr>? m_eligibilityHandler = null;

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
	    public static Activity? Create(string configuration , Action<IntPtr>? action = null)
	    {
		    // 1. Marshal the action delegate to a function pointer if provided.
		    IntPtr actionPtr = IntPtr.Zero;

		    if (null != action)
		    {
			    actionPtr = Marshal.GetFunctionPointerForDelegate<Action<IntPtr>>(action);
		    }

		    // 2. Create the activity via the native API.
		    Activity? activity = Activity.TakeHandle(
			    NativeMethods.BNCreateActivity(
				    configuration ?? string.Empty ,
				    IntPtr.Zero ,
				    actionPtr
			    )
		    );

		    // 3. Root the action delegate on the instance for the Activity's lifetime; otherwise the
		    // function pointer handed to the core would dangle once Create returns and the caller's
		    // delegate is collected (see m_action).
		    if (null != activity && null != action)
		    {
			    activity.m_action = action;
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
		    Action<IntPtr>? action = null ,
		    Action<IntPtr>? eligibilityHandler = null
	    )
	    {
		    // 1. Marshal the action delegate to a function pointer if provided.
		    IntPtr actionPtr = IntPtr.Zero;

		    if (null != action)
		    {
			    actionPtr = Marshal.GetFunctionPointerForDelegate<Action<IntPtr>>(action);
		    }

		    // 2. Marshal the eligibility handler delegate to a function pointer if provided.
		    IntPtr eligibilityPtr = IntPtr.Zero;

		    if (null != eligibilityHandler)
		    {
			    eligibilityPtr = Marshal.GetFunctionPointerForDelegate<Action<IntPtr>>(eligibilityHandler);
		    }

		    // 3. Create the activity with eligibility via the native API.
		    Activity? activity = Activity.TakeHandle(
			    NativeMethods.BNCreateActivityWithEligibility(
				    configuration ?? string.Empty ,
				    IntPtr.Zero ,
				    actionPtr ,
				    eligibilityPtr
			    )
		    );

		    // 4. Root both delegates on the instance for the Activity's lifetime; otherwise the
		    // function pointers handed to the core would dangle once CreateWithEligibility returns
		    // and the caller's delegates are collected (see m_action / m_eligibilityHandler).
		    if (null != activity)
		    {
			    activity.m_action = action;

			    activity.m_eligibilityHandler = eligibilityHandler;
		    }

		    return activity;
	    }
	}
}