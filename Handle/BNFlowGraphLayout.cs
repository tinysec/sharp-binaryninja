using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a registered flow graph layout algorithm that can arrange the nodes
    /// of a flow graph for display. FlowGraphLayout handles are always borrowed —
    /// the layout lifetime is managed by the native engine's global registry.
    /// </summary>
    public sealed class FlowGraphLayout : AbstractSafeHandle<FlowGraphLayout>
    {
        /// <summary>
        /// Initializes a new FlowGraphLayout wrapper around an existing borrowed handle.
        /// The handle is never owned — the layout lifetime is managed by the native engine.
        /// </summary>
        /// <param name="handle">The native pointer to the BNFlowGraphLayout object.</param>
        internal FlowGraphLayout(IntPtr handle)
            : base(handle, false)
        {
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNFlowGraphLayout pointer.</param>
        /// <returns>A new FlowGraphLayout that will not free the handle on dispose.</returns>
        internal static FlowGraphLayout? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new FlowGraphLayout(handle);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNFlowGraphLayout pointer.</param>
        /// <returns>A new FlowGraphLayout that will not free the handle on dispose.</returns>
        internal static FlowGraphLayout MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new FlowGraphLayout(handle);
        }

        /// <summary>
        /// No-op release: flow graph layout handles are always borrowed from the global registry
        /// and must not be freed by this wrapper.
        /// </summary>
        /// <returns>True (always, since no deallocation is performed).</returns>
        protected override bool ReleaseHandle()
        {
            // Layout objects are borrowed from the global registry; the native engine owns their lifetime.
            return true;
        }

        /// <summary>
        /// Gets the registered name that uniquely identifies this flow graph layout algorithm.
        /// </summary>
        public string Name
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the layout name.
                IntPtr raw = NativeMethods.BNGetFlowGraphLayoutName(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Applies this layout algorithm to the given flow graph, arranging the specified nodes.
        /// </summary>
        /// <param name="graph">The flow graph whose nodes will be laid out.</param>
        /// <param name="nodes">The array of nodes to arrange; may be empty but not null.</param>
        /// <returns>True if the layout completed successfully; false on failure.</returns>
        public unsafe bool Layout(FlowGraph graph, FlowGraphNode[] nodes)
        {
            // 1. Validate required parameters.
            if (null == graph)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            if (null == nodes)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            // 2. Handle the empty-node case without pinning an empty array.
            if (0 == nodes.Length)
            {
                return NativeMethods.BNFlowGraphLayoutLayout(
                    this.handle,
                    graph.DangerousGetHandle(),
                    IntPtr.Zero,
                    0
                );
            }

            // 3. Collect raw native pointers from each managed node wrapper.
            IntPtr[] nodePointers = new IntPtr[nodes.Length];
            for (int i = 0; i < nodes.Length; i++)
            {
                // Each element is the BNFlowGraphNode* for that node.
                nodePointers[i] = nodes[i].DangerousGetHandle();
            }

            // 4. Pin the pointer array in memory and pass it to the native layout function.
            fixed (IntPtr* ptr = nodePointers)
            {
                return NativeMethods.BNFlowGraphLayoutLayout(
                    this.handle,
                    graph.DangerousGetHandle(),
                    (IntPtr)ptr,
                    (ulong)nodes.Length
                );
            }
        }

        /// <summary>
        /// Returns all flow graph layout algorithms registered with the native engine.
        /// Each returned layout is a borrowed reference valid for the engine lifetime.
        /// </summary>
        /// <returns>An array of all registered FlowGraphLayout instances.</returns>
        public static unsafe FlowGraphLayout[] GetAll()
        {
            // 1. Retrieve the full list of registered layouts from the native engine.
            ulong count = 0;
            IntPtr ptr = NativeMethods.BNGetFlowGraphLayouts((IntPtr)(&count));

            // 2. Convert the pointer array into managed wrappers and free the native list.
            return UnsafeUtils.TakeHandleArray<FlowGraphLayout>(
                ptr,
                count,
                FlowGraphLayout.MustBorrowHandle,
                NativeMethods.BNFreeFlowGraphLayoutList
            );
        }

        /// <summary>
        /// Looks up a registered flow graph layout by its registered name.
        /// Returns null if no layout is registered under the given name.
        /// </summary>
        /// <param name="name">The registered name to look up.</param>
        /// <returns>The matching FlowGraphLayout, or null if not found.</returns>
        public static FlowGraphLayout? GetByName(string name)
        {
            // 1. Query the engine's registry for a layout matching the given name.
            IntPtr raw = NativeMethods.BNGetFlowGraphLayoutByName(name ?? string.Empty);

            // 2. Wrap as a borrowed reference; null if the engine returned a null pointer.
            return FlowGraphLayout.BorrowHandle(raw);
        }
    }
}
