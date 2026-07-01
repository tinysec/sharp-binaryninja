using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum BranchType : byte
	{
		/// <summary>
		/// 
		/// </summary>
		UnconditionalBranch = 0,
		
		/// <summary>
		/// 
		/// </summary>
		FalseBranch = 1,
		
		/// <summary>
		/// 
		/// </summary>
		TrueBranch = 2,
		
		/// <summary>
		/// 
		/// </summary>
		CallDestination = 3,
		
		/// <summary>
		/// 
		/// </summary>
		FunctionReturn = 4,
		
		/// <summary>
		/// 
		/// </summary>
		SystemCall = 5,
		
		/// <summary>
		/// 
		/// </summary>
		IndirectBranch = 6,
		
		/// <summary>
		/// 
		/// </summary>
		ExceptionBranch = 7,
		
		/// <summary>
		/// 
		/// </summary>
		UnresolvedBranch = 127,
		
		/// <summary>
		/// 
		/// </summary>
		UserDefinedBranch = 128
	}
}