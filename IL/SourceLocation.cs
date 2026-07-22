using System;

namespace BinaryNinja
{
	public sealed class SourceLocation : IEquatable<SourceLocation>,
		IComparable<SourceLocation>
	{
		public ulong Address { get;  } = 0;
		
		public OperandIndex Operand { get; } = 0;

		/// <summary>
		/// Gets the source LLIL instruction when this location was derived from LLIL.
		/// </summary>
		public LowLevelILInstruction? SourceLowLevelILInstruction { get; private set; }

		/// <summary>
		/// Gets the source MLIL instruction when this location was derived from MLIL.
		/// </summary>
		public MediumLevelILInstruction? SourceMediumLevelILInstruction { get; private set; }

		/// <summary>
		/// Gets the source HLIL instruction when this location was derived from HLIL.
		/// </summary>
		public HighLevelILInstruction? SourceHighLevelILInstruction { get; private set; }

		/// <summary>
		/// Gets whether the source IL instruction maps directly to the generated expression.
		/// </summary>
		public bool ILDirect { get; private set; } = true;

		public SourceLocation()
		{
			
		}
		
		public SourceLocation(ulong address, OperandIndex operand)
		{
			this.Address = address;
			this.Operand = operand;
		}

		private SourceLocation(
			ulong address,
			OperandIndex operand,
			bool ilDirect)
			: this(address, operand)
		{
			this.ILDirect = ilDirect;
		}

		/// <summary>
		/// Creates a source location derived from an LLIL instruction.
		/// </summary>
		public static SourceLocation FromInstruction(
			LowLevelILInstruction instruction,
			bool ilDirect = true)
		{
			if (null == instruction)
			{
				throw new ArgumentNullException(nameof(instruction));
			}

			SourceLocation result = new SourceLocation(
				instruction.Address,
				instruction.SourceOperand,
				ilDirect);
			result.SourceLowLevelILInstruction = instruction;

			return result;
		}

		/// <summary>
		/// Creates a source location derived from an MLIL instruction.
		/// </summary>
		public static SourceLocation FromInstruction(
			MediumLevelILInstruction instruction,
			bool ilDirect = true)
		{
			if (null == instruction)
			{
				throw new ArgumentNullException(nameof(instruction));
			}

			SourceLocation result = new SourceLocation(
				instruction.Address,
				instruction.SourceOperand,
				ilDirect);
			result.SourceMediumLevelILInstruction = instruction;

			return result;
		}

		/// <summary>
		/// Creates a source location derived from an HLIL instruction.
		/// </summary>
		public static SourceLocation FromInstruction(
			HighLevelILInstruction instruction,
			bool ilDirect = true)
		{
			if (null == instruction)
			{
				throw new ArgumentNullException(nameof(instruction));
			}

			SourceLocation result = new SourceLocation(
				instruction.Address,
				instruction.SourceOperand,
				ilDirect);
			result.SourceHighLevelILInstruction = instruction;

			return result;
		}
		
		public override bool Equals(object? other)
		{
			return Equals(other as SourceLocation);
		}

		public bool Equals(SourceLocation? other)
		{
			if (other is null)
			{
				return false;
			}

			if (ReferenceEquals(this , other))
			{
				return true;
			}

			if (this.Address != other.Address)
			{
				return false;
			}
			
			return this.Operand == other.Operand;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine<ulong,ulong>(this.Address, (ulong)this.Operand);
		}

		public static bool operator ==(SourceLocation? left, SourceLocation? right)
		{
			if (left is null)
			{
				return right is null;
			}
			
			return left.Equals(right);
		}

		public static bool operator !=(SourceLocation? left, SourceLocation? right)
		{
			return !(left == right);
		}

		public int CompareTo(SourceLocation? other)
		{
			if (other is null)
			{
				return 1;
			}
			
			int result = this.Address.CompareTo(other.Address);

			if (result == 0)
			{
				result = this.Operand.CompareTo(other.Operand);
			}
			
			return result;
		}
	}
}
