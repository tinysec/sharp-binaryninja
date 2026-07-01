using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum FlowGraphOption : byte
	{
		/// <summary>
		/// 
		/// </summary>
		FlowGraphUsesBlockHighlights = 0,
		
		/// <summary>
		/// 
		/// </summary>
		FlowGraphUsesInstructionHighlights = 1,
		
		/// <summary>
		/// 
		/// </summary>
		FlowGraphIncludesUserComments = 2,
		
		/// <summary>
		/// 
		/// </summary>
		FlowGraphAllowsPatching = 3,
		
		/// <summary>
		/// 
		/// </summary>
		FlowGraphAllowsInlineInstructionEditing = 4,
		
		/// <summary>
		/// 
		/// </summary>
		FlowGraphShowsSecondaryRegisterHighlighting = 5,
		
		/// <summary>
		/// 
		/// </summary>
		FlowGraphIsAddressable = 6,
		
		/// <summary>
		/// 
		/// </summary>
		FlowGraphIsWorkflowGraph = 7
	}
}