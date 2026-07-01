using System;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public abstract class AbstractSafeHandle<T_SELF> : SafeHandleZeroOrMinusOneIsInvalid , 
		IEquatable<T_SELF>, 
		IComparable<T_SELF>
		where T_SELF : AbstractSafeHandle<T_SELF>
	{
		internal AbstractSafeHandle(bool owner) : base(owner)
		{
			
		}
		
		internal AbstractSafeHandle(IntPtr handle , bool owner) : base(owner)
		{
			this.SetHandle(handle);
		}
		
		public override bool Equals(object? other)
		{
			T_SELF? otherNative = other as T_SELF;
			
			if (otherNative is null)
			{
				return false;
			}

			if (ReferenceEquals(this , otherNative))
			{
				return true;
			}
			
			return this.handle == otherNative.handle;
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
			
			return this.handle == other.handle;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2218:OverrideGetHashCodeOnOverridingEquals")]
		public override int GetHashCode()
		{
			return HashCode.Combine<IntPtr>( (IntPtr)this.handle);
		}

		public static bool operator ==(AbstractSafeHandle<T_SELF>? left, AbstractSafeHandle<T_SELF>? right)
		{
			if (left is null)
			{
				return right is null;
			}
			
			return left.Equals(right);
		}

		public static bool operator !=(AbstractSafeHandle<T_SELF>? left, AbstractSafeHandle<T_SELF>? right)
		{
			return !(left == right);
		}

		public int CompareTo(T_SELF? other)
		{
			if (other is null)
			{
				return 1;
			}
			
			return this.handle.CompareTo(other.handle);
		}
		
		protected override bool ReleaseHandle()
		{
			if ( !this.IsInvalid )
			{
				this.SetHandleAsInvalid();
			}
	        
			return true;
		}
	}
}
