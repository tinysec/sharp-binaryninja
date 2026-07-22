using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public partial class FlowGraph
	{
		private static readonly object externalReferenceLock = new object();

		private static readonly List<FlowGraph> externallyReferencedGraphs =
			new List<FlowGraph>();

		private NativeDelegates.BNCustomFlowGraphEvent? prepareForLayoutCallback;

		private NativeDelegates.BNCustomFlowGraphEvent? populateNodesCallback;

		private NativeDelegates.BNCustomFlowGraphEvent? completeLayoutCallback;

		private NativeDelegates.BNCustomFlowGraphUpdate? updateCallback;

		private NativeDelegates.BNCustomFlowGraphEvent? externalReferenceTakenCallback;

		private NativeDelegates.BNCustomFlowGraphEvent? externalReferenceReleasedCallback;

		/// <summary>Prepares this graph before the layout worker requests nodes.</summary>
		protected virtual void PrepareForLayout()
		{
			this.FinishPrepareForLayout();
		}

		/// <summary>Populates this graph on the layout worker thread.</summary>
		protected virtual void PopulateNodes()
		{
		}

		/// <summary>Runs after the core completes graph layout.</summary>
		protected virtual void CompleteLayout()
		{
		}

		private void InitializeCustomGraph()
		{
			this.prepareForLayoutCallback = new NativeDelegates.BNCustomFlowGraphEvent(
				this.InvokePrepareForLayout
			);
			this.populateNodesCallback = new NativeDelegates.BNCustomFlowGraphEvent(
				this.InvokePopulateNodes
			);
			this.completeLayoutCallback = new NativeDelegates.BNCustomFlowGraphEvent(
				this.InvokeCompleteLayout
			);
			this.updateCallback = new NativeDelegates.BNCustomFlowGraphUpdate(
				this.InvokeUpdate
			);
			this.externalReferenceTakenCallback =
				new NativeDelegates.BNCustomFlowGraphEvent(
					this.InvokeExternalReferenceTaken
				);
			this.externalReferenceReleasedCallback =
				new NativeDelegates.BNCustomFlowGraphEvent(
					this.InvokeExternalReferenceReleased
				);

			BNCustomFlowGraph callbacks = new BNCustomFlowGraph();
			callbacks.context = IntPtr.Zero;
			callbacks.prepareForLayout = Marshal.GetFunctionPointerForDelegate(
				this.prepareForLayoutCallback
			);
			callbacks.populateNodes = Marshal.GetFunctionPointerForDelegate(
				this.populateNodesCallback
			);
			callbacks.completeLayout = Marshal.GetFunctionPointerForDelegate(
				this.completeLayoutCallback
			);
			callbacks.update = Marshal.GetFunctionPointerForDelegate(
				this.updateCallback
			);
			callbacks.freeObject = IntPtr.Zero;
			callbacks.externalRefTaken = Marshal.GetFunctionPointerForDelegate(
				this.externalReferenceTakenCallback
			);
			callbacks.externalRefReleased = Marshal.GetFunctionPointerForDelegate(
				this.externalReferenceReleasedCallback
			);

			IntPtr handle = NativeMethods.BNCreateCustomFlowGraph(in callbacks);
			if (IntPtr.Zero == handle)
			{
				throw new InvalidOperationException(
					"The core rejected the custom flow graph."
				);
			}

			this.SetHandle(handle);
		}

		private void InvokePrepareForLayout(IntPtr context)
		{
			try
			{
				this.PrepareForLayout();
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in FlowGraph.PrepareForLayout: {0}",
					exception
				);
				this.FinishPrepareForLayout();
			}
		}

		private void InvokePopulateNodes(IntPtr context)
		{
			try
			{
				this.PopulateNodes();
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in FlowGraph.PopulateNodes: {0}",
					exception
				);
			}
		}

		private void InvokeCompleteLayout(IntPtr context)
		{
			try
			{
				this.CompleteLayout();
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in FlowGraph.CompleteLayout: {0}",
					exception
				);
			}
		}

		private IntPtr InvokeUpdate(IntPtr context)
		{
			try
			{
				FlowGraph? graph = this.Update();
				if (null == graph)
				{
					return IntPtr.Zero;
				}

				return NativeMethods.BNNewFlowGraphReference(
					graph.DangerousGetHandle()
				);
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in FlowGraph.Update: {0}",
					exception
				);
				return IntPtr.Zero;
			}
		}

		private void InvokeExternalReferenceTaken(IntPtr context)
		{
			lock (FlowGraph.externalReferenceLock)
			{
				FlowGraph.externallyReferencedGraphs.Add(this);
			}
		}

		private void InvokeExternalReferenceReleased(IntPtr context)
		{
			lock (FlowGraph.externalReferenceLock)
			{
				FlowGraph.externallyReferencedGraphs.Remove(this);
			}
		}

		private sealed class CoreFlowGraph : FlowGraph
		{
			internal CoreFlowGraph(IntPtr handle, bool owner)
				: base(handle, owner)
			{
			}

			public override bool HasUpdates
			{
				get
				{
					if (!this.UpdateQueryMode)
					{
						return false;
					}

					return NativeMethods.BNFlowGraphHasUpdates(this.handle);
				}
			}

			public override FlowGraph? Update()
			{
				return FlowGraph.TakeHandle(
					NativeMethods.BNUpdateFlowGraph(this.handle)
				);
			}
		}
	}
}
