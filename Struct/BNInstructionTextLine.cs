using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNInstructionTextLine 
	{
		/// <summary>
		/// BNInstructionTextToken* tokens
		/// </summary>
		public IntPtr tokens;
		
		/// <summary>
		/// uint64_t count
		/// </summary>
		public ulong count;
	}

    public sealed class InstructionTextLine
    {
		public InstructionTextToken[] Tokens { get; } = Array.Empty<InstructionTextToken>();

		// The virtual address of the first byte of the instruction this line disassembles. Python's
		// Function.instructions / BinaryView.instructions generators yield (tokens, addr) per
		// instruction; this field carries that address so consumers do not lose it. It is 0 when the
		// line was not produced by an address-walking generator (e.g. synthesized token lists).
		public ulong Address { get; } = 0;

		public InstructionTextLine()
		{

		}

		public InstructionTextLine(InstructionTextToken[] tokens)
		{
			this.Tokens = tokens;
		}

		public InstructionTextLine(InstructionTextToken[] tokens, ulong address)
		{
			this.Tokens = tokens;
			this.Address = address;
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			foreach (InstructionTextToken token in Tokens)
			{
				builder.Append(token.Text);
			}

			return builder.ToString();
		}
    }
}