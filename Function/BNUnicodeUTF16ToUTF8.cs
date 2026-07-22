using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class NativeMethods
	{
		/// <summary>
		/// char* BNUnicodeUTF16ToUTF8(const uint8_t* utf16, size_t len)
		/// </summary>
		[DllImport(
			"binaryninjacore",
			CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
			EntryPoint = "BNUnicodeUTF16ToUTF8")]
		internal static extern IntPtr BNUnicodeUTF16ToUTF8(IntPtr utf16, ulong len);
	}
}
