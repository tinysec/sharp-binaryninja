using System;

namespace BinaryNinja
{
	public sealed class FlagOrRegister : IEquatable<FlagOrRegister>,
		IComparable<FlagOrRegister>
	{
		public ILFlag? Flag { get;  } = null;
		
		public ILRegister? Register { get;  } = null;
		
		internal FlagOrRegister(ILFlag flag)
		{
			this.Flag = flag;
		}
		
		internal FlagOrRegister(ILRegister register)
		{
			this.Register = register;
		}
		
		public FlagOrRegister(FlagOrRegister other)
		{
			this.Flag = other.Flag;
			this.Register = other.Register;
		}
	
		public string Name
		{
			get
			{
				if (this.Flag != null)
				{
					return this.Flag.Name;
				}
			
				if (null != this.Register)
				{
					return this.Register.Name;
				}
			
				throw new Exception("Flag or Register must not both null");
			}
		}
		
		public override string ToString()
		{
			if (null != this.Flag)
			{
				return this.Flag.Name;
			}
			
			if (null != this.Register)
			{
				return this.Register.Name;
			}
			
			throw new Exception("Flag or Register must not both null");
		}
		
		public override bool Equals(object? other)
		{
			return Equals(other as FlagOrRegister);
		}

		public bool Equals(FlagOrRegister? other)
		{
			if (other is null)
			{
				return false;
			}

			if (ReferenceEquals(this , other))
			{
				return true;
			}

			if (null != this.Flag && null != other.Flag)
			{
				return this.Flag == other.Flag;
			}
			
			if (null != this.Register && null != other.Register)
			{
				return this.Register == other.Register;
			}
			
			throw new Exception("Flag or Register must not both null");
		}

		public override int GetHashCode()
		{
			if (null != this.Flag)
			{
				return this.Flag.GetHashCode();
			}

			if (null != this.Register)
			{
				return this.Register.GetHashCode();
			}
			
			throw new Exception("Flag or Register must not both null");
		}

		public static bool operator ==(FlagOrRegister? left, FlagOrRegister? right)
		{
			if (left is null)
			{
				return right is null;
			}
			
			return left.Equals(right);
		}

		public static bool operator !=(FlagOrRegister? left, FlagOrRegister? right)
		{
			return !(left == right);
		}

		public int CompareTo(FlagOrRegister? other)
		{
			if (other is null)
			{
				return 1;
			}

			if (null != this.Flag && null != other.Flag)
			{
				return this.Flag.CompareTo(other.Flag);
			}

			if (null != this.Register && null != other.Register)
			{
				return this.Register.CompareTo(other.Register);
			}
			
			throw new Exception("Flag or Register must not both null");
		}
	}
}
