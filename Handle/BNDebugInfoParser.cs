using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class DebugInfoParser : AbstractSafeHandle<DebugInfoParser>
	{
	    internal DebugInfoParser(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }
	    
	    internal static DebugInfoParser? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new DebugInfoParser(
			    NativeMethods.BNNewDebugInfoParserReference(handle) ,
			    true
		    );
	    }
	    
	    internal static DebugInfoParser MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new DebugInfoParser(
			    NativeMethods.BNNewDebugInfoParserReference(handle) ,
			    true
		    );
	    }
	    
	    internal static DebugInfoParser? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new DebugInfoParser(handle, true);
	    }
	    
	    internal static DebugInfoParser MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new DebugInfoParser(handle, true);
	    }
	    
	    internal static DebugInfoParser? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new DebugInfoParser(handle, false);
	    }
	    
	    internal static DebugInfoParser MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new DebugInfoParser(handle, false);
	    }
	    
        /// <summary>
        /// Releases the native BNDebugInfoParser handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native parser reference and mark it invalid to prevent double-free.
                NativeMethods.BNFreeDebugInfoParserReference(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

            // ===================================================================
        // Static lookup methods
        // ===================================================================

        /// <summary>
        /// Looks up a registered debug info parser by its unique name.
        /// Returns null if no parser with the given name exists.
        /// </summary>
        /// <param name="name">The registered name of the parser to find.</param>
        /// <returns>A new owned DebugInfoParser instance, or null if not found.</returns>
        public static DebugInfoParser? GetByName(string name)
        {
            // Query the global registry for a parser with the specified name.
            IntPtr result = NativeMethods.BNGetDebugInfoParserByName(name);

            // Wrap as a new owned reference (the native call returns an addref'd pointer).
            return DebugInfoParser.NewFromHandle(result);
        }

        /// <summary>
        /// Gets all registered debug info parsers from the engine.
        /// Each returned parser is a new owned reference.
        /// </summary>
        /// <returns>An array of all registered DebugInfoParser instances.</returns>
        public static unsafe DebugInfoParser[] GetParsers()
        {
            // 1. Stack-allocate the count variable.
            ulong count = 0;

            // 2. Retrieve the native array of parser pointers.
            IntPtr arrayPointer = NativeMethods.BNGetDebugInfoParsers((IntPtr)(&count));

            // 3. Convert to managed array with new owned references and free the native pointer array.
            return UnsafeUtils.TakeHandleArrayEx<DebugInfoParser>(
                arrayPointer ,
                count ,
                DebugInfoParser.MustNewFromHandle ,
                NativeMethods.BNFreeDebugInfoParserList
            );
        }

        // ===================================================================
        // Instance properties and methods
        // ===================================================================

        /// <summary>
        /// Gets the registered name that uniquely identifies this debug info parser.
        /// </summary>
        public string Name
        {
            get
            {
                // 1. Retrieve the native ANSI string pointer for the parser name.
                IntPtr raw = NativeMethods.BNGetDebugInfoParserName(this.handle);

                // 2. Copy and free the native string, returning empty on null.
                return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
            }
        }

        /// <summary>
        /// Determines whether this parser is applicable to the given binary view.
        /// </summary>
        /// <param name="view">The binary view to check.</param>
        /// <returns>True if this parser can handle the provided view.</returns>
        public bool IsValidForView(BinaryView view)
        {
            // Pass the view handle to the native validity predicate.
            IntPtr viewHandle = (view != null) ? view.DangerousGetHandle() : IntPtr.Zero;

            return NativeMethods.BNIsDebugInfoParserValidForView(this.handle, viewHandle);
        }

        /// <summary>
        /// Parses debug information from the given binary view and optional debug file,
        /// merging the result into an existing DebugInfo object (or creating a fresh one).
        /// </summary>
        /// <param name="view">The primary binary view being analyzed.</param>
        /// <param name="debugFile">An optional separate debug file view; pass null to use the primary view.</param>
        /// <param name="existing">An existing DebugInfo to merge into; pass null to start fresh.</param>
        /// <returns>A new owned DebugInfo containing the parsed debug data, or null on failure.</returns>
        public DebugInfo? Parse(BinaryView view, BinaryView? debugFile = null, DebugInfo? existing = null)
        {
            // 1. Retrieve raw native handles for each optional argument.
            IntPtr viewHandle = (view != null) ? view.DangerousGetHandle() : IntPtr.Zero;
            IntPtr debugFileHandle = (debugFile != null) ? debugFile.DangerousGetHandle() : IntPtr.Zero;
            IntPtr existingHandle = (existing != null) ? existing.DangerousGetHandle() : IntPtr.Zero;

            // 2. Invoke the native parser (progress and context are unused; pass zero).
            IntPtr result = NativeMethods.BNParseDebugInfo(
                this.handle,
                viewHandle,
                debugFileHandle,
                existingHandle,
                IntPtr.Zero,
                IntPtr.Zero
            );

            // 3. Wrap the returned owned handle.
            return DebugInfo.TakeHandle(result);
        }
    }
}