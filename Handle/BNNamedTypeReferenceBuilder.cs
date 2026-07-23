using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class NamedTypeReferenceBuilder :  AbstractSafeHandle<NamedTypeReferenceBuilder>
	{
	    internal NamedTypeReferenceBuilder(IntPtr handle , bool owner)
		    : base(handle , owner)
	    {

	    }

        /// <summary>
        /// Creates a new NamedTypeReferenceBuilder with the specified class, identifier, and name.
        /// </summary>
        /// <param name="cls">The named type reference class (struct, enum, union, etc.).</param>
        /// <param name="id">The type identifier string.</param>
        /// <param name="name">The qualified name for the type reference.</param>
        /// <returns>A new owned NamedTypeReferenceBuilder instance.</returns>
        public static NamedTypeReferenceBuilder Create(
            NamedTypeReferenceClass cls,
            string id,
            QualifiedName name
        )
        {
            // 1. Marshal the qualified name to a native struct.
            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                BNQualifiedName nativeName = name.ToNativeEx(allocator);

                // 2. Create the builder with the given class, ID, and name; the returned handle is owned.
                return NamedTypeReferenceBuilder.MustTakeHandle(
                    NativeMethods.BNCreateNamedTypeBuilder(
                        cls,
                        id ?? string.Empty,
                        allocator.AllocStruct<BNQualifiedName>(nativeName)
                    )
                );
            }
        }

	    internal static NamedTypeReferenceBuilder? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new NamedTypeReferenceBuilder(handle, true);
	    }
	    
	    internal static NamedTypeReferenceBuilder MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new NamedTypeReferenceBuilder(handle, true);
	    }
	    
	    internal static NamedTypeReferenceBuilder? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new NamedTypeReferenceBuilder(handle, false);
	    }
	    
	    internal static NamedTypeReferenceBuilder MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new NamedTypeReferenceBuilder(handle, false);
	    }

		internal void TransferOwnershipToCore()
		{
			this.SetHandle(IntPtr.Zero);
			this.SetHandleAsInvalid();
		}
	    
        /// <summary>
        /// Releases the native BNNamedTypeReferenceBuilder handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native builder handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeNamedTypeReferenceBuilder(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Sets the named type reference class (struct, union, enum, etc.) on this builder.
        /// </summary>
        /// <param name="cls">The named type reference class to assign.</param>
        public void SetTypeClass(NamedTypeReferenceClass cls)
        {
            // Delegate to the native setter with the class enum value.
            NativeMethods.BNSetNamedTypeReferenceBuilderTypeClass(this.handle, cls);
        }

        /// <summary>
        /// Sets the type identifier string on this builder.
        /// The identifier uniquely names the referenced type within the type system.
        /// </summary>
        /// <param name="id">The type ID string to assign.</param>
        public void SetTypeId(string id)
        {
            // Delegate to the native setter with the id string.
            NativeMethods.BNSetNamedTypeReferenceBuilderTypeId(this.handle, id ?? string.Empty);
        }

        /// <summary>
        /// Sets the qualified name on this builder using a scoped allocator to manage
        /// the native memory required by the BNQualifiedName struct.
        /// </summary>
        /// <param name="name">The qualified name to assign.</param>
        public void SetName(QualifiedName name)
        {
            // 1. Use a scoped allocator so the native strings are freed when this block exits.
            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                // 2. Convert the managed QualifiedName into a native BNQualifiedName struct.
                BNQualifiedName nativeName = name.ToNativeEx(allocator);

                // 3. Pass a pointer to the native struct to the native setter.
                unsafe
                {
                    NativeMethods.BNSetNamedTypeReferenceBuilderName(
                        this.handle,
                        (IntPtr)(&nativeName)
                    );
                }
            }
        }

        /// <summary>
        /// Finalizes the builder and returns the constructed NamedTypeReference.
        /// After calling this method the builder should not be used again.
        /// </summary>
        /// <returns>The newly created NamedTypeReference, or null if creation failed.</returns>
        public NamedTypeReference? Finalize()
        {
            // Finalize returns a new BNNamedTypeReference* that we own.
            return BinaryNinja.NamedTypeReference.TakeHandle(
                NativeMethods.BNFinalizeNamedTypeReferenceBuilder(this.handle)
            );
        }
    }
}
