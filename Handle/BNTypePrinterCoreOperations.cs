using System;

namespace BinaryNinja
{
	public abstract partial class TypePrinter
	{
		private unsafe TypeDefinitionLine[] GetTypeLinesCore(
			BinaryNinja.Type type,
			TypeContainer types,
			QualifiedName name,
			int paddingCols,
			bool collapsed,
			TokenEscapingType escaping
		)
		{
			if (null == type)
			{
				throw new ArgumentNullException(nameof(type));
			}

			if (null == types)
			{
				throw new ArgumentNullException(nameof(types));
			}

			IntPtr resultPointer = IntPtr.Zero;
			ulong resultCount = 0;
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				bool ok = NativeMethods.BNGetTypePrinterTypeLines(
					this.handle,
					type.DangerousGetHandle(),
					types.DangerousGetHandle(),
					allocator.AllocStruct<BNQualifiedName>(name.ToNativeEx(allocator)),
					paddingCols,
					collapsed,
					escaping,
					(IntPtr)(&resultPointer),
					(IntPtr)(&resultCount)
				);
				if (!ok)
				{
					return Array.Empty<TypeDefinitionLine>();
				}

				return UnsafeUtils.TakeStructArrayEx<BNTypeDefinitionLine, TypeDefinitionLine>(
					resultPointer,
					resultCount,
					TypeDefinitionLine.FromNative,
					NativeMethods.BNFreeTypeDefinitionLineList
				);
			}
		}

