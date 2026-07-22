using System;

namespace BinaryNinja
{
	/// <summary>
	/// Represents an inclusive Unicode code point range.
	/// </summary>
	public readonly struct UnicodeRange : IEquatable<UnicodeRange>
	{
		public UnicodeRange(uint start, uint end)
		{
			if (start > end)
			{
				throw new ArgumentOutOfRangeException(nameof(end), "The range end must not precede the start.");
			}

			this.Start = start;
			this.End = end;
		}

		/// <summary>
		/// Gets the first code point in the range.
		/// </summary>
		public uint Start { get; }

		/// <summary>
		/// Gets the last code point in the range.
		/// </summary>
		public uint End { get; }

		public bool Equals(UnicodeRange other)
		{
			return this.Start == other.Start && this.End == other.End;
		}

		public override bool Equals(object? other)
		{
			return other is UnicodeRange && this.Equals((UnicodeRange)other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(this.Start, this.End);
		}

		public static bool operator ==(UnicodeRange left, UnicodeRange right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(UnicodeRange left, UnicodeRange right)
		{
			return !left.Equals(right);
		}
	}
}
