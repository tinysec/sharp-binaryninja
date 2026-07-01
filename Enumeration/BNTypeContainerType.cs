using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum TypeContainerType : byte
	{
		/// <summary>
		/// 
		/// </summary>
		AnalysisTypeContainerType = 0,
		
		/// <summary>
		/// 
		/// </summary>
		AnalysisAutoTypeContainerType = 1,
		
		/// <summary>
		/// 
		/// </summary>
		AnalysisUserTypeContainerType = 2,
		
		/// <summary>
		/// 
		/// </summary>
		TypeLibraryTypeContainerType = 3,
		
		/// <summary>
		/// 
		/// </summary>
		TypeArchiveTypeContainerType = 4,
		
		/// <summary>
		/// 
		/// </summary>
		DebugInfoTypeContainerType = 5,
		
		/// <summary>
		/// 
		/// </summary>
		PlatformTypeContainerType = 6,
		
		/// <summary>
		/// 
		/// </summary>
		EmptyTypeContainerType = 7,
		
		/// <summary>
		/// 
		/// </summary>
		OtherTypeContainerType = 8
	}
}