using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a registered splitter that can decompose a complex AnalysisMergeConflict
    /// into smaller, more granular sub-conflicts, making it easier for conflict resolution
    /// handlers to address individual differences within a compound conflicting value.
    /// Splitter handles are always borrowed (never reference-counted) because they are
    /// registered globally and managed by Binary Ninja's analysis engine.
    /// </summary>
    public sealed class AnalysisMergeConflictSplitter : AbstractSafeHandle<AnalysisMergeConflictSplitter>
    {
        /// <summary>
        /// Initializes a new AnalysisMergeConflictSplitter wrapper around an existing borrowed handle.
        /// The handle is never owned — the splitter lifetime is managed by the native engine.
        /// </summary>
        /// <param name="handle">The native pointer to the BNAnalysisMergeConflictSplitter object.</param>
        internal AnalysisMergeConflictSplitter(IntPtr handle)
            : base(handle, false)
        {
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNAnalysisMergeConflictSplitter pointer.</param>
        /// <returns>A new AnalysisMergeConflictSplitter instance that will not free the handle on dispose.</returns>
        internal static AnalysisMergeConflictSplitter? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new AnalysisMergeConflictSplitter(handle);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNAnalysisMergeConflictSplitter pointer.</param>
        /// <returns>A new AnalysisMergeConflictSplitter instance that will not free the handle on dispose.</returns>
        internal static AnalysisMergeConflictSplitter MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new AnalysisMergeConflictSplitter(handle);
        }

        /// <summary>
        /// No-op release: splitter handles are always borrowed from the global registry
        /// and must not be freed by this wrapper.
        /// </summary>
        /// <returns>True (always, since no deallocation is performed).</returns>
        protected override bool ReleaseHandle()
        {
            // Splitter objects are borrowed from the global registry; the native engine owns their lifetime.
            return true;
        }

        /// <summary>
        /// Gets the registered name of this splitter, used to identify it in the conflict resolution pipeline.
        /// </summary>
        public string Name
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the splitter name.
                IntPtr raw = NativeMethods.BNAnalysisMergeConflictSplitterGetName(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Checks whether this splitter is capable of decomposing the given conflict at the given key.
        /// </summary>
        /// <param name="key">The conflict key that would be split.</param>
        /// <param name="conflict">The conflict to test for splittability.</param>
        /// <returns>True if this splitter can split the conflict; false otherwise.</returns>
        public bool CanSplit(string key, AnalysisMergeConflict conflict)
        {
            // Forward the splitter, key, and conflict handles to the native API.
            return NativeMethods.BNAnalysisMergeConflictSplitterCanSplit(
                this.handle,
                key ?? string.Empty,
                conflict.DangerousGetHandle()
            );
        }

        /// <summary>
        /// Splits a complex merge conflict into smaller sub-conflicts using this splitter.
        /// The returned arrays contain the new keys and corresponding new conflicts.
        /// </summary>
        /// <param name="originalKey">The original conflict key to split.</param>
        /// <param name="originalConflict">The original conflict to decompose.</param>
        /// <param name="result">A KeyValueStore to receive the split results.</param>
        /// <param name="newKeys">Receives the array of new conflict keys.</param>
        /// <param name="newConflicts">Receives the array of new sub-conflicts.</param>
        /// <returns>True if the split succeeded; false otherwise.</returns>
        public unsafe bool Split(
            string originalKey ,
            AnalysisMergeConflict originalConflict ,
            KeyValueStore result ,
            out string[] newKeys ,
            out AnalysisMergeConflict[] newConflicts
        )
        {
            // 1. Stack-allocate output pointers.
            IntPtr newKeysPtr = IntPtr.Zero;
            IntPtr newConflictsPtr = IntPtr.Zero;
            ulong newCount = 0;

            // 2. Call the native API.
            bool ok = NativeMethods.BNAnalysisMergeConflictSplitterSplit(
                this.handle ,
                originalKey ?? string.Empty ,
                originalConflict.DangerousGetHandle() ,
                result.DangerousGetHandle() ,
                (IntPtr)(&newKeysPtr) ,
                (IntPtr)(&newConflictsPtr) ,
                (IntPtr)(&newCount)
            );

            // 3. Handle failure.
            if (!ok || 0 == newCount)
            {
                newKeys = Array.Empty<string>();
                newConflicts = Array.Empty<AnalysisMergeConflict>();

                return false;
            }

            // 4. Marshal the string array.
            newKeys = UnsafeUtils.TakeStringArrayEx(newKeysPtr , newCount);

            // 5. Marshal the conflict handle array.
            newConflicts = UnsafeUtils.TakeHandleArrayEx<AnalysisMergeConflict>(
                newConflictsPtr ,
                newCount ,
                AnalysisMergeConflict.MustTakeHandle ,
                null
            );

            return true;
        }

        /// <summary>
        /// Retrieves all registered analysis merge conflict splitters from the engine.
        /// Each returned splitter is a borrowed reference managed by the native engine.
        /// </summary>
        /// <returns>An array of all registered AnalysisMergeConflictSplitter instances.</returns>
        public static unsafe AnalysisMergeConflictSplitter[] GetList()
        {
            // 1. Stack-allocate the count variable.
            ulong count = 0;

            // 2. Retrieve the native array of splitter pointers.
            IntPtr arrayPointer = NativeMethods.BNGetAnalysisMergeConflictSplitterList(
                (IntPtr)(&count)
            );

            // 3. Convert to managed array of borrowed handles and free the native pointer array.
            return UnsafeUtils.TakeHandleArrayEx<AnalysisMergeConflictSplitter>(
                arrayPointer ,
                count ,
                AnalysisMergeConflictSplitter.MustBorrowHandle ,
                NativeMethods.BNFreeAnalysisMergeConflictSplitterList
            );
        }
    }
}
