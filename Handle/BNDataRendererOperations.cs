using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public partial class DataRenderer
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate void NativeFreeObject(IntPtr context);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private delegate bool NativeIsValidForData(
			IntPtr context,
			IntPtr view,
			ulong address,
			IntPtr type,
			IntPtr typeContext,
			ulong contextCount);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate IntPtr NativeGetLinesForData(
			IntPtr context,
			IntPtr view,
			ulong address,
			IntPtr type,
			IntPtr prefix,
			ulong prefixCount,
			ulong width,
			IntPtr count,
			IntPtr typeContext,
			ulong contextCount,
			IntPtr language);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate void NativeFreeLines(IntPtr context, IntPtr lines, ulong count);

		private static readonly NativeFreeObject NativeFreeObjectCallback = FreeObjectCallback;
		private static readonly NativeIsValidForData NativeIsValidForDataCallback = IsValidForDataCallback;
		private static readonly NativeGetLinesForData NativeGetLinesForDataCallback = GetLinesForDataCallback;
		private static readonly NativeFreeLines NativeFreeLinesCallback = FreeLinesCallback;

		private CallbackState? callbackState;

		private sealed class CallbackState
		{
			private readonly object sync = new object();
			private readonly WeakReference<DataRenderer> renderer;
			private readonly Dictionary<IntPtr, ScopedAllocator> lineAllocations =
				new Dictionary<IntPtr, ScopedAllocator>();
			private DataRenderer? registeredRenderer;

			internal CallbackState(DataRenderer renderer)
			{
				this.renderer = new WeakReference<DataRenderer>(renderer);
			}

			internal DataRenderer? GetRenderer()
			{
				lock (this.sync)
				{
					if (null != this.registeredRenderer)
					{
						return this.registeredRenderer;
					}

					this.renderer.TryGetTarget(out DataRenderer? result);
					return result;
				}
			}

			internal void Root(DataRenderer value)
			{
				lock (this.sync)
				{
					this.registeredRenderer = value;
				}
			}

			internal void AddLines(IntPtr lines, ScopedAllocator allocator)
			{
				lock (this.sync)
				{
					this.lineAllocations.Add(lines, allocator);
				}
			}

			internal void FreeLines(IntPtr lines)
			{
				ScopedAllocator? allocator = null;
				lock (this.sync)
				{
					if (this.lineAllocations.TryGetValue(lines, out allocator))
					{
						this.lineAllocations.Remove(lines);
					}
				}

				if (null != allocator)
				{
					allocator.Dispose();
				}
			}

			internal void Dispose()
			{
				ScopedAllocator[] allocators;
				lock (this.sync)
				{
					allocators = new ScopedAllocator[this.lineAllocations.Count];
					this.lineAllocations.Values.CopyTo(allocators, 0);
					this.lineAllocations.Clear();
					this.registeredRenderer = null;
				}

				for (int i = 0; i < allocators.Length; i++)
				{
					allocators[i].Dispose();
				}
			}
		}

		/// <summary>
		/// Creates a managed callback-backed data renderer.
		/// </summary>
		protected DataRenderer()
			: base(true)
		{
			CallbackState state = new CallbackState(this);
			GCHandle contextHandle = GCHandle.Alloc(state);
			BNCustomDataRenderer callbacks = new BNCustomDataRenderer
			{
				context = GCHandle.ToIntPtr(contextHandle),
				freeObject = Marshal.GetFunctionPointerForDelegate<NativeFreeObject>(NativeFreeObjectCallback),
				isValidForData = Marshal.GetFunctionPointerForDelegate<NativeIsValidForData>(NativeIsValidForDataCallback),
				getLinesForData = Marshal.GetFunctionPointerForDelegate<NativeGetLinesForData>(NativeGetLinesForDataCallback),
				freeLines = Marshal.GetFunctionPointerForDelegate<NativeFreeLines>(NativeFreeLinesCallback)
			};
			IntPtr renderer;

			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				renderer = NativeMethods.BNCreateDataRenderer(
					allocator.AllocStruct<BNCustomDataRenderer>(callbacks));
			}

			if (IntPtr.Zero == renderer)
			{
				contextHandle.Free();
				throw new InvalidOperationException("BNCreateDataRenderer failed.");
			}

			this.callbackState = state;
			this.SetHandle(renderer);
		}

		/// <summary>
		/// Tests whether this renderer accepts data of the supplied type and context.
		/// </summary>
		public bool IsValidForData(
			BinaryView view,
			ulong address,
			BinaryNinja.Type type,
			TypeContext[]? context = null)
		{
			ValidateViewAndType(view, type);
			TypeContext[] safeContext = context ?? Array.Empty<TypeContext>();
			if (null != this.callbackState)
			{
				return this.PerformIsValidForData(view, address, type, safeContext);
			}

			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				return NativeMethods.BNIsValidForData(
					this.handle,
					view.DangerousGetHandle(),
					address,
					type.DangerousGetHandle(),
					AllocateTypeContext(safeContext, allocator),
					(ulong)safeContext.Length);
			}
		}

		/// <summary>
		/// Gets lines directly from this renderer.
		/// </summary>
		public unsafe DisassemblyTextLine[] GetLinesForData(
			BinaryView view,
			ulong address,
			BinaryNinja.Type type,
			InstructionTextToken[]? prefix,
			ulong width,
			TypeContext[]? context = null,
			string? language = null)
		{
			ValidateViewAndType(view, type);
			InstructionTextToken[] safePrefix = prefix ?? Array.Empty<InstructionTextToken>();
			TypeContext[] safeContext = context ?? Array.Empty<TypeContext>();
			if (null != this.callbackState)
			{
				return this.PerformGetLinesForData(
					view,
					address,
					type,
					safePrefix,
					width,
					safeContext,
					language ?? string.Empty);
			}

			ulong count = 0;
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr lines = NativeMethods.BNGetLinesForData(
					this.handle,
					view.DangerousGetHandle(),
					address,
					type.DangerousGetHandle(),
					AllocateTokens(safePrefix, allocator),
					(ulong)safePrefix.Length,
					width,
					(IntPtr)(&count),
					AllocateTypeContext(safeContext, allocator),
					(ulong)safeContext.Length,
					language ?? string.Empty);
				return TakeLines(lines, count);
			}
		}

		/// <summary>
		/// Renders data using the registered type-specific and generic renderers.
		/// </summary>
		public static unsafe DisassemblyTextLine[] RenderLinesForData(
			BinaryView view,
			ulong address,
			BinaryNinja.Type type,
			InstructionTextToken[]? prefix,
			ulong width,
			TypeContext[]? context = null,
			string? language = null)
		{
			ValidateViewAndType(view, type);
			InstructionTextToken[] safePrefix = prefix ?? Array.Empty<InstructionTextToken>();
			TypeContext[] safeContext = context ?? Array.Empty<TypeContext>();
			ulong count = 0;
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr lines = NativeMethods.BNRenderLinesForData(
					view.DangerousGetHandle(),
					address,
					type.DangerousGetHandle(),
					AllocateTokens(safePrefix, allocator),
					(ulong)safePrefix.Length,
					width,
					(IntPtr)(&count),
					AllocateTypeContext(safeContext, allocator),
					(ulong)safeContext.Length,
					language ?? string.Empty);
				return TakeLines(lines, count);
			}
		}

		/// <summary>
		/// Determines whether a managed custom renderer accepts the supplied data.
		/// </summary>
		protected virtual bool PerformIsValidForData(
			BinaryView view,
			ulong address,
			BinaryNinja.Type type,
			TypeContext[] context)
		{
			return false;
		}

		/// <summary>
		/// Produces lines for a managed custom renderer.
		/// </summary>
		protected virtual DisassemblyTextLine[] PerformGetLinesForData(
			BinaryView view,
			ulong address,
			BinaryNinja.Type type,
			InstructionTextToken[] prefix,
			ulong width,
			TypeContext[] context,
			string language)
		{
			return Array.Empty<DisassemblyTextLine>();
		}

		private void RootForRegistration()
		{
			if (null != this.callbackState)
			{
				this.callbackState.Root(this);
			}
		}

		private static void FreeObjectCallback(IntPtr context)
		{
			GCHandle contextHandle = GCHandle.FromIntPtr(context);
			CallbackState? state = contextHandle.Target as CallbackState;
			if (null != state)
			{
				state.Dispose();
			}

			contextHandle.Free();
		}

		private static bool IsValidForDataCallback(
			IntPtr context,
			IntPtr view,
			ulong address,
			IntPtr type,
			IntPtr typeContext,
			ulong contextCount)
		{
			CallbackState? state = GetCallbackState(context);
			DataRenderer? renderer = null == state ? null : state.GetRenderer();
			if (null == renderer)
			{
				return false;
			}

			try
			{
				using (BinaryView managedView = BinaryView.MustTakeHandle(
					NativeMethods.BNNewViewReference(view)))
				using (BinaryNinja.Type managedType = BinaryNinja.Type.MustNewFromHandle(type))
				{
					TypeContext[] managedContext = ReadTypeContext(typeContext, contextCount);
					try
					{
						return renderer.PerformIsValidForData(
							managedView,
							address,
							managedType,
							managedContext);
					}
					finally
					{
						DisposeTypeContext(managedContext);
					}
				}
			}
			catch (Exception)
			{
				return false;
			}
		}

		private static IntPtr GetLinesForDataCallback(
			IntPtr context,
			IntPtr view,
			ulong address,
			IntPtr type,
			IntPtr prefix,
			ulong prefixCount,
			ulong width,
			IntPtr count,
			IntPtr typeContext,
			ulong contextCount,
			IntPtr language)
		{
			Marshal.WriteInt64(count, 0);
			CallbackState? state = GetCallbackState(context);
			DataRenderer? renderer = null == state ? null : state.GetRenderer();
			if (null == state || null == renderer)
			{
				return IntPtr.Zero;
			}

			try
			{
				using (BinaryView managedView = BinaryView.MustTakeHandle(
					NativeMethods.BNNewViewReference(view)))
				using (BinaryNinja.Type managedType = BinaryNinja.Type.MustNewFromHandle(type))
				{
					InstructionTextToken[] managedPrefix =
						UnsafeUtils.ReadStructArray<BNInstructionTextToken, InstructionTextToken>(
							prefix,
							prefixCount,
							InstructionTextToken.FromNative);
					TypeContext[] managedContext = ReadTypeContext(typeContext, contextCount);
					try
					{
						DisassemblyTextLine[] lines = renderer.PerformGetLinesForData(
							managedView,
							address,
							managedType,
							managedPrefix,
							width,
							managedContext,
							UnsafeUtils.ReadUtf8String(language));
						if (null == lines || 0 == lines.Length)
						{
							return IntPtr.Zero;
						}

						ScopedAllocator allocator = new ScopedAllocator();
						try
						{
							BNDisassemblyTextLine[] nativeLines =
								allocator.ConvertToNativeArrayEx<BNDisassemblyTextLine, DisassemblyTextLine>(lines);
							IntPtr result = allocator.AllocStructArray<BNDisassemblyTextLine>(nativeLines);
							state.AddLines(result, allocator);
							Marshal.WriteInt64(count, lines.Length);
							return result;
						}
						catch (Exception)
						{
							allocator.Dispose();
							throw;
						}
					}
					finally
					{
						DisposeTypeContext(managedContext);
					}
				}
			}
			catch (Exception)
			{
				Marshal.WriteInt64(count, 0);
				return IntPtr.Zero;
			}
		}

		private static void FreeLinesCallback(IntPtr context, IntPtr lines, ulong count)
		{
			CallbackState? state = GetCallbackState(context);
			if (null != state)
			{
				state.FreeLines(lines);
			}
		}

		private static CallbackState? GetCallbackState(IntPtr context)
		{
			if (IntPtr.Zero == context)
			{
				return null;
			}

			GCHandle contextHandle = GCHandle.FromIntPtr(context);
			return contextHandle.Target as CallbackState;
		}

		private static void ValidateViewAndType(BinaryView view, BinaryNinja.Type type)
		{
			if (null == view)
			{
				throw new ArgumentNullException(nameof(view));
			}

			if (null == type)
			{
				throw new ArgumentNullException(nameof(type));
			}
		}

		private static IntPtr AllocateTokens(
			InstructionTextToken[] tokens,
			ScopedAllocator allocator)
		{
			BNInstructionTextToken[] nativeTokens =
				allocator.ConvertToNativeArrayEx<BNInstructionTextToken, InstructionTextToken>(tokens);
			return allocator.AllocStructArray<BNInstructionTextToken>(nativeTokens);
		}

		private static IntPtr AllocateTypeContext(
			TypeContext[] context,
			ScopedAllocator allocator)
		{
			BNTypeContext[] nativeContext = new BNTypeContext[context.Length];
			for (int i = 0; i < context.Length; i++)
			{
				if (null == context[i] || null == context[i].Type)
				{
					throw new ArgumentException(
						"Type context entries and their types cannot be null.",
						nameof(context));
				}

				nativeContext[i] = new BNTypeContext
				{
					type = context[i].Type!.DangerousGetHandle(),
					offset = context[i].Offset
				};
			}

			return allocator.AllocStructArray<BNTypeContext>(nativeContext);
		}

		private static TypeContext[] ReadTypeContext(IntPtr context, ulong count)
		{
			BNTypeContext[] nativeContext = UnsafeUtils.ReadStructArray<BNTypeContext, BNTypeContext>(
				context,
				count,
				CopyTypeContext);
			TypeContext[] result = new TypeContext[nativeContext.Length];
			for (int i = 0; i < nativeContext.Length; i++)
			{
				result[i] = new TypeContext(
					BinaryNinja.Type.MustNewFromHandle(nativeContext[i].type),
					nativeContext[i].offset);
			}

			return result;
		}

		private static BNTypeContext CopyTypeContext(BNTypeContext context)
		{
			return context;
		}

		private static void DisposeTypeContext(TypeContext[] context)
		{
			for (int i = 0; i < context.Length; i++)
			{
				if (null != context[i].Type)
				{
					context[i].Type!.Dispose();
				}
			}
		}

		private static DisassemblyTextLine[] TakeLines(IntPtr lines, ulong count)
		{
			return UnsafeUtils.TakeStructArrayEx<BNDisassemblyTextLine, DisassemblyTextLine>(
				lines,
				count,
				DisassemblyTextLine.FromNative,
				NativeMethods.BNFreeDisassemblyTextLines);
		}
	}
}
