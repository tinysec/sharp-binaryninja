using System;

namespace BinaryNinja
{
	public abstract class AbstractLabel<T_SELF> : IEquatable<T_SELF>, IComparable<T_SELF>
		where T_SELF : AbstractLabel<T_SELF>
	{
		public ulong Id { get; } = 0;

		internal AbstractLabel(ulong id)
		{
			this.Id = id;
		}

		public abstract string Name
		{
			get;

			set;
		}
		
		public override bool Equals(object? other)
		{
			return Equals(other as T_SELF);
		}

		public bool Equals(T_SELF? other)
		{
			if (other is null)
			{
				return false;
			}

			if (ReferenceEquals(this , other))
			{
				return true;
			}
			
			return this.Id == other.Id;
		}

		public override int GetHashCode()
		{
			return this.Id.GetHashCode();
		}

		public static bool operator ==(AbstractLabel<T_SELF>? left, AbstractLabel<T_SELF>? right)
		{
			if (left is null)
			{
				return right is null;
			}
			
			return left.Equals(right);
		}

		public static bool operator !=(AbstractLabel<T_SELF>? left, AbstractLabel<T_SELF>? right)
		{
			return !(left == right);
		}

		public int CompareTo(T_SELF? other)
		{
			if (other is null)
			{
				return 1;
			}
			
			return this.Id.CompareTo(other.Id);
		}
	}
	
	public class ILLabel : AbstractLabel<ILLabel>
	{
		internal Function Function;
		
		internal ILLabel(Function function , ulong id)
			:base(id)
		{
			this.Function = function;
		}

		public override string Name
		{
			get
			{
				return UnsafeUtils.TakeAnsiString(
					NativeMethods.BNGetGotoLabelName(
						this.Function.DangerousGetHandle() ,
						this.Id
					)
				);
			}

			set
			{
				NativeMethods.BNSetUserGotoLabelName(
					this.Function.DangerousGetHandle(),
					this.Id, 
					value
				);
			}
		}
	}
}
