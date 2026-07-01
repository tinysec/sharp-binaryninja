using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a registered secrets provider that stores and retrieves encrypted key-value pairs.
    /// Secrets provider handles are always borrowed — the provider lifetime is managed by the
    /// native engine's global registry.
    /// </summary>
    public sealed class SecretsProvider : AbstractSafeHandle<SecretsProvider>
    {
        /// <summary>
        /// Initializes a new SecretsProvider wrapper around an existing borrowed handle.
        /// The handle is never owned — the provider lifetime is managed by the native engine.
        /// </summary>
        /// <param name="handle">The native pointer to the BNSecretsProvider object.</param>
        internal SecretsProvider(IntPtr handle)
            : base(handle, false)
        {
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNSecretsProvider pointer.</param>
        /// <returns>A new SecretsProvider instance that will not free the handle on dispose.</returns>
        internal static SecretsProvider? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new SecretsProvider(handle);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNSecretsProvider pointer.</param>
        /// <returns>A new SecretsProvider instance that will not free the handle on dispose.</returns>
        internal static SecretsProvider MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new SecretsProvider(handle);
        }

        /// <summary>
        /// No-op release: secrets provider handles are always borrowed from the global registry
        /// and must not be freed by this wrapper.
        /// </summary>
        /// <returns>True (always, since no deallocation is performed).</returns>
        protected override bool ReleaseHandle()
        {
            // Provider objects are borrowed from the global registry; the native engine owns their lifetime.
            return true;
        }

        // ===================================================================
        // Static lookup methods
        // ===================================================================

        /// <summary>
        /// Looks up a registered secrets provider by its unique name.
        /// Returns null if no provider with the given name exists.
        /// </summary>
        /// <param name="name">The registered name of the provider to find.</param>
        /// <returns>A borrowed SecretsProvider instance, or null if not found.</returns>
        public static SecretsProvider? GetByName(string name)
        {
            // Query the global registry for a provider with the specified name.
            IntPtr result = NativeMethods.BNGetSecretsProviderByName(name);

            // Wrap as a borrowed handle; returns null when the native pointer is zero.
            return SecretsProvider.BorrowHandle(result);
        }

        /// <summary>
        /// Gets all registered secrets providers from the engine.
        /// Each returned provider is a borrowed reference.
        /// </summary>
        /// <returns>An array of all registered SecretsProvider instances.</returns>
        public static unsafe SecretsProvider[] GetList()
        {
            // 1. Stack-allocate the count variable.
            ulong count = 0;

            // 2. Retrieve the native array of provider pointers.
            IntPtr arrayPointer = NativeMethods.BNGetSecretsProviderList((IntPtr)(&count));

            // 3. Convert to managed array of borrowed handles and free the native pointer array.
            return UnsafeUtils.TakeHandleArray<SecretsProvider>(
                arrayPointer ,
                count ,
                SecretsProvider.MustBorrowHandle ,
                NativeMethods.BNFreeSecretsProviderList
            );
        }

        // ===================================================================
        // Instance properties and methods
        // ===================================================================

        /// <summary>
        /// Gets the registered name that uniquely identifies this secrets provider.
        /// </summary>
        public string Name
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the provider name.
                IntPtr raw = NativeMethods.BNGetSecretsProviderName(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Checks whether any data exists in this provider for the given key.
        /// </summary>
        /// <param name="key">The key to test for presence in the secrets store.</param>
        /// <returns>True if data exists for the key; false otherwise.</returns>
        public bool HasData(string key)
        {
            // Delegate to the native API with the provider handle and the key string.
            return NativeMethods.BNSecretsProviderHasData(this.handle, key ?? string.Empty);
        }

        /// <summary>
        /// Retrieves the stored data for the given key.
        /// Returns null if no data is stored for the key or if retrieval fails.
        /// </summary>
        /// <param name="key">The key whose associated value should be retrieved.</param>
        /// <returns>The stored string value, or null if absent or on error.</returns>
        public string? GetData(string key)
        {
            // 1. Retrieve the native ANSI string pointer for the stored value.
            IntPtr raw = NativeMethods.BNGetSecretsProviderData(this.handle, key ?? string.Empty);

            // 2. Copy and free the native string; null means no data for this key.
            return UnsafeUtils.TakeAnsiString(raw);
        }

        /// <summary>
        /// Stores a value under the given key in this secrets provider.
        /// </summary>
        /// <param name="key">The key under which the data will be stored.</param>
        /// <param name="data">The data string to store.</param>
        /// <returns>True if the data was stored successfully; false otherwise.</returns>
        public bool StoreData(string key, string data)
        {
            // Forward the provider handle, key, and data to the native storage API.
            return NativeMethods.BNStoreSecretsProviderData(
                this.handle,
                key ?? string.Empty,
                data ?? string.Empty
            );
        }

        /// <summary>
        /// Deletes the data stored under the given key in this secrets provider.
        /// </summary>
        /// <param name="key">The key whose associated data should be deleted.</param>
        /// <returns>True if the deletion succeeded; false otherwise.</returns>
        public bool DeleteData(string key)
        {
            // Forward the provider handle and key to the native deletion API.
            return NativeMethods.BNDeleteSecretsProviderData(this.handle, key ?? string.Empty);
        }
    }
}
