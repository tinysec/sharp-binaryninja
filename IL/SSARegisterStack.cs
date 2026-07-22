using System;

namespace BinaryNinja
{
	public sealed class SSARegisterStack :  IEquatable<SSARegisterStack>,
		IComparable<SSARegisterStack>
	{
		public RegisterStack RegisterStack { get; }
		
		public ulong Version { get;  } = 0;

		public SSARegisterStack(RegisterStack registerStack , ulong version)
		{
			this.RegisterStack = registerStack;
			this.Version = version;
		}
		
		public override string ToString()
		{
			return $"{this.RegisterStack.Name}#{this.Version}";
		}
		
		public override bool Equals(object? other)
		{
			return Equals(other as SSARegisterStack);
		}

		public bool Equals(SSARegisterStack? other)
		{
			if (other is null)
			{
				return false;
			}

			if (ReferenceEquals(this , other))
			{
				return true;
			}

			if (this.RegisterStack != other.RegisterStack)
			{
				return false;
			}
			
			return this.Version == other.Version;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine<int,ulong>(this.RegisterStack.GetHashCode(), this.Version);
		}

		public static bool operator ==(SSARegisterStack? left, SSARegisterStack? right)
		{
			if (left is null)
			{
				return right is null;
			}
			
			return left.Equals(right);
		}

		public static bool operator !=(SSARegisterStack? left, SSARegisterStack? right)
		{
			return !(left == right);
		}

		public int CompareTo(SSARegisterStack? other)
		{
			if (other is null)
			{
				return 1;
			}
			
			int result = this.RegisterStack.CompareTo(other.RegisterStack);

			if (result == 0)
			{
				result = this.Version.CompareTo(other.Version);
			}
			
			return result;
		}
	}
}
