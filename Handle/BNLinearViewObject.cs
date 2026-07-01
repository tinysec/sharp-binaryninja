using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class LinearViewObject : AbstractSafeHandle<LinearViewObject>
	{
		public LinearViewObject? Parent { get; } = null;
		
		internal LinearViewObject(
			IntPtr handle , 
			bool owner ,
			LinearViewObject? parent = null
		) : base(handle , owner)
		{
			this.Parent = parent;
		}
		
	    internal LinearViewObject(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }
		
	    internal static LinearViewObject? NewFromHandle(IntPtr handle , LinearViewObject? parent = null)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new LinearViewObject(
			    NativeMethods.BNNewLinearViewObjectReference(handle) ,
			    true
		    );
	    }
	    
	    internal static LinearViewObject MustNewFromHandle(IntPtr handle , LinearViewObject? parent = null)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new LinearViewObject(
			    NativeMethods.BNNewLinearViewObjectReference(handle) ,
			    true
		    );
	    }
	    
	    internal static LinearViewObject? TakeHandle(IntPtr handle , LinearViewObject? parent = null)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new LinearViewObject(handle, true);
	    }
	    
	    internal static LinearViewObject MustTakeHandle(IntPtr handle , LinearViewObject? parent = null)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new LinearViewObject(handle, true);
	    }
	    
	    internal static LinearViewObject? BorrowHandle(IntPtr handle , LinearViewObject? parent = null)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new LinearViewObject(handle, false);
	    }
	    
	    internal static LinearViewObject MustBorrowHandle(IntPtr handle , LinearViewObject? parent = null)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new LinearViewObject(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeLinearViewObject(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }

	    
	    public ulong Start
	    {
		    get
		    {
			    return NativeMethods.BNGetLinearViewObjectStart(this.handle);
		    }
	    }
	    
	    public ulong End
	    {
		    get
		    {
			    return NativeMethods.BNGetLinearViewObjectEnd(this.handle);
		    }
	    }
	    
	    public LinearViewObjectIdentifier Identifier
	    {
		    get
		    {
			    return LinearViewObjectIdentifier.TakeNativeStruct(
				    NativeMethods.BNGetLinearViewObjectIdentifier(this.handle)
				);
		    }
	    }

	    public ulong OrderingIndexTotal
	    {
		    get
		    {
			    return NativeMethods.BNGetLinearViewObjectOrderingIndexTotal(this.handle);
		    }
	    }
	    
	    public int CompareChildren(LinearViewObject child1, LinearViewObject child2)
	    {
		    return NativeMethods.BNCompareLinearViewObjectChildren(
			    this.handle ,
			    child1.DangerousGetHandle(),
			    child2.DangerousGetHandle()
		    );
	    }

	    public ulong GetOrderingIndexForChild(LinearViewObject  child)
	    {
		    return NativeMethods.BNGetLinearViewObjectOrderingIndexForChild(
			    this.handle ,
			    child.DangerousGetHandle()
		    );
	    } 
	    
	    public LinearDisassemblyLine[] GetLines(
		    LinearViewObject? prev , 
		    LinearViewObject? next)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetLinearViewObjectLines(
			    this.handle,
			    null == prev ? IntPtr.Zero : prev.DangerousGetHandle(),
			    null == next ? IntPtr.Zero : next.DangerousGetHandle(),
			    out ulong  arrayLength
		    );

		    return UnsafeUtils.TakeStructArrayEx<BNLinearDisassemblyLine , LinearDisassemblyLine>(
			    arrayPointer,
			    arrayLength,
			    LinearDisassemblyLine.FromNative,
			    NativeMethods.BNFreeLinearDisassemblyLines
		    );
	    }
	    
	    
	    
	    public LinearViewObject? FirstChild
	    {
		    get
		    {
			    return LinearViewObject.TakeHandle(
				    NativeMethods.BNGetFirstLinearViewObjectChild(this.handle) ,
				    this
			    );
		    }
	    }
	    
	    public LinearViewObject? LastChild
	    {
		    get
		    {
			    return LinearViewObject.TakeHandle(
				    NativeMethods.BNGetLastLinearViewObjectChild(this.handle) ,
				    this
			    );
		    }
	    }
	    
	    public LinearViewObject? GetPreviousChild(LinearViewObject child)
	    {
		    return LinearViewObject.TakeHandle(
			    NativeMethods.BNGetPreviousLinearViewObjectChild(
				    this.handle ,
				    child.DangerousGetHandle()
			    ) ,
			    this
		    );
	    }
	    
	    public LinearViewObject? Previous
	    {
		    get
		    {
			    if (null == this.Parent)
			    {
				    return null;
			    }
			    
			    return LinearViewObject.TakeHandle(
				    NativeMethods.BNGetPreviousLinearViewObjectChild(
					    this.Parent.DangerousGetHandle() ,
					    this.DangerousGetHandle()
				    ) ,
				    this.Parent
			    );
		    }
	    }
	    
	    public LinearViewObject? GetNextChild(LinearViewObject child)
	    {
		    return LinearViewObject.TakeHandle(
			    NativeMethods.BNGetNextLinearViewObjectChild(
				    this.handle ,
				    child.DangerousGetHandle()
			    ) ,
			    this
		    );
	    }
	    
	    public LinearViewObject? Next
	    {
		    get
		    {
			    if (null == this.Parent)
			    {
				    return null;
			    }
			    
			    return LinearViewObject.TakeHandle(
				    NativeMethods.BNGetNextLinearViewObjectChild(
					    this.Parent.DangerousGetHandle() ,
					    this.DangerousGetHandle()
				    ) ,
				    this.Parent
			    );
		    }
	    }
	    
	    public ulong? OrderingIndex
	    {
		    get
		    {
			    if (null == this.Parent)
			    {
				    return null;
			    }

			    return NativeMethods.BNGetLinearViewObjectOrderingIndexForChild(
				    this.Parent.DangerousGetHandle() ,
				    this.DangerousGetHandle()
			    );
		    }
	    }
	    
	    public LinearViewObject? GetChildForAddress(ulong address)
	    {
		    return LinearViewObject.TakeHandle(
			    NativeMethods.BNGetLinearViewObjectChildForAddress(
				    this.handle ,
				    address
			    ),
			    this
		    );
	    } 
	    
	    public LinearViewObject? GetChildForIdentifier(LinearViewObjectIdentifier id)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    return LinearViewObject.TakeHandle(
				    NativeMethods.BNGetLinearViewObjectChildForIdentifier(
					    this.handle ,
					    id.ToNativeEx(allocator)
				    ),
				    this
			    );
		    }
	    } 
	    
	    public LinearViewObject? GetChildForOrderingIndex(ulong index)
	    {
		    return LinearViewObject.TakeHandle(
			    NativeMethods.BNGetLinearViewObjectChildForOrderingIndex(
				    this.handle ,
				    index
			    ),
			    this
		    );
	    } 
	    
	   
	    public LinearViewCursor CreateCursor()
	    {
		    return new LinearViewCursor( 
			    NativeMethods.BNCreateLinearViewCursor(this.handle) ,
			    true
			);
	    } 
	    
	    public static LinearViewObject Disassembly(
		    BinaryView view,
		    DisassemblySettings? settings = null
	    )
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewDisassembly(
				    view.DangerousGetHandle() ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public static LinearViewObject LowLevelIL(
		    BinaryView view,
		    DisassemblySettings? settings = null
	    )
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewLowLevelIL(
				    view.DangerousGetHandle() ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public static LinearViewObject LowLevelILSSAForm(
		    BinaryView view,
		    DisassemblySettings? settings = null
	    )
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewLowLevelILSSAForm(
				    view.DangerousGetHandle() ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public static LinearViewObject MediumLevelIL(
		    BinaryView view,
		    DisassemblySettings? settings = null
	    )
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewMediumLevelIL(
				    view.DangerousGetHandle() ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public static LinearViewObject MediumLevelILSSAForm(
		    BinaryView view,
		    DisassemblySettings? settings = null
	    )
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewMediumLevelILSSAForm(
				    view.DangerousGetHandle() ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public static LinearViewObject MappedMediumLevelILSSAForm(
		    BinaryView view,
		    DisassemblySettings? settings = null
	    )
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewMappedMediumLevelILSSAForm(
				    view.DangerousGetHandle() ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public static LinearViewObject HighLevelIL(
		    BinaryView view,
		    DisassemblySettings? settings = null
	    )
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewHighLevelIL(
				    view.DangerousGetHandle() ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public static LinearViewObject HighLevelILSSAForm(
		    BinaryView view,
		    DisassemblySettings? settings = null
	    )
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewHighLevelILSSAForm(
				    view.DangerousGetHandle() ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public static LinearViewObject LanguageRepresentation(
		    BinaryView view,
		    DisassemblySettings? settings = null ,
		    string language = "Pseudo C"
	    )
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewLanguageRepresentation(
				    view.DangerousGetHandle() ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle(),
				    language
			    )
		    );
	    }
	    
	    public static LinearViewObject DataOnly(
		    BinaryView view,
		    DisassemblySettings? settings = null
	    )
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.Default();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewDataOnly(
				    view.DangerousGetHandle() ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public static LinearViewObject SingleFunctionDisassembly(
		    Function function,
		    DisassemblySettings? settings = null
	    )
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewSingleFunctionDisassembly(
				    function.DangerousGetHandle() ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public static LinearViewObject SingleFunctionLiftedIL(
		    Function function,
		    DisassemblySettings? settings = null
	    )
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewSingleFunctionLiftedIL(
				    function.DangerousGetHandle() ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public static LinearViewObject SingleFunctionLowLevelILSSAForm(
		    Function function,
		    DisassemblySettings? settings = null
	    )
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewSingleFunctionLowLevelILSSAForm(
				    function.DangerousGetHandle() ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public static LinearViewObject SingleFunctionMediumLevelIL(
		    Function function,
		    DisassemblySettings? settings = null
	    )
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewSingleFunctionMediumLevelIL(
				    function.DangerousGetHandle() ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public static LinearViewObject SingleFunctionMediumLevelILSSAForm(
		    Function function,
		    DisassemblySettings? settings = null
	    )
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewSingleFunctionMediumLevelILSSAForm(
				    function.DangerousGetHandle() ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public static LinearViewObject SingleFunctionMappedMediumLevelILSSAFor(
		    Function function,
		    DisassemblySettings? settings = null
	    )
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewSingleFunctionMappedMediumLevelILSSAForm(
				    function.DangerousGetHandle() ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public static LinearViewObject SingleFunctionHighLevelIL(
		    Function function,
		    DisassemblySettings? settings = null
	    )
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewSingleFunctionHighLevelIL(
				    function.DangerousGetHandle() ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public static LinearViewObject SingleFunctionHighLevelILSSAForm(
		    Function function,
		    DisassemblySettings? settings = null
	    )
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewSingleFunctionHighLevelILSSAForm(
				    function.DangerousGetHandle() ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
			    )
		    );
	    }
	    
	    public static LinearViewObject SingleFunctionLanguageRepresentation(
		    Function function,
		    DisassemblySettings? settings = null ,
		    string language = "Pseudo C"
	    )
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewSingleFunctionLanguageRepresentation(
				    function.DangerousGetHandle() ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle(),
				    language
			    )
		    );
	    }
	    
	}
}