using System;

namespace BinaryNinja
{
	public sealed class ILRegister :  IEquatable<ILRegister>,
		IComparable<ILRegister>
	{
		internal Architecture Architecture;

		public RegisterIndex Index { get; }

		public ILRegister(Architecture arch , RegisterIndex index)
		{
			this.Architecture = arch;
			this.Index = index;
		}
		
		internal ILRegister(ILRegister other)
		{
			this.Architecture = other.Architecture;
			this.Index = other.Index;
		}
		
		public override string ToString()
		{
			return this.Name;
		}

		public bool IsTemp
		{
			get
			{
				return 0 != ( (uint)this.Index & 0x80000000 );
			}
		}
		
		public bool IsGlobal
		{
			get
			{
				return NativeMethods.BNIsArchitectureGlobalRegister(
					this.Architecture.DangerousGetHandle() , 
					this.Index
				);
			}
		}

		public bool IsSystem
		{
			get
			{
				return NativeMethods.BNIsArchitectureSystemRegister(
					this.Architecture.DangerousGetHandle() , 
					this.Index
				);
			}
		}
		
		public string Name
		{
			get
			{
				if (this.IsTemp)
				{
					return $"temp{ (uint)this.Index & 0x7fffffff}";
				}

				return this.Architecture.GetRegisterName(this.Index);
			}
		}
		
		
		public RegisterInfo Info
		{
			get
			{
				return this.Architecture.GetRegisterInfo(this.Index);
			}
		}
		
		
		public override bool Equals(object? other)
		{
			return Equals(other as ILRegister);
		}

		public bool Equals(ILRegister? other)
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

		public static bool operator ==(ILRegister? left, ILRegister? right)
		{
			if (left is null)
			{
				return right is null;
			}
			
			return left.Equals(right);
		}

		public static bool operator !=(ILRegister? left, ILRegister? right)
		{
			return !(left == right);
		}

		public int CompareTo(ILRegister? other)
		{
			if (other is null)
			{
				return 1;
			}
			
			return this.Index.CompareTo(other.Index);
		}
	}
}
