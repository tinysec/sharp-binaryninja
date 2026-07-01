using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum LowLevelILFlagCondition : byte
	{
		/// <summary>
		/// 
		/// </summary>
		LLFC_E = 0,
		
		/// <summary>
		/// 
		/// </summary>
		LLFC_NE = 1,
		
		/// <summary>
		/// 
		/// </summary>
		LLFC_SLT = 2,
		
		/// <summary>
		/// 
		/// </summary>
		LLFC_ULT = 3,
		
		/// <summary>
		/// 
		/// </summary>
		LLFC_SLE = 4,
		
		/// <summary>
		/// 
		/// </summary>
		LLFC_ULE = 5,
		
		/// <summary>
		/// 
		/// </summary>
		LLFC_SGE = 6,
		
		/// <summary>
		/// 
		/// </summary>
		LLFC_UGE = 7,
		
		/// <summary>
		/// 
		/// </summary>
		LLFC_SGT = 8,
		
		/// <summary>
		/// 
		/// </summary>
		LLFC_UGT = 9,
		
		/// <summary>
		/// 
		/// </summary>
		LLFC_NEG = 10,
		
		/// <summary>
		/// 
		/// </summary>
		LLFC_POS = 11,
		
		/// <summary>
		/// 
		/// </summary>
		LLFC_O = 12,
		
		/// <summary>
		/// 
		/// </summary>
		LLFC_NO = 13,
		
		/// <summary>
		/// 
		/// </summary>
		LLFC_FE = 14,
		
		/// <summary>
		/// 
		/// </summary>
		LLFC_FNE = 15,
		
		/// <summary>
		/// 
		/// </summary>
		LLFC_FLT = 16,
		
		/// <summary>
		/// 
		/// </summary>
		LLFC_FLE = 17,
		
		/// <summary>
		/// 
		/// </summary>
		LLFC_FGE = 18,
		
		/// <summary>
		/// 
		/// </summary>
		LLFC_FGT = 19,
		
		/// <summary>
		/// 
		/// </summary>
		LLFC_FO = 20,
		
		/// <summary>
		/// 
		/// </summary>
		LLFC_FUO = 21
	}
}