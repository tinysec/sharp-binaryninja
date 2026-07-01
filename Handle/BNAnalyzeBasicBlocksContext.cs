using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    /// <summary>
    /// Provides managed wrapper methods for the native BNBasicBlockAnalysisContext*
    /// pointer that is passed to architecture basic block analysis callbacks.
    /// This is not a SafeHandle-based class because the context pointer is owned
    /// by the native analysis pipeline and must not be freed by managed code.
    /// All methods are static and take the raw context pointer as the first parameter.
    /// </summary>
    public static class AnalyzeBasicBlocksContext
    {
        /// <summary>
        /// Adds a previously created basic block to the function being analyzed.
        /// </summary>
        /// <param name="context">The native BNBasicBlockAnalysisContext pointer.</param>
        /// <param name="block">The basic block to add to the function.</param>
        public static void AddBasicBlockToFunction(IntPtr context , BasicBlock block)
        {
            // Forward the context and block handle to the native API.
            NativeMethods.BNAnalyzeBasicBlocksContextAddBasicBlockToFunction(
                context ,
                block.DangerousGetHandle()
            );
        }

        /// <summary>
        /// Adds a temporary reference to a target function during analysis.
        /// This prevents the target function from being collected while analysis
        /// of the current function is still in progress.
        /// </summary>
        /// <param name="context">The native BNBasicBlockAnalysisContext pointer.</param>
        /// <param name="target">The target function to add a temporary reference to.</param>
        public static void AddTempReference(IntPtr context , Function target)
        {
            // Forward the context and function handle to the native API.
            NativeMethods.BNAnalyzeBasicBlocksContextAddTempReference(
                context ,
                target.DangerousGetHandle()
            );
        }

        /// <summary>
        /// Creates a new basic block within the analysis context at the given address.
        /// Returns null if the block cannot be created.
        /// </summary>
        /// <param name="context">The native BNBasicBlockAnalysisContext pointer.</param>
        /// <param name="arch">The architecture for the new basic block.</param>
        /// <param name="address">The start address of the new basic block.</param>
        /// <returns>A new BasicBlock, or null if creation failed.</returns>
        public static BasicBlock? CreateBasicBlock(IntPtr context , Architecture arch , ulong address)
        {
            // Call the native API to create the block.
            IntPtr result = NativeMethods.BNAnalyzeBasicBlocksContextCreateBasicBlock(
                context ,
                arch.DangerousGetHandle() ,
                address
            );

            // Wrap the returned handle (takes ownership).
            return BasicBlock.TakeHandle(result);
        }

        /// <summary>
        /// Sets the contextual function return sources and their boolean return values.
        /// Each source describes a call site, and each value indicates whether the
        /// function is contextually known to return at that site.
        /// </summary>
        /// <param name="context">The native BNBasicBlockAnalysisContext pointer.</param>
        /// <param name="sources">An array of architecture-and-address pairs identifying the call sites.</param>
        /// <param name="values">An array of boolean values, one per source, indicating contextual return behavior.</param>
        public static unsafe void SetContextualFunctionReturns(
            IntPtr context ,
            ArchitectureAndAddress[] sources ,
            bool[] values
        )
        {
            // 1. Validate inputs.
            if (null == sources || null == values || sources.Length != values.Length)
            {
                return;
            }

            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                // 2. Marshal the sources array.
                BNArchitectureAndAddress[] nativeSources = new BNArchitectureAndAddress[sources.Length];
                for (int i = 0; i < sources.Length; i++)
                {
                    nativeSources[i] = sources[i].ToNative();
                }
                IntPtr sourcesPtr = allocator.AllocStructArray<BNArchitectureAndAddress>(nativeSources);

                // 3. Marshal the values array (bool[] to native bool array).
                IntPtr valuesPtr = Marshal.AllocHGlobal(sizeof(bool) * values.Length);
                for (int i = 0; i < values.Length; i++)
                {
                    Marshal.WriteByte(valuesPtr , i , values[i] ? (byte)1 : (byte)0);
                }

                // 4. Call the native API.
                NativeMethods.BNAnalyzeBasicBlocksContextSetContextualFunctionReturns(
                    context ,
                    sourcesPtr ,
                    valuesPtr ,
                    (ulong)sources.Length
                );

                // 5. Free the manually allocated values buffer.
                Marshal.FreeHGlobal(valuesPtr);
            }
        }

        /// <summary>
        /// Sets the direct code references discovered during basic block analysis.
        /// Each source is a call/branch site, and each target is the destination address.
        /// </summary>
        /// <param name="context">The native BNBasicBlockAnalysisContext pointer.</param>
        /// <param name="sources">An array of architecture-and-address pairs identifying the reference sources.</param>
        /// <param name="targets">An array of destination addresses, one per source.</param>
        public static unsafe void SetDirectCodeReferences(
            IntPtr context ,
            ArchitectureAndAddress[] sources ,
            ulong[] targets
        )
        {
            // 1. Validate inputs.
            if (null == sources || null == targets || sources.Length != targets.Length)
            {
                return;
            }

            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                // 2. Marshal the sources array.
                BNArchitectureAndAddress[] nativeSources = new BNArchitectureAndAddress[sources.Length];
                for (int i = 0; i < sources.Length; i++)
                {
                    nativeSources[i] = sources[i].ToNative();
                }
                IntPtr sourcesPtr = allocator.AllocStructArray<BNArchitectureAndAddress>(nativeSources);

                // 3. Marshal the targets array.
                IntPtr targetsPtr = allocator.AllocStructArray<ulong>(targets);

                // 4. Call the native API.
                NativeMethods.BNAnalyzeBasicBlocksContextSetDirectCodeReferences(
                    context ,
                    sourcesPtr ,
                    targetsPtr ,
                    (ulong)sources.Length
                );
            }
        }

        /// <summary>
        /// Sets the direct no-return call sites discovered during basic block analysis.
        /// </summary>
        /// <param name="context">The native BNBasicBlockAnalysisContext pointer.</param>
        /// <param name="sources">An array of architecture-and-address pairs identifying the no-return call sites.</param>
        public static unsafe void SetDirectNoReturnCalls(
            IntPtr context ,
            ArchitectureAndAddress[] sources
        )
        {
            // 1. Validate input.
            if (null == sources || 0 == sources.Length)
            {
                return;
            }

            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                // 2. Marshal the sources array.
                BNArchitectureAndAddress[] nativeSources = new BNArchitectureAndAddress[sources.Length];
                for (int i = 0; i < sources.Length; i++)
                {
                    nativeSources[i] = sources[i].ToNative();
                }
                IntPtr sourcesPtr = allocator.AllocStructArray<BNArchitectureAndAddress>(nativeSources);

                // 3. Call the native API.
                NativeMethods.BNAnalyzeBasicBlocksContextSetDirectNoReturnCalls(
                    context ,
                    sourcesPtr ,
                    (ulong)sources.Length
                );
            }
        }

        /// <summary>
        /// Sets the addresses where disassembly was halted during basic block analysis.
        /// </summary>
        /// <param name="context">The native BNBasicBlockAnalysisContext pointer.</param>
        /// <param name="sources">An array of architecture-and-address pairs where disassembly stopped.</param>
        public static unsafe void SetHaltedDisassemblyAddresses(
            IntPtr context ,
            ArchitectureAndAddress[] sources
        )
        {
            // 1. Validate input.
            if (null == sources || 0 == sources.Length)
            {
                return;
            }

            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                // 2. Marshal the sources array.
                BNArchitectureAndAddress[] nativeSources = new BNArchitectureAndAddress[sources.Length];
                for (int i = 0; i < sources.Length; i++)
                {
                    nativeSources[i] = sources[i].ToNative();
                }
                IntPtr sourcesPtr = allocator.AllocStructArray<BNArchitectureAndAddress>(nativeSources);

                // 3. Call the native API.
                NativeMethods.BNAnalyzeBasicBlocksContextSetHaltedDisassemblyAddresses(
                    context ,
                    sourcesPtr ,
                    (ulong)sources.Length
                );
            }
        }

        /// <summary>
        /// Sets the locations of inlined unresolved indirect branches discovered during analysis.
        /// </summary>
        /// <param name="context">The native BNBasicBlockAnalysisContext pointer.</param>
        /// <param name="locations">An array of architecture-and-address pairs where unresolved indirect branches were inlined.</param>
        public static unsafe void SetInlinedUnresolvedIndirectBranches(
            IntPtr context ,
            ArchitectureAndAddress[] locations
        )
        {
            // 1. Validate input.
            if (null == locations || 0 == locations.Length)
            {
                return;
            }

            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                // 2. Marshal the locations array.
                BNArchitectureAndAddress[] nativeLocations = new BNArchitectureAndAddress[locations.Length];
                for (int i = 0; i < locations.Length; i++)
                {
                    nativeLocations[i] = locations[i].ToNative();
                }
                IntPtr locationsPtr = allocator.AllocStructArray<BNArchitectureAndAddress>(nativeLocations);

                // 3. Call the native API.
                NativeMethods.BNAnalyzeBasicBlocksContextSetInlinedUnresolvedIndirectBranches(
                    context ,
                    locationsPtr ,
                    (ulong)locations.Length
                );
            }
        }
    }
}
