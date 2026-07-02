using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public abstract class AbstractFlowGraph<T_SELF> : AbstractSafeHandle<T_SELF>
		where T_SELF: AbstractFlowGraph<T_SELF>
	{
		internal AbstractFlowGraph(IntPtr handle , bool owner) 
			: base(handle , owner)
	    {
	       
	    }
		
	    protected override bool ReleaseHandle()
	    {
	        if ( !this.IsInvalid )
	        {
	            NativeMethods.BNFreeFlowGraph(this.handle);
	            this.SetHandleAsInvalid();
	        }
	        
	        return true;
	    }
	    
	   
	    public Function? Function
	    {
		    get
		    {
			    return Function.TakeHandle(NativeMethods.BNGetFunctionForFlowGraph(this.handle));
		    }

		    set
		    {
			    NativeMethods.BNSetFunctionForFlowGraph(
				    this.handle,
				    null == value ? IntPtr.Zero : value.DangerousGetHandle()
				);
		    }
	    }
	    
	    public BinaryView? BinaryView
	    {
		    get
		    {
			    return BinaryView.TakeHandle(NativeMethods.BNGetViewForFlowGraph(this.handle));
		    }

		    set
		    {
			    NativeMethods.BNSetViewForFlowGraph(
				    this.handle,
				    null == value ? IntPtr.Zero : value.DangerousGetHandle()
			    );
		    }
	    }

	    public bool LayoutComplete
	    {
		    get
		    {
			    return NativeMethods.BNIsFlowGraphLayoutComplete(this.handle);
		    }
	    }

	    
	    public ulong NodeCount
	    {
		    get
		    {
			    return NativeMethods.BNGetFlowGraphNodeCount(this.handle);
		    }
	    }

	    public bool HasNodes
	    {
		    get
		    {
			    return NativeMethods.BNFlowGraphHasNodes(this.handle);
		    }
	    }

	    public int Width
	    {
		    get
		    {
			    return NativeMethods.BNGetFlowGraphWidth(this.handle);
		    }

		    set
		    {
			    NativeMethods.BNFlowGraphSetWidth(this.handle, value);
		    }
	    }
	    
	    public int Height
	    {
		    get
		    {
			    return NativeMethods.BNGetFlowGraphHeight(this.handle);
		    }

		    set
		    {
			    NativeMethods.BNFlowGraphSetHeight(this.handle, value);
		    }
	    }
	    
	    
	    public int HorizontalMargin
	    {
		    get
		    {
			    return NativeMethods.BNGetHorizontalFlowGraphNodeMargin(this.handle);
		    }

		    set
		    {
			    NativeMethods.BNSetFlowGraphNodeMargins(
				    this.handle,
				    value,
				    this.VerticalMargin
				);
		    }
	    }
	    
	    
	    public int VerticalMargin
	    {
		    get
		    {
			    return NativeMethods.BNGetVerticalFlowGraphNodeMargin(this.handle);
		    }

		    set
		    {
			    NativeMethods.BNSetFlowGraphNodeMargins(
				    this.handle, 
				    this.HorizontalMargin,
				    value
				);
		    }
	    }
	    
	    public bool IsIL
	    {
		    get
		    {
			    return NativeMethods.BNIsILFlowGraph(this.handle);
		    }
	    }
	  
	    public bool IsLowLevelIL
	    {
		    get
		    {
			    return NativeMethods.BNIsLowLevelILFlowGraph(this.handle);
		    }
	    }

	    public bool IsMediumLevelIL
	    {
		    get
		    {
			    return NativeMethods.BNIsMediumLevelILFlowGraph(this.handle);
		    }
	    }
	    
	    public bool IsHighLevelIL
	    {
		    get
		    {
			    return NativeMethods.BNIsHighLevelILFlowGraph(this.handle);
		    }
	    }

	    public LowLevelILFunction? LowLevelILFunction
	    {
		    get
		    {
			    return LowLevelILFunction.TakeHandle(
				    NativeMethods.BNGetFlowGraphLowLevelILFunction(this.handle)
			    );
		    }

		    set
		    {
			    NativeMethods.BNSetFlowGraphLowLevelILFunction(
				    this.handle ,
				    null == value ? IntPtr.Zero : value.DangerousGetHandle()
			    );
		    }
	    }
	    
	    public MediumLevelILFunction? MediumLevelILFunction
	    {
		    get
		    {
			    return MediumLevelILFunction.TakeHandle(
				    NativeMethods.BNGetFlowGraphMediumLevelILFunction(this.handle)
			    );
		    }
		    
		    set
		    {
			    NativeMethods.BNSetFlowGraphMediumLevelILFunction(
				    this.handle ,
				    null == value ? IntPtr.Zero : value.DangerousGetHandle()
			    );
		    }
	    }
	    
	    public HighLevelILFunction? HighLevelILFunction
	    {
		    get
		    {
			    return HighLevelILFunction.TakeHandle(
				    NativeMethods.BNGetFlowGraphHighLevelILFunction(this.handle)
			    );
		    }
		    
		    set
		    {
			    NativeMethods.BNSetFlowGraphHighLevelILFunction(
				    this.handle ,
				    null == value ? IntPtr.Zero : value.DangerousGetHandle()
			    );
		    }
	    }

	    public bool IsOptionSet(FlowGraphOption option )
	    {
		    return NativeMethods.BNIsFlowGraphOptionSet(this.handle , option );
	    }

	    public void SetOption(FlowGraphOption option , bool value)
	    {
		    NativeMethods.BNSetFlowGraphOption(this.handle , option , value);
	    }

	    public bool UsesBlockHighlights
	    {
		    get
		    {
			    return this.IsOptionSet(FlowGraphOption.FlowGraphUsesBlockHighlights);
		    }

		    set
		    {
			    this.SetOption(FlowGraphOption.FlowGraphUsesBlockHighlights, value);
		    }
	    }
	    
	    public bool UsesInstructionHighlights
	    {
		    get
		    {
			    return this.IsOptionSet(FlowGraphOption.FlowGraphUsesInstructionHighlights);
		    }

		    set
		    {
			    this.SetOption(FlowGraphOption.FlowGraphUsesInstructionHighlights, value);
		    }
	    }
	    
	    public bool IncludesUserComments
	    {
		    get
		    {
			    return this.IsOptionSet(FlowGraphOption.FlowGraphIncludesUserComments);
		    }

		    set
		    {
			    this.SetOption(FlowGraphOption.FlowGraphIncludesUserComments, value);
		    }
	    }
	    
	    public bool AllowsPatching
	    {
		    get
		    {
			    return this.IsOptionSet(FlowGraphOption.FlowGraphAllowsPatching);
		    }

		    set
		    {
			    this.SetOption(FlowGraphOption.FlowGraphAllowsPatching, value);
		    }
	    }
	    
	    public bool AllowsInlineInstructionEditing
	    {
		    get
		    {
			    return this.IsOptionSet(FlowGraphOption.FlowGraphAllowsInlineInstructionEditing);
		    }

		    set
		    {
			    this.SetOption(FlowGraphOption.FlowGraphAllowsInlineInstructionEditing, value);
		    }
	    }
	    
	    public bool ShowsSecondaryRegisterHighlighting
	    {
		    get
		    {
			    return this.IsOptionSet(FlowGraphOption.FlowGraphShowsSecondaryRegisterHighlighting);
		    }

		    set
		    {
			    this.SetOption(FlowGraphOption.FlowGraphShowsSecondaryRegisterHighlighting, value);
		    }
	    }
	    
	    public bool Addressable
	    {
		    get
		    {
			    return this.IsOptionSet(FlowGraphOption.FlowGraphIsAddressable);
		    }

		    set
		    {
			    this.SetOption(FlowGraphOption.FlowGraphIsAddressable, value);
		    }
	    }
	    
	    public bool WorkflowGraph
	    {
		    get
		    {
			    return this.IsOptionSet(FlowGraphOption.FlowGraphIsWorkflowGraph);
		    }

		    set
		    {
			    this.SetOption(FlowGraphOption.FlowGraphIsWorkflowGraph, value);
		    }
	    }

	    

	    public ulong AddNode(FlowGraphNode node)
	    {
		    return NativeMethods.BNAddFlowGraphNode(this.handle, node.DangerousGetHandle());
	    }
	    
	    public void ReplaceNode(ulong index , FlowGraphNode node)
	    {
		    NativeMethods.BNReplaceFlowGraphNode(this.handle, index ,node.DangerousGetHandle());
	    }
	    
	    public void ClearNodes()
	    {
		    NativeMethods.BNClearFlowGraphNodes(this.handle);
	    }

	    public void ShowReport(string title , BinaryView? view = null)
	    {
		    NativeMethods.BNShowGraphReport(
			    null == view ? IntPtr.Zero  : view.DangerousGetHandle(),  
			    title , 
			    this.handle
			);
	    }
	    
	    public RenderLayer[] RenderLayers
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetFlowGraphRenderLayers(
				    this.handle ,
				    out ulong arrayLength
			    );

			    return UnsafeUtils.TakeHandleArray<RenderLayer>(
				    arrayPointer ,
				    arrayLength ,
				    RenderLayer.FromHandle ,
				    NativeMethods.BNFreeRenderLayerList
			    );
		    }

		    set
		    {
			    foreach (RenderLayer layer in this.RenderLayers)
			    {
				    this.RemoveRenderLayer(layer);
			    }
			    
			    foreach (RenderLayer layer in value)
			    {
				    this.AddRenderLayer(layer);
			    }
		    }
	    }

	    public void AddRenderLayer(RenderLayer layer)
	    {
		    NativeMethods.BNAddFlowGraphRenderLayer(this.handle , layer.DangerousGetHandle());
	    }
	    
	    public void RemoveRenderLayer(RenderLayer layer)
	    {
		    NativeMethods.BNRemoveFlowGraphRenderLayer(this.handle , layer.DangerousGetHandle());
	    }
	    
	    
	    public FlowGraphLayoutRequest StartLayout(Action? callback)
	    {
		    IntPtr argCallback = IntPtr.Zero;

		    if (null != callback)
		    {
			    Action<IntPtr> callbackAdapter = ( _ctx => { callback(); } );
			    
			    argCallback = Marshal.GetFunctionPointerForDelegate<Action<IntPtr>>(
				    callbackAdapter
				);
		    }
		    
		    return new FlowGraphLayoutRequest(
			    NativeMethods.BNStartFlowGraphLayout(
				    this.handle , 
				    IntPtr.Zero ,
				    argCallback
				) ,
			    true
		    );
	    }

	    public bool IsNodeValid(FlowGraphNode node)
	    {
		    return NativeMethods.BNIsNodeValidForFlowGraph(this.handle, node.DangerousGetHandle());
	    }
	}
	
	// x
	public sealed class FlowGraph : AbstractFlowGraph<FlowGraph>
	{
		public FlowGraph()
			:this( NativeMethods.BNCreateFlowGraph() , true)
		{
			
		}

		internal FlowGraph(IntPtr handle , bool owner) 
			: base(handle , owner)
	    {
	       
	    }
		
	    
	    internal static FlowGraph? TakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new FlowGraph(handle, true);
	    }
	    
	    internal static FlowGraph MustTakeHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }
		    
		    return new FlowGraph(handle, true);
	    }
	    
	    internal static FlowGraph? BorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    return null;
		    }
		    
		    return new FlowGraph(handle, false);
	    }
	    
	    internal static FlowGraph MustBorrowHandle(IntPtr handle)
	    {
		    if (handle == IntPtr.Zero)
		    {
			    throw new ArgumentNullException(nameof(handle));
		    }

		    return new FlowGraph(handle, false);
	    }

	    /// <summary>
	    /// Whether the flow graph has pending updates.
	    /// </summary>
	    public bool HasUpdates
	    {
		    get
		    {
			    return NativeMethods.BNFlowGraphHasUpdates(this.handle);
		    }
	    }

	    /// <summary>
	    /// Whether the flow graph is in update query mode.
	    /// </summary>
	    public bool UpdateQueryMode
	    {
		    get
		    {
			    return NativeMethods.BNFlowGraphUpdateQueryMode(this.handle);
		    }
	    }

	    /// <summary>
	    /// Creates a function graph for the given function, view type, and optional disassembly settings.
	    /// </summary>
	    /// <param name="func">The function to create a graph for.</param>
	    /// <param name="viewType">The view type specifying the kind of graph (e.g. normal, IL).</param>
	    /// <param name="settings">Optional disassembly settings, or null for defaults.</param>
	    /// <returns>A new owned FlowGraph, or null on failure.</returns>
	    public static FlowGraph? CreateFunctionGraph(
		    Function func ,
		    FunctionViewType viewType ,
		    DisassemblySettings? settings = null
	    )
	    {
		    // 1. Validate required parameters.
		    if (null == func)
		    {
			    throw new ArgumentNullException(nameof(func));
		    }

		    if (null == viewType)
		    {
			    throw new ArgumentNullException(nameof(viewType));
		    }

		    // 2. Marshal the view type to its native struct and call the core API.
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    return FlowGraph.TakeHandle(
				    NativeMethods.BNCreateFunctionGraph(
					    func.DangerousGetHandle() ,
					    viewType.ToNativeEx(allocator) ,
					    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
				    )
			    );
		    }
	    }

	    /// <summary>
	    /// Creates an immediate function graph (no layout delay) for the given function.
	    /// </summary>
	    /// <param name="func">The function to create an immediate graph for.</param>
	    /// <param name="viewType">The view type specifying the kind of graph.</param>
	    /// <param name="settings">Optional disassembly settings, or null for defaults.</param>
	    /// <returns>A new owned FlowGraph, or null on failure.</returns>
	    public static FlowGraph? CreateImmediateFunctionGraph(
		    Function func ,
		    FunctionViewType viewType ,
		    DisassemblySettings? settings = null
	    )
	    {
		    // 1. Validate required parameters.
		    if (null == func)
		    {
			    throw new ArgumentNullException(nameof(func));
		    }

		    if (null == viewType)
		    {
			    throw new ArgumentNullException(nameof(viewType));
		    }

		    // 2. Marshal the view type to its native struct and call the core API.
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    return FlowGraph.TakeHandle(
				    NativeMethods.BNCreateImmediateFunctionGraph(
					    func.DangerousGetHandle() ,
					    viewType.ToNativeEx(allocator) ,
					    null == settings ? IntPtr.Zero : settings.DangerousGetHandle()
				    )
			    );
		    }
	    }

	    /// <summary>
	    /// Gets the owning flow graph for a given flow graph node.
	    /// </summary>
	    /// <param name="node">The flow graph node to query.</param>
	    /// <returns>The owning FlowGraph, or null if not available.</returns>
	    public static FlowGraph? GetNodeOwner(FlowGraphNode node)
	    {
		    // 1. Validate the required parameter.
		    if (null == node)
		    {
			    throw new ArgumentNullException(nameof(node));
		    }

		    // 2. Query the native API for the node's owner graph.
		    return FlowGraph.TakeHandle(
			    NativeMethods.BNGetFlowGraphNodeOwner(node.DangerousGetHandle())
		    );
	    }

	    /// <summary>
	    /// Gets the workflow graph for a binary view.
	    /// </summary>
	    /// <param name="view">The binary view to get the workflow graph for.</param>
	    /// <param name="name">The workflow name.</param>
	    /// <param name="sequential">Whether to use sequential layout.</param>
	    /// <returns>The workflow FlowGraph, or null if not available.</returns>
	    public static FlowGraph? GetWorkflowGraphForBinaryView(
		    BinaryView view ,
		    string name ,
		    bool sequential = false
	    )
	    {
		    // 1. Validate the required parameter.
		    if (null == view)
		    {
			    throw new ArgumentNullException(nameof(view));
		    }

		    // 2. Query the native API.
		    return FlowGraph.TakeHandle(
			    NativeMethods.BNGetWorkflowGraphForBinaryView(
				    view.DangerousGetHandle() ,
				    name ?? string.Empty ,
				    sequential
			    )
		    );
	    }

	    /// <summary>
	    /// Gets the workflow graph for a function.
	    /// </summary>
	    /// <param name="func">The function to get the workflow graph for.</param>
	    /// <param name="name">The workflow name.</param>
	    /// <param name="sequential">Whether to use sequential layout.</param>
	    /// <returns>The workflow FlowGraph, or null if not available.</returns>
	    public static FlowGraph? GetWorkflowGraphForFunction(
		    Function func ,
		    string name ,
		    bool sequential = false
	    )
	    {
		    // 1. Validate the required parameter.
		    if (null == func)
		    {
			    throw new ArgumentNullException(nameof(func));
		    }

		    // 2. Query the native API.
		    return FlowGraph.TakeHandle(
			    NativeMethods.BNGetWorkflowGraphForFunction(
				    func.DangerousGetHandle() ,
				    name ?? string.Empty ,
				    sequential
			    )
		    );
	    }

	    /// <summary>
	    /// Gets the unresolved stack adjustment graph for a function.
	    /// </summary>
	    /// <param name="func">The function to get the graph for.</param>
	    /// <returns>The unresolved stack adjustment FlowGraph, or null if not available.</returns>
	    public static FlowGraph? GetUnresolvedStackAdjustmentGraph(Function func)
	    {
		    // 1. Validate the required parameter.
		    if (null == func)
		    {
			    throw new ArgumentNullException(nameof(func));
		    }

		    // 2. Query the native API.
		    return FlowGraph.TakeHandle(
			    NativeMethods.BNGetUnresolvedStackAdjustmentGraph(func.DangerousGetHandle())
		    );
	    }

	    /// <summary>
	    /// Signals that the graph preparation for layout is complete.
	    /// This must be called after all nodes and edges have been added
	    /// and the graph is ready for the layout engine to process.
	    /// </summary>
	    public void FinishPrepareForLayout()
	    {
		    // Delegate to the native API.
		    NativeMethods.BNFinishPrepareForLayout(this.handle);
	    }

	    /// <summary>
	    /// All nodes in the graph. Mirrors Python FlowGraph.nodes / iteration.
	    /// </summary>
	    public FlowGraphNode[] Nodes
	    {
		    get
		    {
			    IntPtr arrayPointer = NativeMethods.BNGetFlowGraphNodes(
				    this.handle ,
				    out ulong arrayLength
			    );

			    return UnsafeUtils.TakeHandleArrayEx<FlowGraphNode>(
				    arrayPointer ,
				    arrayLength ,
				    FlowGraphNode.MustNewFromHandle ,
				    NativeMethods.BNFreeFlowGraphNodeList
			    );
		    }
	    }

	    /// <summary>
	    /// The node at the given index, or null when out of range. Mirrors Python
	    /// FlowGraph.__getitem__.
	    /// </summary>
	    public FlowGraphNode? GetNode(ulong index)
	    {
		    return FlowGraphNode.TakeHandle(
			    NativeMethods.BNGetFlowGraphNode(this.handle, index)
		    );
	    }

	    /// <summary>
	    /// The nodes intersecting the given rectangular region. Mirrors Python
	    /// FlowGraph.get_nodes_in_region.
	    /// </summary>
	    public FlowGraphNode[] GetNodesInRegion(
		    int left ,
		    int top ,
		    int right ,
		    int bottom
	    )
	    {
		    IntPtr arrayPointer = NativeMethods.BNGetFlowGraphNodesInRegion(
			    this.handle ,
			    left ,
			    top ,
			    right ,
			    bottom ,
			    out ulong arrayLength
		    );

		    return UnsafeUtils.TakeHandleArrayEx<FlowGraphNode>(
			    arrayPointer ,
			    arrayLength ,
			    FlowGraphNode.MustNewFromHandle ,
			    NativeMethods.BNFreeFlowGraphNodeList
		    );
	    }

	    /// <summary>
	    /// Produces an updated copy of the graph when it has pending updates, or null.
	    /// Mirrors Python CoreFlowGraph.update().
	    /// </summary>
	    public FlowGraph? Update()
	    {
		    return FlowGraph.TakeHandle(
			    NativeMethods.BNUpdateFlowGraph(this.handle)
		    );
	    }
	}
}