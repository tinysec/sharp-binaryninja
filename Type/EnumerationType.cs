using System;

namespace BinaryNinja
{
	public sealed class EnumerationType : BinaryNinja.Type
	{
		public EnumerationType(BinaryNinja.Type type) 
			: base(BinaryNinja.Type.NewReferenceHandle(type), true)
		{
			
		}
		
		internal EnumerationType(IntPtr handle , bool owner) : base(handle , owner)
		{

		}
		
		public Enumeration Enumeration
		{
			get
			{
				return Enumeration.MustTakeHandle(
					NativeMethods.BNGetTypeEnumeration(this.handle)
				);
			}
		}

		/// <summary>
		/// The members of the underlying enumeration. Convenience forwarder for
		/// <c>Enumeration.Members</c> (Python <c>EnumerationType.members</c>).
		/// </summary>
		public EnumerationMember[] Members
		{
			get
			{
				using (Enumeration enumeration = this.Enumeration)
				{
					return enumeration.Members;
				}
			}
		}

		/// <summary>
		/// The member values reinterpreted per this enumeration's signedness and width, mirroring
		/// Python's <c>EnumerationType.members</c> (which applies
		/// <c>binaryninja.types.convert_integer</c> to each raw u64). A signed enumeration reports
		/// -1 for <c>0xFFFFFFFFFFFFFFFF</c>; <see cref="EnumerationMember.Value"/> alone returns the
		/// raw unsigned value, so signed enumerations previously appeared as huge unsigned numbers.
		/// </summary>
		/// <remarks>
		/// For an unsigned enumeration whose value exceeds <see cref="long.MaxValue"/>, the
		/// authoritative value is <see cref="EnumerationMember.Value"/> (ulong); the long returned
		/// here wraps. Signed enumerations — the case this method exists for — are exact.
		/// </remarks>
		public long[] GetMemberSignedValues()
		{
			EnumerationMember[] members = this.Members;
			BoolWithConfidence signed = this.Signed;
			ulong width = this.Width;

			long[] result = new long[members.Length];

			for (int i = 0; i < members.Length; i++)
			{
				result[i] = ConvertInteger(members[i].Value, signed.Value, width);
			}

			return result;
		}

		// Reinterpret a raw 64-bit value as a signed or unsigned integer of the given byte width,
		// mirroring Python binaryninja.types.convert_integer (types.py:68). Mask to the width first,
		// then sign-extend when signed.
		private static long ConvertInteger(ulong value, bool signed, ulong width)
		{
			ulong masked;

			switch (width)
			{
				case 1:
					masked = value & 0xFFUL;
					if (signed)
					{
						return (long)(sbyte)masked;
					}
					return (long)(byte)masked;

				case 2:
					masked = value & 0xFFFFUL;
					if (signed)
					{
						return (long)(short)masked;
					}
					return (long)(ushort)masked;

				case 4:
					masked = value & 0xFFFFFFFFUL;
					if (signed)
					{
						return (long)(int)masked;
					}
					return (long)(uint)masked;

				default:
					// Width 8 (and any other value): the full 64 bits.
					return (long)value;
			}
		}
	}
}
