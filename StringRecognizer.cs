using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class NativeDelegates
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNStringRecognizerIsValidForType(
			IntPtr context,
			IntPtr function,
			IntPtr type
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNStringRecognizerRecognizeConstant(
			IntPtr context,
			IntPtr function,
			UIntPtr expressionIndex,
			IntPtr type,
			long value,
			IntPtr result
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNStringRecognizerRecognizeExternPointer(
			IntPtr context,
			IntPtr function,
			UIntPtr expressionIndex,
			IntPtr type,
			long value,
			ulong offset,
			IntPtr result
		);
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct BNCustomStringRecognizer
	{
		internal IntPtr context;

		internal IntPtr isValidForType;

		internal IntPtr recognizeConstant;

		internal IntPtr recognizeConstantPointer;

		internal IntPtr recognizeExternPointer;

		internal IntPtr recognizeImport;
	}

	/// <summary>
	/// Recognizes derived strings represented by high-level IL constants and pointer expressions.
	/// </summary>
	public class StringRecognizer : AbstractSafeHandle<StringRecognizer>
	{
		private enum RecognitionKind
		{
			Constant,
			ConstantPointer,
			ExternPointer,
			Import
		}

		private static readonly object registrationLock = new object();

		private static readonly List<StringRecognizer> registeredRecognizers =
			new List<StringRecognizer>();

		private readonly string? registrationName;

		private bool isRegistered;

		private NativeDelegates.BNStringRecognizerIsValidForType? isValidForTypeCallback;

		private NativeDelegates.BNStringRecognizerRecognizeConstant? recognizeConstantCallback;

		private NativeDelegates.BNStringRecognizerRecognizeConstant? recognizeConstantPointerCallback;

		private NativeDelegates.BNStringRecognizerRecognizeExternPointer? recognizeExternPointerCallback;

		private NativeDelegates.BNStringRecognizerRecognizeConstant? recognizeImportCallback;

		/// <summary>Creates an unregistered custom string recognizer.</summary>
		protected StringRecognizer(string name)
			: base(false)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			this.registrationName = name;
		}

		private StringRecognizer(IntPtr handle)
			: base(handle, false)
		{
		}

		/// <summary>Gets the registered recognizer name.</summary>
		public string Name
		{
			get
			{
				if (this.IsInvalid)
				{
					return this.registrationName ?? string.Empty;
				}

				return UnsafeUtils.TakeUtf8String(
					NativeMethods.BNGetStringRecognizerName(this.handle)
				);
			}
		}

		/// <summary>Registers this custom recognizer and roots its callbacks for core use.</summary>
		public void Register()
		{
			if (this.isRegistered || !this.IsInvalid)
			{
				throw new InvalidOperationException("The string recognizer is already registered.");
			}

			this.isValidForTypeCallback =
				new NativeDelegates.BNStringRecognizerIsValidForType(this.InvokeIsValidForType);
			this.recognizeConstantCallback =
				new NativeDelegates.BNStringRecognizerRecognizeConstant(this.InvokeRecognizeConstant);
			this.recognizeConstantPointerCallback =
				new NativeDelegates.BNStringRecognizerRecognizeConstant(
					this.InvokeRecognizeConstantPointer
				);
			this.recognizeExternPointerCallback =
				new NativeDelegates.BNStringRecognizerRecognizeExternPointer(
					this.InvokeRecognizeExternPointer
				);
			this.recognizeImportCallback =
				new NativeDelegates.BNStringRecognizerRecognizeConstant(this.InvokeRecognizeImport);

			BNCustomStringRecognizer callbacks = new BNCustomStringRecognizer();
			callbacks.context = IntPtr.Zero;
			callbacks.isValidForType = Marshal.GetFunctionPointerForDelegate(
				this.isValidForTypeCallback
			);
			callbacks.recognizeConstant = Marshal.GetFunctionPointerForDelegate(
				this.recognizeConstantCallback
			);
			callbacks.recognizeConstantPointer = Marshal.GetFunctionPointerForDelegate(
				this.recognizeConstantPointerCallback
			);
			callbacks.recognizeExternPointer = Marshal.GetFunctionPointerForDelegate(
				this.recognizeExternPointerCallback
			);
			callbacks.recognizeImport = Marshal.GetFunctionPointerForDelegate(
				this.recognizeImportCallback
			);

			IntPtr handle = NativeMethods.BNRegisterStringRecognizer(
				this.registrationName ?? string.Empty,
				in callbacks
			);
			if (IntPtr.Zero == handle)
			{
				throw new InvalidOperationException("The core rejected the string recognizer.");
			}

			this.SetHandle(handle);
			this.isRegistered = true;
			lock (StringRecognizer.registrationLock)
			{
				StringRecognizer.registeredRecognizers.Add(this);
			}
		}

		/// <summary>Finds a registered recognizer by name.</summary>
		public static StringRecognizer? FromName(string name)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			IntPtr handle = NativeMethods.BNGetStringRecognizerByName(name);
			if (IntPtr.Zero == handle)
			{
				return null;
			}

			return new CoreStringRecognizer(handle);
		}

		/// <summary>Gets every recognizer registered with the core.</summary>
		public static StringRecognizer[] GetRecognizers()
		{
			IntPtr recognizers = NativeMethods.BNGetStringRecognizerList(out ulong count);
			return UnsafeUtils.TakeHandleArray<StringRecognizer>(
				recognizers,
				count,
				StringRecognizer.CreateCoreRecognizer,
				NativeMethods.BNFreeStringRecognizerList
			);
		}

		private static StringRecognizer CreateCoreRecognizer(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				throw new ArgumentNullException(nameof(handle));
			}

			return new CoreStringRecognizer(handle);
		}

		/// <summary>Determines whether this recognizer accepts expressions of the given type.</summary>
		public virtual bool IsValidForType(HighLevelILFunction function, BinaryNinja.Type type)
		{
			return true;
		}

		public virtual DerivedString? RecognizeConstant(
			HighLevelILInstruction instruction,
			BinaryNinja.Type type,
			long value
		)
		{
			return null;
		}

		public virtual DerivedString? RecognizeConstantPointer(
			HighLevelILInstruction instruction,
			BinaryNinja.Type type,
			long value
		)
		{
			return null;
		}

		public virtual DerivedString? RecognizeExternPointer(
			HighLevelILInstruction instruction,
			BinaryNinja.Type type,
			long value,
			ulong offset
		)
		{
			return null;
		}

		public virtual DerivedString? RecognizeImport(
			HighLevelILInstruction instruction,
			BinaryNinja.Type type,
			long value
		)
		{
			return null;
		}

		private bool InvokeIsValidForType(IntPtr context, IntPtr functionHandle, IntPtr typeHandle)
		{
			try
			{
				using (HighLevelILFunction function =
					HighLevelILFunction.MustNewFromHandle(functionHandle))
				using (BinaryNinja.Type type = BinaryNinja.Type.MustNewFromHandle(typeHandle))
				{
					return this.IsValidForType(function, type);
				}
			}
			catch (Exception exception)
			{
				Core.LogError("Unhandled exception in StringRecognizer.IsValidForType: {0}", exception);
				return false;
			}
		}

		private bool InvokeRecognizeConstant(
			IntPtr context,
			IntPtr function,
			UIntPtr expressionIndex,
			IntPtr type,
			long value,
			IntPtr result
		)
		{
			return this.InvokeRecognition(
				RecognitionKind.Constant,
				function,
				expressionIndex,
				type,
				value,
				0,
				result
			);
		}

		private bool InvokeRecognizeConstantPointer(
			IntPtr context,
			IntPtr function,
			UIntPtr expressionIndex,
			IntPtr type,
			long value,
			IntPtr result
		)
		{
			return this.InvokeRecognition(
				RecognitionKind.ConstantPointer,
				function,
				expressionIndex,
				type,
				value,
				0,
				result
			);
		}

		private bool InvokeRecognizeExternPointer(
			IntPtr context,
			IntPtr function,
			UIntPtr expressionIndex,
			IntPtr type,
			long value,
			ulong offset,
			IntPtr result
		)
		{
			return this.InvokeRecognition(
				RecognitionKind.ExternPointer,
				function,
				expressionIndex,
				type,
				value,
				offset,
				result
			);
		}

		private bool InvokeRecognizeImport(
			IntPtr context,
			IntPtr function,
			UIntPtr expressionIndex,
			IntPtr type,
			long value,
			IntPtr result
		)
		{
			return this.InvokeRecognition(
				RecognitionKind.Import,
				function,
				expressionIndex,
				type,
				value,
				0,
				result
			);
		}

		private bool InvokeRecognition(
			RecognitionKind kind,
			IntPtr functionHandle,
			UIntPtr expressionIndex,
			IntPtr typeHandle,
			long value,
			ulong offset,
			IntPtr resultPointer
		)
		{
			try
			{
				using (HighLevelILFunction function =
					HighLevelILFunction.MustNewFromHandle(functionHandle))
				using (BinaryNinja.Type type = BinaryNinja.Type.MustNewFromHandle(typeHandle))
				{
					HighLevelILInstruction instruction = function.MustGetExpression(
						(HighLevelILExpressionIndex)expressionIndex.ToUInt64()
					);
					DerivedString? recognized;

					switch (kind)
					{
						case RecognitionKind.Constant:
							recognized = this.RecognizeConstant(instruction, type, value);
							break;
						case RecognitionKind.ConstantPointer:
							recognized = this.RecognizeConstantPointer(instruction, type, value);
							break;
						case RecognitionKind.ExternPointer:
							recognized = this.RecognizeExternPointer(
								instruction,
								type,
								value,
								offset
							);
							break;
						case RecognitionKind.Import:
							recognized = this.RecognizeImport(instruction, type, value);
							break;
						default:
							throw new ArgumentOutOfRangeException(nameof(kind));
					}

					return StringRecognizer.WriteResult(recognized, resultPointer);
				}
			}
			catch (Exception exception)
			{
				Core.LogError("Unhandled exception in StringRecognizer recognition callback: {0}", exception);
				return false;
			}
		}

		private static bool WriteResult(DerivedString? result, IntPtr resultPointer)
		{
			if (null == result)
			{
				return false;
			}

			BNDerivedString native = result.ToNativeEx();
			try
			{
				Marshal.StructureToPtr(native, resultPointer, false);
				return true;
			}
			catch
			{
				if (IntPtr.Zero != native.value)
				{
					NativeMethods.BNFreeStringRef(native.value);
				}

				throw;
			}
		}

		private sealed class CoreStringRecognizer : StringRecognizer
		{
			internal CoreStringRecognizer(IntPtr handle)
				: base(handle)
			{
			}

			public override bool IsValidForType(
				HighLevelILFunction function,
				BinaryNinja.Type type
			)
			{
				StringRecognizer.ValidateArguments(function, type);
				return NativeMethods.BNIsStringRecognizerValidForType(
					this.handle,
					function.DangerousGetHandle(),
					type.DangerousGetHandle()
				);
			}

			public override DerivedString? RecognizeConstant(
				HighLevelILInstruction instruction,
				BinaryNinja.Type type,
				long value
			)
			{
				return this.Recognize(
					RecognitionKind.Constant,
					instruction,
					type,
					value,
					0
				);
			}

			public override DerivedString? RecognizeConstantPointer(
				HighLevelILInstruction instruction,
				BinaryNinja.Type type,
				long value
			)
			{
				return this.Recognize(
					RecognitionKind.ConstantPointer,
					instruction,
					type,
					value,
					0
				);
			}

			public override DerivedString? RecognizeExternPointer(
				HighLevelILInstruction instruction,
				BinaryNinja.Type type,
				long value,
				ulong offset
			)
			{
				return this.Recognize(
					RecognitionKind.ExternPointer,
					instruction,
					type,
					value,
					offset
				);
			}

			public override DerivedString? RecognizeImport(
				HighLevelILInstruction instruction,
				BinaryNinja.Type type,
				long value
			)
			{
				return this.Recognize(
					RecognitionKind.Import,
					instruction,
					type,
					value,
					0
				);
			}

			private DerivedString? Recognize(
				RecognitionKind kind,
				HighLevelILInstruction instruction,
				BinaryNinja.Type type,
				long value,
				ulong offset
			)
			{
				StringRecognizer.ValidateArguments(instruction, type);
				UIntPtr expressionIndex = (UIntPtr)(ulong)instruction.ExpressionIndex;
				BNDerivedString result;
				bool recognized;

				switch (kind)
				{
					case RecognitionKind.Constant:
						recognized = NativeMethods.BNStringRecognizerRecognizeConstant(
							this.handle,
							instruction.Function.DangerousGetHandle(),
							expressionIndex,
							type.DangerousGetHandle(),
							value,
							out result
						);
						break;
					case RecognitionKind.ConstantPointer:
						recognized = NativeMethods.BNStringRecognizerRecognizeConstantPointer(
							this.handle,
							instruction.Function.DangerousGetHandle(),
							expressionIndex,
							type.DangerousGetHandle(),
							value,
							out result
						);
						break;
					case RecognitionKind.ExternPointer:
						recognized = NativeMethods.BNStringRecognizerRecognizeExternPointer(
							this.handle,
							instruction.Function.DangerousGetHandle(),
							expressionIndex,
							type.DangerousGetHandle(),
							value,
							offset,
							out result
						);
						break;
					case RecognitionKind.Import:
						recognized = NativeMethods.BNStringRecognizerRecognizeImport(
							this.handle,
							instruction.Function.DangerousGetHandle(),
							expressionIndex,
							type.DangerousGetHandle(),
							value,
							out result
						);
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(kind));
				}

				if (!recognized)
				{
					return null;
				}

				try
				{
					return DerivedString.FromNative(result);
				}
				finally
				{
					if (IntPtr.Zero != result.value)
					{
						NativeMethods.BNFreeStringRef(result.value);
					}
				}
			}
		}

		private static void ValidateArguments(
			HighLevelILFunction function,
			BinaryNinja.Type type
		)
		{
			if (null == function)
			{
				throw new ArgumentNullException(nameof(function));
			}

			if (null == type)
			{
				throw new ArgumentNullException(nameof(type));
			}
		}

		private static void ValidateArguments(
			HighLevelILInstruction instruction,
			BinaryNinja.Type type
		)
		{
			if (null == instruction)
			{
				throw new ArgumentNullException(nameof(instruction));
			}

			if (null == type)
			{
				throw new ArgumentNullException(nameof(type));
			}
		}
	}
}
