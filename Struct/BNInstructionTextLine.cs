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

		// The byte length of the instruction this line disassembles. Python's
		// BasicBlock.__getitem__ (basicblock.py:210) yields (tokens, size) per instruction, so the
		// size is as much a part of an instruction's identity as its address. It is 0 when the line
		// was not produced by an address-walking generator (e.g. synthesized token lists); a block's
		// last instruction has no successor, so its size cannot otherwise be recovered from
		// consecutive addresses.
		public ulong ByteSize { get; } = 0;

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

		public InstructionTextLine(InstructionTextToken[] tokens, ulong address, ulong byteSize)
		{
			this.Tokens = tokens;
			this.Address = address;
			this.ByteSize = byteSize;
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