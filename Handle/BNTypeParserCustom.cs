using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public abstract partial class TypeParser
	{
		private static readonly object registrationLock = new object();

		private static readonly List<TypeParser> registeredParsers =
			new List<TypeParser>();

		private bool isRegistered;

		private NativeDelegates.BNTypeParserGetOptionText? getOptionTextCallback;

		private NativeDelegates.BNTypeParserPreprocessSource? preprocessSourceCallback;

		private NativeDelegates.BNTypeParserParseTypesFromSource?
			parseTypesFromSourceCallback;

		private NativeDelegates.BNTypeParserParseTypeString? parseTypeStringCallback;

		private NativeDelegates.BNTypeParserFreeString? freeStringCallback;

		private NativeDelegates.BNTypeParserFreeResult? freeResultCallback;

		private NativeDelegates.BNTypeParserFreeErrorList? freeErrorListCallback;

		/// <summary>Registers this parser and roots its callbacks for core use.</summary>
		public void Register()
		{
			if (this.isRegistered || !this.IsInvalid)
			{
				throw new InvalidOperationException("The type parser is already registered.");
			}

			this.getOptionTextCallback = new NativeDelegates.BNTypeParserGetOptionText(
				this.InvokeGetOptionText
			);
			this.preprocessSourceCallback =
				new NativeDelegates.BNTypeParserPreprocessSource(
					this.InvokePreprocessSource
				);
			this.parseTypesFromSourceCallback =
				new NativeDelegates.BNTypeParserParseTypesFromSource(
					this.InvokeParseTypesFromSource
				);
			this.parseTypeStringCallback =
				new NativeDelegates.BNTypeParserParseTypeString(
					this.InvokeParseTypeString
				);
			this.freeStringCallback = new NativeDelegates.BNTypeParserFreeString(
				this.InvokeFreeString
			);
			this.freeResultCallback = new NativeDelegates.BNTypeParserFreeResult(
				this.InvokeFreeResult
			);
			this.freeErrorListCallback =
				new NativeDelegates.BNTypeParserFreeErrorList(
					this.InvokeFreeErrorList
				);

			BNTypeParserCallbacks callbacks = new BNTypeParserCallbacks();
			callbacks.context = IntPtr.Zero;
			callbacks.getOptionText = Marshal.GetFunctionPointerForDelegate(
				this.getOptionTextCallback
			);
			callbacks.preprocessSource = Marshal.GetFunctionPointerForDelegate(
				this.preprocessSourceCallback
			);
			callbacks.parseTypesFromSource = Marshal.GetFunctionPointerForDelegate(
				this.parseTypesFromSourceCallback
			);
			callbacks.parseTypeString = Marshal.GetFunctionPointerForDelegate(
				this.parseTypeStringCallback
			);
			callbacks.freeString = Marshal.GetFunctionPointerForDelegate(
				this.freeStringCallback
			);
			callbacks.freeResult = Marshal.GetFunctionPointerForDelegate(
				this.freeResultCallback
			);
			callbacks.freeErrorList = Marshal.GetFunctionPointerForDelegate(
				this.freeErrorListCallback
			);

			IntPtr handle = NativeMethods.BNRegisterTypeParser(
				this.registrationName ?? string.Empty,
				in callbacks
			);
			if (IntPtr.Zero == handle)
			{
				throw new InvalidOperationException("The core rejected the type parser.");
			}

			this.SetHandle(handle);
			this.isRegistered = true;
			lock (TypeParser.registrationLock)
			{
				TypeParser.registeredParsers.Add(this);
			}
		}

		private bool InvokeGetOptionText(
			IntPtr context,
			TypeParserOption option,
			string value,
			IntPtr result
		)
		{
			Marshal.WriteIntPtr(result, IntPtr.Zero);
			try
			{
				string? text = this.GetOptionText(option, value);
				if (null == text)
				{
					return false;
				}

				IntPtr nativeText = NativeMethods.BNAllocString(text);
				if (IntPtr.Zero == nativeText)
				{
					throw new InvalidOperationException("The core could not allocate parser text.");
				}

				Marshal.WriteIntPtr(result, nativeText);
				return true;
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in TypeParser.GetOptionText: {0}",
					exception
				);
				return false;
			}
		}

		private bool InvokePreprocessSource(
			IntPtr context,
			string source,
			string fileName,
			IntPtr platform,
			IntPtr existingTypes,
			IntPtr options,
			ulong optionCount,
			IntPtr includeDirs,
			ulong includeDirCount,
			IntPtr output,
			IntPtr errors,
			IntPtr errorCount
		)
		{
			Marshal.WriteIntPtr(output, IntPtr.Zero);
			TypeParser.ResetErrorOutput(errors, errorCount);
			Platform? managedPlatform = null;
			TypeContainer? managedTypes = null;
			IntPtr nativeOutput = IntPtr.Zero;
			IntPtr nativeErrors = IntPtr.Zero;
			ulong nativeErrorCount = 0;
			try
			{
				managedPlatform = Platform.MustNewFromHandle(platform);
				if (IntPtr.Zero != existingTypes)
				{
					managedTypes = TypeContainer.MustDuplicateFromHandle(existingTypes);
				}

				TypeParserError[] managedErrors;
				string? managedOutput = this.PreprocessSource(
					source,
					fileName,
					managedPlatform,
					managedTypes,
					UnsafeUtils.ReadUtf8StringArray(options, optionCount),
					UnsafeUtils.ReadUtf8StringArray(includeDirs, includeDirCount),
					out managedErrors
				);

				TypeParserError[] safeErrors =
					managedErrors ?? Array.Empty<TypeParserError>();
				nativeErrors = TypeParser.AllocateErrors(safeErrors);
				nativeErrorCount = (ulong)safeErrors.Length;
				TypeParser.WriteErrorOutput(
					errors,
					errorCount,
					nativeErrors,
					(ulong)safeErrors.Length
				);
				nativeErrors = IntPtr.Zero;
				nativeErrorCount = 0;

				if (null == managedOutput)
				{
					return false;
				}

				nativeOutput = NativeMethods.BNAllocString(managedOutput);
				if (IntPtr.Zero == nativeOutput)
				{
					throw new InvalidOperationException(
						"The core could not allocate preprocessed source."
					);
				}

				Marshal.WriteIntPtr(output, nativeOutput);
				nativeOutput = IntPtr.Zero;
				return true;
			}
			catch (Exception exception)
			{
				if (IntPtr.Zero != nativeOutput)
				{
					NativeMethods.BNFreeString(nativeOutput);
				}

				if (IntPtr.Zero != nativeErrors)
				{
					TypeParser.FreeErrors(nativeErrors, nativeErrorCount);
				}

				Core.LogError(
					"Unhandled exception in TypeParser.PreprocessSource: {0}",
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

				if (null != managedPlatform)
				{
					managedPlatform.Dispose();
				}
			}
		}

		private bool InvokeParseTypesFromSource(
			IntPtr context,
			string source,
			string fileName,
			IntPtr platform,
			IntPtr existingTypes,
			IntPtr options,
			ulong optionCount,
			IntPtr includeDirs,
			ulong includeDirCount,
			string autoTypeSource,
			IntPtr result,
			IntPtr errors,
			IntPtr errorCount
		)
		{
			BNTypeParserResult nativeResult = new BNTypeParserResult();
			Marshal.StructureToPtr(nativeResult, result, false);
			TypeParser.ResetErrorOutput(errors, errorCount);
			Platform? managedPlatform = null;
			TypeContainer? managedTypes = null;
			IntPtr nativeErrors = IntPtr.Zero;
			ulong nativeErrorCount = 0;
			try
			{
				managedPlatform = Platform.MustNewFromHandle(platform);
				if (IntPtr.Zero != existingTypes)
				{
					managedTypes = TypeContainer.MustDuplicateFromHandle(existingTypes);
				}

				TypeParserError[] managedErrors;
				TypeParserResult? managedResult = this.ParseTypesFromSource(
					source,
					fileName,
					managedPlatform,
					managedTypes,
					UnsafeUtils.ReadUtf8StringArray(options, optionCount),
					UnsafeUtils.ReadUtf8StringArray(includeDirs, includeDirCount),
					autoTypeSource,
					out managedErrors
				);

				if (null != managedResult)
				{
					nativeResult = TypeParser.AllocateResult(managedResult);
					Marshal.StructureToPtr(nativeResult, result, false);
				}

				TypeParserError[] safeErrors =
					managedErrors ?? Array.Empty<TypeParserError>();
				nativeErrors = TypeParser.AllocateErrors(safeErrors);
				nativeErrorCount = (ulong)safeErrors.Length;
				TypeParser.WriteErrorOutput(
					errors,
					errorCount,
					nativeErrors,
					(ulong)safeErrors.Length
				);
				nativeErrors = IntPtr.Zero;
				nativeErrorCount = 0;
				return null != managedResult;
			}
			catch (Exception exception)
			{
				if (IntPtr.Zero != nativeErrors)
				{
					TypeParser.FreeErrors(nativeErrors, nativeErrorCount);
				}

				TypeParser.FreeResult(nativeResult);
				Marshal.StructureToPtr(new BNTypeParserResult(), result, false);
				Core.LogError(
					"Unhandled exception in TypeParser.ParseTypesFromSource: {0}",
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

				if (null != managedPlatform)
				{
					managedPlatform.Dispose();
				}
			}
		}

		private bool InvokeParseTypeString(
			IntPtr context,
			string source,
			IntPtr platform,
			IntPtr existingTypes,
			IntPtr result,
			IntPtr errors,
			IntPtr errorCount
		)
		{
			// The core duplicates callback output before it checks the success flag.
			// Keep a valid sentinel in place so a failed parse remains safe to copy.
			BNQualifiedNameAndType nativeResult = new BNQualifiedNameAndType();
			nativeResult.name = TypeParser.AllocateQualifiedName(
				new QualifiedName(Array.Empty<string>())
			);
			nativeResult.type = NativeMethods.BNCreateVoidType();
			if (IntPtr.Zero == nativeResult.type)
			{
				TypeParser.FreeQualifiedName(nativeResult.name);
				throw new InvalidOperationException(
					"The core could not allocate a parser result sentinel."
				);
			}

			Marshal.StructureToPtr(nativeResult, result, false);
			TypeParser.ResetErrorOutput(errors, errorCount);
			Platform? managedPlatform = null;
			TypeContainer? managedTypes = null;
			IntPtr nativeErrors = IntPtr.Zero;
			ulong nativeErrorCount = 0;
			try
			{
				managedPlatform = Platform.MustNewFromHandle(platform);
				if (IntPtr.Zero != existingTypes)
				{
					managedTypes = TypeContainer.MustDuplicateFromHandle(existingTypes);
				}

				TypeParserError[] managedErrors;
				QualifiedNameAndType? managedResult = this.ParseTypeString(
					source,
					managedPlatform,
					managedTypes,
					out managedErrors
				);
				if (null != managedResult)
				{
					BNQualifiedNameAndType parsedResult =
						TypeParser.AllocateQualifiedNameAndType(managedResult);
					TypeParser.FreeQualifiedNameAndType(nativeResult);
					nativeResult = parsedResult;
					Marshal.StructureToPtr(nativeResult, result, false);
				}

				TypeParserError[] safeErrors =
					managedErrors ?? Array.Empty<TypeParserError>();
				nativeErrors = TypeParser.AllocateErrors(safeErrors);
				nativeErrorCount = (ulong)safeErrors.Length;
				TypeParser.WriteErrorOutput(
					errors,
					errorCount,
					nativeErrors,
					(ulong)safeErrors.Length
				);
				nativeErrors = IntPtr.Zero;
				nativeErrorCount = 0;
				return null != managedResult;
			}
			catch (Exception exception)
			{
				if (IntPtr.Zero != nativeErrors)
				{
					TypeParser.FreeErrors(nativeErrors, nativeErrorCount);
				}

				// Preserve the valid sentinel because the core copies it on failure.
				Marshal.StructureToPtr(nativeResult, result, false);
				Core.LogError(
					"Unhandled exception in TypeParser.ParseTypeString: {0}",
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

				if (null != managedPlatform)
				{
					managedPlatform.Dispose();
				}
			}
		}

		private void InvokeFreeString(IntPtr context, IntPtr value)
		{
			NativeMethods.BNFreeString(value);
		}

		private void InvokeFreeResult(IntPtr context, IntPtr result)
		{
			try
			{
				BNTypeParserResult native =
					Marshal.PtrToStructure<BNTypeParserResult>(result);
				TypeParser.FreeResult(native);
				Marshal.StructureToPtr(new BNTypeParserResult(), result, false);
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in TypeParser.FreeResult: {0}",
					exception
				);
			}
		}

		private void InvokeFreeErrorList(
			IntPtr context,
			IntPtr errors,
			ulong errorCount
		)
		{
			try
			{
				TypeParser.FreeErrors(errors, errorCount);
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in TypeParser.FreeErrorList: {0}",
					exception
				);
			}
		}

	}
}
