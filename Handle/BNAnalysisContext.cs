using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a Binary Ninja analysis context — the execution environment provided to
    /// analysis passes and activity callbacks. An AnalysisContext carries references to the
    /// binary view being analysed, the function under analysis, and the intermediate
    /// representation (IL) functions produced at each lifting stage.
    /// Analysis passes use the context to retrieve IL functions, inspect lifted code, and
    /// communicate structured requests back to the analysis pipeline via Inform().
    /// </summary>
    public sealed class AnalysisContext : AbstractSafeHandle<AnalysisContext>
    {
        /// <summary>
        /// Initializes a new AnalysisContext wrapper around an existing native handle.
        /// </summary>
        /// <param name="handle">The native pointer to the BNAnalysisContext object.</param>
        /// <param name="owner">True if this instance owns the handle and should free it on dispose.</param>
        internal AnalysisContext(IntPtr handle, bool owner)
            : base(handle, owner)
        {
        }

        /// <summary>
        /// Creates a new managed AnalysisContext by incrementing the reference count on an existing native handle.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNAnalysisContext pointer.</param>
        /// <returns>A new AnalysisContext instance, or null if handle is zero.</returns>
        internal static AnalysisContext? NewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new AnalysisContext(
                NativeMethods.BNNewAnalysisContextReference(handle),
                true
            );
        }

        /// <summary>
        /// Creates a new managed AnalysisContext by incrementing the reference count. Throws if handle is zero.
        /// </summary>
        /// <param name="handle">The native BNAnalysisContext pointer.</param>
        /// <returns>A new AnalysisContext instance.</returns>
        internal static AnalysisContext MustNewFromHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new AnalysisContext(
                NativeMethods.BNNewAnalysisContextReference(handle),
                true
            );
        }

        /// <summary>
        /// Takes ownership of an existing native handle without incrementing the reference count.
        /// Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNAnalysisContext pointer.</param>
        /// <returns>A new AnalysisContext instance, or null if handle is zero.</returns>
        internal static AnalysisContext? TakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new AnalysisContext(handle, true);
        }

        /// <summary>
        /// Takes ownership of an existing native handle without incrementing the reference count. Throws if zero.
        /// </summary>
        /// <param name="handle">The native BNAnalysisContext pointer.</param>
        /// <returns>A new AnalysisContext instance.</returns>
        internal static AnalysisContext MustTakeHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new AnalysisContext(handle, true);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Returns null if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNAnalysisContext pointer.</param>
        /// <returns>A new AnalysisContext instance that will not free the handle on dispose.</returns>
        internal static AnalysisContext? BorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            return new AnalysisContext(handle, false);
        }

        /// <summary>
        /// Borrows a native handle without taking ownership. Throws if the handle is zero.
        /// </summary>
        /// <param name="handle">The native BNAnalysisContext pointer.</param>
        /// <returns>A new AnalysisContext instance that will not free the handle on dispose.</returns>
        internal static AnalysisContext MustBorrowHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return new AnalysisContext(handle, false);
        }

        /// <summary>
        /// Releases the native BNAnalysisContext handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native analysis context handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeAnalysisContext(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Gets the BinaryView associated with this analysis context.
        /// This is the binary view being analysed by the current analysis pass.
        /// </summary>
        public BinaryView? BinaryView
        {
            get
            {
                // Retrieve a new owned reference to the binary view from the native context.
                return BinaryNinja.BinaryView.TakeHandle(
                    NativeMethods.BNAnalysisContextGetBinaryView(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets the Function currently being analysed within this analysis context.
        /// Analysis passes that operate per-function use this to access the target function.
        /// </summary>
        public Function? Function
        {
            get
            {
                // Retrieve a new owned reference to the function from the native context.
                return BinaryNinja.Function.TakeHandle(
                    NativeMethods.BNAnalysisContextGetFunction(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets the High Level IL function produced by lifting for the function under analysis.
        /// Returns null if no High Level IL function is available in this context.
        /// </summary>
        public HighLevelILFunction? HighLevelILFunction
        {
            get
            {
                // Retrieve a new owned reference to the HLIL function from the native context.
                return BinaryNinja.HighLevelILFunction.TakeHandle(
                    NativeMethods.BNAnalysisContextGetHighLevelILFunction(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets the lifted Low Level IL function (the intermediate output of the lifter, before
        /// optimisation) for the function under analysis.
        /// Returns null if no lifted IL function is available in this context.
        /// </summary>
        public LowLevelILFunction? LiftedILFunction
        {
            get
            {
                // Retrieve a new owned reference to the lifted LLIL function from the native context.
                return BinaryNinja.LowLevelILFunction.TakeHandle(
                    NativeMethods.BNAnalysisContextGetLiftedILFunction(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets the Low Level IL function for the function under analysis.
        /// Returns null if no Low Level IL function is available in this context.
        /// </summary>
        public LowLevelILFunction? LowLevelILFunction
        {
            get
            {
                // Retrieve a new owned reference to the LLIL function from the native context.
                return BinaryNinja.LowLevelILFunction.TakeHandle(
                    NativeMethods.BNAnalysisContextGetLowLevelILFunction(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets the Medium Level IL function for the function under analysis.
        /// Returns null if no Medium Level IL function is available in this context.
        /// </summary>
        public MediumLevelILFunction? MediumLevelILFunction
        {
            get
            {
                // Retrieve a new owned reference to the MLIL function from the native context.
                return BinaryNinja.MediumLevelILFunction.TakeHandle(
                    NativeMethods.BNAnalysisContextGetMediumLevelILFunction(this.handle)
                );
            }
        }

        /// <summary>
        /// Sends a structured request string to the analysis pipeline.
        /// Analysis passes use Inform() to communicate results or trigger downstream
        /// actions within Binary Ninja's analysis engine.
        /// </summary>
        /// <param name="request">The structured request string to send to the pipeline.</param>
        /// <returns>True if the request was accepted; false if the pipeline rejected it.</returns>
        public bool Inform(string request)
        {
            // Forward the request string to the native analysis context API.
            return NativeMethods.BNAnalysisContextInform(
                this.handle,
                request ?? string.Empty
            );
        }

        /// <summary>
        /// Sets the basic block list for this analysis context.
        /// </summary>
        /// <param name="blocks">The array of basic blocks to set.</param>
        public void SetBasicBlockList(BasicBlock[] blocks)
        {
            // 1. Validate the required parameter.
            if (null == blocks)
            {
                throw new ArgumentNullException(nameof(blocks));
            }

            // 2. Marshal the basic block handle array.
            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                // 2.1 Build an IntPtr array of the block handles.
                IntPtr[] blockHandles = new IntPtr[blocks.Length];

                for (int i = 0; i < blocks.Length; i++)
                {
                    blockHandles[i] = blocks[i].DangerousGetHandle();
                }

                // 2.2 Allocate and fill the native pointer array.
                IntPtr nativeArray = allocator.AllocStructArray<IntPtr>(blockHandles);

                // 3. Call the native API.
                NativeMethods.BNSetBasicBlockList(
                    this.handle ,
                    nativeArray ,
                    (ulong)blocks.Length
                );
            }
        }

        /// <summary>
        /// Sets the High Level IL function for this analysis context.
        /// </summary>
        /// <param name="hlilFunction">The High Level IL function to set.</param>
        public void SetHighLevelILFunction(HighLevelILFunction hlilFunction)
        {
            // 1. Validate the required parameter.
            if (null == hlilFunction)
            {
                throw new ArgumentNullException(nameof(hlilFunction));
            }

            // 2. Forward to the native API.
            NativeMethods.BNSetHighLevelILFunction(
                this.handle ,
                hlilFunction.DangerousGetHandle()
            );
        }

        /// <summary>
        /// Sets the lifted IL function for this analysis context.
        /// </summary>
        /// <param name="liftedILFunction">The lifted IL function to set.</param>
        public void SetLiftedILFunction(LowLevelILFunction liftedILFunction)
        {
            // 1. Validate the required parameter.
            if (null == liftedILFunction)
            {
                throw new ArgumentNullException(nameof(liftedILFunction));
            }

            // 2. Forward to the native API.
            NativeMethods.BNSetLiftedILFunction(
                this.handle ,
                liftedILFunction.DangerousGetHandle()
            );
        }

        /// <summary>
        /// Sets the Low Level IL function for this analysis context.
        /// </summary>
        /// <param name="llilFunction">The Low Level IL function to set.</param>
        public void SetLowLevelILFunction(LowLevelILFunction llilFunction)
        {
            // 1. Validate the required parameter.
            if (null == llilFunction)
            {
                throw new ArgumentNullException(nameof(llilFunction));
            }

            // 2. Forward to the native API.
            NativeMethods.BNSetLowLevelILFunction(
                this.handle ,
                llilFunction.DangerousGetHandle()
            );
        }

        /// <summary>
        /// Creates a new empty analysis context.
        /// The returned context is not associated with any binary view or function until
        /// the caller populates it through the setter methods.
        /// </summary>
        /// <returns>A new owned AnalysisContext instance.</returns>
        public static AnalysisContext Create()
        {
            // Allocate a fresh native analysis context with no associations.
            return AnalysisContext.MustTakeHandle(NativeMethods.BNCreateAnalysisContext());
        }

        /// <summary>
        /// Sets the Medium Level IL function for this analysis context.
        /// The caller must also provide the LLIL SSA to MLIL SSA instruction and expression mapping arrays.
        /// </summary>
        /// <param name="mlilFunction">The Medium Level IL function to set.</param>
        public void SetMediumLevelILFunction(MediumLevelILFunction mlilFunction)
        {
            // 1. Validate the required parameter.
            if (null == mlilFunction)
            {
                throw new ArgumentNullException(nameof(mlilFunction));
            }

            // 2. Forward to the native API with empty mapping arrays.
            NativeMethods.BNSetMediumLevelILFunction(
                this.handle ,
                mlilFunction.DangerousGetHandle() ,
                IntPtr.Zero ,
                0 ,
                IntPtr.Zero ,
                0
            );
        }
    }
}
