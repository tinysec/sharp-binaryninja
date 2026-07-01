namespace BinaryNinja
{
	public sealed class MediumLevelILFlowGraphEdge : AbstractFlowGraphEdge<MediumLevelILFlowGraphNode>
	{
		internal MediumLevelILFunction ILFunction { get; set; }
		
		public MediumLevelILFlowGraphEdge(
			MediumLevelILFunction ilFunction,
			BNFlowGraphEdge native)
			: base(native , MediumLevelILFlowGraphNode.NewFromHandleEx(ilFunction, native.target))
		{
			this.ILFunction = ilFunction;
		}
		
		internal static MediumLevelILFlowGraphEdge FromNativeEx(
			MediumLevelILFunction ilFunction,
			BNFlowGraphEdge native
		)
		{
			return new MediumLevelILFlowGraphEdge(
				ilFunction ,
				native
			);
		}
	}
	
	
}
