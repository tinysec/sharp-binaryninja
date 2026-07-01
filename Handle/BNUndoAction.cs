using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class UndoAction : AbstractSafeHandle<UndoAction>
	{
	    internal UndoAction(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }
	    
	    internal static UndoAction? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new UndoAction(
			    NativeMethods.BNNewUndoActionReference(handle) ,
			    true
		    );
	    }
	    
	    internal static UndoAction MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new UndoAction(
			    NativeMethods.BNNewUndoActionReference(handle) ,
			    true
		    );
	    }
	    
	    internal static UndoAction? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new UndoAction(handle, true);
	    }
	    
	    internal static UndoAction MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new UndoAction(handle, true);
	    }
	    
	    internal static UndoAction? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new UndoAction(handle, false);
	    }
	    
	    internal static UndoAction MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new UndoAction(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeUndoAction(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }

	    public string Summary
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNUndoActionGetSummaryText(this.handle)
			    );
		    }
	    }

	    /// <summary>
	    /// Gets the summary of this undo action as an array of InstructionTextToken,
	    /// providing richer formatting than the plain-text Summary property.
	    /// </summary>
	    /// <returns>An array of InstructionTextToken representing the formatted summary.</returns>
	    public unsafe InstructionTextToken[] GetSummaryTokens()
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNUndoActionGetSummary(
				    this.handle ,
				    countPtr
			    );

			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    return UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken , InstructionTextToken>(
				    arrayPointer ,
				    arrayLength ,
				    InstructionTextToken.FromNative ,
				    NativeMethods.BNFreeInstructionText
			    );
		    }
	    }
	}
}