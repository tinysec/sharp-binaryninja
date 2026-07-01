using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class DisassemblyTextRenderer : AbstractSafeHandle<DisassemblyTextRenderer>
	{
	    internal DisassemblyTextRenderer(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }
	    
	    internal static DisassemblyTextRenderer? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new DisassemblyTextRenderer(
			    NativeMethods.BNNewDisassemblyTextRendererReference(handle) ,
			    true
		    );
	    }
	    
	    internal static DisassemblyTextRenderer MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new DisassemblyTextRenderer(
			    NativeMethods.BNNewDisassemblyTextRendererReference(handle) ,
			    true
		    );
	    }
	    
	    internal static DisassemblyTextRenderer? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new DisassemblyTextRenderer(handle, true);
	    }
	    
	    internal static DisassemblyTextRenderer MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new DisassemblyTextRenderer(handle, true);
	    }
	    
	    internal static DisassemblyTextRenderer? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new DisassemblyTextRenderer(handle, false);
	    }
	    
	    internal static DisassemblyTextRenderer MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new DisassemblyTextRenderer(handle, false);
	    }

        /// <summary>
        /// Releases the native BNDisassemblyTextRenderer handle when this instance is disposed or finalized.
        /// </summary>
        /// <returns>True if the handle was successfully released.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                // Free the native renderer handle and mark it invalid to prevent double-free.
                NativeMethods.BNFreeDisassemblyTextRenderer(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        /// <summary>
        /// Gets the function this renderer is attached to.
        /// Returns null if no function is associated.
        /// </summary>
        public Function? Function
        {
            get
            {
                // Retrieve a new owned reference to the function from the native renderer.
                return BinaryNinja.Function.NewFromHandle(
                    NativeMethods.BNGetDisassemblyTextRendererFunction(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets the architecture associated with this renderer.
        /// Returns null if no architecture is set.
        /// </summary>
        public Architecture? Architecture
        {
            get
            {
                // Architecture handles are borrowed singletons; do not addref.
                return BinaryNinja.Architecture.FromHandle(
                    NativeMethods.BNGetDisassemblyTextRendererArchitecture(this.handle)
                );
            }

            set
            {
                // Forward the architecture handle (null clears the association).
                NativeMethods.BNSetDisassemblyTextRendererArchitecture(
                    this.handle,
                    (value != null) ? value.DangerousGetHandle() : IntPtr.Zero
                );
            }
        }

        /// <summary>
        /// Gets the basic block currently being rendered.
        /// Returns null if no basic block is set.
        /// </summary>
        public BasicBlock? BasicBlock
        {
            get
            {
                // Retrieve a new owned reference to the basic block from the native renderer.
                return BinaryNinja.BasicBlock.NewFromHandle(
                    NativeMethods.BNGetDisassemblyTextRendererBasicBlock(this.handle)
                );
            }

            set
            {
                // Forward the basic block handle (null clears the association).
                NativeMethods.BNSetDisassemblyTextRendererBasicBlock(
                    this.handle,
                    (value != null) ? value.DangerousGetHandle() : IntPtr.Zero
                );
            }
        }

        /// <summary>
        /// Gets or sets the disassembly settings controlling how output is formatted.
        /// Returns null if no settings are configured.
        /// </summary>
        public DisassemblySettings? Settings
        {
            get
            {
                // Retrieve a new owned reference to the settings from the native renderer.
                return DisassemblySettings.NewFromHandle(
                    NativeMethods.BNGetDisassemblyTextRendererSettings(this.handle)
                );
            }

            set
            {
                // Forward the settings handle (null clears the association).
                NativeMethods.BNSetDisassemblyTextRendererSettings(
                    this.handle,
                    (value != null) ? value.DangerousGetHandle() : IntPtr.Zero
                );
            }
        }

        /// <summary>
        /// Gets the low-level IL function associated with this renderer, if any.
        /// Returns null when this renderer is not attached to an LLIL function.
        /// </summary>
        public LowLevelILFunction? LowLevelILFunction
        {
            get
            {
                // Retrieve a new owned reference to the LLIL function from the native renderer.
                return BinaryNinja.LowLevelILFunction.NewFromHandle(
                    NativeMethods.BNGetDisassemblyTextRendererLowLevelILFunction(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets the medium-level IL function associated with this renderer, if any.
        /// Returns null when this renderer is not attached to an MLIL function.
        /// </summary>
        public MediumLevelILFunction? MediumLevelILFunction
        {
            get
            {
                // Retrieve a new owned reference to the MLIL function from the native renderer.
                return BinaryNinja.MediumLevelILFunction.NewFromHandle(
                    NativeMethods.BNGetDisassemblyTextRendererMediumLevelILFunction(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets the high-level IL function associated with this renderer, if any.
        /// Returns null when this renderer is not attached to an HLIL function.
        /// </summary>
        public HighLevelILFunction? HighLevelILFunction
        {
            get
            {
                // Retrieve a new owned reference to the HLIL function from the native renderer.
                return BinaryNinja.HighLevelILFunction.NewFromHandle(
                    NativeMethods.BNGetDisassemblyTextRendererHighLevelILFunction(this.handle)
                );
            }
        }

        /// <summary>
        /// Gets whether this renderer operates on an IL (Intermediate Language) function
        /// rather than raw disassembly.
        /// </summary>
        public bool IsIL
        {
            get
            {
                // Query the native layer for the IL-renderer flag.
                return NativeMethods.BNIsILDisassemblyTextRenderer(this.handle);
            }
        }

        /// <summary>
        /// Gets whether this renderer has data-flow analysis information available.
        /// </summary>
        public bool HasDataFlow
        {
            get
            {
                // Query the native layer for the data-flow availability flag.
                return NativeMethods.BNDisassemblyTextRendererHasDataFlow(this.handle);
            }
        }

        /// <summary>
        /// Resets the set of deduplicated comments tracked by this renderer.
        /// Call this before re-rendering a block to ensure all comments appear again.
        /// </summary>
        public void ResetDeduplicatedComments()
        {
            // Delegate to the native reset API.
            NativeMethods.BNResetDisassemblyTextRendererDeduplicatedComments(this.handle);
        }

        // ===================================================================
        // Static factory methods
        // ===================================================================

        /// <summary>
        /// Creates a new disassembly text renderer for the given function.
        /// </summary>
        /// <param name="func">The function to render disassembly text for.</param>
        /// <param name="settings">Optional disassembly settings; null uses defaults.</param>
        /// <returns>A new DisassemblyTextRenderer instance.</returns>
        public static DisassemblyTextRenderer Create(Function func , DisassemblySettings? settings = null)
        {
            return DisassemblyTextRenderer.MustTakeHandle(
                NativeMethods.BNCreateDisassemblyTextRenderer(
                    func.DangerousGetHandle() ,
                    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
                )
            );
        }

        /// <summary>
        /// Creates a new disassembly text renderer for a high-level IL function.
        /// </summary>
        /// <param name="func">The HLIL function to render text for.</param>
        /// <param name="settings">Optional disassembly settings; null uses defaults.</param>
        /// <returns>A new DisassemblyTextRenderer instance.</returns>
        public static DisassemblyTextRenderer CreateForHighLevelIL(HighLevelILFunction func , DisassemblySettings? settings = null)
        {
            return DisassemblyTextRenderer.MustTakeHandle(
                NativeMethods.BNCreateHighLevelILDisassemblyTextRenderer(
                    func.DangerousGetHandle() ,
                    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
                )
            );
        }

        /// <summary>
        /// Creates a new disassembly text renderer for a low-level IL function.
        /// </summary>
        /// <param name="func">The LLIL function to render text for.</param>
        /// <param name="settings">Optional disassembly settings; null uses defaults.</param>
        /// <returns>A new DisassemblyTextRenderer instance.</returns>
        public static DisassemblyTextRenderer CreateForLowLevelIL(LowLevelILFunction func , DisassemblySettings? settings = null)
        {
            return DisassemblyTextRenderer.MustTakeHandle(
                NativeMethods.BNCreateLowLevelILDisassemblyTextRenderer(
                    func.DangerousGetHandle() ,
                    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
                )
            );
        }

        /// <summary>
        /// Creates a new disassembly text renderer for a medium-level IL function.
        /// </summary>
        /// <param name="func">The MLIL function to render text for.</param>
        /// <param name="settings">Optional disassembly settings; null uses defaults.</param>
        /// <returns>A new DisassemblyTextRenderer instance.</returns>
        public static DisassemblyTextRenderer CreateForMediumLevelIL(MediumLevelILFunction func , DisassemblySettings? settings = null)
        {
            return DisassemblyTextRenderer.MustTakeHandle(
                NativeMethods.BNCreateMediumLevelILDisassemblyTextRenderer(
                    func.DangerousGetHandle() ,
                    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
                )
            );
        }

        // ===================================================================
        // Instance methods
        // ===================================================================

        /// <summary>
        /// Gets the instruction text tokens at the given address.
        /// Returns null if no instruction was found at the address.
        /// </summary>
        /// <param name="addr">The address to retrieve instruction text for.</param>
        /// <param name="len">Receives the number of bytes consumed by the instruction.</param>
        /// <returns>An array of disassembly text lines, or null on failure.</returns>
        public unsafe DisassemblyTextLine[]? GetInstructionText(ulong addr , out ulong len)
        {
            // 1. Allocate stack-local storage for the three output parameters.
            ulong nativeLen = 0;
            IntPtr nativeResult = IntPtr.Zero;
            ulong nativeCount = 0;

            // 2. Call the native API, passing pointers to our local variables.
            bool ok = NativeMethods.BNGetDisassemblyTextRendererInstructionText(
                this.handle ,
                addr ,
                (IntPtr)(&nativeLen) ,
                (IntPtr)(&nativeResult) ,
                (IntPtr)(&nativeCount)
            );

            // 3. Write the consumed length to the out parameter.
            len = nativeLen;

            if (!ok || nativeResult == IntPtr.Zero)
            {
                return null;
            }

            // 4. Convert the native BNDisassemblyTextLine array to managed objects and free the native memory.
            return UnsafeUtils.TakeStructArrayEx<BNDisassemblyTextLine , DisassemblyTextLine>(
                nativeResult ,
                nativeCount ,
                DisassemblyTextLine.FromNative ,
                NativeMethods.BNFreeDisassemblyTextLines
            );
        }

        /// <summary>
        /// Gets the disassembly lines (including annotations and comments) at the given address.
        /// Returns null if no lines were produced for the address.
        /// </summary>
        /// <param name="addr">The address to retrieve lines for.</param>
        /// <param name="len">Receives the number of bytes consumed by the instruction.</param>
        /// <returns>An array of disassembly text lines, or null on failure.</returns>
        public unsafe DisassemblyTextLine[]? GetLines(ulong addr , out ulong len)
        {
            // 1. Allocate stack-local storage for the three output parameters.
            ulong nativeLen = 0;
            IntPtr nativeResult = IntPtr.Zero;
            ulong nativeCount = 0;

            // 2. Call the native API, passing pointers to our local variables.
            bool ok = NativeMethods.BNGetDisassemblyTextRendererLines(
                this.handle ,
                addr ,
                (IntPtr)(&nativeLen) ,
                (IntPtr)(&nativeResult) ,
                (IntPtr)(&nativeCount)
            );

            // 3. Write the consumed length to the out parameter.
            len = nativeLen;

            if (!ok || nativeResult == IntPtr.Zero)
            {
                return null;
            }

            // 4. Convert the native BNDisassemblyTextLine array to managed objects and free the native memory.
            return UnsafeUtils.TakeStructArrayEx<BNDisassemblyTextLine , DisassemblyTextLine>(
                nativeResult ,
                nativeCount ,
                DisassemblyTextLine.FromNative ,
                NativeMethods.BNFreeDisassemblyTextLines
            );
        }

        /// <summary>
        /// Gets the instruction annotation tokens (e.g., register values) at the given address.
        /// </summary>
        /// <param name="addr">The address to retrieve annotation tokens for.</param>
        /// <returns>An array of annotation tokens. Empty array if none available.</returns>
        public unsafe InstructionTextToken[] GetInstructionAnnotations(ulong addr)
        {
            // 1. Allocate stack-local storage for the count output parameter.
            ulong nativeCount = 0;

            // 2. Call the native API which returns a pointer to the token array.
            IntPtr arrayPointer = NativeMethods.BNGetDisassemblyTextRendererInstructionAnnotations(
                this.handle ,
                addr ,
                (IntPtr)(&nativeCount)
            );

            // 3. Convert the native token array to managed objects and free the native memory.
            return UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken , InstructionTextToken>(
                arrayPointer ,
                nativeCount ,
                InstructionTextToken.FromNative ,
                NativeMethods.BNFreeInstructionText
            );
        }

        /// <summary>
        /// Gets the symbol tokens at the given address for this renderer.
        /// Returns false if no symbol tokens could be resolved.
        /// </summary>
        /// <param name="addr">The address of the symbol reference.</param>
        /// <param name="size">The size of the operand referencing the symbol.</param>
        /// <param name="operand">The operand index within the instruction.</param>
        /// <param name="tokens">Receives the array of symbol tokens on success.</param>
        /// <returns>True if symbol tokens were resolved; false otherwise.</returns>
        public unsafe bool GetSymbolTokens(
            ulong addr ,
            ulong size ,
            ulong operand ,
            out InstructionTextToken[] tokens)
        {
            // 1. Prepare stack-local output slots for the token array pointer and count.
            IntPtr resultPtr = IntPtr.Zero;
            ulong count = 0;

            // 2. Call the native API to resolve symbol tokens.
            bool success = NativeMethods.BNGetDisassemblyTextRendererSymbolTokens(
                this.handle ,
                addr ,
                size ,
                operand ,
                (IntPtr)(&resultPtr) ,
                (IntPtr)(&count)
            );

            if (!success || resultPtr == IntPtr.Zero)
            {
                tokens = Array.Empty<InstructionTextToken>();

                return false;
            }

            // 3. Convert the native token array to managed objects and free the native memory.
            tokens = UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken , InstructionTextToken>(
                resultPtr ,
                count ,
                InstructionTextToken.FromNative ,
                NativeMethods.BNFreeInstructionText
            );

            return true;
        }

        /// <summary>
        /// Gets symbol tokens for a symbol reference using a static context (no renderer instance).
        /// This overload resolves symbols from a binary view and optional function directly.
        /// </summary>
        /// <param name="addr">The address of the symbol reference.</param>
        /// <param name="size">The size of the operand referencing the symbol.</param>
        /// <param name="operand">The operand index within the instruction.</param>
        /// <param name="data">The binary view to resolve symbols from.</param>
        /// <param name="maxSymbolWidth">Maximum character width for symbol name display.</param>
        /// <param name="func">Optional function context; null if outside a function.</param>
        /// <param name="confidence">Confidence level for the symbol reference (0-255).</param>
        /// <param name="symbolDisplay">How to display the symbol (e.g., full name, short name).</param>
        /// <param name="precedence">Operator precedence context for formatting.</param>
        /// <param name="instrAddr">The address of the instruction containing the operand.</param>
        /// <param name="exprIndex">The expression index within the IL representation.</param>
        /// <param name="tokens">Receives the array of symbol tokens on success.</param>
        /// <returns>The symbol display result indicating success or failure type.</returns>
        public static unsafe SymbolDisplayResult GetSymbolTokensStatic(
            ulong addr ,
            ulong size ,
            ulong operand ,
            BinaryView data ,
            ulong maxSymbolWidth ,
            Function? func ,
            byte confidence ,
            SymbolDisplayType symbolDisplay ,
            OperatorPrecedence precedence ,
            ulong instrAddr ,
            ulong exprIndex ,
            out InstructionTextToken[] tokens)
        {
            // 1. Prepare stack-local output slots.
            IntPtr resultPtr = IntPtr.Zero;
            ulong count = 0;

            // 2. Resolve optional function handle.
            IntPtr funcHandle = (func != null) ? func.DangerousGetHandle() : IntPtr.Zero;

            // 3. Call the native static symbol token resolver.
            SymbolDisplayResult displayResult = NativeMethods.BNGetDisassemblyTextRendererSymbolTokensStatic(
                addr ,
                size ,
                operand ,
                data.DangerousGetHandle() ,
                maxSymbolWidth ,
                funcHandle ,
                confidence ,
                symbolDisplay ,
                precedence ,
                instrAddr ,
                exprIndex ,
                (IntPtr)(&resultPtr) ,
                (IntPtr)(&count)
            );

            if (resultPtr == IntPtr.Zero)
            {
                tokens = Array.Empty<InstructionTextToken>();

                return displayResult;
            }

            // 4. Convert the native token array to managed objects and free the native memory.
            tokens = UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken , InstructionTextToken>(
                resultPtr ,
                count ,
                InstructionTextToken.FromNative ,
                NativeMethods.BNFreeInstructionText
            );

            return displayResult;
        }

        /// <summary>
        /// Gets the integer display tokens for a given instruction text token,
        /// applying the renderer's formatting rules (e.g., hex vs decimal display).
        /// </summary>
        /// <param name="token">The instruction text token containing the integer value.</param>
        /// <param name="arch">The architecture context for formatting.</param>
        /// <param name="addr">The address of the instruction containing the token.</param>
        /// <returns>An array of formatted integer tokens.</returns>
        public unsafe InstructionTextToken[] GetIntegerTokens(
            InstructionTextToken token ,
            Architecture arch ,
            ulong addr)
        {
            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                // 1. Convert the managed token to a native struct and allocate it.
                BNInstructionTextToken nativeToken = token.ToNativeEx(allocator);
                IntPtr tokenPtr = allocator.AllocStruct<BNInstructionTextToken>(nativeToken);

                // 2. Stack-allocate the count output parameter.
                ulong count = 0;

                // 3. Call the native API.
                IntPtr resultPtr = NativeMethods.BNGetDisassemblyTextRendererIntegerTokens(
                    this.handle ,
                    tokenPtr ,
                    arch.DangerousGetHandle() ,
                    addr ,
                    (IntPtr)(&count)
                );

                // 4. Convert the native result array to managed objects and free native memory.
                return UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken , InstructionTextToken>(
                    resultPtr ,
                    count ,
                    InstructionTextToken.FromNative ,
                    NativeMethods.BNFreeInstructionText
                );
            }
        }

        /// <summary>
        /// Gets the display tokens for a stack variable reference,
        /// applying the renderer's formatting rules.
        /// </summary>
        /// <param name="reference">The stack variable reference to render tokens for.</param>
        /// <returns>An array of instruction text tokens for the stack variable reference.</returns>
        public unsafe InstructionTextToken[] GetStackVariableReferenceTokens(
            StackVariableReference reference)
        {
            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                // 1. Build the native BNStackVariableReference struct.
                BNStackVariableReference nativeRef = new BNStackVariableReference();
                nativeRef.sourceOperand = reference.SourceOperand;
                nativeRef.typeConfidence = reference.TypeConfidence;
                nativeRef.type = (reference.Type != null) ? reference.Type.DangerousGetHandle() : IntPtr.Zero;
                nativeRef.name = allocator.AllocAnsiString(reference.Name);
                nativeRef.varIdentifier = reference.VarIdentifier;
                nativeRef.referencedOffset = reference.ReferencedOffset;
                nativeRef.size = reference.Size;

                // 2. Allocate the native struct on the unmanaged heap.
                IntPtr refPtr = allocator.AllocStruct<BNStackVariableReference>(nativeRef);

                // 3. Stack-allocate the count output parameter.
                ulong count = 0;

                // 4. Call the native API.
                IntPtr resultPtr = NativeMethods.BNGetDisassemblyTextRendererStackVariableReferenceTokens(
                    this.handle ,
                    refPtr ,
                    (IntPtr)(&count)
                );

                // 5. Convert the native result array to managed objects and free native memory.
                return UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken , InstructionTextToken>(
                    resultPtr ,
                    count ,
                    InstructionTextToken.FromNative ,
                    NativeMethods.BNFreeInstructionText
                );
            }
        }

        /// <summary>
        /// Wraps a comment string into multiple disassembly text lines,
        /// splitting it to fit the configured line width.
        /// </summary>
        /// <param name="line">The source disassembly text line to annotate.</param>
        /// <param name="comment">The comment text to wrap.</param>
        /// <param name="hasAutoAnnotations">True if the line has auto-generated annotations.</param>
        /// <param name="leadingSpaces">Leading whitespace to prepend to each wrapped line.</param>
        /// <param name="indentSpaces">Indentation whitespace for continuation lines.</param>
        /// <returns>An array of disassembly text lines containing the wrapped comment.</returns>
        public unsafe DisassemblyTextLine[] WrapComment(
            DisassemblyTextLine line ,
            string comment ,
            bool hasAutoAnnotations ,
            string leadingSpaces ,
            string indentSpaces
        )
        {
            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                // 1. Convert the managed line to a native struct and allocate it.
                BNDisassemblyTextLine nativeLine = line.ToNativeEx(allocator);
                IntPtr linePtr = allocator.AllocStruct<BNDisassemblyTextLine>(nativeLine);

                // 2. Allocate stack-local storage for the output line count.
                ulong nativeCount = 0;

                // 3. Call the native API to perform the comment wrapping.
                IntPtr resultPointer = NativeMethods.BNDisassemblyTextRendererWrapComment(
                    this.handle ,
                    linePtr ,
                    (IntPtr)(&nativeCount) ,
                    comment ,
                    hasAutoAnnotations ,
                    leadingSpaces ,
                    indentSpaces
                );

                // 4. Convert the native result array to managed objects and free the native memory.
                return UnsafeUtils.TakeStructArrayEx<BNDisassemblyTextLine , DisassemblyTextLine>(
                    resultPointer ,
                    nativeCount ,
                    DisassemblyTextLine.FromNative ,
                    NativeMethods.BNFreeDisassemblyTextLines
                );
            }
        }

        /// <summary>
        /// Post-processes a set of disassembly text lines, applying rendering transformations
        /// such as render layer annotations and indentation. The input lines are consumed and
        /// a new array of transformed lines is returned.
        /// </summary>
        /// <param name="address">The start address of the lines being processed.</param>
        /// <param name="length">The byte length of the address range covered by the lines.</param>
        /// <param name="lines">The input disassembly text lines to post-process.</param>
        /// <param name="indentSpaces">The indentation string to prepend to output lines.</param>
        /// <returns>An array of post-processed DisassemblyTextLine objects.</returns>
        public unsafe DisassemblyTextLine[] PostProcessLines(
            ulong address ,
            ulong length ,
            DisassemblyTextLine[] lines ,
            string indentSpaces = ""
        )
        {
            // 1. Handle empty input.
            if (null == lines || 0 == lines.Length)
            {
                return Array.Empty<DisassemblyTextLine>();
            }

            using (ScopedAllocator allocator = new ScopedAllocator())
            {
                // 2. Marshal the input lines to native structs.
                BNDisassemblyTextLine[] nativeLines = new BNDisassemblyTextLine[lines.Length];
                for (int i = 0; i < lines.Length; i++)
                {
                    nativeLines[i] = lines[i].ToNativeEx(allocator);
                }
                IntPtr linesPtr = allocator.AllocStructArray<BNDisassemblyTextLine>(nativeLines);

                // 3. Stack-allocate the output count.
                ulong outCount = 0;

                // 4. Call the native API.
                IntPtr resultPointer = NativeMethods.BNPostProcessDisassemblyTextRendererLines(
                    this.handle ,
                    address ,
                    length ,
                    linesPtr ,
                    (ulong)lines.Length ,
                    (IntPtr)(&outCount) ,
                    indentSpaces ?? string.Empty
                );

                // 5. Convert the native result array to managed objects and free the native memory.
                return UnsafeUtils.TakeStructArrayEx<BNDisassemblyTextLine , DisassemblyTextLine>(
                    resultPointer ,
                    outCount ,
                    DisassemblyTextLine.FromNative ,
                    NativeMethods.BNFreeDisassemblyTextLines
                );
            }
        }
    }
}