using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	/// <summary>Arranges flow graph nodes for display.</summary>
	public abstract class FlowGraphLayout : AbstractSafeHandle<FlowGraphLayout>
	{
		private static readonly object registrationLock = new object();

		private static readonly List<FlowGraphLayout> registeredLayouts =
			new List<FlowGraphLayout>();

		private readonly string? registrationName;

		private bool isRegistered;

		private NativeDelegates.BNFlowGraphLayout? layoutCallback;

		/// <summary>Creates an unregistered custom flow graph layout.</summary>
		protected FlowGraphLayout(string name)
			: base(false)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			this.registrationName = name;
		}

		private FlowGraphLayout(IntPtr handle)
			: base(handle, false)
		{
		}

		/// <summary>Gets the unique registered layout name.</summary>
		public string Name
		{
			get
			{
				if (this.IsInvalid)
				{
					return this.registrationName ?? string.Empty;
				}

				return UnsafeUtils.TakeUtf8String(
					NativeMethods.BNGetFlowGraphLayoutName(this.handle)
				);
			}
		}

		/// <summary>Registers this layout and roots its callback for core use.</summary>
		public void Register()
		{
			if (this.isRegistered || !this.IsInvalid)
			{
				throw new InvalidOperationException(
					"The flow graph layout is already registered."
				);
			}

			this.layoutCallback = new NativeDelegates.BNFlowGraphLayout(
				this.InvokeLayout
			);
			BNCustomFlowGraphLayout callbacks = new BNCustomFlowGraphLayout();
			callbacks.context = IntPtr.Zero;
			callbacks.layout = Marshal.GetFunctionPointerForDelegate(
				this.layoutCallback
			);

			IntPtr handle = NativeMethods.BNRegisterFlowGraphLayout(
				this.registrationName ?? string.Empty,
				in callbacks
			);
			if (IntPtr.Zero == handle)
			{
				throw new InvalidOperationException(
					"The core rejected the flow graph layout."
				);
			}

			this.SetHandle(handle);
			this.isRegistered = true;
			lock (FlowGraphLayout.registrationLock)
			{
				FlowGraphLayout.registeredLayouts.Add(this);
			}
		}

		/// <summary>Looks up a registered layout by name.</summary>
		public static FlowGraphLayout? GetByName(string name)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			return FlowGraphLayout.FromHandle(
				NativeMethods.BNGetFlowGraphLayoutByName(name)
			);
		}

		/// <summary>Gets every layout registered with the core.</summary>
		public static unsafe FlowGraphLayout[] GetAll()
		{
			ulong count = 0;
			IntPtr layouts = NativeMethods.BNGetFlowGraphLayouts((IntPtr)(&count));
			return UnsafeUtils.TakeHandleArray<FlowGraphLayout>(
				layouts,
				count,
				FlowGraphLayout.MustFromHandle,
				NativeMethods.BNFreeFlowGraphLayoutList
			);
		}

		/// <summary>Arranges the supplied nodes in a flow graph.</summary>
		public abstract bool Layout(FlowGraph graph, FlowGraphNode[] nodes);

		private static FlowGraphLayout? FromHandle(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				return null;
			}

			return new CoreFlowGraphLayout(handle);
		}

		private static FlowGraphLayout MustFromHandle(IntPtr handle)
		{
			FlowGraphLayout? layout = FlowGraphLayout.FromHandle(handle);
			if (null == layout)
			{
				throw new ArgumentNullException(nameof(handle));
			}

			return layout;
		}

		private bool InvokeLayout(
			IntPtr context,
			IntPtr graph,
			IntPtr nodes,
			ulong nodeCount
		)
		{
			FlowGraph? managedGraph = null;
			List<FlowGraphNode> managedNodes = new List<FlowGraphNode>();
			try
			{
				managedGraph = FlowGraph.MustTakeHandle(
					NativeMethods.BNNewFlowGraphReference(graph)
				);
				for (ulong i = 0; i < nodeCount; i++)
				{
					IntPtr node = Marshal.ReadIntPtr(
						nodes,
						checked((int)(i * (ulong)IntPtr.Size))
					);
					managedNodes.Add(FlowGraphNode.MustNewFromHandle(node));
				}

				return this.Layout(managedGraph, managedNodes.ToArray());
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in FlowGraphLayout.Layout: {0}",
					exception
				);
				return false;
			}
			finally
			{
				foreach (FlowGraphNode node in managedNodes)
				{
					node.Dispose();
				}

				if (null != managedGraph)
				{
					managedGraph.Dispose();
				}
			}
		}

		private sealed class CoreFlowGraphLayout : FlowGraphLayout
		{
			internal CoreFlowGraphLayout(IntPtr handle)
				: base(handle)
			{
			}

			public override unsafe bool Layout(
				FlowGraph graph,
				FlowGraphNode[] nodes
			)
			{
				if (null == graph)
				{
					throw new ArgumentNullException(nameof(graph));
				}

				if (null == nodes)
				{
					throw new ArgumentNullException(nameof(nodes));
				}

				if (0 == nodes.Length)
				{
					return NativeMethods.BNFlowGraphLayoutLayout(
						this.handle,
						graph.DangerousGetHandle(),
						IntPtr.Zero,
						0
					);
				}

				IntPtr[] nodeHandles = new IntPtr[nodes.Length];
				for (int i = 0; i < nodes.Length; i++)
				{
					if (null == nodes[i])
					{
						throw new ArgumentNullException(nameof(nodes));
					}

					nodeHandles[i] = nodes[i].DangerousGetHandle();
				}

				fixed (IntPtr* nodePointer = nodeHandles)
				{
					return NativeMethods.BNFlowGraphLayoutLayout(
						this.handle,
						graph.DangerousGetHandle(),
						(IntPtr)nodePointer,
						(ulong)nodeHandles.Length
					);
				}
			}
		}
	}
}
