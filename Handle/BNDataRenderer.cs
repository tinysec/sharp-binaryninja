using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a data renderer that controls how raw bytes are displayed
    /// in Binary Ninja's data variable view. Callers may register renderers
    /// with the global container as generic or type-specific renderers.
    /// </summary>
    public sealed class DataRenderer : AbstractSafeHandle<DataRenderer>
    {
        /// <summary>
        /// Initializes a new DataRenderer wrapper around an existing native handle.
        /// </summary>
        /// <param name="handle">The native pointer to the BNDataRenderer object.</param>
        /// <param name="owner">True if this wrapper owns the handle and should free it on dispose.</param>
        public DataRenderer(IntPtr handle, bool owner)
            : base(handle, owner)
        {
        }

        /// <summary>
        /// Creates an owned reference by incrementing the native reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNDataRenderer pointer.</param>
        /// <returns>A new owned DataRenderer, or null if the handle is zero.</returns>
        internal static DataRenderer? NewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new DataRenderer(
                NativeMethods.BNNewDataRendererReference(handle),
                true
            );
        }

        /// <summary>
        /// Creates an owned reference by incrementing the native reference count.
        /// Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNDataRenderer pointer.</param>
        /// <returns>A new owned DataRenderer.</returns>
        internal static DataRenderer MustNewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new DataRenderer(
                NativeMethods.BNNewDataRendererReference(handle),
                true
            );
        }

        /// <summary>
        /// Takes ownership of an existing handle without incrementing the reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNDataRenderer pointer.</param>
        /// <returns>A new owned DataRenderer, or null if the handle is zero.</returns>
        internal static DataRenderer? TakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new DataRenderer(handle, true);
        }

        /// <summary>
        /// Takes ownership of an existing handle without incrementing the reference count.
        /// Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNDataRenderer pointer.</param>
        /// <returns>A new owned DataRenderer.</returns>
        internal static DataRenderer MustTakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new DataRenderer(handle, true);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNDataRenderer pointer.</param>
        /// <returns>A new DataRenderer that will not free the handle on dispose.</returns>
        internal static DataRenderer? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new DataRenderer(handle, false);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNDataRenderer pointer.</param>
        /// <returns>A new DataRenderer that will not free the handle on dispose.</returns>
        internal static DataRenderer MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new DataRenderer(handle, false);
        }

        /// <summary>
        /// Releases the native BNDataRenderer handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native renderer handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeDataRenderer(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Registers this renderer as a generic data renderer in the given container.
        /// Generic renderers are used as a fallback when no type-specific renderer matches.
        /// </summary>
        /// <param name="container">The global data renderer container to register with.</param>
        public void RegisterGeneric(DataRendererContainer container)
        {
            // 1. Validate that the container is not null before registering.
            if (null == container)
            {
                throw new ArgumentNullException(nameof(container));
            }

            // 2. Forward this renderer and the container to the native registration API.
            NativeMethods.BNRegisterGenericDataRenderer(
                container.DangerousGetHandle(),
                this.handle
            );
        }

        /// <summary>
        /// Registers this renderer as a type-specific data renderer in the given container.
        /// Type-specific renderers are tried before generic renderers for matching data types.
        /// </summary>
        /// <param name="container">The global data renderer container to register with.</param>
        public void RegisterTypeSpecific(DataRendererContainer container)
        {
            // 1. Validate that the container is not null before registering.
            if (null == container)
            {
                throw new ArgumentNullException(nameof(container));
            }

            // 2. Forward this renderer and the container to the native registration API.
            NativeMethods.BNRegisterTypeSpecificDataRenderer(
                container.DangerousGetHandle(),
                this.handle
            );
        }
    }
}
