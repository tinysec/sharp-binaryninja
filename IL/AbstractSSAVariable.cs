using System;

namespace BinaryNinja
{
	public abstract class AbstractSSAVariable<T_VARIABLE>  
		: IEquatable< AbstractSSAVariable<T_VARIABLE>>, 
			IComparable< AbstractSSAVariable<T_VARIABLE>>
		where T_VARIABLE : AbstractFunctionVariable<T_VARIABLE>
	{
		public T_VARIABLE Variable { get; }
		
		public ulong Version { get;  } = 0;

		internal AbstractSSAVariable(T_VARIABLE variable , ulong version)
		{
			this.Variable = variable;
			this.Version = version;
		}
		
		public override string ToString()
		{
			return $"{this.Variable.Name}#{this.Version}";
		}
		
		public override bool Equals(object? other)
		{
			return Equals(other as AbstractSSAVariable<T_VARIABLE> );
		}

		public bool Equals(AbstractSSAVariable<T_VARIABLE> ? other)
		{
			if (other is null)
			{
				return false;
			}

			if (ReferenceEquals(this , other))
			{
				return true;
			}

			if (this.Variable.Identifier != other.Variable.Identifier)
			{
				return false;
			}
			
			return this.Version == other.Version;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine<ulong,ulong>(this.Variable.Identifier, this.Version);
		}

		public static bool operator ==(AbstractSSAVariable<T_VARIABLE> ? left, AbstractSSAVariable<T_VARIABLE> ? right)
		{
			if (left is null)
			{
				return right is null;
			}
			
			return left.Equals(right);
		}

		public static bool operator !=(AbstractSSAVariable<T_VARIABLE> ? left, AbstractSSAVariable<T_VARIABLE> ? right)
		{
			return !(left == right);
		}

		public int CompareTo(AbstractSSAVariable<T_VARIABLE> ? other)
		{
			if (other is null)
			{
				return 1;
			}
			
			int result = this.Variable.Identifier.CompareTo(other.Variable.Identifier);

			if (result == 0)
			{
				result = this.Version.CompareTo(other.Version);
			}
			
			return result;
		}
	}
}
