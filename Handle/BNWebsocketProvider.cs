using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a registered WebSocket provider that can create WebSocket client instances.
    /// WebsocketProvider handles are always borrowed — the provider lifetime is managed by
    /// the native engine's global registry.
    /// </summary>
    public sealed class WebsocketProvider : AbstractSafeHandle<WebsocketProvider>
    {
        /// <summary>
        /// Initializes a new WebsocketProvider wrapper around an existing borrowed handle.
        /// The handle is never owned — the provider lifetime is managed by the native engine.
        /// </summary>
        /// <param name="handle">The native pointer to the BNWebsocketProvider object.</param>
        internal WebsocketProvider(IntPtr handle)
            : base(handle, false)
        {
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNWebsocketProvider pointer.</param>
        /// <returns>A new WebsocketProvider instance that will not free the handle on dispose.</returns>
        internal static WebsocketProvider? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new WebsocketProvider(handle);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNWebsocketProvider pointer.</param>
        /// <returns>A new WebsocketProvider instance that will not free the handle on dispose.</returns>
        internal static WebsocketProvider MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new WebsocketProvider(handle);
        }

        /// <summary>
        /// No-op release: WebSocket provider handles are always borrowed from the global registry
        /// and must not be freed by this wrapper.
        /// </summary>
        /// <returns>True (always, since no deallocation is performed).</returns>
        protected override bool ReleaseHandle()
        {
            // Provider objects are borrowed from the global registry; the native engine owns their lifetime.
            return true;
        }

        /// <summary>
        /// Gets the registered name that uniquely identifies this WebSocket provider.
        /// </summary>
        public string Name
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the provider name.
                IntPtr raw = NativeMethods.BNGetWebsocketProviderName(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Creates a new WebSocket client instance for this provider.
        /// The caller is responsible for disposing the returned client.
        /// Returns null if the native engine cannot allocate a new client.
        /// </summary>
        /// <returns>A new owned WebsocketClient, or null on failure.</returns>
        public WebsocketClient? CreateClient()
        {
            // Create a new client through the provider; the returned handle is owned.
            return WebsocketClient.TakeHandle(
                NativeMethods.BNCreateWebsocketProviderClient(this.handle)
            );
        }
    }
}
