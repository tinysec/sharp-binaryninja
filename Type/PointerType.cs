using System;

namespace BinaryNinja
{
	public sealed class PointerType : BinaryNinja.Type
	{
		public PointerType(
			ulong width ,
			TypeWithConfidence kind,
			BoolWithConfidence? cnst = null,
			BoolWithConfidence? vltl = null, // volatile
			ReferenceType refType = ReferenceType.PointerReferenceType
		) : base( PointerType.create(
			width , 
			kind , 
			null == cnst ? false : cnst, 
			null == vltl ? false : vltl, 
			refType
		) , true)
		{
			
		}
		
		public PointerType(BinaryNinja.Type type) 
			: base(BinaryNinja.Type.NewReferenceHandle(type), true)
		{
			
		}
		
		internal PointerType(IntPtr handle , bool owner) : base(handle , owner)
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
			return NativeMethods.BNCreatePointerTypeOfWidth(
				width , 
				kind.ToNative() , 
				cnst.ToNative() ,  
				vltl.ToNative() ,  
				refType
			);
		}

		public TypeWithConfidence Pointee
		{
			get
			{
				BNTypeWithConfidence  raw = NativeMethods.BNGetChildType(this.handle);

				return TypeWithConfidence.FromNative(raw);
			}
		}

		/// <summary>
		/// The type this pointer points to. Alias of <see cref="Pointee"/> that matches
		/// the Python <c>PointerType.target</c> naming.
		/// </summary>
		public TypeWithConfidence Target
		{
			get
			{
				return this.Pointee;
			}
		}

		public PointerSuffix[] PointerSuffix
		{
			get
			{
				ulong arrayLength = 0;
				
				IntPtr arrayPointer = NativeMethods.BNGetTypePointerSuffix(this.handle , out arrayLength);

				return UnsafeUtils.TakeNumberArrayEx<PointerSuffix>(
					arrayPointer , 
					arrayLength,
					NativeMethods.BNFreePointerSuffixList
				);
			}
		}
		
		public string PointerSuffixString
		{
			get
			{
				return UnsafeUtils.TakeAnsiString(NativeMethods.BNGetTypePointerSuffixString(this.handle));
			}
		}
		
		public InstructionTextToken[] GetPointerSuffixTokens(byte baseConfidence)
		{
			ulong arrayLength = 0;
			
			IntPtr arrayPointer = NativeMethods.BNGetTypePointerSuffixTokens(
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

		public PointerBaseType PointerBaseType
		{
			get
			{
				return NativeMethods.BNTypeGetPointerBaseType(this.handle);
			}
		}
		
		public long PointerBaseOffset
		{
			get
			{
				return NativeMethods.BNTypeGetPointerBaseOffset(this.handle);
			}
		}
	}
}
