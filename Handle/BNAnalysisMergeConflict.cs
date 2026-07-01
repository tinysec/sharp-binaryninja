using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class AnalysisMergeConflict : AbstractSafeHandle<AnalysisMergeConflict>
	{
	    internal AnalysisMergeConflict(IntPtr handle , bool owner)
		    : base(handle , owner)
	    {
	       
	    }
	    
	    internal static AnalysisMergeConflict? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new AnalysisMergeConflict(
			    NativeMethods.BNNewAnalysisMergeConflictReference(handle) ,
			    true
		    );
	    }
	    
	    internal static AnalysisMergeConflict MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new AnalysisMergeConflict(
			    NativeMethods.BNNewAnalysisMergeConflictReference(handle) ,
			    true
		    );
	    }
	    
	    internal static AnalysisMergeConflict? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new AnalysisMergeConflict(handle, true);
	    }
	    
	    internal static AnalysisMergeConflict MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new AnalysisMergeConflict(handle, true);
	    }
	    
	    internal static AnalysisMergeConflict? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new AnalysisMergeConflict(handle, false);
	    }
	    
	    internal static AnalysisMergeConflict MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new AnalysisMergeConflict(handle, false);
	    }
	    
        /// <summary>
        /// Releases the native BNAnalysisMergeConflict handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native conflict handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeAnalysisMergeConflict(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Gets the key string that uniquely identifies this conflict within the merge operation.
        /// The key is the path in the key-value store where the conflicting values reside.
        /// </summary>
        public string Key
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the conflict key.
                IntPtr raw = NativeMethods.BNAnalysisMergeConflictGetKey(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the data type of the conflicting value, indicating how the raw pointer
        /// returned by the Base, First, and Second properties should be interpreted.
        /// </summary>
        public MergeConflictDataType DataType
        {
            get
            {
                // Retrieve the native enum value that describes the kind of conflicting data.
                return NativeMethods.BNAnalysisMergeConflictGetDataType(this.handle);
            }
        }

        /// <summary>
        /// Gets the type name string describing the schema type of the conflicting value.
        /// This is distinct from DataType: it is the high-level type name used inside the
        /// analysis data store (e.g., "function", "tag").
        /// </summary>
        public string? TypeName
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the type name.
                IntPtr raw = NativeMethods.BNAnalysisMergeConflictGetType(this.handle);

                // 2. Copy and free the native string; null means no type name is set.
                return UnsafeUtils.TakeAnsiString(raw);
            }
        }

        /// <summary>
        /// Gets the raw native pointer to the base (common ancestor) value for this conflict.
        /// The actual type of the pointed-to data is described by DataType.
        /// </summary>
        public IntPtr Base
        {
            get
            {
                // Retrieve the opaque native pointer to the base value; caller interprets by DataType.
                return NativeMethods.BNAnalysisMergeConflictGetBase(this.handle);
            }
        }

        /// <summary>
        /// Gets the raw native pointer to the "first" (left-side) value in the conflict.
        /// The actual type of the pointed-to data is described by DataType.
        /// </summary>
        public IntPtr First
        {
            get
            {
                // Retrieve the opaque native pointer to the first (left) conflicting value.
                return NativeMethods.BNAnalysisMergeConflictGetFirst(this.handle);
            }
        }

        /// <summary>
        /// Gets the raw native pointer to the "second" (right-side) value in the conflict.
        /// The actual type of the pointed-to data is described by DataType.
        /// </summary>
        public IntPtr Second
        {
            get
            {
                // Retrieve the opaque native pointer to the second (right) conflicting value.
                return NativeMethods.BNAnalysisMergeConflictGetSecond(this.handle);
            }
        }

        /// <summary>
        /// Gets the analysis database that owns this conflict.
        /// </summary>
        public Database? Database
        {
            get
            {
                // Retrieve a new owned reference to the database from the native conflict.
                return BinaryNinja.Database.TakeHandle(
                    NativeMethods.BNAnalysisMergeConflictGetDatabase(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets the base (common ancestor) snapshot involved in this conflict.
        /// Returns null if the conflict has no common ancestor snapshot.
        /// </summary>
        public Snapshot? BaseSnapshot
        {
            get
            {
                // Retrieve a new owned reference to the base snapshot from the native conflict.
                return BinaryNinja.Snapshot.TakeHandle(
                    NativeMethods.BNAnalysisMergeConflictGetBaseSnapshot(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets the first (left-side) snapshot involved in this conflict.
        /// </summary>
        public Snapshot? FirstSnapshot
        {
            get
            {
                // Retrieve a new owned reference to the first snapshot from the native conflict.
                return BinaryNinja.Snapshot.TakeHandle(
                    NativeMethods.BNAnalysisMergeConflictGetFirstSnapshot(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets the second (right-side) snapshot involved in this conflict.
        /// </summary>
        public Snapshot? SecondSnapshot
        {
            get
            {
                // Retrieve a new owned reference to the second snapshot from the native conflict.
                return BinaryNinja.Snapshot.TakeHandle(
                    NativeMethods.BNAnalysisMergeConflictGetSecondSnapshot(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets the file metadata for the base (common ancestor) snapshot.
        /// Returns null if no file metadata is associated with the base snapshot.
        /// </summary>
        public FileMetadata? BaseFile
        {
            get
            {
                // Retrieve a new owned reference to the base file metadata from the native conflict.
                return BinaryNinja.FileMetadata.TakeHandle(
                    NativeMethods.BNAnalysisMergeConflictGetBaseFile(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets the file metadata for the first (left-side) snapshot.
        /// Returns null if no file metadata is associated with the first snapshot.
        /// </summary>
        public FileMetadata? FirstFile
        {
            get
            {
                // Retrieve a new owned reference to the first file metadata from the native conflict.
                return BinaryNinja.FileMetadata.TakeHandle(
                    NativeMethods.BNAnalysisMergeConflictGetFirstFile(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets the file metadata for the second (right-side) snapshot.
        /// Returns null if no file metadata is associated with the second snapshot.
        /// </summary>
        public FileMetadata? SecondFile
        {
            get
            {
                // Retrieve a new owned reference to the second file metadata from the native conflict.
                return BinaryNinja.FileMetadata.TakeHandle(
                    NativeMethods.BNAnalysisMergeConflictGetSecondFile(this.handle)
                );
            }
        }

        /// <summary>
        /// Retrieves the raw native item at the given dot-separated path within the conflicting
        /// data structure. The returned pointer must be interpreted according to DataType.
        /// Returns IntPtr.Zero if no item exists at the given path.
        /// </summary>
        /// <param name="path">The dot-separated path within the conflict's data structure.</param>
        /// <returns>An opaque native pointer to the item, or IntPtr.Zero if absent.</returns>
        public IntPtr GetPathItem(string path)
        {
            // Forward the conflict handle and the path string to the native API.
            return NativeMethods.BNAnalysisMergeConflictGetPathItem(
                this.handle,
                path ?? string.Empty
            );
        }

        /// <summary>
        /// Retrieves the string value of the item at the given dot-separated path within
        /// the conflicting data structure.
        /// Returns null if no string item exists at the given path.
        /// </summary>
        /// <param name="path">The dot-separated path within the conflict's data structure.</param>
        /// <returns>The string value at the path, or null if absent.</returns>
        public string? GetPathItemString(string path)
        {
            // 1. Retrieve the native ANSI string pointer for the item at the given path.
            IntPtr raw = NativeMethods.BNAnalysisMergeConflictGetPathItemString(
                this.handle,
                path ?? string.Empty
            );

            // 2. Copy and free the native string; null means no string at that path.
            return UnsafeUtils.TakeAnsiString(raw);
        }

        /// <summary>
        /// Retrieves the JSON-serialised representation of the item at the given dot-separated
        /// path within the conflicting data structure.
        /// Returns null if no item exists at the given path.
        /// </summary>
        /// <param name="path">The dot-separated path within the conflict's data structure.</param>
        /// <returns>The serialised string for the item, or null if absent.</returns>
        public string? GetPathItemSerialized(string path)
        {
            // 1. Retrieve the native ANSI string pointer for the serialised item at the given path.
            IntPtr raw = NativeMethods.BNAnalysisMergeConflictGetPathItemSerialized(
                this.handle,
                path ?? string.Empty
            );

            // 2. Copy and free the native string; null means no item at that path.
            return UnsafeUtils.TakeAnsiString(raw);
        }

        /// <summary>
        /// Resolves this conflict by accepting the given serialised value string as the winner.
        /// The value string must be the JSON-serialised form of the chosen resolution.
        /// </summary>
        /// <param name="value">The serialised value to use as the resolution for this conflict.</param>
        /// <returns>True if the resolution was accepted; false if the operation failed.</returns>
        public bool Success(string value)
        {
            // Forward the conflict handle and resolution string to the native API.
            return NativeMethods.BNAnalysisMergeConflictSuccess(
                this.handle,
                value ?? string.Empty
            );
        }
    }
}