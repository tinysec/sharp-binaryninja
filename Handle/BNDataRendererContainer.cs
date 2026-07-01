using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents the global container that holds all registered data renderers.
    /// DataRendererContainer handles are always borrowed — the container lifetime is
    /// managed by the native engine's global state.
    /// </summary>
    public sealed class DataRendererContainer : AbstractSafeHandle<DataRendererContainer>
    {
        /// <summary>
        /// Initializes a new DataRendererContainer wrapper around an existing borrowed handle.
        /// The handle is never owned — the container lifetime is managed by the native engine.
        /// </summary>
        /// <param name="handle">The native pointer to the BNDataRendererContainer object.</param>
        internal DataRendererContainer(IntPtr handle)
            : base(handle, false)
        {
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNDataRendererContainer pointer.</param>
        /// <returns>A new DataRendererContainer that will not free the handle on dispose.</returns>
        internal static DataRendererContainer? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new DataRendererContainer(handle);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNDataRendererContainer pointer.</param>
        /// <returns>A new DataRendererContainer that will not free the handle on dispose.</returns>
        internal static DataRendererContainer MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new DataRendererContainer(handle);
        }

        /// <summary>
        /// No-op release: the data renderer container handle is owned by the native engine's
        /// global state and must not be freed by this wrapper.
        /// </summary>
        /// <returns>True (always, since no deallocation is performed).</returns>
        protected override bool ReleaseHandle()
        {
            // Container is borrowed from the global engine state; the native engine owns its lifetime.
            return true;
        }

        /// <summary>
        /// Gets the global data renderer container instance from the native engine.
        /// Returns null if the engine has not yet initialized the container.
        /// </summary>
        /// <returns>The global DataRendererContainer, or null if unavailable.</returns>
        public static DataRendererContainer? GetInstance()
        {
            // 1. Retrieve the globally registered container from the native engine.
            IntPtr raw = NativeMethods.BNGetDataRendererContainer();

            // 2. Wrap as a borrowed reference — the engine owns the container lifetime.
            return DataRendererContainer.BorrowHandle(raw);
        }
    }
}
