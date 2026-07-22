using System;

#pragma warning disable CS0618 // Legacy visitor recursion intentionally uses compatibility APIs.

namespace BinaryNinja
{
	/// <summary>
	/// Visits a named MLIL instruction or leaf operand.
	/// </summary>
	public delegate bool MediumLevelILVisitorCallback(
		string name,
		object? value,
		string typeName,
		MediumLevelILInstruction? parent);

	public abstract partial class MediumLevelILInstruction
	{
		/// <summary>
		/// Visits this instruction, its child instructions, and all leaf operands in pre-order.
		/// </summary>
		[Obsolete("Use Traverse instead.")]
		public bool VisitAll(
			MediumLevelILVisitorCallback callback,
			string name = "root",
			MediumLevelILInstruction? parent = null)
		{
			if (null == callback)
			{
				throw new ArgumentNullException(nameof(callback));
			}

			if (false == callback(name, this, "MediumLevelILInstruction", parent))
			{
				return false;
			}

			foreach (ILOperand operand in this.DetailedOperands)
			{
				MediumLevelILInstruction? child = operand.Value as MediumLevelILInstruction;
				if (null != child)
				{
					if (false == child.VisitAll(callback, operand.Name, this))
					{
						return false;
					}

					continue;
				}

				MediumLevelILInstruction[]? children =
					operand.Value as MediumLevelILInstruction[];
				if (null != children)
				{
					foreach (MediumLevelILInstruction item in children)
					{
						if (false == item.VisitAll(callback, operand.Name, this))
						{
							return false;
						}
					}

					continue;
				}

				if (false == callback(operand.Name, operand.Value, operand.TypeName, this))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Visits only leaf operands in this instruction tree.
		/// </summary>
		[Obsolete("Use Traverse instead.")]
		public bool VisitOperands(
			MediumLevelILVisitorCallback callback,
			string name = "root",
			MediumLevelILInstruction? parent = null)
		{
			if (null == callback)
			{
				throw new ArgumentNullException(nameof(callback));
			}

			foreach (ILOperand operand in this.DetailedOperands)
			{
				MediumLevelILInstruction? child = operand.Value as MediumLevelILInstruction;
				if (null != child)
				{
					if (false == child.VisitOperands(callback, operand.Name, this))
					{
						return false;
					}

					continue;
				}

				MediumLevelILInstruction[]? children =
					operand.Value as MediumLevelILInstruction[];
				if (null != children)
				{
					foreach (MediumLevelILInstruction item in children)
					{
						if (false == item.VisitOperands(callback, operand.Name, this))
						{
							return false;
						}
					}

					continue;
				}

				if (false == callback(operand.Name, operand.Value, operand.TypeName, this))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Visits only MLIL instruction nodes in this instruction tree.
		/// </summary>
		[Obsolete("Use Traverse instead.")]
		public bool Visit(
			MediumLevelILVisitorCallback callback,
			string name = "root",
			MediumLevelILInstruction? parent = null)
		{
			if (null == callback)
			{
				throw new ArgumentNullException(nameof(callback));
			}

			if (false == callback(name, this, "MediumLevelILInstruction", parent))
			{
				return false;
			}

			foreach (ILOperand operand in this.DetailedOperands)
			{
				MediumLevelILInstruction? child = operand.Value as MediumLevelILInstruction;
				if (null != child)
				{
					if (false == child.Visit(callback, operand.Name, this))
					{
						return false;
					}

					continue;
				}

				MediumLevelILInstruction[]? children =
					operand.Value as MediumLevelILInstruction[];
				if (null == children)
				{
					continue;
				}

				foreach (MediumLevelILInstruction item in children)
				{
					if (false == item.Visit(callback, operand.Name, this))
					{
						return false;
					}
				}
			}

			return true;
		}
	}

	public sealed partial class MediumLevelILFunction
	{
		/// <summary>
		/// Visits all instructions and operands in this function.
		/// </summary>
		[Obsolete("Use Traverse instead.")]
		public bool VisitAll(MediumLevelILVisitorCallback callback)
		{
			if (null == callback)
			{
				throw new ArgumentNullException(nameof(callback));
			}

			foreach (MediumLevelILInstruction instruction in this.Instructions)
			{
				if (false == instruction.VisitAll(callback))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Visits all leaf operands in this function.
		/// </summary>
		[Obsolete("Use Traverse instead.")]
		public bool VisitOperands(MediumLevelILVisitorCallback callback)
		{
			if (null == callback)
			{
				throw new ArgumentNullException(nameof(callback));
			}

			foreach (MediumLevelILInstruction instruction in this.Instructions)
			{
				if (false == instruction.VisitOperands(callback))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Visits all MLIL instruction nodes in this function.
		/// </summary>
		[Obsolete("Use Traverse instead.")]
		public bool Visit(MediumLevelILVisitorCallback callback)
		{
			if (null == callback)
			{
				throw new ArgumentNullException(nameof(callback));
			}

			foreach (MediumLevelILInstruction instruction in this.Instructions)
			{
				if (false == instruction.Visit(callback))
				{
					return false;
				}
			}

			return true;
		}
	}
}

#pragma warning restore CS0618
