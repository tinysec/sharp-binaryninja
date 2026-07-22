using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	internal static partial class NativeDelegates
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNTypePrinterGetTypeTokens(
			IntPtr context, IntPtr type, IntPtr platform, IntPtr name,
			byte baseConfidence, TokenEscapingType escaping,
			IntPtr result, IntPtr resultCount
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNTypePrinterGetTypeTokensWithParent(
			IntPtr context, IntPtr type, IntPtr platform, byte baseConfidence,
			IntPtr parentType, TokenEscapingType escaping,
			IntPtr result, IntPtr resultCount
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNTypePrinterGetTypeString(
			IntPtr context, IntPtr type, IntPtr platform, IntPtr name,
			TokenEscapingType escaping, IntPtr result
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNTypePrinterGetTypeStringPart(
			IntPtr context, IntPtr type, IntPtr platform,
			TokenEscapingType escaping, IntPtr result
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNTypePrinterGetTypeLines(
			IntPtr context, IntPtr type, IntPtr types, IntPtr name,
			int paddingCols, [MarshalAs(UnmanagedType.I1)] bool collapsed,
			TokenEscapingType escaping, IntPtr result, IntPtr resultCount
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNTypePrinterPrintAllTypes(
			IntPtr context, IntPtr names, IntPtr types, ulong typeCount,
			IntPtr data, int paddingCols, TokenEscapingType escaping, IntPtr result
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNTypePrinterFreeList(
			IntPtr context, IntPtr values, ulong count
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNTypePrinterFreeString(IntPtr context, IntPtr value);
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct BNTypePrinterCallbacks
	{
		/// <summary>
		/// void* context
		/// </summary>
		public IntPtr context;
		
		/// <summary>
		/// void** getTypeTokens
		/// </summary>
		public IntPtr getTypeTokens;
		
		/// <summary>
		/// void** getTypeTokensBeforeName
		/// </summary>
		public IntPtr getTypeTokensBeforeName;
		
		/// <summary>
		/// void** getTypeTokensAfterName
		/// </summary>
		public IntPtr getTypeTokensAfterName;
		
		/// <summary>
		/// void** getTypeString
		/// </summary>
		public IntPtr getTypeString;
		
		/// <summary>
		/// void** getTypeStringBeforeName
		/// </summary>
		public IntPtr getTypeStringBeforeName;
		
		/// <summary>
		/// void** getTypeStringAfterName
		/// </summary>
		public IntPtr getTypeStringAfterName;
		
		/// <summary>
		/// void** getTypeLines
		/// </summary>
		public IntPtr getTypeLines;
		
		/// <summary>
		/// void** printAllTypes
		/// </summary>
		public IntPtr printAllTypes;
		
		/// <summary>
		/// void** freeTokens
		/// </summary>
		public IntPtr freeTokens;
		
		/// <summary>
		/// void** freeString
		/// </summary>
		public IntPtr freeString;
		
		/// <summary>
		/// void** freeLines
		/// </summary>
		public IntPtr freeLines;
		
	}

	/// <summary>
	/// Retained for source compatibility. Custom printers derive from TypePrinter directly.
	/// </summary>
	public class TypePrinterCallbacks
	{
	}
}
