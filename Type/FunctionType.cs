using System;

namespace BinaryNinja
{
	public sealed class FunctionType : BinaryNinja.Type
	{
		public FunctionType(
			TypeWithConfidence? returnType = null ,
			CallingConventionWithConfidence? callingConvention = null ,
			FunctionParameter[]? parameters = null ,
			BoolWithConfidence? varArg  = null,
			BoolWithConfidence? canReturn  = null,
			OffsetWithConfidence? stackAdjust = null ,
			uint[]? regStackAdjustRegs = null,
			OffsetWithConfidence[]? regStackAdjustValues = null  ,
			RegisterSetWithConfidence? returnRegs = null ,
			NameType ft  = NameType.NoNameType,
			BoolWithConfidence? pure = null
		) : base(
			FunctionType.rawCreate(
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
			) , true)
		{

		}

		public FunctionType(BinaryNinja.Type type) 
			: base(BinaryNinja.Type.NewReferenceHandle(type), true)
		{
			
		}
		
		internal FunctionType(IntPtr handle , bool owner) : base(handle , owner)
		{

		}
		
		internal new static FunctionType? NewFromHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new FunctionType(
				NativeMethods.BNNewTypeReference(handle) ,
				true
			);
		}
	    
		internal new static FunctionType MustNewFromHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new FunctionType(
				NativeMethods.BNNewTypeReference(handle) ,
				true
			);
		}
	    
		internal new static FunctionType? TakeHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new FunctionType(handle, true);
		}
	    
		internal new static FunctionType MustTakeHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new FunctionType(handle, true);
		}
	    
		internal new static FunctionType? BorrowHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
		    
			return new FunctionType(handle, false);
		}
	    
		internal new static FunctionType MustBorrowHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle));
			}
		    
			return new FunctionType(handle, false);
		}

		private static IntPtr rawCreate(
			TypeWithConfidence returnType ,
			CallingConventionWithConfidence callingConvention,
			FunctionParameter[]  parameters,
			BoolWithConfidence varArg,
			BoolWithConfidence canReturn,
			OffsetWithConfidence  stackAdjust,
			uint[] regStackAdjustRegs,
			OffsetWithConfidence[] regStackAdjustValues,
			RegisterSetWithConfidence returnRegs,
			NameType ft ,
			BoolWithConfidence pure
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				BNTypeWithConfidence returnTypeNative = returnType.ToNative();
				BNRegisterSetWithConfidence returnRegistersNative =
					returnRegs.ToNativeEx(allocator);

				return NativeMethods.BNCreateFunctionType(
					returnTypeNative,
					callingConvention.ToNative(),
					allocator.ConvertToNativeArrayEx<BNFunctionParameter,FunctionParameter>(
						parameters
					),
					(ulong)parameters.Length,
					varArg.ToNative(),
					canReturn.ToNative(),
					stackAdjust.ToNative(),
					regStackAdjustRegs,
					UnsafeUtils.ConvertToNativeArray<BNOffsetWithConfidence,OffsetWithConfidence>(
						regStackAdjustValues
					),
					(ulong)regStackAdjustValues.Length,
					returnRegistersNative,
					ft,
					pure.ToNative()
				);
			}
		}

		public OffsetWithConfidence StackAdjustment
		{
			get
			{
				return OffsetWithConfidence.FromNative( NativeMethods.BNGetTypeStackAdjustment(this.handle) );
			}
		}

		public TypeWithConfidence ReturnType
		{
			get
			{
				return TypeWithConfidence.FromNative( NativeMethods.BNGetChildType(this.handle) );
			}
		}
		
		public CallingConventionWithConfidence CallingConvention
		{
			get
			{
				return CallingConventionWithConfidence.FromNative(NativeMethods.BNGetTypeCallingConvention(this.handle));
			}
		}

		public FunctionParameter[] Parameters
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetTypeParameters(
					this.handle , 
					out ulong arrayLength
				);
				
				return UnsafeUtils.TakeStructArrayEx<BNFunctionParameter , FunctionParameter>(
					arrayPointer,
					arrayLength,
					FunctionParameter.FromNative,
					NativeMethods.BNFreeTypeParameterList
				);
			}
		}

		public BoolWithConfidence HasVariableArguments
		{
			get
			{
				return BoolWithConfidence.FromNative(NativeMethods.BNTypeHasVariableArguments(this.handle));
			}
		}
		
		public BoolWithConfidence CanReturn
		{
			get
			{
				return BoolWithConfidence.FromNative(NativeMethods.BNFunctionTypeCanReturn(this.handle));
			}
		}
		
		public BoolWithConfidence IsPure
		{
			get
			{
				return BoolWithConfidence.FromNative(NativeMethods.BNIsTypePure(this.handle));
			}
		}
	}
}
