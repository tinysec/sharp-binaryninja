using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNVariable 
	{
		/// <summary>
		/// BNVariableSourceType type
		/// </summary>
		internal VariableSourceType type;
		
		/// <summary>
		/// uint32_t index
		/// </summary>
		internal uint index;
		
		/// <summary>
		/// int64_t storage
		/// </summary>
		internal long storage;
	}
	
     public abstract class AbstractVariable<T_SELF> : INativeWrapper<BNVariable>,
	    IEquatable<T_SELF>,
	    IComparable<T_SELF>
     where T_SELF : AbstractVariable<T_SELF>
    {
		public VariableSourceType Type { get; internal set; } = VariableSourceType.StackVariableSourceType;
		
		public uint Index { get; internal set; } = 0;
		
		public long Storage { get; internal set; } = 0;

		internal AbstractVariable()
		{
			
		}
		
		internal AbstractVariable(
			VariableSourceType type,
			uint index,
			long storage
		) 
		{
			this.Type = type;
			this.Index = index;
			this.Storage = storage;
		}
		
		internal AbstractVariable(BNVariable native)
		{
			this.Type = native.type;
			this.Index = native.index;
			this.Storage = native.storage;
		}
		
		public BNVariable ToNative()
		{
			return new BNVariable()
			{
				type = this.Type ,
				index = this.Index ,
				storage = this.Storage
			};
		}

		public ulong Identifier
		{
			get
			{
				return NativeMethods.BNToVariableIdentifier(this.ToNative());
			}
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
			
			return this.Identifier == other.Identifier;
		}

		public override int GetHashCode()
		{
			return this.Identifier.GetHashCode();
		}

		public static bool operator ==(AbstractVariable<T_SELF>? left, AbstractVariable<T_SELF>? right)
		{
			if (left is null)
			{
				return right is null;
			}
			
			return left.Equals(right);
		}

		public static bool operator !=(AbstractVariable<T_SELF>? left, AbstractVariable<T_SELF>? right)
		{
			return !(left == right);
		}

		public int CompareTo(T_SELF? other)
		{
			if (other is null)
			{
				return 1;
			}
			
			return this.Identifier.CompareTo(other.Identifier);
		}
    }
     
    public sealed class CoreVariable : AbstractVariable<CoreVariable>
    {
		internal CoreVariable()
		{
			
		}
		
		internal CoreVariable(
			VariableSourceType type,
			uint index,
			long storage
		) : base(type, index, storage)
		{
			
		}
		
		internal CoreVariable(BNVariable native)
			:base(native)
		{
			
		}
		
		internal static CoreVariable FromIdentifier(ulong identifier)
		{
			return new CoreVariable(
				NativeMethods.BNFromVariableIdentifier(identifier)
			);
		}
		
		internal static CoreVariable FromNative(BNVariable native)
		{
			return new CoreVariable(native);
		}
    }

}