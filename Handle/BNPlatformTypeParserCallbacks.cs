using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public partial class Platform
	{
		private void InvokeAdjustTypeParserInput(
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
		)
		{
			Platform.ResetTypeParserOutput(
				argumentsOut,
				argumentsLengthOut,
				sourceFileNamesOut,
				sourceFileValuesOut,
				sourceFilesLengthOut
			);
			IntPtr nativeArguments = IntPtr.Zero;
			IntPtr nativeSourceNames = IntPtr.Zero;
			IntPtr nativeSourceValues = IntPtr.Zero;
			try
			{
				string[] inputArguments = UnsafeUtils.ReadAnsiStringArray(
					argumentsIn, argumentsLengthIn
				);
				string[] inputSourceNames = UnsafeUtils.ReadAnsiStringArray(
					sourceFileNamesIn, sourceFilesLengthIn
				);
				string[] inputSourceValues = UnsafeUtils.ReadAnsiStringArray(
					sourceFileValuesIn, sourceFilesLengthIn
				);
				List<string> arguments = new List<string>(inputArguments);
				List<PlatformTypeParserSourceFile> sourceFiles =
					new List<PlatformTypeParserSourceFile>();
				for (int i = 0; i < inputSourceNames.Length; i++)
				{
					sourceFiles.Add(new PlatformTypeParserSourceFile(
						inputSourceNames[i], inputSourceValues[i]
					));
				}

				TypeParser managedParser = TypeParser.MustBorrowHandle(parser);
				this.AdjustTypeParserInput(managedParser, arguments, sourceFiles);
				string[] outputArguments = arguments.ToArray();
				string[] outputSourceNames = new string[sourceFiles.Count];
				string[] outputSourceValues = new string[sourceFiles.Count];
				for (int i = 0; i < sourceFiles.Count; i++)
				{
					outputSourceNames[i] = sourceFiles[i].Name;
					outputSourceValues[i] = sourceFiles[i].Contents;
				}

				Marshal.WriteInt64(argumentsLengthOut, outputArguments.Length);
				Marshal.WriteInt64(sourceFilesLengthOut, sourceFiles.Count);
				nativeArguments = Platform.AllocateStringList(outputArguments);
				nativeSourceNames = Platform.AllocateStringList(outputSourceNames);
				nativeSourceValues = Platform.AllocateStringList(outputSourceValues);
				Marshal.WriteIntPtr(argumentsOut, nativeArguments);
				Marshal.WriteIntPtr(sourceFileNamesOut, nativeSourceNames);
				Marshal.WriteIntPtr(sourceFileValuesOut, nativeSourceValues);
			}
			catch (Exception exception)
			{
				Platform.FreeStringList(nativeArguments, argumentsLengthOut);
				Platform.FreeStringList(nativeSourceNames, sourceFilesLengthOut);
				Platform.FreeStringList(nativeSourceValues, sourceFilesLengthOut);
				Platform.ResetTypeParserOutput(
					argumentsOut,
					argumentsLengthOut,
					sourceFileNamesOut,
					sourceFileValuesOut,
					sourceFilesLengthOut
				);
				Core.LogError(
					"Unhandled exception in Platform.AdjustTypeParserInput: {0}",
					exception
				);
			}
		}

		private void InvokeFreeTypeParserInput(
			IntPtr context,
			IntPtr arguments,
			ulong argumentsLength,
			IntPtr sourceFileNames,
			IntPtr sourceFileValues,
			ulong sourceFilesLength
		)
		{
			NativeMethods.BNFreeStringList(arguments, argumentsLength);
			NativeMethods.BNFreeStringList(sourceFileNames, sourceFilesLength);
			NativeMethods.BNFreeStringList(sourceFileValues, sourceFilesLength);
		}

		private static IntPtr AllocateStringList(string[] values)
		{
			if (0 == values.Length)
			{
				return IntPtr.Zero;
			}

			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr pointers = allocator.AllocUtf8StringArray(values);

				return NativeMethods.BNAllocStringList(pointers, (ulong)values.Length);
			}
		}

		private static void FreeStringList(IntPtr values, IntPtr countPointer)
		{
			if (IntPtr.Zero == values)
			{
				return;
			}

			ulong count = unchecked((ulong)Marshal.ReadInt64(countPointer));
			NativeMethods.BNFreeStringList(values, count);
		}

		private static void ResetTypeParserOutput(
			IntPtr argumentsOut,
			IntPtr argumentsLengthOut,
			IntPtr sourceFileNamesOut,
			IntPtr sourceFileValuesOut,
			IntPtr sourceFilesLengthOut
		)
		{
			Marshal.WriteIntPtr(argumentsOut, IntPtr.Zero);
			Marshal.WriteInt64(argumentsLengthOut, 0);
			Marshal.WriteIntPtr(sourceFileNamesOut, IntPtr.Zero);
			Marshal.WriteIntPtr(sourceFileValuesOut, IntPtr.Zero);
			Marshal.WriteInt64(sourceFilesLengthOut, 0);
		}
	}
}
