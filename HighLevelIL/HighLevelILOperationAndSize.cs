using System;

namespace BinaryNinja
{
	public sealed class HighLevelILOperationAndSize :
		IEquatable<HighLevelILOperationAndSize>
	{
		public HighLevelILOperation Operation { get; }

		public ulong Size { get; }

		public HighLevelILOperationAndSize(
			HighLevelILOperation operation,
			ulong size)
		{
			this.Operation = operation;
			this.Size = size;
		}

		public bool Equals(HighLevelILOperationAndSize? other)
		{
			if (null == other)
			{
				return false;
			}

			return this.Operation == other.Operation && this.Size == other.Size;
		}

		public override bool Equals(object? other)
		{
			return this.Equals(other as HighLevelILOperationAndSize);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(this.Operation, this.Size);
		}

		public override string ToString()
		{
			if (0 == this.Size)
			{
				return this.Operation.ToString();
			}

			return this.Operation + " " + this.Size;
		}
	}
}
