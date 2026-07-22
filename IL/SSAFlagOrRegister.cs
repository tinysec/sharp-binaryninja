using System;

namespace BinaryNinja
{
	public sealed class SSAFlagOrRegister  : IEquatable<SSAFlagOrRegister>,
		IComparable<SSAFlagOrRegister>
	{
		public FlagOrRegister FlagOrRegister { get; }
		
		public ulong Version { get; } = 0;

		public SSAFlagOrRegister(FlagOrRegister flagOrRegister , ulong version)
		{
			this.FlagOrRegister = flagOrRegister;
			
			this.Version = version;
		}

		public SSAFlagOrRegister(LowLevelILSSARegister register)
			: this(new FlagOrRegister(register.Register), register.Version)
		{
		}

		public SSAFlagOrRegister(LowLevelILSSAFlag flag)
			: this(new FlagOrRegister(flag.Flag), flag.Version)
		{
		}
		
		public override string ToString()
		{
			return $"{this.FlagOrRegister.Name}#{this.Version}";
		}
		
		public override bool Equals(object? other)
		{
			return Equals(other as SSAFlagOrRegister);
		}

		public bool Equals(SSAFlagOrRegister? other)
		{
			if (other is null)
			{
				return false;
			}

			if (ReferenceEquals(this , other))
			{
				return true;
			}

			if (this.FlagOrRegister != other.FlagOrRegister)
			{
				return false;
			}
			
			return this.Version == other.Version;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine<int,ulong>(this.FlagOrRegister.GetHashCode(), this.Version);
		}

		public static bool operator ==(SSAFlagOrRegister? left, SSAFlagOrRegister? right)
		{
			if (left is null)
			{
				return right is null;
			}
			
			return left.Equals(right);
		}

		public static bool operator !=(SSAFlagOrRegister? left, SSAFlagOrRegister? right)
		{
			return !(left == right);
		}

		public int CompareTo(SSAFlagOrRegister? other)
		{
			if (other is null)
			{
				return 1;
			}
			
			int result = this.FlagOrRegister.CompareTo(other.FlagOrRegister);

			if (result == 0)
			{
				result = this.Version.CompareTo(other.Version);
			}
			
			return result;
		}
	}
}
