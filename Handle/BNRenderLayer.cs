using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	/// <summary>Transforms graph and linear-view output before presentation.</summary>
	public partial class RenderLayer : AbstractSafeHandle<RenderLayer>
	{
		private static readonly object registrationLock = new object();

		private static readonly Dictionary<IntPtr, RenderLayer> registeredLayers =
			new Dictionary<IntPtr, RenderLayer>();

		private readonly object outputLock = new object();

		private readonly Dictionary<IntPtr, ScopedAllocator> pendingOutputs =
			new Dictionary<IntPtr, ScopedAllocator>();

		private readonly string? registrationName;

		private readonly RenderLayerDefaultEnableState registrationEnableState;

		private bool isRegistered;

		private NativeDelegates.BNRenderLayerApplyToFlowGraph? applyToFlowGraphCallback;

		private NativeDelegates.BNRenderLayerApplyToLinearViewObject?
			applyToLinearViewObjectCallback;

		private NativeDelegates.BNRenderLayerFreeLines? freeLinesCallback;

		/// <summary>Creates an unregistered custom render layer.</summary>
		protected RenderLayer(
			string name,
			RenderLayerDefaultEnableState enableState =
				RenderLayerDefaultEnableState.DisabledByDefaultRenderLayerDefaultEnableState
		)
			: base(false)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			this.registrationName = name;
			this.registrationEnableState = enableState;
		}

		private RenderLayer(IntPtr handle)
			: base(handle, false)
		{
		}

		/// <summary>Gets the registered layer name.</summary>
		public string Name
		{
			get
			{
				if (this.IsInvalid)
				{
					return this.registrationName ?? string.Empty;
				}

				return UnsafeUtils.TakeUtf8String(
					NativeMethods.BNGetRenderLayerName(this.handle)
				);
			}
		}

		/// <summary>Gets the default UI enable state.</summary>
		public RenderLayerDefaultEnableState DefaultEnableState
		{
			get
			{
				if (this.IsInvalid)
				{
					return this.registrationEnableState;
				}

				return NativeMethods.BNGetRenderLayerDefaultEnableState(this.handle);
			}
		}

		/// <summary>Registers this layer and roots its callbacks for core use.</summary>
		public void Register()
		{
			if (this.isRegistered || !this.IsInvalid)
			{
				throw new InvalidOperationException(
					"The render layer is already registered."
				);
			}

			this.applyToFlowGraphCallback =
				new NativeDelegates.BNRenderLayerApplyToFlowGraph(
					this.InvokeApplyToFlowGraph
				);
			this.applyToLinearViewObjectCallback =
				new NativeDelegates.BNRenderLayerApplyToLinearViewObject(
					this.InvokeApplyToLinearViewObject
				);
			this.freeLinesCallback = new NativeDelegates.BNRenderLayerFreeLines(
				this.InvokeFreeLines
			);

			BNRenderLayerCallbacks callbacks = new BNRenderLayerCallbacks();
			callbacks.context = IntPtr.Zero;
			callbacks.applyToFlowGraph = Marshal.GetFunctionPointerForDelegate(
				this.applyToFlowGraphCallback
			);
			callbacks.applyToLinearViewObject = Marshal.GetFunctionPointerForDelegate(
				this.applyToLinearViewObjectCallback
			);
			callbacks.freeLines = Marshal.GetFunctionPointerForDelegate(
				this.freeLinesCallback
			);

			IntPtr handle = NativeMethods.BNRegisterRenderLayer(
				this.registrationName ?? string.Empty,
				in callbacks,
				this.registrationEnableState
			);
			if (IntPtr.Zero == handle)
			{
				throw new InvalidOperationException(
					"The core rejected the render layer."
				);
			}

			this.SetHandle(handle);
			this.isRegistered = true;
			lock (RenderLayer.registrationLock)
			{
				RenderLayer.registeredLayers.Add(handle, this);
			}
		}

		/// <summary>Applies this layer to a flow graph.</summary>
		public virtual void ApplyToFlowGraph(FlowGraph graph)
		{
			if (null == graph)
			{
				throw new ArgumentNullException(nameof(graph));
			}

			this.ApplyToFlowGraphDefault(graph);
		}

		/// <summary>Transforms lines produced by a linear-view object.</summary>
		public virtual LinearDisassemblyLine[] ApplyToLinearViewObject(
			LinearViewObject linearView,
			LinearViewObject? previous,
			LinearViewObject? next,
			LinearDisassemblyLine[] lines
		)
		{
			if (null == linearView)
			{
				throw new ArgumentNullException(nameof(linearView));
			}

			if (null == lines)
			{
				throw new ArgumentNullException(nameof(lines));
			}

			return this.ApplyToLinearViewObjectDefault(
				linearView,
				previous,
				next,
				lines
			);
		}

		/// <summary>Looks up a registered layer by name.</summary>
		public static RenderLayer? GetByName(string name)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			return RenderLayer.FromHandle(
				NativeMethods.BNGetRenderLayerByName(name)
			);
		}

		/// <summary>Gets every registered layer.</summary>
		public static unsafe RenderLayer[] GetList()
		{
			ulong count = 0;
			IntPtr layers = NativeMethods.BNGetRenderLayerList((IntPtr)(&count));
			return UnsafeUtils.TakeHandleArray<RenderLayer>(
				layers,
				count,
				RenderLayer.MustFromHandle,
				NativeMethods.BNFreeRenderLayerList
			);
		}

		internal static RenderLayer FromHandle(IntPtr handle)
		{
			RenderLayer? layer = RenderLayer.FromHandleOrNull(handle);
			if (null == layer)
			{
				throw new ArgumentNullException(nameof(handle));
			}

			return layer;
		}

		internal static RenderLayer? BorrowHandle(IntPtr handle)
		{
			return RenderLayer.FromHandleOrNull(handle);
		}

		internal static RenderLayer MustBorrowHandle(IntPtr handle)
		{
			return RenderLayer.FromHandle(handle);
		}

		protected override bool ReleaseHandle()
		{
			return true;
		}

		private static RenderLayer? FromHandleOrNull(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				return null;
			}

			lock (RenderLayer.registrationLock)
			{
				if (RenderLayer.registeredLayers.TryGetValue(
					handle,
					out RenderLayer? registered
				))
				{
					return registered;
				}
			}

			return new CoreRenderLayer(handle);
		}

		private static RenderLayer MustFromHandle(IntPtr handle)
		{
			return RenderLayer.FromHandle(handle);
		}

		private void InvokeApplyToFlowGraph(IntPtr context, IntPtr graph)
		{
			FlowGraph? managedGraph = null;
			try
			{
				managedGraph = FlowGraph.MustTakeHandle(
					NativeMethods.BNNewFlowGraphReference(graph)
				);
				this.ApplyToFlowGraph(managedGraph);
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in RenderLayer.ApplyToFlowGraph: {0}",
					exception
				);
			}
			finally
			{
				if (null != managedGraph)
				{
					managedGraph.Dispose();
				}
			}
		}

		private void InvokeApplyToLinearViewObject(
			IntPtr context,
			IntPtr linearView,
			IntPtr previous,
			IntPtr next,
			IntPtr inputLines,
			ulong inputLineCount,
			IntPtr outputLines,
			IntPtr outputLineCount
		)
		{
			Marshal.WriteIntPtr(outputLines, IntPtr.Zero);
			Marshal.WriteInt64(outputLineCount, 0);
			LinearViewObject? managedView = null;
			LinearViewObject? managedPrevious = null;
			LinearViewObject? managedNext = null;
			LinearDisassemblyLine[] input = Array.Empty<LinearDisassemblyLine>();
			try
			{
				managedView = LinearViewObject.MustNewFromHandle(linearView);
				managedPrevious = LinearViewObject.NewFromHandle(previous);
				managedNext = LinearViewObject.NewFromHandle(next);
				input = UnsafeUtils.ReadStructArray<
					BNLinearDisassemblyLine,
					LinearDisassemblyLine
				>(
					inputLines,
					inputLineCount,
					LinearDisassemblyLine.FromNative
				);

				LinearDisassemblyLine[] output = this.ApplyToLinearViewObject(
					managedView,
					managedPrevious,
					managedNext,
					input
				);
				if (null == output || 0 == output.Length)
				{
					return;
				}

				ScopedAllocator allocator = new ScopedAllocator();
				try
				{
					BNLinearDisassemblyLine[] nativeOutput =
						new BNLinearDisassemblyLine[output.Length];
					for (int i = 0; i < output.Length; i++)
					{
						if (null == output[i])
						{
							throw new InvalidOperationException(
								"Render layer output cannot contain null lines."
							);
						}

						nativeOutput[i] = output[i].ToNativeEx(allocator);
					}

					IntPtr result = allocator.AllocStructArray(nativeOutput);
					lock (this.outputLock)
					{
						this.pendingOutputs.Add(result, allocator);
					}

					Marshal.WriteIntPtr(outputLines, result);
					Marshal.WriteInt64(outputLineCount, output.Length);
				}
				catch
				{
					allocator.Dispose();
					throw;
				}
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in RenderLayer.ApplyToLinearViewObject: {0}",
					exception
				);
			}
			finally
			{
				RenderLayer.DisposeLines(input);
				if (null != managedNext)
				{
					managedNext.Dispose();
				}

				if (null != managedPrevious)
				{
					managedPrevious.Dispose();
				}

				if (null != managedView)
				{
					managedView.Dispose();
				}
			}
		}

		private void InvokeFreeLines(IntPtr context, IntPtr lines, ulong count)
		{
			if (IntPtr.Zero == lines)
			{
				return;
			}

			ScopedAllocator? allocator = null;
			lock (this.outputLock)
			{
				if (this.pendingOutputs.TryGetValue(lines, out allocator))
				{
					this.pendingOutputs.Remove(lines);
				}
			}

			if (null == allocator)
			{
				Core.LogError("RenderLayer received an unknown line allocation.");
				return;
			}

			allocator.Dispose();
		}

		private static void DisposeLines(LinearDisassemblyLine[] lines)
		{
			foreach (LinearDisassemblyLine line in lines)
			{
				if (null != line.Function)
				{
					line.Function.Dispose();
				}

				if (null != line.Block)
				{
					line.Block.Dispose();
				}

				foreach (Tag tag in line.Contents.Tags)
				{
					tag.Dispose();
				}
			}
		}

		private sealed class CoreRenderLayer : RenderLayer
		{
			internal CoreRenderLayer(IntPtr handle)
				: base(handle)
			{
			}

			public override void ApplyToFlowGraph(FlowGraph graph)
			{
				if (null == graph)
				{
					throw new ArgumentNullException(nameof(graph));
				}

				NativeMethods.BNApplyRenderLayerToFlowGraph(
					this.handle,
					graph.DangerousGetHandle()
				);
			}

			public override LinearDisassemblyLine[] ApplyToLinearViewObject(
				LinearViewObject linearView,
				LinearViewObject? previous,
				LinearViewObject? next,
				LinearDisassemblyLine[] lines
			)
			{
				if (null == linearView)
				{
					throw new ArgumentNullException(nameof(linearView));
				}

				if (null == lines)
				{
					throw new ArgumentNullException(nameof(lines));
				}

				using (ScopedAllocator allocator = new ScopedAllocator())
				{
					NativeMethods.BNApplyRenderLayerToLinearViewObject(
						this.handle,
						linearView.DangerousGetHandle(),
						null == previous
							? IntPtr.Zero
							: previous.DangerousGetHandle(),
						null == next ? IntPtr.Zero : next.DangerousGetHandle(),
						allocator.ConvertToNativeArrayEx<
							BNLinearDisassemblyLine,
							LinearDisassemblyLine
						>(lines),
						(ulong)lines.Length,
						out IntPtr outputLines,
						out ulong outputLineCount
					);

					return UnsafeUtils.TakeStructArrayEx<
						BNLinearDisassemblyLine,
						LinearDisassemblyLine
					>(
						outputLines,
						outputLineCount,
						LinearDisassemblyLine.FromNative,
						NativeMethods.BNFreeLinearDisassemblyLines
					);
				}
			}
		}
	}
}
