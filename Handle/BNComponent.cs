using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a Binary Ninja Component — a named, hierarchical grouping of functions,
    /// data variables, and sub-components within a BinaryView. Components allow analysts
    /// to organise and annotate related portions of an analysed binary into logical units.
    /// Each component has a unique GUID, an optional parent, and belongs to exactly one view.
    /// </summary>
    public sealed class Component : AbstractSafeHandle<Component>
    {
        /// <summary>
        /// Initializes a new Component wrapper around an existing native handle.
        /// </summary>
        /// <param name="handle">The native pointer to the BNComponent object.</param>
        /// <param name="owner">True if this instance owns the handle and should free it on dispose.</param>
        internal Component(IntPtr handle, bool owner)
            : base(handle, owner)
        {
        }

        /// <summary>
        /// Creates a new managed Component by incrementing the reference count on an existing native handle.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNComponent pointer.</param>
        /// <returns>A new Component instance, or null if handle is zero.</returns>
        internal static Component? NewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new Component(
                NativeMethods.BNNewComponentReference(handle),
                true
            );
        }

        /// <summary>
        /// Creates a new managed Component by incrementing the reference count. Throws if handle is zero.
        /// </summary>
        /// <param name="handle">The native BNComponent pointer.</param>
        /// <returns>A new Component instance.</returns>
        internal static Component MustNewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new Component(
                NativeMethods.BNNewComponentReference(handle),
                true
            );
        }

        /// <summary>
        /// Takes ownership of an existing native handle without incrementing the reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNComponent pointer.</param>
        /// <returns>A new Component instance, or null if handle is zero.</returns>
        internal static Component? TakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new Component(handle, true);
        }

        /// <summary>
        /// Takes ownership of an existing native handle without incrementing the reference count. Throws if zero.
        /// </summary>
        /// <param name="handle">The native BNComponent pointer.</param>
        /// <returns>A new Component instance.</returns>
        internal static Component MustTakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new Component(handle, true);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNComponent pointer.</param>
        /// <returns>A new Component instance that will not free the handle on dispose.</returns>
        internal static Component? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new Component(handle, false);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNComponent pointer.</param>
        /// <returns>A new Component instance that will not free the handle on dispose.</returns>
        internal static Component MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new Component(handle, false);
        }

        /// <summary>
        /// Releases the native BNComponent handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native component handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeComponent(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Gets the unique GUID string that identifies this component across analysis sessions.
        /// The GUID is assigned at creation time and never changes.
        /// </summary>
        public string Guid
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the GUID.
                IntPtr raw = NativeMethods.BNComponentGetGuid(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the display name of this component.
        /// The display name may differ from the original name when special formatting is applied.
        /// </summary>
        public string DisplayName
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the display name.
                IntPtr raw = NativeMethods.BNComponentGetDisplayName(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the original name of this component as it was assigned at creation time,
        /// before any display-name transformations are applied.
        /// </summary>
        public string OriginalName
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the original name.
                IntPtr raw = NativeMethods.BNComponentGetOriginalName(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the name of this component.
        /// Getting returns the original name; setting forwards the new name to the native layer.
        /// </summary>
        public string Name
        {
            get
            {
                // Delegate to OriginalName for the canonical stored value.
                return this.OriginalName;
            }

            set
            {
                // Forward the new name to the native API, substituting empty for null.
                NativeMethods.BNComponentSetName(this.handle, value ?? string.Empty);
            }
        }

        /// <summary>
        /// Gets the parent component of this component in the hierarchy.
        /// Returns null if this component is the root.
        /// </summary>
        public Component? Parent
        {
            get
            {
                // The native getter returns a new owned reference; take ownership without addref.
                return Component.TakeHandle(
                    NativeMethods.BNComponentGetParent(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets the BinaryView that owns this component.
        /// </summary>
        public BinaryView? View
        {
            get
            {
                // The native getter returns a new owned reference; take ownership without addref.
                return BinaryNinja.BinaryView.TakeHandle(
                    NativeMethods.BNComponentGetView(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets all functions that are directly contained in this component.
        /// Functions inside sub-components are not included in this list.
        /// </summary>
        public Function[] ContainedFunctions
        {
            get
            {
                unsafe
                {
                    // 1. Stack-allocate the count variable and pass its pointer to the native API.
                    ulong count = 0;
                    IntPtr arrayPointer = NativeMethods.BNComponentGetContainedFunctions(
                        this.handle,
                        (IntPtr)(&count)
                    );

                    // 2. Convert the native pointer array to managed Function objects and free native memory.
                    return UnsafeUtils.TakeHandleArrayEx<Function>(
                        arrayPointer,
                        count,
                        Function.MustNewFromHandle,
                        NativeMethods.BNFreeFunctionList
                    );
                }
            }
        }

        /// <summary>
        /// Gets all sub-components that are direct children of this component.
        /// Grandchild components are not included in this list.
        /// </summary>
        public Component[] ContainedComponents
        {
            get
            {
                unsafe
                {
                    // 1. Stack-allocate the count variable and pass its pointer to the native API.
                    ulong count = 0;
                    IntPtr arrayPointer = NativeMethods.BNComponentGetContainedComponents(
                        this.handle,
                        (IntPtr)(&count)
                    );

                    // 2. Convert the native pointer array to managed Component objects and free native memory.
                    return UnsafeUtils.TakeHandleArrayEx<Component>(
                        arrayPointer,
                        count,
                        Component.MustNewFromHandle,
                        NativeMethods.BNFreeComponents
                    );
                }
            }
        }

        /// <summary>
        /// Gets all data variables that are directly contained in this component.
        /// Data variables inside sub-components are not included in this list.
        /// </summary>
        public DataVariable[] ContainedDataVariables
        {
            get
            {
                unsafe
                {
                    // 1. Stack-allocate the count variable and pass its pointer to the native API.
                    ulong count = 0;
                    IntPtr arrayPointer = NativeMethods.BNComponentGetContainedDataVariables(
                        this.handle,
                        (IntPtr)(&count)
                    );

                    // 2. Convert the native BNDataVariable struct array to managed DataVariable objects.
                    return UnsafeUtils.TakeStructArrayEx<BNDataVariable, DataVariable>(
                        arrayPointer,
                        count,
                        native => DataVariable.FromNative(native, this.View),
                        NativeMethods.BNFreeDataVariables
                    );
                }
            }
        }

        /// <summary>
        /// Gets all types that are referenced by code or data directly within this component.
        /// Types referenced by sub-components are not included.
        /// </summary>
        public BinaryNinja.Type[] ReferencedTypes
        {
            get
            {
                unsafe
                {
                    // 1. Stack-allocate the count variable and pass its pointer to the native API.
                    ulong count = 0;
                    IntPtr arrayPointer = NativeMethods.BNComponentGetReferencedTypes(
                        this.handle,
                        (IntPtr)(&count)
                    );

                    // 2. Convert the native pointer array to managed Type objects.
                    //    Use BNComponentFreeReferencedTypes (not BNFreeTypeList) as the free function.
                    return UnsafeUtils.TakeHandleArrayEx<BinaryNinja.Type>(
                        arrayPointer,
                        count,
                        BinaryNinja.Type.MustNewFromHandle,
                        NativeMethods.BNComponentFreeReferencedTypes
                    );
                }
            }
        }

        /// <summary>
        /// Gets all data variables that are referenced by code directly within this component.
        /// Data variables referenced by sub-components are not included.
        /// </summary>
        public DataVariable[] ReferencedDataVariables
        {
            get
            {
                // 1. Retrieve the native struct array; this overload uses out ulong, no unsafe block required.
                IntPtr arrayPointer = NativeMethods.BNComponentGetReferencedDataVariables(
                    this.handle,
                    out ulong count
                );

                // 2. Convert the native BNDataVariable struct array to managed DataVariable objects.
                return UnsafeUtils.TakeStructArrayEx<BNDataVariable, DataVariable>(
                    arrayPointer,
                    count,
                    native => DataVariable.FromNative(native, this.View),
                    NativeMethods.BNFreeDataVariables
                );
            }
        }

        /// <summary>
        /// Gets all types referenced by code or data within this component and all its
        /// sub-components recursively.
        /// </summary>
        public BinaryNinja.Type[] ReferencedTypesRecursive
        {
            get
            {
                unsafe
                {
                    // 1. Stack-allocate the count variable and pass its pointer to the native API.
                    ulong count = 0;
                    IntPtr arrayPointer = NativeMethods.BNComponentGetReferencedTypesRecursive(
                        this.handle,
                        (IntPtr)(&count)
                    );

                    // 2. Convert the native pointer array to managed Type objects.
                    //    Use BNComponentFreeReferencedTypes as the free function for this recursive variant.
                    return UnsafeUtils.TakeHandleArrayEx<BinaryNinja.Type>(
                        arrayPointer,
                        count,
                        BinaryNinja.Type.MustNewFromHandle,
                        NativeMethods.BNComponentFreeReferencedTypes
                    );
                }
            }
        }

        /// <summary>
        /// Gets all data variables referenced by code within this component and all its
        /// sub-components recursively.
        /// </summary>
        public DataVariable[] ReferencedDataVariablesRecursive
        {
            get
            {
                unsafe
                {
                    // 1. Stack-allocate the count variable and pass its pointer to the native API.
                    ulong count = 0;
                    IntPtr arrayPointer = NativeMethods.BNComponentGetReferencedDataVariablesRecursive(
                        this.handle,
                        (IntPtr)(&count)
                    );

                    // 2. Convert the native BNDataVariable struct array to managed DataVariable objects.
                    return UnsafeUtils.TakeStructArrayEx<BNDataVariable, DataVariable>(
                        arrayPointer,
                        count,
                        native => DataVariable.FromNative(native, this.View),
                        NativeMethods.BNFreeDataVariables
                    );
                }
            }
        }

        /// <summary>
        /// Adds the given component as a direct child of this component.
        /// </summary>
        /// <param name="component">The component to nest inside this component.</param>
        /// <returns>True if the operation succeeded; false if the component could not be added.</returns>
        public bool AddComponent(Component component)
        {
            // Forward the parent handle (this) and the child handle to the native API.
            return NativeMethods.BNComponentAddComponent(
                this.handle,
                component.DangerousGetHandle()
            );
        }

        /// <summary>
        /// Removes this component from its parent, detaching it from the component hierarchy.
        /// </summary>
        /// <returns>True if the operation succeeded; false if the removal failed.</returns>
        public bool RemoveComponent()
        {
            // Instruct the native layer to detach this component from its parent.
            return NativeMethods.BNComponentRemoveComponent(this.handle);
        }

        /// <summary>
        /// Adds a reference to the given function inside this component.
        /// </summary>
        /// <param name="function">The function to associate with this component.</param>
        /// <returns>True if the operation succeeded; false if the reference could not be added.</returns>
        public bool AddFunctionReference(Function function)
        {
            // Forward both native handles to the native API.
            return NativeMethods.BNComponentAddFunctionReference(
                this.handle,
                function.DangerousGetHandle()
            );
        }

        /// <summary>
        /// Removes the reference to the given function from this component.
        /// </summary>
        /// <param name="function">The function to disassociate from this component.</param>
        /// <returns>True if the operation succeeded; false if the removal failed.</returns>
        public bool RemoveFunctionReference(Function function)
        {
            // Forward both native handles to the native API.
            return NativeMethods.BNComponentRemoveFunctionReference(
                this.handle,
                function.DangerousGetHandle()
            );
        }

        /// <summary>
        /// Adds the data variable at the given virtual address to this component.
        /// </summary>
        /// <param name="address">The virtual address of the data variable to add.</param>
        /// <returns>True if the operation succeeded; false if the variable could not be added.</returns>
        public bool AddDataVariable(ulong address)
        {
            // Pass the component handle and the variable address to the native API.
            return NativeMethods.BNComponentAddDataVariable(this.handle, address);
        }

        /// <summary>
        /// Removes the data variable at the given virtual address from this component.
        /// </summary>
        /// <param name="address">The virtual address of the data variable to remove.</param>
        /// <returns>True if the operation succeeded; false if the variable could not be removed.</returns>
        public bool RemoveDataVariable(ulong address)
        {
            // Pass the component handle and the variable address to the native API.
            return NativeMethods.BNComponentRemoveDataVariable(this.handle, address);
        }

        /// <summary>
        /// Checks whether this component holds a direct reference to the given function.
        /// </summary>
        /// <param name="function">The function to look for.</param>
        /// <returns>True if the function is a direct member of this component.</returns>
        public bool ContainsFunction(Function function)
        {
            // Ask the native layer whether the function handle is a member of this component.
            return NativeMethods.BNComponentContainsFunction(
                this.handle,
                function.DangerousGetHandle()
            );
        }

        /// <summary>
        /// Checks whether the given component is a direct child of this component.
        /// </summary>
        /// <param name="component">The component to look for.</param>
        /// <returns>True if the component is a direct child of this component.</returns>
        public bool ContainsComponent(Component component)
        {
            // Ask the native layer whether the child handle is nested inside this component.
            return NativeMethods.BNComponentContainsComponent(
                this.handle,
                component.DangerousGetHandle()
            );
        }

        /// <summary>
        /// Checks whether this component holds a reference to the data variable at the given address.
        /// </summary>
        /// <param name="address">The virtual address of the data variable to look for.</param>
        /// <returns>True if the data variable is a direct member of this component.</returns>
        public bool ContainsDataVariable(ulong address)
        {
            // Ask the native layer whether the address belongs to this component.
            return NativeMethods.BNComponentContainsDataVariable(this.handle, address);
        }

        /// <summary>
        /// Copies all members (functions, data variables, and sub-components) from the given source
        /// component into this component. The source component is not modified.
        /// </summary>
        /// <param name="source">The component whose members should be copied into this component.</param>
        public void AddAllMembersFromComponent(Component source)
        {
            // Forward both native handles to the native API; the source is the second argument.
            NativeMethods.BNComponentAddAllMembersFromComponent(
                this.handle,
                source.DangerousGetHandle()
            );
        }

        /// <summary>
        /// Removes all function references from this component.
        /// Data variables and sub-components remain intact.
        /// </summary>
        public void RemoveAllFunctions()
        {
            // Instruct the native layer to clear all function associations on this component.
            NativeMethods.BNComponentRemoveAllFunctions(this.handle);
        }

        /// <summary>
        /// Checks structural equality: returns true if this component and the given component
        /// refer to the same underlying native object (same GUID and same view).
        /// </summary>
        /// <param name="other">The component to compare against.</param>
        /// <returns>True if both instances wrap the same native component; false otherwise.</returns>
        public new bool Equals(Component? other)
        {
            if (other == null)
            {
                // A non-null component is never equal to null.
                return false;
            }

            // Delegate to the native equality check which compares component identity.
            return NativeMethods.BNComponentsEqual(
                this.handle,
                other.DangerousGetHandle()
            );
        }
    }
}
