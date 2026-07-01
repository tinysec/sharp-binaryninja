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
	
		public InstructionTextLine() 
		{
			
		}
		
		public InstructionTextLine(InstructionTextToken[] tokens) 
		{
		    this.Tokens = tokens;
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