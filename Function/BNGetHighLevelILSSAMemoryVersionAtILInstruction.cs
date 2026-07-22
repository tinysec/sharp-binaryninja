using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class NativeMethods
	{
		[DllImport(
			"binaryninjacore",
			CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
			EntryPoint = "BNGetHighLevelILSSAMemoryVersionAtILInstruction")]
		internal static extern ulong BNGetHighLevelILSSAMemoryVersionAtILInstruction(
			IntPtr function,
			HighLevelILInstructionIndex instruction);
	}
}
