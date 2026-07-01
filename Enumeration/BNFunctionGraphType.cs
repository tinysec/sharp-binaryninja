using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum FunctionGraphType : sbyte
	{
		/// <summary>
		///
		/// </summary>
		InvalidILViewType = -1,
		
		/// <summary>
		/// 
		/// </summary>
		NormalFunctionGraph = 0,
		
		/// <summary>
		/// 
		/// </summary>
		LowLevelILFunctionGraph = 1,
		
		/// <summary>
		/// 
		/// </summary>
		LiftedILFunctionGraph = 2,
		
		/// <summary>
		/// 
		/// </summary>
		LowLevelILSSAFormFunctionGraph = 3,
		
		/// <summary>
		/// 
		/// </summary>
		MediumLevelILFunctionGraph = 4,
		
		/// <summary>
		/// 
		/// </summary>
		MediumLevelILSSAFormFunctionGraph = 5,
		
		/// <summary>
		/// 
		/// </summary>
		MappedMediumLevelILFunctionGraph = 6,
		
		/// <summary>
		/// 
		/// </summary>
		MappedMediumLevelILSSAFormFunctionGraph = 7,
		
		/// <summary>
		/// 
		/// </summary>
		HighLevelILFunctionGraph = 8,
		
		/// <summary>
		/// 
		/// </summary>
		HighLevelILSSAFormFunctionGraph = 9,
		
		/// <summary>
		/// 
		/// </summary>
		HighLevelLanguageRepresentationFunctionGraph = 10
	}
}