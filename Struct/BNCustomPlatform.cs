using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class NativeDelegates
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNPlatformInit(IntPtr context, IntPtr platform);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNPlatformViewInit(IntPtr context, IntPtr view);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate IntPtr BNPlatformGetRegisterList(
			IntPtr context,
			IntPtr count
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNPlatformFreeRegisterList(
			IntPtr context,
			IntPtr registers,
			ulong count
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate IntPtr BNPlatformGetGlobalRegisterType(
			IntPtr context,
			uint register
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate ulong BNPlatformGetAddressSize(IntPtr context);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNPlatformAdjustTypeParserInput(
			IntPtr context,
			IntPtr parser,
			IntPtr argumentsIn,
			ulong argumentsLengthIn,
			IntPtr sourceFileNamesIn,
			IntPtr sourceFileValuesIn,
			ulong sourceFilesLengthIn,
			IntPtr argumentsOut,
			IntPtr argumentsLengthOut,
			IntPtr sourceFileNamesOut,
			IntPtr sourceFileValuesOut,
			IntPtr sourceFilesLengthOut
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNPlatformFreeTypeParserInput(
			IntPtr context,
			IntPtr arguments,
			ulong argumentsLength,
			IntPtr sourceFileNames,
			IntPtr sourceFileValues,
			ulong sourceFilesLength
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNPlatformGetFallbackEnabled(IntPtr context);
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct BNCustomPlatform
	{
		internal IntPtr context;
		internal IntPtr init;
		internal IntPtr viewInit;
		internal IntPtr getGlobalRegisters;
		internal IntPtr freeRegisterList;
		internal IntPtr getGlobalRegisterType;
		internal IntPtr getAddressSize;
		internal IntPtr adjustTypeParserInput;
		internal IntPtr freeTypeParserInput;
		internal IntPtr getFallbackEnabled;
	}

	/// <summary>One virtual source file passed through platform type-parser adjustment.</summary>
	public sealed class PlatformTypeParserSourceFile
	{
		public string Name { get; set; }

		public string Contents { get; set; }

		public PlatformTypeParserSourceFile(string name, string contents)
		{
			this.Name = name ?? string.Empty;
			this.Contents = contents ?? string.Empty;
		}
	}

	/// <summary>
	/// Retained for source compatibility. Custom platforms derive from Platform directly.
	/// </summary>
	public class CustomPlatform
	{
	}
}
