using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class Symbol : AbstractSafeHandle<Symbol>
	{
	    internal Symbol(IntPtr handle , bool owner)
		    : base(handle , owner)
	    {

	    }

        /// <summary>
        /// Creates a new Symbol with the specified attributes.
        /// </summary>
        /// <param name="type">The type of the symbol (function, data, etc.).</param>
        /// <param name="shortName">The short display name of the symbol.</param>
        /// <param name="fullName">The fully qualified name of the symbol.</param>
        /// <param name="rawName">The raw (mangled) name of the symbol.</param>
        /// <param name="addr">The virtual address of the symbol.</param>
        /// <param name="binding">The binding type of the symbol (local, global, weak).</param>
        /// <param name="ns">The optional namespace for the symbol, or null for default.</param>
        /// <param name="ordinal">The ordinal value of the symbol (used for imports).</param>
        /// <returns>A new owned Symbol instance, or null on failure.</returns>
        public static Symbol? Create(
            SymbolType type,
            string shortName,
            string fullName,
            string rawName,
            ulong addr,
            SymbolBinding binding = SymbolBinding.NoBinding,
            NameSpace? ns = null,
            ulong ordinal = 0
        )
        {
            // 1. Marshal the optional namespace to a native pointer.
            IntPtr nsPtr = IntPtr.Zero;

            if (null != ns)
            {
                using (ScopedAllocator allocator = new ScopedAllocator())
                {
                    // 1.1. Convert the namespace to native and allocate it on the scoped heap.
                    BNNameSpace nativeNs = ns.ToNativeEx(allocator);
                    nsPtr = allocator.AllocStruct<BNNameSpace>(nativeNs);

                    // 2. Create the symbol with the marshalled namespace pointer.
                    return Symbol.TakeHandle(
                        NativeMethods.BNCreateSymbol(
                            type,
                            shortName ?? string.Empty,
                            fullName ?? string.Empty,
                            rawName ?? string.Empty,
                            addr,
                            binding,
                            nsPtr,
                            ordinal
                        )
                    );
                }
            }

            // 2. Create the symbol without a namespace.
            return Symbol.TakeHandle(
                NativeMethods.BNCreateSymbol(
                    type,
                    shortName ?? string.Empty,
                    fullName ?? string.Empty,
                    rawName ?? string.Empty,
                    addr,
                    binding,
                    IntPtr.Zero,
                    ordinal
                )
            );
        }

	    internal static Symbol? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Symbol(
			    NativeMethods.BNNewSymbolReference(handle) ,
			    true
		    );
	    }
	    
	    internal static Symbol MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Symbol(
			    NativeMethods.BNNewSymbolReference(handle) ,
			    true
		    );
	    }
	    
	    internal static Symbol? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Symbol(handle, true);
	    }
	    
	    internal static Symbol MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Symbol(handle, true);
	    }
	    
	    internal static Symbol? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new Symbol(handle, false);
	    }
	    
	    internal static Symbol MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new Symbol(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeSymbol(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }

	    public override string ToString()
	    {
		    return this.FullName;
	    }

	    /// <summary>
	    /// Given an import-address symbol, navigate to the imported-function symbol placed
	    /// at <paramref name="address"/>. Mirrors Python
	    /// <c>Symbol.imported_function_from_import_address_symbol</c>. Returns null when
	    /// there is no associated imported-function symbol.
	    /// </summary>
	    public Symbol? GetImportedFunctionFromImportAddressSymbol(ulong address)
	    {
		    return Symbol.TakeHandle(
			    NativeMethods.BNImportedFunctionFromImportAddressSymbol(this.handle, address)
		    );
	    }

	    public SymbolType Type
	    {
		    get
		    {
			    return NativeMethods.BNGetSymbolType(handle);
		    }
	    }

	    public SymbolBinding Binding
	    {
		    get
		    {
			    return NativeMethods.BNGetSymbolBinding(handle);
		    }
	    }

	    public string ShortName
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNGetSymbolShortName(this.handle)
			    );
		    }
	    }
	    
	    public string FullName
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNGetSymbolFullName(this.handle)
			    );
		    }
	    }
	    
	    public string RawName
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNGetSymbolRawName(this.handle)
			    );
		    }
	    }

	    /// <summary>
	    /// The symbol name (raw name). Alias of <see cref="RawName"/> for parity with
	    /// Python <c>Symbol.name</c>.
	    /// </summary>
	    public string Name
	    {
		    get
		    {
			    return this.RawName;
		    }
	    }

	    /// <summary>
	    /// The symbol namespace. Alias of <see cref="NameSpaceValue"/> for parity with
	    /// Python <c>Symbol.namespace</c>.
	    /// </summary>
	    public NameSpace Namespace
	    {
		    get
		    {
			    return this.NameSpaceValue;
		    }
	    }

	    public byte[] RawBytes
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetSymbolRawBytes(
				    this.handle ,
				    out ulong arrayLength
				);


			    return UnsafeUtils.TakeNumberArray<byte>(
				    arrayPointer ,
				    arrayLength,
				    NativeMethods.BNFreeSymbolRawBytes
			    );
		    }
	    }
	    
	    public ulong Address
	    {
		    get
		    {
			    return NativeMethods.BNGetSymbolAddress(this.handle);
		    }
	    }
	    
	    public ulong Ordinal
	    {
		    get
		    {
			    return NativeMethods.BNGetSymbolOrdinal(this.handle);
		    }
	    }

	    public bool AutoDefined
	    {
		    get
		    {
			    return NativeMethods.BNIsSymbolAutoDefined(this.handle);
		    }
	    }

	    /// <summary>
	    /// Gets the full name as a borrowed reference (not freed by caller).
	    /// </summary>
	    public string FullNameRef
	    {
		    get
		    {
			    return UnsafeUtils.ReadAnsiString(
				    NativeMethods.BNGetSymbolFullNameRef(this.handle)
			    );
		    }
	    }

	    /// <summary>
	    /// Gets the raw name as a borrowed reference (not freed by caller).
	    /// </summary>
	    public string RawNameRef
	    {
		    get
		    {
			    return UnsafeUtils.ReadAnsiString(
				    NativeMethods.BNGetSymbolRawNameRef(this.handle)
			    );
		    }
	    }

	    /// <summary>
	    /// Gets the short name as a borrowed reference (not freed by caller).
	    /// </summary>
	    public string ShortNameRef
	    {
		    get
		    {
			    return UnsafeUtils.ReadAnsiString(
				    NativeMethods.BNGetSymbolShortNameRef(this.handle)
			    );
		    }
	    }

	    /// <summary>
	    /// Gets the namespace associated with this symbol.
	    /// </summary>
	    public NameSpace NameSpaceValue
	    {
		    get
		    {
			    return NameSpace.TakeNative(
				    NativeMethods.BNGetSymbolNameSpace(this.handle)
			    );
		    }
	    }
	}


}