using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class TypeArchiveMergeConflict : AbstractSafeHandle<TypeArchiveMergeConflict>
	{
	    internal TypeArchiveMergeConflict(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	       
	    }
	    
	    internal static TypeArchiveMergeConflict? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new TypeArchiveMergeConflict(
			    NativeMethods.BNNewTypeArchiveMergeConflictReference(handle) ,
			    true
		    );
	    }
	    
	    internal static TypeArchiveMergeConflict MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new TypeArchiveMergeConflict(
			    NativeMethods.BNNewTypeArchiveMergeConflictReference(handle) ,
			    true
		    );
	    }
	    
	    internal static TypeArchiveMergeConflict? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new TypeArchiveMergeConflict(handle, true);
	    }
	    
	    internal static TypeArchiveMergeConflict MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new TypeArchiveMergeConflict(handle, true);
	    }
	    
	    internal static TypeArchiveMergeConflict? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new TypeArchiveMergeConflict(handle, false);
	    }
	    
	    internal static TypeArchiveMergeConflict MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new TypeArchiveMergeConflict(handle, false);
	    }

        /// <summary>
        /// Releases the native BNTypeArchiveMergeConflict handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native conflict handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeTypeArchiveMergeConflict(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Gets the type archive that owns this merge conflict.
        /// </summary>
        public TypeArchive? Archive
        {
            get
            {
                // Retrieve a new owned reference to the type archive from the native conflict.
                return BinaryNinja.TypeArchive.TakeHandle(
                    NativeMethods.BNTypeArchiveMergeConflictGetTypeArchive(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets the unique identifier string of the type that is in conflict.
        /// </summary>
        public string TypeId
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the type ID.
                IntPtr raw = NativeMethods.BNTypeArchiveMergeConflictGetTypeId(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the snapshot ID of the base (common ancestor) snapshot for this conflict.
        /// Returns an empty string when there is no common ancestor.
        /// </summary>
        public string BaseSnapshotId
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the base snapshot ID.
                IntPtr raw = NativeMethods.BNTypeArchiveMergeConflictGetBaseSnapshotId(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the snapshot ID of the first (left-side) snapshot for this conflict.
        /// </summary>
        public string FirstSnapshotId
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the first snapshot ID.
                IntPtr raw = NativeMethods.BNTypeArchiveMergeConflictGetFirstSnapshotId(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the snapshot ID of the second (right-side) snapshot for this conflict.
        /// </summary>
        public string SecondSnapshotId
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the second snapshot ID.
                IntPtr raw = NativeMethods.BNTypeArchiveMergeConflictGetSecondSnapshotId(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Resolves this conflict by accepting the given serialised value string as the winning type.
        /// The value string must be the serialised form of the chosen resolution.
        /// </summary>
        /// <param name="value">The serialised value to use as the resolution for this conflict.</param>
        /// <returns>True if the resolution was accepted; false if the operation failed.</returns>
        public bool Success(string value)
        {
            // Forward the conflict handle and resolution string to the native API.
            return NativeMethods.BNTypeArchiveMergeConflictSuccess(
                this.handle,
                value ?? string.Empty
            );
        }
    }
}