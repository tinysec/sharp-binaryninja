using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum FlagRole : byte
	{
		/// <summary>
		/// 
		/// </summary>
		SpecialFlagRole = 0,
		
		/// <summary>
		/// 
		/// </summary>
		ZeroFlagRole = 1,
		
		/// <summary>
		/// 
		/// </summary>
		PositiveSignFlagRole = 2,
		
		/// <summary>
		/// 
		/// </summary>
		NegativeSignFlagRole = 3,
		
		/// <summary>
		/// 
		/// </summary>
		CarryFlagRole = 4,
		
		/// <summary>
		/// 
		/// </summary>
		OverflowFlagRole = 5,
		
		/// <summary>
		/// 
		/// </summary>
		HalfCarryFlagRole = 6,
		
		/// <summary>
		/// 
		/// </summary>
		EvenParityFlagRole = 7,
		
		/// <summary>
		/// 
		/// </summary>
		OddParityFlagRole = 8,
		
		/// <summary>
		/// 
		/// </summary>
		OrderedFlagRole = 9,
		
		/// <summary>
		/// 
		/// </summary>
		UnorderedFlagRole = 10,
		
		/// <summary>
		/// 
		/// </summary>
		CarryFlagWithInvertedSubtractRole = 11
	}
}