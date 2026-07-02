namespace BinaryNinja
{
	// Concrete edge for a plain (non-IL) flow graph. The IL levels each have their own
	// edge type (e.g. LowLevelILFlowGraphEdge); this is the base-level counterpart, whose
	// Target is a plain FlowGraphNode.
	public sealed class FlowGraphEdge : AbstractFlowGraphEdge<FlowGraphNode>
	{
		public FlowGraphEdge(BNFlowGraphEdge native)
			: base(native , FlowGraphNode.NewFromHandle(native.target))
		{

		}

		internal static FlowGraphEdge FromNative(BNFlowGraphEdge native)
		{
			return new FlowGraphEdge(native);
		}
	}
}
