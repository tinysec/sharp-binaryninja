using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class NativeMethods
	{
		[DllImport(
			"binaryninjacore",
			CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
			EntryPoint = "BNGetHighLevelILSSAVarVersionAtILInstruction")]
		internal static extern ulong BNGetHighLevelILSSAVarVersionAtILInstruction(
			IntPtr function,
			in BNVariable variable,
			HighLevelILInstructionIndex instruction);
	}
}
