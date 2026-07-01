using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a plugin entry within a Binary Ninja plugin repository.
    /// RepoPlugin handles may be borrowed (non-owning) or owned (via NewFromHandle).
    /// Owned handles are freed via BNFreePlugin on dispose.
    /// </summary>
    public sealed class RepoPlugin : AbstractSafeHandle<RepoPlugin>
    {
        /// <summary>
        /// Initializes a new RepoPlugin wrapper around an existing native handle.
        /// </summary>
        /// <param name="handle">The native pointer to the BNRepoPlugin object.</param>
        /// <param name="owner">True if this wrapper owns the handle and should free it on dispose.</param>
        internal RepoPlugin(IntPtr handle, bool owner)
            : base(handle, owner)
        {
        }

        /// <summary>
        /// Creates an owned reference by incrementing the native reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRepoPlugin pointer.</param>
        /// <returns>A new owned RepoPlugin, or null if the handle is zero.</returns>
        internal static RepoPlugin? NewFromHandle(IntPtr handle)
        {
            if (IntPtr.Zero == handle)
            {
                return null;
            }

            // Increment native refcount and take ownership.
            return new RepoPlugin(
                NativeMethods.BNNewPluginReference(handle),
                true
            );
        }

        /// <summary>
        /// Creates an owned reference by incrementing the native reference count.
        /// Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRepoPlugin pointer.</param>
        /// <returns>A new owned RepoPlugin.</returns>
        internal static RepoPlugin MustNewFromHandle(IntPtr handle)
        {
            if (IntPtr.Zero == handle)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            // Increment native refcount and take ownership.
            return new RepoPlugin(
                NativeMethods.BNNewPluginReference(handle),
                true
            );
        }

        /// <summary>
        /// Takes ownership of an existing handle without incrementing the reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRepoPlugin pointer.</param>
        /// <returns>A new owned RepoPlugin, or null if the handle is zero.</returns>
        internal static RepoPlugin? TakeHandle(IntPtr handle)
        {
            if (IntPtr.Zero == handle)
            {
                return null;
            }

            return new RepoPlugin(handle, true);
        }

        /// <summary>
        /// Takes ownership of an existing handle without incrementing the reference count.
        /// Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRepoPlugin pointer.</param>
        /// <returns>A new owned RepoPlugin.</returns>
        internal static RepoPlugin MustTakeHandle(IntPtr handle)
        {
            if (IntPtr.Zero == handle)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new RepoPlugin(handle, true);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRepoPlugin pointer.</param>
        /// <returns>A new RepoPlugin instance that will not free the handle on dispose.</returns>
        internal static RepoPlugin? BorrowHandle(IntPtr handle)
        {
            if (IntPtr.Zero == handle)
            {
                return null;
            }

            return new RepoPlugin(handle, false);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNRepoPlugin pointer.</param>
        /// <returns>A new RepoPlugin instance that will not free the handle on dispose.</returns>
        internal static RepoPlugin MustBorrowHandle(IntPtr handle)
        {
            if (IntPtr.Zero == handle)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new RepoPlugin(handle, false);
        }

        /// <summary>
        /// Releases the native BNRepoPlugin handle when this instance is disposed or finalized.
        /// Only frees the handle if this wrapper owns it.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native plugin and mark it invalid to prevent double-free.
                NativeMethods.BNFreePlugin(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Gets the unique name of this plugin within its repository.
        /// </summary>
        public string Name
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the plugin name.
                IntPtr raw = NativeMethods.BNPluginGetName(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the human-readable description of what this plugin does.
        /// </summary>
        public string Description
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the plugin description.
                IntPtr raw = NativeMethods.BNPluginGetDescription(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the version string of this plugin.
        /// </summary>
        public string Version
        {
            get
            {
                // 1. Retrieve the current version record (owns its strings until freed).
                BNPluginVersion version = NativeMethods.BNPluginGetCurrentVersion(this.handle);

                try
                {
                    // 2. Copy the borrowed version string, returning empty on null.
                    return UnsafeUtils.ReadAnsiString(version.versionString) ?? string.Empty;
                }
                finally
                {
                    // 3. Release the version record's native allocations.
                    NativeMethods.BNPluginFreeVersion(version);
                }
            }
        }

        /// <summary>
        /// Gets the path of the plugin on the local filesystem (within the repository clone).
        /// </summary>
        public string Path
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the plugin path.
                IntPtr raw = NativeMethods.BNPluginGetPath(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the name of the repository this plugin belongs to.
        /// </summary>
        public string Repository
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the repository name.
                IntPtr raw = NativeMethods.BNPluginGetRepository(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the current installation and runtime status of this plugin.
        /// </summary>
        public PluginStatus Status
        {
            get
            {
                // Retrieve the native status enum value.
                return NativeMethods.BNPluginGetPluginStatus(this.handle);
            }
        }

        /// <summary>
        /// Gets whether this plugin is currently enabled.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                // Query the native layer for the enabled flag.
                return NativeMethods.BNPluginIsEnabled(this.handle);
            }
        }

        /// <summary>
        /// Gets whether this plugin is currently installed on the local system.
        /// </summary>
        public bool IsInstalled
        {
            get
            {
                // Query the native layer for the installed flag.
                return NativeMethods.BNPluginIsInstalled(this.handle);
            }
        }

        /// <summary>
        /// Gets whether this plugin is currently loaded and running.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                // Query the native layer for the running flag.
                return NativeMethods.BNPluginIsRunning(this.handle);
            }
        }

        /// <summary>
        /// Gets whether an updated version of this plugin is available in the repository.
        /// </summary>
        public bool IsUpdateAvailable
        {
            get
            {
                // Query the native layer for the update-available flag.
                return NativeMethods.BNPluginIsUpdateAvailable(this.handle);
            }
        }

        /// <summary>
        /// Enables this plugin. Pass force=true to enable even when dependencies are missing.
        /// </summary>
        /// <param name="force">When true, enables the plugin even with missing dependencies.</param>
        /// <returns>True if the plugin was successfully enabled; false otherwise.</returns>
        public bool Enable(bool force = false)
        {
            // Delegate to the native enable API with the optional force flag.
            return NativeMethods.BNPluginEnable(this.handle, force);
        }

        /// <summary>
        /// Disables this plugin so it will not be loaded on next restart.
        /// </summary>
        /// <returns>True if the plugin was successfully disabled; false otherwise.</returns>
        public bool Disable()
        {
            // Delegate to the native disable API.
            return NativeMethods.BNPluginDisable(this.handle);
        }

        /// <summary>
        /// Installs this plugin from the repository.
        /// </summary>
        /// <returns>True if the plugin was successfully installed; false otherwise.</returns>
        public bool Install(string versionID = "")
        {
            // Delegate to the native install API (versionID selects a specific version).
            return NativeMethods.BNPluginInstall(this.handle , versionID);
        }

        /// <summary>
        /// Uninstalls this plugin, removing it from the local system.
        /// </summary>
        /// <returns>True if the plugin was successfully uninstalled; false otherwise.</returns>
        public bool Uninstall()
        {
            // Delegate to the native uninstall API.
            return NativeMethods.BNPluginUninstall(this.handle);
        }

        /// <summary>
        /// Updates this plugin to the latest available version in the repository.
        /// </summary>
        /// <returns>True if the plugin was successfully updated; false otherwise.</returns>
        public bool Update(string versionID = "")
        {
            // Delegate to the native update API (versionID selects a specific version).
            return NativeMethods.BNPluginUpdate(this.handle , versionID);
        }

        // ────────────────────────────────────────────────────────────
        //  String properties
        // ────────────────────────────────────────────────────────────

        /// <summary>
        /// Gets the author name of this plugin.
        /// </summary>
        public string Author
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the plugin author.
                IntPtr raw = NativeMethods.BNPluginGetAuthor(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the URL of the plugin author's website or profile.
        /// </summary>
        public string AuthorUrl
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the author URL.
                IntPtr raw = NativeMethods.BNPluginGetAuthorUrl(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the commit hash of this plugin's current version in the repository.
        /// </summary>
        public string Commit
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the plugin commit.
                IntPtr raw = NativeMethods.BNPluginGetCommit(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the dependency specification string for this plugin.
        /// </summary>
        public string Dependencies
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the plugin dependencies.
                IntPtr raw = NativeMethods.BNPluginGetDependencies(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the full license text of this plugin.
        /// </summary>
        public string LicenseText
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the license text.
                IntPtr raw = NativeMethods.BNPluginGetLicenseText(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the long-form description of this plugin with additional details.
        /// </summary>
        public string LongDescription
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the long description.
                IntPtr raw = NativeMethods.BNPluginGetLongdescription(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the URL of the downloadable package for this plugin.
        /// </summary>
        public string PackageUrl
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the package URL.
                IntPtr raw = NativeMethods.BNPluginGetPackageUrl(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the project data string associated with this plugin.
        /// </summary>
        public string ProjectData
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the project data.
                IntPtr raw = NativeMethods.BNPluginGetProjectData(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the URL of the project page or source repository for this plugin.
        /// </summary>
        public string ProjectUrl
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the project URL.
                IntPtr raw = NativeMethods.BNPluginGetProjectUrl(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the subdirectory path of this plugin within its repository.
        /// </summary>
        public string Subdir
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the plugin subdirectory.
                IntPtr raw = NativeMethods.BNPluginGetSubdir(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        // ────────────────────────────────────────────────────────────
        //  Bool properties
        // ────────────────────────────────────────────────────────────

        /// <summary>
        /// Gets whether this plugin is view-only and cannot modify the binary view.
        /// </summary>
        public bool ViewOnly
        {
            get
            {
                // Query the native layer for the view-only flag.
                return NativeMethods.BNPluginGetViewOnly(this.handle);
            }
        }

        /// <summary>
        /// Gets whether this plugin's dependencies are currently being installed.
        /// </summary>
        public bool AreDependenciesBeingInstalled
        {
            get
            {
                // Query the native layer for the dependencies-being-installed flag.
                return NativeMethods.BNPluginAreDependenciesBeingInstalled(this.handle);
            }
        }

        /// <summary>
        /// Gets whether this plugin is currently in the process of being deleted.
        /// </summary>
        public bool IsBeingDeleted
        {
            get
            {
                // Query the native layer for the being-deleted flag.
                return NativeMethods.BNPluginIsBeingDeleted(this.handle);
            }
        }

        /// <summary>
        /// Gets whether this plugin is currently in the process of being updated.
        /// </summary>
        public bool IsBeingUpdated
        {
            get
            {
                // Query the native layer for the being-updated flag.
                return NativeMethods.BNPluginIsBeingUpdated(this.handle);
            }
        }

        /// <summary>
        /// Gets whether a delete operation is pending for this plugin.
        /// </summary>
        public bool IsDeletePending
        {
            get
            {
                // Query the native layer for the delete-pending flag.
                return NativeMethods.BNPluginIsDeletePending(this.handle);
            }
        }

        /// <summary>
        /// Gets whether a disable operation is pending for this plugin.
        /// </summary>
        public bool IsDisablePending
        {
            get
            {
                // Query the native layer for the disable-pending flag.
                return NativeMethods.BNPluginIsDisablePending(this.handle);
            }
        }

        /// <summary>
        /// Gets whether an update operation is pending for this plugin.
        /// </summary>
        public bool IsUpdatePending
        {
            get
            {
                // Query the native layer for the update-pending flag.
                return NativeMethods.BNPluginIsUpdatePending(this.handle);
            }
        }

        // ────────────────────────────────────────────────────────────
        //  Numeric property
        // ────────────────────────────────────────────────────────────

        /// <summary>
        /// Gets the timestamp (as a Unix epoch value) of the last update to this plugin.
        /// </summary>
        public ulong LastUpdate
        {
            get
            {
                // Query the native layer for the last-update timestamp.
                return NativeMethods.BNPluginGetLastUpdate(this.handle);
            }
        }

        // ────────────────────────────────────────────────────────────
        //  Bool method
        // ────────────────────────────────────────────────────────────

        /// <summary>
        /// Installs the dependencies required by this plugin.
        /// </summary>
        /// <returns>True if the dependencies were successfully installed; false otherwise.</returns>
        public bool InstallDependencies()
        {
            // Delegate to the native install-dependencies API.
            return NativeMethods.BNPluginInstallDependencies(this.handle);
        }

        // ────────────────────────────────────────────────────────────
        //  Struct return properties (BNVersionInfo)
        // ────────────────────────────────────────────────────────────

        /// <summary>
        /// Gets the maximum Binary Ninja version this plugin is compatible with.
        /// </summary>
        public VersionInfo MaximumVersion
        {
            get
            {
                // 1. Retrieve the native BNVersionInfo struct for the maximum version.
                BNVersionInfo raw = NativeMethods.BNPluginGetMaximumVersionInfo(this.handle);

                // 2. Convert the native struct into a managed VersionInfo object.
                return VersionInfo.FromNative(raw);
            }
        }

        /// <summary>
        /// Gets the minimum Binary Ninja version this plugin requires.
        /// </summary>
        public VersionInfo MinimumVersion
        {
            get
            {
                // 1. Retrieve the native BNVersionInfo struct for the minimum version.
                BNVersionInfo raw = NativeMethods.BNPluginGetMinimumVersionInfo(this.handle);

                // 2. Convert the native struct into a managed VersionInfo object.
                return VersionInfo.FromNative(raw);
            }
        }

        // ────────────────────────────────────────────────────────────
        //  String array properties
        // ────────────────────────────────────────────────────────────

        /// <summary>
        /// Gets the list of API names (e.g., "python3") supported by this plugin.
        /// </summary>
        public unsafe string[] Apis
        {
            get
            {
                // 1. Stack-allocate the count variable and retrieve the native string array.
                ulong count = 0;
                IntPtr ptr = NativeMethods.BNPluginGetApis(
                    this.handle,
                    (IntPtr)(&count)
                );

                // 2. Copy the native string array into managed strings, then free the native list.
                return UnsafeUtils.TakeAnsiStringArray(
                    ptr,
                    count,
                    NativeMethods.BNFreeStringList
                );
            }
        }

        /// <summary>
        /// Gets the list of platform identifiers (e.g., "linux", "windows") this plugin supports.
        /// </summary>
        public unsafe string[] Platforms
        {
            get
            {
                // 1. Stack-allocate the count variable and retrieve the native string array.
                ulong count = 0;
                IntPtr ptr = NativeMethods.BNPluginGetPlatforms(
                    this.handle,
                    (IntPtr)(&count)
                );

                // 2. Copy the native string array into managed strings, then free the native list.
                return UnsafeUtils.TakeAnsiStringArray(
                    ptr,
                    count,
                    NativeMethods.BNFreeStringList
                );
            }
        }

        // ────────────────────────────────────────────────────────────
        //  Enum array property
        // ────────────────────────────────────────────────────────────

        /// <summary>
        /// Gets the array of plugin type classifications for this plugin.
        /// </summary>
        public unsafe PluginType[] PluginTypes
        {
            get
            {
                // 1. Stack-allocate the count variable and retrieve the native enum array.
                ulong count = 0;
                IntPtr ptr = NativeMethods.BNPluginGetPluginTypes(
                    this.handle,
                    (IntPtr)(&count)
                );

                // 2. Copy the native enum array into managed PluginType values, then free.
                return UnsafeUtils.TakeNumberArray<PluginType>(
                    ptr,
                    count,
                    NativeMethods.BNFreePluginTypes
                );
            }
        }
    }
}
