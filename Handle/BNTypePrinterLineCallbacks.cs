using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public abstract partial class TypePrinter
	{
		private readonly object lineOutputLock = new object();

		private readonly Dictionary<IntPtr, TypePrinterLineAllocation> lineOutputs =
			new Dictionary<IntPtr, TypePrinterLineAllocation>();

		private bool InvokeGetTypeLines(
			IntPtr context, IntPtr type, IntPtr types, IntPtr name,
			int paddingCols, bool collapsed, TokenEscapingType escaping,
			IntPtr result, IntPtr resultCount
		)
		{
			TypePrinter.ResetListOutput(result, resultCount);
			BinaryNinja.Type? managedType = null;
			TypeContainer? managedTypes = null;
			try
			{
				managedType = BinaryNinja.Type.MustNewFromHandle(type);
				managedTypes = TypeContainer.MustDuplicateFromHandle(types);
				BNQualifiedName nativeName = Marshal.PtrToStructure<BNQualifiedName>(name);
				TypeDefinitionLine[] lines = this.GetTypeLines(
					managedType, managedTypes, QualifiedName.FromNative(nativeName),
					paddingCols, collapsed, escaping
				);
				this.WriteLineOutput(lines, result, resultCount);
				return true;
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in TypePrinter.GetTypeLines: {0}",
					exception
				);
				return false;
			}
			finally
			{
				if (null != managedTypes)
				{
					managedTypes.Dispose();
				}

				if (null != managedType)
				{
					managedType.Dispose();
				}
			}
		}

		private bool InvokePrintAllTypes(
			IntPtr context, IntPtr names, IntPtr types, ulong typeCount,
			IntPtr data, int paddingCols, TokenEscapingType escaping, IntPtr result
		)
		{
			Marshal.WriteIntPtr(result, IntPtr.Zero);
			BinaryView? managedView = null;
			BinaryNinja.Type[] managedTypes = new BinaryNinja.Type[checked((int)typeCount)];
			try
			{
				QualifiedName[] managedNames = new QualifiedName[checked((int)typeCount)];
				int nameSize = Marshal.SizeOf<BNQualifiedName>();
				for (int i = 0; i < managedNames.Length; i++)
				{
					BNQualifiedName nativeName = Marshal.PtrToStructure<BNQualifiedName>(
						IntPtr.Add(names, i * nameSize)
					);
					managedNames[i] = QualifiedName.FromNative(nativeName);
					managedTypes[i] = BinaryNinja.Type.MustNewFromHandle(
						Marshal.ReadIntPtr(types, i * IntPtr.Size)
					);
				}

				managedView = BinaryView.MustTakeHandle(
					NativeMethods.BNNewViewReference(data)
				);
				string? text = this.PrintAllTypes(
					managedNames, managedTypes, managedView, paddingCols, escaping
				);
				this.WriteStringOutput(text, result);
				return null != text;
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in TypePrinter.PrintAllTypes: {0}",
					exception
				);
				return false;
			}
			finally
			{
				if (null != managedView)
				{
					managedView.Dispose();
				}

				foreach (BinaryNinja.Type? managedType in managedTypes)
				{
					if (null != managedType)
					{
						managedType.Dispose();
					}
				}
			}
		}

		private void WriteLineOutput(
			TypeDefinitionLine[]? lines, IntPtr result, IntPtr resultCount
		)
		{
			TypeDefinitionLine[] safeLines = lines ?? Array.Empty<TypeDefinitionLine>();
			TypePrinterLineAllocation allocation = new TypePrinterLineAllocation();
			try
			{
				BNTypeDefinitionLine[] nativeLines =
					new BNTypeDefinitionLine[safeLines.Length];
				for (int i = 0; i < safeLines.Length; i++)
				{
					nativeLines[i] = allocation.AllocateLine(safeLines[i]);
				}

				IntPtr nativeOutput = allocation.Allocator.AllocStructArray(nativeLines);
				if (IntPtr.Zero != nativeOutput)
				{
					lock (this.lineOutputLock)
					{
						this.lineOutputs.Add(nativeOutput, allocation);
					}
				}
				else
				{
					allocation.Dispose();
				}

				Marshal.WriteIntPtr(result, nativeOutput);
				Marshal.WriteInt64(resultCount, safeLines.Length);
			}
			catch
			{
				allocation.Dispose();
				throw;
			}
		}

		private void InvokeFreeLines(IntPtr context, IntPtr values, ulong count)
		{
			TypePrinterLineAllocation? allocation = null;
			lock (this.lineOutputLock)
			{
				if (this.lineOutputs.TryGetValue(values, out allocation))
				{
					this.lineOutputs.Remove(values);
				}
			}

			if (null != allocation)
			{
				allocation.Dispose();
			}
		}
	}

	internal sealed class TypePrinterLineAllocation : IDisposable
	{
		private readonly List<IntPtr> types = new List<IntPtr>();
		private readonly List<IntPtr> namedTypes = new List<IntPtr>();

		internal ScopedAllocator Allocator { get; } = new ScopedAllocator();

		internal BNTypeDefinitionLine AllocateLine(TypeDefinitionLine line)
		{
			if (null == line)
			{
				throw new ArgumentNullException(nameof(line));
			}

			BNInstructionTextToken[] tokens =
				this.Allocator.ConvertToNativeArrayEx<
					BNInstructionTextToken, InstructionTextToken
				>(line.Tokens ?? Array.Empty<InstructionTextToken>());
			return new BNTypeDefinitionLine()
			{
				lineType = line.LineType,
				tokens = this.Allocator.AllocStructArray(tokens),
				count = (ulong)tokens.Length,
				type = this.RetainType(line.Type),
				parentType = this.RetainType(line.ParentType),
				rootType = this.RetainType(line.RootType),
				rootTypeName = string.IsNullOrEmpty(line.RootTypeName)
					? IntPtr.Zero
					: this.Allocator.AllocUtf8String(line.RootTypeName),
				baseType = this.RetainNamedType(line.BaseType),
				baseOffset = line.BaseOffset,
				offset = line.Offset,
				fieldIndex = line.FieldIndex
			};
		}

		public void Dispose()
		{
			foreach (IntPtr type in this.types)
			{
				NativeMethods.BNFreeType(type);
			}

			foreach (IntPtr namedType in this.namedTypes)
			{
				NativeMethods.BNFreeNamedTypeReference(namedType);
			}

			this.types.Clear();
			this.namedTypes.Clear();
			this.Allocator.Dispose();
		}

		private IntPtr RetainType(BinaryNinja.Type? type)
		{
			if (null == type)
			{
				return IntPtr.Zero;
			}

			IntPtr result = NativeMethods.BNNewTypeReference(type.DangerousGetHandle());
			if (IntPtr.Zero == result)
			{
				throw new InvalidOperationException("The core could not retain a line type.");
			}

			this.types.Add(result);
			return result;
		}

		private IntPtr RetainNamedType(NamedTypeReference? type)
		{
			if (null == type)
			{
				return IntPtr.Zero;
			}

			IntPtr result = NativeMethods.BNNewNamedTypeReference(
				type.DangerousGetHandle()
			);
			if (IntPtr.Zero == result)
			{
				throw new InvalidOperationException(
					"The core could not retain a named line type."
				);
			}

			this.namedTypes.Add(result);
			return result;
		}
	}
}
