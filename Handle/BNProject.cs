using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a Binary Ninja project handle.
    /// Project management APIs require an enterprise license and are not implemented here.
    /// Only the ValidPluginCommands query is exposed as it uses a non-enterprise entry point.
    /// </summary>
    public sealed class Project : AbstractSafeHandle<Project>
    {
        /// <summary>
        /// Initializes a new Project wrapper around an existing native handle.
        /// </summary>
        /// <param name="handle">The native pointer to the BNProject object.</param>
        /// <param name="owner">True if this instance owns the handle and should free it on dispose.</param>
        internal Project(IntPtr handle, bool owner)
            : base(handle, owner)
        {
        }

        /// <summary>
        /// Creates a new managed Project by incrementing the reference count on an existing native handle.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNProject pointer.</param>
        /// <returns>A new Project instance, or null if handle is zero.</returns>
        internal static Project? NewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new Project(
                NativeMethods.BNNewProjectReference(handle),
                true
            );
        }

        /// <summary>
        /// Creates a new managed Project by incrementing the reference count. Throws if handle is zero.
        /// </summary>
        /// <param name="handle">The native BNProject pointer.</param>
        /// <returns>A new Project instance.</returns>
        internal static Project MustNewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new Project(
                NativeMethods.BNNewProjectReference(handle),
                true
            );
        }

        /// <summary>
        /// Takes ownership of an existing native handle without incrementing the reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNProject pointer.</param>
        /// <returns>A new Project instance, or null if handle is zero.</returns>
        internal static Project? TakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new Project(handle, true);
        }

        /// <summary>
        /// Takes ownership of an existing native handle without incrementing the reference count. Throws if zero.
        /// </summary>
        /// <param name="handle">The native BNProject pointer.</param>
        /// <returns>A new Project instance.</returns>
        internal static Project MustTakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new Project(handle, true);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNProject pointer.</param>
        /// <returns>A new Project instance that will not free the handle on dispose.</returns>
        internal static Project? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new Project(handle, false);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNProject pointer.</param>
        /// <returns>A new Project instance that will not free the handle on dispose.</returns>
        internal static Project MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new Project(handle, false);
        }

        /// <summary>
        /// Releases the native BNProject handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native project handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeProject(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Gets all plugin commands that are valid to run in the context of this project.
        /// </summary>
        public PluginCommand[] ValidPluginCommands
        {
            get
            {
                // Retrieve the list of valid plugin commands using an out count parameter.
                IntPtr arrayPointer = NativeMethods.BNGetValidPluginCommandsForProject(
                    this.handle,
                    out ulong arrayLength
                );

                return UnsafeUtils.TakeStructArray<BNPluginCommand, PluginCommand>(
                    arrayPointer,
                    arrayLength,
                    PluginCommand.FromNative,
                    NativeMethods.BNFreePluginCommandList
                );
            }
        }
    }
}
