using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNTypeDefinitionLine 
	{
		/// <summary>
		/// BNTypeDefinitionLineType lineType
		/// </summary>
		internal TypeDefinitionLineType lineType;
		
		/// <summary>
		/// BNInstructionTextToken* tokens
		/// </summary>
		internal IntPtr tokens;
		
		/// <summary>
		/// uint64_t count
		/// </summary>
		internal ulong count;
		
		/// <summary>
		/// BNType* type
		/// </summary>
		internal IntPtr type;
		
		/// <summary>
		/// BNType* parentType
		/// </summary>
		internal IntPtr parentType;
		
		/// <summary>
		/// BNType* rootType
		/// </summary>
		internal IntPtr rootType;
		
		/// <summary>
		/// const char* rootTypeName
		/// </summary>
		internal IntPtr rootTypeName;
		
		/// <summary>
		/// BNNamedTypeReference* baseType
		/// </summary>
		internal IntPtr baseType;
		
		/// <summary>
		/// uint64_t baseOffset
		/// </summary>
		internal ulong baseOffset;
		
		/// <summary>
		/// uint64_t offset
		/// </summary>
		internal ulong offset;
		
		/// <summary>
		/// uint64_t fieldIndex
		/// </summary>
		internal ulong fieldIndex;
	}

    public sealed class TypeDefinitionLine : INativeWrapperEx<BNTypeDefinitionLine>
    {
		public TypeDefinitionLineType LineType { get; set; } = TypeDefinitionLineType.TypedefLineType;
		
		public InstructionTextToken[] Tokens { get; set; } = Array.Empty<InstructionTextToken>();
		
		public BinaryNinja.Type? Type { get; set; } = null;
		
		public BinaryNinja.Type? ParentType { get; set; } = null;
		
		public BinaryNinja.Type? RootType { get; set; } = null;
		
		public string RootTypeName { get; set; } = string.Empty;
		
		public NamedTypeReference? BaseType { get; set; } = null;
		
		public ulong BaseOffset { get; set; } = 0;
		
		public ulong Offset { get; set; } = 0;
	
		public ulong FieldIndex { get; set; } = 0;
		
		public TypeDefinitionLine() 
		{
		    
		}

		internal static TypeDefinitionLine FromNative(BNTypeDefinitionLine native)
		{
			return new TypeDefinitionLine()
			{
				LineType = native.lineType ,
				Tokens = UnsafeUtils.ReadStructArray<BNInstructionTextToken , InstructionTextToken>(
					native.tokens ,
					native.count ,
					InstructionTextToken.FromNative
				) ,
				Type = BinaryNinja.Type.NewFromHandle(native.type) ,
				ParentType = BinaryNinja.Type.NewFromHandle(native.parentType) ,
				RootType = BinaryNinja.Type.NewFromHandle(native.rootType) ,
				RootTypeName = UnsafeUtils.ReadAnsiString(native.rootTypeName) ,
				BaseType = NamedTypeReference.NewFromHandle(native.baseType) ,
				BaseOffset = native.baseOffset ,
				Offset = native.offset ,
				FieldIndex = native.fieldIndex 
			};
		}

		public BNTypeDefinitionLine ToNativeEx(ScopedAllocator allocator)
		{
			return new BNTypeDefinitionLine()
			{
				lineType = this.LineType,
				tokens = allocator.AllocStructArray(
					
					allocator.ConvertToNativeArrayEx<BNInstructionTextToken,InstructionTextToken>(
						this.Tokens
					)
				),
				count = (ulong)this.Tokens.Length,
				type = ( null == this.Type ? IntPtr.Zero : this.Type.DangerousGetHandle() ),
				parentType = ( null == this.ParentType ? IntPtr.Zero : this.ParentType.DangerousGetHandle() ),
				rootType = ( null == this.RootType ? IntPtr.Zero : this.RootType.DangerousGetHandle() ),
				rootTypeName = ( 0 == this.RootTypeName.Length ? IntPtr.Zero : allocator.AllocAnsiString(this.RootTypeName) ),
				baseType =  ( null == this.BaseType ? IntPtr.Zero : this.BaseType.DangerousGetHandle() ),
				baseOffset = this.BaseOffset,
				offset = this.Offset,
				fieldIndex = this.FieldIndex
			};
		}
    }
}
