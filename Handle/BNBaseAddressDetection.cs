using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class BaseAddressDetection : AbstractSafeHandle<BaseAddressDetection>
	{
	    internal BaseAddressDetection(IntPtr handle , bool owner)
		    : base(handle , owner)
	    {
	        
	    }

	   
	    internal static BaseAddressDetection? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new BaseAddressDetection(handle, true);
	    }
	    
	    internal static BaseAddressDetection MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new BaseAddressDetection(handle, true);
	    }
	    
	    internal static BaseAddressDetection? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new BaseAddressDetection(handle, false);
	    }
	    
	    internal static BaseAddressDetection MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new BaseAddressDetection(handle, false);
	    }
	    
        /// <summary>
        /// Releases the native BNBaseAddressDetection handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native detection handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeBaseAddressDetection(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Gets whether the detection algorithm was aborted before it completed.
        /// Check this property after DetectBaseAddress returns false.
        /// </summary>
        public bool IsAborted
        {
            get
            {
                // Query the native layer for the abort flag on the running detection.
                return NativeMethods.BNIsBaseAddressDetectionAborted(this.handle);
            }
        }

        /// <summary>
        /// Signals the running DetectBaseAddress call to stop as soon as possible.
        /// The detection may complete a partial set of candidates before stopping.
        /// </summary>
        public void Abort()
        {
            // Send the abort signal to the native detection algorithm.
            NativeMethods.BNAbortBaseAddressDetection(this.handle);
        }

        /// <summary>
        /// Runs the base-address detection algorithm using the given settings.
        /// This call blocks until detection completes or is aborted.
        /// </summary>
        /// <param name="settings">The detection settings controlling architecture, boundaries, and heuristics.</param>
        /// <returns>True if the detection completed successfully; false if it was aborted or failed.</returns>
        public unsafe bool DetectBaseAddress(BaseAddressDetectionSettings settings)
        {
            // 1. Copy the managed settings into a native struct on the stack.
            BNBaseAddressDetectionSettings native = new BNBaseAddressDetectionSettings();

            // 2. Pin the architecture and analysis strings in memory for the duration of the call.
            //    Use fixed blocks so the GC cannot relocate them while the native call runs.
            fixed (byte* archBytes = System.Text.Encoding.ASCII.GetBytes(
                (settings.Architecture ?? string.Empty) + '\0'))
            fixed (byte* analysisBytes = System.Text.Encoding.ASCII.GetBytes(
                (settings.Analysis ?? string.Empty) + '\0'))
            {
                // 2.1 Point the native struct at the pinned string buffers.
                native.Architecture = (IntPtr)archBytes;
                native.Analysis = (IntPtr)analysisBytes;

                // 2.2 Copy the scalar fields from the managed settings.
                native.MinStrlen = settings.MinStrlen;
                native.Alignment = settings.Alignment;
                native.LowerBoundary = settings.LowerBoundary;
                native.UpperBoundary = settings.UpperBoundary;
                native.POIAnalysis = settings.POIAnalysis;
                native.MaxPointersPerCluster = settings.MaxPointersPerCluster;
                native.AnalysisMode = settings.AnalysisMode;

                // 3. Invoke the native detection algorithm with a pointer to the settings struct.
                return NativeMethods.BNDetectBaseAddress(this.handle, (IntPtr)(&native));
            }
        }

        /// <summary>
        /// Retrieves the top candidate base addresses with their confidence scores.
        /// Call this after DetectBaseAddress has completed.
        /// </summary>
        /// <param name="maxCount">The maximum number of candidate scores to retrieve.</param>
        /// <param name="confidence">Receives the confidence level of the detection result.</param>
        /// <param name="lastTestedBaseAddress">Receives the last base address tested by the algorithm.</param>
        /// <returns>An array of candidate scores, ordered by score descending.</returns>
        public unsafe BaseAddressDetectionScore[] GetScores(
            ulong maxCount,
            out BaseAddressDetectionConfidence confidence,
            out ulong lastTestedBaseAddress)
        {
            // 1. Allocate a native array of the requested size on the stack for small counts, or the heap.
            BNBaseAddressDetectionScore[] nativeScores = new BNBaseAddressDetectionScore[maxCount];

            BaseAddressDetectionConfidence confidenceOut = BaseAddressDetectionConfidence.NoConfidence;
            ulong lastTestedOut = 0;
            ulong actualCount = 0;

            fixed (BNBaseAddressDetectionScore* scoresPtr = nativeScores)
            {
                // 2. Call the native function with the pre-allocated array and output pointers.
                actualCount = NativeMethods.BNGetBaseAddressDetectionScores(
                    this.handle,
                    (IntPtr)scoresPtr,
                    maxCount,
                    (IntPtr)(&confidenceOut),
                    (IntPtr)(&lastTestedOut)
                );
            }

            // 3. Return the out parameters.
            confidence = confidenceOut;
            lastTestedBaseAddress = lastTestedOut;

            // 4. Convert the native score structs to managed objects up to actualCount.
            BaseAddressDetectionScore[] result = new BaseAddressDetectionScore[(int)actualCount];
            for (ulong i = 0; i < actualCount; i++)
            {
                // 4.1 Map each native struct field to the managed object.
                BaseAddressDetectionScore managed = new BaseAddressDetectionScore();
                managed.Score = nativeScores[i].Score;
                managed.BaseAddress = nativeScores[i].BaseAddress;
                result[i] = managed;
            }

            return result;
        }

        /// <summary>
        /// Retrieves the detection reasons that explain why the given base address was scored.
        /// Call this after GetScores to understand the evidence supporting a candidate.
        /// </summary>
        /// <param name="baseAddress">The candidate base address to retrieve reasons for.</param>
        /// <returns>An array of reasons describing the heuristic evidence for the candidate.</returns>
        public unsafe BaseAddressDetectionReason[] GetReasons(ulong baseAddress)
        {
            // 1. Stack-allocate the count variable and retrieve the native reason array.
            ulong count = 0;
            IntPtr ptr = NativeMethods.BNGetBaseAddressDetectionReasons(
                this.handle,
                baseAddress,
                (IntPtr)(&count)
            );

            if (ptr == IntPtr.Zero || count == 0)
            {
                // No reasons available for this base address; return an empty array.
                return Array.Empty<BaseAddressDetectionReason>();
            }

            // 2. Cast the raw pointer to the native struct type for indexed access.
            BNBaseAddressDetectionReason* rawArray = (BNBaseAddressDetectionReason*)ptr;

            // 3. Convert each native struct to a managed object.
            BaseAddressDetectionReason[] result = new BaseAddressDetectionReason[(int)count];
            for (ulong i = 0; i < count; i++)
            {
                // 3.1 Copy each field from the native struct to the managed object.
                BaseAddressDetectionReason managed = new BaseAddressDetectionReason();
                managed.Pointer = rawArray[i].Pointer;
                managed.POIOffset = rawArray[i].POIOffset;
                managed.POIType = rawArray[i].POIType;
                result[i] = managed;
            }

            // 4. Free the native reason array (takes only the pointer, no count).
            NativeMethods.BNFreeBaseAddressDetectionReasons(ptr);

            return result;
        }

        /// <summary>
        /// Creates a new BaseAddressDetection context for the given binary view.
        /// The returned object can be used to run base address detection analysis.
        /// </summary>
        /// <param name="view">The binary view to analyze for base address detection.</param>
        /// <returns>A new BaseAddressDetection instance, or null if creation failed.</returns>
        public static BaseAddressDetection? Create(BinaryView view)
        {
            // 1. Validate the required parameter.
            if (null == view)
            {
                throw new ArgumentNullException(nameof(view));
            }

            // 2. Call the native API.
            IntPtr result = NativeMethods.BNCreateBaseAddressDetection(view.DangerousGetHandle());

            // 3. Wrap the returned handle.
            return BaseAddressDetection.TakeHandle(result);
        }
    }
}