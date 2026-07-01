using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum ILInstructionAttribute : ushort
	{
		/// <summary>
		/// 
		/// </summary>
		ILAllowDeadStoreElimination = 1,
		
		/// <summary>
		/// 
		/// </summary>
		ILPreventDeadStoreElimination = 2,
		
		/// <summary>
		/// 
		/// </summary>
		MLILAssumePossibleUse = 4,
		
		/// <summary>
		/// 
		/// </summary>
		MLILUnknownSize = 8,
		
		/// <summary>
		/// 
		/// </summary>
		SrcInstructionUsesPointerAuth = 16,
		
		/// <summary>
		/// 
		/// </summary>
		ILPreventAliasAnalysis = 32,
		
		/// <summary>
		/// 
		/// </summary>
		ILIsCFGProtected = 64,
		
		/// <summary>
		/// 
		/// </summary>
		MLILPossiblyUnusedIntermediate = 128,
		
		/// <summary>
		/// 
		/// </summary>
		HLILFoldableExpr = 256,
		
		/// <summary>
		/// 
		/// </summary>
		HLILInvertableCondition = 512,
		
		/// <summary>
		/// 
		/// </summary>
		HLILEarlyReturnPossible = 1024,
		
		/// <summary>
		/// 
		/// </summary>
		HLILSwitchRecoveryPossible = 2048,
		
		/// <summary>
		/// 
		/// </summary>
		ILTransparentCopy = 4096
	}
}