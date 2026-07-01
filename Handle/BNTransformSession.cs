using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class TransformSession : AbstractSafeHandle<TransformSession>
	{
	    internal TransformSession(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }
	    
	    internal static TransformSession? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new TransformSession(
			    NativeMethods.BNNewTransformSessionReference(handle) ,
			    true
		    );
	    }
	    
	    internal static TransformSession MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new TransformSession(
			    NativeMethods.BNNewTransformSessionReference(handle) ,
			    true
		    );
	    }
	    
	    internal static TransformSession? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new TransformSession(handle, true);
	    }
	    
	    internal static TransformSession MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new TransformSession(handle, true);
	    }
	    
	    internal static TransformSession? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new TransformSession(handle, false);
	    }
	    
	    internal static TransformSession MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new TransformSession(handle, false);
	    }

        /// <summary>
        /// Releases the native BNTransformSession handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native session handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeTransformSession(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        // ===================================================================
        // Static factory methods
        // ===================================================================

        /// <summary>
        /// Creates a new transform session for the given filename.
        /// </summary>
        /// <param name="filename">The path to the file to open in the session.</param>
        /// <returns>A new owned TransformSession instance, or null on failure.</returns>
        public static TransformSession? Create(string filename , string options = "")
        {
            // Call the native factory to create a session from a filename.
            IntPtr result = NativeMethods.BNCreateTransformSession(filename , options);

            // Wrap as a new owned handle.
            return TransformSession.TakeHandle(result);
        }

        /// <summary>
        /// Creates a new transform session from an existing binary view.
        /// </summary>
        /// <param name="initialView">The binary view to use as the initial input.</param>
        /// <returns>A new owned TransformSession instance, or null on failure.</returns>
        public static TransformSession? CreateFromBinaryView(BinaryView initialView , string options = "")
        {
            // Call the native factory to create a session from a binary view.
            IntPtr result = NativeMethods.BNCreateTransformSessionFromBinaryView(
                initialView.DangerousGetHandle() ,
                options
            );

            // Wrap as a new owned handle.
            return TransformSession.TakeHandle(result);
        }

        /// <summary>
        /// Creates a new transform session for the given filename with a specific mode.
        /// </summary>
        /// <param name="filename">The path to the file to open in the session.</param>
        /// <param name="mode">The transform session mode to use.</param>
        /// <returns>A new owned TransformSession instance, or null on failure.</returns>
        public static TransformSession? CreateWithMode(string filename , TransformSessionMode mode , string options = "")
        {
            // Call the native factory to create a session with mode from a filename.
            IntPtr result = NativeMethods.BNCreateTransformSessionWithMode(filename , mode , options);

            // Wrap as a new owned handle.
            return TransformSession.TakeHandle(result);
        }

        /// <summary>
        /// Creates a new transform session from an existing binary view with a specific mode.
        /// </summary>
        /// <param name="initialView">The binary view to use as the initial input.</param>
        /// <param name="mode">The transform session mode to use.</param>
        /// <returns>A new owned TransformSession instance, or null on failure.</returns>
        public static TransformSession? CreateFromBinaryViewWithMode(
            BinaryView initialView ,
            TransformSessionMode mode ,
            string options = "")
        {
            // Call the native factory to create a session with mode from a binary view.
            IntPtr result = NativeMethods.BNCreateTransformSessionFromBinaryViewWithMode(
                initialView.DangerousGetHandle() ,
                mode ,
                options
            );

            // Wrap as a new owned handle.
            return TransformSession.TakeHandle(result);
        }

        // ===================================================================
        // Instance properties
        // ===================================================================

        /// <summary>
        /// Gets the binary view that is the current focus of this transform session.
        /// Returns null if no binary view is currently associated.
        /// </summary>
        public BinaryView? CurrentView
        {
            get
            {
                // Retrieve a new owned reference to the current binary view.
                return BinaryNinja.BinaryView.TakeHandle(
                    NativeMethods.BNTransformSessionGetCurrentView(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets the transform context at the current step in the session pipeline.
        /// Returns null if no current context is available.
        /// </summary>
        public TransformContext? CurrentContext
        {
            get
            {
                // Retrieve a new owned reference to the current transform context.
                return BinaryNinja.TransformContext.TakeHandle(
                    NativeMethods.BNTransformSessionGetCurrentContext(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets the root transform context at the top of the session pipeline.
        /// Returns null if no root context is available.
        /// </summary>
        public TransformContext? RootContext
        {
            get
            {
                // Retrieve a new owned reference to the root transform context.
                return BinaryNinja.TransformContext.TakeHandle(
                    NativeMethods.BNTransformSessionGetRootContext(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets whether this session has any processing stages registered.
        /// </summary>
        public bool HasAnyStages
        {
            get
            {
                // Query the native layer for the has-any-stages flag.
                return NativeMethods.BNTransformSessionHasAnyStages(this.handle);
            }
        }

        /// <summary>
        /// Gets whether this session follows a single linear processing path.
        /// </summary>
        public bool HasSinglePath
        {
            get
            {
                // Query the native layer for the single-path flag.
                return NativeMethods.BNTransformSessionHasSinglePath(this.handle);
            }
        }

        /// <summary>
        /// Advances this session by one step, executing the current pipeline stage.
        /// </summary>
        /// <returns>True if processing succeeded and more stages remain; false on completion or error.</returns>
        public bool Process()
        {
            // Delegate to the native step-forward API.
            return NativeMethods.BNTransformSessionProcess(this.handle);
        }

        public TransformContext[] GetSelectedContexts()
        {
            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                IntPtr countPtr = allocator.AllocStruct<ulong>(0);

                IntPtr arrayPointer = NativeMethods.BNTransformSessionGetSelectedContexts(
                    this.handle ,
                    countPtr
                );

                ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

                return UnsafeUtils.TakeHandleArrayEx<TransformContext>(
                    arrayPointer ,
                    arrayLength ,
                    TransformContext.MustBorrowHandle ,
                    NativeMethods.BNFreeTransformContextList
                );
            }
        }

        public void SetSelectedContexts(TransformContext[] contexts)
        {
            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                NativeMethods.BNTransformSessionSetSelectedContexts(
                    this.handle ,
                    allocator.AllocHandleArray<TransformContext>(contexts) ,
                    (ulong)contexts.Length
                );
            }
        }

        public bool ProcessFrom(TransformContext context)
        {
            return NativeMethods.BNTransformSessionProcessFrom(
                this.handle ,
                context.DangerousGetHandle()
            );
        }
    }
}