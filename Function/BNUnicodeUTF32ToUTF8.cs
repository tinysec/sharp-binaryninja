using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class NativeMethods
	{
		/// <summary>
		/// char* BNUnicodeUTF32ToUTF8(const uint8_t* utf32)
		/// </summary>
		[DllImport(
			"binaryninjacore",
			CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
			EntryPoint = "BNUnicodeUTF32ToUTF8")]
		internal static extern IntPtr BNUnicodeUTF32ToUTF8(IntPtr utf32);
	}
}
