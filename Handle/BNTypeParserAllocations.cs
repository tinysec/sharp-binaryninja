using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public abstract partial class TypeParser
	{
		private static BNTypeParserResult AllocateResult(TypeParserResult result)
		{
			BNTypeParserResult native = new BNTypeParserResult();
			try
			{
				ParsedType[] types = result.Types ?? Array.Empty<ParsedType>();
				ParsedType[] variables = result.Variables ?? Array.Empty<ParsedType>();
				ParsedType[] functions = result.Functions ?? Array.Empty<ParsedType>();

				native.types = TypeParser.AllocateParsedTypes(types);
				native.typeCount = (ulong)types.Length;
				native.variables = TypeParser.AllocateParsedTypes(variables);
				native.variableCount = (ulong)variables.Length;
				native.functions = TypeParser.AllocateParsedTypes(functions);
				native.functionCount = (ulong)functions.Length;

				return native;
			}
			catch
			{
				TypeParser.FreeResult(native);
				throw;
			}
		}

		private static IntPtr AllocateParsedTypes(ParsedType[] values)
		{
			if (0 == values.Length)
			{
				return IntPtr.Zero;
			}

			int itemSize = Marshal.SizeOf<BNParsedType>();
			IntPtr result = Marshal.AllocHGlobal(itemSize * values.Length);
			int initialized = 0;
			try
			{
				for (int i = 0; i < values.Length; i++)
				{
					ParsedType value = values[i];
					if (null == value || null == value.Type)
					{
						throw new ArgumentException(
							"Parsed type entries and their types cannot be null.",
							nameof(values)
						);
					}

					BNParsedType native = new BNParsedType();
					native.name = TypeParser.AllocateQualifiedName(value.Name);
					try
					{
						native.type = NativeMethods.BNNewTypeReference(
							value.Type.DangerousGetHandle()
						);
						if (IntPtr.Zero == native.type)
						{
							throw new InvalidOperationException(
								"The core could not retain a parsed type."
							);
						}

						native.isUser = value.IsUser;
						Marshal.StructureToPtr(
							native,
							IntPtr.Add(result, i * itemSize),
							false
						);
						initialized++;
					}
					catch
					{
						TypeParser.FreeQualifiedName(native.name);
						if (IntPtr.Zero != native.type)
						{
							NativeMethods.BNFreeType(native.type);
						}

						throw;
					}
				}

				return result;
			}
			catch
			{
				TypeParser.FreeParsedTypes(result, (ulong)initialized);
				throw;
			}
		}

		private static BNQualifiedName AllocateQualifiedName(QualifiedName name)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			string[] components = name.Name ?? Array.Empty<string>();
			BNQualifiedName native = new BNQualifiedName();
			try
			{
				native.nameCount = (ulong)components.Length;
				if (0 != components.Length)
				{
					using (ScopedAllocator allocator = new ScopedAllocator())
					{
						IntPtr input = allocator.AllocUtf8StringArray(components);
						native.name = NativeMethods.BNAllocStringList(
							input,
							(ulong)components.Length
						);
						if (IntPtr.Zero == native.name)
						{
							throw new InvalidOperationException(
								"The core could not allocate a qualified name."
							);
						}
					}
				}

				native.join = NativeMethods.BNAllocString(name.Join ?? string.Empty);
				if (IntPtr.Zero == native.join)
				{
					throw new InvalidOperationException(
						"The core could not allocate a qualified-name join string."
					);
				}

				return native;
			}
			catch
			{
				TypeParser.FreeQualifiedName(native);
				throw;
			}
		}

		private static BNQualifiedNameAndType AllocateQualifiedNameAndType(
			QualifiedNameAndType value
		)
		{
			BNQualifiedNameAndType native = new BNQualifiedNameAndType();
			try
			{
				native.name = TypeParser.AllocateQualifiedName(value.Name);
				native.type = NativeMethods.BNNewTypeReference(
					value.Type.DangerousGetHandle()
				);
				if (IntPtr.Zero == native.type)
				{
					throw new InvalidOperationException(
						"The core could not retain a parsed type."
					);
				}

				return native;
			}
			catch
			{
				TypeParser.FreeQualifiedNameAndType(native);
				throw;
			}
		}

		private static IntPtr AllocateErrors(TypeParserError[]? errors)
		{
			TypeParserError[] safeErrors = errors ?? Array.Empty<TypeParserError>();
			if (0 == safeErrors.Length)
			{
				return IntPtr.Zero;
			}

			int itemSize = Marshal.SizeOf<BNTypeParserError>();
			IntPtr result = Marshal.AllocHGlobal(itemSize * safeErrors.Length);
			int initialized = 0;
			try
			{
				for (int i = 0; i < safeErrors.Length; i++)
				{
					TypeParserError error = safeErrors[i];
					if (null == error)
					{
						throw new ArgumentException(
							"Parser error entries cannot be null.",
							nameof(errors)
						);
					}

					BNTypeParserError native = new BNTypeParserError();
					native.severity = error.Severity;
					native.message = NativeMethods.BNAllocString(error.Message);
					if (IntPtr.Zero == native.message)
					{
						throw new InvalidOperationException(
							"The core could not allocate a parser error message."
						);
					}

					try
					{
						native.fileName = NativeMethods.BNAllocString(error.FileName);
						if (IntPtr.Zero == native.fileName)
						{
							throw new InvalidOperationException(
								"The core could not allocate a parser error file name."
							);
						}

						native.line = error.Line;
						native.column = error.Column;
						Marshal.StructureToPtr(
							native,
							IntPtr.Add(result, i * itemSize),
							false
						);
						initialized++;
					}
					catch
					{
						NativeMethods.BNFreeString(native.message);
						if (IntPtr.Zero != native.fileName)
						{
							NativeMethods.BNFreeString(native.fileName);
						}

						throw;
					}
				}

				return result;
			}
			catch
			{
				TypeParser.FreeErrors(result, (ulong)initialized);
				throw;
			}
		}

		private static void FreeResult(BNTypeParserResult result)
		{
			TypeParser.FreeParsedTypes(result.types, result.typeCount);
			TypeParser.FreeParsedTypes(result.variables, result.variableCount);
			TypeParser.FreeParsedTypes(result.functions, result.functionCount);
		}

		private static void FreeParsedTypes(IntPtr values, ulong count)
		{
			if (IntPtr.Zero == values)
			{
				return;
			}

			int itemSize = Marshal.SizeOf<BNParsedType>();
			for (ulong i = 0; i < count; i++)
			{
				BNParsedType value = Marshal.PtrToStructure<BNParsedType>(
					IntPtr.Add(values, checked((int)i) * itemSize)
				);
				TypeParser.FreeQualifiedName(value.name);
				if (IntPtr.Zero != value.type)
				{
					NativeMethods.BNFreeType(value.type);
				}
			}

			Marshal.FreeHGlobal(values);
		}

		private static void FreeQualifiedNameAndType(BNQualifiedNameAndType value)
		{
			TypeParser.FreeQualifiedName(value.name);
			if (IntPtr.Zero != value.type)
			{
				NativeMethods.BNFreeType(value.type);
			}
		}

		private static void FreeQualifiedName(BNQualifiedName name)
		{
			if (IntPtr.Zero != name.name)
			{
				NativeMethods.BNFreeStringList(name.name, name.nameCount);
			}

			if (IntPtr.Zero != name.join)
			{
				NativeMethods.BNFreeString(name.join);
			}
		}

		private static void FreeErrors(IntPtr errors, ulong count)
		{
			if (IntPtr.Zero == errors)
			{
				return;
			}

			int itemSize = Marshal.SizeOf<BNTypeParserError>();
			for (ulong i = 0; i < count; i++)
			{
				BNTypeParserError error = Marshal.PtrToStructure<BNTypeParserError>(
					IntPtr.Add(errors, checked((int)i) * itemSize)
				);
				NativeMethods.BNFreeString(error.message);
				NativeMethods.BNFreeString(error.fileName);
			}

			Marshal.FreeHGlobal(errors);
		}

		private static void ResetErrorOutput(IntPtr errors, IntPtr errorCount)
		{
			Marshal.WriteIntPtr(errors, IntPtr.Zero);
			Marshal.WriteInt64(errorCount, 0);
		}

		private static void WriteErrorOutput(
			IntPtr errors,
			IntPtr errorCount,
			IntPtr nativeErrors,
			ulong count
		)
		{
			Marshal.WriteIntPtr(errors, nativeErrors);
			Marshal.WriteInt64(errorCount, unchecked((long)count));
		}
	}
}
