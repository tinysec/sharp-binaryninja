using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public static partial class Core
	{
		// ────────────────────────────────────────────────────────────────────
		//  Demangling
		// ────────────────────────────────────────────────────────────────────

		/// <summary>
		/// Demangle a GNU3 (Itanium ABI) mangled name into its component name parts.
		/// </summary>
		/// <param name="arch">The architecture context for demangling.</param>
		/// <param name="mangledName">The mangled symbol name.</param>
		/// <param name="simplify">Whether to simplify the output.</param>
		/// <param name="outType">Receives the demangled type, or null if only a name is returned.</param>
		/// <param name="outNames">Receives the demangled name parts.</param>
		/// <returns>True if demangling succeeded.</returns>
		public static unsafe bool DemangleGNU3(
			Architecture arch ,
			string mangledName ,
			bool simplify ,
			out BinaryNinja.Type? outType ,
			out string[] outNames)
		{
			// 1. Prepare output slots on the stack.
			IntPtr typePtr = IntPtr.Zero;
			IntPtr namesPtr = IntPtr.Zero;
			ulong nameCount = 0;

			// 2. Call the native demangler.
			bool success = NativeMethods.BNDemangleGNU3(
				arch.DangerousGetHandle() ,
				mangledName ,
				(IntPtr)(&typePtr) ,
				(IntPtr)(&namesPtr) ,
				(IntPtr)(&nameCount) ,
				simplify
			);

			// 3. Wrap the returned type pointer (new owned reference when non-null).
			outType = BinaryNinja.Type.TakeHandle(typePtr);

			// 4. Copy the name strings and free the native array.
			outNames = UnsafeUtils.TakeAnsiStringArray(namesPtr , nameCount , NativeMethods.BNFreeStringList);

			return success;
		}

		/// <summary>
		/// Demangle a GNU3 (Itanium ABI) mangled name with options from a BinaryView.
		/// </summary>
		/// <param name="arch">The architecture context for demangling.</param>
		/// <param name="mangledName">The mangled symbol name.</param>
		/// <param name="view">Optional binary view providing demangling options (null for defaults).</param>
		/// <param name="outType">Receives the demangled type, or null if only a name is returned.</param>
		/// <param name="outNames">Receives the demangled name parts.</param>
		/// <returns>True if demangling succeeded.</returns>
		public static unsafe bool DemangleGNU3WithOptions(
			Architecture arch ,
			string mangledName ,
			BinaryView? view ,
			out BinaryNinja.Type? outType ,
			out string[] outNames)
		{
			// 1. Prepare output slots on the stack.
			IntPtr typePtr = IntPtr.Zero;
			IntPtr namesPtr = IntPtr.Zero;
			ulong nameCount = 0;

			// 2. Resolve the optional binary view handle.
			IntPtr viewHandle = (view != null) ? view.DangerousGetHandle() : IntPtr.Zero;

			// 3. Call the native demangler.
			bool success = NativeMethods.BNDemangleGNU3WithOptions(
				arch.DangerousGetHandle() ,
				mangledName ,
				(IntPtr)(&typePtr) ,
				(IntPtr)(&namesPtr) ,
				(IntPtr)(&nameCount) ,
				viewHandle
			);

			// 4. Wrap the returned type pointer.
			outType = BinaryNinja.Type.TakeHandle(typePtr);

			// 5. Copy and free the name string array.
			outNames = UnsafeUtils.TakeAnsiStringArray(namesPtr , nameCount , NativeMethods.BNFreeStringList);

			return success;
		}

		/// <summary>
		/// Demangle an LLVM mangled name into its component name parts.
		/// </summary>
		/// <param name="mangledName">The mangled symbol name.</param>
		/// <param name="simplify">Whether to simplify the output.</param>
		/// <param name="outNames">Receives the demangled name parts.</param>
		/// <returns>True if demangling succeeded.</returns>
		public static unsafe bool DemangleLLVM(
			string mangledName ,
			bool simplify ,
			out string[] outNames)
		{
			// 1. Prepare output slots on the stack.
			IntPtr namesPtr = IntPtr.Zero;
			ulong nameCount = 0;

			// 2. Call the native demangler.
			bool success = NativeMethods.BNDemangleLLVM(
				mangledName ,
				(IntPtr)(&namesPtr) ,
				(IntPtr)(&nameCount) ,
				simplify
			);

			// 3. Copy and free the name string array.
			outNames = UnsafeUtils.TakeAnsiStringArray(namesPtr , nameCount , NativeMethods.BNFreeStringList);

			return success;
		}

		/// <summary>
		/// Demangle an LLVM mangled name with options from a BinaryView.
		/// </summary>
		/// <param name="mangledName">The mangled symbol name.</param>
		/// <param name="view">Optional binary view providing demangling options (null for defaults).</param>
		/// <param name="outNames">Receives the demangled name parts.</param>
		/// <returns>True if demangling succeeded.</returns>
		public static unsafe bool DemangleLLVMWithOptions(
			string mangledName ,
			BinaryView? view ,
			out string[] outNames)
		{
			// 1. Prepare output slots on the stack.
			IntPtr namesPtr = IntPtr.Zero;
			ulong nameCount = 0;

			// 2. Resolve the optional binary view handle.
			IntPtr viewHandle = (view != null) ? view.DangerousGetHandle() : IntPtr.Zero;

			// 3. Call the native demangler.
			bool success = NativeMethods.BNDemangleLLVMWithOptions(
				mangledName ,
				(IntPtr)(&namesPtr) ,
				(IntPtr)(&nameCount) ,
				viewHandle
			);

			// 4. Copy and free the name string array.
			outNames = UnsafeUtils.TakeAnsiStringArray(namesPtr , nameCount , NativeMethods.BNFreeStringList);

			return success;
		}

		/// <summary>
		/// Demangle a Microsoft Visual C++ mangled name into its component name parts.
		/// </summary>
		/// <param name="arch">The architecture context for demangling.</param>
		/// <param name="mangledName">The mangled symbol name.</param>
		/// <param name="simplify">Whether to simplify the output.</param>
		/// <param name="outType">Receives the demangled type, or null if only a name is returned.</param>
		/// <param name="outNames">Receives the demangled name parts.</param>
		/// <returns>True if demangling succeeded.</returns>
		public static unsafe bool DemangleMS(
			Architecture arch ,
			string mangledName ,
			bool simplify ,
			out BinaryNinja.Type? outType ,
			out string[] outNames)
		{
			// 1. Prepare output slots on the stack.
			IntPtr typePtr = IntPtr.Zero;
			IntPtr namesPtr = IntPtr.Zero;
			ulong nameCount = 0;

			// 2. Call the native demangler.
			bool success = NativeMethods.BNDemangleMS(
				arch.DangerousGetHandle() ,
				mangledName ,
				(IntPtr)(&typePtr) ,
				(IntPtr)(&namesPtr) ,
				(IntPtr)(&nameCount) ,
				simplify
			);

			// 3. Wrap the returned type pointer.
			outType = BinaryNinja.Type.TakeHandle(typePtr);

			// 4. Copy and free the name string array.
			outNames = UnsafeUtils.TakeAnsiStringArray(namesPtr , nameCount , NativeMethods.BNFreeStringList);

			return success;
		}

		/// <summary>
		/// Demangle a Microsoft Visual C++ mangled name with options from a BinaryView.
		/// </summary>
		/// <param name="arch">The architecture context for demangling.</param>
		/// <param name="mangledName">The mangled symbol name.</param>
		/// <param name="view">Optional binary view providing demangling options (null for defaults).</param>
		/// <param name="outType">Receives the demangled type, or null if only a name is returned.</param>
		/// <param name="outNames">Receives the demangled name parts.</param>
		/// <returns>True if demangling succeeded.</returns>
		public static unsafe bool DemangleMSWithOptions(
			Architecture arch ,
			string mangledName ,
			BinaryView? view ,
			out BinaryNinja.Type? outType ,
			out string[] outNames)
		{
			// 1. Prepare output slots on the stack.
			IntPtr typePtr = IntPtr.Zero;
			IntPtr namesPtr = IntPtr.Zero;
			ulong nameCount = 0;

			// 2. Resolve the optional binary view handle.
			IntPtr viewHandle = (view != null) ? view.DangerousGetHandle() : IntPtr.Zero;

			// 3. Call the native demangler.
			bool success = NativeMethods.BNDemangleMSWithOptions(
				arch.DangerousGetHandle() ,
				mangledName ,
				(IntPtr)(&typePtr) ,
				(IntPtr)(&namesPtr) ,
				(IntPtr)(&nameCount) ,
				viewHandle
			);

			// 4. Wrap the returned type pointer.
			outType = BinaryNinja.Type.TakeHandle(typePtr);

			// 5. Copy and free the name string array.
			outNames = UnsafeUtils.TakeAnsiStringArray(namesPtr , nameCount , NativeMethods.BNFreeStringList);

			return success;
		}

		/// <summary>
		/// Demangle a Microsoft Visual C++ mangled name using a platform instead of an architecture.
		/// </summary>
		/// <param name="platform">The platform context for demangling.</param>
		/// <param name="mangledName">The mangled symbol name.</param>
		/// <param name="simplify">Whether to simplify the output.</param>
		/// <param name="outType">Receives the demangled type, or null if only a name is returned.</param>
		/// <param name="outNames">Receives the demangled name parts.</param>
		/// <returns>True if demangling succeeded.</returns>
		public static unsafe bool DemangleMSPlatform(
			Platform platform ,
			string mangledName ,
			bool simplify ,
			out BinaryNinja.Type? outType ,
			out string[] outNames)
		{
			// 1. Prepare output slots on the stack.
			IntPtr typePtr = IntPtr.Zero;
			IntPtr namesPtr = IntPtr.Zero;
			ulong nameCount = 0;

			// 2. Call the native demangler.
			bool success = NativeMethods.BNDemangleMSPlatform(
				platform.DangerousGetHandle() ,
				mangledName ,
				(IntPtr)(&typePtr) ,
				(IntPtr)(&namesPtr) ,
				(IntPtr)(&nameCount) ,
				simplify
			);

			// 3. Wrap the returned type pointer.
			outType = BinaryNinja.Type.TakeHandle(typePtr);

			// 4. Copy and free the name string array.
			outNames = UnsafeUtils.TakeAnsiStringArray(namesPtr , nameCount , NativeMethods.BNFreeStringList);

			return success;
		}

		/// <summary>
		/// Attempt generic demangling of a mangled name using all registered demanglers.
		/// Returns a QualifiedName and optionally a type.
		/// </summary>
		/// <param name="arch">The architecture context for demangling.</param>
		/// <param name="mangledName">The mangled symbol name.</param>
		/// <param name="view">Optional binary view providing demangling options (null for defaults).</param>
		/// <param name="simplify">Whether to simplify the output.</param>
		/// <param name="outType">Receives the demangled type, or null if only a name is returned.</param>
		/// <param name="outVarName">Receives the demangled qualified variable name.</param>
		/// <returns>True if demangling succeeded.</returns>
		public static unsafe bool DemangleGeneric(
			Architecture arch ,
			string mangledName ,
			BinaryView? view ,
			bool simplify ,
			out BinaryNinja.Type? outType ,
			out QualifiedName outVarName)
		{
			// 1. Prepare output slots on the stack.
			IntPtr typePtr = IntPtr.Zero;
			BNQualifiedName nativeVarName = new BNQualifiedName();

			// 2. Resolve the optional binary view handle.
			IntPtr viewHandle = (view != null) ? view.DangerousGetHandle() : IntPtr.Zero;

			// 3. Call the native demangler.
			bool success = NativeMethods.BNDemangleGeneric(
				arch.DangerousGetHandle() ,
				mangledName ,
				(IntPtr)(&typePtr) ,
				(IntPtr)(&nativeVarName) ,
				viewHandle ,
				simplify
			);

			// 4. Wrap the returned type pointer.
			outType = BinaryNinja.Type.TakeHandle(typePtr);

			// 5. Convert the native qualified name struct to managed, freeing native memory.
			outVarName = QualifiedName.TakeNative(nativeVarName);

			return success;
		}

		/// <summary>
		/// Check whether the given name appears to be mangled according to any registered demangler.
		/// </summary>
		/// <param name="demangler">The demangler instance to query.</param>
		/// <param name="name">The symbol name to check.</param>
		/// <returns>True if the name looks like a mangled name for this demangler.</returns>
		public static bool IsDemanglerMangledName(Demangler demangler , string name)
		{
			return NativeMethods.BNIsDemanglerMangledName(
				demangler.DangerousGetHandle() ,
				name
			);
		}

		/// <summary>
		/// Check whether the given name appears to be a GNU3 (Itanium ABI) mangled string.
		/// </summary>
		/// <param name="mangledName">The symbol name to check.</param>
		/// <returns>True if the name looks like a GNU3 mangled string.</returns>
		public static bool IsGNU3MangledString(string mangledName)
		{
			return NativeMethods.BNIsGNU3MangledString(mangledName);
		}

		/// <summary>
		/// Escape a type name so it can be safely used in text output.
		/// </summary>
		/// <param name="name">The type name to escape.</param>
		/// <param name="escaping">The escaping style to apply.</param>
		/// <returns>The escaped type name string.</returns>
		public static string EscapeTypeName(string name , TokenEscapingType escaping)
		{
			return UnsafeUtils.TakeAnsiString(
				NativeMethods.BNEscapeTypeName(name , escaping)
			);
		}

		/// <summary>
		/// Unescape a previously escaped type name back to its original form.
		/// </summary>
		/// <param name="name">The escaped type name to unescape.</param>
		/// <param name="escaping">The escaping style that was used.</param>
		/// <returns>The unescaped type name string.</returns>
		public static string UnescapeTypeName(string name , TokenEscapingType escaping)
		{
			return UnsafeUtils.TakeAnsiString(
				NativeMethods.BNUnescapeTypeName(name , escaping)
			);
		}

		// ────────────────────────────────────────────────────────────────────
		//  Compression
		// ────────────────────────────────────────────────────────────────────

		/// <summary>
		/// Compress a data buffer using zlib.
		/// </summary>
		/// <param name="data">The data buffer to compress.</param>
		/// <returns>A new data buffer containing the compressed data.</returns>
		public static DataBuffer ZlibCompress(DataBuffer data)
		{
			return DataBuffer.MustTakeHandle(
				NativeMethods.BNZlibCompress(data.DangerousGetHandle())
			);
		}

		/// <summary>
		/// Decompress a zlib-compressed data buffer.
		/// </summary>
		/// <param name="data">The compressed data buffer.</param>
		/// <returns>A new data buffer containing the decompressed data.</returns>
		public static DataBuffer ZlibDecompress(DataBuffer data)
		{
			return DataBuffer.MustTakeHandle(
				NativeMethods.BNZlibDecompress(data.DangerousGetHandle())
			);
		}

		/// <summary>
		/// Decompress an LZMA-compressed data buffer.
		/// </summary>
		/// <param name="data">The compressed data buffer.</param>
		/// <returns>A new data buffer containing the decompressed data.</returns>
		public static DataBuffer LzmaDecompress(DataBuffer data)
		{
			return DataBuffer.MustTakeHandle(
				NativeMethods.BNLzmaDecompress(data.DangerousGetHandle())
			);
		}

		/// <summary>
		/// Decompress an LZMA2-compressed data buffer.
		/// </summary>
		/// <param name="data">The compressed data buffer.</param>
		/// <returns>A new data buffer containing the decompressed data.</returns>
		public static DataBuffer Lzma2Decompress(DataBuffer data)
		{
			return DataBuffer.MustTakeHandle(
				NativeMethods.BNLzma2Decompress(data.DangerousGetHandle())
			);
		}

		/// <summary>
		/// Decompress an XZ-compressed data buffer.
		/// </summary>
		/// <param name="data">The compressed data buffer.</param>
		/// <returns>A new data buffer containing the decompressed data.</returns>
		public static DataBuffer XzDecompress(DataBuffer data)
		{
			return DataBuffer.MustTakeHandle(
				NativeMethods.BNXzDecompress(data.DangerousGetHandle())
			);
		}

		// ────────────────────────────────────────────────────────────────────
		//  Unicode Utilities
		// ────────────────────────────────────────────────────────────────────

		/// <summary>
		/// Get all known Unicode block names.
		/// </summary>
		/// <returns>An array of Unicode block name strings.</returns>
		public static unsafe string[] UnicodeGetBlockNames()
		{
			// 1. Prepare output slots on the stack.
			IntPtr namesPtr = IntPtr.Zero;
			ulong count = 0;

			// 2. Call the native function to retrieve block names.
			NativeMethods.BNUnicodeGetBlockNames(
				(IntPtr)(&namesPtr) ,
				(IntPtr)(&count)
			);

			// 3. Copy and free the name string array.
			return UnsafeUtils.TakeAnsiStringArray(namesPtr , count , NativeMethods.BNFreeStringList);
		}

		/// <summary>
		/// Get the code point range for a named Unicode block.
		/// </summary>
		/// <param name="name">The Unicode block name.</param>
		/// <param name="rangeStart">Receives the starting code point of the block.</param>
		/// <param name="rangeEnd">Receives the ending code point of the block.</param>
		/// <returns>True if the block was found.</returns>
		public static unsafe bool UnicodeGetBlockRange(
			string name ,
			out uint rangeStart ,
			out uint rangeEnd)
		{
			// 1. Prepare output slots on the stack.
			uint start = 0;
			uint end = 0;

			// 2. Call the native function.
			bool success = NativeMethods.BNUnicodeGetBlockRange(
				name ,
				(IntPtr)(&start) ,
				(IntPtr)(&end)
			);

			// 3. Copy results out.
			rangeStart = start;
			rangeEnd = end;

			return success;
		}

		/// <summary>
		/// Get all Unicode block names, start code points, and end code points.
		/// </summary>
		/// <param name="names">Receives the block name strings.</param>
		/// <param name="rangeStarts">Receives the starting code points.</param>
		/// <param name="rangeEnds">Receives the ending code points.</param>
		public static unsafe void UnicodeGetBlockRanges(
			out string[] names ,
			out uint[] rangeStarts ,
			out uint[] rangeEnds)
		{
			// 1. Prepare output slots on the stack.
			IntPtr namesPtr = IntPtr.Zero;
			IntPtr startsPtr = IntPtr.Zero;
			IntPtr endsPtr = IntPtr.Zero;
			ulong count = 0;

			// 2. Call the native function.
			NativeMethods.BNUnicodeGetBlockRanges(
				(IntPtr)(&namesPtr) ,
				(IntPtr)(&startsPtr) ,
				(IntPtr)(&endsPtr) ,
				(IntPtr)(&count)
			);

			try
			{
				// Copy the arrays before releasing the paired range allocation.
				names = UnsafeUtils.TakeAnsiStringArray(namesPtr, count, NativeMethods.BNFreeStringList);
				namesPtr = IntPtr.Zero;
				rangeStarts = UnsafeUtils.ReadNumberArray<uint>(startsPtr, count);
				rangeEnds = UnsafeUtils.ReadNumberArray<uint>(endsPtr, count);
			}
			finally
			{
				if (IntPtr.Zero != namesPtr)
				{
					NativeMethods.BNFreeStringList(namesPtr, count);
				}

				if (IntPtr.Zero != startsPtr || IntPtr.Zero != endsPtr)
				{
					NativeMethods.BNFreeUnicodeRangeList(startsPtr, endsPtr);
				}
			}
		}

		/// <summary>
		/// Convert a single UTF-32 code point (as a 4-byte buffer) to a UTF-8 string.
		/// </summary>
		/// <param name="utf32Bytes">The raw UTF-32 bytes (at least 4 bytes).</param>
		/// <returns>The UTF-8 string representation.</returns>
		public static unsafe string UnicodeUTF32ToUTF8(byte[] utf32Bytes)
		{
			if (null == utf32Bytes)
			{
				throw new ArgumentNullException(nameof(utf32Bytes));
			}

			if (4 > utf32Bytes.Length)
			{
				throw new ArgumentException("A UTF-32 code point requires at least four bytes.", nameof(utf32Bytes));
			}

			fixed (byte* data = utf32Bytes)
			{
				return UnsafeUtils.TakeUtf8String(
					NativeMethods.BNUnicodeUTF32ToUTF8((IntPtr)data));
			}
		}

		/// <summary>
		/// Convert the first UTF-16 code point in a byte buffer to a UTF-8 string.
		/// </summary>
		/// <param name="utf16Bytes">The raw UTF-16 bytes.</param>
		/// <returns>The UTF-8 string representation.</returns>
		public static unsafe string UnicodeUTF16ToUTF8(byte[] utf16Bytes)
		{
			if (null == utf16Bytes)
			{
				throw new ArgumentNullException(nameof(utf16Bytes));
			}

			if (2 > utf16Bytes.Length)
			{
				throw new ArgumentException("A UTF-16 code point requires at least two bytes.", nameof(utf16Bytes));
			}

			fixed (byte* data = utf16Bytes)
			{
				return UnsafeUtils.TakeUtf8String(
					NativeMethods.BNUnicodeUTF16ToUTF8((IntPtr)data, (ulong)utf16Bytes.Length));
			}
		}

		// ────────────────────────────────────────────────────────────────────
		//  Worker Threads
		// ────────────────────────────────────────────────────────────────────

		/// <summary>
		/// Get the current number of worker threads used by the analysis engine.
		/// </summary>
		/// <returns>The current worker thread count.</returns>
		public static ulong GetWorkerThreadCount()
		{
			return NativeMethods.BNGetWorkerThreadCount();
		}

		/// <summary>
		/// Set the number of worker threads used by the analysis engine.
		/// </summary>
		/// <param name="count">The desired worker thread count.</param>
		public static void SetWorkerThreadCount(ulong count)
		{
			NativeMethods.BNSetWorkerThreadCount(count);
		}

		/// <summary>
		/// Check whether the current thread is the main Binary Ninja thread.
		/// </summary>
		/// <returns>True if this is the main thread.</returns>
		public static bool IsMainThread()
		{
			return NativeMethods.BNIsMainThread();
		}

		/// <summary>
		/// Set a human-readable name for the current thread, visible in debuggers and logs.
		/// </summary>
		/// <param name="name">The thread name to set.</param>
		public static void SetThreadName(string name)
		{
			NativeMethods.BNSetThreadName(name);
		}

		// ────────────────────────────────────────────────────────────────────
		//  Product / License Information
		// ────────────────────────────────────────────────────────────────────

		/// <summary>
		/// Get the product name string (e.g., "Binary Ninja Personal").
		/// </summary>
		/// <returns>The product name.</returns>
		public static string GetProduct()
		{
			return UnsafeUtils.TakeAnsiString(
				NativeMethods.BNGetProduct()
			);
		}

		/// <summary>
		/// Get the product type string (e.g., "Personal", "Commercial").
		/// </summary>
		/// <returns>The product type.</returns>
		public static string GetProductType()
		{
			return UnsafeUtils.TakeAnsiString(
				NativeMethods.BNGetProductType()
			);
		}

		/// <summary>
		/// Get the number of seats on the current license.
		/// </summary>
		/// <returns>The license seat count.</returns>
		public static int GetLicenseCount()
		{
			return NativeMethods.BNGetLicenseCount();
		}

		/// <summary>
		/// Get the license expiration time as a Unix timestamp.
		/// </summary>
		/// <returns>The expiration time in seconds since epoch.</returns>
		public static ulong GetLicenseExpirationTime()
		{
			return NativeMethods.BNGetLicenseExpirationTime();
		}

		/// <summary>
		/// Get the email address of the licensed user.
		/// </summary>
		/// <returns>The licensed user email string.</returns>
		public static string GetLicensedUserEmail()
		{
			return UnsafeUtils.TakeAnsiString(
				NativeMethods.BNGetLicensedUserEmail()
			);
		}

		// ────────────────────────────────────────────────────────────────────
		//  Bit Manipulation
		// ────────────────────────────────────────────────────────────────────

		/// <summary>
		/// Sign-extend a value from one bit width to another.
		/// </summary>
		/// <param name="value">The value to sign-extend.</param>
		/// <param name="sourceSize">The original bit width in bytes.</param>
		/// <param name="destSize">The target bit width in bytes.</param>
		/// <returns>The sign-extended value.</returns>
		public static long SignExtend(long value , ulong sourceSize , ulong destSize)
		{
			return NativeMethods.BNSignExtend(value , sourceSize , destSize);
		}

		/// <summary>
		/// Zero-extend a value from one bit width to another.
		/// </summary>
		/// <param name="value">The value to zero-extend.</param>
		/// <param name="sourceSize">The original bit width in bytes.</param>
		/// <param name="destSize">The target bit width in bytes.</param>
		/// <returns>The zero-extended value.</returns>
		public static long ZeroExtend(long value , ulong sourceSize , ulong destSize)
		{
			return NativeMethods.BNZeroExtend(value , sourceSize , destSize);
		}

		/// <summary>
		/// Mask a value to the given size by zeroing bits above the specified byte count.
		/// </summary>
		/// <param name="value">The value to mask.</param>
		/// <param name="size">The size in bytes to mask to.</param>
		/// <returns>The masked value.</returns>
		public static long MaskToSize(long value , ulong size)
		{
			return NativeMethods.BNMaskToSize(value , size);
		}

		/// <summary>
		/// Get a bitmask that covers all bits for the given byte size.
		/// </summary>
		/// <param name="size">The size in bytes.</param>
		/// <returns>A bitmask with all bits set for the given size.</returns>
		public static long GetMaskForSize(ulong size)
		{
			return NativeMethods.BNGetMaskForSize(size);
		}

		// ────────────────────────────────────────────────────────────────────
		//  String Utilities
		// ────────────────────────────────────────────────────────────────────

		/// <summary>
		/// Get the literal prefix string for a given string type (e.g., "L" for wide strings).
		/// </summary>
		/// <param name="type">The string type.</param>
		/// <returns>The literal prefix string for that type.</returns>
		public static string GetStringLiteralPrefix(StringType type)
		{
			// BNGetStringLiteralPrefix returns a static const char* that must not be freed.
			return UnsafeUtils.ReadAnsiString(
				NativeMethods.BNGetStringLiteralPrefix(type)
			);
		}

		/// <summary>
		/// Get the rendered width in characters for a given address value.
		/// </summary>
		/// <param name="addr">The address value.</param>
		/// <returns>The number of characters needed to render the address.</returns>
		public static uint GetAddressRenderedWidth(ulong addr)
		{
			return NativeMethods.BNGetAddressRenderedWidth(addr);
		}

		/// <summary>
		/// Simplify a Rust mangled symbol name to a human-readable string.
		/// </summary>
		/// <param name="name">The Rust mangled name.</param>
		/// <returns>The simplified name string.</returns>
		public static string RustSimplifyStrToStr(string name)
		{
			return UnsafeUtils.TakeAnsiString(
				NativeMethods.BNRustSimplifyStrToStr(name)
			);
		}

		/// <summary>
		/// Simplifies a templated C++ name to a qualified name, mirroring Python
		/// <c>demangle.simplify_name_to_qualified_name</c> (demangle.py:253). This can also tokenize a
		/// string to a qualified name without simplifying. Returns <c>null</c> when the simplifier
		/// yields no components.
		/// </summary>
		/// <param name="inputName">The name to simplify.</param>
		/// <param name="simplify">Whether to simplify the name; defaults to <c>true</c>.</param>
		/// <returns>The simplified qualified name, or <c>null</c> if the result is empty.</returns>
		public static QualifiedName? SimplifyNameToQualifiedName(string inputName, bool simplify = true)
		{
			if (null == inputName)
			{
				throw new ArgumentNullException(nameof(inputName));
			}

			// BNRustSimplifyStrToFQN returns a BNQualifiedName by value. The struct is blittable
			// (two IntPtrs + a ulong), so the P/Invoke marshaller handles the struct-return (sret)
			// ABI directly -- the same pattern as BNTypeBuilderGetStructureName. TakeNative reads the
			// components eagerly then frees the native array the core allocated.
			QualifiedName result = QualifiedName.TakeNative(
				NativeMethods.BNRustSimplifyStrToFQN(inputName, simplify)
			);

			if (0 == result.Name.Length)
			{
				return null;
			}

			return result;
		}

		/// <summary>
		/// Simplifies an already-tokenized qualified name, mirroring Python
		/// <c>demangle.simplify_name_to_qualified_name</c> for <c>QualifiedName</c> input
		/// (demangle.py:267). Python forces simplification for qualified-name input, so
		/// <paramref name="inputName"/> is always simplified.
		/// </summary>
		/// <param name="inputName">The qualified name to simplify.</param>
		/// <returns>The simplified qualified name, or <c>null</c> if the result is empty.</returns>
		public static QualifiedName? SimplifyNameToQualifiedName(QualifiedName inputName)
		{
			if (null == inputName)
			{
				throw new ArgumentNullException(nameof(inputName));
			}

			return Core.SimplifyNameToQualifiedName(inputName.ToString(), true);
		}

		/// <summary>
		/// Compute a fuzzy match score between a target string and a query.
		/// Returns 0 if there is no match. Higher values indicate better matches.
		/// </summary>
		/// <param name="target">The string to match against.</param>
		/// <param name="query">The query string.</param>
		/// <returns>The fuzzy match score (0 means no match).</returns>
		public static ulong FuzzyMatchSingle(string target , string query)
		{
			return NativeMethods.BNFuzzyMatchSingle(target , query);
		}

		// ────────────────────────────────────────────────────────────────────
		//  Core Enum Conversion
		// ────────────────────────────────────────────────────────────────────

		/// <summary>
		/// Convert a core enum value to its string representation.
		/// </summary>
		/// <param name="enumName">The name of the enum type (e.g., "BNStringType").</param>
		/// <param name="value">The enum integer value.</param>
		/// <param name="result">Receives the string representation on success.</param>
		/// <returns>True if the enum value was recognized and converted.</returns>
		public static bool CoreEnumToString(string enumName , ulong value , out string result)
		{
			// 1. Call the native function; result is a char* the core allocates.
			IntPtr resultPointer;
			bool success = NativeMethods.BNCoreEnumToString(enumName , value , out resultPointer);

			// 2. Decode + free the core-allocated string (no-op on null).
			result = UnsafeUtils.TakeUtf8String(resultPointer);

			return success;
		}

		/// <summary>
		/// Convert a core enum string representation to its integer value.
		/// </summary>
		/// <param name="enumName">The name of the enum type (e.g., "BNStringType").</param>
		/// <param name="value">The string representation of the enum value.</param>
		/// <param name="result">Receives the integer value on success.</param>
		/// <returns>True if the string was recognized and converted.</returns>
		public static unsafe bool CoreEnumFromString(string enumName , string value , out ulong result)
		{
			// 1. Prepare output slot on the stack.
			ulong nativeResult = 0;

			// 2. Call the native function.
			bool success = NativeMethods.BNCoreEnumFromString(
				enumName ,
				value ,
				(IntPtr)(&nativeResult)
			);

			// 3. Copy the result.
			result = nativeResult;

			return success;
		}

		// ────────────────────────────────────────────────────────────────────
		//  Stack Trace
		// ────────────────────────────────────────────────────────────────────

		/// <summary>
		/// Gets the current native stack trace as a string.
		/// Useful for debugging native interop issues.
		/// </summary>
		/// <returns>A string representation of the current native stack trace.</returns>
		public static string GetCurrentStackTraceString()
		{
			return UnsafeUtils.TakeAnsiString(
				NativeMethods.BNGetCurrentStackTraceString()
			);
		}

		// ────────────────────────────────────────────────────────────────────
		//  Version Parsing and Comparison
		// ────────────────────────────────────────────────────────────────────

		// TODO: ParseVersionString uses sret (struct return) calling convention
		//       for BNVersionInfo, and BNVersionLessThan takes BNVersionInfo by value.
		//       The P/Invoke bindings pass the managed VersionInfo class by reference,
		//       which may not match the native ABI. These need careful ABI-level verification
		//       before use.
		//
		// public static VersionInfo ParseVersionString(string version) { ... }
		// public static bool VersionLessThan(VersionInfo smaller , VersionInfo larger) { ... }

		// ────────────────────────────────────────────────────────────────────
		//  Memory Usage
		// ────────────────────────────────────────────────────────────────────

		/// <summary>
		/// Gets memory usage information for the Binary Ninja core engine,
		/// showing memory consumed by each major subsystem.
		/// </summary>
		/// <returns>An array of MemoryUsageInfo entries with name and byte count.</returns>
		public static unsafe MemoryUsageInfo[] GetMemoryUsageInfo()
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr countPtr = allocator.AllocStruct<ulong>(0);

				IntPtr arrayPointer = NativeMethods.BNGetMemoryUsageInfo(countPtr);

				ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

				MemoryUsageInfo[] results = UnsafeUtils.ReadStructArray<BNMemoryUsageInfo , MemoryUsageInfo>(
					arrayPointer ,
					arrayLength ,
					(BNMemoryUsageInfo native) => new MemoryUsageInfo()
					{
						Name = UnsafeUtils.ReadAnsiString(native.name) ,
						Value = native._value
					}
				);

				if (arrayPointer != IntPtr.Zero)
				{
					NativeMethods.BNFreeMemoryUsageInfo(arrayPointer , arrayLength);
				}

				return results;
			}
		}

		// ────────────────────────────────────────────────────────────────────
		//  Token Type Classification
		// ────────────────────────────────────────────────────────────────────

		/// <summary>
		/// Checks whether the given instruction text token type represents an integer value.
		/// </summary>
		/// <param name="type">The token type to check.</param>
		/// <returns>True if the token type is an integer token.</returns>
		public static bool IsIntegerToken(InstructionTextTokenType type)
		{
			return NativeMethods.BNIsIntegerToken(type);
		}

		// ────────────────────────────────────────────────────────────────────
		//  Database / Archive Detection
		// ────────────────────────────────────────────────────────────────────

		/// <summary>
		/// Checks whether the given raw byte data represents a Binary Ninja database file.
		/// </summary>
		/// <param name="data">The raw byte array to inspect.</param>
		/// <returns>True if the data looks like a Binary Ninja database.</returns>
		public static unsafe bool IsDatabaseFromData(byte[] data)
		{
			// 1. Reject null or empty input.
			if (data == null || data.Length == 0)
			{
				return false;
			}

			// 2. Pin the byte array and pass the pointer and length to the native API.
			fixed (byte* pinned = data)
			{
				return NativeMethods.BNIsDatabaseFromData(
					(IntPtr)pinned ,
					(ulong)data.Length
				);
			}
		}

		/// <summary>
		/// Checks whether the file at the given path is a Binary Ninja type archive.
		/// </summary>
		/// <param name="path">The filesystem path to check.</param>
		/// <returns>True if the file is a type archive.</returns>
		public static bool IsTypeArchive(string path)
		{
			// Delegate to the native detection function.
			return NativeMethods.BNIsTypeArchive(path);
		}

		/// <summary>
		/// Checks whether a Binary Ninja update installation is pending.
		/// Returns true if an update has been downloaded but not yet installed.
		/// </summary>
		/// <returns>True if an update installation is pending.</returns>
		public static bool IsUpdateInstallationPending()
		{
			// Delegate to the native check function.
			return NativeMethods.BNIsUpdateInstallationPending();
		}

		/// <summary>
		/// Parses a version string into a VersionInfo object.
		/// The version string should be in the form "major.minor.build".
		/// </summary>
		/// <param name="version">The version string to parse.</param>
		/// <returns>A VersionInfo populated from the parsed string.</returns>
		public static VersionInfo ParseVersionString(string version)
		{
			// ABI 176 returns the parsed version by value.
			return VersionInfo.FromNative(
				NativeMethods.BNParseVersionString(version ?? string.Empty)
			);
		}

		/// <summary>
		/// Compares two VersionInfo objects and determines if the first is less than the second.
		/// </summary>
		/// <param name="smaller">The version to test as the smaller value.</param>
		/// <param name="larger">The version to test as the larger value.</param>
		/// <returns>True if smaller is strictly less than larger.</returns>
		public static bool VersionLessThan(VersionInfo smaller , VersionInfo larger)
		{
			// Delegate to the native comparison function.
			return NativeMethods.BNVersionLessThan(smaller , larger);
		}

		/// <summary>
		/// Executes a worker process with the given arguments, input data, and captures stdout/stderr.
		/// </summary>
		/// <param name="path">The filesystem path to the executable.</param>
		/// <param name="args">Command-line arguments to pass to the process.</param>
		/// <param name="input">A DataBuffer containing stdin data to send, or null for no input.</param>
		/// <param name="stdoutIsText">True if stdout should be captured as text.</param>
		/// <param name="stderrIsText">True if stderr should be captured as text.</param>
		/// <param name="stdout">Receives the stdout output.</param>
		/// <param name="stderr">Receives the stderr output.</param>
		/// <returns>True if the process executed successfully.</returns>
		/// <summary>
		/// Displays a form input dialog to the user with the specified fields.
		/// The fields array is modified in-place with user-provided values.
		/// </summary>
		/// <param name="fields">An array of BNFormInputField structs describing the form fields.
		/// On return, the fields contain the user-entered values.</param>
		/// <param name="title">The title string for the dialog.</param>
		/// <returns>True if the user accepted the dialog; false if canceled.</returns>
		public static bool GetFormInput(BNFormInputField[] fields , string title)
		{
			// Forward the field array, count, and title to the native API.
			return NativeMethods.BNGetFormInput(
				fields ,
				(ulong)fields.Length ,
				title ?? string.Empty
			);
		}

		public static bool ExecuteWorkerProcess(
			string path ,
			string[] args ,
			DataBuffer? input ,
			bool stdoutIsText ,
			bool stderrIsText ,
			out string stdout ,
			out string stderr
		)
		{
			// 1. Get the input handle.
			IntPtr inputHandle = (null != input) ? input.DangerousGetHandle() : IntPtr.Zero;

			// 2. Call the native API. output/error are each an out char* the core
			//    allocates; args is left as the existing null-terminated string[]
			//    marshaling (the core takes no arg count, so widening it is out of scope).
			IntPtr outputPointer;
			IntPtr errorPointer;
			bool ok = NativeMethods.BNExecuteWorkerProcess(
				path ,
				args ?? Array.Empty<string>() ,
				inputHandle ,
				out outputPointer ,
				out errorPointer ,
				stdoutIsText ,
				stderrIsText
			);

			// 3. Decode + free the core-allocated output strings (no-op on null).
			stdout = UnsafeUtils.TakeUtf8String(outputPointer);
			stderr = UnsafeUtils.TakeUtf8String(errorPointer);

			return ok;
		}

		// ────────────────────────────────────────────────────────────────────
		//  LLVM Services
		// ────────────────────────────────────────────────────────────────────

		/// <summary>
		/// Assembles source text using the LLVM assembler backend.
		/// </summary>
		/// <param name="source">The assembly source text to assemble.</param>
		/// <param name="dialect">The assembly dialect (0 = AT&amp;T, 1 = Intel).</param>
		/// <param name="triplet">The LLVM target triple string (e.g., "x86_64-unknown-linux-gnu").</param>
		/// <param name="codeModel">The code model to use for assembly.</param>
		/// <param name="relocMode">The relocation mode to use.</param>
		/// <param name="assembledBytes">Receives the assembled bytes as a string on success.</param>
		/// <param name="errorMessage">Receives any error message from the assembler.</param>
		/// <returns>The assembler result code (0 = success).</returns>
		public static unsafe int LlvmAssemble(
			string source ,
			int dialect ,
			string triplet ,
			int codeModel ,
			int relocMode ,
			out byte[] assembledBytes ,
			out string errorMessage
		)
		{
			// 1. Initialize the LLVM assembler backend (idempotent in the core).
			NativeMethods.BNLlvmServicesInit();

			// 2. Call the native assembler. outBytes is a length-delimited machine-code
			//    buffer (NOT a null-terminated string); err is an error string. Both are
			//    core-allocated and freed together by BNLlvmServicesAssembleFree.
			int outLen = 0;
			int errLen = 0;
			IntPtr outBytesPointer;
			IntPtr errPointer;
			int result = NativeMethods.BNLlvmServicesAssemble(
				source ,
				dialect ,
				triplet ,
				codeModel ,
				relocMode ,
				out outBytesPointer ,
				(IntPtr)(&outLen) ,
				out errPointer ,
				(IntPtr)(&errLen)
			);

			// 3. Copy the assembled bytes out as raw binary before freeing.
			if (IntPtr.Zero != outBytesPointer && outLen > 0)
			{
				assembledBytes = new byte[outLen];
				Marshal.Copy(outBytesPointer , assembledBytes , 0 , outLen);
			}
			else
			{
				assembledBytes = Array.Empty<byte>();
			}

			// 4. Decode the error string (UTF-8, null-terminated).
			errorMessage = UnsafeUtils.ReadUtf8String(errPointer);

			// 5. Free both core buffers in a single call, matching the C++ SDK.
			NativeMethods.BNLlvmServicesAssembleFree(outBytesPointer , errPointer);

			return result;
		}

		// ────────────────────────────────────────────────────────────────────
		//  Source Preprocessing
		// ────────────────────────────────────────────────────────────────────

		/// <summary>
		/// Preprocesses C/C++ source text through the built-in preprocessor.
		/// Expands macros and include directives, producing preprocessed output.
		/// </summary>
		/// <param name="source">The source text to preprocess.</param>
		/// <param name="fileName">The filename to use for diagnostics.</param>
		/// <param name="includeDirs">An array of include directory paths.</param>
		/// <param name="output">Receives the preprocessed source text on success.</param>
		/// <param name="error">Receives any error messages from the preprocessor.</param>
		/// <returns>True if preprocessing succeeded.</returns>
		public static bool PreprocessSource(
			string source ,
			string fileName ,
			string[] includeDirs ,
			out string output ,
			out string error
		)
		{
			// 1. Normalize the include-dir input.
			string[] safeDirs = includeDirs ?? Array.Empty<string>();

			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				// 2. Build the const char** include-dir block as UTF-8.
				IntPtr includeDirsBlock = allocator.AllocUtf8StringArray(safeDirs);

				// 3. Call the native preprocessor. `output` and `errors` are each an
				//    out char*: the core allocates a single string into each.
				IntPtr outputPointer;
				IntPtr errorsPointer;
				bool ok = NativeMethods.BNPreprocessSource(
					source ,
					fileName ,
					out outputPointer ,
					out errorsPointer ,
					includeDirsBlock ,
					(ulong)safeDirs.Length
				);

				// 4. Decode + free the core-allocated strings (no-op on null).
				output = UnsafeUtils.TakeUtf8String(outputPointer);
				error = UnsafeUtils.TakeUtf8String(errorsPointer);

				return ok;
			}
		}

		// ────────────────────────────────────────────────────────────────────
		//  Type Parser Options
		// ────────────────────────────────────────────────────────────────────

		/// <summary>
		/// Parses a text string containing type parser options into an array of individual option strings.
		/// The input text is split according to the type parser option syntax rules.
		/// </summary>
		/// <param name="optionsText">The options text to parse.</param>
		/// <returns>An array of parsed option strings.</returns>
		public static unsafe string[] ParseTypeParserOptionsText(string optionsText)
		{
			// 1. Stack-allocate the count variable.
			ulong count = 0;

			// 2. Call the native parser to split the options text.
			IntPtr ptr = NativeMethods.BNParseTypeParserOptionsText(
				optionsText ,
				(IntPtr)(&count)
			);

			// 3. Return empty if no results.
			if (0 == count || IntPtr.Zero == ptr)
			{
				return Array.Empty<string>();
			}

			// 4. Copy and free the string array.
			return UnsafeUtils.TakeAnsiStringArray(ptr , count , NativeMethods.BNFreeStringList);
		}

		// TODO: BNRunProgressDialog — requires callback delegate (void** task, void* taskCtxt).
		//       Needs managed delegate infrastructure for the progress callback.

		// TODO: BNRenderLinesForData / BNGetLinesForData — complex callback-based renderer
		//       with custom data renderer context callback parameters.

		// TODO: BNAppendSymbolQueue / BNProcessSymbolQueue — internal callback-based
		//       symbol resolution pipeline. Requires managed delegate infrastructure.

		// TODO: BNPerformCustomRequest / BNPerformDownloadRequest — on DownloadInstance,
		//       requires callback context parameters for progress/completion notifications.

		// TODO: BNPerformSearch — on BinaryView, requires callback context for search results.

		// TODO: BNTypeArchiveMergeSnapshots / BNTypeArchiveNewSnapshotTransaction —
		//       callback-based transaction functions requiring managed delegate infrastructure.

		// NOTE: BNReadSnapshotDataWithProgress — implemented on Snapshot class as ReadDataWithProgress().

		// TODO: BNResolveStructureMemberOrBaseMember — on Structure, requires callback
		//       context (void* callbackContext, void** resolveFunc) for member resolution.

		// TODO: BNComponentsNotEqual — skip, handled by AbstractSafeHandle operators (== and !=).

		// TODO: BNRepositoryFreePluginDirectoryList — internal free function, not a wrapper target.

		// TODO: BNUnicodeGetBlocksForNames / BNUnicodeGetUTF8String / BNUnicodeToEscapedString —
		//       Complex multi-level pointer parameters (uint32_t***, uint64_t**) requiring
		//       specialized marshalling infrastructure. Implement when Unicode block support is needed.
	}
}
