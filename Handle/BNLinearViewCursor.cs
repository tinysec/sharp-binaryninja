using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class LinearViewCursor : AbstractSafeHandle<LinearViewCursor>
	{
		public LinearViewCursor(LinearViewObject root) 
			: this( 
				NativeMethods.BNCreateLinearViewCursor(root.DangerousGetHandle()), 
				true 
			)
		{
			
		}
		
	    internal LinearViewCursor(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }
	    
	    internal static LinearViewCursor? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new LinearViewCursor(
			    NativeMethods.BNNewLinearViewCursorReference(handle) ,
			    true
		    );
	    }
	    
	    internal static LinearViewCursor MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new LinearViewCursor(
			    NativeMethods.BNNewLinearViewCursorReference(handle) ,
			    true
		    );
	    }
	    
	    internal static LinearViewCursor? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new LinearViewCursor(handle, true);
	    }
	    
	    internal static LinearViewCursor MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new LinearViewCursor(handle, true);
	    }
	    
	    internal static LinearViewCursor? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new LinearViewCursor(handle, false);
	    }
	    
	    internal static LinearViewCursor MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new LinearViewCursor(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeLinearViewCursor(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }

	    public bool BeforeBegin
	    {
		    get
		    {
			    return NativeMethods.BNIsLinearViewCursorBeforeBegin(this.handle);
		    }
	    }
	    
	    public bool AfterEnd
	    {
		    get
		    {
			    return NativeMethods.BNIsLinearViewCursorAfterEnd(this.handle);
		    }
	    }

	    public bool IsValid
	    {
		    get
		    {
			    if (this.BeforeBegin)
			    {
				    return false;
			    }
			    
			    if (this.AfterEnd)
			    {
				    return false;
			    }
			    
			    return true;
		    }
	    }

	    public LinearViewObjectIdentifier[] Path
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetLinearViewCursorPath(
				    this.handle, 
				    out ulong arrayLength
			    );

			    return UnsafeUtils.TakeStructArrayEx<BNLinearViewObjectIdentifier, LinearViewObjectIdentifier>
			    (
				    arrayPointer,
				    arrayLength,
				    LinearViewObjectIdentifier.FromNative,
				    NativeMethods.BNFreeLinearViewCursorPath
			    );
		    }
	    }
	    
	    public AddressRange OrderingIndex
	    {
		    get
		    {
			    return AddressRange.FromNative(
				    NativeMethods.BNGetLinearViewCursorOrderingIndex(this.handle)
				);
		    }
	    }
	    
	    public ulong OrderingIndexTotal
	    {
		    get
		    {
			    return NativeMethods.BNGetLinearViewCursorOrderingIndexTotal(this.handle);
		    }
	    }

	    public void SeekToBegin()
	    {
		    NativeMethods.BNSeekLinearViewCursorToBegin(this.handle);
	    }
	    
	    public void SeekToEnd()
	    {
		    NativeMethods.BNSeekLinearViewCursorToEnd(this.handle);
	    }
	    
	    public void SeekToAddress(ulong address)
	    {
		    NativeMethods.BNSeekLinearViewCursorToAddress(this.handle , address);
	    }
	    
	    public bool SeekToPath(LinearViewObjectIdentifier[] path)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    return NativeMethods.BNSeekLinearViewCursorToPath(
				    this.handle ,
				    allocator.ConvertToNativeArrayEx<BNLinearViewObjectIdentifier , LinearViewObjectIdentifier>(
					    path
					),
				    (ulong)path.Length
			    );
		    }
	    }
	    
	    public void SeekToOrderingIndex(ulong index)
	    {
		    NativeMethods.BNSeekLinearViewCursorToOrderingIndex(this.handle , index);
	    }

	    /// <summary>
	    /// Seek the cursor to match the path of another cursor.
	    /// </summary>
	    public bool SeekToCursorPath(LinearViewCursor path)
	    {
		    return NativeMethods.BNSeekLinearViewCursorToCursorPath(
			    this.handle ,
			    path.DangerousGetHandle()
		    );
	    }

	    /// <summary>
	    /// Seek the cursor to match the path of another cursor at a specific address.
	    /// </summary>
	    public bool SeekToCursorPathAndAddress(LinearViewCursor path , ulong addr)
	    {
		    return NativeMethods.BNSeekLinearViewCursorToCursorPathAndAddress(
			    this.handle ,
			    path.DangerousGetHandle() ,
			    addr
		    );
	    }

	    /// <summary>
	    /// Seek the cursor to a path specified by an array of identifier IDs at a specific address.
	    /// </summary>
	    public unsafe bool SeekToPathAndAddress(ulong[] ids , ulong addr)
	    {
		    fixed (ulong* ptr = ids)
		    {
			    return NativeMethods.BNSeekLinearViewCursorToPathAndAddress(
				    this.handle ,
				    (IntPtr)ptr ,
				    (ulong)ids.Length ,
				    addr
			    );
		    }
	    }
	    
	    public bool Previous()
	    {
			return NativeMethods.BNLinearViewCursorPrevious(this.handle);
	    }
	    
	    public bool Next()
	    {
		    return NativeMethods.BNLinearViewCursorNext(this.handle);
	    }
	    
	    public static int Compare(LinearViewCursor a, LinearViewCursor b)
	    {
		    return NativeMethods.BNCompareLinearViewCursors(
			    a.DangerousGetHandle() ,
			    b.DangerousGetHandle()
		    );
	    }

	    public void AddRenderLayer(RenderLayer layer)
	    {
		    NativeMethods.BNAddLinearViewCursorRenderLayer(this.handle , layer.DangerousGetHandle());
	    }
	    
	    public void RemoveRenderLayer(RenderLayer layer)
	    {
		    NativeMethods.BNRemoveLinearViewCursorRenderLayer(this.handle , layer.DangerousGetHandle());
	    }
	    
	    public LinearViewObject? CurrentObject
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetLinearViewCursorPathObjects(
				    this.handle, 
				    out ulong arrayLength
				);

			    LinearViewObject? view = null;

			    if (( IntPtr.Zero != arrayPointer ) && ( 0 != arrayLength ))
			    {
				    for (ulong i = 0; i < arrayLength; i++)
				    {
					    int offset = checked((int)(i * (ulong)IntPtr.Size));
					
					    IntPtr addressOfElement = IntPtr.Add(arrayPointer, offset);
					
					    IntPtr element = Marshal.ReadIntPtr(addressOfElement);

					    if (element != IntPtr.Zero)
					    {
						    // object chain
						    view = LinearViewObject.MustNewFromHandle(element, view);
					    }
				    }
			    }
			    
			    if (arrayPointer != IntPtr.Zero )
			    {
				    NativeMethods.BNFreeLinearViewCursorPathObjects(arrayPointer ,arrayLength);
			    }

			    return view;
		    }
	    }
	    
	    public LinearViewObject[] PathObjects
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetLinearViewCursorPathObjects(
				    this.handle, 
				    out ulong arrayLength
			    );

			    LinearViewObject? parent = null;
			    
			    List<LinearViewObject> targets = new List<LinearViewObject>();

			    if (( IntPtr.Zero != arrayPointer ) && ( 0 != arrayLength ))
			    {
				    for (ulong i = 0; i < arrayLength; i++)
				    {
					    int offset = checked((int)(i * (ulong)IntPtr.Size));
					
					    IntPtr addressOfElement = IntPtr.Add(arrayPointer, offset);
					
					    IntPtr element = Marshal.ReadIntPtr(addressOfElement);

					    if (element != IntPtr.Zero)
					    {
						    // object chain
						    LinearViewObject item = LinearViewObject.MustNewFromHandle(element, parent);
						    
						    targets.Add(item);
						    
						    parent = item;
					    }
				    }
			    }
			    
			    if (arrayPointer != IntPtr.Zero )
			    {
				    NativeMethods.BNFreeLinearViewCursorPathObjects(arrayPointer ,arrayLength);
			    }

			    return targets.ToArray();
		    }
	    }
	 
	    public LinearDisassemblyLine[] Lines
	    {
		    get
		    {
			    return this.GetLines();
		    }
	    }
	    
	    public LinearDisassemblyLine[] GetLines()
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetLinearViewCursorLines(
			    this.handle ,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArrayEx<BNLinearDisassemblyLine , LinearDisassemblyLine>(
			    arrayPointer ,
			    arrayLength ,
			    LinearDisassemblyLine.FromNative ,
			    NativeMethods.BNFreeLinearDisassemblyLines
		    );
	    }

	    public LinearDisassemblyLine[] PreviousLines
	    {
		    get
		    {
			    LinearDisassemblyLine[] items = Array.Empty<LinearDisassemblyLine>();

			    while (0 == items.Length)
			    {
				    items = this.Lines;

				    if (!this.Previous())
				    {
					    return items;
				    }
			    }
			    
			    return items;
		    }
	    }
	    
	    public LinearDisassemblyLine[] NextLines
	    {
		    get
		    {
			    LinearDisassemblyLine[] items = Array.Empty<LinearDisassemblyLine>();

			    while (0 == items.Length)
			    {
				    items = this.Lines;

				    if (!this.Next())
				    {
					    return items;
				    }
			    }
			    
			    return items;
		    }
	    }
	
	    public LinearViewCursor Duplicate()
	    {
		    return new LinearViewCursor(
				    NativeMethods.BNDuplicateLinearViewCursor(this.handle),
				    true
			);
	    }
	
	    public RenderLayer[] RenderLayers
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetLinearViewCursorRenderLayers(
				    this.handle ,
				    out ulong arrayLength
			    );

			    return UnsafeUtils.TakeHandleArray<RenderLayer>(
				    arrayPointer ,
				    arrayLength ,
				    RenderLayer.FromHandle ,
				    NativeMethods.BNFreeRenderLayerList
			    );
		    }

		    set
		    {
			    foreach (RenderLayer layer in this.RenderLayers)
			    {
				    this.RemoveRenderLayer(layer);
			    }
			    
			    foreach (RenderLayer layer in value)
			    {
				    this.AddRenderLayer(layer);
			    }
		    }
	    }
	}
}