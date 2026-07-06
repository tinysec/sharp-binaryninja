using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public class Workflow :  AbstractSafeHandle<Workflow>
	{
		public Workflow(string name) : base(true)
		{
			this.SetHandle( NativeMethods.BNCreateWorkflow(name) );
		}
		
	    internal Workflow(IntPtr handle , bool owner) : base(owner)
	    {
	        this.SetHandle(handle);
	    }

	    internal static Workflow? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Workflow(
			    NativeMethods.BNNewWorkflowReference(handle) ,
			    true
		    );
	    }
	    
	    internal static Workflow MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Workflow(
			    NativeMethods.BNNewWorkflowReference(handle) ,
			    true
		    );
	    }
	    
	    internal static Workflow? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Workflow(handle, true);
	    }
	    
	    internal static Workflow MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Workflow(handle, true);
	    }
	    
	    internal static Workflow? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Workflow(handle, false);
	    }
	    
	    internal static Workflow MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Workflow(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeWorkflow(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }
	    
	    public static Workflow[] GetWorkflows()
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetWorkflowList(
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeHandleArrayEx(
			    arrayPointer ,
			    arrayLength ,
			    Workflow.MustNewFromHandle ,
			    NativeMethods.BNFreeWorkflowList
		    );
	    }

	    public static Workflow? Get(string name)
	    {
		    return Workflow.TakeHandle(
			    NativeMethods.BNWorkflowGet(name)
		    );
	    }
	    
	    public static Workflow? GetOrCreate(string name)
	    {
		    return Workflow.TakeHandle(
			    NativeMethods.BNWorkflowGetOrCreate(name)
		    );
	    }

	    public bool RegisterWorkflow(string config)
	    {
		    return NativeMethods.BNRegisterWorkflow(this.handle, config);
	    }
	    
	    public Workflow Clone(string name , string activity)
	    {
		    return Workflow.MustTakeHandle(
			    NativeMethods.BNWorkflowClone(
				    this.handle ,
				    name ,
				    activity
			    )
		    );
	    }

	    public bool ContainsActivity(string activity)
	    {
		    return NativeMethods.BNWorkflowContains(this.handle, activity);
	    }
	    
	    public string GetConfiguration(string activity)
	    {
		    return UnsafeUtils.TakeUtf8String(
			    NativeMethods.BNWorkflowGetConfiguration(this.handle , activity)
		    );
	    }
	    
	    public string Name
	    {
		    get
		    {
			    return UnsafeUtils.TakeUtf8String(
				    NativeMethods.BNGetWorkflowName(this.handle)
			    );
		    }
	    }

	    public bool IsRegistered
	    {
		    get
		    {
			    return NativeMethods.BNWorkflowIsRegistered(this.handle);
		    }
	    }
	    
	    public ulong Size
	    {
		    get
		    {
			    return NativeMethods.BNWorkflowSize(this.handle);
		    }
	    }

	    public Activity? GetActivity(string activity)
	    {
		    return Activity.TakeHandle(
			    NativeMethods.BNWorkflowGetActivity(this.handle, activity)
			);
	    }

	    public string[] GetActivityRoots(string activity)
	    {
		    ulong arrayLength = 0;

		    IntPtr arrayPointer = NativeMethods.BNWorkflowGetActivityRoots(
			    this.handle ,
			    activity ,
			    ref arrayLength
		    );

		    return UnsafeUtils.TakeAnsiStringArray(
			    arrayPointer ,
			    arrayLength ,
			    NativeMethods.BNFreeStringList
		    );
	    }
	    
	    public string[] GetSubactivities(string activity , bool immediate)
	    {
		    ulong arrayLength = 0;

		    IntPtr arrayPointer = NativeMethods.BNWorkflowGetSubactivities(
			    this.handle ,
			    activity ,
			    immediate,
			    ref arrayLength
		    );

		    return UnsafeUtils.TakeAnsiStringArray(
			    arrayPointer ,
			    arrayLength ,
			    NativeMethods.BNFreeStringList
		    );
	    }
	    
	    public bool Clear()
	    {
		    return NativeMethods.BNWorkflowClear(this.handle);
	    }
	    
	    public bool Insert(string activity , string[] activities)
	    {
		    string[] safeActivities = activities ?? Array.Empty<string>();

		    // activities is a const char** UTF-8 input block; build it by hand
		    // because .NET cannot apply LPUTF8Str to string[] array elements
		    // (non-ASCII would otherwise corrupt through the system ANSI code page).
		    bool ok;
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr activitiesBlock = allocator.AllocUtf8StringArray(safeActivities);

			    ok = NativeMethods.BNWorkflowInsert(
				    this.handle,
				    activity ,
				    activitiesBlock ,
				    (ulong)safeActivities.Length
			    );
		    }

		    return ok;
	    }
	    
	    public bool InsertAfter(string activity , string[] activities)
	    {
		    string[] safeActivities = activities ?? Array.Empty<string>();

		    // activities is a const char** UTF-8 input block; build it by hand
		    // because .NET cannot apply LPUTF8Str to string[] array elements
		    // (non-ASCII would otherwise corrupt through the system ANSI code page).
		    bool ok;
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr activitiesBlock = allocator.AllocUtf8StringArray(safeActivities);

			    ok = NativeMethods.BNWorkflowInsertAfter(
				    this.handle,
				    activity ,
				    activitiesBlock ,
				    (ulong)safeActivities.Length
			    );
		    }

		    return ok;
	    }
	    
	    public bool Remove(string activity)
	    {
		    return NativeMethods.BNWorkflowRemove(
			    this.handle,
			    activity 
		    );
	    }
	    
	    public bool Replace(string activity , string newActivity)
	    {
		    return NativeMethods.BNWorkflowReplace(
			    this.handle,
			    activity ,
			    newActivity
		    );
	    }

	    public bool AssignSubactivities(string activity , string[] activities)
	    {
		    string[] safeActivities = activities ?? Array.Empty<string>();

		    // activities is a const char** UTF-8 input block; build it by hand
		    // because .NET cannot apply LPUTF8Str to string[] array elements
		    // (non-ASCII would otherwise corrupt through the system ANSI code page).
		    bool ok;
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr activitiesBlock = allocator.AllocUtf8StringArray(safeActivities);

			    ok = NativeMethods.BNWorkflowAssignSubactivities(
				    this.handle,
				    activity ,
				    activitiesBlock ,
				    (ulong)safeActivities.Length
			    );
		    }

		    return ok;
	    }

	    public FlowGraph? GetGraph(string activity = "" , bool sequential = false)
	    {
		    return FlowGraph.TakeHandle(
			    NativeMethods.BNWorkflowGetGraph(
				    this.handle ,
				    activity ,
				    sequential
			    )
		    );
	    }

	    public Activity? RegisterActivity(Activity activity , string[] subactivities)
	    {
		    string[] safeSubactivities = subactivities ?? Array.Empty<string>();

		    // subactivities is a const char** UTF-8 input block; build it by hand
		    // because .NET cannot apply LPUTF8Str to string[] array elements
		    // (non-ASCII would otherwise corrupt through the system ANSI code page).
		    IntPtr activityHandle;
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr subactivitiesBlock = allocator.AllocUtf8StringArray(safeSubactivities);

			    activityHandle = NativeMethods.BNWorkflowRegisterActivity(
				    this.handle ,
				    activity.DangerousGetHandle() ,
				    subactivitiesBlock ,
				    (ulong)safeSubactivities.Length
			    );
		    }

		    return Activity.TakeHandle(activityHandle);
	    }

	    public void ShowReport(string name)
	    {
		    NativeMethods.BNWorkflowShowReport(this.handle , name);
	    }

	    public unsafe Settings? GetEligibilitySettings()
	    {
		    ulong size = 0;

		    IntPtr raw = NativeMethods.BNWorkflowGetEligibilitySettings(
			    this.handle ,
			    (IntPtr)(&size)
		    );

		    return Settings.TakeHandle(raw);
	    }
	}
}