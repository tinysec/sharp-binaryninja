using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class ReportCollection : AbstractSafeHandle<ReportCollection>
	{
		public ReportCollection() 
			: this( NativeMethods.BNCreateReportCollection() , true)
		{
			
		}
		
		internal ReportCollection(IntPtr handle , bool owner) : base(handle , owner)
	    {
	       
	    }
	    
	    internal static ReportCollection? NewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new ReportCollection(
			    NativeMethods.BNNewReportCollectionReference(handle) ,
			    true
		    );
	    }
	    
	    internal static ReportCollection MustNewFromHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new ReportCollection(
			    NativeMethods.BNNewReportCollectionReference(handle) ,
			    true
		    );
	    }
	    
	    internal static ReportCollection? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new ReportCollection(handle, true);
	    }
	    
	    internal static ReportCollection MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new ReportCollection(handle, true);
	    }
	    
	    internal static ReportCollection? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new ReportCollection(handle, false);
	    }
	    
	    internal static ReportCollection MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new ReportCollection(handle, false);
	    }

	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeReportCollection(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }

	    public ulong Count
	    {
		    get
		    {
			    return NativeMethods.BNGetReportCollectionCount(this.handle);
		    }
	    }

	    public IEnumerable<IReport> Reports
	    {
		    get
		    {
			    for (ulong index = 0; index < this.Count; index++)
			    {
				    ReportType type = NativeMethods.BNGetReportType(this.handle, index);

				    if (ReportType.PlainTextReportType == type)
				    {
					    yield return new PlainTextReport(this,  index);
				    }
				    else if (ReportType.MarkdownReportType == type)
				    {
					    yield return new MarkdownReport(this,  index);
				    }
				    else if (ReportType.HTMLReportType == type)
				    {
					    yield return new HTMLReport(this,  index);
				    }
				    else if (ReportType.FlowGraphReportType == type)
				    {
					    yield return new FlowGraphReport(this,  index);
				    }
				    else
				    {
					    throw new NotSupportedException();
				    }
			    }
		    }
	    }

	    public void AddPlainTextReport(
		    string title ,
		    string contents ,
		    BinaryView? view
		)
	    {
		    NativeMethods.BNAddPlainTextReportToCollection(
			    this.handle, 
			    null == view ? IntPtr.Zero : view.DangerousGetHandle(),
			    title ,
			    contents 
			);
	    }
	    
	    public void AddMarkdownReport(
		    string title ,
		    string contents ,
		    string plaintext,
		    BinaryView? view
	    )
	    {
		    NativeMethods.BNAddMarkdownReportToCollection(
			    this.handle, 
			    null == view ? IntPtr.Zero : view.DangerousGetHandle(),
			    title ,
			    contents ,
			    plaintext
		    );
	    }
	    
	    public void AddHTMLReport(
		    string title ,
		    string contents ,
		    string plaintext,
		    BinaryView? view
	    )
	    {
		    NativeMethods.BNAddHTMLReportToCollection(
			    this.handle, 
			    null == view ? IntPtr.Zero : view.DangerousGetHandle(),
			    title ,
			    contents ,
			    plaintext
		    );
	    }
	    
	    public void AddGraphReport(
		    string title ,
		    FlowGraph graph,
		    BinaryView? view
	    )
	    {
		    NativeMethods.BNAddGraphReportToCollection(
			    this.handle, 
			    null == view ? IntPtr.Zero : view.DangerousGetHandle(),
			    title ,
			    graph.DangerousGetHandle() 
		    );
	    }

	    public void Show(string title)
	    {
		    NativeMethods.BNShowReportCollection(
			    title,
			    this.handle
			);
	    }

	    /// <summary>
	    /// Updates the flow graph for the report at the specified index.
	    /// </summary>
	    /// <param name="index">The zero-based index of the report in the collection.</param>
	    /// <param name="graph">The new flow graph to associate with the report.</param>
	    public void UpdateFlowGraph(ulong index , FlowGraph graph)
	    {
		    // Forward to the native API with the collection handle, index, and graph handle.
		    NativeMethods.BNUpdateReportFlowGraph(
			    this.handle ,
			    index ,
			    graph.DangerousGetHandle()
		    );
	    }
	}
}