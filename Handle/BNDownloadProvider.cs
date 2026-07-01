using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a registered download provider that can create download instances for
    /// performing HTTP/HTTPS requests. DownloadProvider handles are always borrowed — the
    /// provider lifetime is managed by the native engine's global registry.
    /// </summary>
    public sealed class DownloadProvider : AbstractSafeHandle<DownloadProvider>
    {
        /// <summary>
        /// Initializes a new DownloadProvider wrapper around an existing borrowed handle.
        /// The handle is never owned — the provider lifetime is managed by the native engine.
        /// </summary>
        /// <param name="handle">The native pointer to the BNDownloadProvider object.</param>
        internal DownloadProvider(IntPtr handle)
            : base(handle, false)
        {
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNDownloadProvider pointer.</param>
        /// <returns>A new DownloadProvider instance that will not free the handle on dispose.</returns>
        internal static DownloadProvider? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new DownloadProvider(handle);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNDownloadProvider pointer.</param>
        /// <returns>A new DownloadProvider instance that will not free the handle on dispose.</returns>
        internal static DownloadProvider MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new DownloadProvider(handle);
        }

        /// <summary>
        /// No-op release: download provider handles are always borrowed from the global registry
        /// and must not be freed by this wrapper.
        /// </summary>
        /// <returns>True (always, since no deallocation is performed).</returns>
        protected override bool ReleaseHandle()
        {
            // Provider objects are borrowed from the global registry; the native engine owns their lifetime.
            return true;
        }

        /// <summary>
        /// Gets the registered name that uniquely identifies this download provider.
        /// </summary>
        public string Name
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the provider name.
                IntPtr raw = NativeMethods.BNGetDownloadProviderName(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Creates a new download instance for this provider.
        /// The caller is responsible for disposing the returned instance.
        /// Returns null if the native engine cannot allocate a new instance.
        /// </summary>
        /// <returns>A new owned DownloadInstance, or null on failure.</returns>
        public DownloadInstance? CreateInstance()
        {
            // Create a new instance through the provider; the returned handle is owned.
            return DownloadInstance.TakeHandle(
                NativeMethods.BNCreateDownloadProviderInstance(this.handle)
            );
        }
    }
}
