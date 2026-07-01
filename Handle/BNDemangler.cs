using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a registered name demangler that translates mangled compiler-generated names
    /// (such as C++ symbols) into human-readable qualified names and types.
    /// Demangler handles are always borrowed — the demangler lifetime is managed by the
    /// native engine's global registry.
    /// </summary>
    public sealed class Demangler : AbstractSafeHandle<Demangler>
    {
        /// <summary>
        /// Initializes a new Demangler wrapper around an existing borrowed handle.
        /// The handle is never owned — the demangler lifetime is managed by the native engine.
        /// </summary>
        /// <param name="handle">The native pointer to the BNDemangler object.</param>
        internal Demangler(IntPtr handle)
            : base(handle, false)
        {
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNDemangler pointer.</param>
        /// <returns>A new Demangler instance that will not free the handle on dispose.</returns>
        internal static Demangler? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new Demangler(handle);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNDemangler pointer.</param>
        /// <returns>A new Demangler instance that will not free the handle on dispose.</returns>
        internal static Demangler MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new Demangler(handle);
        }

        /// <summary>
        /// No-op release: demangler handles are always borrowed from the global registry
        /// and must not be freed by this wrapper.
        /// </summary>
        /// <returns>True (always, since no deallocation is performed).</returns>
        protected override bool ReleaseHandle()
        {
            // Demangler objects are borrowed from the global registry; the native engine owns their lifetime.
            return true;
        }

        /// <summary>
        /// Gets the registered name that uniquely identifies this demangler.
        /// </summary>
        public string Name
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the demangler name.
                IntPtr raw = NativeMethods.BNGetDemanglerName(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        // ===================================================================
        // Static lookup methods
        // ===================================================================

        /// <summary>
        /// Looks up a registered demangler by its unique name.
        /// Returns null if no demangler with the given name exists.
        /// </summary>
        /// <param name="name">The registered name of the demangler to find.</param>
        /// <returns>A borrowed Demangler instance, or null if not found.</returns>
        public static Demangler? GetByName(string name)
        {
            // Query the global registry for a demangler with the specified name.
            IntPtr result = NativeMethods.BNGetDemanglerByName(name);

            // Wrap as a borrowed handle; returns null when the native pointer is zero.
            return Demangler.BorrowHandle(result);
        }

        /// <summary>
        /// Gets all registered demanglers from the engine.
        /// Each returned demangler is a borrowed reference.
        /// </summary>
        /// <returns>An array of all registered Demangler instances.</returns>
        public static unsafe Demangler[] GetList()
        {
            // 1. Stack-allocate the count variable.
            ulong count = 0;

            // 2. Retrieve the native array of demangler pointers.
            IntPtr arrayPointer = NativeMethods.BNGetDemanglerList((IntPtr)(&count));

            // 3. Convert to managed array of borrowed handles and free the native pointer array.
            return UnsafeUtils.TakeHandleArray<Demangler>(
                arrayPointer ,
                count ,
                Demangler.MustBorrowHandle ,
                NativeMethods.BNFreeDemanglerList
            );
        }

        /// <summary>
        /// Promotes a demangler so it takes priority over other demanglers in the registry.
        /// </summary>
        /// <param name="demangler">The demangler to promote.</param>
        public static void Promote(Demangler demangler)
        {
            // Forward the demangler handle to the native promote API.
            NativeMethods.BNPromoteDemangler(demangler.DangerousGetHandle());
        }

        // ===================================================================
        // Instance methods
        // ===================================================================

        /// <summary>
        /// Attempts to demangle the given mangled name using this demangler.
        /// On success, outType receives the demangled type (may be null for pure names),
        /// and outVarName receives the demangled qualified name.
        /// </summary>
        /// <param name="arch">The architecture context for the demangling operation.</param>
        /// <param name="name">The mangled name string to demangle.</param>
        /// <param name="view">The binary view context for the demangling operation (may be null).</param>
        /// <param name="outType">Receives the demangled type, or null when only a name is returned.</param>
        /// <param name="outVarName">Receives the demangled qualified variable name.</param>
        /// <returns>True if demangling succeeded; false if this demangler cannot handle the name.</returns>
        public unsafe bool Demangle(
            Architecture arch,
            string name,
            BinaryView? view,
            out BinaryNinja.Type? outType,
            out QualifiedName outVarName)
        {
            // 1. Prepare a stack slot for the native type pointer output.
            IntPtr typePtr = IntPtr.Zero;

            // 2. Prepare a zeroed BNQualifiedName struct on the stack for the name output.
            BNQualifiedName nativeVarName = new BNQualifiedName();

            // 3. Resolve the raw architecture and optional binary view handles.
            IntPtr archHandle = arch.DangerousGetHandle();

            IntPtr viewHandle = (view != null) ? view.DangerousGetHandle() : IntPtr.Zero;

            // 4. Call the native demangler function with pointers to the output slots.
            bool success = NativeMethods.BNDemanglerDemangle(
                this.handle,
                archHandle,
                name ?? string.Empty,
                (IntPtr)(&typePtr),
                (IntPtr)(&nativeVarName),
                viewHandle
            );

            // 5. Wrap the output type pointer; it is a new owned reference when non-null.
            outType = BinaryNinja.Type.TakeHandle(typePtr);

            // 6. Convert the native qualified name struct to the managed QualifiedName.
            //    TakeNative copies the strings and frees the native allocation.
            outVarName = QualifiedName.TakeNative(nativeVarName);

            return success;
        }
    }
}
