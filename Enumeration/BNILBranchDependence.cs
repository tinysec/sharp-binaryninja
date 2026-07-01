using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum ILBranchDependence : byte
	{
		/// <summary>
		/// 
		/// </summary>
		NotBranchDependent = 0,
		
		/// <summary>
		/// 
		/// </summary>
		TrueBranchDependent = 1,
		
		/// <summary>
		/// 
		/// </summary>
		FalseBranchDependent = 2
	}
}