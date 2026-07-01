using System;

namespace BinaryNinja
{
	/// <summary>
	///
	/// </summary>
    public enum ValueLocationSource : byte
	{
		/// <summary>
		///
		/// </summary>
		DefaultLocationSource = 0,

		/// <summary>
		///
		/// </summary>
		PassByValueLocationSource = 1,

		/// <summary>
		///
		/// </summary>
		PassByReferenceLocationSource = 2,

		/// <summary>
		///
		/// </summary>
		CustomLocationSource = 3
	}
}
