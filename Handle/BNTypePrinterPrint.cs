using System;

namespace BinaryNinja
{
	public abstract partial class TypePrinter
	{
		/// <summary>
		/// Prints all provided types using the default implementation of this printer.
		/// This is the base (non-overridden) version of PrintAllTypes.
		/// </summary>
		/// <param name="printer">The printer whose default implementation to use.</param>
		/// <param name="names">An array of qualified names for the types.</param>
		/// <param name="types">An array of types to print.</param>
		/// <param name="data">The binary view for context.</param>
		/// <param name="paddingCols">Number of padding columns for indentation.</param>
		/// <param name="escaping">The token escaping mode.</param>
		/// <returns>The printed string, or null on failure.</returns>
		public static string? DefaultPrintAllTypes(
			TypePrinter printer,
			QualifiedName[] names,
			BinaryNinja.Type[] types,
			BinaryView data,
			int paddingCols,
			TokenEscapingType escaping
		)
		{
			if (null == printer)
			{
				throw new ArgumentNullException(nameof(printer));
			}

			TypePrinter.ValidatePrintAllTypesArguments(names, types, data);
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr namesArray = TypePrinter.AllocateQualifiedNames(names, allocator);
				IntPtr typesArray = TypePrinter.AllocateTypeHandles(types, allocator);
				IntPtr resultPointer;
				bool ok = NativeMethods.BNTypePrinterDefaultPrintAllTypes(
					printer.handle,
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

		/// <summary>Prints all provided types using this printer instance.</summary>
		/// <param name="names">An array of qualified names for the types.</param>
		/// <param name="types">An array of types to print.</param>
		/// <param name="data">The binary view for context.</param>
		/// <param name="paddingCols">Number of padding columns for indentation.</param>
		/// <param name="escaping">The token escaping mode.</param>
		/// <returns>The printed string, or null on failure.</returns>
		public virtual string? PrintAllTypes(
			QualifiedName[] names,
			BinaryNinja.Type[] types,
			BinaryView data,
			int paddingCols,
			TokenEscapingType escaping
		)
		{
			return TypePrinter.DefaultPrintAllTypes(
				this, names, types, data, paddingCols, escaping
			);
		}

		private static IntPtr AllocateQualifiedNames(
			QualifiedName[] names,
			ScopedAllocator allocator
		)
		{
			BNQualifiedName[] nativeNames = new BNQualifiedName[names.Length];
			for (int i = 0; i < names.Length; i++)
			{
				nativeNames[i] = names[i].ToNativeEx(allocator);
			}

			return allocator.AllocStructArray<BNQualifiedName>(nativeNames);
		}

		private static IntPtr AllocateTypeHandles(
			BinaryNinja.Type[] types,
			ScopedAllocator allocator
		)
		{
			IntPtr[] typeHandles = new IntPtr[types.Length];
			for (int i = 0; i < types.Length; i++)
			{
				typeHandles[i] = types[i].DangerousGetHandle();
			}

			return allocator.AllocStructArray<IntPtr>(typeHandles);
		}

		private static void ValidatePrintAllTypesArguments(
			QualifiedName[] names,
			BinaryNinja.Type[] types,
			BinaryView data
		)
		{
			if (null == names || null == types)
			{
				throw new ArgumentNullException(null == names ? nameof(names) : nameof(types));
			}

			if (names.Length != types.Length)
			{
				throw new ArgumentException("Type names and values must have the same length.");
			}

			if (null == data)
			{
				throw new ArgumentNullException(nameof(data));
			}
		}
	}
}
