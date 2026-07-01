using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a plugin repository containing a collection of Binary Ninja plugins
    /// hosted at a remote URL and cloned to a local path.
    /// </summary>
    public sealed class Repository : AbstractSafeHandle<Repository>
    {
        /// <summary>
        /// Initializes a new Repository wrapper around an existing native handle.
        /// </summary>
        /// <param name="handle">The native pointer to the BNRepository object.</param>
        /// <param name="owner">True if this wrapper owns the handle and should free it on dispose.</param>
        public Repository(IntPtr handle, bool owner)
            : base(handle, owner)
        {
        }

        /// <summary>
        /// Creates an owned reference by incrementing the native reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRepository pointer.</param>
        /// <returns>A new owned Repository, or null if the handle is zero.</returns>
        internal static Repository? NewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new Repository(
                NativeMethods.BNNewRepositoryReference(handle),
                true
            );
        }

        /// <summary>
        /// Creates an owned reference by incrementing the native reference count.
        /// Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRepository pointer.</param>
        /// <returns>A new owned Repository.</returns>
        internal static Repository MustNewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new Repository(
                NativeMethods.BNNewRepositoryReference(handle),
                true
            );
        }

        /// <summary>
        /// Takes ownership of an existing handle without incrementing the reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRepository pointer.</param>
        /// <returns>A new owned Repository, or null if the handle is zero.</returns>
        internal static Repository? TakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new Repository(handle, true);
        }

        /// <summary>
        /// Takes ownership of an existing handle without incrementing the reference count.
        /// Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRepository pointer.</param>
        /// <returns>A new owned Repository.</returns>
        internal static Repository MustTakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new Repository(handle, true);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRepository pointer.</param>
        /// <returns>A new Repository that will not free the handle on dispose.</returns>
        internal static Repository? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new Repository(handle, false);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRepository pointer.</param>
        /// <returns>A new Repository that will not free the handle on dispose.</returns>
        internal static Repository MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new Repository(handle, false);
        }

        /// <summary>
        /// Releases the native BNRepository handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native repository and mark it invalid to prevent double-free.
                NativeMethods.BNFreeRepository(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Gets the remote URL from which this repository was cloned.
        /// </summary>
        public string Url
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the repository URL.
                IntPtr raw = NativeMethods.BNRepositoryGetUrl(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the local filesystem path where this repository's plugins are stored.
        /// </summary>
        public string PluginsPath
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the plugins directory path.
                IntPtr raw = NativeMethods.BNRepositoryGetPluginsPath(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the local filesystem path where this repository itself is cloned.
        /// </summary>
        public string RepoPath
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the repository root path.
                IntPtr raw = NativeMethods.BNRepositoryGetRepoPath(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets all plugins available in this repository.
        /// Each returned plugin is a new owned reference.
        /// </summary>
        /// <returns>An array of all plugins in this repository.</returns>
        public unsafe RepoPlugin[] GetPlugins()
        {
            // 1. Stack-allocate the count variable and retrieve the native plugin array.
            ulong count = 0;
            IntPtr ptr = NativeMethods.BNRepositoryGetPlugins(
                this.handle,
                (IntPtr)(&count)
            );

            // 2. Convert the pointer array of BNRepoPlugin* into managed RepoPlugin objects.
            return UnsafeUtils.TakeHandleArray<RepoPlugin>(
                ptr,
                count,
                RepoPlugin.MustNewFromHandle,
                NativeMethods.BNFreeRepositoryPluginList
            );
        }

        /// <summary>
        /// Looks up a plugin in this repository by its relative path within the plugins directory.
        /// Returns null if no plugin is found at the given path.
        /// </summary>
        /// <param name="pluginPath">The relative path of the plugin within the plugins directory.</param>
        /// <returns>The matching RepoPlugin as a new owned reference, or null if not found.</returns>
        public RepoPlugin? GetPluginByPath(string pluginPath)
        {
            // Query the repository for a plugin at the specified relative path.
            return RepoPlugin.NewFromHandle(
                NativeMethods.BNRepositoryGetPluginByPath(
                    this.handle,
                    pluginPath ?? string.Empty
                )
            );
        }
    }
}
