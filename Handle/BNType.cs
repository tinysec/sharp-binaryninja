using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public class Type: AbstractSafeHandle<Type>
	{
	    internal Type(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }
	    
	    internal static BinaryNinja.Type? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new BinaryNinja.Type(
			    NativeMethods.BNNewTypeReference(handle) ,
			    true
		    );
	    }
	    
	    internal static BinaryNinja.Type MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new BinaryNinja.Type(
			    NativeMethods.BNNewTypeReference(handle) ,
			    true
		    );
	    }
	    
	    internal static BinaryNinja.Type? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new BinaryNinja.Type(handle, true);
	    }
	    
	    internal static BinaryNinja.Type MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new BinaryNinja.Type(handle, true);
	    }
	    
	    internal static BinaryNinja.Type? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new BinaryNinja.Type(handle, false);
	    }
	    
	    internal static BinaryNinja.Type MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new BinaryNinja.Type(handle, false);
	    }

	    /// <summary>
	    /// Acquires an independent native reference for a typed managed view of an existing type.
	    /// </summary>
	    internal static IntPtr NewReferenceHandle(BinaryNinja.Type type)
	    {
		    if (null == type)
		    {
			    throw new ArgumentNullException(nameof(type));
		    }

		    return NativeMethods.BNNewTypeReference(type.DangerousGetHandle());
	    }
	
	    
	    public static string GetNameTypeString( NameType classFunctionType )
	    {
		    return Core.GetNameTypeString(classFunctionType);
	    }
	    
	    public static string GenerateAutoTypeId(string source  , QualifiedName name)
	    {
		    return Core.GenerateAutoTypeId(source , name);
	    }
	    
	    public static string GenerateAutoDemangledTypeId( QualifiedName name)
	    {
		    return Core.GenerateAutoDemangledTypeId( name);
	    }
	    
	    public static string GetAutoDemangledTypeIdSource()
	    {
		    return Core.GetAutoDemangledTypeIdSource();
	    }
	    
	    public static string GetAutoDebugTypeIdSource()
	    {
		    return Core.GetAutoDebugTypeIdSource();
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeType(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        return true;
	    }

	    public override string ToString()
	    {
		    return this.GetString();
	    }
	    
	    public NamedTypeReference GetNamedTypeReference()
	    {
		    return NamedTypeReference.MustTakeHandle(
			    NativeMethods.BNGetTypeNamedTypeReference(this.handle)
			);
	    }

	    /// <summary>
	    /// Dereferences any named type references to find the underlying type. If this type is
	    /// not a named type reference, the same type is returned unchanged. The result may still
	    /// be a named type reference if there are circular references. Mirrors Python
	    /// <c>Type.deref_named_type_reference</c> / C++ <c>Type::DerefNamedTypeReference</c>.
	    /// </summary>
	    /// <param name="view">The BinaryView that owns this type (used to resolve the reference).</param>
	    /// <returns>The resolved type, or null if the core returned no type.</returns>
	    public BinaryNinja.Type? DerefNamedTypeReference(BinaryView view)
	    {
		    return BinaryNinja.Type.TakeHandle(
			    NativeMethods.BNDerefNamedTypeReference(
				    view.DangerousGetHandle() ,
				    this.handle
			    )
		    );
	    }


	    public NameType GetNameType()
	    {
		    return NativeMethods.BNTypeGetNameType(this.handle);
	    }
	    
	    public bool ShouldDisplayReturnType()
	    {
		    return NativeMethods.BNTypeShouldDisplayReturnType(this.handle);
	    }
	    
	    public QualifiedName Name
	    {
		    get
		    {
			    return QualifiedName.TakeNative(
				    NativeMethods.BNTypeGetTypeName(this.handle)
				);
		    }
	    }
	    
	    public QualifiedName GetName()
	    {
		    return QualifiedName.TakeNative(
			    NativeMethods.BNTypeGetTypeName(this.handle)
		    );
	    }
	    
	    public string AlternateName
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNGetTypeAlternateName(this.handle)
			    );
		    }
	    }

	    public TypeWithConfidence WithConfidence(byte confidence )
	    {
		    return new TypeWithConfidence(
			    BinaryNinja.Type.MustNewFromHandle(this.DangerousGetHandle())
			    , confidence
			);
	    }
	    
	    /// <summary>
	    /// seems just size i bytes
	    /// </summary>
	    public ulong Width
	    {
		    get
		    {
			    return NativeMethods.BNGetTypeWidth(this.handle);
		    }
	    }
	    
	    /// <summary>
	    /// Whether this integer-like type is signed (wraps BNIsTypeSigned).
	    /// </summary>
	    public BoolWithConfidence Signed
	    {
	    	get
	    	{
	    		return BoolWithConfidence.FromNative(NativeMethods.BNIsTypeSigned(this.handle));
	    	}
	    }
	    
	    
	    public ulong Alignment
	    {
		    get
		    {
			    return NativeMethods.BNGetTypeAlignment(this.handle);
		    }
	    }
	    
	    public ulong Offset
	    {
		    get
		    {
			    return NativeMethods.BNGetTypeOffset(this.handle);
		    }
	    }

	    public string GetString(
		    Platform? platform = null,
		    TokenEscapingType escaping = TokenEscapingType.NoTokenEscapingType
		)
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNGetTypeString(
				    this.handle ,
				    null == platform ? IntPtr.Zero : platform.DangerousGetHandle() ,
				    escaping
			    )
		    );
	    }
	    
	    public string GetStringBeforeName(
		    Platform? platform = null,
		    TokenEscapingType escaping = TokenEscapingType.NoTokenEscapingType
	    )
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNGetTypeStringBeforeName(
				    this.handle ,
				    null == platform ? IntPtr.Zero : platform.DangerousGetHandle() ,
				    escaping
			    )
		    );
	    }
	    
	    public string GetStringAfterName(
		    Platform? platform = null,
		    TokenEscapingType escaping = TokenEscapingType.NoTokenEscapingType
	    )
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNGetTypeStringAfterName(
				    this.handle ,
				    null == platform ? IntPtr.Zero : platform.DangerousGetHandle() ,
				    escaping
			    )
		    );
	    }

	    public InstructionTextToken[] GetTokens(
		    byte baseConfidence = Core.MaxConfidence,
		    Platform? platform = null,
		    TokenEscapingType escaping = TokenEscapingType.NoTokenEscapingType
		)
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetTypeTokens(
			    this.handle ,
			    null == platform ? IntPtr.Zero : platform.DangerousGetHandle() ,
			    baseConfidence ,
			    escaping ,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken , InstructionTextToken>(
			    arrayPointer ,
			    arrayLength ,
			    InstructionTextToken.FromNative ,
			    NativeMethods.BNFreeInstructionText
		    );
	    }
	    
	    public InstructionTextToken[] GetTokensBeforeName(
		    byte baseConfidence = Core.MaxConfidence,
		    Platform? platform = null,
		    TokenEscapingType escaping = TokenEscapingType.NoTokenEscapingType
	    )
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetTypeTokensBeforeName(
			    this.handle ,
			    null == platform ? IntPtr.Zero : platform.DangerousGetHandle() ,
			    baseConfidence ,
			    escaping ,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken , InstructionTextToken>(
			    arrayPointer ,
			    arrayLength ,
			    InstructionTextToken.FromNative ,
			    NativeMethods.BNFreeInstructionText
		    );
	    }
	    
	    public InstructionTextToken[] GetTokensAfterName(
		    byte baseConfidence = Core.MaxConfidence,
		    Platform? platform = null,
		    TokenEscapingType escaping = TokenEscapingType.NoTokenEscapingType
	    )
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetTypeTokensAfterName(
			    this.handle ,
			    null == platform ? IntPtr.Zero : platform.DangerousGetHandle() ,
			    baseConfidence ,
			    escaping ,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken , InstructionTextToken>(
			    arrayPointer ,
			    arrayLength ,
			    InstructionTextToken.FromNative ,
			    NativeMethods.BNFreeInstructionText
		    );
	    }
	    
	    public TypeDefinitionLine[] GetLines(
		    TypeContainer types,
		    string name,
		    int paddingCols,
		    bool collapsed,
		    TokenEscapingType escaping = TokenEscapingType.NoTokenEscapingType
	    )
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetTypeLines(
			    this.handle ,
			    types.DangerousGetHandle(),
			    name,
			    paddingCols,
			    collapsed,
			    escaping ,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeStructArrayEx<BNTypeDefinitionLine , TypeDefinitionLine>(
			    arrayPointer ,
			    arrayLength ,
			    TypeDefinitionLine.FromNative ,
			    NativeMethods.BNFreeTypeDefinitionLineList
		    );
	    }
	    
	    public BoolWithConfidence IsConst
	    {
		    get
		    {
			    return BoolWithConfidence.FromNative(

				    NativeMethods.BNIsTypeConst(this.handle)
			    );
		    }
	    }
	    
	    public BoolWithConfidence IsVolatile
	    {
		    get
		    {
			    return BoolWithConfidence.FromNative(

				    NativeMethods.BNIsTypeVolatile(this.handle)
			    );
		    }
	    }
	    
	    public bool IsSystemCall
	    {
		    get
		    {
			    return NativeMethods.BNTypeIsSystemCall(this.handle);
		    }
	    }
	    
	    public uint GetSystemCallNumber()
	    {
		    return NativeMethods.BNTypeGetSystemCallNumber(this.handle);
	    }

	    /// <summary>
	    /// The calling convention name associated with this type.
	    /// </summary>
	    public CallingConventionName CallingConventionNameValue
	    {
		    get
		    {
			    return NativeMethods.BNGetTypeCallingConventionName(this.handle);
		    }
	    }

	    /// <summary>
	    /// Gets the value of a type attribute by name.
	    /// </summary>
	    public string GetAttributeByName(string name)
	    {
		    return UnsafeUtils.TakeAnsiString(
			    NativeMethods.BNGetTypeAttributeByName(this.handle , name)
		    );
	    }

	    /// <summary>
	    /// Gets all type attributes associated with this type.
	    /// </summary>
	    public unsafe TypeAttribute[] GetAttributes()
	    {
		    ulong count = 0;

		    IntPtr arrayPointer = NativeMethods.BNGetTypeAttributes(
			    this.handle ,
			    (IntPtr)(&count)
		    );

		    return UnsafeUtils.TakeStructArrayEx<BNTypeAttribute , TypeAttribute>(
			    arrayPointer ,
			    count ,
			    TypeAttribute.FromNative ,
			    NativeMethods.BNFreeTypeAttributeList
		    );
	    }

	    /// <summary>
	    /// Gets the type and name string, returning the name via out parameter.
	    /// </summary>
	    public unsafe string GetTypeAndName(
		    out QualifiedName name,
		    TokenEscapingType escaping = TokenEscapingType.NoTokenEscapingType
	    )
	    {
		    BNQualifiedName rawName = default;

		    IntPtr raw = NativeMethods.BNGetTypeAndName(
			    this.handle ,
			    (IntPtr)(&rawName) ,
			    escaping
		    );

		    name = QualifiedName.TakeNative(rawName);

		    return UnsafeUtils.TakeAnsiString(raw);
	    }

	    /// <summary>
	    /// Gets the reference type of this type (pointer, reference, etc.).
	    /// </summary>
	    public ReferenceType ReferenceType
	    {
		    get
		    {
			    return NativeMethods.BNTypeGetReferenceType(this.handle);
		    }
	    }

	    /// <summary>
	    /// Whether this type has template arguments.
	    /// </summary>
	    public bool HasTemplateArguments
	    {
		    get
		    {
			    return NativeMethods.BNTypeHasTemplateArguments(this.handle);
		    }
	    }

	    /// <summary>
	    /// Returns a new Type with the specified enumeration replaced.
	    /// </summary>
	    public BinaryNinja.Type WithReplacedEnumeration(Enumeration from, Enumeration to)
	    {
		    return BinaryNinja.Type.MustTakeHandle(
			    NativeMethods.BNTypeWithReplacedEnumeration(
				    this.handle ,
				    from.DangerousGetHandle() ,
				    to.DangerousGetHandle()
			    )
		    );
	    }

	    /// <summary>
	    /// Returns a new Type with the specified named type reference replaced.
	    /// </summary>
	    public BinaryNinja.Type WithReplacedNamedTypeReference(NamedTypeReference from, NamedTypeReference to)
	    {
		    return BinaryNinja.Type.MustTakeHandle(
			    NativeMethods.BNTypeWithReplacedNamedTypeReference(
				    this.handle ,
				    from.DangerousGetHandle() ,
				    to.DangerousGetHandle()
			    )
		    );
	    }

	    /// <summary>
	    /// Returns a new Type with the specified structure replaced.
	    /// </summary>
	    public BinaryNinja.Type WithReplacedStructure(Structure from, Structure to)
	    {
		    return BinaryNinja.Type.MustTakeHandle(
			    NativeMethods.BNTypeWithReplacedStructure(
				    this.handle ,
				    from.DangerousGetHandle() ,
				    to.DangerousGetHandle()
			    )
		    );
	    }

	    /// <summary>
	    /// Checks if two types are equal.
	    /// </summary>
	    public static bool TypesEqual(BinaryNinja.Type a, BinaryNinja.Type b)
	    {
		    return NativeMethods.BNTypesEqual(
			    a.DangerousGetHandle() ,
			    b.DangerousGetHandle()
		    );
	    }

	    /// <summary>
	    /// Checks if two types are not equal.
	    /// </summary>
	    public static bool TypesNotEqual(BinaryNinja.Type a, BinaryNinja.Type b)
	    {
		    return NativeMethods.BNTypesNotEqual(
			    a.DangerousGetHandle() ,
			    b.DangerousGetHandle()
		    );
	    }

	    /// <summary>
	    /// Generates instruction text tokens for accessing a member of this type at the
	    /// given offset. Also returns the chain of field names leading to the accessed member.
	    /// </summary>
	    /// <param name="data">The binary view providing data context.</param>
	    /// <param name="offset">The byte offset within the type to access.</param>
	    /// <param name="size">The size in bytes of the access.</param>
	    /// <param name="indirect">Whether the access is through a pointer indirection.</param>
	    /// <param name="tokens">Receives the instruction text tokens describing the member access.</param>
	    /// <param name="names">Receives the field name chain leading to the accessed member.</param>
	    /// <returns>True if the member access was resolved successfully.</returns>
	    public unsafe bool AddTypeMemberTokens(
		    BinaryView data ,
		    long offset ,
		    ulong size ,
		    bool indirect ,
		    out InstructionTextToken[] tokens ,
		    out string[] names
	    )
	    {
		    // 1. Stack-allocate output pointers for tokens and names.
		    IntPtr tokensPtr = IntPtr.Zero;
		    ulong tokenCount = 0;
		    IntPtr nameListPtr = IntPtr.Zero;
		    ulong nameCount = 0;

		    // 2. Call the native API.
		    bool ok = NativeMethods.BNAddTypeMemberTokens(
			    this.handle ,
			    data.DangerousGetHandle() ,
			    (IntPtr)(&tokensPtr) ,
			    (IntPtr)(&tokenCount) ,
			    offset ,
			    (IntPtr)(&nameListPtr) ,
			    (IntPtr)(&nameCount) ,
			    size ,
			    indirect ,
			    IntPtr.Zero
		    );

		    // 3. Marshal the results on success.
		    if (ok)
		    {
			    tokens = UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken , InstructionTextToken>(
				    tokensPtr ,
				    tokenCount ,
				    InstructionTextToken.FromNative ,
				    NativeMethods.BNFreeInstructionText
			    );

			    names = UnsafeUtils.TakeAnsiStringArray(
				    nameListPtr ,
				    nameCount ,
				    NativeMethods.BNFreeStringList
			    );
		    }
		    else
		    {
			    tokens = Array.Empty<InstructionTextToken>();
			    names = Array.Empty<string>();
		    }

		    return ok;
	    }

	    // TODO: EnumerateTypesForAccess requires callback delegate infrastructure
	    //       (void** terminal, void* ctxt).

	    /// <summary>
	    /// Gets the type class of this type (void, bool, integer, float, structure, etc.).
	    /// </summary>
	    public TypeClass TypeClass
	    {
		    get
		    {
			    return NativeMethods.BNGetTypeClass(this.handle);
		    }
	    }

	    /// <summary>
	    /// This type as a <see cref="StructureType"/>, or <c>null</c> if it is not a
	    /// structure. Safe alternative to a raw <c>new StructureType(type)</c> cast:
	    /// checks <see cref="TypeClass"/> and takes a fresh reference.
	    /// </summary>
	    public StructureType? AsStructure()
	    {
		    if (TypeClass.StructureTypeClass != this.TypeClass)
		    {
			    return null;
		    }

		    return new StructureType(NativeMethods.BNNewTypeReference(this.handle) , true);
	    }

	    /// <summary>
	    /// This type as an <see cref="EnumerationType"/>, or <c>null</c> if it is not
	    /// an enumeration.
	    /// </summary>
	    public EnumerationType? AsEnumeration()
	    {
		    if (TypeClass.EnumerationTypeClass != this.TypeClass)
		    {
			    return null;
		    }

		    return new EnumerationType(NativeMethods.BNNewTypeReference(this.handle) , true);
	    }

	    /// <summary>
	    /// This type as a <see cref="PointerType"/>, or <c>null</c> if it is not a
	    /// pointer.
	    /// </summary>
	    public PointerType? AsPointer()
	    {
		    if (TypeClass.PointerTypeClass != this.TypeClass)
		    {
			    return null;
		    }

		    return new PointerType(NativeMethods.BNNewTypeReference(this.handle) , true);
	    }

	    /// <summary>
	    /// This type as an <see cref="ArrayType"/>, or <c>null</c> if it is not an
	    /// array.
	    /// </summary>
	    public ArrayType? AsArray()
	    {
		    if (TypeClass.ArrayTypeClass != this.TypeClass)
		    {
			    return null;
		    }

		    return new ArrayType(NativeMethods.BNNewTypeReference(this.handle) , true);
	    }

	    /// <summary>
	    /// This type as a <see cref="FunctionType"/>, or <c>null</c> if it is not a
	    /// function type.
	    /// </summary>
	    public FunctionType? AsFunction()
	    {
		    if (TypeClass.FunctionTypeClass != this.TypeClass)
		    {
			    return null;
		    }

		    return new FunctionType(NativeMethods.BNNewTypeReference(this.handle) , true);
	    }

	    /// <summary>
	    /// This type as a <see cref="NamedTypeReferenceType"/>, or <c>null</c> if it is
	    /// not a named type reference.
	    /// </summary>
	    public NamedTypeReferenceType? AsNamedTypeReference()
	    {
		    if (TypeClass.NamedTypeReferenceClass != this.TypeClass)
		    {
			    return null;
		    }

		    return new NamedTypeReferenceType(NativeMethods.BNNewTypeReference(this.handle) , true);
	    }

	    /// <summary>
	    /// Gets whether this type represents a floating point value.
	    /// </summary>
	    public bool IsFloatingPoint
	    {
		    get
		    {
			    return NativeMethods.BNIsTypeFloatingPoint(this.handle);
		    }
	    }

	    /// <summary>
	    /// Creates a new enumeration type with the specified architecture, enumeration definition,
	    /// width, and signed-ness.
	    /// </summary>
	    /// <param name="arch">The architecture to associate with the type.</param>
	    /// <param name="enumeration">The enumeration definition.</param>
	    /// <param name="width">The width in bytes of the underlying integer type.</param>
	    /// <param name="isSigned">Whether the enumeration values are signed.</param>
	    /// <returns>A new enumeration Type, or null on failure.</returns>
	    public static unsafe BinaryNinja.Type? CreateEnumerationType(
		    Architecture arch ,
		    Enumeration enumeration ,
		    ulong width ,
		    BoolWithConfidence isSigned
	    )
	    {
		    // 1. Convert the BoolWithConfidence to its native representation.
		    BNBoolWithConfidence nativeSigned = isSigned.ToNative();

		    // 2. Call the native factory function.
		    return BinaryNinja.Type.TakeHandle(
			    NativeMethods.BNCreateEnumerationType(
				    arch.DangerousGetHandle() ,
				    enumeration.DangerousGetHandle() ,
				    width ,
				    (IntPtr)(&nativeSigned)
			    )
		    );
	    }

	    /// <summary>
	    /// Creates a new pointer type using an architecture to determine the pointer width.
	    /// </summary>
	    /// <param name="arch">The architecture to determine pointer width.</param>
	    /// <param name="pointee">The type that the pointer points to, with confidence.</param>
	    /// <param name="cnst">Whether the pointer is const, or null for default.</param>
	    /// <param name="vltl">Whether the pointer is volatile, or null for default.</param>
	    /// <param name="refType">The reference type (pointer, reference, etc.).</param>
	    /// <returns>A new pointer Type, or null on failure.</returns>
	    public static unsafe BinaryNinja.Type? CreatePointerType(
		    Architecture arch ,
		    TypeWithConfidence pointee ,
		    BoolWithConfidence? cnst = null ,
		    BoolWithConfidence? vltl = null ,
		    ReferenceType refType = ReferenceType.PointerReferenceType
	    )
	    {
		    // 1. Convert the type-with-confidence to its native representation.
		    BNTypeWithConfidence nativeType = pointee.ToNative();

		    // 2. Convert optional const/volatile to native.
		    BNBoolWithConfidence nativeCnst = (null == cnst ? new BoolWithConfidence(false) : cnst).ToNative();
		    BNBoolWithConfidence nativeVltl = (null == vltl ? new BoolWithConfidence(false) : vltl).ToNative();

		    // 3. Call the native factory function.
		    return BinaryNinja.Type.TakeHandle(
			    NativeMethods.BNCreatePointerType(
				    arch.DangerousGetHandle() ,
				    (IntPtr)(&nativeType) ,
				    (IntPtr)(&nativeCnst) ,
				    (IntPtr)(&nativeVltl) ,
				    refType
			    )
		    );
	    }

	    /// <summary>
	    /// Creates a new value type from a string representation.
	    /// </summary>
	    /// <param name="value">The string value to create the type from.</param>
	    /// <returns>A new value Type, or null on failure.</returns>
	    public static BinaryNinja.Type? CreateValueType(string value)
	    {
		    return BinaryNinja.Type.TakeHandle(
			    NativeMethods.BNCreateValueType(value)
		    );
	    }

	    /// <summary>
	    /// Creates a new var-args (variadic arguments) type used in function signatures.
	    /// </summary>
	    /// <returns>A new var-args Type, or null on failure.</returns>
	    public static BinaryNinja.Type? CreateVarArgsType()
	    {
		    return BinaryNinja.Type.TakeHandle(
			    NativeMethods.BNCreateVarArgsType()
		    );
	    }

	    /// <summary>
	    /// Gets the named type reference registered for this type, if this type was created
	    /// via a named type reference registration.
	    /// </summary>
	    /// <returns>The registered NamedTypeReference, or null if this type has no registered name.</returns>
	    public NamedTypeReference? GetRegisteredTypeName()
	    {
		    return NamedTypeReference.TakeHandle(
			    NativeMethods.BNGetRegisteredTypeName(this.handle)
		    );
	    }
	}

}
