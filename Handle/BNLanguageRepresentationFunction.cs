using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryNinja
{
	public abstract partial class LanguageRepresentationFunction : AbstractSafeHandle<LanguageRepresentationFunction>
	{
		private bool custom;

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
		    
		    return new CoreLanguageRepresentationFunction(
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
		    
		    return new CoreLanguageRepresentationFunction(
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
		    
		    return new CoreLanguageRepresentationFunction(handle, true);
	    }
	    
	    internal static LanguageRepresentationFunction MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new CoreLanguageRepresentationFunction(handle, true);
	    }
	    
	    internal static LanguageRepresentationFunction? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new CoreLanguageRepresentationFunction(handle, false);
	    }
	    
	    internal static LanguageRepresentationFunction MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new CoreLanguageRepresentationFunction(handle, false);
	    }
	    
	    protected override bool ReleaseHandle()
	    {
		    if (this.custom)
		    {
			    if (this.initialReferencePending)
			    {
				    this.initialReferencePending = false;
				    NativeMethods.BNFreeLanguageRepresentationFunction(this.handle);
			    }

			    return true;
		    }

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
	    public virtual string AnnotationStartString
	    {
		    get
		    {
			    if (this.custom)
			    {
				    return "{";
			    }

			    return UnsafeUtils.TakeUtf8String(
				    NativeMethods.BNGetLanguageRepresentationFunctionAnnotationStartString(this.handle)
			    );
		    }
	    }

	    /// <summary>
	    /// Gets the string that marks the end of an annotation in this language.
	    /// </summary>
	    public virtual string AnnotationEndString
	    {
		    get
		    {
			    if (this.custom)
			    {
				    return "}";
			    }

			    return UnsafeUtils.TakeUtf8String(
				    NativeMethods.BNGetLanguageRepresentationFunctionAnnotationEndString(this.handle)
			    );
		    }
	    }

	    /// <summary>
	    /// Gets the string that marks the start of a comment in this language.
	    /// </summary>
	    public virtual string CommentStartString
	    {
		    get
		    {
			    if (this.custom)
			    {
				    return "// ";
			    }

			    return UnsafeUtils.TakeUtf8String(
				    NativeMethods.BNGetLanguageRepresentationFunctionCommentStartString(this.handle)
			    );
		    }
	    }

	    /// <summary>
	    /// Gets the string that marks the end of a comment in this language.
	    /// </summary>
	    public virtual string CommentEndString
	    {
		    get
		    {
			    if (this.custom)
			    {
				    return string.Empty;
			    }

			    return UnsafeUtils.TakeUtf8String(
				    NativeMethods.BNGetLanguageRepresentationFunctionCommentEndString(this.handle)
			    );
		    }
	    }

	}
}
