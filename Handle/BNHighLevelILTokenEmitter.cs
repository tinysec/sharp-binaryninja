using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed partial class HighLevelILTokenEmitter : AbstractSafeHandle<HighLevelILTokenEmitter>
	{
	    internal HighLevelILTokenEmitter(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }
	    
	    internal static HighLevelILTokenEmitter? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new HighLevelILTokenEmitter(
			    NativeMethods.BNNewHighLevelILTokenEmitterReference(handle) ,
			    true
		    );
	    }
	    
	    internal static HighLevelILTokenEmitter MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new HighLevelILTokenEmitter(
			    NativeMethods.BNNewHighLevelILTokenEmitterReference(handle) ,
			    true
		    );
	    }
	    
	    internal static HighLevelILTokenEmitter? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new HighLevelILTokenEmitter(handle, true);
	    }
	    
	    internal static HighLevelILTokenEmitter MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new HighLevelILTokenEmitter(handle, true);
	    }
	    
	    internal static HighLevelILTokenEmitter? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new HighLevelILTokenEmitter(handle, false);
	    }
	    
	    internal static HighLevelILTokenEmitter MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new HighLevelILTokenEmitter(handle, false);
	    }

	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeHighLevelILTokenEmitter(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }

	    public void InitLine()
	    {
		    NativeMethods.BNHighLevelILTokenEmitterInitLine(this.handle);
	    }
	    
	    public void NewLine()
	    {
		    NativeMethods.BNHighLevelILTokenEmitterNewLine(this.handle);
	    }
	    
	    public void IncreaseIndent()
	    {
		    NativeMethods.BNHighLevelILTokenEmitterIncreaseIndent(this.handle);
	    }
	    
	    public void DecreaseIndent()
	    {
		    NativeMethods.BNHighLevelILTokenEmitterDecreaseIndent(this.handle);
	    }
	    
	    public void ScopeSeparator()
	    {
		    NativeMethods.BNHighLevelILTokenEmitterScopeSeparator(this.handle);
	    }
	    
	    public void BeginScope(ScopeType type)
	    {
		    NativeMethods.BNHighLevelILTokenEmitterBeginScope(this.handle , type);
	    }
	    
	    public void EndScope(ScopeType type)
	    {
		    NativeMethods.BNHighLevelILTokenEmitterEndScope(this.handle , type);
	    }
	    
	    public void ScopeContinuation(bool forceSameLine)
	    {
		    NativeMethods.BNHighLevelILTokenEmitterScopeContinuation(this.handle , forceSameLine);
	    }
	    
	    public void FinalizeScope()
	    {
		    NativeMethods.BNHighLevelILTokenEmitterFinalizeScope(this.handle);
	    }
	    
	    public void NoIndentForThisLine()
	    {
		    NativeMethods.BNHighLevelILTokenEmitterNoIndentForThisLine(this.handle);
	    }

	    public void PrependCollapseBlankIndicator()
	    {
		    NativeMethods.BNHighLevelILTokenPrependCollapseBlankIndicator(this.handle);
	    }
	    
	    public void PrependCollapseIndicator(InstructionTextTokenContext context , ulong hash )
	    {
		    NativeMethods.BNHighLevelILTokenPrependCollapseIndicator(
			    this.handle,
			    context,
			    hash
			);
	    }
	    
	    public bool HasCollapsableRegions
	    {
		    get
		    {
			    return NativeMethods.BNHighLevelILTokenEmitterHasCollapsableRegions(this.handle);
		    }

		    set
		    {
			    NativeMethods.BNHighLevelILTokenEmitterSetHasCollapsableRegions(this.handle, value);
		    }
	    }
	    
	    public void BeginForceZeroConfidence()
	    {
		    NativeMethods.BNHighLevelILTokenEmitterBeginForceZeroConfidence(
			    this.handle
		    );
	    }
	    
	    public void EndForceZeroConfidence()
	    {
		    NativeMethods.BNHighLevelILTokenEmitterEndForceZeroConfidence(
			    this.handle
		    );
	    }
	    
	    public TokenEmitterExpression SetCurrentExpr(TokenEmitterExpression expr)
	    {
		    if (null == expr)
		    {
			    throw new ArgumentNullException(nameof(expr));
		    }

		    BNTokenEmitterExpr previous =
			    NativeMethods.BNHighLevelILTokenEmitterSetCurrentExpr(
			    this.handle ,
			    expr.ToNative()
		    );

		    return TokenEmitterExpression.FromNative(previous);
	    }
	    
	    public void RestoreCurrentExpr(TokenEmitterExpression expr)
	    {
		    if (null == expr)
		    {
			    throw new ArgumentNullException(nameof(expr));
		    }

		    NativeMethods.BNHighLevelILTokenEmitterRestoreCurrentExpr(
			    this.handle,
			    expr.ToNative()
		    );
	    }
	    
	    public void FinalizeEmit()
	    {
		    NativeMethods.BNHighLevelILTokenEmitterFinalize(
			    this.handle
		    );
	    }
	    
	    public void Append(InstructionTextToken token)
	    {
		    if (null == token)
		    {
			    throw new ArgumentNullException(nameof(token));
		    }

		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    NativeMethods.BNHighLevelILTokenEmitterAppend(
				    this.handle,
				    token.ToNativeEx(allocator)
			    );
		    }
	    }
	    
	    public void AppendOpenParen()
	    {
		    NativeMethods.BNHighLevelILTokenEmitterAppendOpenParen(
			    this.handle
		    );
	    }
	    
	    public void AppendCloseParen()
	    {
		    NativeMethods.BNHighLevelILTokenEmitterAppendCloseParen(
			    this.handle
		    );
	    }
	    
	    public void AppendOpenBracket()
	    {
		    NativeMethods.BNHighLevelILTokenEmitterAppendOpenBracket(
			    this.handle
		    );
	    }
	    
	    public void AppendCloseBracket()
	    {
		    NativeMethods.BNHighLevelILTokenEmitterAppendCloseBracket(
			    this.handle
		    );
	    }
	    
	    public void AppendOpenBrace()
	    {
		    NativeMethods.BNHighLevelILTokenEmitterAppendOpenBrace(
			    this.handle
		    );
	    }
	    
	    public void AppendCloseBrace()
	    {
		    NativeMethods.BNHighLevelILTokenEmitterAppendCloseBrace(
			    this.handle
		    );
	    }
	    
	    public void AppendSemicolon()
	    {
		    NativeMethods.BNHighLevelILTokenEmitterAppendSemicolon(
			    this.handle
		    );
	    }
	    
	    public BraceRequirement BraceRequirement
	    {
		    get
		    {
			    return NativeMethods.BNHighLevelILTokenEmitterGetBraceRequirement(
				    this.handle
			    );
		    }

		    set
		    {
			    this.SetBraceRequirement(value);
		    }
	    }

	    public InstructionTextToken[] GetCurrentTokens()
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNHighLevelILTokenEmitterGetCurrentTokens(
				    this.handle ,
				    countPtr
			    );

			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    return UnsafeUtils.TakeStructArrayEx<BNInstructionTextToken , InstructionTextToken>(
				    arrayPointer ,
				    arrayLength ,
				    InstructionTextToken.FromNative ,
				    NativeMethods.BNFreeInstructionText
			    );
		    }
	    }

	    public void SetCurrentTokens(InstructionTextToken[] tokens)
	    {
		    if (null == tokens)
		    {
			    throw new ArgumentNullException(nameof(tokens));
		    }

		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    NativeMethods.BNHighLevelILTokenEmitterSetCurrentTokens(
				    this.handle ,
				    allocator.AllocStructArray(
					    allocator.ConvertToNativeArrayEx<BNInstructionTextToken,InstructionTextToken>(
						    tokens
					    )
				    ) ,
				    (ulong)tokens.Length
			    );
		    }
	    }

	    public DisassemblyTextLine[] GetLines()
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    IntPtr countPtr = allocator.AllocStruct<ulong>(0);

			    IntPtr arrayPointer = NativeMethods.BNHighLevelILTokenEmitterGetLines(
				    this.handle ,
				    countPtr
			    );

			    ulong arrayLength = (ulong)Marshal.ReadInt64(countPtr);

			    return UnsafeUtils.TakeStructArrayEx<BNDisassemblyTextLine,DisassemblyTextLine>(
				    arrayPointer ,
				    arrayLength ,
				    DisassemblyTextLine.FromNative ,
				    NativeMethods.BNFreeDisassemblyTextLines
			    );
		    }
	    }

	    public bool DefaultBracesOnSameLine
	    {
		    get
		    {
			    return NativeMethods.BNHighLevelILTokenEmitterGetDefaultBracesOnSameLine(
				    this.handle
			    );
		    }

		    set
		    {
			    NativeMethods.BNHighLevelILTokenEmitterSetDefaultBracesOnSameLine(
				    this.handle ,
				    value
			    );
		    }
	    }

	    public bool HasBracesAroundSwitchCases
	    {
		    get
		    {
			    return NativeMethods.BNHighLevelILTokenEmitterHasBracesAroundSwitchCases(
				    this.handle
			    );
		    }

		    set
		    {
			    NativeMethods.BNHighLevelILTokenEmitterSetBracesAroundSwitchCases(
				    this.handle ,
				    value
			    );
		    }
	    }

	    public bool IsSimpleScopeAllowed
	    {
		    get
		    {
			    return NativeMethods.BNHighLevelILTokenEmitterIsSimpleScopeAllowed(
				    this.handle
			    );
		    }

		    set
		    {
			    NativeMethods.BNHighLevelILTokenEmitterSetSimpleScopeAllowed(
				    this.handle ,
				    value
			    );
		    }
	    }

	    public void SetBraceRequirement(BraceRequirement required)
	    {
		    NativeMethods.BNHighLevelILTokenEmitterSetBraceRequirement(
			    this.handle ,
			    required
		    );
	    }

	    public ulong GetMaxTernarySimplficationTokens()
	    {
		    return NativeMethods.BNHighLevelILTokenEmitterGetMaxTernarySimplficationTokens(
			    this.handle
		    );
	    }

	    public ulong GetMaxTernarySimplificationTokens()
	    {
		    return this.GetMaxTernarySimplficationTokens();
	    }

	    /// <summary>
	    /// Emits a floating-point size token (e.g., "float", "double") into the given token emitter.
	    /// This is a static utility that operates on a token emitter instance.
	    /// </summary>
	    /// <param name="size">The floating-point size in bytes (e.g., 4 for float, 8 for double).</param>
	    /// <param name="type">The token type to use for the emitted token.</param>
	    /// <param name="tokens">The HighLevelILTokenEmitter to emit into.</param>
	    public static void AddFloatSizeToken(ulong size , InstructionTextTokenType type , HighLevelILTokenEmitter tokens)
	    {
		    // Forward the size, token type, and emitter handle to the native API.
		    NativeMethods.BNAddHighLevelILFloatSizeToken(
			    size ,
			    type ,
			    tokens.DangerousGetHandle()
		    );
	    }

	    /// <summary>
	    /// Emits an integer size token (e.g., "int8_t", "int32_t") into the given token emitter.
	    /// This is a static utility that operates on a token emitter instance.
	    /// </summary>
	    /// <param name="size">The integer size in bytes (e.g., 1 for byte, 4 for int32).</param>
	    /// <param name="type">The token type to use for the emitted token.</param>
	    /// <param name="tokens">The HighLevelILTokenEmitter to emit into.</param>
	    public static void AddSizeToken(ulong size , InstructionTextTokenType type , HighLevelILTokenEmitter tokens)
	    {
		    // Forward the size, token type, and emitter handle to the native API.
		    NativeMethods.BNAddHighLevelILSizeToken(
			    size ,
			    type ,
			    tokens.DangerousGetHandle()
		    );
	    }
	}
}
