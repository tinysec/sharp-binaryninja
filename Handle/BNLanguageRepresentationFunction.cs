using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class LanguageRepresentationFunction : AbstractSafeHandle<LanguageRepresentationFunction>
	{
	    internal LanguageRepresentationFunction(IntPtr handle , bool owner) 
		    : base(handle , owner)
	    {
	        
	    }
	    
	    internal static LanguageRepresentationFunction? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new LanguageRepresentationFunction(
			    NativeMethods.BNNewLanguageRepresentationFunctionReference(handle) ,
			    true
		    );
	    }
	    
	    internal static LanguageRepresentationFunction MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new LanguageRepresentationFunction(
			    NativeMethods.BNNewLanguageRepresentationFunctionReference(handle) ,
			    true
		    );
	    }
	    
	    internal static LanguageRepresentationFunction? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new LanguageRepresentationFunction(handle, true);
	    }
	    
	    internal static LanguageRepresentationFunction MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new LanguageRepresentationFunction(handle, true);
	    }
	    
	    internal static LanguageRepresentationFunction? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new LanguageRepresentationFunction(handle, false);
	    }
	    
	    internal static LanguageRepresentationFunction MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new LanguageRepresentationFunction(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeLanguageRepresentationFunction(this.handle);
	            this.SetHandleAsInvalid();
	        }

	        return true;
	    }

        // ===================================================================
        // Static factory methods
        // ===================================================================

        /// <summary>
        /// Creates a new language representation function from the given components.
        /// </summary>
        /// <param name="type">The language representation function type (e.g., Pseudo C).</param>
        /// <param name="arch">The architecture this function targets.</param>
        /// <param name="func">The owning function.</param>
        /// <param name="highLevelIL">The high-level IL function to represent.</param>
        /// <returns>A new owned LanguageRepresentationFunction instance.</returns>
        public static LanguageRepresentationFunction Create(
            LanguageRepresentationFunctionType type ,
            Architecture arch ,
            Function func ,
            HighLevelILFunction highLevelIL)
        {
            // Call the native factory with all four required handle arguments.
            IntPtr result = NativeMethods.BNCreateLanguageRepresentationFunction(
                type.DangerousGetHandle() ,
                arch.DangerousGetHandle() ,
                func.DangerousGetHandle() ,
                highLevelIL.DangerousGetHandle()
            );

            // Wrap as a new owned handle.
            return LanguageRepresentationFunction.MustTakeHandle(result);
        }

        // ===================================================================
        // Instance properties and methods
        // ===================================================================

	    public override string ToString()
	    {
		    return this.GetLinearDisassemblyText();
	    }

	    public Architecture Architecture
	    {
		    get
		    {
			    return Architecture.MustFromHandle(
				    NativeMethods.BNGetLanguageRepresentationArchitecture(this.handle)
			    );
		    }
	    }
	    
	    public Function OwnerFunction
	    {
		    get
		    {
			    return Function.MustTakeHandle(
				    NativeMethods.BNGetLanguageRepresentationOwnerFunction(this.handle)
			    );
		    }
	    }
	    
	    // forward
	    public ulong HighestAddress
	    {
		    get
		    {
			    return this.OwnerFunction.HighestAddress;
		    }
	    }
	    
	    public ulong LowestAddress
	    {
		    get
		    {
			    return this.OwnerFunction.LowestAddress;
		    }
	    }
	    
	    public Symbol Symbol
	    {
		    get
		    {
			    return this.OwnerFunction.Symbol;
		    }
	    }
	    
	    public string ShortName
	    {
		    get
		    {
			    return this.Symbol.ShortName;
		    }
	    }
	    
	    public string FullName
	    {
		    get
		    {
			    return this.Symbol.FullName;
		    }
	    }
	    
	    public string RawName
	    {
		    get
		    {
			    return this.Symbol.RawName;
		    }
	    }
	    
	    public HighLevelILFunction HighLevelIL
	    {
		    get
		    {
			    return HighLevelILFunction.MustTakeHandle(
				    NativeMethods.BNGetLanguageRepresentationILFunction(this.handle)
			    );
		    }
	    }
	    
	    public MediumLevelILFunction? MediumLevelIL
	    {
		    get
		    {
			    return this.HighLevelIL.MediumLevelIL;
		    }
	    }
	    
	    public MediumLevelILFunction? MediumLevelILSSAForm
	    {
		    get
		    {
			    return this.HighLevelIL.MediumLevelILSSAForm;
		    }
	    }
	    
	    public MediumLevelILFunction? MediumLevelILNonSSAForm
	    {
		    get
		    {
			    return this.HighLevelIL.MediumLevelILNonSSAForm;
		    }
	    }
	    
	    public LowLevelILFunction? LowLevelIL
	    {
		    get
		    {
			    return this.HighLevelIL.LowLevelIL;
		    }
	    }
	    
	    public LinearViewObject CreateLinearView( 
		    DisassemblySettings? settings = null,
		    string language = "Pseudo C"
	    ) 
	    {
		    if (null == settings)
		    {
			    settings = DisassemblySettings.DefaultLinear();
			    settings.SetOption(DisassemblyOption.WaitForIL , true);
		    }
		    
		    return LinearViewObject.MustTakeHandle(
			    NativeMethods.BNCreateLinearViewSingleFunctionLanguageRepresentation(
				    this.OwnerFunction.DangerousGetHandle() ,
				    null == settings ? IntPtr.Zero : settings.DangerousGetHandle() ,
				    language
			    )
		    );
	    }
	    
	    public LinearDisassemblyLine[] GetLinearDisassemblyLines(
		    DisassemblySettings? settings = null,
		    string language = "Pseudo C"
		)
	    {
		    LinearViewObject linear = this.CreateLinearView(settings);
		    
		    LinearViewCursor cursor = linear.CreateCursor();
		    
		    List<LinearDisassemblyLine> targets = new List<LinearDisassemblyLine>();
		    
		    cursor.SeekToAddress(this.LowestAddress);
		    foreach (LinearDisassemblyLine line in cursor.PreviousLines)
		    {
			    if (!targets.Contains(line))
			    {
				    targets.Add(line);
			    }
		    }
		    
		    cursor.SeekToAddress(this.HighestAddress);
		    foreach (LinearDisassemblyLine line in cursor.NextLines)
		    {
			    if (!targets.Contains(line))
			    {
				    targets.Add(line);
			    }
		    }

		    return targets.ToArray();
	    }
	    
	    public IEnumerable<LinearDisassemblyLine> LinearDisassemblyLines
	    {
		    get
		    {
			    return this.GetLinearDisassemblyLines();
		    }
	    }
	    
	    public string GetLinearDisassemblyText(
		    DisassemblySettings? settings = null,
		    string language = "Pseudo C"
		)
	    {
		    StringBuilder builder = new StringBuilder();

		    foreach (LinearDisassemblyLine line in this.GetLinearDisassemblyLines(
			             settings ,
			             language
			             )
		             )
		    {
			    builder.AppendLine(line.ToString());
		    }

		    return builder.ToString();
	    }

	    public string LinearDisassemblyText
	    {
		    get
		    {
			    return this.GetLinearDisassemblyText();
		    }
	    }

	    // ===================================================================
	    // Annotation and comment strings
	    // ===================================================================

	    /// <summary>
	    /// Gets the string that marks the start of an annotation in this language.
	    /// </summary>
	    public string AnnotationStartString
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNGetLanguageRepresentationFunctionAnnotationStartString(this.handle)
			    );
		    }
	    }

	    /// <summary>
	    /// Gets the string that marks the end of an annotation in this language.
	    /// </summary>
	    public string AnnotationEndString
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNGetLanguageRepresentationFunctionAnnotationEndString(this.handle)
			    );
		    }
	    }

	    /// <summary>
	    /// Gets the string that marks the start of a comment in this language.
	    /// </summary>
	    public string CommentStartString
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNGetLanguageRepresentationFunctionCommentStartString(this.handle)
			    );
		    }
	    }

	    /// <summary>
	    /// Gets the string that marks the end of a comment in this language.
	    /// </summary>
	    public string CommentEndString
	    {
		    get
		    {
			    return UnsafeUtils.TakeAnsiString(
				    NativeMethods.BNGetLanguageRepresentationFunctionCommentEndString(this.handle)
			    );
		    }
	    }

	    // ===================================================================
	    // Highlight
	    // ===================================================================

	    /// <summary>
	    /// Gets the highlight color for this language representation function at the specified basic block.
	    /// </summary>
	    /// <param name="block">The basic block to get the highlight for.</param>
	    /// <returns>The highlight color for the block.</returns>
	    public HighlightColor GetHighlight(BasicBlock block)
	    {
		    return HighlightColor.FromNative(
			    NativeMethods.BNGetLanguageRepresentationFunctionHighlight(
				    this.handle ,
				    block.DangerousGetHandle()
			    )
		    );
	    }

	    // ===================================================================
	    // Language type lookup
	    // ===================================================================

	    /// <summary>
	    /// Gets the language representation function type associated with this instance.
	    /// </summary>
	    /// <returns>The LanguageRepresentationFunctionType, or null if not available.</returns>
	    public LanguageRepresentationFunctionType? GetLanguageType()
	    {
		    return LanguageRepresentationFunctionType.BorrowHandle(
			    NativeMethods.BNGetLanguageRepresentationType(this.handle)
		    );
	    }

	    /// <summary>
	    /// Gets a registered language representation function type by its name.
	    /// </summary>
	    /// <param name="name">The name of the language type (e.g., "Pseudo C").</param>
	    /// <returns>The matching LanguageRepresentationFunctionType, or null if not found.</returns>
	    public static LanguageRepresentationFunctionType? GetLanguageTypeByName(string name)
	    {
		    return LanguageRepresentationFunctionType.BorrowHandle(
			    NativeMethods.BNGetLanguageRepresentationFunctionTypeByName(name)
		    );
	    }

	    /// <summary>
	    /// Gets the line formatter settings for this language representation function
	    /// using the given disassembly settings. Returns a LineFormatterSettings populated
	    /// with language-specific formatting defaults.
	    /// </summary>
	    /// <param name="settings">The disassembly settings providing base formatting context.</param>
	    /// <returns>A LineFormatterSettings populated with language-appropriate values.</returns>
	    public LineFormatterSettings GetLineFormatterSettings(DisassemblySettings settings)
	    {
		    // 1. Call the native API.
		    IntPtr ptr = NativeMethods.BNGetLanguageRepresentationLineFormatterSettings(
			    settings.DangerousGetHandle() ,
			    this.handle
		    );

		    // 2. Handle null return.
		    if (IntPtr.Zero == ptr)
		    {
			    return new LineFormatterSettings();
		    }

		    // 3. Read the native struct from the pointer.
		    BNLineFormatterSettings native = Marshal.PtrToStructure<BNLineFormatterSettings>(ptr);

		    // 4. Convert to managed type.
		    LineFormatterSettings result = new LineFormatterSettings();
		    result.DesiredLineLength = native.desiredLineLength;
		    result.MinimumContentLength = native.minimumContentLength;
		    result.TabWidth = native.tabWidth;
		    result.MaximumAnnotationLength = native.maximumAnnotationLength;
		    result.StringWrappingWidth = native.stringWrappingWidth;
		    result.LanguageName = UnsafeUtils.ReadAnsiString(native.languageName);
		    result.CommentStartString = UnsafeUtils.ReadAnsiString(native.commentStartString);
		    result.CommentEndString = UnsafeUtils.ReadAnsiString(native.commentEndString);
		    result.AnnotationStartString = UnsafeUtils.ReadAnsiString(native.annotationStartString);
		    result.AnnotationEndString = UnsafeUtils.ReadAnsiString(native.annotationEndString);

		    // 5. The HLIL function is a borrowed pointer inside the struct; wrap if non-null.
		    result.HighLevelIL = (native.highLevelIL != IntPtr.Zero)
			    ? HighLevelILFunction.NewFromHandle(native.highLevelIL)
			    : null;

		    // 6. Free the native struct allocation.
		    NativeMethods.BNFreeLineFormatterSettings(ptr);

		    return result;
	    }

	}
}