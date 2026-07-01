using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum FormInputFieldType : byte
	{
		/// <summary>
		/// 
		/// </summary>
		LabelFormField = 0,
		
		/// <summary>
		/// 
		/// </summary>
		SeparatorFormField = 1,
		
		/// <summary>
		/// 
		/// </summary>
		TextLineFormField = 2,
		
		/// <summary>
		/// 
		/// </summary>
		MultilineTextFormField = 3,
		
		/// <summary>
		/// 
		/// </summary>
		IntegerFormField = 4,
		
		/// <summary>
		/// 
		/// </summary>
		AddressFormField = 5,
		
		/// <summary>
		/// 
		/// </summary>
		ChoiceFormField = 6,
		
		/// <summary>
		/// 
		/// </summary>
		OpenFileNameFormField = 7,
		
		/// <summary>
		/// 
		/// </summary>
		SaveFileNameFormField = 8,
		
		/// <summary>
		/// 
		/// </summary>
		DirectoryNameFormField = 9,
		
		/// <summary>
		/// 
		/// </summary>
		CheckboxFormField = 10
	}
}