using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class NamedTypeReference : AbstractSafeHandle<NamedTypeReference>
	{
		public NamedTypeReference(NamedTypeReferenceClass cls , string id , QualifiedName name) 
			: this( NamedTypeReference.rawCreate(cls , id , name) , true )
		{
			
		}

	    internal NamedTypeReference(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	       
	    }

	    private static IntPtr rawCreate(NamedTypeReferenceClass cls , string id , QualifiedName name)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    return NativeMethods.BNCreateNamedType(
				    cls,
				    id,
				    name.ToNativeEx(allocator)
			    );
		    }
	    }

        /// <summary>
        /// Creates a NamedTypeReference by looking up a type by name in a binary view.
        /// Returns a Type wrapping the named type reference, or null if not found.
        /// </summary>
        /// <param name="view">The binary view containing the type definitions.</param>
        /// <param name="name">The qualified name of the type to reference.</param>
        /// <returns>A new owned Type representing the named type reference, or null on failure.</returns>
        public static BinaryNinja.Type? CreateFromType(BinaryView view, QualifiedName name)
        {
            // 1. Validate required parameters.
            if (null == view)
            {
                throw new ArgumentNullException(nameof(view));
            }

            // 2. Marshal the qualified name and call the native factory.
            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                BNQualifiedName nativeName = name.ToNativeEx(allocator);

                return BinaryNinja.Type.TakeHandle(
                    NativeMethods.BNCreateNamedTypeReferenceFromType(
                        view.DangerousGetHandle(),
                        allocator.AllocStruct<BNQualifiedName>(nativeName)
                    )
                );
            }
        }

        /// <summary>
        /// Creates a NamedTypeReference by looking up a type by name and ID in relation to an existing type.
        /// Returns a Type wrapping the named type reference, or null if not found.
        /// </summary>
        /// <param name="id">The type identifier string.</param>
        /// <param name="name">The qualified name of the type to reference.</param>
        /// <param name="type">The existing type to create the reference from.</param>
        /// <returns>A new owned Type representing the named type reference, or null on failure.</returns>
        public static BinaryNinja.Type? CreateFromTypeAndId(string id, QualifiedName name, BinaryNinja.Type type)
        {
            // 1. Validate required parameters.
            if (null == type)
            {
                throw new ArgumentNullException(nameof(type));
            }

            // 2. Marshal the qualified name and call the native factory.
            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                BNQualifiedName nativeName = name.ToNativeEx(allocator);

                return BinaryNinja.Type.TakeHandle(
                    NativeMethods.BNCreateNamedTypeReferenceFromTypeAndId(
                        id ?? string.Empty,
                        allocator.AllocStruct<BNQualifiedName>(nativeName),
                        type.DangerousGetHandle()
                    )
                );
            }
        }

	    internal static NamedTypeReference? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new NamedTypeReference(
			    NativeMethods.BNNewNamedTypeReference(handle) ,
			    true
		    );
	    }
	    
	    internal static NamedTypeReference MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new NamedTypeReference(
			    NativeMethods.BNNewNamedTypeReference(handle) ,
			    true
		    );
	    }
	    
	    internal static NamedTypeReference? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new NamedTypeReference(handle, true);
	    }
	    
	    internal static NamedTypeReference MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new NamedTypeReference(handle, true);
	    }
	    
	    internal static NamedTypeReference? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new NamedTypeReference(handle, false);
	    }
	    
	    internal static NamedTypeReference MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new NamedTypeReference(handle, false);
	    }

	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeNamedTypeReference(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }

	    public QualifiedName Name
	    {
		    get
		    {
			    return QualifiedName.TakeNative(
				    NativeMethods.BNGetTypeReferenceName(this.handle)
				);
		    }
	    }

	    /// <summary>
	    /// The type id this reference points at. Mirrors C++ NamedTypeReference::GetTypeId()
	    /// and Python NamedTypeReference.type_id.
	    /// </summary>
	    public string TypeId
	    {
		    get
		    {
			    // this.handle is a genuine BNNamedTypeReference*, which is exactly what
			    // BNGetTypeReferenceId expects.
			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNGetTypeReferenceId(this.handle)
			    );
		    }
	    }

	    /// <summary>
	    /// The reference class (struct/enum/union/typedef/unknown). Mirrors C++
	    /// NamedTypeReference::GetTypeReferenceClass() and Python NamedTypeReference.named_type_class.
	    /// </summary>
	    public NamedTypeReferenceClass TypeReferenceClass
	    {
		    get
		    {
			    return NativeMethods.BNGetTypeReferenceClass(this.handle);
		    }
	    }
	}
}