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
		internal delegate bool BNTypeParserGetOptionText(
			IntPtr context,
			TypeParserOption option,
			[MarshalAs(UnmanagedType.LPUTF8Str)] string value,
			IntPtr result
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNTypeParserPreprocessSource(
			IntPtr context,
			[MarshalAs(UnmanagedType.LPUTF8Str)] string source,
			[MarshalAs(UnmanagedType.LPUTF8Str)] string fileName,
			IntPtr platform,
			IntPtr existingTypes,
			IntPtr options,
			ulong optionCount,
			IntPtr includeDirs,
			ulong includeDirCount,
			IntPtr output,
			IntPtr errors,
			IntPtr errorCount
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNTypeParserParseTypesFromSource(
			IntPtr context,
			[MarshalAs(UnmanagedType.LPUTF8Str)] string source,
			[MarshalAs(UnmanagedType.LPUTF8Str)] string fileName,
			IntPtr platform,
			IntPtr existingTypes,
			IntPtr options,
			ulong optionCount,
			IntPtr includeDirs,
			ulong includeDirCount,
			[MarshalAs(UnmanagedType.LPUTF8Str)] string autoTypeSource,
			IntPtr result,
			IntPtr errors,
			IntPtr errorCount
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNTypeParserParseTypeString(
			IntPtr context,
			[MarshalAs(UnmanagedType.LPUTF8Str)] string source,
			IntPtr platform,
			IntPtr existingTypes,
			IntPtr result,
			IntPtr errors,
			IntPtr errorCount
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNTypeParserFreeString(IntPtr context, IntPtr value);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNTypeParserFreeResult(IntPtr context, IntPtr result);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNTypeParserFreeErrorList(
			IntPtr context,
			IntPtr errors,
			ulong errorCount
		);
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct BNTypeParserCallbacks
	{
		/// <summary>
		/// void* context
		/// </summary>
		public IntPtr context;
		
		/// <summary>
		/// void** getOptionText
		/// </summary>
		public IntPtr getOptionText;
		
		/// <summary>
		/// void** preprocessSource
		/// </summary>
		public IntPtr preprocessSource;
		
		/// <summary>
		/// void** parseTypesFromSource
		/// </summary>
		public IntPtr parseTypesFromSource;
		
		/// <summary>
		/// void** parseTypeString
		/// </summary>
		public IntPtr parseTypeString;
		
		/// <summary>
		/// void** freeString
		/// </summary>
		public IntPtr freeString;
		
		/// <summary>
		/// void** freeResult
		/// </summary>
		public IntPtr freeResult;
		
		/// <summary>
		/// void** freeErrorList
		/// </summary>
		public IntPtr freeErrorList;
	}

	/// <summary>
	/// Retained for source compatibility. Custom parsers derive from TypeParser directly.
	/// </summary>
	public class TypeParserCallbacks
	{
	}
}
