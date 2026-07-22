using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public class TypeBuilder : AbstractSafeHandle<TypeBuilder>
	{
		internal TypeBuilder(IntPtr handle , bool owner)
			: base(handle , owner)
		{

		}

        /// <summary>
        /// Creates a TypeBuilder from an existing Type instance.
        /// </summary>
        /// <param name="type">The type to create a builder from.</param>
        /// <returns>A new owned TypeBuilder, or null on failure.</returns>
        public static TypeBuilder? FromType(BinaryNinja.Type type)
        {
            // 1. Validate the required type parameter.
            if (null == type)
            {
                throw new ArgumentNullException(nameof(type));
            }

            // 2. Create a type builder from the given type; the returned handle is owned.
            return TypeBuilder.TakeHandle(
                NativeMethods.BNCreateTypeBuilderFromType(type.DangerousGetHandle())
            );
        }

        /// <summary>
        /// Creates a TypeBuilder representing a variable-arguments (varargs) type.
        /// </summary>
        /// <returns>A new owned TypeBuilder for varargs.</returns>
        public static TypeBuilder CreateVarArgs()
        {
            // Create a varargs type builder; the returned handle is owned.
            return TypeBuilder.MustTakeHandle(
                NativeMethods.BNCreateVarArgsTypeBuilder()
            );
        }

        /// <summary>
        /// Creates a TypeBuilder representing a value type with the given string value.
        /// </summary>
        /// <param name="value">The string value for the value type.</param>
        /// <returns>A new owned TypeBuilder for the value type.</returns>
        public static TypeBuilder CreateValue(string value)
        {
            // Create a value type builder; the returned handle is owned.
            return TypeBuilder.MustTakeHandle(
                NativeMethods.BNCreateValueTypeBuilder(value ?? string.Empty)
            );
        }

        /// <summary>
        /// Creates a TypeBuilder representing a structure type from a Structure.
        /// </summary>
        /// <param name="structure">The structure to create a type builder from.</param>
        /// <returns>A new owned TypeBuilder for the structure type.</returns>
        public static TypeBuilder CreateStructureType(Structure structure)
        {
            // 1. Validate the required structure parameter.
            if (null == structure)
            {
                throw new ArgumentNullException(nameof(structure));
            }

            // 2. Create a structure type builder; the returned handle is owned.
            return TypeBuilder.MustTakeHandle(
                NativeMethods.BNCreateStructureTypeBuilder(structure.DangerousGetHandle())
            );
        }

        /// <summary>
        /// Creates a TypeBuilder representing a structure type from a StructureBuilder.
        /// </summary>
        /// <param name="builder">The structure builder to create a type builder from.</param>
        /// <returns>A new owned TypeBuilder for the structure type.</returns>
        public static TypeBuilder CreateStructureTypeWithBuilder(StructureBuilder builder)
        {
            // 1. Validate the required builder parameter.
            if (null == builder)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            // 2. Create a structure type builder from the builder; the returned handle is owned.
            return TypeBuilder.MustTakeHandle(
                NativeMethods.BNCreateStructureTypeBuilderWithBuilder(builder.DangerousGetHandle())
            );
        }

        /// <summary>
        /// Creates a TypeBuilder representing an enumeration type from an Architecture, Enumeration, width, and signedness.
        /// </summary>
        /// <param name="arch">The architecture context.</param>
        /// <param name="enumeration">The enumeration definition.</param>
        /// <param name="width">The width of the enumeration type in bytes.</param>
        /// <param name="isSigned">The signedness with confidence.</param>
        /// <returns>A new owned TypeBuilder for the enumeration type.</returns>
        public static unsafe TypeBuilder CreateEnumerationType(
            Architecture arch,
            Enumeration enumeration,
            ulong width,
            BoolWithConfidence isSigned
        )
        {
            // 1. Validate required parameters.
            if (null == arch)
            {
                throw new ArgumentNullException(nameof(arch));
            }

            if (null == enumeration)
            {
                throw new ArgumentNullException(nameof(enumeration));
            }

            // 2. Convert signedness to native struct and take its address.
            BNBoolWithConfidence nativeSigned = isSigned.ToNative();

            // 3. Create an enumeration type builder; the returned handle is owned.
            return TypeBuilder.MustTakeHandle(
                NativeMethods.BNCreateEnumerationTypeBuilder(
                    arch.DangerousGetHandle(),
                    enumeration.DangerousGetHandle(),
                    width,
                    (IntPtr)(&nativeSigned)
                )
            );
        }

		internal static TypeBuilder? TakeHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new TypeBuilder(handle, true);
		}
	    
		internal static TypeBuilder MustTakeHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new TypeBuilder(handle, true);
		}
	    
		internal static TypeBuilder? BorrowHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new TypeBuilder(handle, false);
		}
	    
		internal static TypeBuilder MustBorrowHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new TypeBuilder(handle, false);
		}
		
		protected override bool ReleaseHandle()
		{
			if (!this.IsInvalid)
			{
				NativeMethods.BNFreeTypeBuilder(this.handle);
				this.SetHandleAsInvalid();
			}

			return true;
		}

		/// <summary>Creates an independent mutable copy of this type builder.</summary>
		public TypeBuilder Duplicate()
		{
			return TypeBuilder.MustTakeHandle(
				NativeMethods.BNDuplicateTypeBuilder(this.handle)
			);
		}

		public ulong Width
		{
			get
			{
				return NativeMethods.BNGetTypeBuilderWidth(this.handle);
			}

			set
			{
				NativeMethods.BNTypeBuilderSetWidth(this.handle , value);
			}
		}

		public BoolWithConfidence Const
		{
			get
			{
				return BoolWithConfidence.FromNative(NativeMethods.BNIsTypeBuilderConst(this.handle) );
			}

			set
			{
				NativeMethods.BNTypeBuilderSetConst(this.handle , value.ToNative());
			}
		}

		public BoolWithConfidence Volatile
		{
			get
			{
				return BoolWithConfidence.FromNative( NativeMethods.BNIsTypeBuilderVolatile(this.handle) );
			}

			set
			{
				NativeMethods.BNTypeBuilderSetVolatile(this.handle , value.ToNative());
			}
		}

		public ulong Alignment
		{
			get
			{
				return NativeMethods.BNGetTypeBuilderAlignment(this.handle);
			}

			set
			{
				NativeMethods.BNTypeBuilderSetAlignment(this.handle , value);
			}
		}

		public string AlternateName
		{
			get
			{
				return UnsafeUtils.TakeAnsiString(NativeMethods.BNGetTypeBuilderAlternateName(this.handle));
			}

			set
			{
				NativeMethods.BNTypeBuilderSetAlternateName(this.handle , value);
			}
		}

		public bool SystemCall
		{
			get
			{
				return NativeMethods.BNTypeBuilderIsSystemCall(this.handle);
			}
		}

		public uint SystemCallNumber
		{
			get
			{
				return NativeMethods.BNTypeBuilderGetSystemCallNumber(this.handle);
			}

			set
			{
				NativeMethods.BNTypeBuilderSetSystemCallNumber(this.handle , true , value);
			}
		}

		public void ClearSystemCall()
		{
			NativeMethods.BNTypeBuilderSetSystemCallNumber(this.handle , false , 0);
		}

		public TypeClass TypeClass
		{
			get
			{
				return NativeMethods.BNGetTypeBuilderClass(this.handle);
			}
		}

		public BoolWithConfidence Signed
		{
			get
			{
				return BoolWithConfidence.FromNative( NativeMethods.BNIsTypeBuilderSigned(this.handle) );
			}

			set
			{
				NativeMethods.BNTypeBuilderSetSigned(this.handle , value.ToNative());
			}
		}

		/// <summary>
		/// The reference type of this type builder (pointer, reference, etc.).
		/// </summary>
		public ReferenceType ReferenceType
		{
			get
			{
				return NativeMethods.BNTypeBuilderGetReferenceType(this.handle);
			}
		}

		/// <summary>
		/// The structure name associated with this type builder.
		/// </summary>
		public QualifiedName StructureName
		{
			get
			{
				return QualifiedName.TakeNative(NativeMethods.BNTypeBuilderGetStructureName(this.handle));
			}
		}

		/// <summary>
		/// The type name associated with this type builder.
		/// </summary>
		public QualifiedName TypeName
		{
			get
			{
				return QualifiedName.TakeNative(NativeMethods.BNTypeBuilderGetTypeName(this.handle));
			}

			set
			{
				using (ScopedAllocator allocator = new ScopedAllocator())
				{
					NativeMethods.BNTypeBuilderSetTypeName(
						this.handle ,
						allocator.AllocStruct<BNQualifiedName>(value.ToNativeEx(allocator))
					);
				}
			}
		}

		/// <summary>
		/// Whether this type builder has template arguments.
		/// </summary>
		public bool HasTemplateArguments
		{
			get
			{
				return NativeMethods.BNTypeBuilderHasTemplateArguments(this.handle);
			}

			set
			{
				NativeMethods.BNSetTypeBuilderHasTemplateArguments(this.handle , value);
			}
		}

		/// <summary>
		/// Get or set the calling convention name for this type builder.
		/// </summary>
		public CallingConventionName CallingConventionNameValue
		{
			get
			{
				return NativeMethods.BNGetTypeBuilderCallingConventionName(this.handle);
			}

			set
			{
				NativeMethods.BNTypeBuilderSetCallingConventionName(this.handle , value);
			}
		}

		/// <summary>
		/// The name type of this type builder (e.g. class, function, etc.).
		/// </summary>
		public NameType NameType
		{
			get
			{
				return NativeMethods.BNGetTypeBuilderNameType(this.handle);
			}
		}

		/// <summary>
		/// The enumeration associated with this type builder, or null if none.
		/// </summary>
		public Enumeration? EnumerationBuilder
		{
			get
			{
				return Enumeration.TakeHandle(
					NativeMethods.BNGetTypeBuilderEnumeration(this.handle)
				);
			}
		}

		/// <summary>
		/// The named type reference associated with this type builder, or null if none.
		/// </summary>
		public NamedTypeReference? NamedTypeReference
		{
			get
			{
				return NamedTypeReference.TakeHandle(
					NativeMethods.BNGetTypeBuilderNamedTypeReference(this.handle)
				);
			}
		}

		/// <summary>
		/// The structure associated with this type builder, or null if none.
		/// </summary>
		public Structure? StructureBuilder
		{
			get
			{
				return Structure.TakeHandle(
					NativeMethods.BNGetTypeBuilderStructure(this.handle)
				);
			}
		}

		/// <summary>
		/// Gets the value of a type attribute by name.
		/// </summary>
		public string GetAttributeByName(string name)
		{
			return UnsafeUtils.TakeAnsiString(
				NativeMethods.BNGetTypeBuilderAttributeByName(this.handle , name)
			);
		}

		/// <summary>
		/// Gets all type attributes associated with this type builder.
		/// </summary>
		public unsafe TypeAttribute[] GetAttributes()
		{
			ulong count = 0;

			IntPtr arrayPointer = NativeMethods.BNGetTypeBuilderAttributes(
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
		/// Gets the string representation of this type builder.
		/// </summary>
		public string GetString(Platform? platform = null)
		{
			return UnsafeUtils.TakeAnsiString(
				NativeMethods.BNGetTypeBuilderString(
					this.handle ,
					null == platform ? IntPtr.Zero : platform.DangerousGetHandle()
				)
			);
		}

		/// <summary>
		/// Gets the string representation of this type builder after the name.
		/// </summary>
		public string GetStringAfterName(Platform? platform = null)
		{
			return UnsafeUtils.TakeAnsiString(
				NativeMethods.BNGetTypeBuilderStringAfterName(
					this.handle ,
					null == platform ? IntPtr.Zero : platform.DangerousGetHandle()
				)
			);
		}

		/// <summary>
		/// Gets the string representation of this type builder before the name.
		/// </summary>
		public string GetStringBeforeName(Platform? platform = null)
		{
			return UnsafeUtils.TakeAnsiString(
				NativeMethods.BNGetTypeBuilderStringBeforeName(
					this.handle ,
					null == platform ? IntPtr.Zero : platform.DangerousGetHandle()
				)
			);
		}

		/// <summary>
		/// Gets the instruction text tokens for this type builder.
		/// </summary>
		public unsafe InstructionTextToken[] GetTokens(
			Platform? platform = null,
			byte baseConfidence = Core.MaxConfidence
		)
		{
			ulong count = 0;

			IntPtr arrayPointer = NativeMethods.BNGetTypeBuilderTokens(
				this.handle ,
				null == platform ? IntPtr.Zero : platform.DangerousGetHandle() ,
				baseConfidence ,
				(IntPtr)(&count)
			);

			return UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken , InstructionTextToken>(
				arrayPointer ,
				count ,
				InstructionTextToken.FromNative ,
				NativeMethods.BNFreeInstructionText
			);
		}

		/// <summary>
		/// Gets the instruction text tokens after the name for this type builder.
		/// </summary>
		public unsafe InstructionTextToken[] GetTokensAfterName(
			Platform? platform = null,
			byte baseConfidence = Core.MaxConfidence
		)
		{
			ulong count = 0;

			IntPtr arrayPointer = NativeMethods.BNGetTypeBuilderTokensAfterName(
				this.handle ,
				null == platform ? IntPtr.Zero : platform.DangerousGetHandle() ,
				baseConfidence ,
				(IntPtr)(&count)
			);

			return UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken , InstructionTextToken>(
				arrayPointer ,
				count ,
				InstructionTextToken.FromNative ,
				NativeMethods.BNFreeInstructionText
			);
		}

		/// <summary>
		/// Gets the instruction text tokens before the name for this type builder.
		/// </summary>
		public unsafe InstructionTextToken[] GetTokensBeforeName(
			Platform? platform = null,
			byte baseConfidence = Core.MaxConfidence
		)
		{
			ulong count = 0;

			IntPtr arrayPointer = NativeMethods.BNGetTypeBuilderTokensBeforeName(
				this.handle ,
				null == platform ? IntPtr.Zero : platform.DangerousGetHandle() ,
				baseConfidence ,
				(IntPtr)(&count)
			);

			return UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken , InstructionTextToken>(
				arrayPointer ,
				count ,
				InstructionTextToken.FromNative ,
				NativeMethods.BNFreeInstructionText
			);
		}

		/// <summary>
		/// Gets the type and name string, returning the name via out parameter.
		/// </summary>
		public unsafe string GetTypeAndName(out QualifiedName name)
		{
			BNQualifiedName rawName = default;

			IntPtr raw = NativeMethods.BNGetTypeBuilderTypeAndName(
				this.handle ,
				(IntPtr)(&rawName)
			);

			name = QualifiedName.TakeNative(rawName);

			return UnsafeUtils.TakeAnsiString(raw);
		}

		/// <summary>
		/// Gets whether this type builder represents a floating-point type.
		/// </summary>
		public bool IsFloatingPoint
		{
			get
			{
				// Query the native API for the floating-point flag.
				return NativeMethods.BNIsTypeBuilderFloatingPoint(this.handle);
			}
		}

		/// <summary>
		/// Removes a named attribute from this type builder.
		/// </summary>
		/// <param name="name">The name of the attribute to remove.</param>
		public void RemoveAttribute(string name)
		{
			// Forward the removal to the native API.
			NativeMethods.BNRemoveTypeBuilderAttribute(this.handle , name ?? string.Empty);
		}

		/// <summary>
		/// Sets (or adds) a named attribute on this type builder.
		/// </summary>
		/// <param name="name">The attribute name.</param>
		/// <param name="value">The attribute value.</param>
		public void SetAttribute(string name , string value)
		{
			// Forward the name-value pair to the native API.
			NativeMethods.BNSetTypeBuilderAttribute(
				this.handle ,
				name ?? string.Empty ,
				value ?? string.Empty
			);
		}

		/// <summary>
		/// Replaces the entire attribute list on this type builder.
		/// </summary>
		/// <param name="attributes">The array of type attributes to set.</param>
		public unsafe void SetAttributeList(TypeAttribute[] attributes)
		{
			// 1. Validate the required parameter.
			if (null == attributes)
			{
				throw new ArgumentNullException(nameof(attributes));
			}

			// 2. Marshal the managed array to a native array and forward to the native API.
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				// 2.1 Build the native array of BNTypeAttribute structs.
				BNTypeAttribute[] nativeAttrs = new BNTypeAttribute[attributes.Length];

				for (int i = 0; i < attributes.Length; i++)
				{
					nativeAttrs[i].name = allocator.AllocAnsiString(attributes[i].Name ?? string.Empty);
					nativeAttrs[i]._value = allocator.AllocAnsiString(attributes[i].Value ?? string.Empty);
				}

				// 2.2 Allocate the native array and copy the structs.
				IntPtr nativeArray = allocator.AllocStructArray<BNTypeAttribute>(nativeAttrs);

				// 3. Call the native API with the marshalled array and count.
				NativeMethods.BNSetTypeBuilderAttributeList(
					this.handle ,
					nativeArray ,
					(ulong)attributes.Length
				);
			}
		}

		/// <summary>
		/// Sets the name type for this type builder.
		/// </summary>
		/// <param name="nameType">The name type to set.</param>
		public void SetNameType(NameType nameType)
		{
			// Forward the name type to the native API.
			NativeMethods.BNSetTypeBuilderNameType(this.handle , nameType);
		}

		/// <summary>
		/// Sets the named type reference for this type builder.
		/// </summary>
		/// <param name="namedTypeRef">The named type reference to set.</param>
		public void SetNamedTypeReference(NamedTypeReference namedTypeRef)
		{
			// 1. Validate the required parameter.
			if (null == namedTypeRef)
			{
				throw new ArgumentNullException(nameof(namedTypeRef));
			}

			// 2. Forward to the native API.
			NativeMethods.BNSetTypeBuilderNamedTypeReference(
				this.handle ,
				namedTypeRef.DangerousGetHandle()
			);
		}

		/// <summary>
		/// Sets the integer display type for this type builder.
		/// </summary>
		/// <param name="displayType">The integer display type to set.</param>
		public void SetIntegerTypeDisplayType(IntegerDisplayType displayType)
		{
			// Forward the display type to the native API.
			NativeMethods.BNSetIntegerTypeDisplayType(this.handle , displayType);
		}

		/// <summary>
		/// Creates a TypeBuilder representing an enumeration type from an Architecture, EnumerationBuilder, width, and signedness.
		/// </summary>
		/// <param name="arch">The architecture context.</param>
		/// <param name="builder">The enumeration builder definition.</param>
		/// <param name="width">The width of the enumeration type in bytes.</param>
		/// <param name="isSigned">The signedness with confidence.</param>
		/// <returns>A new owned TypeBuilder for the enumeration type.</returns>
		public static unsafe TypeBuilder CreateEnumerationTypeWithBuilder(
			Architecture arch ,
			EnumerationBuilder builder ,
			ulong width ,
			BoolWithConfidence isSigned
		)
		{
			// 1. Validate required parameters.
			if (null == arch)
			{
				throw new ArgumentNullException(nameof(arch));
			}

			if (null == builder)
			{
				throw new ArgumentNullException(nameof(builder));
			}

			// 2. Convert signedness to native struct and take its address.
			BNBoolWithConfidence nativeSigned = isSigned.ToNative();

			// 3. Create an enumeration type builder from the builder; the returned handle is owned.
			return TypeBuilder.MustTakeHandle(
				NativeMethods.BNCreateEnumerationTypeBuilderWithBuilder(
					arch.DangerousGetHandle() ,
					builder.DangerousGetHandle() ,
					width ,
					(IntPtr)(&nativeSigned)
				)
			);
		}

		/// <summary>
		/// Creates a named type reference TypeBuilder from a BinaryView and qualified name.
		/// </summary>
		/// <param name="view">The binary view to look up the type in.</param>
		/// <param name="name">The qualified name of the type.</param>
		/// <returns>A new owned TypeBuilder, or null on failure.</returns>
		public static TypeBuilder? CreateNamedTypeReferenceBuilderFromType(
			BinaryView view ,
			QualifiedName name
		)
		{
			// 1. Validate required parameters.
			if (null == view)
			{
				throw new ArgumentNullException(nameof(view));
			}

			// 2. Marshal the qualified name and create the builder.
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				return TypeBuilder.TakeHandle(
					NativeMethods.BNCreateNamedTypeReferenceBuilderFromType(
						view.DangerousGetHandle() ,
						allocator.AllocStruct<BNQualifiedName>(name.ToNativeEx(allocator))
					)
				);
			}
		}

		/// <summary>
		/// Creates a named type reference TypeBuilder from an ID string, qualified name, and type.
		/// </summary>
		/// <param name="id">The type identifier string.</param>
		/// <param name="name">The qualified name of the type.</param>
		/// <param name="type">The type to create the reference from.</param>
		/// <returns>A new owned TypeBuilder, or null on failure.</returns>
		public static TypeBuilder? CreateNamedTypeReferenceBuilderFromTypeAndId(
			string id ,
			QualifiedName name ,
			BinaryNinja.Type type
		)
		{
			// 1. Validate required parameters.
			if (null == type)
			{
				throw new ArgumentNullException(nameof(type));
			}

			// 2. Marshal the qualified name and create the builder.
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				return TypeBuilder.TakeHandle(
					NativeMethods.BNCreateNamedTypeReferenceBuilderFromTypeAndId(
						id ?? string.Empty ,
						allocator.AllocStruct<BNQualifiedName>(name.ToNativeEx(allocator)) ,
						type.DangerousGetHandle()
					)
				);
			}
		}

		/// <summary>
		/// Creates a named type reference TypeBuilder from a NamedTypeReferenceBuilder, width, alignment, and const/volatile.
		/// </summary>
		/// <param name="builder">The named type reference builder.</param>
		/// <param name="width">The width of the type in bytes.</param>
		/// <param name="align">The alignment of the type in bytes.</param>
		/// <param name="cnst">The const qualifier with confidence.</param>
		/// <param name="vltl">The volatile qualifier with confidence.</param>
		/// <returns>A new owned TypeBuilder for the named type reference.</returns>
		public static unsafe TypeBuilder CreateNamedTypeReferenceBuilderWithBuilder(
			NamedTypeReferenceBuilder builder ,
			ulong width ,
			ulong align ,
			BoolWithConfidence cnst ,
			BoolWithConfidence vltl
		)
		{
			// 1. Validate the required builder parameter.
			if (null == builder)
			{
				throw new ArgumentNullException(nameof(builder));
			}

			// 2. Convert const and volatile to native structs and take their addresses.
			BNBoolWithConfidence nativeCnst = cnst.ToNative();
			BNBoolWithConfidence nativeVltl = vltl.ToNative();

			// 3. Create the builder; the returned handle is owned.
			return TypeBuilder.MustTakeHandle(
				NativeMethods.BNCreateNamedTypeReferenceBuilderWithBuilder(
					builder.DangerousGetHandle() ,
					width ,
					align ,
					(IntPtr)(&nativeCnst) ,
					(IntPtr)(&nativeVltl)
				)
			);
		}

		/// <summary>
		/// Creates a pointer TypeBuilder using an architecture to determine pointer width.
		/// </summary>
		/// <param name="arch">The architecture that determines pointer width.</param>
		/// <param name="type">The pointed-to type with confidence.</param>
		/// <param name="cnst">Optional const qualifier with confidence.</param>
		/// <param name="vltl">Optional volatile qualifier with confidence.</param>
		/// <param name="refType">The reference type (pointer, reference, etc.).</param>
		/// <returns>A new owned TypeBuilder for the pointer type.</returns>
		public static unsafe TypeBuilder CreatePointerTypeBuilder(
			Architecture arch ,
			TypeWithConfidence type ,
			BoolWithConfidence? cnst = null ,
			BoolWithConfidence? vltl = null ,
			ReferenceType refType = ReferenceType.PointerReferenceType
		)
		{
			// 1. Validate the required architecture parameter.
			if (null == arch)
			{
				throw new ArgumentNullException(nameof(arch));
			}

			// 2. Convert all structs to native form and take their addresses.
			BNTypeWithConfidence nativeType = type.ToNative();
			BNBoolWithConfidence nativeCnst = (cnst ?? new BoolWithConfidence()).ToNative();
			BNBoolWithConfidence nativeVltl = (vltl ?? new BoolWithConfidence()).ToNative();

			// 3. Create the pointer type builder; the returned handle is owned.
			return TypeBuilder.MustTakeHandle(
				NativeMethods.BNCreatePointerTypeBuilder(
					arch.DangerousGetHandle() ,
					(IntPtr)(&nativeType) ,
					(IntPtr)(&nativeCnst) ,
					(IntPtr)(&nativeVltl) ,
					refType
				)
			);
		}
	}

	public sealed class VoidTypeBuilder : TypeBuilder
	{
		public VoidTypeBuilder() 
			: base(NativeMethods.BNCreateVoidTypeBuilder() , true)
		{
			
		}
		
		public VoidType Build()
		{
			return new VoidType( NativeMethods.BNFinalizeTypeBuilder(this.handle) , true);
		}
	}
	
	public sealed class BoolTypeBuilder : TypeBuilder
	{
		public BoolTypeBuilder() 
			: base(NativeMethods.BNCreateBoolTypeBuilder() , true)
		{
			
		}

		public BoolType Build()
		{
			return new BoolType( NativeMethods.BNFinalizeTypeBuilder(this.handle) , true);
		}
	}
	
	public sealed class IntegerTypeBuilder : TypeBuilder
	{
		public IntegerTypeBuilder(
			ulong width , 
			BoolWithConfidence? sign = null, 
			string altName = ""
		) : base(
			IntegerTypeBuilder.create(
				width ,
				null == sign ? new BoolWithConfidence() : sign ,
				altName 
				) , true)
		{
			
		}
	
		internal static IntPtr create(
			ulong width , 
			BoolWithConfidence sign,
			string altName = ""
		)
		{
			
			return NativeMethods.BNCreateIntegerTypeBuilder(
				width , 
				sign.ToNative() ,
				altName
			);
		}
		
		public IntegerType Build()
		{
			return new IntegerType( NativeMethods.BNFinalizeTypeBuilder(this.handle) , true);
		}
	}
	
	public sealed class FloatTypeBuilder : TypeBuilder
	{
		public FloatTypeBuilder(ulong width , string altName = "") 
			: base(NativeMethods.BNCreateFloatTypeBuilder(width , altName ) , true)
		{
			
		}

		public FloatType Build()
		{
			return new FloatType( NativeMethods.BNFinalizeTypeBuilder(this.handle) , true);
		}
	}
	
	public sealed class CharTypeBuilder : TypeBuilder
	{
		public CharTypeBuilder(ulong width ,  string altName) 
			: base(
				IntegerTypeBuilder.create(
					width , 
					new BoolWithConfidence(true, Core.MaxConfidence) ,
					altName 
				) ,
				true
		)
		{
			
		}
		
		public CharType Build()
		{
			return new CharType( NativeMethods.BNFinalizeTypeBuilder(this.handle) , true);
		}
	}
	
	public sealed class WideCharTypeBuilder : TypeBuilder
	{
		public WideCharTypeBuilder(ulong width , string altName = "") 
			: base(NativeMethods.BNCreateWideCharTypeBuilder(width , altName ) , true)
		{
			
		}

		public WideCharType Build()
		{
			return new WideCharType( NativeMethods.BNFinalizeTypeBuilder(this.handle) , true);
		}
	}
	
	public sealed class PointerTypeBuilder : TypeBuilder
	{
		public PointerTypeBuilder(
			ulong width ,
			TypeWithConfidence kind ,
			BoolWithConfidence? cnst = null ,
			BoolWithConfidence? vltl = null, // volatile
			ReferenceType refType = ReferenceType.PointerReferenceType
		) 
			: base(
				PointerTypeBuilder.create(
					width , 
					kind , 
					cnst ?? new BoolWithConfidence() , 
					vltl?? new BoolWithConfidence() , 
					refType
				) , true)
		{
			
		}

		
		private static IntPtr create(
			ulong width ,
			TypeWithConfidence kind,
			BoolWithConfidence cnst ,
			BoolWithConfidence vltl, // volatile
			ReferenceType refType 
		)
		{
			return NativeMethods.BNCreatePointerTypeBuilderOfWidth(
				width , 
				kind.ToNative() , 
				cnst.ToNative() ,  
				vltl.ToNative() ,  
				refType
			);
		}
		
		public PointerType Build()
		{
			return new PointerType( NativeMethods.BNFinalizeTypeBuilder(this.handle) , true);
		}

		/// <summary>
		/// The type this pointer builder points to. Mirrors Python PointerBuilder.target and
		/// the sibling ArrayTypeBuilder.ElementType / FunctionTypeBuilder.ReturnType accessors.
		/// </summary>
		public TypeWithConfidence Target
		{
			get
			{
				return TypeWithConfidence.FromNative( NativeMethods.BNGetTypeBuilderChildType(this.handle) );
			}

			set
			{
				NativeMethods.BNTypeBuilderSetChildType(this.handle , value.ToNative());
			}
		}

		public ulong Offset
		{
			get
			{
				return NativeMethods.BNGetTypeBuilderOffset(this.handle);
			}

			set
			{
				NativeMethods.BNSetTypeBuilderOffset(this.handle, value);
			}
		}
		
		public PointerSuffix[] PointerSuffix
		{
			get
			{
				ulong arrayLength = 0;
				
				IntPtr arrayPointer = NativeMethods.BNGetTypeBuilderPointerSuffix(this.handle , out arrayLength);

				return UnsafeUtils.TakeNumberArrayEx<PointerSuffix>(
					arrayPointer ,
					arrayLength ,
					NativeMethods.BNFreePointerSuffixList
				);
			}

			set
			{
				NativeMethods.BNSetTypeBuilderPointerSuffix(
					this.handle, 
					value,
					(ulong)value.Length
				);
			}
		}
		
		public string PointerSuffixString
		{
			get
			{
				return UnsafeUtils.TakeAnsiString(NativeMethods.BNGetTypeBuilderPointerSuffixString(this.handle));
			}
		}
		
		public void AddPointerSuffix(PointerSuffix suffix)
		{
			NativeMethods.BNAddTypeBuilderPointerSuffix(this.handle, suffix);
		}

		public InstructionTextToken[] GetPointerSuffixTokens(byte baseConfidence)
		{
			ulong arrayLength = 0;
			
			IntPtr arrayPointer = NativeMethods.BNGetTypeBuilderPointerSuffixTokens(
				this.handle ,
				baseConfidence , 
				out arrayLength
			);

			return UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken , InstructionTextToken>(
				arrayPointer,
				arrayLength,
				InstructionTextToken.FromNative,
				NativeMethods.BNFreeInstructionText
			);
		}

		public void SetPointerBase(PointerBaseType baseType , long baseOffset)
		{
			NativeMethods.BNSetTypeBuilderPointerBase(this.handle , baseType , baseOffset);
		}
		
		public PointerBaseType PointerBaseType
		{
			get
			{
				return NativeMethods.BNTypeBuilderGetPointerBaseType(this.handle);
			}
		}

		public long PointerBaseOffset
		{
			get
			{
				return NativeMethods.BNTypeBuilderGetPointerBaseOffset(this.handle);
			}

			set
			{
				NativeMethods.BNSetTypeBuilderPointerBase(this.handle, this.PointerBaseType, value);
			}
		}
		
	}

	public sealed class ArrayTypeBuilder : TypeBuilder
	{
		public ArrayTypeBuilder(
			TypeWithConfidence elementType ,
			ulong elementCount
		) : base(ArrayTypeBuilder.create(elementType , elementCount) , true)
		{

		}

		private static IntPtr create(
			TypeWithConfidence elementType ,
			ulong elementCount
		)
		{
			return NativeMethods.BNCreateArrayTypeBuilder(
				elementType.ToNative() ,
				elementCount
			);
		}
		
		public ArrayType Build()
		{
			return new ArrayType( NativeMethods.BNFinalizeTypeBuilder(this.handle) , true);
		}
		
		public ulong ElementCount
		{
			get
			{
				return NativeMethods.BNGetTypeBuilderElementCount(this.handle);
			}

		}

		public TypeWithConfidence ElementType
		{
			get
			{
				return TypeWithConfidence.FromNative( NativeMethods.BNGetTypeBuilderChildType(this.handle) );
			}

			set
			{
				NativeMethods.BNTypeBuilderSetChildType(this.handle , value.ToNative());
			}
		}

	}

	public sealed class FunctionTypeBuilder : TypeBuilder
	{
		public FunctionTypeBuilder(
			TypeWithConfidence? returnType ,
			CallingConventionWithConfidence? callingConvention ,
			FunctionParameter[]? parameters ,
			BoolWithConfidence? varArg ,
			BoolWithConfidence? canReturn ,
			OffsetWithConfidence? stackAdjust ,
			uint[]? regStackAdjustRegs = null,
			OffsetWithConfidence[]? regStackAdjustValues = null  ,
			RegisterSetWithConfidence? returnRegs = null ,
			NameType ft  = NameType.NoNameType,
			BoolWithConfidence? pure = null
		) : base(
			FunctionTypeBuilder.create(
				returnType ?? new TypeWithConfidence(new VoidType() , 0),
				callingConvention ?? new CallingConventionWithConfidence(callingConvention?.Convention , 0) ,
				parameters ?? Array.Empty<FunctionParameter>() ,
				varArg ?? new BoolWithConfidence() ,
				canReturn  ?? new BoolWithConfidence(),
				stackAdjust ?? new OffsetWithConfidence(),
				regStackAdjustRegs ?? Array.Empty<uint>() ,
				regStackAdjustValues ?? Array.Empty<OffsetWithConfidence>() ,
				returnRegs ?? new RegisterSetWithConfidence() ,
				ft ,
				pure ?? new BoolWithConfidence()
			) , true
		)
		{

		}
		
		private static IntPtr create(
			TypeWithConfidence returnType ,
			CallingConventionWithConfidence callingConvention ,
			FunctionParameter[] parameters ,
			BoolWithConfidence varArg ,
			BoolWithConfidence canReturn ,
			OffsetWithConfidence stackAdjust ,
			uint[] regStackAdjustRegs ,
			OffsetWithConfidence[] regStackAdjustValues ,
			RegisterSetWithConfidence returnRegs ,
			NameType ft ,
			BoolWithConfidence pure
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				BNTypeWithConfidence returnTypeNative = returnType.ToNative();
				BNRegisterSetWithConfidence returnRegistersNative =
					returnRegs.ToNativeEx(allocator);

				return NativeMethods.BNCreateFunctionTypeBuilder(
					returnTypeNative ,
					callingConvention.ToNative() ,
					allocator.ConvertToNativeArrayEx<BNFunctionParameter,FunctionParameter>(
						parameters
					),
					(ulong)parameters.Length ,
					varArg.ToNative() ,
					canReturn.ToNative() ,
					stackAdjust.ToNative() ,
					regStackAdjustRegs ,
					UnsafeUtils.ConvertToNativeArray<BNOffsetWithConfidence,OffsetWithConfidence>(
						regStackAdjustValues
						),
					(ulong)regStackAdjustValues.Length ,
					returnRegistersNative,
					ft ,
					pure.ToNative()
				);
			}
		}
		
		public FunctionType Build()
		{
			return new FunctionType( NativeMethods.BNFinalizeTypeBuilder(this.handle) , true);
		}

		public OffsetWithConfidence StackAdjustment
		{
			get
			{
				return OffsetWithConfidence.FromNative( NativeMethods.BNGetTypeBuilderStackAdjustment(this.handle) );
			}

			set
			{
				NativeMethods.BNTypeBuilderSetStackAdjustment(this.handle , value.ToNative() );
			}
		}

		public TypeWithConfidence ReturnType
		{
			get
			{
				return TypeWithConfidence.FromNative( NativeMethods.BNGetTypeBuilderChildType(this.handle) );
			}

			set
			{
				NativeMethods.BNTypeBuilderSetChildType(this.handle, value.ToNative() );
			}
		}
		
		public CallingConventionWithConfidence CallingConvention
		{
			get
			{
				return CallingConventionWithConfidence.FromNative(NativeMethods.BNGetTypeBuilderCallingConvention(this.handle));
			}

			set
			{
				NativeMethods.BNTypeBuilderSetCallingConvention(this.handle, value.ToNative() );
			}
		}

		public FunctionParameter[] Parameters
		{
			get
			{
				ulong arrayLength = 0;
				
				IntPtr arrayPointer = NativeMethods.BNGetTypeBuilderParameters(this.handle , out arrayLength);
				
				return UnsafeUtils.TakeStructArrayEx<BNFunctionParameter , FunctionParameter>(
					arrayPointer,
					arrayLength,
					FunctionParameter.FromNative,
					NativeMethods.BNFreeTypeParameterList
				);
			}

			set
			{
				using (ScopedAllocator allocator = new ScopedAllocator())
				{
					NativeMethods.BNSetFunctionTypeBuilderParameters(
						this.handle ,
						allocator.ConvertToNativeArrayEx<BNFunctionParameter,FunctionParameter>(value),
						(ulong)value.Length
					);
				}
			}
		}

		public BoolWithConfidence HasVariableArguments
		{
			get
			{
				return BoolWithConfidence.FromNative(NativeMethods.BNTypeBuilderHasVariableArguments(this.handle));
			}
		}
		
		public BoolWithConfidence CanReturn
		{
			get
			{
				return BoolWithConfidence.FromNative(NativeMethods.BNFunctionTypeBuilderCanReturn(this.handle));
			}

			set
			{
				NativeMethods.BNSetFunctionTypeBuilderCanReturn(this.handle, value.ToNative());
			}
		}
		
		public BoolWithConfidence Pure
		{
			get
			{
				return BoolWithConfidence.FromNative(NativeMethods.BNIsTypeBuilderPure(this.handle));
			}

			set
			{
				NativeMethods.BNSetTypeBuilderPure(this.handle, value.ToNative());
			}
		}
	}

	public sealed class NamedTypeReferenceTypeBuilder : TypeBuilder
	{
		public NamedTypeReferenceTypeBuilder(
			NamedTypeReference namedType ,
			ulong width ,
			ulong align ,
			BoolWithConfidence cnst ,
			BoolWithConfidence vltl
		)
			: base(
				NamedTypeReferenceTypeBuilder.create(
					namedType ,
					width ,
					align ,
					cnst ,
					vltl
				) ,
				true
			)
		{

		}
		
		private static IntPtr create(
			NamedTypeReference namedType ,
			ulong width ,
			ulong align ,
			BoolWithConfidence cnst ,
			BoolWithConfidence vltl
		)
		{
			return NativeMethods.BNCreateNamedTypeReferenceBuilder(
				namedType.DangerousGetHandle() ,
				width ,
				align ,
				cnst.ToNative() ,
				vltl.ToNative()
			);
		}
		
		public NamedTypeReferenceType Build()
		{
			return new NamedTypeReferenceType( NativeMethods.BNFinalizeTypeBuilder(this.handle) , true);
		}
		
		public QualifiedName Name
		{
			get
			{
				return QualifiedName.FromNative(NativeMethods.BNGetTypeReferenceBuilderName(this.handle));
			}
		}
		
		public NamedTypeReferenceClass NamedTypeReferenceClass
		{
			get
			{
				return NativeMethods.BNGetTypeReferenceBuilderClass(this.handle);
			}
		}
		
		public string TypeReferenceId
		{
			get
			{
				return UnsafeUtils.TakeAnsiString(NativeMethods.BNGetTypeReferenceBuilderId(this.handle));
			}
		}

	}

	

}
