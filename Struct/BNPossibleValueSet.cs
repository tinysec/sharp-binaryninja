using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNPossibleValueSet 
	{
		/// <summary>
		/// BNRegisterValueType state
		/// </summary>
		internal RegisterValueType state;
		
		/// <summary>
		/// int64_t value
		/// </summary>
		internal long value;
		
		/// <summary>
		/// int64_t offset
		/// </summary>
		internal long offset;
		
		/// <summary>
		/// uint64_t size
		/// </summary>
		internal ulong size;
		
		/// <summary>
		/// BNValueRange* ranges
		/// </summary>
		internal IntPtr ranges;
		
		/// <summary>
		/// int64_t* valueSet
		/// </summary>
		internal IntPtr valueSet;
		
		/// <summary>
		/// BNLookupTableEntry* table
		/// </summary>
		internal IntPtr table;
		
		/// <summary>
		/// uint64_t count
		/// </summary>
		internal ulong count;
	}

    public sealed class PossibleValueSet : INativeWrapperEx<BNPossibleValueSet>
    {
		private delegate BNPossibleValueSet BinaryOperation(
			IntPtr left,
			IntPtr right,
			UIntPtr size
		);

		private delegate BNPossibleValueSet UnaryOperation(IntPtr value, UIntPtr size);

		public RegisterValueType State { get; set; } = RegisterValueType.UndeterminedValue;
		
		public long Value { get; set; } = 0;
		
		public long Offset { get; set; } = 0;
		
		public ulong Size { get; set; } = 0;
		
		public ValueRange[] Ranges { get; set; } = Array.Empty<ValueRange>();
		
		public ulong[] ValueSet { get; set; } = Array.Empty<ulong>();
		
		public LookupTableEntry[] Table { get; set; } = Array.Empty<LookupTableEntry>();
		
		public PossibleValueSet() 
		{
		    
		}

		internal static PossibleValueSet FromNative(BNPossibleValueSet native)
		{
			return new PossibleValueSet()
			{
				State = native.state ,
				Value = native.value ,
				Offset = native.offset ,
				Size = native.size ,
				Ranges = UnsafeUtils.ReadStructArray<BNValueRange , ValueRange>(
					native.ranges ,
					native.count ,
					ValueRange.FromNative
				),
				ValueSet = UnsafeUtils.ReadNumberArray<ulong>(
					native.valueSet ,
					native.count 
				),
				Table = UnsafeUtils.ReadStructArray<BNLookupTableEntry , LookupTableEntry>(
					native.table ,
					native.count ,
					LookupTableEntry.FromNative
				),
			};
		}

		internal static PossibleValueSet TakeNative(BNPossibleValueSet native)
		{
			PossibleValueSet valueSet = PossibleValueSet.FromNative(native);
			
			NativeMethods.BNFreePossibleValueSet(native);

			return valueSet;
		}

		public BNPossibleValueSet ToNativeEx(ScopedAllocator allocator)
		{
			return new BNPossibleValueSet()
			{
				state = this.State ,
				value = this.Value ,
				offset = this.Offset ,
				size = this.Size ,
				ranges = (
					0 == this.Ranges.Length
						? IntPtr.Zero
						: allocator.AllocStructArray<BNValueRange>(
							UnsafeUtils.ConvertToNativeArray<BNValueRange , ValueRange>(this.Ranges)
						)
				) ,
				valueSet = (
					0 == this.ValueSet.Length ? IntPtr.Zero : allocator.AllocIntegerArray(this.ValueSet)
				) ,
				table = (
					0 == this.Table.Length
						? IntPtr.Zero
						: allocator.AllocStructArray<BNLookupTableEntry>(
							allocator.ConvertToNativeArrayEx<BNLookupTableEntry , LookupTableEntry>(this.Table)
						)
				) ,
				count = ( 0 != this.Ranges.Length 
						? (ulong)this.Ranges.Length
						: (  0 != this.ValueSet.Length ? 
							(ulong)this.ValueSet.Length : (ulong)this.Table.Length)
					),
			};
		}

		private PossibleValueSet ApplyBinary(
			PossibleValueSet other,
			ulong size,
			BinaryOperation operation
		)
		{
			if (null == other)
			{
				throw new ArgumentNullException(nameof(other));
			}

			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr left = allocator.AllocStruct<BNPossibleValueSet>(this.ToNativeEx(allocator));
				IntPtr right = allocator.AllocStruct<BNPossibleValueSet>(other.ToNativeEx(allocator));
				BNPossibleValueSet result = operation(left, right, (UIntPtr)size);
				return PossibleValueSet.TakeNative(result);
			}
		}

		private PossibleValueSet ApplyUnary(ulong size, UnaryOperation operation)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr value = allocator.AllocStruct<BNPossibleValueSet>(this.ToNativeEx(allocator));
				BNPossibleValueSet result = operation(value, (UIntPtr)size);
				return PossibleValueSet.TakeNative(result);
			}
		}

		/// <summary>Computes the union with another possible value set.</summary>
		public PossibleValueSet Union(PossibleValueSet other, ulong size)
		{
			return this.ApplyBinary(other, size, NativeMethods.BNPossibleValueSetUnion);
		}

		/// <summary>Computes the intersection with another possible value set.</summary>
		public PossibleValueSet Intersection(PossibleValueSet other, ulong size)
		{
			return this.ApplyBinary(other, size, NativeMethods.BNPossibleValueSetIntersection);
		}

		/// <summary>Adds another possible value set.</summary>
		public PossibleValueSet Add(PossibleValueSet other, ulong size)
		{
			return this.ApplyBinary(other, size, NativeMethods.BNPossibleValueSetAdd);
		}

		/// <summary>Subtracts another possible value set.</summary>
		public PossibleValueSet Subtract(PossibleValueSet other, ulong size)
		{
			return this.ApplyBinary(other, size, NativeMethods.BNPossibleValueSetSubtract);
		}

		/// <summary>Multiplies by another possible value set.</summary>
		public PossibleValueSet Multiply(PossibleValueSet other, ulong size)
		{
			return this.ApplyBinary(other, size, NativeMethods.BNPossibleValueSetMultiply);
		}

		/// <summary>Performs signed division by another possible value set.</summary>
		public PossibleValueSet SignedDivide(PossibleValueSet other, ulong size)
		{
			return this.ApplyBinary(other, size, NativeMethods.BNPossibleValueSetSignedDivide);
		}

		/// <summary>Performs unsigned division by another possible value set.</summary>
		public PossibleValueSet UnsignedDivide(PossibleValueSet other, ulong size)
		{
			return this.ApplyBinary(other, size, NativeMethods.BNPossibleValueSetUnsignedDivide);
		}

		/// <summary>Performs signed modulo with another possible value set.</summary>
		public PossibleValueSet SignedMod(PossibleValueSet other, ulong size)
		{
			return this.ApplyBinary(other, size, NativeMethods.BNPossibleValueSetSignedMod);
		}

		/// <summary>Performs unsigned modulo with another possible value set.</summary>
		public PossibleValueSet UnsignedMod(PossibleValueSet other, ulong size)
		{
			return this.ApplyBinary(other, size, NativeMethods.BNPossibleValueSetUnsignedMod);
		}

		/// <summary>Performs bitwise AND with another possible value set.</summary>
		public PossibleValueSet And(PossibleValueSet other, ulong size)
		{
			return this.ApplyBinary(other, size, NativeMethods.BNPossibleValueSetAnd);
		}

		/// <summary>Performs bitwise OR with another possible value set.</summary>
		public PossibleValueSet Or(PossibleValueSet other, ulong size)
		{
			return this.ApplyBinary(other, size, NativeMethods.BNPossibleValueSetOr);
		}

		/// <summary>Performs bitwise XOR with another possible value set.</summary>
		public PossibleValueSet Xor(PossibleValueSet other, ulong size)
		{
			return this.ApplyBinary(other, size, NativeMethods.BNPossibleValueSetXor);
		}

		/// <summary>Shifts left by another possible value set.</summary>
		public PossibleValueSet ShiftLeft(PossibleValueSet other, ulong size)
		{
			return this.ApplyBinary(other, size, NativeMethods.BNPossibleValueSetShiftLeft);
		}

		/// <summary>Performs a logical right shift by another possible value set.</summary>
		public PossibleValueSet LogicalShiftRight(PossibleValueSet other, ulong size)
		{
			return this.ApplyBinary(other, size, NativeMethods.BNPossibleValueSetLogicalShiftRight);
		}

		/// <summary>Performs an arithmetic right shift by another possible value set.</summary>
		public PossibleValueSet ArithShiftRight(PossibleValueSet other, ulong size)
		{
			return this.ApplyBinary(other, size, NativeMethods.BNPossibleValueSetArithShiftRight);
		}

		/// <summary>Rotates left by another possible value set.</summary>
		public PossibleValueSet RotateLeft(PossibleValueSet other, ulong size)
		{
			return this.ApplyBinary(other, size, NativeMethods.BNPossibleValueSetRotateLeft);
		}

		/// <summary>Rotates right by another possible value set.</summary>
		public PossibleValueSet RotateRight(PossibleValueSet other, ulong size)
		{
			return this.ApplyBinary(other, size, NativeMethods.BNPossibleValueSetRotateRight);
		}

		/// <summary>Negates this possible value set.</summary>
		public PossibleValueSet Negate(ulong size)
		{
			return this.ApplyUnary(size, NativeMethods.BNPossibleValueSetNegate);
		}

		/// <summary>Performs bitwise NOT on this possible value set.</summary>
		public PossibleValueSet Not(ulong size)
		{
			return this.ApplyUnary(size, NativeMethods.BNPossibleValueSetNot);
		}
    }
}
