using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNTokenEmitterExpr 
	{
		/// <summary>
		/// uint64_t address
		/// </summary>
		internal ulong address;
		
		/// <summary>
		/// uint32_t sourceOperand
		/// </summary>
		internal uint sourceOperand;
		
		/// <summary>
		/// uint64_t exprIndex
		/// </summary>
		internal ulong exprIndex;
		
		/// <summary>
		/// uint64_t instrIndex
		/// </summary>
		internal ulong instrIndex;
	}

    public sealed class TokenEmitterExpression
    {
		public ulong Address { get; set; } = 0;

		public uint SourceOperand { get; set; } = 0;
		
		public HighLevelILExpressionIndex ExpressionIndex { get; set; } = 0;
		
		public HighLevelILInstructionIndex InstructionIndex { get; set; } = 0;
		
		public TokenEmitterExpression() 
		{
		    
		}

		public BNTokenEmitterExpr ToNative()
		{
			return new BNTokenEmitterExpr()
			{
				address = this.Address ,
				sourceOperand = this.SourceOperand ,
				exprIndex = (ulong)this.ExpressionIndex ,
				instrIndex = (ulong)this.InstructionIndex ,
			};
		}

		internal static TokenEmitterExpression FromNative(
			BNTokenEmitterExpr native
		)
		{
			TokenEmitterExpression expression = new TokenEmitterExpression();
			expression.Address = native.address;
			expression.SourceOperand = native.sourceOperand;
			expression.ExpressionIndex =
				(HighLevelILExpressionIndex)native.exprIndex;
			expression.InstructionIndex =
				(HighLevelILInstructionIndex)native.instrIndex;

			return expression;
		}

		internal static TokenEmitterExpression FromInstruction(
			HighLevelILInstruction instruction
		)
		{
			if (null == instruction)
			{
				throw new ArgumentNullException(nameof(instruction));
			}

			TokenEmitterExpression expression = new TokenEmitterExpression();
			expression.Address = instruction.Address;
			expression.SourceOperand = (uint)instruction.SourceOperand;
			expression.ExpressionIndex = instruction.ExpressionIndex;
			expression.InstructionIndex = instruction.InstructionIndex;

			return expression;
		}
    }
}
