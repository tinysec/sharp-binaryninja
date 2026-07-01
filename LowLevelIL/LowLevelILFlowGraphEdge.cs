namespace BinaryNinja
{
	public sealed class LowLevelILFlowGraphEdge : AbstractFlowGraphEdge<LowLevelILFlowGraphNode>
	{
		public LowLevelILFlowGraphEdge(
			LowLevelILFunction ilFunction,
			BNFlowGraphEdge native)
			: base(native , LowLevelILFlowGraphNode.NewFromHandleEx(ilFunction, native.target))
		{
			
		}
		
		internal static LowLevelILFlowGraphEdge FromNativeEx(
			LowLevelILFunction ilFunction,
			BNFlowGraphEdge native
		)
		{
			return new LowLevelILFlowGraphEdge(
				ilFunction ,
				native
			);
		}
	}
}
