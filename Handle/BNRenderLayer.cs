using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class RenderLayer : AbstractSafeHandle<RenderLayer>
	{
		internal RenderLayer(IntPtr handle)
			:base(handle, false)
		{

		}

		internal static RenderLayer FromHandle(IntPtr handle)
		{
			return new RenderLayer(handle);
		}

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRenderLayer pointer.</param>
        /// <returns>A new RenderLayer instance that will not free the handle on dispose.</returns>
        internal static RenderLayer? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new RenderLayer(handle);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRenderLayer pointer.</param>
        /// <returns>A new RenderLayer instance that will not free the handle on dispose.</returns>
        internal static RenderLayer MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new RenderLayer(handle);
        }

        /// <summary>
        /// No-op release: render layer handles are always borrowed from the global registry
        /// and must not be freed by this wrapper.
        /// </summary>
        /// <returns>True (always, since no deallocation is performed).</returns>
        protected override bool ReleaseHandle()
        {
            // RenderLayer objects are borrowed from the global registry; the native engine owns their lifetime.
            return true;
        }
		
		public string Name
		{
			get
			{
				return UnsafeUtils.TakeAnsiString(
					NativeMethods.BNGetRenderLayerName(this.handle)
				);
			}
		}

		public void ApplyToFlowGraph(FlowGraph graph)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				NativeMethods.BNApplyRenderLayerToFlowGraph(
					this.handle, 
					graph.DangerousGetHandle()
				);
			}
		}
		
		
		public LinearDisassemblyLine[] ApplyToLinearViewObject(
			LinearViewObject linearView,
			LinearViewObject? prev,
			LinearViewObject? next,
			LinearDisassemblyLine[] lines
		)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				NativeMethods.BNApplyRenderLayerToLinearViewObject(
					this.handle,
					linearView.DangerousGetHandle(),
					null == prev ? IntPtr.Zero : prev.DangerousGetHandle(),
					null == next ? IntPtr.Zero : next.DangerousGetHandle(),
					allocator.ConvertToNativeArrayEx<BNLinearDisassemblyLine,LinearDisassemblyLine>(lines),
					(ulong)lines.Length,
					out IntPtr outLines,
					out ulong outLineCount
				);

				return UnsafeUtils.TakeStructArrayEx<BNLinearDisassemblyLine , LinearDisassemblyLine>(
					outLines ,
					outLineCount,
					LinearDisassemblyLine.FromNative,
					NativeMethods.BNFreeLinearDisassemblyLines
				);
			}
		}

        // ===================================================================
        // Static lookup methods
        // ===================================================================

        /// <summary>
        /// Looks up a registered render layer by its unique name.
        /// Returns null if no render layer with the given name exists.
        /// </summary>
        /// <param name="name">The registered name of the render layer to find.</param>
        /// <returns>A borrowed RenderLayer instance, or null if not found.</returns>
        public static RenderLayer? GetByName(string name)
        {
            // Query the global registry for a render layer with the specified name.
            IntPtr result = NativeMethods.BNGetRenderLayerByName(name);

            // Wrap as a borrowed handle; returns null when the native pointer is zero.
            return RenderLayer.BorrowHandle(result);
        }

        /// <summary>
        /// Gets the default enable state of a render layer by querying its handle.
        /// The render layer must already be resolved (use GetByName first).
        /// </summary>
        /// <returns>The default enable state for this render layer.</returns>
        public RenderLayerDefaultEnableState DefaultEnableState
        {
            get
            {
                // Query the native API for the default enable state of this layer.
                return NativeMethods.BNGetRenderLayerDefaultEnableState(this.handle);
            }
        }

        /// <summary>
        /// Gets all registered render layers from the engine.
        /// Each returned render layer is a borrowed reference.
        /// </summary>
        /// <returns>An array of all registered RenderLayer instances.</returns>
        public static unsafe RenderLayer[] GetList()
        {
            // 1. Stack-allocate the count variable.
            ulong count = 0;

            // 2. Retrieve the native array of render layer pointers.
            IntPtr arrayPointer = NativeMethods.BNGetRenderLayerList((IntPtr)(&count));

            // 3. Convert to managed array of borrowed handles and free the native pointer array.
            return UnsafeUtils.TakeHandleArray<RenderLayer>(
                arrayPointer ,
                count ,
                RenderLayer.MustBorrowHandle ,
                NativeMethods.BNFreeRenderLayerList
            );
        }
	}
}