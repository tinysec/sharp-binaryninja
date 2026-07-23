using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a registered scripting provider (e.g., Python) that can create
    /// scripting instances for interactive script execution. ScriptingProvider handles
    /// are always borrowed — the provider lifetime is managed by the native engine.
    /// </summary>
    public sealed class ScriptingProvider : AbstractSafeHandle<ScriptingProvider>
    {
        /// <summary>
        /// Initializes a new ScriptingProvider wrapper around an existing borrowed handle.
        /// The handle is never owned — the provider lifetime is managed by the native engine.
        /// </summary>
        /// <param name="handle">The native pointer to the BNScriptingProvider object.</param>
        internal ScriptingProvider(IntPtr handle)
            : base(handle, false)
        {
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNScriptingProvider pointer.</param>
        /// <returns>A new ScriptingProvider instance that will not free the handle on dispose.</returns>
        internal static ScriptingProvider? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new ScriptingProvider(handle);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNScriptingProvider pointer.</param>
        /// <returns>A new ScriptingProvider instance that will not free the handle on dispose.</returns>
        internal static ScriptingProvider MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new ScriptingProvider(handle);
        }

        /// <summary>
        /// No-op release: scripting provider handles are always borrowed from the global registry
        /// and must not be freed by this wrapper.
        /// </summary>
        /// <returns>True (always, since no deallocation is performed).</returns>
        protected override bool ReleaseHandle()
        {
            // Provider objects are borrowed from the global registry; the native engine owns their lifetime.
            return true;
        }

        /// <summary>
        /// Gets the human-readable display name of this scripting provider (e.g., "Python 3").
        /// </summary>
        public string Name
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the provider display name.
                IntPtr raw = NativeMethods.BNGetScriptingProviderName(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the API name used to programmatically identify this scripting provider (e.g., "python3").
        /// </summary>
        public string ApiName
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the provider API name.
                IntPtr raw = NativeMethods.BNGetScriptingProviderAPIName(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>Gets every registered scripting provider.</summary>
        /// <returns>The registered providers as borrowed handles.</returns>
        public static unsafe ScriptingProvider[] GetList()
        {
            ulong count = 0;
            IntPtr providers = NativeMethods.BNGetScriptingProviderList(
                (IntPtr)(&count)
            );
            return UnsafeUtils.TakeHandleArray<ScriptingProvider>(
                providers,
                count,
                ScriptingProvider.MustBorrowHandle,
                NativeMethods.BNFreeScriptingProviderList
            );
        }

        /// <summary>Looks up a scripting provider by its display name.</summary>
        /// <param name="name">The provider display name.</param>
        /// <returns>The borrowed provider, or null when no provider has that name.</returns>
        public static ScriptingProvider? GetByName(string name)
        {
            if (null == name)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return ScriptingProvider.BorrowHandle(
                NativeMethods.BNGetScriptingProviderByName(name)
            );
        }

        /// <summary>Looks up a scripting provider by its programmatic API name.</summary>
        /// <param name="name">The provider API name.</param>
        /// <returns>The borrowed provider, or null when no provider has that API name.</returns>
        public static ScriptingProvider? GetByApiName(string name)
        {
            if (null == name)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return ScriptingProvider.BorrowHandle(
                NativeMethods.BNGetScriptingProviderByAPIName(name)
            );
        }

        /// <summary>
        /// Creates a new scripting instance for this provider.
        /// The caller is responsible for disposing the returned instance.
        /// Returns null if the native engine cannot allocate a new instance.
        /// </summary>
        /// <returns>A new owned ScriptingInstance, or null on failure.</returns>
        public ScriptingInstance? CreateInstance()
        {
            // Create a new scripting instance through the provider; the returned handle is owned.
            return ScriptingInstance.TakeHandle(
                NativeMethods.BNCreateScriptingProviderInstance(this.handle)
            );
        }
    }
}
