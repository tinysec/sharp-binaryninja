using System;

namespace BinaryNinja
{
	public sealed class RegisterStack : IEquatable<RegisterStack>,
		IComparable<RegisterStack>
	{
		internal Architecture Architecture { get; }

		public RegisterStackIndex Index { get; }

		public RegisterStack(Architecture arch , RegisterStackIndex index)
		{
			this.Architecture = arch;
			this.Index = index;
		}
		
		internal RegisterStack(RegisterStack other)
		{
			this.Architecture = other.Architecture;
			this.Index = other.Index;
		}
		
		public override bool Equals(object? other)
		{
			return Equals(other as RegisterStack);
		}

		public bool Equals(RegisterStack? other)
		{
			if (other is null)
			{
				return false;
			}

			if (ReferenceEquals(this , other))
			{
				return true;
			}

			if (this.Architecture != other.Architecture)
			{
				return false;
			}
			
			return this.Index == other.Index;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine<int,uint>(this.Architecture.GetHashCode(), (uint)this.Index);
		}

		public static bool operator ==(RegisterStack? left, RegisterStack? right)
		{
			if (left is null)
			{
				return right is null;
			}
			
			return left.Equals(right);
		}

		public static bool operator !=(RegisterStack? left, RegisterStack? right)
		{
			return !(left == right);
		}

		public int CompareTo(RegisterStack? other)
		{
			if (other is null)
			{
				return 1;
			}
			
			int result = this.Architecture.CompareTo(other.Architecture);

			if (0 == result)
			{
				result = this.Index.CompareTo(other.Index);
			}
			
			return result;
		}
		
		
		public override string ToString()
		{
			return this.Name;
		}
		
		public string Name
		{
			get
			{
				return this.Architecture.GetRegisterStackName(this.Index);
			}
		}
	}
}
