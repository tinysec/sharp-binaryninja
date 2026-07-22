using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class NativeDelegates
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNConstantRendererIsValidForType(
			IntPtr context,
			IntPtr function,
			IntPtr type
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNConstantRendererRenderConstant(
			IntPtr context,
			IntPtr function,
			UIntPtr expressionIndex,
			IntPtr type,
			long value,
			IntPtr tokens,
			IntPtr settings,
			OperatorPrecedence precedence
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNConstantRendererRenderConstantPointer(
			IntPtr context,
			IntPtr function,
			UIntPtr expressionIndex,
			IntPtr type,
			long value,
			IntPtr tokens,
			IntPtr settings,
			SymbolDisplayType symbolDisplay,
			OperatorPrecedence precedence
		);
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct BNCustomConstantRenderer
	{
		internal IntPtr context;

		internal IntPtr isValidForType;

		internal IntPtr renderConstant;

		internal IntPtr renderConstantPointer;
	}

	/// <summary>
	/// Renders constants in high-level language representations.
	/// </summary>
	public class ConstantRenderer : AbstractSafeHandle<ConstantRenderer>
	{
		private static readonly object registrationLock = new object();

		private static readonly List<ConstantRenderer> registeredRenderers =
			new List<ConstantRenderer>();

		private readonly string? registrationName;

		private bool isRegistered;

		private NativeDelegates.BNConstantRendererIsValidForType? isValidForTypeCallback;

		private NativeDelegates.BNConstantRendererRenderConstant? renderConstantCallback;

		private NativeDelegates.BNConstantRendererRenderConstantPointer?
			renderConstantPointerCallback;

		/// <summary>Creates an unregistered custom constant renderer.</summary>
		protected ConstantRenderer(string name)
			: base(false)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			this.registrationName = name;
		}

		private ConstantRenderer(IntPtr handle)
			: base(handle, false)
		{
		}

		/// <summary>Gets the registered renderer name.</summary>
		public string Name
		{
			get
			{
				if (this.IsInvalid)
				{
					return this.registrationName ?? string.Empty;
				}

				return UnsafeUtils.TakeUtf8String(
					NativeMethods.BNGetConstantRendererName(this.handle)
				);
			}
		}

		/// <summary>Registers this renderer and roots its callbacks for core use.</summary>
		public void Register()
		{
			if (this.isRegistered || !this.IsInvalid)
			{
				throw new InvalidOperationException("The constant renderer is already registered.");
			}

			this.isValidForTypeCallback =
				new NativeDelegates.BNConstantRendererIsValidForType(
					this.InvokeIsValidForType
				);
			this.renderConstantCallback =
				new NativeDelegates.BNConstantRendererRenderConstant(
					this.InvokeRenderConstant
				);
			this.renderConstantPointerCallback =
				new NativeDelegates.BNConstantRendererRenderConstantPointer(
					this.InvokeRenderConstantPointer
				);

			BNCustomConstantRenderer callbacks = new BNCustomConstantRenderer();
			callbacks.context = IntPtr.Zero;
			callbacks.isValidForType = Marshal.GetFunctionPointerForDelegate(
				this.isValidForTypeCallback
			);
			callbacks.renderConstant = Marshal.GetFunctionPointerForDelegate(
				this.renderConstantCallback
			);
			callbacks.renderConstantPointer = Marshal.GetFunctionPointerForDelegate(
				this.renderConstantPointerCallback
			);

			IntPtr handle = NativeMethods.BNRegisterConstantRenderer(
				this.registrationName ?? string.Empty,
				in callbacks
			);
			if (IntPtr.Zero == handle)
			{
				throw new InvalidOperationException("The core rejected the constant renderer.");
			}

			this.SetHandle(handle);
			this.isRegistered = true;
			lock (ConstantRenderer.registrationLock)
			{
				ConstantRenderer.registeredRenderers.Add(this);
			}
		}

		/// <summary>Finds a registered renderer by name.</summary>
		public static ConstantRenderer? FromName(string name)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			IntPtr handle = NativeMethods.BNGetConstantRendererByName(name);
			if (IntPtr.Zero == handle)
			{
				return null;
			}

			return new CoreConstantRenderer(handle);
		}

		/// <summary>Gets every renderer registered with the core.</summary>
		public static ConstantRenderer[] GetRenderers()
		{
			IntPtr renderers = NativeMethods.BNGetConstantRendererList(out ulong count);
			return UnsafeUtils.TakeHandleArray<ConstantRenderer>(
				renderers,
				count,
				ConstantRenderer.CreateCoreRenderer,
				NativeMethods.BNFreeConstantRendererList
			);
		}

		private static ConstantRenderer CreateCoreRenderer(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				throw new ArgumentNullException(nameof(handle));
			}

			return new CoreConstantRenderer(handle);
		}

		/// <summary>Determines whether this renderer accepts expressions of the given type.</summary>
		public virtual bool IsValidForType(HighLevelILFunction function, BinaryNinja.Type type)
		{
			return true;
		}

		/// <summary>Renders a non-pointer constant into the supplied token emitter.</summary>
		public virtual bool RenderConstant(
			HighLevelILInstruction instruction,
			BinaryNinja.Type type,
			long value,
			HighLevelILTokenEmitter tokens,
			DisassemblySettings? settings,
			OperatorPrecedence precedence
		)
		{
			return false;
		}

		/// <summary>Renders a pointer constant into the supplied token emitter.</summary>
		public virtual bool RenderConstantPointer(
			HighLevelILInstruction instruction,
			BinaryNinja.Type type,
			long value,
			HighLevelILTokenEmitter tokens,
			DisassemblySettings? settings,
			SymbolDisplayType symbolDisplay,
			OperatorPrecedence precedence
		)
		{
			return false;
		}

		private bool InvokeIsValidForType(
			IntPtr context,
			IntPtr functionHandle,
			IntPtr typeHandle
		)
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
				Core.LogError("Unhandled exception in ConstantRenderer.IsValidForType: {0}", exception);
				return false;
			}
		}

		private bool InvokeRenderConstant(
			IntPtr context,
			IntPtr function,
			UIntPtr expressionIndex,
			IntPtr type,
			long value,
			IntPtr tokens,
			IntPtr settings,
			OperatorPrecedence precedence
		)
		{
			return this.InvokeRender(
				false,
				function,
				expressionIndex,
				type,
				value,
				tokens,
				settings,
				default(SymbolDisplayType),
				precedence
			);
		}

		private bool InvokeRenderConstantPointer(
			IntPtr context,
			IntPtr function,
			UIntPtr expressionIndex,
			IntPtr type,
			long value,
			IntPtr tokens,
			IntPtr settings,
			SymbolDisplayType symbolDisplay,
			OperatorPrecedence precedence
		)
		{
			return this.InvokeRender(
				true,
				function,
				expressionIndex,
				type,
				value,
				tokens,
				settings,
				symbolDisplay,
				precedence
			);
		}

		private bool InvokeRender(
			bool pointer,
			IntPtr functionHandle,
			UIntPtr expressionIndex,
			IntPtr typeHandle,
			long value,
			IntPtr tokensHandle,
			IntPtr settingsHandle,
			SymbolDisplayType symbolDisplay,
			OperatorPrecedence precedence
		)
		{
			try
			{
				using (HighLevelILFunction function =
					HighLevelILFunction.MustNewFromHandle(functionHandle))
				using (BinaryNinja.Type type = BinaryNinja.Type.MustNewFromHandle(typeHandle))
				using (HighLevelILTokenEmitter tokens =
					HighLevelILTokenEmitter.MustNewFromHandle(tokensHandle))
				using (DisassemblySettings? settings =
					DisassemblySettings.NewFromHandle(settingsHandle))
				{
					HighLevelILInstruction instruction = function.MustGetExpression(
						(HighLevelILExpressionIndex)expressionIndex.ToUInt64()
					);
					if (pointer)
					{
						return this.RenderConstantPointer(
							instruction,
							type,
							value,
							tokens,
							settings,
							symbolDisplay,
							precedence
						);
					}

					return this.RenderConstant(
						instruction,
						type,
						value,
						tokens,
						settings,
						precedence
					);
				}
			}
			catch (Exception exception)
			{
				Core.LogError("Unhandled exception in ConstantRenderer render callback: {0}", exception);
				return false;
			}
		}

		private sealed class CoreConstantRenderer : ConstantRenderer
		{
			internal CoreConstantRenderer(IntPtr handle)
				: base(handle)
			{
			}

			public override bool IsValidForType(
				HighLevelILFunction function,
				BinaryNinja.Type type
			)
			{
				ConstantRenderer.ValidateArguments(function, type);
				return NativeMethods.BNIsConstantRendererValidForType(
					this.handle,
					function.DangerousGetHandle(),
					type.DangerousGetHandle()
				);
			}

			public override bool RenderConstant(
				HighLevelILInstruction instruction,
				BinaryNinja.Type type,
				long value,
				HighLevelILTokenEmitter tokens,
				DisassemblySettings? settings,
				OperatorPrecedence precedence
			)
			{
				ConstantRenderer.ValidateArguments(instruction, type, tokens);
				return NativeMethods.BNConstantRendererRenderConstant(
					this.handle,
					instruction.Function.DangerousGetHandle(),
					(UIntPtr)(ulong)instruction.ExpressionIndex,
					type.DangerousGetHandle(),
					value,
					tokens.DangerousGetHandle(),
					null == settings ? IntPtr.Zero : settings.DangerousGetHandle(),
					precedence
				);
			}

			public override bool RenderConstantPointer(
				HighLevelILInstruction instruction,
				BinaryNinja.Type type,
				long value,
				HighLevelILTokenEmitter tokens,
				DisassemblySettings? settings,
				SymbolDisplayType symbolDisplay,
				OperatorPrecedence precedence
			)
			{
				ConstantRenderer.ValidateArguments(instruction, type, tokens);
				return NativeMethods.BNConstantRendererRenderConstantPointer(
					this.handle,
					instruction.Function.DangerousGetHandle(),
					(UIntPtr)(ulong)instruction.ExpressionIndex,
					type.DangerousGetHandle(),
					value,
					tokens.DangerousGetHandle(),
					null == settings ? IntPtr.Zero : settings.DangerousGetHandle(),
					symbolDisplay,
					precedence
				);
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
			BinaryNinja.Type type,
			HighLevelILTokenEmitter tokens
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

			if (null == tokens)
			{
				throw new ArgumentNullException(nameof(tokens));
			}
		}
	}
}