		private string? GetTypeStringCore(
			BinaryNinja.Type type,
			Platform? platform,
			QualifiedName name,
			TokenEscapingType escaping
		)
		{
			if (null == type)
			{
				throw new ArgumentNullException(nameof(type));
			}

			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr resultPointer;
				bool ok = NativeMethods.BNGetTypePrinterTypeString(
					this.handle,
					type.DangerousGetHandle(),
					null == platform ? IntPtr.Zero : platform.DangerousGetHandle(),
					allocator.AllocStruct<BNQualifiedName>(name.ToNativeEx(allocator)),
					escaping,
					out resultPointer
				);
				if (!ok)
				{
					return null;
				}

				return UnsafeUtils.TakeUtf8String(resultPointer);
			}
		}

		private string? GetTypeStringAfterNameCore(
			BinaryNinja.Type type,
			Platform? platform,
			TokenEscapingType escaping
		)
		{
			if (null == type)
			{
				throw new ArgumentNullException(nameof(type));
			}

			IntPtr resultPointer;
			bool ok = NativeMethods.BNGetTypePrinterTypeStringAfterName(
				this.handle,
				type.DangerousGetHandle(),
				null == platform ? IntPtr.Zero : platform.DangerousGetHandle(),
				escaping,
				out resultPointer
			);
			if (!ok)
			{
				return null;
			}

			return UnsafeUtils.TakeUtf8String(resultPointer);
		}

		private string? GetTypeStringBeforeNameCore(
			BinaryNinja.Type type,
			Platform? platform,
			TokenEscapingType escaping
		)
		{
			if (null == type)
			{
				throw new ArgumentNullException(nameof(type));
			}

			IntPtr resultPointer;
			bool ok = NativeMethods.BNGetTypePrinterTypeStringBeforeName(
				this.handle,
				type.DangerousGetHandle(),
				null == platform ? IntPtr.Zero : platform.DangerousGetHandle(),
				escaping,
				out resultPointer
			);
			if (!ok)
			{
				return null;
			}

			return UnsafeUtils.TakeUtf8String(resultPointer);
		}

		private unsafe InstructionTextToken[] GetTypeTokensCore(
			BinaryNinja.Type type,
			Platform? platform,
			QualifiedName name,
			byte baseConfidence,
			TokenEscapingType escaping
		)
		{
			if (null == type)
			{
				throw new ArgumentNullException(nameof(type));
			}

			IntPtr resultPointer = IntPtr.Zero;
			ulong resultCount = 0;
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				bool ok = NativeMethods.BNGetTypePrinterTypeTokens(
					this.handle,
					type.DangerousGetHandle(),
					null == platform ? IntPtr.Zero : platform.DangerousGetHandle(),
					allocator.AllocStruct<BNQualifiedName>(name.ToNativeEx(allocator)),
					baseConfidence,
					escaping,
					(IntPtr)(&resultPointer),
					(IntPtr)(&resultCount)
				);
				if (!ok)
				{
					return Array.Empty<InstructionTextToken>();
				}

				return UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken, InstructionTextToken>(
					resultPointer,
					resultCount,
					InstructionTextToken.FromNative,
					NativeMethods.BNFreeInstructionText
				);
			}
		}

		private unsafe InstructionTextToken[] GetTypeTokensAfterNameCore(
			BinaryNinja.Type type,
			Platform? platform,
			byte baseConfidence,
			BinaryNinja.Type? parentType,
			TokenEscapingType escaping
		)
		{
			if (null == type)
			{
				throw new ArgumentNullException(nameof(type));
			}

			IntPtr resultPointer = IntPtr.Zero;
			ulong resultCount = 0;
			bool ok = NativeMethods.BNGetTypePrinterTypeTokensAfterName(
				this.handle,
				type.DangerousGetHandle(),
				null == platform ? IntPtr.Zero : platform.DangerousGetHandle(),
				baseConfidence,
				null == parentType ? IntPtr.Zero : parentType.DangerousGetHandle(),
				escaping,
				(IntPtr)(&resultPointer),
				(IntPtr)(&resultCount)
			);
			if (!ok)
			{
				return Array.Empty<InstructionTextToken>();
			}

			return UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken, InstructionTextToken>(
				resultPointer,
				resultCount,
				InstructionTextToken.FromNative,
				NativeMethods.BNFreeInstructionText
			);
		}

		private unsafe InstructionTextToken[] GetTypeTokensBeforeNameCore(
			BinaryNinja.Type type,
			Platform? platform,
			byte baseConfidence,
			BinaryNinja.Type? parentType,
			TokenEscapingType escaping
		)
		{
			if (null == type)
			{
				throw new ArgumentNullException(nameof(type));
			}

			IntPtr resultPointer = IntPtr.Zero;
			ulong resultCount = 0;
			bool ok = NativeMethods.BNGetTypePrinterTypeTokensBeforeName(
				this.handle,
				type.DangerousGetHandle(),
				null == platform ? IntPtr.Zero : platform.DangerousGetHandle(),
				baseConfidence,
				null == parentType ? IntPtr.Zero : parentType.DangerousGetHandle(),
				escaping,
				(IntPtr)(&resultPointer),
				(IntPtr)(&resultCount)
			);
			if (!ok)
			{
				return Array.Empty<InstructionTextToken>();
			}

			return UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken, InstructionTextToken>(
				resultPointer,
				resultCount,
				InstructionTextToken.FromNative,
				NativeMethods.BNFreeInstructionText
			);
		}

		private string? PrintAllTypesCore(
			QualifiedName[] names,
			BinaryNinja.Type[] types,
			BinaryView data,
			int paddingCols,
			TokenEscapingType escaping
		)
		{
			TypePrinter.ValidatePrintAllTypesArguments(names, types, data);
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr namesArray = TypePrinter.AllocateQualifiedNames(names, allocator);
				IntPtr typesArray = TypePrinter.AllocateTypeHandles(types, allocator);
				IntPtr resultPointer;
				bool ok = NativeMethods.BNTypePrinterPrintAllTypes(
					this.handle,
					namesArray,
					typesArray,
					(ulong)types.Length,
					data.DangerousGetHandle(),
					paddingCols,
					escaping,
					out resultPointer
				);
				if (!ok)
				{
					return null;
				}

				return UnsafeUtils.TakeUtf8String(resultPointer);
			}
		}
	}
}
