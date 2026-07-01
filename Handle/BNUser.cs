using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a Binary Ninja user account.
    /// A user record holds an identifier, display name, and email address.
    /// User objects are returned from database and collaboration APIs that
    /// record which user performed each action.
    /// </summary>
    public sealed class User : AbstractSafeHandle<User>
    {
        /// <summary>
        /// Initializes a new User wrapper around an existing native handle.
        /// </summary>
        /// <param name="handle">The native pointer to the BNUser object.</param>
        /// <param name="owner">True if this instance owns the handle and should free it on dispose.</param>
        internal User(IntPtr handle, bool owner)
            : base(handle, owner)
        {
        }

        /// <summary>
        /// Creates a new managed User by incrementing the reference count on an existing native handle.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNUser pointer.</param>
        /// <returns>A new User instance, or null if handle is zero.</returns>
        internal static User? NewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new User(
                NativeMethods.BNNewUserReference(handle),
                true
            );
        }

        /// <summary>
        /// Creates a new managed User by incrementing the reference count. Throws if handle is zero.
        /// </summary>
        /// <param name="handle">The native BNUser pointer.</param>
        /// <returns>A new User instance.</returns>
        internal static User MustNewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new User(
                NativeMethods.BNNewUserReference(handle),
                true
            );
        }

        /// <summary>
        /// Takes ownership of an existing native handle without incrementing the reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNUser pointer.</param>
        /// <returns>A new User instance, or null if handle is zero.</returns>
        internal static User? TakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new User(handle, true);
        }

        /// <summary>
        /// Takes ownership of an existing native handle without incrementing the reference count. Throws if zero.
        /// </summary>
        /// <param name="handle">The native BNUser pointer.</param>
        /// <returns>A new User instance.</returns>
        internal static User MustTakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new User(handle, true);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNUser pointer.</param>
        /// <returns>A new User instance that will not free the handle on dispose.</returns>
        internal static User? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new User(handle, false);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNUser pointer.</param>
        /// <returns>A new User instance that will not free the handle on dispose.</returns>
        internal static User MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new User(handle, false);
        }

        /// <summary>
        /// Releases the native BNUser handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native user handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeUser(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Gets the unique string identifier for this user.
        /// The ID is assigned by the platform and never changes.
        /// </summary>
        public string Id
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the user ID.
                IntPtr raw = NativeMethods.BNGetUserId(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the display name of this user.
        /// </summary>
        public string Name
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the user name.
                IntPtr raw = NativeMethods.BNGetUserName(this.handle);

                // 2. Copy and free the native string.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the email address of this user.
        /// </summary>
        public string Email
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the user email.
                IntPtr raw = NativeMethods.BNGetUserEmail(this.handle);

                // 2. Copy and free the native string.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }
    }
}
