using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents an asynchronous flow graph layout operation. The request tracks whether
    /// the layout algorithm has finished arranging the graph's nodes. The caller owns the
    /// request handle returned by the layout engine and is responsible for disposing it.
    /// </summary>
    public class FlowGraphLayoutRequest : AbstractSafeHandle<FlowGraphLayoutRequest>
    {
        /// <summary>
        /// Initializes a new FlowGraphLayoutRequest wrapper around an existing native handle.
        /// </summary>
        /// <param name="handle">The native pointer to the BNFlowGraphLayoutRequest object.</param>
        /// <param name="owner">True if this wrapper owns the handle and should free it on dispose.</param>
        internal FlowGraphLayoutRequest(IntPtr handle, bool owner)
            : base(handle, owner)
        {
        }

        /// <summary>
        /// Creates an owned reference by incrementing the native reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNFlowGraphLayoutRequest pointer.</param>
        /// <returns>A new owned FlowGraphLayoutRequest, or null if the handle is zero.</returns>
        internal static FlowGraphLayoutRequest? NewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new FlowGraphLayoutRequest(
                NativeMethods.BNNewFlowGraphLayoutRequestReference(handle),
                true
            );
        }

        /// <summary>
        /// Creates an owned reference by incrementing the native reference count.
        /// Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNFlowGraphLayoutRequest pointer.</param>
        /// <returns>A new owned FlowGraphLayoutRequest.</returns>
        internal static FlowGraphLayoutRequest MustNewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new FlowGraphLayoutRequest(
                NativeMethods.BNNewFlowGraphLayoutRequestReference(handle),
                true
            );
        }

        /// <summary>
        /// Takes ownership of an existing handle without incrementing the reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNFlowGraphLayoutRequest pointer.</param>
        /// <returns>A new owned FlowGraphLayoutRequest, or null if the handle is zero.</returns>
        internal static FlowGraphLayoutRequest? TakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new FlowGraphLayoutRequest(handle, true);
        }

        /// <summary>
        /// Takes ownership of an existing handle without incrementing the reference count.
        /// Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNFlowGraphLayoutRequest pointer.</param>
        /// <returns>A new owned FlowGraphLayoutRequest.</returns>
        internal static FlowGraphLayoutRequest MustTakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new FlowGraphLayoutRequest(handle, true);
        }

        /// <summary>
        /// Releases the native BNFlowGraphLayoutRequest handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native layout request and mark it invalid to prevent double-free.
                NativeMethods.BNFreeFlowGraphLayoutRequest(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Gets a value indicating whether the layout operation has finished.
        /// True means the nodes have been positioned and the graph is ready to render.
        /// </summary>
        public bool Complete
        {
            get
            {
                // Query the native engine for the completion status of this layout operation.
                return NativeMethods.BNIsFlowGraphLayoutRequestComplete(this.handle);
            }
        }

        /// <summary>
        /// Gets the flow graph that this layout request is operating on.
        /// Returns null if the native engine cannot resolve the graph.
        /// </summary>
        public FlowGraph? Graph
        {
            get
            {
                // Retrieve a new reference to the graph being laid out.
                return FlowGraph.TakeHandle(
                    NativeMethods.BNGetGraphForFlowGraphLayoutRequest(this.handle)
                );
            }
        }

        /// <summary>
        /// Requests cancellation of this layout operation. The layout may not stop immediately.
        /// </summary>
        public void Abort()
        {
            // Delegate to the native abort API.
            NativeMethods.BNAbortFlowGraphLayoutRequest(this.handle);
        }
    }
}
