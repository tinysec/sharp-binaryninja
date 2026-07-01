using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class RepositoryManager : AbstractSafeHandle<RepositoryManager>
	{
	    internal RepositoryManager(IntPtr handle , bool owner)
		    : base(handle , owner)
	    {
	        
	    }

	    internal static RepositoryManager? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }

		    // Core no longer exposes an addref for repository managers; wrap as non-owning.
		    return new RepositoryManager(handle, false);
	    }

	    internal static RepositoryManager MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }

		    // Core no longer exposes an addref for repository managers; wrap as non-owning.
		    return new RepositoryManager(handle, false);
	    }

	    internal static RepositoryManager? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }

		    // Core no longer frees repository managers; wrap as non-owning.
		    return new RepositoryManager(handle, false);
	    }

	    internal static RepositoryManager MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }

		    // Core no longer frees repository managers; wrap as non-owning.
		    return new RepositoryManager(handle, false);
	    }
	    
	    internal static RepositoryManager? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new RepositoryManager(handle, false);
	    }
	    
	    internal static RepositoryManager MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new RepositoryManager(handle, false);
	    }
	    
        /// <summary>
        /// Releases the native BNRepositoryManager handle when this instance is disposed or finalized.
        /// Core no longer exposes a free/addref for repository managers, so the handle is
        /// always non-owning and there is nothing to release.
        /// </summary>
        /// <returns>True, since no native resource is owned by this instance.</returns>
        protected override bool ReleaseHandle()
        {
            return true;
        }

        // ===================================================================
        // Instance properties and methods
        // ===================================================================

        /// <summary>
        /// Gets all repositories registered with this manager.
        /// Each returned repository is a new owned reference.
        /// </summary>
        public static unsafe Repository[] Repositories
        {
            get
            {
                // 1. Stack-allocate the count variable and retrieve the native repository array.
                ulong count = 0;

                IntPtr ptr = NativeMethods.BNRepositoryManagerGetRepositories(
                    (IntPtr)(&count)
                );

                // 2. Convert the pointer array of BNRepository* into managed Repository objects.
                return UnsafeUtils.TakeHandleArray<Repository>(
                    ptr,
                    count,
                    Repository.MustNewFromHandle,
                    NativeMethods.BNFreeRepositoryManagerRepositoriesList
                );
            }
        }

        /// <summary>
        /// Gets the default repository managed by this manager.
        /// Returns null if no default repository is configured.
        /// </summary>
        public static Repository? DefaultRepository
        {
            get
            {
                // GetDefaultRepository returns a borrowed reference; wrap with NewFromHandle to addref.
                return Repository.NewFromHandle(
                    NativeMethods.BNRepositoryManagerGetDefaultRepository()
                );
            }
        }

        /// <summary>
        /// Checks for updates to all plugins in all registered repositories.
        /// This call may contact remote servers.
        /// </summary>
        /// <returns>True if the check succeeded; false on failure.</returns>
        public static bool CheckForUpdates()
        {
            // Delegate to the native update-check API.
            return NativeMethods.BNRepositoryManagerCheckForUpdates();
        }

        /// <summary>
        /// Adds a new repository to this manager at the given URL and local path.
        /// </summary>
        /// <param name="url">The remote URL of the repository.</param>
        /// <param name="repoPath">The local filesystem path where the repository will be stored.</param>
        /// <returns>True if the repository was added successfully; false otherwise.</returns>
        public static bool AddRepository(string url, string repoPath)
        {
            // Forward the URL and path to the native API (global repository manager).
            return NativeMethods.BNRepositoryManagerAddRepository(
                url ?? string.Empty,
                repoPath ?? string.Empty
            );
        }

        /// <summary>
        /// Looks up a repository in this manager by its local filesystem path.
        /// Returns null if no repository is found at the given path.
        /// </summary>
        /// <param name="repoPath">The local filesystem path of the repository.</param>
        /// <returns>The matching Repository as a new owned reference, or null if not found.</returns>
        public static Repository? GetRepositoryByPath(string repoPath)
        {
            // Query the global repository manager for a repository at the specified path.
            return Repository.NewFromHandle(
                NativeMethods.BNRepositoryGetRepositoryByPath(
                    repoPath ?? string.Empty
                )
            );
        }
    }
}