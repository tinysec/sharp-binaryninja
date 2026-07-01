using System;

namespace BinaryNinja
{
	public class ILFlag : IEquatable<ILFlag>, IComparable<ILFlag>
	{
		internal Architecture Architecture { get; }

		public FlagIndex Index { get; }

		internal ILFlag(Architecture arch , FlagIndex index)
		{
			this.Architecture = arch;
			this.Index = index;
		}
		
		internal ILFlag(ILFlag other)
		{
			this.Architecture = other.Architecture;
			this.Index = other.Index;
		}
		
		public override string ToString()
		{
			return this.Name;
		}
		
		public bool IsTemp()
		{
			return 0 != ( (uint)this.Index & 0x80000000 );
		}

		public string Name
		{
			get
			{
				if (this.IsTemp())
				{
					return $"cond:{ (uint)this.Index & 0x7fffffff}";
				}

				return this.Architecture.GetFlagName(this.Index);
			}
		}
		
		public override bool Equals(object? other)
		{
			return Equals(other as ILFlag);
		}

		public bool Equals(ILFlag? other)
		{
			if (other is null)
			{
				return false;
			}

			if (ReferenceEquals(this , other))
			{
				return true;
			}
			
			return this.Index == other.Index;
		}

		public override int GetHashCode()
		{
			return this.Index.GetHashCode();
		}

		public static bool operator ==(ILFlag? left, ILFlag? right)
		{
			if (left is null)
			{
				return right is null;
			}
			
			return left.Equals(right);
		}

		public static bool operator !=(ILFlag? left, ILFlag? right)
		{
			return !(left == right);
		}

		public int CompareTo(ILFlag? other)
		{
			if (other is null)
			{
				return 1;
			}
			
			return this.Index.CompareTo(other.Index);
		}
	}
}
