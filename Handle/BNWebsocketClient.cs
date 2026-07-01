using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a WebSocket client instance created by a WebSocket provider.
    /// The caller takes ownership of the handle returned from the provider's CreateClient method.
    /// </summary>
    public sealed class WebsocketClient : AbstractSafeHandle<WebsocketClient>
    {
        /// <summary>
        /// Initializes a new WebsocketClient wrapper around an existing native handle.
        /// </summary>
        /// <param name="handle">The native pointer to the BNWebsocketClient object.</param>
        /// <param name="owner">True if this wrapper owns the handle and should free it on dispose.</param>
        internal WebsocketClient(IntPtr handle, bool owner)
            : base(handle, owner)
        {
        }

        /// <summary>
        /// Creates an owned reference by incrementing the native reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNWebsocketClient pointer.</param>
        /// <returns>A new owned WebsocketClient, or null if the handle is zero.</returns>
        internal static WebsocketClient? NewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new WebsocketClient(
                NativeMethods.BNNewWebsocketClientReference(handle),
                true
            );
        }

        /// <summary>
        /// Creates an owned reference by incrementing the native reference count.
        /// Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNWebsocketClient pointer.</param>
        /// <returns>A new owned WebsocketClient.</returns>
        internal static WebsocketClient MustNewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new WebsocketClient(
                NativeMethods.BNNewWebsocketClientReference(handle),
                true
            );
        }

        /// <summary>
        /// Takes ownership of an existing handle without incrementing the reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNWebsocketClient pointer.</param>
        /// <returns>A new owned WebsocketClient, or null if the handle is zero.</returns>
        internal static WebsocketClient? TakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new WebsocketClient(handle, true);
        }

        /// <summary>
        /// Takes ownership of an existing handle without incrementing the reference count.
        /// Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNWebsocketClient pointer.</param>
        /// <returns>A new owned WebsocketClient.</returns>
        internal static WebsocketClient MustTakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new WebsocketClient(handle, true);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNWebsocketClient pointer.</param>
        /// <returns>A new WebsocketClient that will not free the handle on dispose.</returns>
        internal static WebsocketClient? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new WebsocketClient(handle, false);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNWebsocketClient pointer.</param>
        /// <returns>A new WebsocketClient that will not free the handle on dispose.</returns>
        internal static WebsocketClient MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new WebsocketClient(handle, false);
        }

        /// <summary>
        /// Releases the native BNWebsocketClient handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native client handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeWebsocketClient(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Disconnects this WebSocket client, closing the underlying connection.
        /// </summary>
        /// <returns>True if the disconnect succeeded; false on failure.</returns>
        public bool Disconnect()
        {
            // Delegate to the native disconnect API.
            return NativeMethods.BNDisconnectWebsocketClient(this.handle);
        }

        /// <summary>
        /// Writes raw byte data to the WebSocket connection.
        /// The caller is responsible for ensuring the pointer is valid for the given length.
        /// </summary>
        /// <param name="data">Pointer to the byte buffer to send.</param>
        /// <param name="length">Number of bytes to send from the buffer.</param>
        /// <returns>The number of bytes actually written; zero on failure.</returns>
        public ulong WriteData(IntPtr data, ulong length)
        {
            // Forward the raw data pointer and byte count to the native write API.
            return NativeMethods.BNWriteWebsocketClientData(this.handle, data, length);
        }
    }
}
